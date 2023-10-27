namespace GoogleAuthDemo.Areas.Identity.Pages.Account.Manage
{
    public class GoogleSettings : IGoogleSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scope { get; set; }
        public string ApplicationName { get; set; }
        public string User { get; set; }
        public string CalendarId { get; set; }
    }
}
