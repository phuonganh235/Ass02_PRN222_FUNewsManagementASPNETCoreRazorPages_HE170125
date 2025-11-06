using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public DashboardViewModel Dashboard { get; set; } = new DashboardViewModel();

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public DashboardModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is Admin
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole != 0)
            {
                return RedirectToPage("/Index");
            }

            Dashboard = await _dashboardService.GetDashboardDataAsync(StartDate, EndDate);

            return Page();
        }

        public async Task<IActionResult> OnGetGetStatsAsync()
        {
            var dashboard = await _dashboardService.GetDashboardDataAsync();

            return new JsonResult(new
            {
                totalArticles = dashboard.TotalArticles,
                activeArticles = dashboard.ActiveArticles,
                inactiveArticles = dashboard.InactiveArticles,
                totalAccounts = dashboard.TotalAccounts,
                totalCategories = dashboard.TotalCategories,
                totalTags = dashboard.TotalTags,
                totalComments = dashboard.TotalComments,
                articlesByCategory = dashboard.ArticlesByCategory,
                articlesByAuthor = dashboard.ArticlesByAuthor
            });
        }
    }
}