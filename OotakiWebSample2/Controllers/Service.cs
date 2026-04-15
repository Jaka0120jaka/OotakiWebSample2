using OotakiWebSample2.Models;
using OotakiWebSample2.Repository;

namespace OotakiWebSample2.Controllers
{
    /// <summary>
    /// 画面データの取得および項目表示制御に関するビジネスロジックを提供するサービスクラス
    /// </summary>
    public class Service : IService
    {
        private readonly IItemRepository _repository;

        /// /// <summary>
        /// Service のコンストラクター
        /// </summary>
        /// <param name="repository">データベースアクセスに使用するリポジトリインスタンス</param>
        public Service(IItemRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 指定した画面IDとユーザーIDに対応する画面データを非同期で取得する
        /// </summary>
        /// <remarks>
        /// リポジトリから取得したDTOリストを ItemName 単位でグループ化し、
        /// ListViewModel のリストに変換して ScreenViewModel として返す。
        /// VisibleFlag はグループ内の先頭レコードの値を使用する。
        /// </remarks>
        /// <param name="screenId">取得対象の画面ID</param>
        /// <param name="userId">データを取得するユーザーのID</param>
        /// <returns>画面表示に必要なデータを含む <see cref="ScreenViewModel"/></returns>
        public async Task<ScreenViewModel> GetScreenAsync(int screenId, int userId)
        {
            var dtoList = await _repository.GetScreenItemsAsync(screenId, userId);

            // ItemNameごとにグループ化
            var grouped = dtoList
                .GroupBy(x => x.ItemName)
                .ToList();

            // ViewModelに変換d
            var viewModel = new ScreenViewModel
            {
                ListViewModels = grouped.Select(g => new ListViewModel
                {
                    ItemName = g.Key,
                    Values = g.Select(x => x.Value).ToList(), 
                    VisibleFlag = g.First().VisibleFlag
                }).ToList()
            };

            return viewModel;
        }

        /// <summary>
        /// 指定した項目の表示・非表示フラグを非同期で更新する
        /// </summary>
        /// <remarks>
        /// ビジネスロジックはなく、リポジトリへの更新処理を委譲する。
        /// </remarks>
        /// <param name="visibleFlg">true：表示、false：非表示</param>
        /// <param name="itemName">更新対象の項目名</param>
        /// <param name="screenId">更新対象の画面ID</param>
        /// <param name="userId">更新を実行するユーザーのID</param>
        public async Task UpdateVisibleFlagAsync(bool visibleFlg, string itemName, int screenId, int userId)
        {
            await _repository.UpdateVisibleFlagAsync(visibleFlg, itemName, screenId, userId);
        }
    }
}