# ContactApi
ContactApi is a sample of a REST service that showcases a production quality application.  It includes Entity Framework as ORM, logging, versioning, routing, Ninject DI, and unit testing.  This API performs a secured basic CRUD operations.  I exposed a status update as well to change a contact's status from active to inactive and vice versa.

The main folders are lib, logs, and src.

## Documentation

### Folder Structure
**lib**: This is where I set Nuget to store all the third party binaries I used for ContactApi. It includes, but is not limited to log4net, Moq, and Ninject.

**logs**: This is where I set log4net to store all the exception that happen within the ContactApi solution.

**src**: This is where the ContactApi.sln and all the projects related to it reside.

### Projects
**ContactApi.Data**: Contains ContactApi's model plain old CLR objects, ContactApiDb context, and the ContactService.  I used Entity Framework for ORM.

**ContactApi.Web.Api**: This is the main REST service.  This is hosted by IIS.  This project contains all the Web API controllers, models, and ninject configurations.

**ContactApi.Web.Common**: Contains functionality common to the Web.Api and service applications.

### Test Projects: 
**Tests/ContactApi.Data.Tests**: Unit tests for the ContactService.

**Tests/ContactApi.Test.Utility**: MockHelper is the only class found here. This is a helper class for unit testing.

**Tests/ContactApi.Web.Api.Tests**: Unit tests for the controllers and models.  I tested the different data annotations for the models.  I want to showcase how you can test the ModelState (data annotations).

### References
- ContactApi.Web.Api
  - ContactApi.Data
  - ContactApi.Web.Common
- ContactApi.Web.Common
  - ContactApi.Data
- ContactApi.Data.Tests
  - ContactApi.Data
  - ContactApi.Test.Utility
- ContactApi.Test.Utility
  - ContactApi.Data
- ContactApi.Web.Api.Tests
  - ContactApi.Data
  - ContactApi.Test.Utility
  - ContactApi.Web.Api

### Database
The database for this solution only has one table (dbo.Contacts).

```
CREATE TABLE [dbo].[Contacts](
	[ContactId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[EmailAddress] [nvarchar](255) NOT NULL,
	[PhoneNumber] [varchar](15) NULL,
	[Status] [char](8) NOT NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Contacts] ON [dbo].[Contacts]
(
	[EmailAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
```

I created a unique non-clustered index for email address. I want to make sure that all email addresses are unique but at the same time, I want the client to be able to change the email address.

### Object-Relational Mapping
Entity framework is used as the ORM framework for ContactApi.  ContactApiDb class inherits DbContext that uses the connectionString's name with similar name for auto mapping.  The connectionString is separated in a different configuration file (`ConnectionString.config`).  If you look at the `web.config`, the `<connectionStrings />` section is pointing to `ConnectionString.config`.  If you are forking this repository, don't forget to add your own `ConnectionString.config`.

### Logging
[log4net](https://logging.apache.org) is used as the logging mechanism for ContactApi.  The settings are found in the `web.config` with its own `<log4net />` section.  The logging is stored in the main `./logs` folder.  The settings is a rolling log file that has a max file size of 5MB. There are multiple ways to configure `log4net`.  Here is the [documentation](https://logging.apache.org/log4net/release/manual/introduction.html) for `log4net`.

### Versioning
With a public API, you will always want to have versioning in your API solution.  ContactApi showcased versioned API controllers to support legacy API methods.  If you have two similar controllers on different namespaces (`ContactApi.Web.Api.Controllers.V1` and `ContactApi.Web.Api.Controllers.V2`), the code will compile.  However, requests will always be routed to the first namespace it will find.  With this issue, I have implemented an `ApiVersionConstraint.cs` class and an `ApiVersion1RoutePrefixAttribute.cs` that will handle this issue.  The `ApiVersionConstraint.cs` inherits `IHttpRouteConstraint` which checks if the value matches the allowed version.  The value comes from the attribute that is set in the controller.  If you look at `ContactsController.cs` you will see that it is attributed with `[ApiVersion1RoutePrefix]` which then calls the `ApiVersion1RoutePrefixAttribute.cs` class.  

```
public class ApiVersion1RoutePrefixAttribute : RoutePrefixAttribute
    {
        private const string RouteBase = "api/{apiVersion:apiVersionConstraint(v1)}";
        private const string PrefixRouteBase = RouteBase + "/";

        public ApiVersion1RoutePrefixAttribute(string routePrefix) : base(string.IsNullOrWhiteSpace(routePrefix)
            ? RouteBase
            : PrefixRouteBase + routePrefix)
        {
        }
    }
```

In the class above, when used as an attribute, it is similar to what I've shown in `JwtController.cs`

```
..
    [RoutePrefix("api/{apiVersion:apiVersionConstraint(v2)}/jwt")]
..
```

The `ApiVersion1RoutePrefix` handles all that text found in `JwtController` and it will pass the value to the inherited `RoutePrefixAttribute` base.

### Routing
The routing is inspired by Mike Wasson's blog post about [ASP.NET Web API: Using Namespaces to Version Web APIs](https://blogs.msdn.microsoft.com/webdev/2013/03/07/asp-net-web-api-using-namespaces-to-version-web-apis/).  `NamespaceHttpControllerSelector.cs` inherits from `IHttpControllerSelector` which handles the selection of what controller to use.  I added a catch all route in `WebApiConfig.cs`.  If the controller can't be found, it will throw a 404 status code.  Here is a snippet of the `WebApiConfig.cs`:

```
            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));

            ...

            config.Routes.MapHttpRoute(
                name: "CatchAllUrlTo404",
                routeTemplate: "{*uri}"
            );
```

Here, I replaced the `IHttpControllerSelector` and replaced with the `NamespaceHttpControllerSelector` and if a client tries to hit any unknown URLs, it will return a 404 instead of showing the IIS default error message stating that the path is not found.

### Dependency Injections
Loose coupling is a major design philosophy of object oriented programming.  Dependency injections make this possible.  You want to be able to use classes that are loosely coupled to one another.  It should use interfaces rather than concrete classes.
ContactApi uses [Ninject DI](http://www.ninject.org/) tool.  Install the `Ninject.Web.Common.WebHost` and it will add a `Ninject.Web.Common.cs` which handles the creation and destruction of the container instance.  In order for the DI container to be useful, it must be created early in the application start up.  It must be available at all times while the application is running and must be destroyed as one of the last steps in the application during shutdown. `Ninject.Web.Common`s `Start()` method will initialize the kernel and add the `NinjectDependecyResolver` as the dependencyresolver shown below:

```
		public static void Start() 
        {
        	... 

            IKernel container = null;

            bootstrapper.Initialize(() =>
            {
                container = CreateKernel();
                return container;
            });

            var resolver = new NinjectDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
```

The Kernel creation calls the `NinjectConfigurator`, which is where I bind all the the necessary DI. For this example I have the `log4net` and the `IContactService`.

### Unit Testing
ContactApi uses [Moq](https://github.com/Moq/moq4/wiki/Quickstart) and [NUnit](https://nunit.org/) for unit testing.  Unit testing is now implemented by many software companies.   A code without tests is a bad code.  With tests, we can change our code quickly and verifiably.  I created 2 major test projects.  `ContactApi.Data.Tests` tests the very heart of the application which is the `ContactServices` and the other project is the `ContactApi.Web.Api.Tests` which tests the controllers and model's data annotataions.  As stated earlier, the data annotations / ModelState doesn’t need to be tested, but I want to show how it is done if someone insists.

## Instructions on how to run locally
After forking this repository, You will need two files.
- AppSettings.config
- ConnectionString.config

I ignored these files for security purposes.

`AppSettings.config` needs a `jwtSecretKey` which looks something like:

```
<appSettings>
  <add key="jwtSecretKey" value="txT!h?::baNfs)}=UP=8s=6w!T-G,b]Jx%?g"/>
</appSettings>
``` 

and the `ConnectionString.config` will look something like:

```
<connectionStrings>
  <add name="ContactApiDb" connectionString="Data Source=DataSourceFoo;Initial Catalog=DB_Foo;User ID=DB_UserId;Password=DBPassword;Integrated Security=False;" providerName="System.Data.SqlClient" />
</connectionStrings>
```

You can create your own database with the script above, and modify this connection string with correct data.

Exposed Apis:
- api/v1/contacts/list
  - HTTPGET
  - Request headers
    - authorization: Bearer jwtToken
- api/v1/contacts/add
  - HTTPPOST
  - Request body
  	- { contactmodel }
  - Request headers
    - authorization: Bearer jwtToken
- api/v1/contacts/edit
  - HTTPPUT
  - Query string
  	- contactid={guidId}
  - Request body
  	- { contactmodel }
  - Request headers
    - authorization: Bearer jwtToken
- api/v1/contacts/delete
  - HTTPDELETE
  - Query string
  	- contactid={guidId}
  - Request headers
    - authorization: Bearer jwtToken
- api/v1/contacts/updatestatus
  - HTTPPUT
  - Query string
  	- contactid={guidId}
  	- status={active|inactive}
  - Request headers
    - authorization: Bearer jwtToken
- api/v2/jwt/generatetoken
  - HTTPGET
  - Query string
    - expiresinminutes={int}

To get the jwtToken, hit the `api/v2/jwt/generatetoken` first to generate the token and add it as a authorization headers.

A sample complete URL:
`http://localhost:50602/api/v2/jwt/generatetoken?expiresinminutes=60`




