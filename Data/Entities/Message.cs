using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        [Required] public User Recipient { get; set; }
        [Required] public User Author { get; set; }
        [MaxLength(64)] public string Contents { get; set; }
        [Required] public MessageType MessageType { get; set; }
    }
}
