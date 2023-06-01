using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using WebAPITutorial.Controllers;
using WebAPITutorial.DailyTask;
using WebAPITutorial.DBContexts;
using WebAPITutorial.DollarCurrency;
using WebAPITutorial.Exchanges;
using WebAPITutorial.Models.Identity;
using WebAPITutorial.TokenService;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
	options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
	options.SerializerSettings.ContractResolver = new DefaultContractResolver
	{
		IgnoreSerializableAttribute = true,
		IgnoreSerializableInterface = true,
		IgnoreShouldSerializeMembers = true
	};
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CryptoBook", Version = "v0.4"});


	options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "Enter a valid token...",
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});

	options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[]{ }
		}
	});
});
builder.Services.AddHostedService<DailyTasksService>();
builder.Services.AddSingleton<IDailyTask, DollarCurrencyDailyTask>();
builder.Services.AddSingleton<IDailyTask, KucoinVolatilityDailyTask>();
builder.Services.AddSingleton<IDollarCurrency, DollarCurrencyXML>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<BinanceExchange>();
builder.Services.AddSingleton<KucoinExchange>();
builder.Services.AddSingleton<HuobiExchange>();
builder.Services.AddSingleton<ExchangeControllerHelper>();

string? connectionString = builder.Configuration.GetConnectionString("PostgreSqlString");
builder.Services.AddDbContext<KucoinVolContext>(options =>
{
	options.UseNpgsql(connectionString);
});

builder.Services.AddDbContext<UserContext>(options =>
{
	options.UseNpgsql(connectionString);
});

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,  //определяет, должна ли проверка токена включать проверку issuer (издателя) токена. Издатель токена - это сущность, которая выпустила или создала токен.
		ValidateAudience = false,    //определяет, должна ли проверка токена включать проверку аудитории (audience) токена. Аудитория токена - это сущность или ресурс, для которого предназначен токен.
		ValidateLifetime = true,	//тоже самое с временем жизни
		ValidateIssuerSigningKey = true,    //определяет, должна ли проверка токена включать проверку ключа подписи (issuer signing key). Ключ подписи используется для проверки подлинности токена и обеспечения его целостности.
		ValidIssuer = builder.Configuration["Jwt:Issuer"],	//издатель токена
		ValidAudience = builder.Configuration["Jwt:Audience"],	//аудитория токена
		IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))	//ключ подлинности
	};
});

builder.Services.AddAuthorization(options =>
{
	options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
		.RequireAuthenticatedUser()
		.Build();
});

builder.Services.AddIdentity<User, IdentityRole<long>>(options =>
{
	options.User.RequireUniqueEmail = true;
})
	.AddEntityFrameworkStores<UserContext>()
	.AddDefaultTokenProviders()
	.AddUserManager<UserManager<User>>()
	.AddSignInManager<SignInManager<User>>();



WebApplication? app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors(builder =>
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader()
	);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
