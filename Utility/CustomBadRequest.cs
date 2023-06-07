using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Utility
{
    public class CustomBadRequest : APIResponse
    {
        public CustomBadRequest(ActionContext context)
        {
            StatusCode = HttpStatusCode.BadRequest;
            IsSuccess = false;
            ErrorMessages = ConstructErrorMessages(context);
            Result = null;
        }

        private static List<string> ConstructErrorMessages(ActionContext context)
        {
            var errorMessages = new List<string>();
            foreach (var keyModelStatePair in context.ModelState)
            {
                //var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    if (errors.Count == 1)
                    {
                        errorMessages.Add(errors[0].ErrorMessage);
                    }
                    else
                    {
                        for (var i = 0; i < errors.Count; i++)
                        {
                            errorMessages.Add(errors[i].ErrorMessage);
                        }
                    }
                }
            }
            return errorMessages;
        }
    }
}
