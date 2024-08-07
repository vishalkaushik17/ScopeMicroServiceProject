"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.jsDashBoardController = exports.JSLayoutPageComponent = void 0;
var _Modules = require("./SPAModuleList");
/*import * as pController from './alert.js'*/
var currentUrl = "".concat(window.location.protocol, "//").concat(window.location.host);
var btnCheckData = document.getElementById('btnCheckData');
//put first if need to pass as param in another iife function
var JSFetchPartialViewService = function () {
    var _this = this;
    //Here we have to pass RenderViewAsHtmlString, PrintError as a function parameters
    var FetchPartialViewAsString = function (currentUrl, path, RenderViewAsHtmlString, PrintError) {
        fetch("".concat(currentUrl).concat(path), {
            method: 'GET',
            headers: ApiHeaders
        })
            .then(JsonResponse).then(RenderViewAsHtmlString).catch(PrintError);
    };
    var ApiHeaders = {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    };
    var JsonResponse = function (response) { return __awaiter(_this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, response.json()];
                case 1: return [2 /*return*/, _a.sent()];
            }
        });
    }); };
    var TextResponse = function (response) { return __awaiter(_this, void 0, void 0, function () {
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, response.text()];
                case 1: return [2 /*return*/, _a.sent()];
            }
        });
    }); };
    return {
        FetchPartialViewAsString: FetchPartialViewAsString
    };
}();
exports.JSLayoutPageComponent = function (jsFetchPartialViewService) {
    //Find page navigation links and register event listener to link element
    var FindSPANavigationLinks = function () {
        var navLinkElements = Array.from(document.querySelectorAll('a')).filter(function (element) {
            return element.hasAttribute('Partial_Nav_Link');
        });
        navLinkElements.forEach(function (e) {
            e.addEventListener('click', BindNavigationLinkOnPageNavigationElement);
        });
    };
    var callbackFunc = function () {
        return window.document.location.href;
    };
    //Generate navigation link for SPA operation.
    var BindNavigationLinkOnPageNavigationElement = function () {
        var link = this.href;
        var indexValue = link.indexOf('#') + 1;
        if (indexValue > 0) {
            var lastPath = link.substring(indexValue);
            //When page navigation link get clicked, it will fetch api data as per page link # tag
            jsFetchPartialViewService.FetchPartialViewAsString(currentUrl, lastPath, RenderViewAsHtmlString, PrintError);
        }
    };
    //fetch view from respective action method and render on html page.
    var RenderViewAsHtmlString = function (result) {
        var partialId = document.getElementById('partial');
        if (result.status === true) {
            partialId.innerHTML = result.htmlResponse;
            //now we are calling event binding for all elements which are rendered as innerHtml.
            _Modules.SPAModules(result.staticFilePath, result.fullViewName, result.isCssModuleAvailable, result.isJsModuleAvailable, result.jsVersion, result.cssVersion);
        }
        return;
    };
    var RenderViewAsJSON = function (result) {
        var partialId = document.getElementById('partial');
        if (result.status === true) {
            if (result.responseType === 'JSON') {
                var element = document.getElementById('renderData');
                element.innerHTML = result.jsonResponse.userName;
            }
            PrintSuccess(result);
            return;
        }
        return;
    };
    var PrintError = function (error) {
        // swal({
        //     title: "Error!",
        //     text: error.message,
        //     icon: "warning",
        //     buttons: false,
        //     type: "warning",
        //     timer: 4000
        // });
        console.log(error);
    };
    var PrintSuccess = function (success) {
        if (success.showMessage) {
            // swal({
            //     title: "Success!",
            //     text: success.message,
            //     icon: "success",
            //     buttons: false,
            //     type: "success",
            //     timer: 4000
            // });
        }
    };
    return {
        FindSPANavigationLinks: FindSPANavigationLinks,
        RenderViewAsHtmlString: RenderViewAsHtmlString,
        RenderViewAsJSON: RenderViewAsJSON,
        PrintSuccess: PrintSuccess,
        PrintError: PrintError,
        BindNavigationLinkOnPageNavigationElement: BindNavigationLinkOnPageNavigationElement
    };
}(JSFetchPartialViewService);
exports.jsDashBoardController = function (jsFetchPartialViewService, jsLayoutPageComponent) {
    var AddClickEventHandlerOnClickButton = function (element) {
        if (element) {
            element.addEventListener('click', CheckData);
        }
    };
    var CheckData = function () {
        jsFetchPartialViewService.FetchPartialViewAsString(currentUrl, '/Home/CheckData', jsLayoutPageComponent.RenderViewAsJSON, jsLayoutPageComponent.PrintError);
    };
    var clickOnTest = function () {
        fetch("".concat(currentUrl, "/home/test")).then(function (data) {
            if (data) {
                return data.text();
            }
            return null;
        }).then(function (result) {
            if (result) {
                var partialId = document.getElementById('partial');
                if (partialId) {
                    partialId.innerHTML = result;
                    var btnEmployee = document.getElementById('btnEmployee');
                    var btnClear = document.getElementById('btnClear');
                    if (btnEmployee) {
                        RegisterClickEventOnTheFly(btnEmployee);
                        RegisterClickEventOnTheFlyForClear(btnClear);
                    }
                }
                else {
                    console.log('div not available for rendering!!!');
                }
            }
        });
    };
    var RegisterClickEventOnTheFlyForClear = function (btnClear) {
        btnClear.addEventListener('click', ClearField);
    };
    var RegisterClickEventOnTheFly = function (btnEmployee) {
        btnEmployee.addEventListener('click', RenderEmployeeDataOnFly);
    };
    var ClearField = function () {
        var elementId = document.getElementById('EmployeeName');
        if (elementId) {
            elementId.innerHTML = '';
            document.getElementById('empTitle').style.display = 'none';
        }
    };
    var RenderEmployeeDataOnFly = function () {
        var element = document.getElementById('EmpDataHiddenField');
        if (element) {
            var elementValue = element.getAttribute('EmpAttr');
            if (elementValue && elementValue !== '') {
                var jObject = JSON.parse(elementValue);
                element.setAttribute('EmpAttr', '');
                var elementId = document.getElementById('EmployeeName');
                if (element) {
                    elementId.innerHTML = jObject.Id + ' ' + jObject.Name;
                }
            }
        }
    };
    return {
        ClickOnTest: clickOnTest,
        AddClickEventOnButton: AddClickEventHandlerOnClickButton,
    };
}(JSFetchPartialViewService, exports.JSLayoutPageComponent);
// JSLayoutPageComponent.FindSPANavigationLinks();
// jsHomeControllerComponent.AddClickEventOnButton(btnCheckData);
// var loadJS = function (url, implementationCode, location) {
//     //url is URL of external file, implementationCode is the code
//     //to be called from the file, location is the location to 
//     //insert the <script> element
//     var scriptTag = document.createElement('script');
//     scriptTag.src = url;
//     scriptTag.onload = implementationCode;
//     scriptTag.onreadystatechange = implementationCode;
//     location.appendChild(scriptTag);
// };
/*loadJS('yourcode.js', yourCodeToBeCalled, document.body);*/
// function al() {
//     alert(2);
// }
