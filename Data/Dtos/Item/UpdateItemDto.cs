namespace Inventory_API.Data.Dtos.Item
{
    public record UpdateItemDto(
        string Name,
        float Quantity,
        decimal Value,
        int CategoryId,
        int? ParentItemId,
        string Comments,
        int RoomId,
        bool Sold);

}
