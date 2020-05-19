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
* Adding view
* Style view
