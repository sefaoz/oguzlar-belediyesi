using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OguzlarBelediyesi.WebAPI.Contracts.Responses;

namespace OguzlarBelediyesi.WebAPI.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = new Dictionary<string, List<string>>();
            
            foreach (var key in context.ModelState.Keys)
            {
                var state = context.ModelState[key];
                if (state?.Errors.Count > 0)
                {
                    errors[key] = state.Errors.Select(e => e.ErrorMessage).ToList();
                }
            }

            var response = new ValidationErrorResponse
            {
                Success = false,
                Message = "Validasyon hataları oluştu.",
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
