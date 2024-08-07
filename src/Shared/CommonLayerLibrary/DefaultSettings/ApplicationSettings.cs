namespace GenericFunction.DefaultSettings
{
    /// <summary>
    /// Default application settings
    /// </summary>
    public class ApplicationSettings
    {
        public CacheServer CacheServer { get; set; }
        public ModuleCacheSettings ModuleCacheSettings { get; set; }
        public SendExceptionEmailTo SendExceptionEmailTo { get; set; }
        public NotificationHubRoute NotificationHubRoute { get; set; }
      
    }
}
