using System;
using System.Collections.Generic;
using DDDPPP.Chap19.EFExample.Application.Model.Auction;
using DDDPPP.Chap19.EFExample.Application.Model.BidHistory;
using DDDPPP.Chap19.EFExample.Application.Infrastructure;

namespace DDDPPP.Chap19.EFExample.Application.Application.Queries
{
    public class AuctionStatusQuery
    {
        private readonly IAuctionRepository _auctions;
        private readonly IBidHistoryRepository _bidHistory;
        private readonly IClock _clock;

        public AuctionStatusQuery(IAuctionRepository auctions, 
                                  IBidHistoryRepository bidHistory,
                                  IClock clock)
        {
            _auctions = auctions;
            _bidHistory = bidHistory;
            _clock = clock;
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
            status.TimeRemaining = TimeRemaining(snapShot.EndsAt);

            if (snapShot.CurrentBid != null)
            {
                status.NumberOfBids = _bidHistory.NoOfBidsFor(snapShot.Id);
                status.WinningBidderId = snapShot.CurrentBid.BiddersId;
                status.CurrentPrice = snapShot.CurrentBid.CurrentPrice;
            }
            
            return status;
        }

        public TimeSpan TimeRemaining(DateTime AuctionEnds)
        {
            if (_clock.Time() < AuctionEnds)
                return AuctionEnds.Subtract(_clock.Time());
            else
                return new TimeSpan();
        }
    }
}
