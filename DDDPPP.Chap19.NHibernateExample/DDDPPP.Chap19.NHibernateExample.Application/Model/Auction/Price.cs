using System;
using System.Collections.Generic;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;
using NHibernate.Mapping;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.Auction
{
    public class Price : ValueObject<Price>
    {
        private Price()
        { }

        public Price(Money amount)
        {
            if (amount == null)
                throw new ArgumentNullException("Amount cannot be null");

            Amount = amount;
        }

        public Money Amount { get; private set; }

        public Money BidIncrement()
        {
            // Enter £1.04 or more
            return Amount.add(new Money(0.5m));
            //£0.01 - £0.99 -> £0.05
            //£1.00 - £4.99 -> £0.20
            //£5.00 - £14.99 -> £0.50
            //£15.00 - £59.99 -> £1.00
            //£60.00 - £149.99 -> £2.00
            //£150.00 -  £299.99 -> £5.00
            //£300.00 - £599.99 -> £10.00
            //£600.00 - £1,499.99 -> £20.00
            //£1,500.00 - £2,999.99 -> £50.00
            //£3,000.00 and up -> £100.00
        }

        public bool CanBeExceededBy(Money offer)
        {
            return offer.IsGreaterThanOrEqualTo(BidIncrement());
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>() {Amount};
        }
    }
}
