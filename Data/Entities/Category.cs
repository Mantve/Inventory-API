using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(32)] [Required] public string Name { get; set; }
        [Required] public User Author { get; set; }
        [MaxLength(64)] public string Description { get; set; }
    }
}
