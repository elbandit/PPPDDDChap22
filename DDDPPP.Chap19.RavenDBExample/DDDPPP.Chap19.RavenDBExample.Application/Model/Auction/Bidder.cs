using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.RavenDBExample.Application.Model.Auction
{
    public class Bidder
    {
        public Bidder(Guid memberId, Money maximumBid, DateTime timeOfBid)
        {
            MemberId = memberId;
            MaximumBid = maximumBid;
            TimeOfBid = timeOfBid;
        }

        public Guid MemberId { get; private set; }
        public Money MaximumBid { get; private set; }
        public DateTime TimeOfBid{get; private set;}

        public Bidder RaiseMaximumBidTo(Money newAmount)
        {
            // Check that newAmount is greater than old max amount
            return new Bidder(this.MemberId, newAmount, DateTime.Now);
        }
    }
}
