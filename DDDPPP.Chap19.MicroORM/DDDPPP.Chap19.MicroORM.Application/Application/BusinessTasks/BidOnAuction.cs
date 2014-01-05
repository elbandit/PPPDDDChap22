using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.MicroORM.Application.Model.Auction;
using DDDPPP.Chap19.MicroORM.Application.Model.BidHistory;
using DDDPPP.Chap19.MicroORM.Application.Infrastructure;

namespace DDDPPP.Chap19.MicroORM.Application.Application.BusinessTasks
{
    public class BidOnAuction
    {
        private IAuctions _auctions;
        private IBidHistory _bidHistory;
        private IUnitOfWork _unitOfWork;
        private IClock _clock;

        public BidOnAuction(IAuctions auctions, IBidHistory bidHistory, IUnitOfWork unitOfWork, IClock clock)
        {
            _auctions = auctions;
            _bidHistory = bidHistory;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        public void Bid(Guid auctionId, Guid memberId, decimal amount)
        {
            try
            {                 
                using (DomainEvents.Register(BidPlaced()))
                {
                    var auction = _auctions.FindBy(auctionId);

                    var bidAmount = new Money(amount);

                    auction.PlaceBidFor(new Offer(memberId, bidAmount, _clock.Time()), _clock.Time());

                    _auctions.Save(auction);
                }

                _unitOfWork.Commit();
            }
            catch (ConcurrencyException ex)
            {
                // What happens if the auction is changed after we retrieve it and before we save it?               
                // try again with the updated auction
                _unitOfWork.Clear();

                Bid(auctionId, memberId, amount);
            }            
        }

        private Action<BidPlaced> BidPlaced()
        {
            return (BidPlaced e) =>
            {
                var bidEvent = new BidEvent(e.AuctionId, e.Bidder, e.AmountBid, e.TimeOfBid);
              
                _bidHistory.Add(bidEvent);
            };
        }
    }
}
