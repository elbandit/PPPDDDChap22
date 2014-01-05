using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.Auction
{
    public class Offer
    {
        public Offer(Guid bidderId, Money maximumBid, DateTime timeOfOffer)
        {
            Bidder = bidderId;
            MaximumBid = maximumBid;
            TimeOfOffer = timeOfOffer;
        }

        public Guid Bidder { get; private set; }
        public Money MaximumBid { get; private set; }
        public DateTime TimeOfOffer { get; private set; }
    }
}
