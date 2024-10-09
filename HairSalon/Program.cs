using HairSalon.Model.Configuration;
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
            builder.Services.AddSingleton<IRepositoryOfServices<Service>, FakeRepoOfService>();
            //репозиторий для настроек(управление сайтом)
            builder.Services.AddSingleton<IRepositoryOfConfiguration, JsonRepoOfConfiguration>();


            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.MapDefaultControllerRoute();
            app.UseSpa(spa=>spa.Options.SourcePath = "ClientApp");
            app.Run();
        }
    }
}
