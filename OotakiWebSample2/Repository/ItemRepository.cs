using Microsoft.EntityFrameworkCore;
using OotakiWebSample2.DTO;
using OotakiWebSample2.SqlQueries;
using YourApp.Data;

namespace OotakiWebSample2.Repository
{
    /// <summary>
    /// 項目データのデータベースアクセスを担当するリポジトリクラス
    /// </summary>
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// ItemRepository のコンストラクター
        /// </summary>
        /// <param name="context">データベース操作に使用するコンテキストインスタンス</param>
        public ItemRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 指定した画面IDとユーザーIDに対応する項目一覧を非同期で取得する
        /// </summary>
        /// <remarks>
        /// <see cref="SqlQueryConstants.GetScreenList"/> のSQLを使用してデータを取得する。
        /// 変更追跡を無効化（AsNoTracking）しており、読み取り専用で使用する。
        /// </remarks>
        /// <param name="screenId">取得対象の画面ID</param>
        /// <param name="userId">データを取得するユーザーのID</param>
        /// <returns>項目データのリスト。該当データがない場合は空のリスト</returns>
        public async Task<List<ItemListDto>> GetScreenItemsAsync(int screenId, int userId)
        {
            return await _context.Set<ItemListDto>()
                .FromSqlRaw(SqlQueryConstants.GetScreenList, screenId, userId)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// 指定した項目の表示・非表示フラグを非同期で更新する
        /// </summary>
        /// <remarks>
        /// <see cref="SqlQueryConstants.UpdateVisibleFlag"/> のSQLを使用して直接DB更新する。
        /// SELECT ではなく UPDATE のため、ExecuteSqlRawAsync を使用する。
        /// </remarks>
        /// <param name="visibleFlg">true：表示、false：非表示</param>
        /// <param name="itemName">更新対象の項目名</param>
        /// <param name="screenId">更新対象の画面ID</param>
        /// <param name="userId">更新を実行するユーザーのID</param>
        public async Task UpdateVisibleFlagAsync(bool visibleFlg, string itemName, int screenId, int userId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                SqlQueryConstants.UpdateVisibleFlag,
                visibleFlg, itemName, screenId, userId);
        }
    }
}