using System;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.BidHistory
{
    public interface IBidHistory
    {
        int NoOfBidsFor(Guid autionId);
        void Add(BidEvent bid);
    }
}
