using Microsoft.AspNetCore.Mvc;
using OotakiWebSample2.Models;
using System.Diagnostics;

namespace OotakiWebSample2.Controllers
{
    /// <summary>
    /// アプリケーションのホーム画面を管理するコントローラー
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IService _service;

        /// <summary>
        /// HomeController のコンストラクター
        /// </summary>
        /// <param name="service">画面データ取得・更新に使用するサービスインスタンス</param>
        public HomeController(IService service)
        {
            _service = service;
        }

        /// <summary>
        /// ホーム画面を表示する
        /// </summary>
        /// <returns>ScreenViewModel をバインドした Index ビュー</returns>
        public async Task<IActionResult> Index()
        {
            // 画面データ取得
            var vm = await _service.GetScreenAsync(1, 1);
            // ViewにViewModelを渡す
            return View(vm);
        }

        /// <summary>
        /// 項目の表示・非表示フラグをAjaxで更新する
        /// </summary>
        /// <remarks>
        /// クライアント（JS）からJSON形式でリクエストを受け取り、
        /// 対象項目の VisibleFlg をデータベースに反映する。
        /// </remarks>
        /// <param name="request">更新対象の項目情報（VisibleFlg・ItemName・ScreenId・UserId）</param>
        /// <returns>更新成功時は 200 OK</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateVisible([FromBody] UpdateVisibleRequest request)
        {
            // クライアント（JS）から送られてきた値を元にDB更新
            await _service.UpdateVisibleFlagAsync(request.VisibleFlg, request.ItemName, request.ScreenId, request.UserId);
            return Ok();
        }

        /// <summary>
        /// エラー画面を表示する
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
