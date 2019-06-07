# CineTest
Application made for Fullstack Code Challenge (v2)

The solution has a Web API and a MVC Web Application, both written in C# Asp.Net Core v2.2, which I decided to use because:

● I'm pretty comfortable with it, since I use every day;

● .Net Core is multi-platform, so I can broaden my deployment environment choices.

● As I already had a linux virtual machine set up with some domains/subdomains, I could use it to deploy the solution.

The app is deployed at https://cinetest.youkko.com, and the api is deployed at https://cinetestapi.youkko.com/api/ (doesn't contain any view).

The Web API uses JWT Bearer Token for authentication, but for the purpose of this test, it has a fixed authentication key.
It has one controller with two functions: List and Search.
It returns a JSON similar to the one it takes data from, but with more results and different pagination (depending on how many movies it shows per page). Nevertherless, this pagination system needs improvements and fixes.

The MVC Web App uses Razor with JQuery and Bootstrap (default for .Net MVC WebApps), and has a single responsive page which shows a list of movies on the right side, and loads the movie info in the left pane. There's a search bar in the header and the pagination numbers / links in the footer.
Since that was the last part I made, due to the short time I had to develop everything, It has tons of bugs and need improvements, specially on the responsivity part.

Current status: the API is fully working. The Web App has some errors on startup routine that I couldn't fix, so it's useless for now (should be something simple to fix, but I ran out of time).

Deploy instructions will vary depending on chosen environment.
As I deployed on Ubuntu 18.04 x64, I created a batch file to compile all solution projects and, after setting up the environment (install dotnet core 2.2, set up subdomains, redirections, https certificates, etc. in apache), I just needed to run all apps there.

All "appsettings.json" files should be reviewed to configure API details, such as Keys and URLs.

About third-party libraries, I used:

● Newtonsoft.Json, a library to work with Json on C#;

● RestSharp, a simple REST and HTTP API Client, to make it easier to request data from Web APIs.

Note: despite RestSharp, I made my own client later to request from my WebAPI, so I do use both, one in MVC Web App, and the other in Web API.
