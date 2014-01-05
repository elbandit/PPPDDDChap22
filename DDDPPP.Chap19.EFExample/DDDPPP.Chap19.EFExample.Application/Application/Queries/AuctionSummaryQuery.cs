using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.EFExample.Application.Model.Auction;
using DDDPPP.Chap19.EFExample.Application.Model.BidHistory;
using DDDPPP.Chap19.EFExample.Application.Infrastructure;

namespace DDDPPP.Chap19.EFExample.Application.Application.Queries
{
    public class AuctionSummaryQuery
    {
        private readonly IAuctions _auctions;
        private readonly IBidHistory _bidHistory;

        public AuctionSummaryQuery(IAuctions auctions, IBidHistory bidHistory)
        {
            _auctions = auctions;
            _bidHistory = bidHistory;
        }

        public AuctionStatus AuctionStatus(Guid auctionId)
        {            
            var auction = _auctions.FindBy(auctionId);

            var snapshot = auction.GetSnapShot();

            return ConvertToStatus(snapshot);
        }

        public AuctionStatus ConvertToStatus(AuctionSnapShot snapShot)
        {
            var status = new AuctionStatus();

            status.AuctionEnds = snapShot.EndsAt;            
            status.Id = snapShot.Id;

            if (snapShot.CurrentBid != null)
            {
                status.NumberOfBids = _bidHistory.NoOfBidsFor(snapShot.Id);
                status.WinningBidderId = snapShot.CurrentBid.BiddersId;
                status.CurrentPrice = snapShot.CurrentBid.CurrentPrice;
            }
            
            return status;
        }
    }
}
