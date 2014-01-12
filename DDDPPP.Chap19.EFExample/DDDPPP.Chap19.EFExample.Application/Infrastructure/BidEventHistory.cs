using System;
using System.Collections.Generic;
using System.Linq;
using DDDPPP.Chap19.EFExample.Application.Model.BidHistory;
using DDDPPP.Chap19.EFExample.Application.Model.Auction;
using DDDPPP.Chap19.EFExample.Application.Infrastructure.DataModel;

namespace DDDPPP.Chap19.EFExample.Application.Infrastructure
{
    public class BidEventHistory : IBidHistory
    {
        private readonly AuctionExampleContext _auctionExampleContext;

        public BidEventHistory(AuctionExampleContext auctionExampleContext)
        {
            _auctionExampleContext = auctionExampleContext;
        }

        public int NoOfBidsFor(Guid autionId)
        {                     
            return _auctionExampleContext.BidHistories.Count(x => x.AuctionId == autionId);                                        
        }

        public void Add(BidEvent bid)
        
        {
            var bidHistoryDTO = new BidHistoryDTO();

            bidHistoryDTO.AuctionId = bid.AuctionId;
            bidHistoryDTO.Bid = bid.AmountBid.GetSnapshot().Value;
            bidHistoryDTO.BidderId = bid.Bidder;
            bidHistoryDTO.TimeOfBid = bid.TimeOfBid;

            bidHistoryDTO.Id = Guid.NewGuid();

            _auctionExampleContext.BidHistories.Add(bidHistoryDTO); 
        }

        public BidHistory FindBy(Guid auctionId)
        {
            var bids = _auctionExampleContext.BidHistories.Where<BidHistoryDTO>(x => x.AuctionId == auctionId).ToList();
            var bidds = new List<BidEvent>();

            foreach (var bid in bids)
            { 
                bidds.Add(new BidEvent(bid.AuctionId, bid.BidderId, new Money(bid.Bid), bid.TimeOfBid));
            }

            return new BidHistory(bidds);
        }
    }
}
