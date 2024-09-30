namespace SimbirHealth.Account.Models.Info
{
    public class JwtInfo
    {
        public JwtInfo() { }
        public JwtInfo(string issuerName, string secretKey, double accessLiveHours, double accessLiveMinutes, double refreshLiveDays)
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
        public double AccessLiveHours { get; set; }
        public double AccessLiveMinutes { get; set; }
        public double RefreshLiveDays { get; set; }
    }
}
