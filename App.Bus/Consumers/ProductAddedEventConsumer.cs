using App.Application.Contracts.ServiceBus;
using App.Application.Features.Email;
using App.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace App.Bus.Consumers;


public class ProductAddedEventConsumer(IServiceBus bus, IConfiguration config) : IConsumer<ProductAddedEvent>
{
    public async Task Consume(ConsumeContext<ProductAddedEvent> context)
    {
        var e = context.Message;
        var to = config["Notifications:ProductAdded:To"] ?? "senin@mailin.com";
        var subject = $"New product added: {e.Name}";
        var text = $"""
                    A new product was added.

                    Id: {e.Id}
                    Name: {e.Name}
                    Price: {e.Price}
                    """;

        await bus.SendAsync(new EmailMessage { To = to, Subject = subject, Text = text },
            "emailQueue",
            context.CancellationToken);
    }
}
