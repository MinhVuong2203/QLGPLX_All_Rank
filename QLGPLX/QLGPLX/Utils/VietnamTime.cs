namespace Backend.Utils;

public static class VietnamTime
{
    private static readonly TimeZoneInfo TimeZone = CreateTimeZone();

    public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZone);

    public static DateTime Today => Now.Date;

    public static DateOnly TodayDate => DateOnly.FromDateTime(Now);

    private static TimeZoneInfo CreateTimeZone()
    {
        foreach (var id in new[] { "Asia/Ho_Chi_Minh", "Asia/Bangkok", "SE Asia Standard Time" })
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
            }
            catch (InvalidTimeZoneException)
            {
            }
        }

        return TimeZoneInfo.CreateCustomTimeZone(
            "Vietnam Standard Time",
            TimeSpan.FromHours(7),
            "Vietnam Standard Time",
            "Vietnam Standard Time");
    }
}
