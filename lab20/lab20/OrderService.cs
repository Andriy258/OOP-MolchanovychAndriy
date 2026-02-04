using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace lab20;

public class OrderService
{
    private readonly IOrderValidator validator;
    private readonly IOrderRepository repository;
    private readonly IEmailService emailService;

    public OrderService(
        IOrderValidator validator,
        IOrderRepository repository,
        IEmailService emailService)
    {
        this.validator = validator;
        this.repository = repository;
        this.emailService = emailService;
    }

    public void ProcessOrder(Order order)
    {
        if (!validator.IsValid(order))
        {
            Console.WriteLine("Замовлення невалідне");
            order.Status = OrderStatus.Cancelled;
            return;
        }

        repository.Save(order);
        emailService.SendOrderConfirmation(order);
        order.Status = OrderStatus.Processed;

        Console.WriteLine("Замовлення оброблено");
    }
}