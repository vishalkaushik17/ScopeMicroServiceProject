import * as _toastr from "toastr";
import * as _type from "../../CustomType/customTypes"
import * as _moduleFunctions from "../../Modules/ModuleFunctions";
export class Notify{
     constructor(){
        toastr.options = {
            "closeButton": true,
            "newestOnTop": false,
            "progressBar": true,
            "preventDuplicates": true,
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut",
            "timeOut" : 1000,
            "showDuration": 300,
            "hideDuration": 200,
            "extendedTimeOut": 500,
            iconClasses: {
                error: 'fas fa-trash',
                info: 'fa fa-info',
                success: 'fas fa-check',
                warning: 'something',
            },
        }
    }
    static ShowErrorAlert(alertMessage:string){
        toastr.options.timeOut = 10000;
        toastr.error(alertMessage, "Error");
    }
    static Alert(apiResponseData:_type.ApiResponse<object|unknown>):void {
        if (Array.isArray(apiResponseData.Result)) {
            if (apiResponseData.recordCount > 0)
                toastr.info('Record populated : ' + apiResponseData.recordCount, apiResponseData.status);
            
        }

        switch (apiResponseData.messageType) {
            case _type.MessageType.Information:
            case _type.MessageType.Message:
                toastr.options.timeOut = 1000;
                toastr.success(apiResponseData.message, apiResponseData.status);
                
                break;
            case  _type.MessageType.Exception:
            case  _type.MessageType.ExceptionCache:
                toastr.options.timeOut = 10000;
                toastr.error(apiResponseData.message, apiResponseData.status);
                break;
            case  _type.MessageType.Warning:
            case  _type.MessageType.ModelState:
            case  _type.MessageType.DefaultRecordWarning:
                toastr.options.timeOut = 10000;
                if (apiResponseData.messageType ==  _type.MessageType.ModelState) {
                    let strError = "";
                    apiResponseData.modelStateErrors.forEach(function (item) {
                        strError += item + '<br>';
                    });
                    
                    toastr.warning(strError, apiResponseData.status);
                    break;
                }
                toastr.error(apiResponseData.message, apiResponseData.status);
                break;
            default:
                toastr.error(apiResponseData.message, apiResponseData.status    );
                break;
        }
        _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
    };    
}
