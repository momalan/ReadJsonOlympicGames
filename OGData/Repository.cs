using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace OGData
{
    public class Repository
    {
        private JsonProjectDataBseNewEntities _db = new JsonProjectDataBseNewEntities();
        public void Dispose()
        {
            _db.Dispose();
        }

        public void AddAthlete(string firstName, string lastName, string dateOfBirth, string nationality, string badgeNumber, string photo, string gender)
        {
            Helper h = new Helper();
            TableOfUser user = new TableOfUser();
            user.firstName = firstName;
            user.lastName = lastName;
            user.dateOfBirth = dateOfBirth;
            string hashPass = Crypto.HashPassword(dateOfBirth);
            user.password = hashPass;
            user.nationality = h.Nationality(nationality);
            user.badgeNumber = badgeNumber;
            user.photo = photo;
            user.gender = gender;
            user.role = "athlete";
            _db.TableOfUsers.Add(user);
            _db.SaveChanges();
        }

        public List<TableOfUser> GetSpecificAthletes(string sport)
        {
            return _db.OlympicSports.FirstOrDefault(x => x.nameOfSport == sport).TableOfUsers.ToList();

        }

        public bool UniqueSportName(string nameOfSport)
        {
            var sport = _db.OlympicSports.FirstOrDefault(x => x.nameOfSport == nameOfSport);
            if (sport == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Competition GetCompetition(string nameOfCompetiton)
        {
            var com = _db.Competitions.FirstOrDefault(x => x.nameOfCompetition == nameOfCompetiton);
            return com;

        }

        public void AssignCompetitionToAthlete(string badge, Competition competition)
        {
            _db.TableOfUsers.FirstOrDefault(x => x.badgeNumber == badge).Competitions.Add(competition);
            _db.SaveChanges();
        }

        public List<Competition> GetAllCompetitions()
        {
            return _db.Competitions.ToList();
        }

        public bool UniqueCompetitionName(string comName)
        {
            var competition = _db.Competitions.FirstOrDefault(x => x.nameOfCompetition == comName);
            if (competition == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveCompetition(string nameOfCompetiton, string description, string sport, string startDate, string time)
        {
            Competition competition = new Competition();
            competition.nameOfCompetition = nameOfCompetiton;
            competition.description = description;
            competition.sportName = sport;
            competition.startDate = startDate;
            competition.time = time;
            competition.status = "toGo";
            _db.Competitions.Add(competition);
            _db.SaveChanges();
        }

        public void DeleteCompetition(int id)
        {
            var competition = _db.Competitions.Include("TableOfUsers").FirstOrDefault(x => x.id == id);
            var compresults = _db.CompetitionResults.Include("TableOfUsers").Where(x => x.nameOfCompetition == competition.nameOfCompetition).ToList();
            foreach (var i in compresults)
            {
                _db.CompetitionResults.Remove(i);
                _db.SaveChanges();

            }

            _db.Competitions.Remove(competition);
            _db.SaveChanges();
        }

        public List<OlympicSport> GetAllSports()
        {
            return _db.OlympicSports.ToList();
        }

        public void DeleteAthlete(int id)
        {
            var athleteToDelete = _db.TableOfUsers.Include("OlympicSports").Include("Competitions").Include("CompetitionResults").FirstOrDefault(x => x.id == id);
            _db.TableOfUsers.Remove(athleteToDelete);
            _db.SaveChanges();
        }

        public List<TableOfUser> GetAthletes()
        {
            return _db.TableOfUsers.Where(x => x.role == "athlete").ToList();
        }

        public void InsertSport(string nameOfSport, string description)
        {
            OlympicSport sport = new OlympicSport();
            sport.nameOfSport = nameOfSport;
            sport.description = description;
            _db.OlympicSports.Add(sport);
            _db.SaveChanges();
        }

        public TableOfUser FindUserByEmail(string username)
        {
            var userVolunteer = _db.TableOfUsers.FirstOrDefault(x => x.email == username);
            if (userVolunteer == null)
            {
                return _db.TableOfUsers.FirstOrDefault(x => x.badgeNumber == username);
            }
            else
            {
                return userVolunteer;
            }
        }

        public TableOfUser GetSpecificUserByBadge(string badgeNumber)
        {
            return _db.TableOfUsers.FirstOrDefault(x => x.badgeNumber == badgeNumber);
        }

        public int FindUserByBadge(string badgeName)
        {
            return _db.TableOfUsers.FirstOrDefault(x => x.badgeNumber == badgeName).id;
        }

        public void AssignSportToUser(int idUser, OlympicSport sport)
        {
            _db.TableOfUsers.FirstOrDefault(x => x.id == idUser).OlympicSports.Add(sport);
            _db.SaveChanges();
        }

        public List<TableOfUser> GetAllUsers()
        {
            return _db.TableOfUsers.ToList();
        }

        public List<CompetitionResult> GetAthletesResults(string comName)
        {
            return _db.CompetitionResults.Where(x => x.nameOfCompetition == comName).ToList();
        }

        public void StartCompetition(string nameOfCompetition)
        {
            var competition = _db.Competitions.FirstOrDefault(x => x.nameOfCompetition == nameOfCompetition);
            competition.status = "started";
            _db.Entry(competition).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }

        public void EndCompetition(string nameOfCompetition)
        {
            var competition = _db.Competitions.FirstOrDefault(x => x.nameOfCompetition == nameOfCompetition);
            competition.status = "ended";
            _db.Entry(competition).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }

        public string UserRole(string name)
        {
            var user = _db.TableOfUsers.FirstOrDefault(x => x.email == name);
            if (user == null)
            {
                var athlete = _db.TableOfUsers.FirstOrDefault(x => x.badgeNumber == name);
                if (athlete == null)
                {
                    return "none";
                }
                else
                {
                    var role = athlete.role;
                    return role;
                }

            }
            else
            {
                return user.role;
            }
        }

        public OlympicSport GetSport(string sportName)
        {
            return _db.OlympicSports.FirstOrDefault(x => x.nameOfSport == sportName);
        }

        public bool LoginIsValid(string username, string password)
        {
            var userAthletePassword = _db.TableOfUsers.FirstOrDefault(x => x.badgeNumber == username);
            var athletePassword = "";
            if (userAthletePassword != null)
            {
                athletePassword = athletePassword + userAthletePassword.password;
            }
            if (userAthletePassword == null)
            {

                var volunteer = _db.TableOfUsers.FirstOrDefault(x => x.email == username);
                var volunteerPassword = "";
                if (volunteer != null)
                {
                    volunteerPassword = volunteerPassword + volunteer.password;
                }
                if (volunteerPassword == null)
                {
                    return false;
                }
                else
                {
                    if (Crypto.VerifyHashedPassword(volunteerPassword, password))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (Crypto.VerifyHashedPassword(athletePassword, password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<AthletesMedalResults> CompetitionMedals(string nameOfCompetition)
        {
            var athletes = _db.Competitions.FirstOrDefault(z => z.nameOfCompetition == nameOfCompetition).TableOfUsers.ToList();
            int x = 0;
            List<AthletesMedalResults> list = new List<AthletesMedalResults>();

            for (int i = 0; i < athletes.Count; i++)
            {
                var results = athletes[i].CompetitionResults.ToList();
                AthletesMedalResults amr = new AthletesMedalResults();
                List<CompetitionResult> listofThisComResults = new List<CompetitionResult>();
                for (int j = 0; j < results.Count; j++)
                {
                    if (results[j].nameOfCompetition == nameOfCompetition)
                    {
                        listofThisComResults.Add(results[j]);

                    }
                }
                List<int> longestJump = new List<int>();
                for (int z = 0; z < listofThisComResults.Count; z++)
                {
                    longestJump.Add(Int32.Parse(listofThisComResults[z].result));
                    if (z == listofThisComResults.Count - 1)
                    {
                        longestJump.Sort();
                        int lJump = longestJump.Count();
                        amr.id = athletes[i].id;
                        amr.firstName = athletes[i].firstName;
                        amr.lastName = athletes[i].lastName;
                        amr.badgeNumber = athletes[i].badgeNumber;
                        amr.nationality = athletes[i].nationality;
                        amr.longestJump = longestJump[lJump - 1];
                        list.Add(amr);
                        x = 0;
                    }
                }
            }
            return list;
        }

        public List<TableOfUser> GetAthletesOfCompetition(string comName)
        {
            var com = _db.Competitions.FirstOrDefault(x => x.nameOfCompetition == comName);
            var athletes = com.TableOfUsers.ToList();

            for (int i = 0; i < athletes.Count; i++)
            {
                var results = athletes[i].CompetitionResults.ToList();
                List<CompetitionResult> listOfResults = new List<CompetitionResult>();
                athletes[i].CompetitionResults.Clear();
                for (int j = 0; j < results.Count; j++)
                {
                    if (results[j].nameOfCompetition == comName)
                    {
                        listOfResults.Add(results[j]);
                    }
                }
                athletes[i].CompetitionResults = listOfResults;

            }
            var ath = athletes;
            return athletes;
        }

        public void AddCompetitionResultToAthlete(string comName, int userId, int length)
        {
            CompetitionResult cr = new CompetitionResult();
            cr.nameOfCompetition = comName;
            cr.result = length.ToString();
            var competitionResult = _db.CompetitionResults.Add(cr);
            _db.SaveChanges();
            _db.TableOfUsers.FirstOrDefault(x => x.id == userId).CompetitionResults.Add(competitionResult);
            _db.SaveChanges();
        }

        public void EditUser(int id, string firstName, string lastname, string nationality)
        {
            var userEdit = _db.TableOfUsers.FirstOrDefault(x => x.id == id);

            userEdit.firstName = firstName;
            userEdit.lastName = lastname;
            userEdit.nationality = nationality;

            _db.Entry(userEdit).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

        }

        public void CreateUser(string email, string password, string firstName, string lastName, string nationality, string dateOfBirth)
        {
            Helper h = new Helper();
            string hashedPassword = Crypto.HashPassword(password);
            TableOfUser user = new TableOfUser();
            user.email = email;
            user.password = hashedPassword;
            user.firstName = firstName;
            user.lastName = lastName;
            user.nationality = h.Nationality(nationality);
            user.dateOfBirth = dateOfBirth;
            user.role = "volunteer";

            _db.TableOfUsers.Add(user);
            _db.SaveChanges();
        }

        public bool UniqueEmail(string email)
        {
            var uniqueEmail = _db.TableOfUsers.FirstOrDefault(x => x.email == email);
            if (uniqueEmail == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UniqueBadgeNumber(string badgeNumber)
        {
            var user = _db.TableOfUsers.FirstOrDefault(x => x.badgeNumber == badgeNumber);
            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public TableOfUser GetSpecificUser(int id)
        {
            return _db.TableOfUsers.FirstOrDefault(x => x.id == id);
        }
    }
}
