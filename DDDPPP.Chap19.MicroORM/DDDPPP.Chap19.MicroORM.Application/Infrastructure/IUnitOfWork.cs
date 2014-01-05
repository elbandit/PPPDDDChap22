using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDPPP.Chap19.MicroORM.Application.Infrastructure
{
    public interface IUnitOfWork
    {
        void RegisterAmended(IAggregateDataModel entity, IUnitOfWorkRepository unitofWorkRepository);
        void RegisterNew(IAggregateDataModel entity, IUnitOfWorkRepository unitofWorkRepository);
        void Commit();
        void Clear();
    }
}
