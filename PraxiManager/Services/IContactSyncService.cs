namespace PraxiManager.Services
{
    public interface IContactSyncService
    {
        public Task SyncContactsFromMupi(int frequency,DateTime from, CancellationToken stoppingToken);
    }
}
