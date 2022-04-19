using System;

namespace Inventory_API.Data.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public User User { get; set; }
        public byte[] Endpoint { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public byte[] P256dh { get; set; }
        public byte[] Auth { get; set; }
    }
}
