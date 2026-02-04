using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab20
{
   public interface IOrderRepository
{
    void Save(Order order);
    Order GetById(int id);
}
}