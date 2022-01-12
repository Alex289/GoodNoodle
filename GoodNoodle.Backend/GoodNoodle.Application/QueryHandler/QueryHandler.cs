using GoodNoodle.Domain.Notifications;
using MediatR;
using System.Threading.Tasks;

namespace GoodNoodle.Application.QueryHandler;

public class QueryHandler
{
    private readonly IMediator _bus;

    public QueryHandler(IMediator bus)
    {
        _bus = bus;
    }

    protected async Task NotifyErrorAsync(string key, string value)
    {
        await _bus.Publish(
            new DomainNotification(
                key,
                value));
    }
}
