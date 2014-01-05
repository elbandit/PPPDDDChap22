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
    public class CreateAuction
    {
        private IAuctions _auctions;
        private IUnitOfWork _unitOfWork;

        public CreateAuction(IAuctions auctions, IUnitOfWork unitOfWork)
        {
            _auctions = auctions;            
            _unitOfWork = unitOfWork;
        }

        public Guid Create(AuctionCreation command)
        {
            var auctionId = Guid.NewGuid();
            var startingPrice = new Money(command.StartingPrice);
           
            _auctions.Add(new Auction(auctionId, startingPrice, command.EndsAt));

            _unitOfWork.Commit();
            
            return auctionId;
        }
    }
}
