using HotelManagementAPI.Entities.DTOs;

namespace HotelManagementAPI.Entities.Wrappers;

public class ChartStatistic
{
    public required IEnumerable<ChartData> ChartsData { get; set; } = new List<ChartData>();
    public required double ValueSum { get; set; }
    public required double Percentage { get; set; }
}