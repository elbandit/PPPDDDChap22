using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.MicroORM.Application.Model.BidHistory;
using DDDPPP.Chap19.MicroORM.Application.Model.Auction;
using DDDPPP.Chap19.MicroORM.Application.Infrastructure.DataModel;
using Dapper;
using System.Data.SqlClient;

namespace DDDPPP.Chap19.MicroORM.Application.Infrastructure
{
    public class BidEventHistory : IBidHistory, IUnitOfWorkRepository
    {
        private IUnitOfWork _unitOfWork;

        public BidEventHistory(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int NoOfBidsFor(Guid auctionId)
        {            
            // Sometimes the item page will show that there are 2 bids, yet there is only one bidder. 
            // This happens when a member places more then one bid to increase their maximum bid amount. 
            // For example, if you are the first bidder on an item and you place a second bid to increase your maximum bid amount, the item page would show the current high bid at the opening bid amount, but would show that two bids have been placed on this item.
            int count;

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AuctionDB"].ConnectionString))
            {
                var count1 = connection.Query<int>("Select Count(*) From BidHistory Where AuctionId = @Id", new { Id = auctionId }).FirstOrDefault();

                count = count1 != null ? count1 : 1;
            }
                                     
            return count;
        }

        public void Add(BidEvent bid)        
        {
            var bidHistoryDTO = new BidHistoryDTO();

            bidHistoryDTO.AuctionId = bid.AuctionId;
            bidHistoryDTO.Bid = bid.AmountBid.GetSnapshot().Value;
            bidHistoryDTO.BidderId = bid.Bidder;
            bidHistoryDTO.TimeOfBid = bid.TimeOfBid;

            bidHistoryDTO.Id = Guid.NewGuid();

            _unitOfWork.RegisterNew(bidHistoryDTO, this);
        }

        public BidHistory FindBy(Guid auctionId)
        {
            IEnumerable<BidHistoryDTO> bids;

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AuctionDB"].ConnectionString))
            {
                bids = connection.Query<BidHistoryDTO>("Select * From BidHistory Where AuctionId = @Id", new { Id = auctionId });
            }
  
            var bidds = new List<BidEvent>();

            foreach (var bid in bids)
            { 
                bidds.Add(new BidEvent(bid.AuctionId, bid.BidderId, new Money(bid.Bid), bid.TimeOfBid));
            }

            return new BidHistory(bidds);
        }


        public void PersistCreationOf(IAggregateDataModel entity)
        {
            var bidHistoryDTO = (BidHistoryDTO)entity;

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AuctionDB"].ConnectionString))
            {
                var recordsUpdated = connection.Execute(@"                
                    INSERT INTO [dbo].[BidHistory]
                           ([AuctionId]
                           ,[BidderId]
                           ,[Bid]
                           ,[TimeOfBid]
                           ,[Id])
                     VALUES
                           (@AuctionId
                           ,@BidderId
                           ,@Bid
                           ,@TimeOfBid
                           ,@Id)"
                    , new
                    {
                        Id = bidHistoryDTO.Id,
                        BidderId = bidHistoryDTO.BidderId,
                        Bid = bidHistoryDTO.Bid,
                        TimeOfBid = bidHistoryDTO.TimeOfBid,
                        AuctionId = bidHistoryDTO.AuctionId 
                    });

                if (recordsUpdated.Equals(1))
                {
                    // 1 rows inserted or throw concurrency exception
                }               
            }
        }

        public void PersistUpdateOf(IAggregateDataModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
