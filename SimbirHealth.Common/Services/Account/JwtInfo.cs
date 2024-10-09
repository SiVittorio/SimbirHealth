namespace SimbirHealth.Common.Services.Account
{
    public class JwtInfo
    {
        public JwtInfo() { }
        public JwtInfo(string issuerName, string secretKey, int accessLiveHours, int accessLiveMinutes, int refreshLiveDays)
        {
            IssuerName = issuerName;
            SecretKey = secretKey;
            AccessLiveHours = accessLiveHours;
            AccessLiveMinutes = accessLiveMinutes;
            RefreshLiveDays = refreshLiveDays;
        }

        public static readonly string SectionName = "JwtAuthenticationInfo";
        public string IssuerName { get; set; }
        public string SecretKey { get; set; }
        public int AccessLiveHours { get; set; }
        public int AccessLiveMinutes { get; set; }
        public int RefreshLiveDays { get; set; }
    }
}
