using GenericFunction;
using GenericFunction.DefaultSettings;
using StackExchange.Redis;
namespace DataCacheLayer.CacheRepositories
{
    /// <summary>
    /// This class will help to connect with default redis server which is common for all the web hosting instances.
    /// </summary>
    public sealed class ConnectionHelper
    {
        /// <summary>
        /// it will connect to default redis database with static constructor.
        /// </summary>
        static ConnectionHelper()
        {
            ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {

                ApplicationSettings applicationSettings = SettingsConfigHelper.GetOptions<ApplicationSettings>();
                var options = ConfigurationOptions.Parse($"{applicationSettings.CacheServer.Url}:{applicationSettings.CacheServer.Port}");
                options.Password = applicationSettings.CacheServer.Password;
                options.User = applicationSettings.CacheServer.User;
                options.ConnectTimeout = applicationSettings.CacheServer.connectTimeout;
                options.SyncTimeout = applicationSettings.CacheServer.syncTimeout;
                options.AsyncTimeout = applicationSettings.CacheServer.syncTimeout;
                options.AbortOnConnectFail = applicationSettings.CacheServer.abortConnect;
                return ConnectionMultiplexer.Connect(options);
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
