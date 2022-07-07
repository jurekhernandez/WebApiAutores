using WebApiAutores;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

ILogger<Startup> servicioLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));
 startup.Configure(app, app.Environment, servicioLogger);
app.Run();

