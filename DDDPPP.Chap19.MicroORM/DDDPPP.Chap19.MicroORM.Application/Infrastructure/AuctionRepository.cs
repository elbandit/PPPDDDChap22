using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.MicroORM.Application.Model.Auction;
using DDDPPP.Chap19.MicroORM.Application.Application;
using DDDPPP.Chap19.MicroORM.Application.Infrastructure.DataModel;
using System.Data.SqlClient;
using Dapper;

namespace DDDPPP.Chap19.MicroORM.Application.Infrastructure
{
    public class AuctionRepository : IAuctionRepository, IUnitOfWorkRepository
    {
        private IUnitOfWork _unitOfWork;

        public AuctionRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            _unitOfWork.RegisterNew(auctionDTO, this);
        }

        public void Save(Auction auction)
        {
            var snapShot = auction.GetSnapShot();

            AuctionDTO auctionDTO;

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AuctionDB"].ConnectionString))
            {
                auctionDTO = connection.Query<AuctionDTO>("Select * From Auctions Where Id = @Id", new { Id = snapShot.Id }).FirstOrDefault();
            }
                        
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

            _unitOfWork.RegisterAmended(auctionDTO, this);
        }

        public Auction FindBy(Guid Id)
        {
            AuctionDTO auctionDTO;

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AuctionDB"].ConnectionString))
            {
                auctionDTO = connection.Query<AuctionDTO>("Select * From Auctions Where Id = CAST(@Id AS uniqueidentifier)", new { Id = Id }).FirstOrDefault();
            }

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

        public void PersistCreationOf(IAggregateDataModel entity)
        {
            var auctionDTO = (AuctionDTO)entity;

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AuctionDB"].ConnectionString))
            {
                var recordsAdded = connection.Execute(@"                
                    INSERT INTO [AuctionExample].[dbo].[Auctions]
                           ([Id]
                           ,[StartingPrice]
                           ,[BidderMemberId]
                           ,[TimeOfBid]
                           ,[MaximumBid]
                           ,[CurrentPrice]
                           ,[AuctionEnds]
                           ,[Version])
                     VALUES
                           (@Id, @StartingPrice, @BidderMemberId, @TimeOfBid, @MaximumBid, @CurrentPrice, @AuctionEnds, @Version)"
                    , new { Id = auctionDTO.Id, StartingPrice = auctionDTO.StartingPrice, BidderMemberId = auctionDTO.BidderMemberId, 
                            TimeOfBid = auctionDTO.TimeOfBid, MaximumBid = auctionDTO.MaximumBid, CurrentPrice = auctionDTO.CurrentPrice,
                            AuctionEnds = auctionDTO.AuctionEnds, Version = auctionDTO.Version });
            }
        }

        public void PersistUpdateOf(IAggregateDataModel  entity)
        {
            var auctionDTO = (AuctionDTO)entity;

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AuctionDB"].ConnectionString))
            {
                var recordsUpdated = connection.Execute(@"                
                    UPDATE 
                        [AuctionExample].[dbo].[Auctions]
                    SET 
                        [Id] = @Id
                       ,[StartingPrice] = @StartingPrice
                       ,[BidderMemberId] = @BidderMemberId
                       ,[TimeOfBid] = @TimeOfBid
                       ,[MaximumBid] = @MaximumBid
                       ,[CurrentPrice] = @CurrentPrice
                       ,[AuctionEnds] = @AuctionEnds
                       ,[Version] = @Version
                    WHERE
                        Id = @Id AND Version = @PreviousVersion"
                    , new
                    {
                        Id = auctionDTO.Id,
                        StartingPrice = auctionDTO.StartingPrice,
                        BidderMemberId = auctionDTO.BidderMemberId,
                        TimeOfBid = auctionDTO.TimeOfBid,
                        MaximumBid = auctionDTO.MaximumBid,
                        CurrentPrice = auctionDTO.CurrentPrice,
                        AuctionEnds = auctionDTO.AuctionEnds,
                        Version = auctionDTO.Version + 1,
                        PreviousVersion = auctionDTO.Version
                    });

                if (!recordsUpdated.Equals(1))
                {
                    throw new ConcurrencyException();
                }  
            }
        }
    }
}
