using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadJsonOlympicGames.Models
{
    public class UserTable
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dateOfBirth { get; set; }
        public string nationality { get; set; }
        public string badgeNumber { get; set; }
        public string photo { get; set; }
        public string gender { get; set; }
        public string role { get; set; }
        public string password { get; set; }
        public int result { get; set; }
        public List<OlympicSports> sports { get; set; }
        public List<CompetitionResultsModel> results { get; set; }
    }
}