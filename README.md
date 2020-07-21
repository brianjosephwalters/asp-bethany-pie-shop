# Bethany's Pie Shop

Based on Pluralsight course: [Building Web Applications with ASP.NET Core MVC](https://app.pluralsight.com/library/courses/building-aspdotnet-core-mvc-web-applications/table-of-contents)
And Continuing with: [Building an Enterprise Application with ASP.NET Core MVC](https://app.pluralsight.com/library/courses/aspdotnet-core-mvc-enterprise-application/table-of-contents)

## Building Web Applications with ASP.NET Core MVC
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
    * services.AddSession() AND app.UseSession()
        * UseSession() called before routing.
    * "Shopping Cart" session wired up in StartUp class.
        * Created when user accesses site
        * requests check for presence of Shopping Cart GUID - either create one or return
    * HttpContextAccessor is required for getCart() to grab the CardId from the headers.
        * Add to services in AddHttpContextAccessor() for injection.
* View Components
    * Partial Views data is passed from calling view.
    * Similar to Partial View - only used for displaying partial content
    * Data not passed from calling view.  Supports Dependency Injected.  Always linked from parent view.
    * Standalone components where logic sits behind it.  Login, Navigation, Shopping Carts
    * base ViewComponent:  public, non-abstract, non-nested class.
    * needs a View: Views/Shared/Components
    * Need to use @await and invoke async from a View or general layout.  New to Razor.
* Custom Tag Helpers
    * Enable server-side C# code to participate in creating and rendering HTML elements in Razor files.
    * Inherits from TagHelper. Name of class is element name + TagHelper
    * Register all tag helpers.  Place them in the same location.

### Module 8 - Order Forms
* Built-in Tag Helpers to build Forms
    * Form, Input, Lable, Textarea, Select, Validation
    * <label asp-for="attribute"></label> => <label for="attribute">Attribute</label>
    * Some tag helpers are attributes for other tag helpers:
        * asp-controller
        * asp-action
        * asp-route, asp-route-*
        * asp-anti-forgery
    * [Display] attribute on model tells us what to display in the label.
* Model Binding
    * ASP .NET Core will match the arguments coming from the form post to the object properties.
    * Model binders are components that help take values from the request and build out method parameters.  Invoked in this order!
        * Form Data
        * Route variables
        * Query String
* Validation
    * ASP .NET Core's model binding engine can perform a check to see if the values received by model binding match requirements.
        * ModelState.IsValid is a side product of Model Binding - True/False based on validation errors.
        * ModelState.GetValidAtionState() allows us to probe a particular property.
        * AddModelError()
    * Default validations on as [attributes] on the properties in the model class.
        * Can create custom attributes to be applied.
        * Required, StringLength, Range, RegularExpression, DataType (Phone, email URL)
    * ValidationSummary is a tag helper to display validation errors. (All is default for all errors)
    * What is BindNever?

### Module 9 - Login Capabilities
* ASP .NET Core Identity
    * Membership system, able to authenticate and authorize users.
    * Supports external providers
    * SQL Server built-in
    * Scaffolding views in Core 2.1+
    * IdentityDbContext, Configuration Changes, Constraints on Registration, Security Cookies, User Options
    * Must add nuGet packages in Core 3.0+
        * Microsoft.AspNetCore.Identity.EntityFrameworkCore
        * Microsoft.AspNetCore.Identity.UI
    * Update AppDbContext to inherit from IdentityDbUser<IdentityUser> (and created-migration / update-database)
* Adding Authentication
    * Use scaffolding in ASP.NET 2.1 and 3.0 (can still use manual approach) - Razor Class Libraries
    * Razor Class Libraries with views, pages, controllers, view components, etc. added via a library for reuse.
        * Can override items in these libraries.
    * Use scaffolding to generate source in your application - copies from Razor Class libraries. 
        * We can modify these
        * UserManager<IdentityUser> CRUD on users.  Handles changes to database
        * SignInManager<IdentityUser>
    * What is the difference between MVC Views and Razor Pages?
* Enabling Authorization
    * Allow or deny access to resources for users.
    * [Authorize] attribute on controller or actions.
        * Many more options such as Roles

## Summary Notes
* ViewResult is an ActionResult that renders a view
    * We'll return the superclass or interface if there is a return value that doesn't have a view.
    * It seems like we try to compartmentalize methods that simply return ViewResults from those that involve ActionResults or RedirectTos as well.
* RedirectTo... is used when we need to navigate.
* Use ViewModels when the view needs to begin with an empty object that it will return (add forms)


## Building an Enterprise Application with ASP.NET Core MVC
* I had to implement a bunch of features that were not in the previous course (probably a version difference).
* I implemented am IHostedService that executes on startup, accesses the database services, and creates an admin user if one doesn't exit.  Need to scop this to local environments once I learn environments.
[Identity Scaffold Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/scaffold-identity?view=aspnetcore-3.1&tabs=visual-studio)

### Module 2 - Overview
* ASP .NET Core Identity System
* Security Features
* Administration Backend and Model validation
* Tag Helpers & View Components
* Performance with Caching & Logging
* CI/CD Visual Studio Team Services

*** Module 3 - Authenticating and Authorizing Users with ASP.Net Identity
* User Management
    * Create & manage users from within the application
    * User management is built-in
    * CRUD operations and more - UserManager<IdentityUser>
    * Microsoft.AspNetCore.Identity.EntityFrameCore (.csproj file)
    * Most actions done using the UserManager class.
        * Create users using `await _userManager.CreateAsync(...)`
        * Framework handles password storage/salting/etc.
        * AddIdentity() call in service can set password configurations.
* Extending Identity User Class
    * Extend IdentityUser, add new fields, and update migrations
    * Replace use of IdentityUser with ApplicationUser (even though the subclass is registered, it doesn't seem to allow the use of the superclass.)
    * Update views for the new properties.
* Role Management
    * Role-based / Claims-based / Policy-based
    * RoleManager Framework should handle creating roles, adding roles to users.  How roles are used is part of MVC and [Authorize].
* Claims-based authorization
    * External source of information for user's permissions.
    * talks about what the subject is.
    * key value pairs
    * policy based - we have to register polices defining claim requirements.
    * service.AddAuthorization(
        {=> options.AddPolicy("DeletePie", => policy.RequireClaim("Delete Pie"))}
    * We refer to the Policy later in our code.
        [Authorize(Policy = "DeletePie")]
    * Can combine several policies.  All must be met.
    * Combine with Roles. Both must be met.
    * Custom Policies - built-in makes use of a Requirement, a Handler for Requirement, and a preconfigured Policy.
        * Add one or more requirements to a Policy.
        * IAuthorizationRequirement implements an empty interface but can hold custom methods around that requirement.
        * AuthorizationHandler<RequirementType> evalues the properties of the requirement.  One requirement can have multiple handlers.
        * Handlers are then registered as a property of a Policy, which are created via service options.
* Adding Third-party Authentication
    * Use 3rd party providers to provide claims about the identity of the user.
    * Don't need to store user credentials in our apps.
    * Provide two factor authentication out of the box.
    * Most common 3rd part yproviders are built into ASP .Net Core 3.
        * Register Authentication Middleware as a service.
    * Enable SSL in our application

### Module 4 - Hardening your site against attacks
* Sanitizing Input
    * XSS attacks (cross site scripting)
    * Input data should be cleaned before it is stored in the database of the application.
        * HTML input
        * Http Headers
        * Query strings
        * parsing attributes on images
    * HTML Encoding 
        * Razor does HTML encoding for us.
        * Check javascript and attributes
    * Regular expressions when accepting input.
        * used to have anti-XSS.
    * Request Validation
        * checks incoming data for potential threats. 
        * Throws error and input is blocked.
        * Not being added to ASP Core, but gives us full control.
    * Built in Encoders - Html, Javascript, Url.
* CSRF Cross-Site Request Forgery
    * Tricks end user to execute an unwanted action on site they are currently authenticated.
    * Typically a state changing request, since they usually can't get access to the response.
    * For example, place a link for a request on one site onto another site.  
      A user logged into the first site but in the second site could send the request to to the second 
      site without realizing it because they have a cookie.
    * Victim is logged in to a site.  Browser doesn't know forged request is malicious (confused deputy).
    * Social engineering required - need to get user to click link.
    * Can be used along side xss
    * OWASP Cheat sheets:
        * Header Validation - validate Http request.  Headers can be spoofed, but harder to do on victim's browser
        * Synchronizer token
            * A random string is sent back from server. Client needs to send back same string with next request.  Value not available to attacker.
        * Double submit cookie.
            * Value is sent to client in cookie and request param.
            * Compared on server.
    * .Net Core Anti-Forgery Token
        * first time requesting a page, a unique token is sent to client associated with user's identity.
        * when user makes a request, server validates token is present and matches the identity.
        * for attacks, Request is sent, but token is not.
        * Already registered and available via DI.  Extra config in the startup class.
        * [ValidateAntiForgeryToken] / [AutoValidateAntiForgeryToken] - to all POST requests.

### Module 5 - Validating Complex Models
* Model Binding
    *  Allows us to create .Net objects from the data in an Http Request, used as parameters for Action methods.
    *  Binding values is abstracted away by the framework.
    *  Works with simple types, complex types, arrays.
* Model Binders
    *  objects responsible for providing modeling binding systems with data from particular parts of the request.
    *  Form Values, Routing Values, Query String Values - in that order.
    *  Binding simple data types - string when expecting int will turn to 0.
        * use int? to allow differentiation between 0 and null.
    *  Binding complex types
