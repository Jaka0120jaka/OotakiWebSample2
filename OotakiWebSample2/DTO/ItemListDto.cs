namespace OotakiWebSample2.DTO
{
    /// <summary>
    /// 項目一覧データを保持するDTO。
    /// SQLクエリの結果をマッピングするために使用する（主キーなし）。
    /// </summary>
    public class ItemListDto
    {
        /// <summary>項目の表示名</summary>
        public string? ItemName { get; set; }

        /// <summary>項目の値</summary>
        public string? Value { get; set; }

        /// <summary>表示・非表示フラグ。true：表示、false：非表示</summary>
        public bool VisibleFlag { get; set; }
    }
}