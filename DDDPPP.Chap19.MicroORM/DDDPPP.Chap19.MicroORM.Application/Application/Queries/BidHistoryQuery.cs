using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.MicroORM.Application.Infrastructure;
using DDDPPP.Chap19.MicroORM.Application.Model.BidHistory;

namespace DDDPPP.Chap19.MicroORM.Application.Application.Queries
{
    public class BidHistoryQuery
    {
        private readonly IBidHistory _bidHistory;

        public BidHistoryQuery(IBidHistory bidHistory)
        {
            _bidHistory = bidHistory;         
        }

        public IEnumerable<BidInformation> BidHistoryFor(Guid auctionId)
        {
            var bidHistory = _bidHistory.FindBy(auctionId);

            return Convert(bidHistory.ShowAllBids());
        }

        public IEnumerable<BidInformation> Convert(IEnumerable<BidEvent> bids)
        {
            var bidInfo = new List<BidInformation>();

            foreach (var bid in bids)
            {
                bidInfo.Add(new BidInformation() { Bidder = bid.Bidder, AmountBid = bid.AmountBid.GetSnapshot().Value, TimeOfBid = bid.TimeOfBid });
            }

            return bidInfo;
        }
    }
}
