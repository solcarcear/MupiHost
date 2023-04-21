namespace MupiModel.Dtos.Results
{
    public class ResultSyncEntitiesParams
    {
        public string Entity { get; set; }
        public int Frequency { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Active { get; set; }
        public bool IsFirstLoad { get; set; }


    }
}
