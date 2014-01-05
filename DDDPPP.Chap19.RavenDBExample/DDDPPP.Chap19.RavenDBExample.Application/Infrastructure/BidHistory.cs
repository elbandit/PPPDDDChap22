using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.RavenDBExample.Application.Model.BidHistory;
using Raven.Client;

namespace DDDPPP.Chap19.RavenDBExample.Application.Infrastructure
{
    public class BidHistory : IBidHistory
    {
        private readonly IDocumentSession _documentSession;

        public BidHistory(IDocumentSession documentSession)
        { 
            _documentSession = documentSession;
        }

        public int NoOfBidsFor(Guid autionId)
        {            
            // Sometimes the item page will show that there are 2 bids, yet there is only one bidder. This happens when a member places more then one bid to increase their maximum bid amount. For example, if you are the first bidder on an item and you place a second bid to increase your maximum bid amount, the item page would show the current high bid at the opening bid amount, but would show that two bids have been placed on this item.

            var count = _documentSession.Query<BidHistory_NumberOfBids.ReduceResult, BidHistory_NumberOfBids>()
                            .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                            .FirstOrDefault(x => x.AuctionId == autionId)
                                ?? new BidHistory_NumberOfBids.ReduceResult();

            return count.Count;                                           
        }

        public void Add(BidEvent bid)
        {
            _documentSession.Store(bid); 
        }

        public Model.BidHistory.BidHistory FindBy(Guid auctionId)
        {
            var bids = _documentSession.Query<BidEvent>()
                                .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                .Where(x => x.AuctionId == auctionId)                                
                                .ToList();

            return new Model.BidHistory.BidHistory(bids);
        }
    }
}
