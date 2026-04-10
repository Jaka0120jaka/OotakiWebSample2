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
        WHERE h.visible_flg = true 
          AND h.scr_id = {0}
          AND h.user_id = {1}";
    }
}
