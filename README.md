# Read Json - Olympic Games
**ASP.NET project - FULL STACK**

Main functionalities of this solution:
- Register as volunteer
- Login as admin, volunteer and athelete
- Upload `jsonData.json` file - it will read a file and store all athletes(from file) to database.
- View and delete athletes
- Add Olympic Sports
- Assign sports to athletes
- Create olympic competition (Case - Ski Jumping)
- Assign athletes(only those who are asigned to sport that competition is about) to created competition
- Jump with every athlete 3 times - jump length is randomly generated
- After the competition is done, system will award medals to the first 3 athletes

To login as **admin** you have to use `admin` for username and `admin` for password.

To login as **athlete** you have to use *badge number* as a username and *birth date* as a password.

You have to update filePath in the UploadFile method in HomeController. There is static filePath.

System requirements: 
- import database `JsonProjectDataBseNew.bacpac` to Microsoft SQL Server Management Studio.
- update connection strings inside `web.config` and `app.config` files - `data source="your sql server name"`.
- update filePath in the UploadFile method in HomeController. There is static filePath.
- load the solution using `ReadJsonOlympicGames.sln` into Visual Studio and run.

