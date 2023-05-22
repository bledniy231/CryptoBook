using Microsoft.EntityFrameworkCore;
using WebAPITutorial.Controllers;
using WebAPITutorial.DailyTask;
using WebAPITutorial.DBContext;
using WebAPITutorial.DollarCurrency;
using WebAPITutorial.Exchanges;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<DollarCurrencyDailyTaskService>();
builder.Services.AddHostedService<KucoinVolatilityDailyTaskService>();
builder.Services.AddSingleton<DollarCurrencyDailyTask>();
builder.Services.AddSingleton<KucoinVolatilityDailyTask>();
builder.Services.AddSingleton<IDollarCurrency, GetDollarCurrencyXML>();
builder.Services.AddSingleton<BinanceExchange>();
builder.Services.AddSingleton<KucoinExchange>();
builder.Services.AddSingleton<HuobiExchange>();
builder.Services.AddSingleton<ExchangeControllerHelper>();
builder.Services.AddDbContext<KucoinVolContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlString"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
