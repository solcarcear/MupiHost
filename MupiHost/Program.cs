using CreatioManager.HttpClients;
using CreatioManager.Services;
using CreatioManager.Services.Imp;
using Microsoft.EntityFrameworkCore;
using MupiModel.Dtos.AppSettings;
using MupiSource.DbContext;
using PraxiManager.Services;
using PraxiManager.Services.Imp;

using ICreatioContactService = CreatioManager.Services.IContactService;
using IMupiContactService = MupiBussines.Services.IContactService;

using CreatioContactService = CreatioManager.Services.Imp.ContactService;
using MupiContactService = MupiBussines.Services.Imp.ContactService;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var config = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json").Build();
        var creatioSettings = config.GetSection(nameof(CreatioSetting)).Get<CreatioSetting>() ?? new CreatioSetting();
        var mupiSettings = config.GetSection(nameof(MupiSetting)).Get<MupiSetting>() ?? new MupiSetting();
        builder.Services.AddSingleton(mupiSettings);
        builder.Services.AddSingleton(creatioSettings);

        builder.Services.AddDbContext<MupiDbContext>((s, opts) => {
            var confService = s.GetRequiredService<MupiSetting>();

            opts.UseSqlServer(confService.DbConnection);
        });

        builder.Services.AddSingleton(s =>
        {
            var confService = s.GetRequiredService<CreatioSetting>();
            return new CreatioAuth(confService);
        });
        builder.Services.AddHttpClient<BatchClient>("BatchClient", (s, c) =>
        {
            var authService = s.GetRequiredService<CreatioAuth>();

            c.BaseAddress = new Uri($"{creatioSettings.UrlCreatio}");
            c.Timeout = TimeSpan.FromHours(2);
            // Account API ContentType
            c.DefaultRequestHeaders.Add("ForceUseSession", "true");
            c.DefaultRequestHeaders.Add("Accept", "application/json");
            c.DefaultRequestHeaders.Add("BPMCSRF", authService.CsrfToken);
        }).ConfigurePrimaryHttpMessageHandler((s) =>
        {
            var authService = s.GetRequiredService<CreatioAuth>();
            return new HttpClientHandler()
            {
                CookieContainer = authService.AuthCookie
            };
        });

        builder.Services.AddSingleton<ISyncParametersService, SyncParametersService>();
        builder.Services.AddSingleton<IScheduleService, ScheduleService>();

        builder.Services.AddHostedService<SyncWorker>();

        builder.Services.AddScoped<ICreatioContactService, CreatioContactService>();
        builder.Services.AddScoped<IMupiContactService, MupiContactService>();
        builder.Services.AddScoped<IContactSyncService, ContactSyncService>();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}