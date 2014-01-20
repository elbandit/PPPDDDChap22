using System;
using DDDPPP.Chap19.MicroORM.Application.Infrastructure;

namespace DDDPPP.Chap19.MicroORM.Application.Model.Auction
{
    public class Auction : Entity<Guid>
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
                WinningBid = WinningBid.CreateFrom(snapShot.CurrentBid);
        }

        public static Auction CreateFrom(AuctionSnapShot snapShot)
        {
            return new Auction(snapShot);
        }

        private Money StartingPrice { get; set; }
        private WinningBid WinningBid { get; set; }
        private DateTime EndsAt { get; set; }

        public AuctionSnapShot GetSnapShot()
        {
            var snapShot = new AuctionSnapShot();
            snapShot.Id = this.Id;
            snapShot.StartingPrice = this.StartingPrice.GetSnapshot().Value;
            snapShot.EndsAt = this.EndsAt;

            if (HasACurrentBid())
                snapShot.CurrentBid = WinningBid.GetSnapShot();

            return snapShot;
        }

        private bool HasACurrentBid()
        {
            return WinningBid != null;
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
                    WinningBid = WinningBid.RaiseMaximumBidTo(offer.MaximumBid);
                else if (WinningBid.CanBeExceededBy(offer.MaximumBid))
                {
                    var newBids = new AutomaticBidder().GenerateNextSequenceOfBidsAfter(offer, WinningBid);

                    foreach (var bid in newBids)
                        Place(bid);
                }
            }
        }

        private bool BidderIsIncreasingMaximumBidToNew(Offer offer)
        {
            return WinningBid.WasMadeBy(offer.Bidder) && offer.MaximumBid.IsGreaterThan(WinningBid.MaximumBid);
        }

        private bool FirstOffer()
        {
            return WinningBid == null;
        }

        private void PlaceABidForTheFirst(Offer offer)
        {
            if (offer.MaximumBid.IsGreaterThanOrEqualTo(StartingPrice))
                Place(new WinningBid(offer.Bidder, offer.MaximumBid, StartingPrice, offer.TimeOfOffer));
        }

        private void Place(WinningBid newBid)
        {
            if (!FirstOffer() && WinningBid.WasMadeBy(newBid.Bidder))
                DomainEvents.Raise(new OutBid(Id, WinningBid.Bidder));               

            WinningBid = newBid;
            DomainEvents.Raise(new BidPlaced(Id, newBid.Bidder, newBid.CurrentAuctionPrice.Amount, newBid.TimeOfBid));
        }
    }
}
