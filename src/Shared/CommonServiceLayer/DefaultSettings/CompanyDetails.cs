//namespace SharedLibrary.DefaultSettings
//{
//    public class CompanyDetails
//    {
//        public string Name { get; set; } = string.Empty;
//        public string ContactEmail { get; set; } = string.Empty;
//        public string SupportEmaill { get; set; } = string.Empty;
//        public string ContactDetails { get; set; } = string.Empty;
//        public string SuffixDomain { get; set; } = string.Empty;
//        public string Website { get; set; } = string.Empty;
//    }
//    public class DbHostingRecord
//    {
//        public string SuffixName { get; set; } = string.Empty;
//        public string DbConnectionString { get; set; } = string.Empty;
//    }  
    
//    public class ModuleCacheSettings
//    {
//        public double LibraryModule { get; set; } 
//        public double AuthenticationModule { get; set; }
//        public double DbHostRecords { get; set; } 
//        public double Books { get; set; } 
//    }
//    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
//    public class ApplicationSettings
//    {
//        public CacheServer CacheServer { get; set; }
//        public ModuleCacheSettings ModuleCacheSettings { get; set; }
//        public SendExceptionEmailTo SendExceptionEmailTo { get; set; }
      
//    }

//    public class CacheServer
//    {
//        public string Url { get; set; }
//        public string Port { get; set; }
//        public string User { get; set; }
//        public string Password { get; set; }
//        public int connectTimeout { get; set; }
//        public int syncTimeout { get; set; }
//        public int asyncTimeout { get; set; }
//        public bool abortConnect { get; set; }
//        public string HashKey { get; set; }
//    }


//    public class Root
//    {
//        public ApplicationSettings ApplicationSettings { get; set; }
//    }

//    public class SendExceptionEmailTo
//    {
//        public string Email { get; set; }
//    }
//}
