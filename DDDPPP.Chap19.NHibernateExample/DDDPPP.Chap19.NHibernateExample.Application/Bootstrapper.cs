using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using DDDPPP.Chap19.NHibernateExample.Application.Infrastructure;
using DDDPPP.Chap19.NHibernateExample.Application.Model.Auction;
using DDDPPP.Chap19.NHibernateExample.Application.Model.BidHistory;
using NHibernate;
using NHibernate.Cfg;

namespace DDDPPP.Chap19.NHibernateExample.Application
{
    public static class Bootstrapper
    {
        public static void Startup()
        {
            Configuration config = new Configuration();
           
            //log4net.Config.XmlConfigurator.Configure();

            config.Configure();
            config.AddAssembly("DDDPPP.Chap19.NHibernateExample.Application");

            var sessionFactory = config.BuildSessionFactory();

            ObjectFactory.Initialize(structureMapConfig =>
            {
                structureMapConfig.For<IAuctions>().Use<Auctions>();
                structureMapConfig.For<IBidHistory>().Use<Infrastructure.BidHistory>();
                structureMapConfig.For<IClock>().Use<SystemClock>();

                structureMapConfig.For<ISessionFactory>().Use(sessionFactory);
                structureMapConfig.For<ISession>()
                    .HybridHttpOrThreadLocalScoped()
                    .Use(x =>
                    {
                        var factory = x.GetInstance<ISessionFactory>();
                        return factory.OpenSession();
                    });
            });

        }
    }
}
