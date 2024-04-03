using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using AttorneyScheduler.DAL;

public class Program
{
    public static void Main(string[] args)
    {
        Batteries.Init();

        DbBuilder.BuildDatabase("Data Source=DAL\\Data\\AttorneyScheduler.db", "DAL\\Data\\CreateAll.sql");
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
