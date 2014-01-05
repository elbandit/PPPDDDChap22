using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.NHibernateExample.Application.Model.Auction;
using DDDPPP.Chap19.NHibernateExample.Application.Model.BidHistory;
using NHibernate;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;

namespace DDDPPP.Chap19.NHibernateExample.Application.Application.BusinessTasks
{
    public class CreateAuction
    {
        private IAuctions _auctions;
        private ISession _unitOfWork;

        public CreateAuction(IAuctions auctions,ISession unitOfWork)
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
