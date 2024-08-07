namespace DBOperationsLayer.Data.StoredProcedure.Air.Params
{
    //Same name property must be avaialble in SPConst class file. 
    public static class SpGetAirline
    {

        public const string AirportCode = "@AirportCode";
        public const string AirportName = "@AirportName";
        public const string City = "@City";
        public const string Country = "@Country";
        public const string Id = "@Id";
        public const string OutId = "@OutId";


        public static string ToName()
        {
            return nameof(SpGetAirline);
        }
    }
}
