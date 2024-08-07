namespace DBOperationsLayer.Data.StoredProcedure.Air.Params
{
    //Same name property must be avaialble in SPConst class file. 
    public static class SpSaveAirline
    {
        public const string AirportCode = "AirportCode";
        public const string AirportName = "AirportName";
        public const string City = "City";
        public const string Country = "Country";
        public static string ToName()
        {
            return nameof(SpSaveAirline);
        }


    }
}
