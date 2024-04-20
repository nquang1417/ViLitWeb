using ViL.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder, builder.Services); // calling ConfigureServices method
var app = builder.Build();
startup.Configure(app, builder.Environment);