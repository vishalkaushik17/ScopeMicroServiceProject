namespace GenericFunction.DefaultSettings
{
    /// <summary>
    /// Default redis cache server settings
    /// </summary>
    public class CacheServer
    {
        public string Url { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int connectTimeout { get; set; }
        public int syncTimeout { get; set; }
        public int asyncTimeout { get; set; }
        public bool abortConnect { get; set; }
        public string HashKey { get; set; }
    }
}
