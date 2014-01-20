using System;

namespace DDDPPP.Chap19.MicroORM.Application.Model.BidHistory
{
    public class BidSnapshot
    {
        public Guid AuctionId { get; set; }
        public Guid Bidder { get; set; }
        public Decimal AmountBid { get; set; }
        public DateTime TimeOfMemberBid { get; set; }
    }
}
