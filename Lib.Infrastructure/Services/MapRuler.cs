using Lib.Core.BaseClasses;
using Lib.Core.Enums;
using Lib.Infrastructure.Database;
using Lib.Infrastructure.Database.Repositories;
using Serilog;

namespace Lib.Infrastructure.Services;

public class MapRuler
{
    private readonly RoomRepository _roomRepo;
    private readonly CharacterRepository _charRepo;
    private readonly Random _rand = new Random();

    public MapRuler(RoomRepository roomRepo, CharacterRepository charRepo)
    {
        _roomRepo = roomRepo;
        _charRepo = charRepo;
    }

    public void GenerateMap(int charId, long telegramId, int location, int floor)
    {
        Log.Information("Beginning generation of Location {Location}, Floor {Floor} for hero {CharId}", location, floor, charId);
        _roomRepo.ClearMapForCharacter(charId);

        int width = 3 + location + floor;  
        int height = 2 + location + floor; 

        GenerateGridLevel(charId, telegramId, width, height);

        var hero = _charRepo.GetActiveCharacter(telegramId);
        if (hero != null)
        {
            _charRepo.ResetTurnsForFloor(charId, location, floor);
            Log.Debug("Turns of hero {CharId} are reset for Location {Location}, Floor {Floor}", charId, location, floor);
        }
    }

    private void GenerateGridLevel(int charId, long telegramId, int width, int height)
    {
        int[,] grid = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                RoomType type;
                if (x == 0 && y == 0)
                    type = RoomType.Empty;
                else if (x == width - 1 && y == height - 1)
                    type = RoomType.Exit;
                else
                    type = GetRandomRoomType();

                grid[x, y] = _roomRepo.CreateRoom(charId, type);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < width - 1)
                {
                    _roomRepo.CreateConnection(grid[x, y], grid[x + 1, y], "East");
                    _roomRepo.CreateConnection(grid[x + 1, y], grid[x, y], "West");
                }
                if (y < height - 1)
                {
                    _roomRepo.CreateConnection(grid[x, y], grid[x, y + 1], "North");
                    _roomRepo.CreateConnection(grid[x, y + 1], grid[x, y], "South");
                }
            }
        }

        _charRepo.UpdateCharacterRoom(telegramId, grid[0, 0]);
        _roomRepo.MarkRoomExplored(charId, grid[0, 0]);
        _charRepo.UpdateMapSize(charId, width, height);
    }

    private RoomType GetRandomRoomType()
    {
        int roll = _rand.Next(1, 101);
        if (roll <= 50) return RoomType.Enemy;
        if (roll <= 80) return RoomType.Empty;
        return RoomType.Loot;
    }

    public string ProcessRoomEntry(long telegramId, int targetRoomId)
    {
        var hero = _charRepo.GetActiveCharacter(telegramId);
        if (hero == null) return "error";

        hero.TurnsLeft--;

        if (hero.TurnsLeft <= 0)
        {
            Log.Information("Hero {HeroId} died from lack of turns", hero.Id);
            _charRepo.UpdateTurnsLeft(hero.Id, 0);
            _charRepo.KillCharacter(hero.Id);
            return "no_turns";
        }

        Log.Debug("Hero {HeroId} went into room {RoomId}. Turns left: {TurnsLeft}", hero.Id, targetRoomId, hero.TurnsLeft);

        _charRepo.UpdatePreviousRoom(hero.Id, hero.CurrentRoomId);
        _charRepo.UpdateTurnsLeft(hero.Id, hero.TurnsLeft);
        _charRepo.UpdateCharacterRoom(telegramId, targetRoomId);
        _roomRepo.MarkRoomExplored(hero.Id, targetRoomId);

        var room = _roomRepo.GetRoom(targetRoomId);
        if (room == null) return "ok";

        if (room.Type == RoomType.Exit)
            return AdvanceFloor(hero, telegramId);

        return "ok";
    }

    public string AdvanceFloor(Character hero, long telegramId)
    {
        int nextFloor = hero.Floor + 1;
        int nextLocation = hero.Location;

        if (nextFloor > 3)
        {
            nextFloor = 1;
            nextLocation++;
        }

        if (nextLocation > 3)
        {
            Log.Information("Hero {HeroId} have beat the game!", hero.Id);
            return "victory";
        }

        Log.Information("Hero {HeroId} going to the next level: Location {Location}, Floor {Floor}", hero.Id, nextLocation, nextFloor);
        _charRepo.UpdateLocationAndFloor(hero.Id, nextLocation, nextFloor);
        GenerateMap(hero.Id, telegramId, nextLocation, nextFloor);

        return $"next_floor:{nextLocation}:{nextFloor}";
    }
}   