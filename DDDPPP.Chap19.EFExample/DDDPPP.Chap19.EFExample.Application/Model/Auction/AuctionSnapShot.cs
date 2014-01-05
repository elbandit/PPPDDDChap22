using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.EFExample.Application.Model.Auction
{
    public class AuctionSnapShot
    {
        public Guid Id { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime EndsAt { get; set; }
        public BidSnapShot CurrentBid { get; set; }
    }
}
