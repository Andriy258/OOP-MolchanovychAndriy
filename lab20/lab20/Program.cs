namespace lab20;

class Program
{
    static void Main()
    {
       IOrderValidator validator = new OrderValidator();
        IOrderRepository repository = new InMemoryOrderRepository();
        IEmailService emailService = new ConsoleEmailService();

        OrderService service = new OrderService(
            validator,
            repository,
            emailService
        );

        Order validOrder = new Order(1, "Андрій", 1000);
        service.ProcessOrder(validOrder);

        Order invalidOrder = new Order(2, "Іван", -50);
        service.ProcessOrder(invalidOrder);
    }
}

