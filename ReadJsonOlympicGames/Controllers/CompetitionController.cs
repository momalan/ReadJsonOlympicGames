using ReadJsonOlympicGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadJsonOlympicGames.Controllers
{
    public class CompetitionController : Controller
    {
        OGData.Repository _repository = new OGData.Repository();
        // GET: Competition

        public ActionResult GetAllCompetitions()
        {
            var competitions = _repository.GetAllCompetitions();

            Competitions com = new Competitions();
            com.competitions = competitions.Select(x => new CompetitionTable
            {
                id = x.id,
                nameOfCompetition = x.nameOfCompetition,
                sportName = x.sportName,
                startDate = x.startDate,
                time = x.time,
                status = x.status,
                athletes = x.TableOfUsers.Select(y => new UserTable { id = y.id, badgeNumber = y.badgeNumber, firstName = y.firstName, lastName = y.lastName, nationality = y.nationality }).ToList()
            }).ToList();


            return View("CompetitionList", com);
        }
        public ActionResult CreateCompetition()
        {
            var listOfSports = _repository.GetAllSports();
            Sports s = new Sports();
            s.sports = listOfSports.Select(x => new OlympicSports { id = x.id, nameOfSport = x.nameOfSport }).ToList();
            return View("CreateCompetition", s);
        }

        public ActionResult ChosenSport(string sport)
        {
            var specAthletes = _repository.GetSpecificAthletes(sport);
            UsersInserted users = new UsersInserted();
            users.users = specAthletes.Select(x => new UserTable { id = x.id, firstName = x.firstName, lastName = x.lastName, badgeNumber = x.badgeNumber, nationality = x.nationality }).ToList();
            ViewBag.sport = sport;
            return View("AddCompetitionDetails", users);
        }

        [HttpPost]
        public ActionResult SaveCompetition(FormCollection collection)
        {
            if (_repository.UniqueCompetitionName((collection["nameOfCompetition"])) && collection["nameOfCompetition"] != "" && collection["description"] != "" && collection["sport"] != "" && collection["startDate"] != "" && collection["time"] != "")
            {
                _repository.SaveCompetition(collection["nameOfCompetition"], collection["description"], collection["sport"], collection["startDate"], collection["time"]);
                var competition = _repository.GetCompetition(collection["nameOfCompetition"]);

                for (int i = 5; i < collection.Count; i++)
                {
                    var badge = collection[i];
                    _repository.AssignCompetitionToAthlete(badge, competition);
                }
                var competitions = _repository.GetAllCompetitions();

                Competitions com = new Competitions();
                com.competitions = competitions.Select(x => new CompetitionTable
                {
                    id = x.id,
                    nameOfCompetition = x.nameOfCompetition,
                    sportName = x.sportName,
                    startDate = x.startDate,
                    time = x.time,
                    status = x.status,
                    athletes = x.TableOfUsers.Select(y => new UserTable { id = y.id, badgeNumber = y.badgeNumber, firstName = y.firstName, lastName = y.lastName, nationality = y.nationality }).ToList()
                }).ToList();
                return View("CompetitionList", com);
            }
            else
            {
                var specAthletes = _repository.GetSpecificAthletes(collection["sport"]);
                UsersInserted users = new UsersInserted();
                users.users = specAthletes.Select(x => new UserTable { id = x.id, firstName = x.firstName, lastName = x.lastName, badgeNumber = x.badgeNumber, nationality = x.nationality }).ToList();
                ViewBag.sport = collection["sport"];
                ViewBag.error = "Choose another competition name or fill the inputs!";
                return View("AddCompetitionDetails", users);
            }
        }

        public ActionResult DeleteCompetition(int id)
        {
            _repository.DeleteCompetition(id);
            var competitions = _repository.GetAllCompetitions();

            Competitions com = new Competitions();
            com.competitions = competitions.Select(x => new CompetitionTable
            {
                id = x.id,
                nameOfCompetition = x.nameOfCompetition,
                sportName = x.sportName,
                startDate = x.startDate,
                time = x.time,
                status = x.status,
                athletes = x.TableOfUsers.Select(y => new UserTable { id = y.id, badgeNumber = y.badgeNumber, firstName = y.firstName, lastName = y.lastName, nationality = y.nationality }).ToList()
            }).ToList();


            return View("CompetitionList", com);
        }

        public ActionResult CompetitionDetails(string comName)
        {
            var competition = _repository.GetCompetition(comName);
            var list = _repository.CompetitionMedals(comName);
            CompetitionTable com = new CompetitionTable();
            com.id = competition.id;
            com.nameOfCompetition = comName;
            com.sportName = competition.sportName;
            com.startDate = competition.startDate;
            com.time = competition.time;
            com.status = competition.status;
            var athletes = _repository.GetAthletesOfCompetition(comName);
            com.athletes = athletes.Select(x => new UserTable
            {
                id = x.id,
                firstName = x.firstName,
                lastName = x.lastName,
                badgeNumber = x.badgeNumber,
                nationality = x.nationality,
                results = x.CompetitionResults.Select(y => new CompetitionResultsModel { id = y.id, nameOfCompetition = y.nameOfCompetition, result = y.result }).ToList()
            }).ToList();

            var orderedList = from s in list
                              orderby s.longestJump descending
                              select s;

            com.athletesWithMedals = orderedList.Select(x => new UserTable
            {
                id = x.id,
                firstName = x.firstName,
                lastName = x.lastName,
                nationality = x.nationality,
                badgeNumber = x.badgeNumber,
                result = x.longestJump
            }).ToList();

            com.athletesWithMedals = com.athletesWithMedals.Take(3).ToList();
            return View("CompetitionInfo", com);
        }

        [HttpPost]
        public ActionResult StartCompetition(string nameOfCompetition)
        {
            _repository.StartCompetition(nameOfCompetition);
            var competition = _repository.GetCompetition(nameOfCompetition);

            CompetitionTable com = new CompetitionTable();
            com.id = competition.id;
            com.nameOfCompetition = nameOfCompetition;
            com.sportName = competition.sportName;
            com.startDate = competition.startDate;
            com.time = competition.time;
            com.status = competition.status;
            com.athletes = competition.TableOfUsers.Select(x => new UserTable { id = x.id, firstName = x.firstName, lastName = x.lastName, badgeNumber = x.badgeNumber, nationality = x.nationality }).ToList();
            return View("CompetitionInfo", com);
        }

        [HttpPost]
        public ActionResult EndCompetition(string nameOfCompetition)
        {
            _repository.EndCompetition(nameOfCompetition);
            var competition = _repository.GetCompetition(nameOfCompetition);

            CompetitionTable com = new CompetitionTable();
            com.id = competition.id;
            com.nameOfCompetition = nameOfCompetition;
            com.sportName = competition.sportName;
            com.startDate = competition.startDate;
            com.time = competition.time;
            com.status = competition.status;
            var athletes = _repository.GetAthletesOfCompetition(nameOfCompetition);
            com.athletes = athletes.Select(x => new UserTable
            {
                id = x.id,
                firstName = x.firstName,
                lastName = x.lastName,
                badgeNumber = x.badgeNumber,
                nationality = x.nationality,
                results = x.CompetitionResults.Select(y => new CompetitionResultsModel { id = y.id, nameOfCompetition = y.nameOfCompetition, result = y.result }).ToList()
            }).ToList();


            var list = _repository.CompetitionMedals(nameOfCompetition);
            var orderedList = from s in list
                              orderby s.longestJump descending
                              select s;

            com.athletesWithMedals = orderedList.Select(x => new UserTable
            {
                id = x.id,
                firstName = x.firstName,
                lastName = x.lastName,
                nationality = x.nationality,
                badgeNumber = x.badgeNumber,
                result = x.longestJump
            }).ToList();

            com.athletesWithMedals = com.athletesWithMedals.Take(3).ToList();
            return View("CompetitionInfo", com);
        }

        public ActionResult AthleteJump(string comName, int userId)
        {
            Random rnd = new Random();
            int length = rnd.Next(44, 120);
            _repository.AddCompetitionResultToAthlete(comName, userId, length);

            var competition = _repository.GetCompetition(comName);

            CompetitionTable com = new CompetitionTable();
            com.id = competition.id;
            com.nameOfCompetition = comName;
            com.sportName = competition.sportName;
            com.startDate = competition.startDate;
            com.time = competition.time;
            com.status = competition.status;

            var athletes = _repository.GetAthletesOfCompetition(comName);
            com.athletes = athletes.Select(x => new UserTable
            {
                id = x.id,
                firstName = x.firstName,
                lastName = x.lastName,
                badgeNumber = x.badgeNumber,
                nationality = x.nationality,
                results = x.CompetitionResults.Select(y => new CompetitionResultsModel { id = y.id, nameOfCompetition = y.nameOfCompetition, result = y.result }).ToList()
            }).ToList();

            return View("CompetitionInfo", com);

        }

    }
}