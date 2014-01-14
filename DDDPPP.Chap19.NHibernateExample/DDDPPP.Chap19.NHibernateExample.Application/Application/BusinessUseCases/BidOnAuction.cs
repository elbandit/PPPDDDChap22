using System;
using System.Collections.Generic;
using DDDPPP.Chap19.NHibernateExample.Application.Model.Auction;
using DDDPPP.Chap19.NHibernateExample.Application.Model.BidHistory;
using NHibernate;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;

namespace DDDPPP.Chap19.NHibernateExample.Application.Application.BusinessUseCases
{
    public class BidOnAuction
    {
        private IAuctionRepository _auctions;
        private IBidHistoryRepository _bidHistory;
        private ISession _unitOfWork;
        private IClock _clock;

        public BidOnAuction(IAuctionRepository auctions, IBidHistoryRepository bidHistory, ISession unitOfWork, IClock clock)
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
                using (ITransaction transaction = _unitOfWork.BeginTransaction())
                {
                    using (DomainEvents.Register(OutBid()))
                    using (DomainEvents.Register(BidPlaced()))
                    {
                        var auction = _auctions.FindBy(auctionId);

                        var bidAmount = new Money(amount);

                        auction.PlaceBidFor(new Offer(memberId, bidAmount, _clock.Time()), _clock.Time());
                    }

                    transaction.Commit();
                }
            }
            catch (StaleObjectStateException ex)
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
                var bidEvent = new Bid(e.AuctionId, e.Bidder, e.AmountBid, e.TimeOfMemberBid);
              
                _bidHistory.Add(bidEvent);
            };
        }

        private Action<OutBid> OutBid()
        {
            return (OutBid e) => 
            { 
                // Email customer to say that he has been out bid                
            };
        }
    }
}
