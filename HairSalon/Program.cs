using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Services;

namespace HairSalon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddSpaStaticFiles(config=>config.RootPath = "ClientApp/dist");

            //репозитоий для услуг
            builder.Services.AddSingleton<IRepositoryOfServices, FakeRepoOfServices>();
            //репозиторий для настроек(управление сайтом)
            builder.Services.AddSingleton<IRepositoryOfConfiguration, JsonRepoOfConfiguration>();
            //репозиторий для записей клиентов
            builder.Services.AddSingleton<IRepositoryOfRecords, FakeRepoOfRecords>();
            //репозиторий сотрудников
            builder.Services.AddSingleton<IRepositoryOfEmployees, FakeRepoOfEmployees>();


            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapDefaultControllerRoute();
            app.UseSpa(spa=>spa.Options.SourcePath = "ClientApp");
            app.Run();
        }
    }
}
