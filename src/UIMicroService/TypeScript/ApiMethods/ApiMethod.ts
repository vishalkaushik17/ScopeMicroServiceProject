import * as _storage from "../BrowserStorage/tsStorage"
import * as _security from "../Security/utils.js"
import * as _type from "../CustomType/customTypes"
import * as _notifications from "../UIComponents/Notifications/NotificationComponents"
import * as _moduleFunctions from "../Modules/ModuleFunctions";
import * as _utils from "../Security/utils";
export class ApiFunctionality {

    /**
     * This method initialized dictionaries for api modules to fetch data from api server.
     */
    public static InitApiDictionary(dictionaryObjectForApiModuleCall?: object): object {
        _utils.GenericFunctionality.HtmlLogs(`InitApiDictionary - `, "Api dictionary initializing started", '');
        dictionaryObjectForApiModuleCall = {};
        //here ApiModule mean api url on which api get hit.
        dictionaryObjectForApiModuleCall[_type.ApiModuleKey.Department] = { ApiModule: "EmployeeDepartment", CacheKey: "DepartmentModule", FieldName: "Department", FieldNameReflectedId: `DepartmentId`, ResultPropertyName: "name", ResultPropertyId: "id", FieldType: _type.ElementType.LIST };
        dictionaryObjectForApiModuleCall[_type.ApiModuleKey.Designation] = { ApiModule: "EmployeeDesignation", CacheKey: "DesignationModule", FieldName: "Designation", FieldNameReflectedId: `DesignationId`, ResultPropertyName: "name", ResultPropertyId: "id", FieldType: _type.ElementType.LIST };
        dictionaryObjectForApiModuleCall[_type.ApiModuleKey.Bank] = { ApiModule: "EmployeeBank", CacheKey: "BankModule", FieldName: "Bank", FieldNameReflectedId: `BankId`, ResultPropertyName: "name", ResultPropertyId: "id", FieldType: _type.ElementType.LIST };
        dictionaryObjectForApiModuleCall[_type.ApiModuleKey.Employee] = { ApiModule: "Employee", CacheKey: "EmployeeModule", FieldName: "Employee", FieldNameReflectedId: `EmployeeId`, ResultPropertyName: "name", ResultPropertyId: "id", FieldType: _type.ElementType.LIST };
        _utils.GenericFunctionality.HtmlLogs(`InitApiDictionary - `, "Api dictionary initialized", '');
        return dictionaryObjectForApiModuleCall;

    }


    public static async GetApi<T>(url: string, token: _type.TokenObject, moduleCacheKey: string, ApplicationState: _type.ApplicationState, showAlert: boolean): Promise<T> {
        _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(true);
        _utils.GenericFunctionality.HtmlLogs(`GetApi - `, "URL", url);
        try {
            return await <T>fetch(url, {
                method: 'GET',
                headers: {
                    // "UserName": token.UN,
                    "Authorization": token.TK,
                    'Content-Type': 'application/json; charset=utf-8',
                    // "TokenSessionId": token.SessionId,
                    // "TokenScopeId": token.ScopeId,
                    // "ClientId": token.ClientId,
                    // "UserId": token.UserId,
                }
            })
                .then(async response => {
                    return response.json() as Promise<T>;
                    // _utils.GenericFunctionality.HtmlLogs(`GetApi - `, "Raw response", jsonRes);
                    
                    
                    // if (Number(response.status) >= 500 && Number(response.status) <= 599) {
                    //     throw new Error(`Code: ${response.status} Authentication error!`)
                    // }
                    // if (Number(response.status) >= 400 && Number(response.status) <= 499) {

                    //     console.log(response);
                    //     throw new Error(`Code: ${response.status} Service not found!`)
                    // }
                    // if (!response.ok) {
                    //     throw new Error(`Code: ${response.status} Something went wrong!`)
                    // }


                    // response.json();
                })
                .then(async jsonResponse => {
                    // console.log(await jsonResponse);
                    let responseObj = jsonResponse as _type.ApiResponse<any>;
                    let logs = responseObj.log;
                    responseObj.log = '';
                    _utils.GenericFunctionality.HtmlLogs(`GetApi - `, "Application State", ApplicationState);
                    _utils.GenericFunctionality.HtmlLogs(`GetApi - Data fetched from api url:`, url, responseObj);
                    responseObj.log = logs;
                    //when user want to modify record do not show sweet alert
                    if (ApplicationState !== _type.ApplicationState.Modify) {
                        if (showAlert) {
                            _notifications.Notify.Alert(jsonResponse as _type.ApiResponse<unknown>);
                        } else {
                            _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
                        }
                    }
                    else if (ApplicationState === _type.ApplicationState.Modify) {
                        //when record is going to modify, hide loader, because SpinnerLoading hide/show is written in Notify.Alert method.
                        _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
                    }
                    _utils.GenericFunctionality.HtmlApiLogs(`Api Logs for url: ${url}`, `${responseObj.log}`);
                    if (ApplicationState != _type.ApplicationState.Modify) {
                        responseObj = jsonResponse as _type.ApiResponse<object[]>;
                        if (responseObj.Result !== null) {
                            (await _storage.CacheStorage).SetDataToLocalStorage((responseObj as _type.ApiResponse<object[]>), moduleCacheKey, `${moduleCacheKey}|${token.ClientId}`, 30);
                        }

                    }
                    return <T>jsonResponse;
                })
                .catch(error => {
                    _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
                    _utils.GenericFunctionality.HtmlLogs(`GetApi`, `api error ${error}`, 'Api communication issue.');

                });
        } catch (error) {

            _utils.GenericFunctionality.HtmlLogs(`GetApi`, `exception`, error);
        }
        return Promise.reject<T>();
    }


    public static async SaveApiWithFile<T>(url: string, token: _type.TokenObject, DataToSave: FormData): Promise<T> {
        _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(true);
        _utils.GenericFunctionality.HtmlLogs("Request object: ",JSON.stringify(DataToSave));
        return await <T>fetch(url, {
            method: 'POST',
            headers: {
                "UserName": token.UN,
                "Authorization": token.TK,
                //'Content-Type': 'application/x-www-form-urlencoded',
                "TokenSessionId": token.SessionId,
                "TokenScopeId": token.ScopeId,
                "ClientId": token.ClientId,
                "UserId": token.UserId,
            },
            //body: JSON.stringify(DataToSave),
            body: DataToSave,
        })
            .then(async response => {
                return response.json() as Promise<T>
                // if (Number(response.status) >= 500 && Number(response.status) <= 599) {
                //     throw new Error(`Code: ${response.status} Authentication error!`)
                // }
                // if (Number(response.status) >= 400 && Number(response.status) <= 499) {
                //     throw new Error(`Code: ${response.status} Service not found!`)
                // }
                // if (!response.ok) {
                //     throw new Error(`Code: ${response.status} Something went wrong!`)
                // }
                // return response.json() as Promise<T>
            })
            .then(async jsonResponse => {

                _notifications.Notify.Alert(jsonResponse as _type.ApiResponse<unknown>)
                _utils.GenericFunctionality.HtmlLogs(`SaveApiWithFile`, `api response`, jsonResponse);
                return <T>jsonResponse;
            })
            .catch(error => {
                toastr.error(error, "Error");
                _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
                _utils.GenericFunctionality.HtmlLogs(`SaveApiWithFile`, `api error`, error);
            });
    }

    
    public static async SaveApi<T>(url: string, token: _type.TokenObject, DataToSave: object): Promise<T> {
        _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(true);
        let headerObject = {};
        headerObject["UserName"] =  token.UN;
        headerObject["Authorization"] =  token.TK;
        headerObject["Content-Type"] =  `application/json; charset=utf-8;application/x-www-form-urlencoded`;

        _utils.GenericFunctionality.HtmlLogs("Request headers for url ",url,JSON.stringify(headerObject));
        _utils.GenericFunctionality.HtmlLogs("Request object for url ",url,JSON.stringify(DataToSave));
        return await <T>fetch(url, {
            method: 'POST',
            headers: {
                "UserName": token.UN,
                "Authorization": token.TK,
                'Content-Type': 'application/json; charset=utf-8;application/x-www-form-urlencoded;',
                // "TokenSessionId": token.SessionId,
                // "TokenScopeId": token.ScopeId,
                // "ClientId": token.ClientId,
                // "UserId": token.UserId,
            },
            body: JSON.stringify(DataToSave),
        })
            .then(async response => {

                return response.json() as Promise<T>
                // if (Number(response.status) >= 500 && Number(response.status) <= 599) {
                //     throw new Error(`Code: ${response.status} Authentication error!`)
                // }
                // if (Number(response.status) >= 400 && Number(response.status) <= 499) {
                //     throw new Error(`Code: ${response.status} Service not found!`)
                // }
                // if (!response.ok) {
                //     throw new Error(`Code: ${response.status} Something went wrong!`)
                // }
                // return response.json() as Promise<T>
            })
            .then(async jsonResponse => {
                _utils.GenericFunctionality.HtmlLogs(`SaveApi`, `api response`, jsonResponse);

                _notifications.Notify.Alert(jsonResponse as _type.ApiResponse<unknown>)
                return <T>jsonResponse;
            })
            .catch(error => {

                toastr.error(error, "Error");
                _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
                _utils.GenericFunctionality.HtmlLogs(`SaveApi`, `api error`, error);
            });
    }
    public static async DeleteApi<T>(url: string, token: _type.TokenObject): Promise<T> {
        _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(true);
        _utils.GenericFunctionality.HtmlLogs(`DeleteApi`, `Url`, url);
        return await <T>fetch(url, {
            method: 'GET',
            headers: {
                "UserName": token.UN,
                "Authorization": token.TK,
                'Content-Type': 'application/json; charset=utf-8',
                "TokenSessionId": token.SessionId,
                "TokenScopeId": token.ScopeId,
            },
        })
            .then(async response => {
                return response.json() as Promise<T>
                // if (Number(response.status) >= 500 && Number(response.status) <= 599) {
                //     throw new Error(`Code: ${response.status} Authentication error!`)
                // }
                // if (Number(response.status) >= 400 && Number(response.status) <= 499) {
                //     throw new Error(`Code: ${response.status} Service not found!`)
                // }
                // if (!response.ok) {
                //     throw new Error(`Code: ${response.status} Something went wrong!`)
                // }
                // return response.json() as Promise<T>
            })
            .then(async jsonResponse => {
                _utils.GenericFunctionality.HtmlLogs(`DeleteApi`, `api response`, jsonResponse);
                _notifications.Notify.Alert(jsonResponse as _type.ApiResponse<unknown>)
                return <T>jsonResponse;
            })
            .catch(error => {
                toastr.error(error, "Error");
                _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
                _utils.GenericFunctionality.HtmlLogs(`DeleteApi`, `api response`, error);
            });
    }
}
