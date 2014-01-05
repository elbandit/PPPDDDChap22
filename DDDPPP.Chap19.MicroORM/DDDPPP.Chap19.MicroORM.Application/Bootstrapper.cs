using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using DDDPPP.Chap19.MicroORM.Application.Infrastructure;
using DDDPPP.Chap19.MicroORM.Application.Model.Auction;
using DDDPPP.Chap19.MicroORM.Application.Model.BidHistory;

namespace DDDPPP.Chap19.MicroORM.Application
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

                config.For<IUnitOfWork>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use(x =>
                    {
                        var uow = new UnitOfWork();
                        return uow;
                    });              
            });

        }
    }
}
