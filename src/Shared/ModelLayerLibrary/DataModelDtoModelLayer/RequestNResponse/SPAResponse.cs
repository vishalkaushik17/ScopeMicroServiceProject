using GenericFunction;
using GenericFunction.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
namespace ModelTemplates.RequestNResponse
{
    public class SPAResponse<T>
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SPAResponse(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            StaticFilePath = AppSettingsConfigurationManager.AppSetting.GetValue<string>("StaticFilesPathForHTMLRendering");
        }
        public string? Logs { get; set; }
        public string StaticFilePath { get; set; }
        public string jsVersion { get; set; }
        public string cssVersion { get; set; }
        public SPAResponse()
        {
            StaticFilePath = AppSettingsConfigurationManager.AppSetting.GetValue<string>("StaticFilesPathForHTMLRendering");
        }
        public string GetFileRootPath()
        {
            return Path.Combine(_hostingEnvironment.WebRootPath, StaticFilePath);
        }
        public string GetRootFile()
        {
            return Path.Combine(_hostingEnvironment.WebRootPath, StaticFilePath, FullViewName);
        }
        public string GetJsVersion()
        {
            if (IsJsModuleAvailable)
            {

                string staticFilePath = AppSettingsConfigurationManager.AppSetting.GetValue<string>("StaticFilesPathForVersioning");
                string filepath = Path.Combine(_hostingEnvironment.WebRootPath, staticFilePath, @$"scripts\{SupportedFileName}.min.js");
                jsVersion = filepath.CalculateMd5();
                return jsVersion;
            }
            return DateTime.Now.ToString("ddMMMyyyy");
        }
        public string GetCssVersion()
        {
            if (IsCssModuleAvailable)
            {
                string staticFilePath = AppSettingsConfigurationManager.AppSetting.GetValue<string>("StaticFilesPathForVersioning");
                string filepath = Path.Combine(_hostingEnvironment.WebRootPath, staticFilePath, @$"styles\{SupportedFileName}.css");
                cssVersion = filepath.CalculateMd5();
                return cssVersion;
            }
            return DateTime.Now.ToString("ddMMMyyyy");
        }

        public void InitFileVersion()
        {
            GetJsVersion();
            GetCssVersion();
        }
        public string HtmlResponse { get; set; }
        public string FullViewName { get; set; }
        public string SupportedFileName { get; set; }
        public string ViewName { get; set; }
        public T JSONResponse { get; set; }
        public SPAResponseType ResponseType { get; set; } = SPAResponseType.JSON;
        public SPAMessageType MessageType { get; set; } = SPAMessageType.INFO;
        public bool Status { get; set; } = true;
        public string Message { get; set; }
        public bool ShowMessage { get; set; } = false;
        public bool IsCssModuleAvailable { get; set; } = false;
        public bool IsJsModuleAvailable { get; set; } = false;
    }
}
