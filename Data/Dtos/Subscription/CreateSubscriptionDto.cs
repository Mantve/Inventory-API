using System;

namespace Inventory_API.Data.Dtos.Subscription
{
    public record CreateSubscriptionDto(
        string Username,
        string Endpoint,
        DateTime ExpirationTime,
        string P256dh,
        string Auth);
}
