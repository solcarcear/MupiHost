using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MupiModel.Dtos.Results;

namespace PraxiManager.Services.Imp
{
    public class SyncWorker : BackgroundService
    {

        private readonly ILogger<SyncWorker> logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IScheduleService _scheduleService;
        //private readonly IContactSyncService _contactSyncService;

        public SyncWorker(IScheduleService scheduleService, ILogger<SyncWorker> logger, IServiceProvider serviceProvider)
        {
            _scheduleService = scheduleService;
            this.logger = logger;
            _serviceProvider = serviceProvider;
        }

        private List<ResultSyncEntitiesParams> EntitiesSync = new List<ResultSyncEntitiesParams>();

        public override async Task StartAsync(CancellationToken cancellationToken)
        {

            EntitiesSync = await _scheduleService.GetSyncEntities();
            await base.StartAsync(cancellationToken);

        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                logger.LogInformation($"prueba de worker Azure a horas {DateTime.UtcNow:d}");

                if (EntitiesSync.Any(x => x.Entity == "Contactos"))
                {
                    var contactsConf = EntitiesSync.First(x => x.Entity == "Contactos");
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var scopeService = scope.ServiceProvider.GetRequiredService<IContactSyncService>();

                        await scopeService.SyncContactsFromMupi(contactsConf.Frequency, contactsConf.From, stoppingToken);
                    }
                }

                //await Task.Delay(5*1000, stoppingToken);
            }
        }
    }
}


//KIND TO USE SCOPE
//using (var scope = _serviceProvider.CreateScope())
//{
//    var scopeService = scope.ServiceProvider.GetRequiredService<ICaaoScopeService>();

//    scopeService.TestScopeService();
//    scopeService.TestScopeService();
//}
