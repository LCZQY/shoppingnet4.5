using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZapiCore;
namespace ShoppingApi.Common.Filter
{
    /// <summary>
    /// 模型验证过滤器
    /// </summary>
    public class ApiActionFilter : IActionFilter
    {

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                string msg = string.Empty;
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        msg += error.ErrorMessage + ",";
                    }
                }
                var result = new ResponseMessage()
                {
                    Code = ResponseCodeDefines.ModelStateInvalid,
                    Message = msg,

                };
                context.Result = new JsonResult(result);
            }
        }
    }
}
