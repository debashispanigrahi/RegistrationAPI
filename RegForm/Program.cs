using RegForm.DBClasses;
using RegForm.Interfaces;
using RegForm.Middleware;
using RegForm.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .WriteTo.Console().WriteTo.File(Path.Combine(AppContext.BaseDirectory, "Log", DateTime.Now.ToString("dd-MM-yyyy") + ".txt"))
  .Enrich.FromLogContext()
  .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder.WithOrigins(new string[] { "http://localhost:4200" }).AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddTransient<IConnection, BaseConnection>();
builder.Services.AddTransient<IRegistration, RegService>();
builder.Services.AddTransient<IEmail, EmailService>();
builder.Services.AddTransient<IDBInsert, DBInsert>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();