using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.SharedKernel.Interfaces.Models
{
    public interface IEntity<Tkey>
    {
         Tkey Id { get; set; }
    }
}
