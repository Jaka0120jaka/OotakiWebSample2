using Microsoft.AspNetCore.Mvc;
using OotakiWebSample2.Models;
using System.Diagnostics;

namespace OotakiWebSample2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IService _service;

        public HomeController(IService service)
        {
            _service = service;
        }

        // 画面表示
        public async Task<IActionResult> Index()
        {
            // 画面データ取得
            var vm = await _service.GetScreenAsync(1, 1);
            // ViewにViewModelを渡す
            return View(vm);
        }

        // 表示/非表示フラグ更新（Ajax用API）
        [HttpPost]
        public async Task<IActionResult> UpdateVisible([FromBody] UpdateVisibleRequest request)
        {
            // クライアント（JS）から送られてきた値を元にDB更新
            await _service.UpdateVisibleFlagAsync(request.VisibleFlg, request.ItemName, request.ScreenId, request.UserId);
            return Ok();
        }

        //エラー画面
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
