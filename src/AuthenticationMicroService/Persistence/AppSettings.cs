//namespace ScopeCoreApi.Persistence
//{
//    public sealed class MailConfiguration
//    {
//        private static MailConfiguration _instance = null;
//        private static readonly object padlock = new object();
//        public readonly string _connectionString = string.Empty;

//        private MailConfiguration()
//        {
//            var configuration = new ConfigurationBuilder()
//                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false)
//                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json"), optional: false)
//                .Build();

//            _connectionString = configuration.GetSection("MailConfiguration").GetSection("ConnectionString").Value;
//        }

//        public static MailConfiguration Instance
//        {
//            get
//            {
//                lock (padlock)
//                {
//                    if (_instance == null)
//                    {
//                        _instance = new MailConfiguration();
//                    }
//                    return _instance;
//                }
//            }
//        }

//        public string ConnectionString
//        {
//            get => _connectionString;
//        }
//    }
//}
