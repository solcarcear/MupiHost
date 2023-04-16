using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatioManager.Models
{
    public class UsrMupiSyncParameters
    {
        public static readonly string urlGetActiveParams = $"{nameof(UsrMupiSyncParameters)}?$filter=UsrActivo eq true&$select=Id,name,UsrFrecuencia,UsrFechaModificacionMUPI";

        public string Id { get; set; }
        public string Name { get; set; }
        public bool UsrActivo { get; set; }
        public DateTime UsrFechaModificacionMUPI { get; set; }
        public int UsrFrecuencia { get; set; }
    }
}
