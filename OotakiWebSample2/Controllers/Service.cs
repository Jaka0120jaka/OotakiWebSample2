using OotakiWebSample2.Models;
using OotakiWebSample2.Repository;

namespace OotakiWebSample2.Controllers
{
    public class Service : IService
    {
        private readonly IItemRepository _repository;

        public Service(IItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<ScreenViewModel> GetScreenAsync(int screenId, int userId)
        {
            var dtoList = await _repository.GetScreenItemsAsync(screenId, userId);

            // ItemNameごとにグループ化
            var grouped = dtoList
                .GroupBy(x => x.ItemName)
                .ToList();

            // ViewModelに変換d
            var viewModel = new ScreenViewModel
            {
                ListViewModels = grouped.Select(g => new ListViewModel
                {
                    ItemName = g.Key,
                    Values = g.Select(x => x.Value).ToList(), 
                    VisibleFlag = g.First().VisibleFlag
                }).ToList()
            };

            return viewModel;
        }

        public async Task UpdateVisibleFlagAsync(bool visibleFlg, string itemName, int screenId, int userId)
        {
            await _repository.UpdateVisibleFlagAsync(visibleFlg, itemName, screenId, userId);
        }
    }
}