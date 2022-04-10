using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inventory_API.Data.Entities
{
    public class ListItem
    {
        public int Id { get; set; }
        [Required] public Item Item { get; set; }
        [DefaultValue(false)] public bool Completed { get; set; }
        [Required] public List ParentList { get; set; }
    }
}
