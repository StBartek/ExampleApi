using System.Collections.Generic;
using TestApi.Models;

namespace TestApi.Data
{
    public class CitiesData
    {
        public static List<CityModel> Data()
        {
            return new List<CityModel>
            {
                new CityModel
                {
                    CityId = 1,
                    Name = "Krakow",
                    Gps = "0,0 0,0",
                    PostCode = "31-450",
                    ResidentCount = 1000000
                },
                new CityModel
                {
                    CityId = 2,
                    Name = "Wroclaw",
                    Gps = "0,0 0,0",
                    PostCode = "50-001",
                    ResidentCount = 700000
                },
                new CityModel
                {
                    CityId = 3,
                    Name = "Sopot",
                    Gps = "0,0 0,0",
                    PostCode = "80-340",
                    ResidentCount = 300000
                },
                new CityModel
                {
                    CityId = 4,
                    Name = "Pasierbiec",
                    Gps = "0,0 0,0",
                    PostCode = "31-601",
                    ResidentCount = 500
                },
                new CityModel
                {
                    CityId = 5,
                    Name = "Milówka",
                    Gps = "0,0 0,0",
                    PostCode = "34-600",
                    ResidentCount = 5000
                }
            };
        }
    }
}
