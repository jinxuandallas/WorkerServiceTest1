using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceTest1.Model
{
    public class Product
    {
        public Guid id { get; set; }
        public int Keyword { get; set; }
        public string GlobalText { get; set; } = null;
    }
}
