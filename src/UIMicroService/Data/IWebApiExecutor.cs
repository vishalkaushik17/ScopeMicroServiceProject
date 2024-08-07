

using GenericFunction.ResultObject;
using ModelTemplates.RequestNResponse.Accounts;

namespace ScopeCoreWebApp.Data
{
    public interface IWebApiExecutor
    {
        Task<T?> InvokeGet<T>(string relativeUrl);
        //Task<T?> InvokePost<T, U>(string relativeUrl, U obj, string username, string TokenSessionId, string TokenScopeId);
        //Task<T?> InvokePost<T, U>(string relativeUrl, U obj, string username, string TokenSessionId, string TokenScopeId, ITrace trace);
        Task<ResponseDto<ApplicationUserDtoModel>> InvokePost<T>(string relativeUrl, T loginModal, string username, string TokenSessionId, string TokenScopeId, ITrace trace);
    }
}