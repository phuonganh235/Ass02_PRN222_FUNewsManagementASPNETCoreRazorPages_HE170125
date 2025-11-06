using BusinessLogic.ViewModels;

namespace BusinessLogic.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}