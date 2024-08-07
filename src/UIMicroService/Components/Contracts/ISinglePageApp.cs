namespace ScopeCoreWebApp.Components.Contracts
{

    /// <summary>
    /// Contract for rendering SPA page.
    /// </summary>
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
