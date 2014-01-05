using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.RavenDBExample.Application.Infrastructure;

namespace DDDPPP.Chap19.RavenDBExample.Application.Model.Auction
{
    public class Auction
    {
        private Auction() { }

        public Auction(Guid id, Money startingPrice, DateTime endsAt)
        {
            Id = id;
            StartingPrice = startingPrice;
            EndsAt = endsAt;
        }

        public Guid Id { get; private set; }
        public Money StartingPrice { get; private set; }
        public Bid CurrentWinningBid { get; private set; }
        public DateTime EndsAt { get; private set; }

        public bool HasACurrentBid()
        {
            return CurrentWinningBid != null;
        }

        public bool StillInProgress(DateTime currentTime)
        {
            return (EndsAt > currentTime);
        }

        public void PlaceBidFor(Offer offer, DateTime currentTime)
        {
            if (StillInProgress(currentTime))
            {
                if (FirstOffer())
                    PlaceABidForTheFirst(offer);
                else if (BidderIsIncreasingMaximumBidToNew(offer))
                    CurrentWinningBid = CurrentWinningBid.RaiseMaximumBidTo(offer.MaximumBid);
                else if (CurrentWinningBid.CanBeExceededBy(offer.MaximumBid))
                {
                    var newBids = new AutomaticBidder().GenerateNextSequenceOfBidsAfter(offer, CurrentWinningBid);

                    foreach (var bid in newBids)
                        Place(bid);
                }
            }
        }

        public bool BidderIsIncreasingMaximumBidToNew(Offer offer)
        {
            return CurrentWinningBid.WasMadeBy(offer.Bidder) && offer.MaximumBid.IsGreaterThan(CurrentWinningBid.MaximumBid);
        }

        public bool FirstOffer()
        {
            return CurrentWinningBid == null;
        }

        private void PlaceABidForTheFirst(Offer offer)
        {
            if (offer.MaximumBid.IsGreaterThanOrEqualTo(StartingPrice))
                Place(new Bid(offer.Bidder, offer.MaximumBid, StartingPrice, offer.TimeOfOffer));
        }

        private void Place(Bid newBid)
        {
            if (!FirstOffer() && CurrentWinningBid.WasMadeBy(newBid.Bidder))
                DomainEvents.Raise(new OutBid(Id, CurrentWinningBid.Bidder, newBid.CurrentAuctionPrice.Amount));

            CurrentWinningBid = newBid;
            DomainEvents.Raise(new BidPlaced(Id, newBid.Bidder, newBid.CurrentAuctionPrice.Amount, newBid.TimeOfBid));
        }
    }
}
