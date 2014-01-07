using System;

namespace DDDPPP.Chap19.NHibernateExample.Application.Model.Auction
{
    public interface IAuctions
    {
        void Add(Auction auction);
        Auction FindBy(Guid Id);
    }
}
