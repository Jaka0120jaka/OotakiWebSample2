namespace OotakiWebSample2.Models
{
    public class UpdateVisibleRequest
    {
        public bool VisibleFlg { get; set; }
        public string ItemName { get; set; } = "";
        public int ScreenId { get; set; }
        public int UserId { get; set; }
    }
}
