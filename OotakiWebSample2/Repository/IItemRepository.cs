using OotakiWebSample2.DTO;

namespace OotakiWebSample2.Repository
{
    /// <summary>
    /// 項目データのデータベースアクセスに関する操作を定義するリポジトリインターフェース
    /// </summary>
    public interface IItemRepository
    {
        /// <summary>
        /// 指定した画面IDとユーザーIDに対応する項目一覧を非同期で取得する
        /// </summary>
        /// <param name="screenId">取得対象の画面ID</param>
        /// <param name="userId">データを取得するユーザーのID</param>
        /// <returns>項目データのリスト。該当データがない場合は空のリスト</returns>
        Task<List<ItemListDto>> GetScreenItemsAsync(int screenId, int userId);

        /// <summary>
        /// 指定した項目の表示・非表示フラグを非同期で更新する
        /// </summary>
        /// <param name="visibleFlg">true：表示、false：非表示</param>
        /// <param name="itemName">更新対象の項目名</param>
        /// <param name="screenId">更新対象の画面ID</param>
        /// <param name="userId">更新を実行するユーザーのID</param>
        Task UpdateVisibleFlagAsync(bool visibleFlg, string itemName, int screenId, int userId);
    }
}