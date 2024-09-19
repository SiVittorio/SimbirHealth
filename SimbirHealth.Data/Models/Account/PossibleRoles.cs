namespace SimbirHealth.Data.Models.Account
{
    /// <summary>
    /// Возможные роли, которые могут быть у одного аккаунта
    /// </summary>
    public static class PossibleRoles
    {
        public static string Admin { get { return nameof(Admin); } }
        public static string Manager { get { return nameof(Manager); } }
        public static string Doctor { get { return nameof(Doctor); } }
        public static string User { get { return nameof(User); } }
    }
}
