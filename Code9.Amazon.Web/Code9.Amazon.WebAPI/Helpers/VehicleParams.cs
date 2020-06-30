namespace Code9.Amazon.WebAPI.Helpers
{
    public class VehicleParams
    {
        private const int MaxPageSize = 8;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 8;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public int Make { get; set; } = 0;
        public int Model { get; set; } = 0;
        public string FuelType { get; set; } = "";
        public string City { get; set; } = "";
        public int MaxMileage { get; set; } = 999999;
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = 999999;
        public int MinYear { get; set; } = 1950;
        public int MaxYear { get; set; } = 2020;
        public string OrderBy { get; set; } = "";
    }
}
