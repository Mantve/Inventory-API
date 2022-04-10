using System;

namespace Inventory_API.Data.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Endpoint { get; set; }
        public DateTime? RxpirationTime { get; set; }
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}
