using System;

namespace DDDPPP.Chap19.NHibernateExample.Application.Infrastructure
{
    public abstract class EntityBase
    {
        public int Version { get; private set; }
    }
}
