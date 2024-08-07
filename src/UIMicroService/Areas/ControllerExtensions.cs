using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ScopeCoreWebApp.Areas
{
    //public interface IViewRenderService
    //{
    //    Task<string> RenderToStringAsync(string viewName, object model);
    //}

    //public class ViewRenderService : IViewRenderService
    //{
    //    private readonly IRazorViewEngine _razorViewEngine;
    //    private readonly ITempDataProvider _tempDataProvider;
    //    private readonly IServiceProvider _serviceProvider;

    //    public ViewRenderService(IRazorViewEngine razorViewEngine,
    //        ITempDataProvider tempDataProvider,
    //        IServiceProvider serviceProvider)
    //    {
    //        _razorViewEngine = razorViewEngine;
    //        _tempDataProvider = tempDataProvider;
    //        _serviceProvider = serviceProvider;
    //    }

    //    public async Task<string> RenderToStringAsync(string viewName, object model)
    //    {
    //        var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
    //        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

    //        using (var sw = new StringWriter())
    //        {
    //            var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

    //            if (viewResult.View == null)
    //            {
    //                throw new ArgumentNullException($"{viewName} does not match any available view");
    //            }

    //            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
    //            {
    //                Model = model
    //            };

    //            var viewContext = new ViewContext(
    //                actionContext,
    //                viewResult.View,
    //                viewDictionary,
    //                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
    //                sw,
    //                new HtmlHelperOptions()
    //            );

    //            await viewResult.View.RenderAsync(viewContext);
    //            return sw.ToString();
    //        }
    //    }
    //}
    //public static class ControllerExtensions1
    //{
    //    public static async Task<string> RenderViewToStringAsync(
    //string viewName, object model,
    //ControllerContext controllerContext,
    //bool isPartial = false)
    //    {
    //        var actionContext = controllerContext as ActionContext;

    //        var serviceProvider = controllerContext.HttpContext.RequestServices;
    //        var razorViewEngine = serviceProvider.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
    //        var tempDataProvider = serviceProvider.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

    //        using (var sw = new StringWriter())
    //        {
    //            var viewResult = razorViewEngine.FindView(actionContext, viewName, !isPartial);

    //            if (viewResult?.View == null)
    //                throw new ArgumentException($"{viewName} does not match any available view");

    //            var viewDictionary =
    //                new ViewDataDictionary(new EmptyModelMetadataProvider(),
    //                    new ModelStateDictionary())
    //                { Model = model };

    //            var viewContext = new ViewContext(
    //                actionContext,
    //                viewResult.View,
    //                viewDictionary,
    //                new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
    //                sw,
    //                new HtmlHelperOptions()
    //            );

    //            await viewResult.View.RenderAsync(viewContext);
    //            return sw.ToString();
    //        }
    //    }
    //    /// <summary>
    //    /// Render a partial view to string.
    //    /// </summary>
    //    /// <typeparam name="TModel"></typeparam>
    //    /// <param name="controller"></param>
    //    /// <param name="viewNamePath"></param>
    //    /// <param name="model"></param>
    //    /// <returns></returns>
    //    public static async Task<string> RenderViewToStringAsync<TModel>(this Controller controller, string viewNamePath, TModel model)
    //    {
    //        if (string.IsNullOrEmpty(viewNamePath))
    //            viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

    //        controller.ViewData.Model = model;

    //        using (StringWriter writer = new StringWriter())
    //        {
    //            try
    //            {
    //                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

    //                ViewEngineResult viewResult = null;

    //                if (viewNamePath.EndsWith(".cshtml"))
    //                    viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
    //                else
    //                    viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

    //                if (!viewResult.Success)
    //                    return $"A view with the name '{viewNamePath}' could not be found";

    //                ViewContext viewContext = new ViewContext(
    //                    controller.ControllerContext,
    //                    viewResult.View,
    //                    controller.ViewData,
    //                    controller.TempData,
    //                    writer,
    //                    new HtmlHelperOptions()
    //                );

    //                await viewResult.View.RenderAsync(viewContext);

    //                return writer.GetStringBuilder().ToString();
    //            }
    //            catch (Exception exc)
    //            {
    //                return $"Failed - {exc.Message}";
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Render a partial view to string, without a model present.
    //    /// </summary>
    //    /// <typeparam name="TModel"></typeparam>
    //    /// <param name="controller"></param>
    //    /// <param name="viewNamePath"></param>
    //    /// <returns></returns>
    //    public static async Task<string> RenderViewToStringAsync(this Controller controller, string viewNamePath)
    //    {
    //        if (string.IsNullOrEmpty(viewNamePath))
    //            viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

    //        using (StringWriter writer = new StringWriter())
    //        {
    //            try
    //            {
    //                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

    //                ViewEngineResult viewResult = null;

    //                if (viewNamePath.EndsWith(".cshtml"))
    //                    viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
    //                else
    //                    viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

    //                if (!viewResult.Success)
    //                    return $"A view with the name '{viewNamePath}' could not be found";

    //                ViewContext viewContext = new ViewContext(
    //                    controller.ControllerContext,
    //                    viewResult.View,
    //                    controller.ViewData,
    //                    controller.TempData,
    //                    writer,
    //                    new HtmlHelperOptions()
    //                );

    //                await viewResult.View.RenderAsync(viewContext);

    //                return writer.GetStringBuilder().ToString();
    //            }
    //            catch (Exception exc)
    //            {
    //                return $"Failed - {exc.Message}";
    //            }
    //        }
    //    }

    //}

    public static class ControllerExtensions
    {
        public static async Task<string> RenderViewAsync<TModel>(Controller controller, string viewName, TModel model, bool partial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            if (model != null)
                controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

                if (viewResult.Success == false)
                {
                    return $"A view with the name {viewName} could not be found";
                }

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString() + "<div class=\"floting-sidebar\">";
            }
        }
    }
}
