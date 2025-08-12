using App.Application.Features.Email;

namespace App.Application.Features.Messaging;

public interface IEmailQueuePublisher
{
    Task PublishAsync(EmailMessage message, CancellationToken ct = default);
}