using Microsoft.Extensions.Logging;
using Sms.Test;
using TestDLL.Entities;
using TestDLL.Services.GrpcApiClient;

namespace TestConsoleApp;

public sealed class MenuConsoleService
{
    private readonly ILogger<MenuConsoleService> _logger;
    private readonly IGrpcApiClient _client;
    private readonly DbInitializer _db;

    public MenuConsoleService(
        ILogger<MenuConsoleService> logger,
        IGrpcApiClient client,
        DbInitializer db)
    {
        _logger = logger;
        _client = client;
        _db = db;
    }

    public async Task RunAsync()
    {
        try
        {
            _logger.LogInformation("Запрос меню с сервера...");

            var menu = await _client.GetMenuAsync(withPrice: true);

            if (menu == null || menu.Count == 0)
            {
                Console.WriteLine("Ошибка: меню не получено от сервера.");
                return;
            }

            await _db.InsertMenuAsync(menu);

            Console.WriteLine("Меню:");

            foreach (var item in menu)
                Console.WriteLine($"{item.Name} – {item.Article} – {item.Price}");

            var order = new TestDLL.Entities.Order
            {
                Id = Guid.NewGuid(),
                Items = new List<OrderItem>()
            };

            var orderItems = ReadOrderFromConsole(menu);

            order.Items.AddRange(orderItems);

            await _client.SendOrderAsync(order.Id, order.Items);

            Console.WriteLine("УСПЕХ");

            _logger.LogInformation("Заказ успешно отправлен. Id={OrderId}", order.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при работе с меню или заказом");
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static IReadOnlyList<OrderItem> ReadOrderFromConsole(
        IReadOnlyList<MenuItem> menu)
    {
        var dict = menu.ToDictionary(x => x.Article);
        var result = new List<OrderItem>();

        while (true)
        {
            Console.WriteLine("Введите заказ (Код:Кол-во;Код:Кол-во):");
            var input = Console.ReadLine();

            result.Clear();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            var parts = input.Split(';', StringSplitOptions.RemoveEmptyEntries);
            var valid = true;

            foreach (var part in parts)
            {
                var kv = part.Split(':');
                if (kv.Length != 2 ||
                    !dict.TryGetValue(kv[0], out var menuItem) ||
                    !double.TryParse(kv[1], out var qty) ||
                    qty <= 0)
                {
                    valid = false;
                    break;
                }

                result.Add(new OrderItem
                {
                    Id = menuItem.Id,
                    Quantity = qty
                });
            }

            if (valid && result.Count > 0)
                return result;

            Console.WriteLine("Ошибка ввода, попробуйте ещё раз.");
        }
    }
}
