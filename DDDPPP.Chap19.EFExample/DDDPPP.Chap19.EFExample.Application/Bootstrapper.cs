using System;
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
            });

        }
    }
}
