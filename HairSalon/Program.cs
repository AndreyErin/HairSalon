using HairSalon.Model.Authorization.Admin;
using HairSalon.Model.Authorization.Api_Jwt;
using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace HairSalon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddSpaStaticFiles(config => config.RootPath = "ClientApp/dist");

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthJwtOptions.ISSUER,
                            ValidateAudience = true,
                            ValidAudience = AuthJwtOptions.AUDIENCE,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthJwtOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });


            string? connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));


            builder.Services.AddAuthorization();


            //репозитоий для услуг
            builder.Services.AddSingleton<IRepositoryOfServices, FakeRepoOfServices>();
            //репозиторий для настроек(управление сайтом)
            builder.Services.AddSingleton<IRepositoryOfConfiguration, JsonRepoOfConfiguration>();
            //репозиторий для записей клиентов
            builder.Services.AddSingleton<IRepositoryOfRecords, FakeRepoOfRecords>();
            //репозиторий сотрудников
            builder.Services.AddSingleton<IRepositoryOfEmployees, FakeRepoOfEmployees>();
            builder.Services.AddTransient<IPicturesManager, PicturesManager>();


            var app = builder.Build();

            //для тестов
            app.UseCors(options => options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapDefaultControllerRoute();
            app.UseSpa(spa => spa.Options.SourcePath = "ClientApp");
            app.Run();
        }
    }
}
