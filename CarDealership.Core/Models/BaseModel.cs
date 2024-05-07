using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CarDealership.Core.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; }

        public BaseModel(Guid id)
        {
            Id = id;
        }
    }
}
