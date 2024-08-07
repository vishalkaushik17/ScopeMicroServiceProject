import * as _storage from "../BrowserStorage/tsStorage";
import * as _type from "../CustomType/customTypes";
import * as _events from "./EventRegistraion";
import * as _utils from "../Security/utils"
import * as _apiModule from "../ApiMethods/ApiMethod"
import { IdentityController } from "../Security/IdentityController";
import * as jQuery from 'jquery';
import 'select2';
let _userCache = true;
//it will reset css on aside bar li items.

export class ApplyEffectOnModuleSpecific {
    static ApplyCssOnModule(moduleId: string, innerModuleId: string): void {
        document.querySelectorAll('.active')?.forEach((activeElement) => { activeElement.classList.remove('active'); });
        document.querySelectorAll('.ActiveSideBarLiSpan')?.forEach((activeElement) => { activeElement.classList.remove('ActiveSideBarLiSpan'); });
        document.querySelectorAll(`.selectedBarIcon`).forEach(element => {
            element.classList.remove(`selectedBarIcon`);
        });
        document.querySelectorAll(`#LayoutUl li`).forEach((item) => {
            item.classList.remove("active");
            if (item.id === "LayoutUl") {
                item.classList.add("active");
                item.classList.add("treeview");
            } else {
                item.classList.remove("active");
            }
            if (item.id === innerModuleId) {
                document.querySelector(`#${moduleId} a span`)?.classList.add(`ActiveSideBarLiSpan`);
                item.querySelector(`a`)?.classList.add("ActiveAsideBarALink");
                item.querySelector(`a`)?.classList.add("selectedBarIcon");
                item.parentElement?.parentElement?.querySelector(`a i`)?.classList.add("selectedBarIcon");
                item.parentElement?.querySelector(`a i`)?.classList.add("selectedBarIcon");
            }
            else {
                item.querySelector(`a`)?.classList.remove("ActiveAsideBarALink");
            }
        })
    }
    static SpinnerLoading(effect: boolean): void {
        const spinner = document.querySelector('#LoadSpinner')! as HTMLDivElement;
        if (effect) {
            if (spinner) {
                spinner.style.display = 'block';
            }
        }
        else {
            spinner.style.display = 'none';
        }
    }
}

export class GenericModuleFunctions {
    static async SetIndexPageTitle(objPrimary: object | any) {
        const indexPageTitle = document.querySelector(`#indexPageTitle`)! as HTMLSpanElement;
        indexPageTitle.textContent = objPrimary.IndexTitle;
        indexPageTitle.innerHTML = indexPageTitle.textContent + ` <small>Index</small>`;
    }
    //when refresh button get clicked
    static async RefreshPageWithoutBrowserCache(objPrimary: object | any, clsPrimary: object | any) {
        _utils.GenericFunctionality.HtmlLogs(`RefreshPageWithoutBrowserCache`, `Execution started!.`, '');
        //first clear the browser cache

        (await _storage.CacheStorage).ClearModuleKeys([objPrimary.CacheKey]);
        //set default app config and app behaviour 
        (await _storage.CacheStorage).SetDefaultData();
        _events.EventBinding.SetApplicationState(_type.ApplicationState.Index);
        //default use cache is false here, we have to bind all the records from cache or db directly.
        //here we are disabling browser cache.
        let checkBrowserCache = objPrimary.UseBrowserCache;
        objPrimary.UseBrowserCache = false;
        objPrimary.UseRedisCache = true;
        const url = await _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/getall`, objPrimary.UseRedisCache)
        await GenericModuleFunctions.GetModelData(url, await _events.EventBinding.GetApplicationState(), objPrimary, clsPrimary);
        _utils.GenericFunctionality.HtmlLogs(`RefreshPageWithoutBrowserCache`, `Browser cache status`, checkBrowserCache);

        if (objPrimary.FetchAdditionalDataOnIndexPage) {
            objPrimary.FetchAdditionalDataOnIndexPage();
        }
        if (checkBrowserCache) {
            objPrimary.UseBrowserCache = true;
        } else {
            objPrimary.UseBrowserCache = false;
        }
    }

    static async ResetFormData() {
        _utils.GenericFunctionality.HtmlLogs(`ResetFormData`, `Resetting form data`, '');

        const form = document.querySelector('#modelForm')! as HTMLFormElement;
        const idElement = document.querySelector('#Id')! as HTMLInputElement;
        if (idElement) {
            idElement.value = ``;
        }
        form.reset();
        
        Array.from(form.elements).forEach((input) => {
            let element: any;

            switch (input.tagName) {
                case _type.TagName.INPUT:
                    element = input as HTMLInputElement;
                    // element.value = '';
                    GenericModuleFunctions.ReflectOnProperty(element, '')
                    break;
                case _type.TagName.TEXTAREA:
                    element = input as HTMLTextAreaElement;
                    // element.value = '';
                    GenericModuleFunctions.ReflectOnProperty(element, '')
                    break;
                case _type.TagName.SELECT:
                    element = input as HTMLSelectElement;
                    //checking element has select2 library has applied
                    if (element.hasAttribute(`data-select2-id`)) {
                        jQuery(`#${element.id}`).select2({
                            placeholder: "Select",
                            initSelection: function ( ) {
                            }
                        });
                    }
                    else{ //normal select 2 dropdown list
                        element.selectedIndex = 0;
                        element.selected = true;
    
                    }
                    break;

                default:
                    break;
            }
        });


    }

    //Set reflection on property which is assign using data attribute.
    static ReflectOnProperty(elementType: HTMLElement, value: string) {
        let reflectionType: HTMLElement | any;
        let reflectionOn = elementType.getAttribute(`reflectionOn`)!;

        if (reflectionOn) {
            reflectionType = document.getElementById(reflectionOn)! as HTMLElement;
        }
        //here tagName reflects type of the element
        switch (reflectionType?.tagName) {
            case _type.TagName.IMG:
                reflectionType['src'] = (value != '' || value != null) ? "data:image/png;base64," + value : '';
                reflectionType['alt'] = 'product image';
                break;
            case _type.TagName.SELECT:
                ///here do the things
                _utils.GenericFunctionality.SetSelectedValue(reflectionType, value)
                break;

            default:
                break;
        }
    }
    static async GetModelData(url: string, ApplicationState: _type.ApplicationState, objPrimary: _type.ModuleSpecificObjectType, clsPrimary: _type.ModuleSpecificObjectType | any): Promise<void> {

        let response: any;

        //checking data to get from bw cache
        _utils.GenericFunctionality.HtmlLogs(`GetModelData - Browser cache state`, 'UseBrowserCache', objPrimary.UseBrowserCache)

        if (objPrimary.UseBrowserCache) {
            if (ApplicationState !== _type.ApplicationState.Modify) {
                //getting data from browser storage.
                let data = await (await _storage.CacheStorage).GetDataFromLocalStorage(`${objPrimary.CacheKey}|${objPrimary.ClientId}`, objPrimary.UseBrowserCache) as Promise<_type.StorageData<_type.ApiResponse<object[] | any[]>>>;
                if ((await data)?.Data?.recordCount > 0) {
                    response = (await data) as _type.StorageData<_type.ApiResponse<object[]>>;
                }
                else {
                    //Get data from api when browser storage is null
                    response = await _apiModule.ApiFunctionality.GetApi<_type.ApiResponse<any>>(url, (await _events.EventBinding.GetApiToken()).Data, objPrimary.CacheKey, ApplicationState, true);

                }
            } else {
                //get data from api to modify or delete record.
                response = await _apiModule.ApiFunctionality.GetApi<_type.ApiResponse<any>>(url, (await _events.EventBinding.GetApiToken()).Data, objPrimary.CacheKey, ApplicationState, true);
                //_utils.GenericFunctionality.HtmlLogs(`GetModelData - Data fetched from api`,response)
            }
        }
        else {
            //Get data from direct api server
            response = await _apiModule.ApiFunctionality.GetApi<_type.ApiResponse<any>>(url, (await _events.EventBinding.GetApiToken()).Data, objPrimary.CacheKey, ApplicationState, true);
            //_utils.GenericFunctionality.HtmlLogs(`GetModelData - Data fetched from api`,response)

            if (ApplicationState === _type.ApplicationState.Index && objPrimary.UseBrowserCache) {
                response = await (await _storage.CacheStorage).GetDataFromLocalStorage(`${objPrimary.CacheKey}|${objPrimary.ClientId}`, objPrimary.UseBrowserCache) as Promise<_type.StorageData<_type.ApiResponse<object[] | any[]>>>;
            }
        }
        if (!response) {
            toastr.error("Server communication error!", "Error");
        }
        else {

            let responseData: _type.ApiResponse<any> = response.Data == undefined ? response : response.Data;
            switch (ApplicationState) {
                case _type.ApplicationState.Modify:
                    const form = new FormData(document.querySelector('#modelForm')! as HTMLFormElement);

                    //get all elements of form and make array of those.
                    let formElements = _utils.GenericFunctionality.getFormElementKeyValuePairs(form);
                    _utils.GenericFunctionality.HtmlLogs(`GetModelData`, 'Record modify', formElements)

                    //it will get all the key value pairs of result object
                    let resultObjectKeyValuePairs = _utils.GenericFunctionality.getKeyValuePairs(responseData.Result);

                    resultObjectKeyValuePairs.forEach((arrayElement) => {
                        let obj = formElements.find(o => o.key === arrayElement.key);

                        if (obj) {
                            let elementOfForm = document.querySelector(`[name='${obj.actualKey}']`)! as HTMLElement;
                            obj.htmlElement = elementOfForm;
                            switch (elementOfForm.tagName) {
                                case _type.TagName.INPUT:
                                    
                                    let InputElement = elementOfForm as HTMLInputElement;
                                    if (InputElement.type === 'checkbox'){
                                        debugger;
                                        if (arrayElement.value === 1 || arrayElement.value === true)
                                        {
                                            InputElement.checked = true;
                                        }
                                        else{
                                            InputElement.checked = false;
                                        }
                                    }else{
                                        InputElement.value = arrayElement.value;
                                    }
                                    GenericModuleFunctions.ReflectOnProperty(elementOfForm, InputElement.value)
                                    break;
                                case _type.TagName.TEXTAREA:
                                    let TextAreaElement = elementOfForm as HTMLTextAreaElement;
                                    TextAreaElement.value = arrayElement.value;
                                    GenericModuleFunctions.ReflectOnProperty(elementOfForm, TextAreaElement.value)
                                    break;
                                case _type.TagName.IMG:
                                    let imageElement = elementOfForm as HTMLImageElement;
                                    imageElement.src = "data:image/png;base64," + arrayElement.value;
                                    GenericModuleFunctions.ReflectOnProperty(elementOfForm, imageElement.src)
                                    break;
                                case _type.TagName.SELECT:
                                    let selectElement = elementOfForm as HTMLSelectElement;
                                    selectElement.value = arrayElement.value;

                                    GenericModuleFunctions.ReflectOnProperty(elementOfForm, selectElement.value)
                                    break;

                                default:
                                    break;
                            }

                        }
                    })
                    await _events.EventBinding.ResetOperationButtons(ApplicationState);
                    break;
                case _type.ApplicationState.Index:
                default:

                    //when data is available then fill the index page.
                    if (responseData?.recordCount > 0) {
                        await clsPrimary.FillDataOnIndexTable((responseData.Result));
                    }

                    break;
            }
            // let logsModal = document.querySelector(`#logs`)! as HTMLDivElement;
            // if (logsModal) {

            //     if (response?.Data) {
            //         logsModal.innerHTML = logsModal.innerHTML + response.Data.log;
            //     } else {
            //         logsModal.innerHTML = logsModal.innerHTML + response.log;
            //     }
            // }

        }
    }
    static async GetAdditionalModelData(url: string, ApplicationState: _type.ApplicationState, objPrimary: _type.ModuleSpecificObjectType, additionalDataFetchingPath: _type.AdditionalDataFetchingPath, showAlert: boolean): Promise<any> {
        let response: any;
        //checking data to get from bw cache
        if (objPrimary.UseBrowserCache) {
            _utils.GenericFunctionality.HtmlLogs(`GetAdditionalModelData - Browser cache state`, 'UseBrowserCache', objPrimary.UseBrowserCache)
            _utils.GenericFunctionality.HtmlLogs(`GetAdditionalModelData -`, `Application state`, ApplicationState)

            debugger;
            if (ApplicationState !== _type.ApplicationState.Modify) {
                //getting data from browser storage.
                let cacheResponseData = await (await _storage.CacheStorage).GetDataFromLocalStorage(`${additionalDataFetchingPath.CacheKey}|${objPrimary.ClientId}`, objPrimary.UseBrowserCache) as Promise<_type.StorageData<_type.ApiResponse<object[] | any[]>>>;
                response = await (await _storage.CacheStorage).GetDataFromLocalStorage(`${additionalDataFetchingPath.CacheKey}|${objPrimary.ClientId}`, objPrimary.UseBrowserCache) as Promise<_type.StorageData<_type.ApiResponse<object[] | any[]>>>;
                debugger;
                if (response && (await response).Data.recordCount > 0) {
                    debugger;
                    response = (await response) as _type.StorageData<_type.ApiResponse<object[]>>;
                    //_utils.GenericFunctionality.HtmlLogs(`Fetched Data from browser storage`,response)
                }
                else {
                    debugger;
                    //Get data from api when browser storage is null
                    response = await _apiModule.ApiFunctionality.GetApi<_type.ApiResponse<any>>(url, (await _events.EventBinding.GetApiToken()).Data, additionalDataFetchingPath.CacheKey, ApplicationState, showAlert);
                    //_utils.GenericFunctionality.HtmlLogs(`Fetched Data from api`,response)

                }
            } else {
                //get data from api to modify or delete record.
                debugger;
                response = await _apiModule.ApiFunctionality.GetApi<_type.ApiResponse<any>>(url, (await _events.EventBinding.GetApiToken()).Data, additionalDataFetchingPath.CacheKey, ApplicationState, showAlert);
                // _utils.GenericFunctionality.HtmlLogs(`Fetched Data from api`,`With application state: ${ApplicationState} - ${response}`)
            }
        }
        else {
            debugger;
            //Get data from direct api server and store in browser cache
            response = await _apiModule.ApiFunctionality.GetApi<_type.ApiResponse<any>>(url, (await _events.EventBinding.GetApiToken()).Data, additionalDataFetchingPath.CacheKey, ApplicationState, showAlert);

            if (ApplicationState === _type.ApplicationState.Index && objPrimary.UseBrowserCache) {
                response = await (await _storage.CacheStorage).GetDataFromLocalStorage(`${additionalDataFetchingPath.CacheKey}|${objPrimary.ClientId}`, objPrimary.UseBrowserCache) as Promise<_type.StorageData<_type.ApiResponse<object[] | any[]>>>;
            }
        }
        if (!response) {
            toastr.error("Server communication error!", "Error");
        }
        else {
            return response.Data == undefined ? response : response.Data;;
        }
    }
}
