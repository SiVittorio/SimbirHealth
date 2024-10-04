using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimbirHealth.Data.Models._Base
{
    public interface IDeleteable
    {
        bool IsDeleted { get; }
    }
}
