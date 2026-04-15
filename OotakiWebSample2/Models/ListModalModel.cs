namespace OotakiWebSample2.Models
{
    /// <summary>
    /// 項目一覧表示用モーダルダイアログのビューモデル
    /// </summary>
    public class ListModalModel
    {
        /// <summary>モーダル内に表示する項目のリスト</summary>
        public List<ListViewModel>? ListViewModels { get; set; }

        /// <summary>モーダルの識別子</summary>
        public string? ListModal { get; set; }
    }
}