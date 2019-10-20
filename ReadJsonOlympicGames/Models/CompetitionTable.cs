using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadJsonOlympicGames.Models
{
    public class CompetitionTable
    {
        public int id { get; set; }
        public string nameOfCompetition { get; set; }
        public string description { get; set; }
        public string startDate { get; set; }
        public string time { get; set; }
        public string sportName { get; set; }
        public string status { get; set; }
        public List<UserTable> athletes { get; set; }
        public List<UserTable> athletesWithMedals { get; set; }
    }
}