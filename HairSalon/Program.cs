using HairSalon.Model.Authorization;
using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace HairSalon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddSpaStaticFiles(config=>config.RootPath = "ClientApp/dist");

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            string? connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<AppDbContext>(options=>options.UseSqlite(connectionString));


            builder.Services.AddAuthorization();


            //���������� ��� �����
            builder.Services.AddSingleton<IRepositoryOfServices, FakeRepoOfServices>();
            //����������� ��� ��������(���������� ������)
            builder.Services.AddSingleton<IRepositoryOfConfiguration, JsonRepoOfConfiguration>();
            //����������� ��� ������� ��������
            builder.Services.AddSingleton<IRepositoryOfRecords, FakeRepoOfRecords>();
            //����������� �����������
            builder.Services.AddSingleton<IRepositoryOfEmployees, FakeRepoOfEmployees>();
            builder.Services.AddTransient<IPicturesManager, PicturesManager>();


            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapDefaultControllerRoute();
            app.UseSpa(spa=>spa.Options.SourcePath = "ClientApp");
            app.Run();
        }
    }
}
