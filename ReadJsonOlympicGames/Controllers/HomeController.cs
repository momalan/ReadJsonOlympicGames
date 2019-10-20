using Newtonsoft.Json.Linq;
using ReadJsonOlympicGames.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReadJsonOlympicGames.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        OGData.Repository _repository = new OGData.Repository();
        public ActionResult Index()
        {
            UsersInserted ui = new UsersInserted();
            ui.users = null;
            return View(ui);
        }
        public ActionResult AddFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase txtFile)
        {
            string fileName = Path.GetFileNameWithoutExtension(txtFile.FileName);
            string extension = Path.GetExtension(txtFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string filePath = @"C:\Users\Muhammed\source\repos\ReadJsonOlympicGames\ReadJsonOlympicGames\Files\" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Files/"), fileName);
            txtFile.SaveAs(fileName);
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                var details = JObject.Parse(json);
                List<string> notInsertedUsers = new List<string>();
                List<UserTable> insertedUsers = new List<UserTable>();
                UsersInserted ui = new UsersInserted();
                foreach (var i in details["athlets"])
                {
                    if (_repository.UniqueBadgeNumber(i["badge_number"].ToString()))
                    {
                        string firstName = i["fist_name"].ToString();
                        string lastName = i["last_name"].ToString();
                        string dateOfBirth = i["date_of_birth"].ToString();
                        string nationality = i["nationality"].ToString();
                        string badgeNumber = i["badge_number"].ToString();
                        string photo = i["photo"].ToString();
                        string gender = i["gender"].ToString();
                        _repository.AddAthlete(firstName, lastName, dateOfBirth, nationality, badgeNumber, photo, gender);
                        var user = _repository.GetSpecificUserByBadge(badgeNumber);
                        UserTable ut = new UserTable();

                        ut.id = user.id;
                        ut.firstName = user.firstName;
                        ut.lastName = user.lastName;
                        ut.badgeNumber = user.badgeNumber;
                        insertedUsers.Add(ut);
                    }
                    else
                    {
                        notInsertedUsers.Add(i["badge_number"].ToString());
                    }
                }
                ui.users = insertedUsers;
                ViewBag.data = notInsertedUsers;
                return View("Index", ui);

            }

        }

        public ActionResult GetAllUsers()
        {
            var listOfUsers = _repository.GetAllUsers();
            UsersInserted ui = new UsersInserted();
            ui.users = listOfUsers.Select(x => new UserTable
            {

                id = x.id,
                firstName = x.firstName,
                lastName = x.lastName,
                role = x.role


            }).ToList();

            return View("AllUsers", ui);
        }

        public ActionResult DeleteUser(int id)
        {
            _repository.DeleteAthlete(id);
            return RedirectToAction("GetAllUsers");
        }

        public ActionResult UserInfo(int id)
        {

            var user = _repository.GetSpecificUser(id);
            UserTable userinfo = new UserTable();
            userinfo.id = user.id;
            userinfo.firstName = user.firstName;
            userinfo.lastName = user.lastName;
            userinfo.dateOfBirth = user.dateOfBirth;
            userinfo.gender = user.gender;
            userinfo.nationality = user.nationality;
            userinfo.badgeNumber = user.badgeNumber;

            return View("UserInfo", userinfo);
        }

        public ActionResult MyProfile(int id)
        {

            var user = _repository.GetSpecificUser(id);
            UserTable userinfo = new UserTable();
            userinfo.id = user.id;
            userinfo.firstName = user.firstName;
            userinfo.lastName = user.lastName;
            userinfo.dateOfBirth = user.dateOfBirth;
            userinfo.gender = user.gender;
            userinfo.nationality = user.nationality;
            userinfo.badgeNumber = user.badgeNumber;

            return View("MyProfile", userinfo);
        }

        public ActionResult EditMyProfile(int id)
        {

            var user = _repository.GetSpecificUser(id);
            UserTable userinfo = new UserTable();
            userinfo.id = user.id;
            userinfo.firstName = user.firstName;
            userinfo.lastName = user.lastName;
            userinfo.dateOfBirth = user.dateOfBirth;
            userinfo.gender = user.gender;
            userinfo.nationality = user.nationality;
            userinfo.badgeNumber = user.badgeNumber;

            return View("EditProfile", userinfo);
        }


        public ActionResult EditMyProfileNow(int id, string firstName, string lastname, string nationality)
        {

            _repository.EditUser(id, firstName, lastname, nationality);

            return RedirectToAction("Index");
        }

    }
}