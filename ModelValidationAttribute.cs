using System.IdentityModel;
using System.Linq;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace DevTeam.ModelValidationAttribute
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var messages = actionContext.ModelState.Values
                    .SelectMany(e => e.Errors)
                    .Select(GetErrorInfo);
                var error = string.Join("; ", messages);

                throw new BadRequestException(error);
            }
            base.OnActionExecuting(actionContext);
        }

        private string GetErrorInfo(ModelError error)
        {
            var message = new StringBuilder();

            if (!string.IsNullOrEmpty(error.ErrorMessage))
                message.Append(error.ErrorMessage);

            if (error.Exception != null && !string.IsNullOrEmpty(error.Exception.Message))
                message.Append(error.Exception.Message);

            return message.ToString();
        }
    }
}