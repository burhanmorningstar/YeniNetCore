using App.Domain.Events;

namespace App.Application.Features.Email;

public class EmailMessage : IEventOrMessage
{
    public string To { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Text { get; set; } = null!;
}