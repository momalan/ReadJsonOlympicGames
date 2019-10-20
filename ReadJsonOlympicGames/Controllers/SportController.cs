using ReadJsonOlympicGames.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadJsonOlympicGames.Controllers
{
    public class SportController : Controller
    {
        // GET: Sport
        OGData.Repository _repository = new OGData.Repository();
        public ActionResult GetOlympicSports()
        {
            var listOfSports = _repository.GetAllSports();
            Sports sp = new Sports();
            sp.sports = listOfSports.Select(x => new OlympicSports
            {
                id = x.id,
                nameOfSport = x.nameOfSport,
                description = x.description
            }).ToList();

            return View("Sports", sp);
        }

        public ActionResult GetAthletes()
        {
            var listOfAthletes = _repository.GetAthletes();
            UsersInserted ui = new UsersInserted();
            ui.users = listOfAthletes.Select(x => new UserTable
            {
                id = x.id,
                firstName = x.firstName,
                lastName = x.lastName,
                nationality = x.nationality,
                badgeNumber = x.badgeNumber,
                sports = x.OlympicSports.Select(y => new OlympicSports { id = y.id, nameOfSport = y.nameOfSport }).ToList()
            }).ToList();

            return View("Athletes", ui);
        }
        public ActionResult AddSport()
        {
            return View();
        }

        public ActionResult DeleteAthlete(int id)
        {
            _repository.DeleteAthlete(id);
            return RedirectToAction("GetAthletes");
        }

        [HttpPost]
        public ActionResult AddSportToDb(string nameOfSport, string description)
        {
            if (_repository.UniqueSportName(nameOfSport))
            {
                _repository.InsertSport(nameOfSport, description);
                return RedirectToAction("GetOlympicSports");
            }
            else
            {
                ViewBag.error = "Sport name already exist!";
                return View("AddSport");
            }

        }

        public ActionResult AssignSports(int id)
        {
            var user = _repository.GetSpecificUser(id);
            UserTable ut = new UserTable();
            ut.id = user.id;
            ut.firstName = user.firstName;
            ut.lastName = user.lastName;
            ut.badgeNumber = user.badgeNumber;

            var lisOfSports = _repository.GetAllSports();

            ut.sports = lisOfSports.Select(x => new OlympicSports
            {

                id = x.id,
                nameOfSport = x.nameOfSport

            }).ToList();

            return View("AssignSportsToUser", ut);
        }
        [HttpPost]
        public ActionResult AssignSportToThisUser(FormCollection collection)
        {
            var userId = _repository.FindUserByBadge(collection["badge"]);
            for (int i = 1; i < collection.Count; i++)
            {
                var sportName = collection[i];
                var sport = _repository.GetSport(sportName);
                _repository.AssignSportToUser(userId, sport);
            }
            return RedirectToAction("GetAthletes");
        }
    }
}