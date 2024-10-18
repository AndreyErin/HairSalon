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

            //���������� ��� �����
            builder.Services.AddSingleton<IRepositoryOfServices, FakeRepoOfServices>();
            //����������� ��� ��������(���������� ������)
            builder.Services.AddSingleton<IRepositoryOfConfiguration, JsonRepoOfConfiguration>();
            //����������� ��� ������� ��������
            builder.Services.AddSingleton<IRepositoryOfRecords, FakeRepoOfRecords>();
            //����������� �����������
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
