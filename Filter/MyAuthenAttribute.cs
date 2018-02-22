using qiushi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace qiushi.Web.Filter
{
    public class MyAuthenAttribute : FilterAttribute, IAuthenticationFilter
    {
        string roleName = "";

        public MyAuthenAttribute()
        {

        }

        public MyAuthenAttribute(string roleName)
        {
            this.roleName = roleName;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = filterContext.HttpContext.Session["User"];
            if (user == null)
            {
                //filterContext.HttpContext.Response.Redirect("/Account/Logon");
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Login", "Account");
                filterContext.Result = new RedirectResult(url);

                return;
            }

            var currentUser = (UserEntity)user;
            if(roleName!= "" && currentUser.UserRole.First().Role.Code != roleName)
            {
                var Url = new UrlHelper(filterContext.RequestContext);
                var url = Url.Action("Login", "Account");
                filterContext.Result = new RedirectResult(url);

                return;
            }
            
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var a = "";
           // throw new NotImplementedException();
        }
    }
}