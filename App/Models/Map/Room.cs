using KPI_PROJECT.Models.Enums;

namespace KPI_PROJECT.Models.Map;

public class Room
{
    public int Id { get; set; }
    public RoomType Type { get; set; }
    public List<RoomConnection> Exits { get; set; } = new();
}

public class RoomConnection
{
    public int TargetRoomId { get; set; }
    public string Direction { get; set; } = ""; 
}