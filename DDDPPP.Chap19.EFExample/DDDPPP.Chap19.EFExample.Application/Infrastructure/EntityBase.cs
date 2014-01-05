using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.EFExample.Application.Infrastructure
{
    public abstract class EntityBase
    {
        public int Version { get; private set; }
    }
}
