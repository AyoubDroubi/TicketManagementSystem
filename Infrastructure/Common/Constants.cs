namespace Infrastructure.Common
{
    public class Constants
    {
        public const string DEFAULT_PASSWORD = "NeedReset%123";

        public const string STATUS_OPEN = "OPEN";
        public const string STATUS_NEW = "NEW";
        public const string STATUS_CLOSED = "CLOSED";


        public const string ROLE_ADMIN_ID = "48395b9b-a292-4a48-80df-ebc6a057f6c0";
        public const string ROLE_USER_ID = "6367a40d-3014-43bc-a704-135f41abae2d";
        public const string ROLE_ADMIN = "Admin";
        public const string ROLE_USER = "User";

        public static readonly Dictionary<string, string> UserRoles = new Dictionary<string, string>
        {
            { ROLE_ADMIN_ID, ROLE_ADMIN },
            { ROLE_USER_ID, ROLE_USER }
        };


        public const string DEFAULT_AVATAR = "avatar.png";
    }
}
