using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupiModel.Dtos.Results
{
    public class ResultSyncEntitiesParams
    {
        public string Entity { get; set; }
        public int Frequency { get; set; }
        public DateTime From { get; set; }
    }
}
