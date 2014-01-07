using System;
using DDDPPP.Chap19.NHibernateExample.Application.Model.Auction;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.BidHistory
{
    public class BidEvent : EntityBase
    {
        private BidEvent()
        { }

        public BidEvent(Guid auctionId, Guid bidderId, Money amountBid, DateTime timeOfBid)
        {
            AuctionId = auctionId;
            Bidder = bidderId;
            AmountBid = amountBid;
            TimeOfBid = timeOfBid;
        }

        public Guid AuctionId { get; private set; }
        public Guid Bidder { get; private set; }
        public Money AmountBid {get; private set;}
        public DateTime TimeOfBid { get; private set; }
        public Guid Id { get; private set; }
    }
}
