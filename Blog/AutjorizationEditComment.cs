using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DAL.Models;

namespace Blog
{
    public class AuthorizationEditComment : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Here I can get userId from my params.
            var commentId = context.RouteData.Values["id"].ToString();

            // It is then being checked against current user claims.
            // The user is only authorized if the userId is equals to ClaimsType.Value and claims Type is equals to NameIdentifier. 
            var isUserAuthorized = context.HttpContext.User.IsInRole("Модератор")
                    || context.HttpContext.User.IsInRole("Администратор")
                    || context.HttpContext.User.Claims.Any(c => c.Type == "Comment" && c.Value == commentId);

            if (!isUserAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
