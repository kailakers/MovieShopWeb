# Introduction and Creating our Project

## Introduction and scope of the project

The goal of this course is to build a real-world web application using .NET technologies and Angular. To be more specific we are going to use ASP.NET Core version 3, Entity Framework Core version 3 and Angular 8 as our main technologies. Along with these technologies we are going to use HTML, CSS and Bootstrap 4 for making a relatively good-looking and responsive web app. For our database we are going to use SQL Server development version, or you can also use SQL Server Express.

Before we get started, I would like to show you guys the web application we are going to build. Here is the app that acts a Movie Store where registered customers you can buy movies and add movies to their favorites list. This web app has lots of real-world functionality like Server-side pagination, searching, JWT Authentication for security, Role based Authorization where we restrict access to some pages by roles. During the course we are also going to learn some good design principles, coding standards etc. Finally, the data we are going to use for this course comes from TMDB website which has excellent API to get lots of information about movies. SO, I would like to thank them.

## Prerequisites

As I said in the introduction video, we are going to build the real-world application I am expecting certain level of knowledge from you. 
Very First thing is you should have good knowledge of C#, at least 3-6 months of working experience and understand some of the core C# concepts such as Interfaces, Extension methods, Collections and some LINQ.

Good understanding of REST concepts and HTTP protocol including different HTTP verbs such as GET, POST, PUT and DELETE.
Previous working knowledge of either ASP.NET MVC or ASP.NET Web API would be good, but I am expecting you to be expert in either of those. At least understand what Model is, what is Controller is etc.

We will be using Entity Framework Core 3.0 in this project, but I am not expecting any knowledge on that, but decent knowledge of Entity Framework 6 would be required. But we will go through all the steps from beginning to create the database using EF. 

Finally coming to front-end having beginner knowledge in HTML, CSS and JavaScript is enough. Bootstrap knowledge is helpful but not required. But I am expecting you to have used Angular before for at least 6 weeks. So, you know the concept of Components, Services, Directives etc. Even if you don’t know those things, I Definitely think you can follow this course as we will be creating or using all of those.

## Installing the Required Software

If you are on Windows, please go ahead and download the required software from the document I provided. We are going to use Visual Studio 2019 for our ASP.NET Core API Development and Visual Studio Code which is  a lightweight IDE for our Angular development. If you are on macOS then you can use Visual Studio Code for both API and Angular development. But for SQL Server on mac you need to have Docker installed and have SQL Server run on it. I have provided the instructions on how to install it. 

We are going to use Postman to test our API and SQL Management Studio for general database development like checking whether tables have been properly created, running SQL scripts to create initial data etc. If you are on macOS then Visual Studio Code has an extension for connecting to SQL Server which I have provided in the document.

Here are the links for required software

  * <https://visualstudio.microsoft.com/>
  * <https://code.visualstudio.com/>
  * <https://www.getpostman.com/>
  * <https://www.microsoft.com/en-us/sql-server/sql-server-downloads>
  


## Brief Introduction about .NET Core and ASP.NET Core

.NET Core and ASP.NET Core are the latest offerings from Microsoft which have lots of advantages and new features from the traditional .NET Framework

  * .NET core cross platform, that means you can develop and run it in Windows, Mac OS and Linux.
  * Its lightweight modular platform (making it extremely flexible and extensible) and runs very fast.
  * Open-source and community-focused
  * It was architected to provide an optimized development framework for apps that are deployed to the cloud or run on-premises
  * Its an unified framework for building Web Applications, Web API's and also integrates easily with other modern client-side frameworks such as Angular, React and Vue 
  * Built-in dependency injection
  * It can be hosted in IIS, self-hosted, Apache or even Docker
  
## Creating a ASP.NET Core Api project and testing the basic setup with postman

**Create Empty Solution and name it MovieShop**

**Empty Visual Studio Solution named MovieShop**
![alt text](../images/01.01&#32;Create&#32;Empty&#32;Solution.png "Empty Solution")

Add two new Folders called src (remember they are physical folders in windows explorer) and test that will contain all our application source code and testing code respectively

In the src folder right click and add new project select ASP.NET Core Web Application and in the next screen make sure your Project name is MovieShop.API and Location is pointing to src folder and then click Create.

**Solution Folders organization**
![alt text](images/01.02&#32;MovieShop&#32;Solution.png "Solution Folders organization")

**ASP.NET Core API Project**
![alt text](images/01.03&#32;ASP.NET&#32;Core&#32;API&#32;Create.jpg "ASP.NET Core API Project")

**Create MovieShop.API ASP.NET Core API Project inside src folder**
![alt text](images/01.04&#32;Web&#32;API&#32;Project.png "Create MovieShop.API ASP.NET Core API Project inside src folder")


## Creating our First Controller and testing in Postman

The goal of this section is to create an simple controller that returns some json data and test it out in Postman
First go ahead and delete the existing ValuesController that was created by default.

Lets create a new controller
Right-Click on controllers folder and Select Add new Controller and then select Empty API Controller.Name it as GenresController. Controllers Folder **&rarr;** Add Controller **&rarr;** API Controller-Empty **&rarr;** **GenresController**

When we create an Web API Controller class it derives from **ControllerBase** class by default. There is another class in ASP.NET Core called **Controller**, which derives from **ControllerBase** class. But we usually don't use **Controller** class as it has support for Views, which we don't need in our Application.

__ControllerBase__ class has many methods and properties that help us in handling all the HTTP requests. It has methods such as returning different HTTP status codes such as OK 200, Created 201, NotFound 404 etc.

If you look at the two attributes on top of the class we have **[Route("api/[controller]")]** which helps in constructing the URL path for each method in the controller

  * It will replace **[controller]** with the name of the controller (which by convention is the controller class name minus the "Controller" ), in our scenario the controller name is **Genres**. Also Routing is case insensitive
  * **[ApiController]**  indicates that the controller responds to web API requests
  
We have another attribute __[ApiController]__ which helps a controller to have more API-specific behaviors such as following:

  * Attribute Routing, so that by default it tells Controller to use Attribute-based routing where it will map incoming request to the action methods. Those action methods will be inaccessible via conventional routes.
  * Lets say when a model is sent to our action methods and is invalid then ApiController is smart enough to trigger HTTP 400 BadRequest response.
  * There will be many scenarios when building your REST API where you want to get information from from HTTP Request from its body, headers, query string etc. __[ApiController]__ will help us in those scenarios.
  
Lets add a simple Action method that returns some dummy Genres to our GenresController.


```cs

        [HttpGet]
        public IActionResult GetAllGenres()
        {
            var genres = new[]
                         {
                             new {Id = 1, Name = "Action"},
                             new {Id = 2, Name = "Comedy"},
                             new {Id = 3, Name = "Thriller"}
                         };
            return Ok(genres);
        }
```

Here **[HttpGet]** denotes a method that responds to an HTTP GET request.
Now lets run the application and open __Postman__ and go to the url <https://localhost:44346/api/genres> (remember your port may vary). You should see the following output.

  **Making GET Request with Postman**
  ![alt text](images/01.10&#32;Postman&#32;Result.png " Making GET Request with Postman")

```json
  [
    {
        "id": 1,
        "name": "Action"
    },
    {
        "id": 2,
        "name": "Comedy"
    },
    {
        "id": 3,
        "name": "Thriller"
    }
]
```
