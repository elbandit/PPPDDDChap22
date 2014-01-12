using System;

namespace DDDPPP.Chap19.MicroORM.Application.Model.BidHistory
{
    public class BidHistory
    {
        private IEnumerable<BidEvent> _bids;

        public BidHistory(IEnumerable<BidEvent> bids)
        {
            _bids = bids;
        }

        public IEnumerable<BidEvent> ShowAllBids()
        {
            var bids = _bids.OrderByDescending(x => x.AmountBid.GetSnapshot().Value).ThenBy(x => x.TimeOfBid);

            return bids;
        }
    }
}
