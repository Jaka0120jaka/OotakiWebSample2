using OotakiWebSample2.Models;

namespace OotakiWebSample2.Controllers
{
    public interface IService
    {
        Task<ScreenViewModel> GetScreenAsync(int screenId, int userId);
    }
}