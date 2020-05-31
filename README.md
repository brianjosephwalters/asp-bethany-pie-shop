# Bethany's Pie Shop

Based on Pluralsight course: [Building Web Applications with ASP.NET Core MVC](https://app.pluralsight.com/library/courses/building-aspdotnet-core-mvc-web-applications/table-of-contents)

### Module 3 Notes:
* Program.cs: main method, like in Spring.  Specify "startup" class to bootstrap framework.
* Startup.cs: Define request handling pipeline. Configuring Services needed. Dependency Injection starting point.
    * register AddControllersWithView service for MVC
    * configure middleware components (more explicit than SpringBoot applications). Order matters.
* CreateDefaultBuilder: adds appsettings.json file for configuration, sets environment (dev by default)
* ConfigureWebHostDefaults: creates webserver (castrol?), IIS
* UseStartup: Configures our own application.
* IServiceCollection is entrypoint to add services needed by application - built-in and custom.
    * AddControllersWithViews() = MVC
    * Debug Environment variables (under project configuration) ASPNETCORE_ENVIRONMENT is checked by env.isDevelopment();  Can configure based on environment.
    * Add requirement for https: app.UseHttpsRedirection()
    * App should serve static files: app.UseStaticFiles(), searching /wwwroot by default.
    * UseRouting() && UseEndpoints() enable MVC to respond to incoming requests - mapping requests to code.
    * Add MapControllerRoute to allow route mapping definitions.

### Module 4 Notes
Model View Controller pattern - separation of concerns.
Razor is the view engine.
* Model & Repository
    * Group of classes that make up domain data
    * Classes that manage the data
    * Repository consumed by the Consumer class. - use DI. services.AddScoped<Interface, Concrete>
        AddTransitent - every time we get a new clean instance
        AddSingleton - every time we get back hte same instance
        AddScoped - creates one instance per request, but shared within a request.
* Controller
    * responds to user input, updates the model, no knowledge of data persistence (hidden by Model)
    * an Action.  In the case of MVC, returns a View as a result.
    * Binds data to the View. View is just a template.
* Adding Views
    * HTML template .cshtml, "plain" or strongly-typed views.
    * Razor markup allows C# code in template.  Code is executed on _server side_.
    * Each controller has a folder where all of its view are rendered.
    * ViewBag addds data to the view. dynamic.
    * Normally, use strongly typed.
        * type the model in the template. 
        * c# for looping
    * View model doesn't always map to domain model.
        * use view bag
        * use viewmodel - wraps multiple properties.
    * Layout files are templates that help prevent code duplication.
        * Views can point to the layout the want to use.
        * Views can use a ViewStart.cshtml file.
        * Views can load data with an @using syntax.
* Style view
    * Library Manager for client-side package sources. Older versions used bower. Or add packages manually.
    * UseStaticFiles() points to wwwroot

### Module 5 - Entity Framework Core
* Entry Framework as an ORM (EF Core)
    * LINQ statements
    * SQL Server and other relational (and non-relational) databases
    * Only supports code-first based support (not edmx base approach)
    * Id (or IID) are default primary keys
    * What you need - Domain Classes, Database Context (manages entity objects during runtime, change tracking, persisting), Application Configuration (AppSettings.json file)
    * Configure by adding `services.AddDbContext` in `ConfigureServices()`.
    * DbSet return type in DbContext indicates a mapping to a table?
    * Added Microsoft.EntityFrameworkCore.SqlServer & ...Tools as nugets
    * Access appsettings.json using IConfiguration injected into Startup.cs
* Initializing Db from code
    * EF Core supports Migrations to create database model
        * add-migration <Migration Name>
        * update-database
    * EF Core can populate database
        * HasData() to seed data through a migration
* Migrations
    * Adding data with `OnModelCreating()`.
    * Creating data initialization migrations and applying to database.

### Module 6 - Routing
*  Overview
    *  Maps URL to endpoint
    *  ASP.Net can generate outgoing links
    *  Convention based route (ASP .Net Core) / Attribute based routing (APIs)
    *  Middleware names are different between 2.1 and 3.
*  Segments:  First part after host points to controller, second part to action. 
    *  pattern: {controller}/{action}
    *  URL Pattern -> Handler -> Action on MVC Controller.
    *  Configure with MapControllerRoute - ensure defined in correct order with most specific at top.
    *  Route Defaults: {controller=Home}/{action=Index}
    *  Passing Values: {controller=Home}/{action=Index}/{id?}
        * ? = optional
        * Adding constraints for {id:int?}
*  Tag Helpers
    * Adds navigation to site - ASP.Net Core will generate correct links.
        * Use instead of Html helpers.
    * Executes server-side - triggers generation of correct link.
    * Built in or custom.
    * asp-controller, asp-action, asp-route-*, asp-route (forces)
    * @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
### Module 7 - Improving Views
* Partial Views
    * Partial Views. (start with _)
    * partial tag indicates it should be search for in the shared folder.
* Sessions
    * Asp .NET Core adds sessions and associates with a cookie.
    * services.AddSessions() and app.UseSessions()