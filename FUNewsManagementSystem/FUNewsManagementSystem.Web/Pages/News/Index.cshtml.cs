using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLogic.Services;
using DataAccess.Context;
using BusinessLogic.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Web.Pages.News
{
    public class IndexModel : PageModel
    {
        public List<NewsArticleVM> NewsList { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Cấu hình chuỗi kết nối
            var options = new DbContextOptionsBuilder<FUNewsDbContext>()
                .UseSqlServer("Server=.;Database=FUNewsManagement;Trusted_Connection=True;Encrypt=False")
                .Options;

            // Khởi tạo context và service
            using var ctx = new FUNewsDbContext(options);
            var newsService = new NewsArticleService(ctx);

            // Lấy danh sách bài viết
            NewsList = await newsService.GetPagedAsync(page: 1, pageSize: 10);
        }
    }
}
