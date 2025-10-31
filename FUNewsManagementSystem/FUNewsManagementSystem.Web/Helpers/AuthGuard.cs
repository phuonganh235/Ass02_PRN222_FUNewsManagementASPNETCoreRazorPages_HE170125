using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Web.Helpers
{
    public class AuthGuard
    {
        public static IActionResult? RequireLogin(PageModel page)
        {
            var httpCtx = page.HttpContext;
            var accountId = httpCtx.Session.GetInt32("AccountID");
            var email = httpCtx.Session.GetString("AccountEmail");

            if (accountId == null && string.IsNullOrEmpty(email))
            {
                // Chưa login -> quay lại trang login
                return page.RedirectToPage("/Auth/Login");
            }
            return null;
        }

        public static bool IsAdmin(PageModel page)
        {
            var role = page.HttpContext.Session.GetInt32("AccountRole");
            return role == 3;
        }

        public static bool IsStaff(PageModel page)
        {
            var role = page.HttpContext.Session.GetInt32("AccountRole");
            return role == 2;
        }

        public static bool IsLecturer(PageModel page)
        {
            var role = page.HttpContext.Session.GetInt32("AccountRole");
            return role == 1;
        }
    }
}
