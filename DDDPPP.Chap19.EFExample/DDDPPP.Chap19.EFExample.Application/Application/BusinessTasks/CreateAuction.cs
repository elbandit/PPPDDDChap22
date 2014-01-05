using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.EFExample.Application.Model.Auction;
using DDDPPP.Chap19.EFExample.Application.Model.BidHistory;
using DDDPPP.Chap19.EFExample.Application.Infrastructure;

namespace DDDPPP.Chap19.EFExample.Application.Application.BusinessTasks
{
    public class CreateAuction
    {
        private IAuctions _auctions;
        private AuctionExampleContext _unitOfWork;

        public CreateAuction(IAuctions auctions, AuctionExampleContext unitOfWork)
        {
            _auctions = auctions;            
            _unitOfWork = unitOfWork;
        }

        public Guid Create(AuctionCreation command)
        {
            var auctionId = Guid.NewGuid();
            var startingPrice = new Money(command.StartingPrice);
           
            _auctions.Add(new Auction(auctionId, startingPrice, command.EndsAt));

            _unitOfWork.SaveChanges();
            
            return auctionId;
        }
    }
}
