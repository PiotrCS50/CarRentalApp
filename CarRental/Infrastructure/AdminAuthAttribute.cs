using CarRental.App_Start;
using CarRental.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace CarRental.Infrastructure
{
    public class AdminAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            DataContext context = new DataContext();
            IIdentity identity = filterContext.Principal.Identity;
            var user = context.Users.Where(u => u.UserName == identity.Name).FirstOrDefault();
            if (!identity.IsAuthenticated || user.AccountType != Role.Administrator)
                filterContext.Result = new HttpUnauthorizedResult();
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if(filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "Account"},
                    {"action", "Login"},
                    {"returnUrl", filterContext.HttpContext.Request.RawUrl }
                });
            }
        }
    }
}