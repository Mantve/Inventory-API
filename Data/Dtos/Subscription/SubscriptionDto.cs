using Inventory_API.Data.Dtos.User;
using System;

namespace Inventory_API.Data.Dtos.Subscription
{
    public record SubscriptionDto(
        UserDto User,
        string Endpoint,
        DateTime ExpirationTime,
        string P256dh,
        string Auth);
}
