using System;
using System.Collections.Generic;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;
using NHibernate.Mapping;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.Auction
{
    public class Offer : ValueObject<Offer>
    {
        public Offer(Guid bidderId, Money maximumBid, DateTime timeOfOffer)
        {
            if (bidderId == null)

            Bidder = bidderId;
            MaximumBid = maximumBid;
            TimeOfOffer = timeOfOffer;
        }

        public Guid Bidder { get; private set; }
        public Money MaximumBid { get; private set; }
        public DateTime TimeOfOffer { get; private set; }

        // Equality overrides
       
        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>()
            {
                Bidder, MaximumBid, TimeOfOffer
            };
        }
    }
    
}
