using System;

namespace DDDPPP.Chap19.NHibernateExample.Application.Infrastructure
{
    public abstract class Entity
    {
        public int Version { get; private set; }
    }
}
