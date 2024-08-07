"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Notify = void 0;
var Notify = /** @class */ (function () {
    function Notify() {
        toastr.options = {
            "closeButton": true,
            "newestOnTop": false,
            "progressBar": true,
            "preventDuplicates": true,
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut",
            "timeOut": 5000,
            "showDuration": 300,
            "hideDuration": 1000,
            "extendedTimeOut": 1000,
            iconClasses: {
                error: 'fas fa-trash',
                info: 'fa fa-info',
                success: 'fas fa-check',
                warning: 'something',
            },
        };
    }
    Notify.Alert = function (object) {
        if (Array.isArray(object.Result)) {
            toastr.info('Record populated : ' + object.recordCount, object.status);
        }
        switch (object.messageType) {
            case "Information":
            case "Message":
                toastr.success(object.message, object.status);
                break;
            case "Error":
            case "Exception":
                toastr.error(object.message, object.status);
                break;
            case "Warning":
            case "ModelState":
            case "DefaultRecordWarning":
                if (object.messageType == "ModelState") {
                    var strError_1 = "";
                    object.modelStateErrors.forEach(function (item) {
                        strError_1 += item + '<br>';
                    });
                    toastr.warning(strError_1, object.status);
                    break;
                }
                toastr.error(object.message, object.status);
                break;
            default:
                toastr.error(object.message, object.status);
                break;
        }
    };
    ;
    return Notify;
}());
exports.Notify = Notify;
