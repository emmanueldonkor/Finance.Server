using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using server.Data;



var builder = WebApplication.CreateBuilder(args);


/*var conString = builder.Configuration.GetConnectionString("FinanceContext");
builder.Services.AddNpgsql<FinanceContext>(conString); */

builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddProblemDetails();

var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");



builder.Services.AddControllers();

builder.Services.AddAuthentication(opts =>
{
 opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
 opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
 opts.TokenValidationParameters = new TokenValidationParameters
 {
  ValidateIssuerSigningKey = true,
  ValidIssuer = issuer,
  ValidateAudience = false,
  IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret!))
 };
});

builder.Services.AddControllers();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(settings =>{
   settings.SwaggerDoc("v1", new OpenApiInfo { Title = "Expenses", Version = "v1" });
});

//Cors
builder.Services.AddCors(options =>
{
 options.AddPolicy("ExpensesPolicy",
 builder =>
 {
   builder.WithOrigins("*")
           .AllowAnyHeader()
           .AllowAnyMethod();
 });
});


var app = builder.Build();

await app.Services.InitializeDataBaseAsync();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("ExpensesPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
