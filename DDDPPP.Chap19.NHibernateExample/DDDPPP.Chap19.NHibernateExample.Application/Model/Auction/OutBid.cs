using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.Auction
{
    public class OutBid
    {
        public OutBid(Guid auctionId, Guid bidderId, Money amountBid)
        {
            AuctionId = auctionId;
            Bidder = bidderId;
            AmountBid = amountBid;
        }

        public Guid AuctionId { get; private set; }
        public Guid Bidder { get; private set; }
        public Money AmountBid { get; private set; }
    }
}
