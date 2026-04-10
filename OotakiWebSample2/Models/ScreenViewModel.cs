namespace OotakiWebSample2.Models
{
    public class ScreenViewModel
    {
        public List<ListViewModel>? ListViewModels { get; set; }
    }

    public class ListViewModel
    {
        public string? ItemName { get; set; }
        public List<string?>? Values { get; set; }  // ←複数対応
    }
}
