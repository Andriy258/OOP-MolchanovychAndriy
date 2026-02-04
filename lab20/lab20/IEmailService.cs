using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab20
{
    public interface IEmailService
{
    void SendOrderConfirmation(Order order);
}
}