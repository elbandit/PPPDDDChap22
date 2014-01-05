using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.BidHistory
{
    public interface IBidHistory
    {
        int NoOfBidsFor(Guid autionId);
        void Add(BidEvent bid);
    }
}
