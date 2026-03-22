using System.Collections.Generic;

namespace TestProj.Models.NewFolder
{
    public class AnalyticsViewModel
    {
        public int TotalOrders { get; set; }
        public int TotalTickets { get; set; }

        public List<MuseumStats> TopMuseums { get; set; }
        public List<CityStats> CityStats { get; set; }
        public List<CountryStats> CountryStats { get; set; }
    }
    public class MuseumStats
    {
        public MuseumModel Museum { get; set; }
        public int TicketsSold { get; set; }
        public int OrdersCount { get; set; }
    }

    public class CityStats
    {
        public string City { get; set; }
        public int TicketsSold { get; set; }
        public int OrdersCount { get; set; }
    }

    public class CountryStats
    {
        public string Country { get; set; }
        public int TicketsSold { get; set; }
        public int OrdersCount { get; set; }
    }
}