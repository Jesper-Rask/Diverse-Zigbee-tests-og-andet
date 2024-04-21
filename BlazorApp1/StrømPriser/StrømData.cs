using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strømpriser
{
    public class StrømData
    {
        public List<Record> records { get; set; }

    }

    public class Record
    {
        public DateTime HourDK { get; set; }
        public string PriceArea { get; set; }
        public double SpotPriceDKK { get; set; }

    }

    public class ShownPrice
    {
        public DateTime date { get; set; }
        public TimeOnly time { get; set; }
        public double price{ get; set; }
        public int pricePercent { get; set; }
        public string unit { get; set; }
        public string priceInString { get; set; }
    }

}
