namespace OotakiWebSample2.SqlQueries
{
    /// <summary>
    /// データベースアクセスで使用するSQLクエリ文字列を管理する静的クラス
    /// </summary>
    public static class SqlQueryConstants
    {
        /// <summary>
        /// 指定した画面IDとユーザーIDに紐づく項目一覧を取得するSQLクエリ。
        /// 表示設定・項目情報・項目値を結合して取得する。
        /// </summary>
        /// <remarks>
        /// パラメーター：
        ///   {0} : 画面ID（scr_id）
        ///   {1} : ユーザーID（user_id）
        ///
        /// 結合テーブル：
        ///   INNER JOIN item_info_mst … 項目名などの項目マスター情報（必須）
        ///   INNER JOIN item_list_mst … 項目の値一覧（必須）
        /// </remarks>
        public const string GetScreenList = @"
            SELECT 
                i.item_name AS ""ItemName"",
                l.value AS ""Value"",
                h.visible_flg AS ""VisibleFlag""
            FROM hyouji_set_tbl h
            INNER JOIN item_info_mst i ON h.item_id = i.item_id
            INNER JOIN item_list_mst l ON l.item_id = i.item_id
              AND h.scr_id = {0}
              AND h.user_id = {1}";


        /// <summary>
        /// 指定した項目の表示・非表示フラグを更新するSQLクエリ。
        /// </summary>
        /// <remarks>
        /// パラメーター：
        ///   {0} : 表示フラグ（visible_flg）true：表示、false：非表示
        ///   {1} : 項目名（item_name）
        ///   {2} : 画面ID（scr_id）
        ///   {3} : ユーザーID（user_id）
        ///
        /// 更新対象テーブル：
        ///   hyouji_set_tbl … ユーザーごとの表示設定テーブル
        ///
        /// 結合テーブル：
        ///   item_info_mst … 項目名から item_id を特定するために結合
        /// </remarks>
        public const string UpdateVisibleFlag = @"
            UPDATE hyouji_set_tbl h
            SET visible_flg = {0}
            FROM item_info_mst i
            WHERE h.item_id = i.item_id
              AND i.item_name = {1}
              AND h.scr_id = {2}
              AND h.user_id = {3}";
    }
}