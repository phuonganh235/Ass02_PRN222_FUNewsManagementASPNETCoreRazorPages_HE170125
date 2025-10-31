using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FUNewsManagementSystem.Web.Filters
{
    // Filter này nhận danh sách role hợp lệ cho trang/folder
    public class RequireRoleFilter : IPageFilter
    {
        private readonly int[] _allowedRoles;

        // Vì filter được thêm qua AddScoped trong Program.cs,
        // Razor Pages sẽ tạo instance thông qua DI.
        public RequireRoleFilter(int[]? allowedRoles = null)
        {
            _allowedRoles = allowedRoles ?? Array.Empty<int>();
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var httpCtx = context.HttpContext;
            var roleVal = httpCtx.Session.GetInt32("Role");

            // chưa login -> redirect login
            if (roleVal == null)
            {
                context.Result = new RedirectToPageResult("/Auth/Login");
                return;
            }

            // đã login nhưng không có quyền
            if (_allowedRoles.Length > 0 && !_allowedRoles.Contains(roleVal.Value))
            {
                // nếu cố vào admin mà ko phải admin -> về login hoặc báo quyền
                context.Result = new ContentResult
                {
                    Content = "Access denied. You do not have permission to view this page.",
                    StatusCode = 403
                };
            }
        }
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context) { }
    }
}
