using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDPPP.Chap19.EFExample.Application.Model.Auction
{
    public interface IClock
    {
        DateTime Time();
    }
}
