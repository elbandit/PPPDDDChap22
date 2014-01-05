using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.RavenDBExample.Application.Model.BidHistory
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
            var bids = _bids.OrderByDescending(x => x.AmountBid.Value).ThenBy(x => x.TimeOfBid);

            return bids;
        }
    }
}
