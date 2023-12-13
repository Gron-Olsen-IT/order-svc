using OrderAPI.Infrastructure;
using OrderAPI.Models;
using OrderAPI.Services;

namespace OrderAPI;
public class OrderWorker : BackgroundService
{
    private readonly ILogger<OrderWorker> _logger;
    private readonly IServiceOrder _serviceOrder;

    public OrderWorker(ILogger<OrderWorker> logger, IServiceOrder serviceOrder)
    {
        _serviceOrder = serviceOrder;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (DateTime.Now.Second % 10 == 0 ){
                _logger.LogInformation("Worker is running at: {time}", DateTimeOffset.Now);
            }
            if (DateTime.Now.Minute % 1 == 0 && DateTime.Now.Second == 1)
            {
                try
                {
                    _logger.LogInformation("Check if any auctions is finish");
                    List<Order>? orders = await _serviceOrder.CheckIfAnyAuctionsAreDone();
                    if (orders != null)
                    {
                        foreach (var order in orders)
                        {
                            _logger.LogInformation("Order created: " + order.Id);
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("Some errors occured", e.Message);
                }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}