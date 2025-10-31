using BusinessLogic.Interfaces;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Web.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsSrv;
        private readonly ICategoryService _catSrv;
        private readonly ITagService _tagSrv;

        public IndexModel(
            INewsArticleService newsSrv,
            ICategoryService catSrv,
            ITagService tagSrv)
        {
            _newsSrv = newsSrv;
            _catSrv = catSrv;
            _tagSrv = tagSrv;
        }

        public List<NewsArticleVM> NewsList { get; set; } = new();

        // ---- Binding form fields ----
        [BindProperty] public string? EditId { get; set; }

        [BindProperty, Required]
        public string Title { get; set; } = string.Empty;

        [BindProperty, Required]
        public string Headline { get; set; } = string.Empty;

        [BindProperty, Required]
        public string Content { get; set; } = string.Empty;

        [BindProperty]
        public string Source { get; set; } = string.Empty;

        [BindProperty, Required]
        public short CategoryId { get; set; }

        [BindProperty]
        public new bool Status { get; set; } = true;

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new();

        public async Task OnGetAsync()
        {
            NewsList = await _newsSrv.GetAllAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                NewsList = await _newsSrv.GetAllAsync();
                return Page();
            }

            short createdById = (short)(HttpContext.Session.GetInt32("AccountId") ?? 0);

            await _newsSrv.CreateAsync(
                Title,
                Headline,
                Content,
                Source,
                CategoryId,
                createdById,
                Status,
                SelectedTagIds ?? new List<int>()
            );

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                NewsList = await _newsSrv.GetAllAsync();
                return Page();
            }

            await _newsSrv.UpdateAsync(
                EditId ?? "",
                Title,
                Headline,
                Content,
                Source,
                CategoryId,
                Status,
                SelectedTagIds ?? new List<int>()
            );

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string deleteId)
        {
            await _newsSrv.DeleteAsync(deleteId);
            return RedirectToPage();
        }
    }
}
