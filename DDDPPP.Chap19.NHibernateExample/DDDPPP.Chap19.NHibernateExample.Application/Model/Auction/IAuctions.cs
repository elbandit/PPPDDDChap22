using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDPPP.Chap19.NHibernateExample.Application.Application;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.Auction
{
    public interface IAuctions
    {
        void Add(Auction auction);
        Auction FindBy(Guid Id);
    }
}
