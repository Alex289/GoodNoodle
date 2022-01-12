using GoodNoodle.Domain.Errors;
using MediatR;
using System;

namespace GoodNoodle.Domain.Notifications;

public class DomainNotification : INotification
{
    public Guid DomainNotificationId { get; private set; }
    public DomainError Error { get; private set; }

    public DomainNotification(string key, string value)
    {
        Initialize(key, value);
    }

    private void Initialize(string key, string value)
    {
        DomainNotificationId = Guid.NewGuid();
        Error = new DomainError() { Key = key, Value = value };
    }
}
