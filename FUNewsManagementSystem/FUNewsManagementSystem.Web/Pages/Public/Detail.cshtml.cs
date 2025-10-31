using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Pages.Public
{
    public class DetailModel : PageModel
    {
        private readonly INewsArticleService _newsSrv;

        public DetailModel(INewsArticleService newsSrv)
        {
            _newsSrv = newsSrv;
        }

        public NewsArticleVM? Article { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var art = await _newsSrv.GetByIdAsync(id);
            if (art == null || !art.NewsStatus)
            {
                return NotFound();
            }

            Article = art;
            return Page();
        }
    }
}
