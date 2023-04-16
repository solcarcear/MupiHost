using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupiModel.Dtos.Results
{
    public class ResultCreatioRequest<T>
    {
        public bool IsSuccess { get; set; }
        public T Entity { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Messages { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }
}
