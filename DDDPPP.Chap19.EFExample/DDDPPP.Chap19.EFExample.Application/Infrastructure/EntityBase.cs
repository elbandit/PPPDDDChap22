using System;

namespace DDDPPP.Chap19.EFExample.Application.Infrastructure
{
    public abstract class EntityBase
    {
        public int Version { get; private set; }
    }
}
