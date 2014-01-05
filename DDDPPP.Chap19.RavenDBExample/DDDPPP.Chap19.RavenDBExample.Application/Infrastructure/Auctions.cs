using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.RavenDBExample.Application.Model.Auction;
using Raven.Client;
using DDDPPP.Chap19.RavenDBExample.Application.Application.Queries;

namespace DDDPPP.Chap19.RavenDBExample.Application.Infrastructure
{
    public class Auctions : IAuctions
    {
        private readonly IDocumentSession _documentSession;

        public Auctions(IDocumentSession documentSession)
        { 
            _documentSession = documentSession;
        }

        public void Add(Auction auction)
        {
            _documentSession.Store(auction); 
        }

        public Auction FindBy(Guid Id)
        {
            return _documentSession.Load<Auction>("Auctions/" + Id);
        }
    }
}
