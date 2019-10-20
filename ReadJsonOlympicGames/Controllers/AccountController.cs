using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ReadJsonOlympicGames.Controllers
{
    public class AccountController : Controller
    {
        OGData.Repository _repository = new OGData.Repository();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterNow()
        {
            return View("Register");
        }

        public ActionResult LoginNow()
        {
            return View("Login");
        }

        public ActionResult Login(string username, string password)
        {
            if (username == "" || password == "")
            {
                ViewBag.error = "Empty fields!";
                return View();
            }
            else
            {
                if (_repository.LoginIsValid(username, password))
                {
                    var name = _repository.FindUserByEmail(username);
                    FormsAuthentication.SetAuthCookie(username, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Incorrect email or password";
                    return View("login");
                }
            }
        }

        public ActionResult Register(string email, string firstName, string lastName, string dateOfBirth, string nationality, string password, string confirmPassword)
        {

            if (email == "" || password == "" || confirmPassword == "" || firstName == "" || lastName == "" || nationality == "" || dateOfBirth == "")
            {
                ViewBag.error = "Empty fields!";
                return View("register");
            }
            else
            {
                if (password == confirmPassword)
                {
                    if (_repository.UniqueEmail(email))
                    {
                        _repository.CreateUser(email, password, firstName, lastName, nationality, dateOfBirth);
                        return View("login");
                    }
                    else
                    {
                        ViewBag.error = "Email already exist!";
                        return View("register");
                    }
                }
                else
                {
                    ViewBag.error = "ConfirmPassword";
                    return View("register");
                }
            }

        }

        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}