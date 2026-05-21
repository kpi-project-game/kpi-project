using Lib.Core.BaseClasses;
using Lib.Core.Models.Enemies.FirstLevel;
using Lib.Core.Models.Enemies.SecondLocation;
using Lib.Core.Models.Enemies.ThirdLocation;
using Serilog;

namespace Lib.Core.Factories;

public static class EnemyFactory
{
    private static readonly Random _rand = new Random();

    public static List<EnemyBase> GenerateEnemiesForLocation(int locationId)
    {
        int count = locationId switch
        {
            1 => 1,
            2 => _rand.Next(1, 3),
            3 => _rand.Next(1, 4),
            _ => 1
        };

        var enemies = new List<EnemyBase>();

        for (int i = 0; i < count; i++)
        {
            enemies.Add(locationId switch
            {
                1 => GetFirstLocationEnemy(),
                2 => GetSecondLocationEnemy(),
                3 => GetThirdLocationEnemy(),
                _ => GetFirstLocationEnemy()
            });
        }

        Log.Debug("Generated {EnemyCount} enemies for location {LocationId}", count, locationId);
        return enemies;
    }

    private static EnemyBase GetFirstLocationEnemy()
    {
        return _rand.Next(1, 4) switch
        {
            1 => new StarterMagician(),
            2 => new WiseMagician(),
            _ => new MagicianShishian()
        };
    }

    private static EnemyBase GetSecondLocationEnemy()
    {
        return _rand.Next(1, 4) switch
        {
            1 => new Squire(),
            2 => new RoyalKnight(),
            _ => new Warlord()
        };
    }

    public static EnemyBase CreateByClassName(string className)
    {
        return className switch
        {
            "StarterMagician"  => new StarterMagician(),
            "WiseMagician"     => new WiseMagician(),
            "MagicianShishian" => new MagicianShishian(),
            "Squire"           => new Squire(),
            "RoyalKnight"      => new RoyalKnight(),
            "Warlord"          => new Warlord(),
            "Skeleton"         => new Skeleton(),
            "Zombie"           => new Zombie(),
            "HeadlessWarden"   => new HeadlessWarden(),
            _                  => new StarterMagician()
        };
    }

    private static EnemyBase GetThirdLocationEnemy()
    {
        return _rand.Next(1, 4) switch
        {
            1 => new Skeleton(),
            2 => new Zombie(),
            _ => new HeadlessWarden()
        };
    }
}