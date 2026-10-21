using System.Globalization;

namespace Payroll_Management_System.Helpers
{
    public static class ReportMonth
    {
        public static DateOnly Current()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return new DateOnly(today.Year, today.Month, 1);
        }

        // The last completed month (reports calculated for "last month")
        public static DateOnly Previous()
        {
            return Current().AddMonths(-1);
        }

        // Missing, invalid or future month falls back to the current month
        public static DateOnly Parse(string? month)
        {
            var current = Current();

            return DateOnly.TryParseExact(month + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed)
                   && parsed <= current
                ? parsed
                : current;
        }
    }
}
