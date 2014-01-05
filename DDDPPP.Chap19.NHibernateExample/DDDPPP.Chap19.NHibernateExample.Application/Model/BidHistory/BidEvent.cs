using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.NHibernateExample.Application.Model.Auction;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.BidHistory
{
    public class BidEvent : EntityBase<Guid>
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
    }
}
