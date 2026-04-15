namespace OotakiWebSample2.Models
{
    /// <summary>
    /// 表示・非表示フラグ更新リクエストのモデル。
    /// Ajax（JSON）経由で HomeController.UpdateVisible に送信される。
    /// </summary>
    public class UpdateVisibleRequest
    {
        /// <summary>更新後の表示・非表示フラグ。true：表示、false：非表示</summary>
        public bool VisibleFlg { get; set; }

        /// <summary>更新対象の項目名</summary>
        public string ItemName { get; set; } = "";

        /// <summary>更新対象の画面ID</summary>
        public int ScreenId { get; set; }

        /// <summary>更新を実行するユーザーのID</summary>
        public int UserId { get; set; }
    }
}