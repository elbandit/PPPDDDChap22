using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.EFExample.Application.Model.BidHistory
{
    public class BidEventSnapshot
    {
        public Guid AuctionId { get; set; }
        public Guid Bidder { get; set; }
        public Decimal AmountBid { get; set; }
        public DateTime TimeOfMemberBid { get; set; }
    }
}
