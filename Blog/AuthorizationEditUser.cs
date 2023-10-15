using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Blog
{
    public class AuthorizationEditUser : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Here I can get userId from my params.
            var userId = context.RouteData.Values["id"].ToString();

            // It is then being checked against current user claims.
            // The user is only authorized if the userId is equals to ClaimsType.Value and claims Type is equals to NameIdentifier. 
            var isUserAuthorized = context.HttpContext.User.IsInRole("Модератор")
                    || context.HttpContext.User.IsInRole("Администратор")
                    || context.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && c.Value == userId);

            if (!isUserAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
