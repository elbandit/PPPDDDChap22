using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.RavenDBExample.Application.Model.Auction
{
    public class Money
    {
        public decimal Value { get; private set; }
        // TODO: If we need to change currency we can do nice and easily here

        public Money()
            : this(0m)
        {
        }

        public Money(decimal value)
        {
            Value = value;
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

        public override bool Equals(System.Object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;

            return ((Money)obj).Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("£{0}", Value);
        }
    }
}
