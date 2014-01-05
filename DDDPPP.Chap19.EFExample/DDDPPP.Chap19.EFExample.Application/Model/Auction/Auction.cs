using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.EFExample.Application.Infrastructure;

namespace DDDPPP.Chap19.EFExample.Application.Model.Auction
{
    public class Auction
    {        
        public Auction(Guid id, Money startingPrice, DateTime endsAt)
        {             
            Id = id;
            StartingPrice = startingPrice;
            EndsAt = endsAt;
        }

        private Auction(AuctionSnapShot snapShot)
        {
            this.Id = snapShot.Id;
            this.StartingPrice = new Money(snapShot.StartingPrice);
            this.EndsAt = snapShot.EndsAt;

            if (snapShot.CurrentBid != null)                          
                CurrentWinningBid = Bid.CreateFrom(snapShot.CurrentBid);            
        }

        public static Auction CreateFrom(AuctionSnapShot snapShot)
        {
            return new Auction(snapShot);
        }

        private Guid Id { get; set; }
        private Money StartingPrice { get; set; }
        private Bid CurrentWinningBid { get; set; }
        private DateTime EndsAt { get; set; }

        public AuctionSnapShot GetSnapShot()
        {
            var snapShot = new AuctionSnapShot();
            snapShot.Id = this.Id;
            snapShot.StartingPrice = this.StartingPrice.GetSnapshot().Value;
            snapShot.EndsAt = this.EndsAt;

            if (HasACurrentBid())            
                snapShot.CurrentBid = CurrentWinningBid.GetSnapShot();            

            return snapShot;
        }

        private bool HasACurrentBid()
        {
            return CurrentWinningBid != null;
        }

        private bool StillInProgress(DateTime currentTime)
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

        private bool BidderIsIncreasingMaximumBidToNew(Offer offer)
        {
            return CurrentWinningBid.WasMadeBy(offer.Bidder) && offer.MaximumBid.IsGreaterThan(CurrentWinningBid.MaximumBid);
        }

        private bool FirstOffer()
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
