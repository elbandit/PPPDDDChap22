using System;
using DDDPPP.Chap19.NHibernateExample.Application.Model.Auction;
using NHibernate;

namespace DDDPPP.Chap19.NHibernateExample.Application.Application.BusinessUseCases
{
    public class CreateAuctionRequest
    {
        private IAuctionRepository _auctions;
        private ISession _unitOfWork;

        public CreateAuctionRequest(IAuctionRepository auctions,ISession unitOfWork)
        {
            _auctions = auctions;            
            _unitOfWork = unitOfWork;
        }

        public Guid Create(AuctionCreation command)
        {
            var auctionId = Guid.NewGuid();
            var startingPrice = new Money(command.StartingPrice);

            using (ITransaction transaction = _unitOfWork.BeginTransaction())
            {
                _auctions.Add(new Auction(auctionId, startingPrice, command.EndsAt));

                transaction.Commit();
            }

            return auctionId;
        }
    }
}
