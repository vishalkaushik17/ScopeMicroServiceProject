namespace GenericFunction.Constants.AppConfig
{
    public static class AppConst
    {
        /// <summary>
        /// List of Sections in appsettings.json
        /// </summary>
        public const string AllowSpecificOriginsPolicy = "AllowSpecificOrigins";
        public const string AllowGetMethodPolicy = "AllowGetMethod";
        public const string AllowGetMethod = "GET";
        public const string AllowPOSTMethod = "POST";

        public const string AllowedHosts = "AllowedHosts";
        public const string AllowOrigins = "AllowOrigins";
        public const string App = "AllowedHosts";
        public const string AppSettingFile = "appsettings.json";
        public const string AppCookieName = ".AspNetCore.Identity.Application";
        public const string CorsPolicy = "CorsPolicy";
        public const string MailSettings = "MailSettings";
        public const string ServerConfig = "ServerConfig";
        public const string EmailService = "EmailService";
        //for routing 
        public const string DefaultRoute = "api/[controller]/[action]";
    }


}
