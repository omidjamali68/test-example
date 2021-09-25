namespace Cooking.Entities.ApplicationIdentities
{
    public class UserTimeZone
    {
        public UserTimeZone(string zoneName, string languageCode)
        {
            ZoneName = zoneName;
            LanguageCode = languageCode;
        }

        public string ZoneName { get; set; }
        public string LanguageCode { get; set; }
    }
}