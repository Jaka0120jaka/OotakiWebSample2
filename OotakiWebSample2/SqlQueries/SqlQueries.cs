namespace OotakiWebSample2.SqlQueries
{
    public class SqlQueryConstants
    {
        /*************画面ごとの項目リストを取得************/
        public const string GetScreenList = @"
        SELECT 
            i.item_name AS ItemName,
            l.value AS Value,
            h.visible_flg AS VisibleFlag
        FROM hyouji_set_tbl h
        INNER JOIN item_info_mst i ON h.item_id = i.item_id
        INNER JOIN item_list_mst l ON l.item_id = i.item_id
          AND h.scr_id = {0}
          AND h.user_id = {1}";


        /*************visible_flgを更新するSQL************/
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
