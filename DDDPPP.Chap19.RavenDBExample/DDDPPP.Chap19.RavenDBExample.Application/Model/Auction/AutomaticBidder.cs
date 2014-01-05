using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.RavenDBExample.Application.Model.Auction
{
    public class AutomaticBidder // DomainService
    {
        public IEnumerable<Bid> GenerateNextSequenceOfBidsAfter(Offer offer, Bid currentWinningBid)
        {
            var bids = new List<Bid>();

            if (currentWinningBid.MaximumBid.IsGreaterThanOrEqualTo(offer.MaximumBid))
            {
                var bidFromOffer = new Bid(offer.Bidder, offer.MaximumBid, offer.MaximumBid, offer.TimeOfOffer);
                bids.Add(bidFromOffer);

                bids.Add(CalculateNextBid(bidFromOffer, new Offer(currentWinningBid.Bidder, currentWinningBid.MaximumBid, currentWinningBid.TimeOfBid)));
            }
            else
            {
                if (currentWinningBid.HasNotReachedMaximumBid())
                {
                    var currentBiddersLastBid = new Bid(currentWinningBid.Bidder, currentWinningBid.MaximumBid, currentWinningBid.MaximumBid, currentWinningBid.TimeOfBid);
                    bids.Add(currentBiddersLastBid);

                    bids.Add(CalculateNextBid(currentBiddersLastBid, offer));
                }
                else
                    bids.Add(new Bid(offer.Bidder, currentWinningBid.CurrentAuctionPrice.BidIncrement(), offer.MaximumBid, offer.TimeOfOffer));
            }

            return bids;
        }

        private Bid CalculateNextBid(Bid winningbid, Offer offer)
        {
            Bid bid;

            if (winningbid.CanBeExceededBy(offer.MaximumBid))
                bid = new Bid(offer.Bidder, offer.MaximumBid, winningbid.CurrentAuctionPrice.BidIncrement(), offer.TimeOfOffer);
            else
                bid = new Bid(offer.Bidder, offer.MaximumBid, offer.MaximumBid, offer.TimeOfOffer);

            return bid;
        }
    }
}
