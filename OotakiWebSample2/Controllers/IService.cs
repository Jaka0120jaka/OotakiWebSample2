using OotakiWebSample2.Models;

namespace OotakiWebSample2.Controllers
{
    /// <summary>
    /// 画面データの取得および項目表示制御に関する操作を定義するサービスインターフェース
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 指定した画面IDとユーザーIDに対応する画面データを非同期で取得する
        /// </summary>
        /// <param name="screenId">取得対象の画面ID</param>
        /// <param name="userId">データを取得するユーザーのID</param>
        /// <returns>画面表示に必要なデータを含む <see cref="ScreenViewModel"/></returns>
        Task<ScreenViewModel> GetScreenAsync(int screenId, int userId);

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