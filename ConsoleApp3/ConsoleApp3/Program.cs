using System;
using System.Collections.Generic;

public interface IOrderObserver
{
    void Update(Order order);
}

public class OrderSystem
{
    private List<IOrderObserver> _observers = new List<IOrderObserver>();
    private Order _currentOrder;

    public void AddObserver(IOrderObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IOrderObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_currentOrder);
        }
    }

    public void CreateOrder(int orderId, string customerName, List<string> items)
    {
        _currentOrder = new Order
        {
            Id = orderId,
            CustomerName = customerName,
            Items = items,
            Status = "Создан"
        };
        Console.WriteLine($"\nНовый заказ #{orderId} создан для {customerName}");
        NotifyObservers();
    }

    public void UpdateOrderStatus(string newStatus)
    {
        if (_currentOrder != null)
        {
            string oldStatus = _currentOrder.Status;
            _currentOrder.Status = newStatus;
            Console.WriteLine($"\nСтатус заказа #{_currentOrder.Id} изменен: {oldStatus} -> {newStatus}");
            NotifyObservers();
        }
    }
}

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public List<string> Items { get; set; }
    public string Status { get; set; }
}

public class Customer : IOrderObserver
{
    private string _name;

    public Customer(string name)
    {
        _name = name;
    }

    public void Update(Order order)
    {
        if (order.CustomerName == _name)
        {
            Console.WriteLine($"[Клиент {_name}] Получено обновление заказа #{order.Id}: {order.Status}");
            if (order.Status == "Готов к подаче")
            {
                Console.WriteLine($"[Клиент {_name}] Заказ готов! Можно забирать.");
            }
        }
    }
}

public class Chef : IOrderObserver
{
    public void Update(Order order)
    {
        Console.WriteLine($"[Шеф-повар] Получен заказ #{order.Id}: {string.Join(", ", order.Items)}");

        if (order.Status == "Создан")
        {
            Console.WriteLine($"[Шеф-повар] Начинаю готовить заказ #{order.Id}");
        }
    }
}

public class Waiter : IOrderObserver
{
    private string _name;

    public Waiter(string name)
    {
        _name = name;
    }

    public void Update(Order order)
    {
        Console.WriteLine($"[Официант {_name}] Статус заказа #{order.Id}: {order.Status}");

        if (order.Status == "Готов к подаче")
        {
            Console.WriteLine($"[Официант {_name}] Несу заказ #{order.Id} клиенту {order.CustomerName}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var orderSystem = new OrderSystem();

        var customer1 = new Customer("Иван Петров");
        var chef = new Chef();
        var waiter1 = new Waiter("Анна");
        var waiter2 = new Waiter("Сергей");

        orderSystem.AddObserver(customer1);
        orderSystem.AddObserver(chef);
        orderSystem.AddObserver(waiter1);
        orderSystem.AddObserver(waiter2);

        orderSystem.CreateOrder(1, "Иван Петров", new List<string> { "Стейк", "Картофель", "Салат" });

        orderSystem.UpdateOrderStatus("В процессе приготовления");
        System.Threading.Thread.Sleep(2000);

        orderSystem.UpdateOrderStatus("Готов к подаче");
        System.Threading.Thread.Sleep(1000);

        orderSystem.UpdateOrderStatus("Завершен");

        orderSystem.RemoveObserver(waiter2);

        orderSystem.CreateOrder(2, "Иван Петров", new List<string> { "Суп", "Рыба" });
        orderSystem.UpdateOrderStatus("Готов к подаче");
    }
}