using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.RavenDBExample.Application.Model.BidHistory
{
    public class BidHistory
    {
        private IEnumerable<Bid> _bids;

        public BidHistory(IEnumerable<Bid> bids)
        {
            _bids = bids;
        }

        public IEnumerable<Bid> ShowAllBids()
        {
            var bids = _bids.OrderByDescending(x => x.AmountBid.Value).ThenBy(x => x.TimeOfBid);

            return bids;
        }
    }
}
