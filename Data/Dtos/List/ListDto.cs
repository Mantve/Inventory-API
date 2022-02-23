using Inventory_API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Dtos.List
{
    public record ListDto(int Id, string Name, List<Item> Items);
}
