using System.Collections.Generic;
using TestApi.Models;

namespace TestApi.Data
{
    public class StreetsData
    {
        public static List<StreetModel> Data()
        {
            return new List<StreetModel>
            {
                new StreetModel
                {
                    StreetId = 1,
                    Name = "Wroclawska",
                    CityId = 1,
                    PostCode = "31-451"
                },
                new StreetModel
                {
                    StreetId = 2,
                    Name = "Szkolna",
                    CityId = 1,
                    PostCode = "31-452"
                },
                new StreetModel
                {
                    StreetId = 3,
                    Name = "Koscielna",
                    CityId = 1,
                    PostCode = "31-453"
                },
                new StreetModel
                {
                    StreetId = 4,
                    Name = "Krakowska",
                    CityId = 2,
                    PostCode = "50-001"
                },
                new StreetModel
                {
                    StreetId = 5,
                    Name = "Szeroka",
                    CityId = 2,
                    PostCode = "50-002"
                },
                new StreetModel
                {
                    StreetId = 6,
                    Name = "Długa",
                    CityId = 2,
                    PostCode = "50-003"
                },
                new StreetModel
                {
                    StreetId = 7,
                    Name = "Wąska",
                    CityId = 2,
                    PostCode = "50-004"
                },
                new StreetModel
                {
                    StreetId = 8,
                    Name = "Orzechowa",
                    CityId = 3,
                    PostCode = "80-340"
                },
                new StreetModel
                {
                    StreetId = 9,
                    Name = "Akacjowa",
                    CityId = 3,
                    PostCode = "80-342"
                },
                new StreetModel
                {
                    StreetId = 10,
                    Name = "Leśna",
                    CityId = 3,
                    PostCode = "80-344"
                },
                 new StreetModel
                {
                    StreetId = 11,
                    Name = "Słoneczna",
                    CityId = 5,
                    PostCode = "34-350"
                },
            };
        }
    }
}
