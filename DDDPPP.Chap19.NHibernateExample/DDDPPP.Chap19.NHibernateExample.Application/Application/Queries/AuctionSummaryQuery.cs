using System;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using DDDPPP.Chap19.NHibernateExample.Application.Model.BidHistory;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;

namespace DDDPPP.Chap19.NHibernateExample.Application.Application.Queries
{
    public class AuctionSummaryQuery
    {
        private readonly ISession _session;
        private IBidHistory _bidHistory;

        public AuctionSummaryQuery(ISession session, IBidHistory bidHistory)
        {
            _session = session;
            _bidHistory = bidHistory;
        }

        public AuctionStatus AuctionStatus(Guid auctionId)
        {
            var status = _session
                       .CreateSQLQuery(String.Format("select Id, CurrentPrice, BidderMemberId as WinningBidderId, AuctionEnds from Auctions Where Id = '{0}'", auctionId))
                       .SetResultTransformer(Transformers.AliasToBean<AuctionStatus>())
                       .UniqueResult<AuctionStatus>();
           
            status.NumberOfBids = _bidHistory.NoOfBidsFor(auctionId);

            return status;
        }
    }
}
