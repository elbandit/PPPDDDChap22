using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.MicroORM.Application.Model.Auction
{
    public class Bid
    {
        private Bid() { }

        public Bid(Guid bidder, Money maximumBid, Money bid, DateTime timeOfBid)
        {
            Bidder = bidder;
            MaximumBid = maximumBid;
            TimeOfBid = timeOfBid;
            CurrentAuctionPrice = new Price(bid);
        }
        
        public Guid Bidder { get; private set; }
        public Money MaximumBid { get; private set; }
        public DateTime TimeOfBid { get; private set; }
        public Price CurrentAuctionPrice { get; private set; }

        public Bid RaiseMaximumBidTo(Money newAmount)
        {
            if (newAmount.IsGreaterThan(MaximumBid))
                return new Bid(Bidder, newAmount, CurrentAuctionPrice.Amount, DateTime.Now);
            else
                throw new ApplicationException("Maximum bid increase must be larger than current maximum bid.");
        }

        public bool WasMadeBy(Guid bidder)
        {
            return Bidder.Equals(bidder);
        }

        public bool CanBeExceededBy(Money offer)
        {
            return CurrentAuctionPrice.CanBeExceededBy(offer);
        }

        public bool HasNotReachedMaximumBid()
        {
            return MaximumBid.IsGreaterThan(CurrentAuctionPrice.Amount);
        }

        public BidSnapShot GetSnapShot()
        {
            var snapShot = new BidSnapShot();

            snapShot.BiddersId = this.Bidder;
            snapShot.BiddersMaximumBid = this.MaximumBid.GetSnapshot().Value;
            snapShot.CurrentPrice = this.CurrentAuctionPrice.Amount.GetSnapshot().Value;
            snapShot.TimeOfBid = this.TimeOfBid;

            return snapShot;
        }

        public static Bid CreateFrom(BidSnapShot bidSnapShot)
        {
            return new Bid(bidSnapShot.BiddersId, new Money(bidSnapShot.BiddersMaximumBid), new Money(bidSnapShot.CurrentPrice), bidSnapShot.TimeOfBid);
        }
    }
}
