using GenericFunction.ResultObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;
using static GenericFunction.CommonMessages;

namespace SharedLibrary.Services.ModelStateValidation;

/// <summary>
/// Common model state validation class to validate all the post request.
/// </summary>
public sealed class ModelStateValidationAttribute : ActionFilterAttribute
{
    readonly StringBuilder _sb = new();
    private List<string> _modelList = new();
    public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ModelStateDictionary? modelState = context.ModelState;

        if (!modelState.IsValid)
        {
            _modelList = new();
            foreach (var mState in modelState.Values)
            {
                foreach (var error in mState.Errors)
                {
                    _sb.Append($"{error.ErrorMessage}\n");
                    _modelList.Add(error.ErrorMessage);
                }
            }
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var exceptionResult = JsonSerializer.Serialize(new ResponseDto<object>(new HttpContextAccessor())
            {
                Status = Status.Failed,
                RecordCount = modelState.Count,
                Message = InvalidModelState,
                MessageType = MessageType.ModelState,
                StatusCode = StatusCodes.Status400BadRequest,
                ModelStateErrors = _modelList,
            }, options);
            context.Result = new BadRequestObjectResult(exceptionResult);
        }

        return base.OnActionExecutionAsync(context, next);
    }
}