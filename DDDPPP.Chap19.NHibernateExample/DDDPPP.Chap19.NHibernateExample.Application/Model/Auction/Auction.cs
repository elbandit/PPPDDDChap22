using System;
using System.Collections.Generic;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.Auction
{
    public class Auction : Entity
    {
        private Auction() { }

        public Auction(Guid id, Money startingPrice, DateTime endsAt)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("Auction Id cannot be null");

            if (startingPrice == null)
                throw new ArgumentNullException("Starting Price cannot be null");

            if (endsAt == DateTime.MinValue)
                throw new ArgumentNullException("EndsAt must have a value");

            Id = id;
            StartingPrice = startingPrice;
            EndsAt = endsAt;              
        }

        public Guid Id { get; private set; }
        private Money StartingPrice { get; set; }
        private Bid CurrentWinningBid { get; set; }
        private DateTime EndsAt { get; set; }

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
