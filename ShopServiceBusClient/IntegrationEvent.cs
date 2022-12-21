using System.Text.Json.Serialization;

namespace ShopServiceBusClient;

public record IntegrationEvent
{
    public IntegrationEvent()
    {
        EventId = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonConstructor]
    public IntegrationEvent(Guid eventId, DateTime createDate)
    {
        EventId = eventId;
        CreationDate = createDate;
    }

    [JsonInclude]
    public Guid EventId { get; private init; }

    [JsonInclude]
    public DateTime CreationDate { get; private init; }
}
