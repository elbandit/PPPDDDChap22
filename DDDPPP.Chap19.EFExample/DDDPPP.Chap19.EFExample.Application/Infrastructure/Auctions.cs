using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.EFExample.Application.Model.Auction;
using DDDPPP.Chap19.EFExample.Application.Application;
using DDDPPP.Chap19.EFExample.Application.Infrastructure.DataModel;

namespace DDDPPP.Chap19.EFExample.Application.Infrastructure
{
    public class Auctions : IAuctions
    {
        private readonly AuctionExampleContext _auctionExampleContext;

        public Auctions(AuctionExampleContext auctionExampleContext)
        {
            _auctionExampleContext = auctionExampleContext;
        }

        public void Add(Auction auction)
        {
            var snapShot = auction.GetSnapShot();
            var auctionDTO = new AuctionDTO();

            // map to auctionDTO
            auctionDTO.Id = snapShot.Id;
            auctionDTO.StartingPrice = snapShot.StartingPrice;
            auctionDTO.AuctionEnds = snapShot.EndsAt;
            auctionDTO.Version = 1;

            if (snapShot.CurrentBid != null)
            {
                auctionDTO.BidderMemberId = snapShot.CurrentBid.BiddersId;
                auctionDTO.CurrentPrice = snapShot.CurrentBid.CurrentPrice;
                auctionDTO.MaximumBid = snapShot.CurrentBid.BiddersMaximumBid;
                auctionDTO.TimeOfBid = snapShot.CurrentBid.TimeOfBid;
            }

            _auctionExampleContext.Auctions.Add(auctionDTO); 
        }

        public void Save(Auction auction)
        {
            var snapShot = auction.GetSnapShot();
            var auctionDTO = _auctionExampleContext.Auctions.Find(snapShot.Id);
                        
            // map to auctionDTO
            auctionDTO.Id = snapShot.Id;
            auctionDTO.StartingPrice = snapShot.StartingPrice;
            auctionDTO.AuctionEnds = snapShot.EndsAt;

            if (snapShot.CurrentBid != null)
            {
                auctionDTO.BidderMemberId = snapShot.CurrentBid.BiddersId;
                auctionDTO.CurrentPrice = snapShot.CurrentBid.CurrentPrice;
                auctionDTO.MaximumBid = snapShot.CurrentBid.BiddersMaximumBid;
                auctionDTO.TimeOfBid = snapShot.CurrentBid.TimeOfBid;
            }             
        }

        public Auction FindBy(Guid Id)
        {
            var auctionDTO = _auctionExampleContext.Auctions.Find(Id);
            var auctionSnapShot = new AuctionSnapShot();

            auctionSnapShot.Id = auctionDTO.Id;
            auctionSnapShot.EndsAt = auctionDTO.AuctionEnds;
            auctionSnapShot.StartingPrice = auctionDTO.StartingPrice;

            if (auctionDTO.BidderMemberId.HasValue)
            {
                var bidSnapShot = new BidSnapShot();

                bidSnapShot.BiddersMaximumBid = auctionDTO.MaximumBid.Value;
                bidSnapShot.CurrentPrice = auctionDTO.CurrentPrice.Value;
                bidSnapShot.BiddersId = auctionDTO.BidderMemberId.Value;
                bidSnapShot.TimeOfBid = auctionDTO.TimeOfBid.Value;
                auctionSnapShot.CurrentBid = bidSnapShot;
            }
           
            return Auction.CreateFrom(auctionSnapShot);
        }
    }
}
