using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReadJsonOlympicGames.Models
{
    public class CompetitionResultsModel
    {
        public int id { get; set; }
        public string result { get; set; }
        public string nameOfCompetition { get; set; }
    }
}