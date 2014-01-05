using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDPPP.Chap19.MicroORM.Application.Infrastructure
{
    public interface IUnitOfWorkRepository
    {
        void PersistCreationOf(IAggregateDataModel entity);
        void PersistUpdateOf(IAggregateDataModel entity);
    }
}
