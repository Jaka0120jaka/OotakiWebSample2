using Microsoft.EntityFrameworkCore;
using OotakiWebSample2.DTO;
using OotakiWebSample2.SqlQueries;
using YourApp.Data;

namespace OotakiWebSample2.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        //コンストラクタ
        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        // 受付一覧データを取得する
        public async Task<List<ItemListDto>> GetScreenItemsAsync(int screenId, int userId)
        {
            return await _context.Set<ItemListDto>()
                .FromSqlRaw(SqlQueryConstants.GetScreenList, screenId, userId)
                .AsNoTracking()
                .ToListAsync();
        }

        // visible_flgを更新する
        public async Task UpdateVisibleFlagAsync(bool visibleFlg, string itemName, int screenId, int userId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                SqlQueryConstants.UpdateVisibleFlag,
                visibleFlg, itemName, screenId, userId);
        }
    }
}
