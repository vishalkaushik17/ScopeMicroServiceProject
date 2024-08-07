import * as _Modules from "./SPAModuleList";
import * as _spa from "./../UIComponents/SPALink"
import * as _notifications from "./Notifications/NotificationComponents"
import * as _utils from "./../Security/utils";
/*import * as pController from './alert.js'*/
const currentUrl = `${window.location.protocol}//${window.location.host}`;

//put first if need to pass as param in another iife function
const JSFetchPartialViewService = function () {
    _utils.GenericFunctionality.HtmlLogs(`JSFetchPartialViewService`, `Execution Started`,'');
debugger;
    //Here we have to pass RenderViewAsHtmlString, PrintError as a function parameters
    const FetchPartialViewAsString = function (currentUrl:string, path:string, RenderViewAsHtmlString, PrintError) {
        _utils.GenericFunctionality.HtmlLogs(`FetchPartialViewAsString`, `ApiHeaders`,ApiHeaders);
        _utils.GenericFunctionality.HtmlLogs(`FetchPartialViewAsString`, `currentUrl & path`,`${currentUrl}${path}`);
        fetch(`${currentUrl}${path}`, {
            method: 'GET',
            headers: ApiHeaders
        })
        .then(JsonResponse).then(RenderViewAsHtmlString).catch(PrintError);
    };
    const ApiHeaders = {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    };
    const JsonResponse = async (response) => {
        _utils.GenericFunctionality.HtmlLogs(`FetchPartialViewAsString`, `json response`,``);
        return await response.json();
    };
    const TextResponse = async (response) => {
        _utils.GenericFunctionality.HtmlLogs(`FetchPartialViewAsString`, `text response`,``);

        return await response.text();
    };
    return {
        FetchPartialViewAsString: FetchPartialViewAsString
    }
}();
export const JSLayoutPageComponent = function (jsFetchPartialViewService) {
    
    //Find page navigation links and register event listener to link element
    const FindSPANavigationLinks = function () {
        const navLinkElements = Array.from(document.querySelectorAll('a')).filter(element => {
            return element.hasAttribute('Partial_Nav_Link');
        });
        
        navLinkElements.forEach((e) => {
             e.addEventListener('click', BindNavigationLinkOnPageNavigationElement)
        });
    };
    const callbackFunc = function():any{
        return window.document.location.href;
    }
    //Generate navigation link for SPA operation.
    const BindNavigationLinkOnPageNavigationElement = function (this):void {
        const aLink = this as HTMLAnchorElement;
        let link:string = this.href;
        const indexValue = link.indexOf('#') + 1;
        // aLink.style.color = 'red';
        // aLink.style.fontWeight = "bold";

        // const selectedNode = (aLink.parentElement?.parentElement?.parentElement as HTMLElement).querySelector('a span')! as  HTMLSpanElement;;
        // selectedNode.style.color = "red"

        if (indexValue > 0) {
            const lastPath =  link.substring(indexValue);
            //When page navigation link get clicked, it will fetch api data as per page link # tag
            jsFetchPartialViewService.FetchPartialViewAsString(currentUrl, lastPath, RenderViewAsHtmlString, PrintError);
         
        }
    };
    
    //fetch view from respective action method and render on html page.
    const RenderViewAsHtmlString = (result) => {
        
        
        const partialId = document.getElementById('partial')!;
        const  logsModal = document.querySelector(`#logs`)! as HTMLDivElement;
        if (logsModal){
            logsModal.innerHTML = logsModal.innerHTML + result.logs;
        }
        if (result.status === true) {
            partialId.innerHTML = result.htmlResponse;
            //now we are calling event binding for all elements which are rendered as innerHtml.
            _Modules.SPAModules(result.staticFilePath, result.fullViewName,result.isCssModuleAvailable,result.isJsModuleAvailable,result.jsVersion,result.cssVersion);
            _spa.JSLayoutPageComponent.FindSPANavigationLinks();
        }
        else{
            if(result.showMessage)
            {
                _notifications.Notify.ShowErrorAlert(result.message);
            }
        }
        return;
    }
    const RenderViewAsJSON = (result) => {
        const partialId = document.getElementById('partial');
        if (result.status === true) {
            if (result.responseType === 'JSON') {
                const element = document.getElementById('renderData')!;
                element.innerHTML = result.jsonResponse.userName;
            }
            PrintSuccess(result);
            return;
        }
        return;
    }
    const PrintError = (error) => {
        
        // swal({
        //     title: "Error!",
        //     text: error.message,
        //     icon: "warning",
        //     buttons: false,
        //     type: "warning",
        //     timer: 4000
        // });
        console.log(error);
    }
    const PrintSuccess = (success) => {
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
    }

    return {
        FindSPANavigationLinks: FindSPANavigationLinks,
        RenderViewAsHtmlString: RenderViewAsHtmlString,
        RenderViewAsJSON: RenderViewAsJSON,
        PrintSuccess: PrintSuccess,
        PrintError: PrintError,
        BindNavigationLinkOnPageNavigationElement: BindNavigationLinkOnPageNavigationElement
    }
}(JSFetchPartialViewService);

export const jsDashBoardController = function (jsFetchPartialViewService, jsLayoutPageComponent) {
   
    const AddClickEventHandlerOnClickButton = function (element) {
        if (element) {
            element.addEventListener('click', CheckData);
        }
    };
    const CheckData = function () {
        jsFetchPartialViewService.FetchPartialViewAsString(currentUrl, '/Home/CheckData', jsLayoutPageComponent.RenderViewAsJSON, jsLayoutPageComponent.PrintError);
    };
    const clickOnTest = function () {
        fetch(`${currentUrl}/home/test`).then(data => {
            if (data) {
                return data.text();
            }
            return null;
        }).then(result => {
            if (result) {
                const partialId = document.getElementById('partial');
                if (partialId) {
                    partialId.innerHTML = result;
                    const btnEmployee = document.getElementById('btnEmployee');
                    const btnClear = document.getElementById('btnClear');
                    if (btnEmployee) {
                        RegisterClickEventOnTheFly(btnEmployee);
                        RegisterClickEventOnTheFlyForClear(btnClear);
                    }

                } else {
                    console.log('div not available for rendering!!!')
                }
            }
        })
    };
    const RegisterClickEventOnTheFlyForClear = function (btnClear) {
        btnClear.addEventListener('click', ClearField);
    };
    const RegisterClickEventOnTheFly = function (btnEmployee) {
        btnEmployee.addEventListener('click', RenderEmployeeDataOnFly);
    };
    const ClearField = function () {

            const elementId = document.getElementById('EmployeeName')!;
            if (elementId) {
                elementId.innerHTML = '';
                document.getElementById('empTitle')!.style.display = 'none';
            }
    };
    const RenderEmployeeDataOnFly = function () {
        const element = document.getElementById('EmpDataHiddenField');
        
        if (element) {
            const elementValue = element.getAttribute('EmpAttr');
            if (elementValue && elementValue !== '') {
                const jObject = JSON.parse(elementValue);
                element.setAttribute('EmpAttr', '');
                const elementId = document.getElementById('EmployeeName')!;

                if (element) {
                    elementId.innerHTML = jObject.Id + ' ' + jObject.Name;
                }

            }
        }
    };
    return {
        ClickOnTest: clickOnTest,
        AddClickEventOnButton: AddClickEventHandlerOnClickButton,
    }
}(JSFetchPartialViewService, JSLayoutPageComponent);

// JSLayoutPageComponent.FindSPANavigationLinks();




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
