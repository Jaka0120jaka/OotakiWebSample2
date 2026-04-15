namespace OotakiWebSample2.Models
{
    /// <summary>
    /// 画面全体のビューモデル。
    /// 表示する項目一覧を保持する。
    /// </summary>
    public class ScreenViewModel
    {
        /// <summary>画面に表示する項目のリスト</summary>
        public List<ListViewModel>? ListViewModels { get; set; }
    }

    /// <summary>
    /// 個別の項目データを表すビューモデル。
    /// 1つの項目に対して複数の値を持つことができる。
    /// </summary>
    public class ListViewModel
    {
        /// <summary>項目の表示名</summary>
        public string? ItemName { get; set; }

        /// <summary>
        /// 項目に紐づく値のリスト。
        /// 同一項目に複数の値が存在する場合に対応する。
        /// </summary>
        public List<string?>? Values { get; set; }

        /// <summary>表示・非表示フラグ。true：表示、false：非表示</summary>
        public bool VisibleFlag { get; set; }
    }
}