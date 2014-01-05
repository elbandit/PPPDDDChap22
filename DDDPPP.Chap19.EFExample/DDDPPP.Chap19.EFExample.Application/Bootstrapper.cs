using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using DDDPPP.Chap19.EFExample.Application.Infrastructure;
using DDDPPP.Chap19.EFExample.Application.Model.Auction;
using DDDPPP.Chap19.EFExample.Application.Model.BidHistory;

namespace DDDPPP.Chap19.EFExample.Application
{
    public static class Bootstrapper
    {
        public static void Startup()
        {                      
            ObjectFactory.Initialize(config =>
            {
                config.For<IAuctions>().Use<Auctions>();
                config.For<IBidHistory>().Use<BidEventHistory>();
                config.For<IClock>().Use<SystemClock>();

                //config.For<AuctionExampleContext>().Use<AuctionExampleContext>();
                //config.For<IDocumentSession>()
                //    .HybridHttpOrThreadLocalScoped()
                //    .Use(x =>
                //    {
                //        var store = x.GetInstance<IDocumentStore>();
                //        return store.OpenSession();
                //    });              
            });

        }
    }
}
