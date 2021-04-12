using System.Collections.Generic;
using TestApi.Models.User;

namespace TestApi.Data
{
    public static class UsersData
    {
        public static List<FullUserModel> Data()
        {
            return new List<FullUserModel> {
                new FullUserModel
                {
                    FirstName = "Jan",
                    Surname = "Kowalski",
                    Phone = 501501501,
                    Email = "janKowalski@onet.com",
                    Age = 21,
                    CityId = 1,
                    StreetId= 1,
                    HouseNo = "14b",
                    FlatNo = "1"
                },
                new FullUserModel
                {
                    FirstName = "Adam",
                    Surname = "Śmiały",
                    Phone = 501501502,
                    Email = "jadams@onet.com",
                    Age = 31,
                    CityId = 2,
                    StreetId= 1,
                    HouseNo = "112",
                    FlatNo = "24"
                },
                new FullUserModel
                {
                    FirstName = "Jola",
                    Surname = "Wysoka",
                    Phone = 501502503,
                    Email = "jola@vp.pl",
                    Age = 11,
                    CityId = 2,
                    StreetId= 7,
                    HouseNo = "1a",
                    FlatNo = "89"
                },
                new FullUserModel
                {
                    FirstName = "Julian",
                    Surname = "Wysoki",
                    Phone = 501502504,
                    Email = "julek@zywiec.pl",
                    Age = 89,
                    CityId = 3,
                    StreetId= 9,
                    HouseNo = "56",
                    FlatNo = "110b"
                },
                new FullUserModel
                {
                    FirstName = "Tadeusz",
                    Surname = "Leniwy",
                    Phone = 501502505,
                    Email = "tad@len.pl",
                    Age = 11,
                    CityId = 3,
                    StreetId= 10,
                    HouseNo = "19",
                    FlatNo = "22"
                },
                new FullUserModel
                {
                    FirstName = "Jerzy",
                    Surname = "Nowak",
                    Phone = 501502510,
                    Email = "jerzy.nowak@gmail.com",
                    Age = 51,
                    CityId = 4,
                    StreetId = null,
                    HouseNo = "12",
                    FlatNo = null
                },
                new FullUserModel
                {
                    FirstName = "Joanna",
                    Surname = "Kowalska",
                    Phone = 503502501,
                    Email = "j.kowal@gmail.com",
                    Age = 31,
                    CityId = 5,
                    StreetId = 11,
                    HouseNo = "45",
                    FlatNo = null
                }
            };
        }
    }
}
