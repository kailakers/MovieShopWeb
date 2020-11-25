# Architecture of the Application n-layer

Now we have to decide how we want to architect our application. Whatever the ways of creating our application one thing we have keep in mind is that our application should be maintainable and testable easily.
There are several design principles that we can apply so that our code base is not tightly coupled to each other.

* **Separation of Concerns**
  * The general idea behind Separation of Concerns is the your code should be written in such a way that so that different aspects of the application deal with only aspects that re related to them, no more no less. For example you might wanna display list of movies in the web page and you want to highlight the movies whose rating is more than 9. So here we are dealing with two things, one piece of code that deals with getting the list of movies and another piece of code that deals with highlighting those movies. It would be ideal to have business logic deal with getting list of movies and the UI logic dealing with highlighting those movies.
  
* **Single Responsibility**
  * The idea behind SRP is that a class should only have one reason to change. If your class has more than single responsibility then ideally it should be broken in to smaller classes( each one of those smaller classes again should follow SRP). But, again if you have a class that is very stable and does not require any changes then there is no point in applying SRP just for the sake of it. From our application architecture point of view we can have we can have different responsibilities separated into different layers of the application. Things such as Data Access code, Exceptional Handling, Mapping, Validation, Logging etc can be viewed as separate responsibilities that can be easily tested and maintainable.  
  
* **Don’t Repeat Yourself (DRY)**
  * As programmers our goal to avoid as much duplications as possible and in case if such duplication exists then we have to refactor them in to small re-usable components.
  
* **Encapsulation**
  * The idea behind encapsulation is that objects should be managing their own state and internal implementations and any external objects shouldn't able to change the state or implementation behavior.
  
* **Dependency Inversion**
  * The goal behind Dependency Inversion Principle is that high-level modules should not depend on low level modules but instead depend on abstractions.

Now that we know what kinda design principles we are going to follow when building our application it's time to decide how we want to architect our application.
There are two options we can go with

We can create a single ASP.NET Core project and have all our logic that may include business logic, domain entities, data access logic,error handling, logging etc in that single project. We can technically separate them into different folders. Although it might look simple there are very distinct disadvantages for this kinda architecture. Even though our MovieShop project might be on the smaller side we want to make sure our project architecture is adaptable to new changes and additional complexity. However having Single Project with multiple folders is not going to work in longer run as it only adds to additional organization nightmare. To overcome some of these issues its often we divide our application into multiple projects with each project residing inside a layer.
  
  **Single ASP.NET Core Project where separation is done by folders**

![alt text](images/01.05&#32;Monolithic&#32;Application.png "Single ASP.NET Core Project")

## Layered architecture

  As the application grows in complexity developers should be able to easily find where the certain functionality is implemented. Therefore it is ideal to divide our application into different layers where each layer is concerned with its own responsibilities which adheres to separation of concerns principle which we discussed above.

  There are number of advantages with Layered architecture, first thing which is obvious is your code organization is very clean, once the code is organized into layers common functionality can be reused throughout the application which intern helps us in writing less amount of code and follows the Don't Repeat yourself principle.

  With Layered architecture you can restrict the communication between layers so that when one of the layers is changed or completely replaced only those layers that communicate with that layer will be affected. This way we are not affecting the whole application instead confining the impact of those changes to only dependent layers.

  For example in the project that we are going to build we are going to have a layer called MovieShop.Services which will have all the business logic for the application. We are also going to have MovieShop.Data layer which is gonna deal with data access functionality with SQL Server using Entity Framework ORM. If we properly implement out our layers against public interfaces (abstractions) then even if we change the database to another one like Oracle, or change our ORM from Entity Framework to micro-orm like Dapper our Service Layer should have minimal to no impact. Thats because our Service Layer is inherently depending on public abstractions rather than concrete implementations. Not only we should be easily change the implementations but it will help us in writing Unit Tests where we don't need to touch our real database, instead our test cases will run against the Mock implementations of those public interfaces.

### Lets Create all the required layers for our Application

  **First**, we are going to create a layer called MovieShop.Entities. Except for our MovieShop.API project all of our layers are going to be a .NET Standard Class library projects.

  Right Click on src folder and add new project and select .NET Standard as project type and enter **MovieShop.Core** for Project name and point the location to src folder as shown in the below figure.
  
  The Entities project is the one that represents the business model classes are persisted and the idea is to use Entities Project across the application in almost all layers and it is the one with very minimal dependencies. 

  **MovieShop.Entities class library project (.NET Standard 2.1)**
  ![alt text](images/01.06&#32;MovieShop.Core.png " MovieShop.Core class library project (.NET Standard 2.1)")


  **Second**, we are going to create a layer called MovieShop.Data. 

  Right Click on src folder and add new project and select .NET Standard as project type and enter **MovieShop.Data** for Project name and point the location to src folder as shown in the below figure.
  
  The Data project is the one has data access implementations where all our repository code will reside. We are going to use Entity Framework Core to communicate with our SQL Server database. Along with repositories we will also have EF Core Types such as DbContext, Migrations etc.
  
  Since DbContext needs DbSets which are represented by our Entities (that needs to be persisted) we need to add reference for MovieShop.Entities project inside the MovieShop.Data project.

  **MovieShop.Data class library project (.NET Standard 2.1)**
  ![alt text](images/01.07&#32;MovieShop.Data.png " MovieShop.Data class library project (.NET Standard 2.1)")


  **Third**, we are going to create a layer called MovieShop.Services.

  Right Click on src folder and add new project and select .NET Standard as project type and enter **MovieShop.Services** for Project name and point the location to src folder as shown in the below figure.
  
  The Services project is the one has all the business logic code for our application  and its going to communicate with MovieShop.Data project and use MovieShop.Entities project. Therefore we need to add reference for MovieShop.Entities project and MovieShop.Data inside the MovieShop.Services project.

  **MovieShop.Services class library project (.NET Standard 2.1)**
  ![alt text](images/01.08&#32;MovieShop.Services.png " MovieShop.Services class library project (.NET Standard 2.1)")


#### Here is how your solution should look like after creating all the layers

 **Solution with all the layers**
  ![alt text](images/01.09&#32;All&#32;Layers.png"Solution with all the layers")

**Dependency Diagram of all the layers**
 ![alt text](images/01.09&#32;Dependency&#32;Graph.png "Dependency Diagram of all the layers")
