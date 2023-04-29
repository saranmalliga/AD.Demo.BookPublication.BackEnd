using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AD.Demo.BookPublication.Api.CustomActionFilter
{
    public class HasRoleAccess: ActionFilterAttribute
    {
        public string Roles { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var result = new ObjectResult(context.ModelState)
                {
                    Value = "Access Denied",
                    StatusCode = StatusCodes.Status403Forbidden
                };

                //Implement custom Filters Business logics
                if (true)
                {
                    return;
                }
                context.Result = result;
            }
            catch (Exception e)
            {
            }
        }
    }
}
