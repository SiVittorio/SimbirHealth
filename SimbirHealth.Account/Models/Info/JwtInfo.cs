namespace SimbirHealth.Account.Models.Info
{
    public class JwtInfo
    {
        public JwtInfo() { }
        public JwtInfo(string issuerName, string secretKey, double liveHours, double liveMinutes)
        {
            IssuerName = issuerName;
            SecretKey = secretKey;
            LiveHours = liveHours;
            LiveMinutes = liveMinutes;
        }

        public static readonly string SectionName = "JwtAuthenticationInfo";
        public string IssuerName { get; set; }
        public string SecretKey { get; set; }
        public double LiveHours { get; set; }
        public double LiveMinutes { get; set; }
    }
}
