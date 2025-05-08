namespace TicketManagementUI.Extensions
{
    public static class DatetimeExtensions
    {
        public static string BeautifyDate(this DateTime dateTime) {
            return dateTime.ToString("dd MMMM yyyy hh:mm tt");
        }

    }
}
