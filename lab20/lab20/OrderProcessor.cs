using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab20
{
    public class OrderProcessor
    {
        public void ProcessOrder(Order order)
    {
        if (order.TotalAmount <= 0)
        {
            Console.WriteLine("Невалідне замовлення");
            order.Status = OrderStatus.Cancelled;
            return;
        }

        Console.WriteLine("Замовлення збережено");
        Console.WriteLine("Email відправлено");

        order.Status = OrderStatus.Processed;
    }
    }
}