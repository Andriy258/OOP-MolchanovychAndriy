using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab20;

public class InMemoryOrderRepository : IOrderRepository
{
    private Dictionary<int, Order> orders = new();

    public void Save(Order order)
    {
        orders[order.Id] = order;
        Console.WriteLine("Замовлення збережено в памʼяті");
    }

    public Order? GetById(int id)
    {
        return orders.ContainsKey(id) ? orders[id] : null;
    }
}