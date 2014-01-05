using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using DDDPPP.Chap19.RavenDBExample.Application.Model.Auction;
using DDDPPP.Chap19.RavenDBExample.Application.Model.BidHistory;
using DDDPPP.Chap19.RavenDBExample.Application.Infrastructure;

namespace DDDPPP.Chap19.RavenDBExample.Application.Application.Queries
{
    public class AuctionSummaryQuery
    {
        private readonly IAuctions _auctions;
        private IBidHistory _bidHistory;

        public AuctionSummaryQuery(IAuctions auctions, IBidHistory bidHistory)
        {
            _auctions = auctions;
            _bidHistory = bidHistory;
        }

        public AuctionStatus AuctionStatus(Guid auctionId)
        {
            var auction = _auctions.FindBy(auctionId);

            var status = new AuctionStatus();

            status.AuctionEnds = auction.EndsAt;
            status.Id = auction.Id;

            if (auction.HasACurrentBid())
            {
                status.CurrentPrice = auction.CurrentWinningBid.CurrentAuctionPrice.Amount.Value;
                status.WinningBidderId = auction.CurrentWinningBid.Bidder;
            }
                       
            status.NumberOfBids = _bidHistory.NoOfBidsFor(auctionId);

            return status;
        }
    }
}
