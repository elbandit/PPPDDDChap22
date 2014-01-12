﻿using System;
using System.Collections.Generic;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;
using NHibernate.Mapping;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.Auction
{
    public class Money : ValueObject<Money>
    {
        protected decimal Value { get; set; }

        public Money()
            : this(0m)
        {
        }

        public Money(decimal value)
        {
            ThrowExceptionIfNotValid(value);

            Value = value;
        }

        private void ThrowExceptionIfNotValid(decimal value)
        {
            if (value % 0.01m != 0)
                throw new MoreThanTwoDecimalPlacesInMoneyValueException();

            if(value < 0)
                throw new MoneyCannotBeANegativeValueException();
        }

        public Money add(Money money)
        {
            return new Money(Value + money.Value);
        }

        public bool IsGreaterThan(Money money)
        {
            return this.Value > money.Value;
        }

        public bool IsGreaterThanOrEqualTo(Money money)
        {
            return this.Value > money.Value || this.Equals(money);
        }

        public override string ToString()
        {
            return string.Format("{0}", Value);
        }

        // Equality Implementtion

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>() {Value};
        }
    }
}
