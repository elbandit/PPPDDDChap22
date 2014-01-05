using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.MicroORM.Application.Model.Auction
{
    public class BidSnapShot
    {
        public Guid BiddersId { get; set; }
        public DateTime TimeOfBid { get; set; }
        public decimal BiddersMaximumBid { get; set; }
        public decimal CurrentPrice { get; set; }
    }
}
