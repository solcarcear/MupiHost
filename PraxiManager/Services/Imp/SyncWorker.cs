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

        private int _executionCount;
        private int _executionCount2;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{

            //    logger.LogInformation($"CAAO {DateTime.UtcNow:g}");

            //    if (EntitiesSync.Any(x => x.Entity == "Contactos"))
            //    {
            //        var contactsConf = EntitiesSync.First(x => x.Entity == "Contactos");
            //        using (var scope = _serviceProvider.CreateScope())
            //        {
            //            var scopeService = scope.ServiceProvider.GetRequiredService<IContactSyncService>();

            //           // await scopeService.SyncContactsFromMupi(contactsConf.Frequency, contactsConf.From, stoppingToken);
            //            logger.LogInformation($"CAAO {DateTime.UtcNow:g}");

            //            await Task.Yield();
            //            await Task.Run(async () => {


            //                logger.LogInformation("CAAO 10");
            //                await Task.Delay(10 * 1000, stoppingToken);
            //            });
            //        }
            //    }

            //    //await Task.Delay(5*1000, stoppingToken);
            //}


            logger.LogInformation("Timed Hosted Service running.");

            // When the timer should have no due-time, then do the work once now.

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(5));


            await DoWork(stoppingToken);
            await DoWork2();
          

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {

                   var asdasdas= await Task.WhenAny(DoWork(stoppingToken), DoWork2());


            

                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Timed Hosted Service is stopping.");
            }




        }
        private async Task DoWork(CancellationToken stoppingToken)
        {
            int count = Interlocked.Increment(ref _executionCount);



            Thread.Sleep(10 * 1000);
            logger.LogInformation($"DoWork1. Thread {count}: {DateTime.Now:G}");
            File.AppendAllLines("D:\\CAAO.txt",new List<string> { $"DoWork1.{count}: {DateTime.Now:G}" });

        }
        private async Task DoWork2()
        {

            int count = Interlocked.Increment(ref _executionCount2);

            logger.LogInformation($"DoWork2. Thread {_executionCount2}: {DateTime.Now:G}");
          
            File.AppendAllLines("D:\\CAAO2.txt", new List<string> { $"DoWork2.{count}: {DateTime.Now:G}" });


        }
    }
}

