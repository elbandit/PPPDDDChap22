﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.NHibernateExample.Application.Model.BidHistory;
using NHibernate;

namespace DDDPPP.Chap19.NHibernateExample.Application.Infrastructure
{
    public class BidHistory : IBidHistory
    {
        private readonly ISession _session;

        public BidHistory(ISession session)
        {
            _session = session;
        }

        public int NoOfBidsFor(Guid autionId)
        {
            var sql = String.Format("SELECT Count(*) FROM BidHistory Where AuctionId = '{0}'", autionId);
            var query = _session.CreateSQLQuery(sql);
            var result = query.UniqueResult();
           
            return Convert.ToInt32(result);                                                
        }

        public void Add(BidEvent bid)
        {                       
            _session.Save(bid);
        }
    }
}
