using OotakiWebSample2.DTO;

namespace OotakiWebSample2.Repository
{
    public interface IItemRepository
    {
        Task<List<ItemListDto>> GetScreenItemsAsync(int screenId, int userId);
        Task UpdateVisibleFlagAsync(bool visibleFlg, string itemName, int screenId, int userId);
    }
}