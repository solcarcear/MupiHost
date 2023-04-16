using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupiModel.Dtos.AppSettings
{
    public class MupiSetting
    {
        public string DbConnection { get; set; }
        public int DefaultSyncFrequency { get; set; }
        public DateTime DefaultSyncDate { get; set; } 
    }
}
