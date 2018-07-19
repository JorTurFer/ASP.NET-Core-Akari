using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Akari_Net.Core.Areas.Usuarios.Manage.Manage
{
    public static class ManageNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Index => "Index";

        public static string UsersManage => "Users";

        public static string RolesManage => "Roles";

        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string UsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string RolesPasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, UsersManage);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
