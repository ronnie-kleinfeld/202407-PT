using Comtec.BE.Config;
using Comtec.BE.LogEx;
using System.Reflection;

namespace Comtec.Tdi2.Web {
    public class Program {
        public static void Main(string[] args) {
            AppConfigHandler.Init();
            // override the log file directory
            AppConfigHandler.Data.Log.LogToFile = true;
            AppConfigHandler.Data.Log.LogFileDirectory = "C:\\Tdi2Log";
            LogHelper.Init(AppConfigHandler.Data.Log);
            LogHelper.WriteInformation(MethodBase.GetCurrentMethod(), "Website initializing");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}