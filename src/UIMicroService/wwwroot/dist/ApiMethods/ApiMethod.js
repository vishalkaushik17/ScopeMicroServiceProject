var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import * as _storage from "../BrowserStorage/tsStorage";
import * as _type from "../CustomType/customTypes";
import * as _notifications from "../UIComponents/Notifications/NotificationComponents";
import * as _moduleFunctions from "../Modules/ModuleFunctions";
export class ApiFunctionality {
    static GetApi(url, token, moduleCacheKey, ApplicationState) {
        return __awaiter(this, void 0, void 0, function* () {
            _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(true);
            return yield fetch(url, {
                method: 'GET',
                headers: {
                    "UserName": token.UN,
                    "Authorization": token.TK,
                    'Content-Type': 'application/json; charset=utf-8',
                    "TokenSessionId": token.SessionId,
                    "TokenScopeId": token.ScopeId,
                    "ClientId": token.ClientId,
                    "UserId": token.UserId,
                }
            })
                .then((response) => __awaiter(this, void 0, void 0, function* () {
                return response.json();
                if (Number(response.status) >= 500 && Number(response.status) <= 599) {
                    throw new Error(`Code: ${response.status} Authentication error!`);
                }
                if (Number(response.status) >= 400 && Number(response.status) <= 499) {
                    console.log(response);
                    throw new Error(`Code: ${response.status} Service not found!`);
                }
                if (!response.ok) {
                    throw new Error(`Code: ${response.status} Something went wrong!`);
                }
            }))
                .then((jsonResponse) => __awaiter(this, void 0, void 0, function* () {
                if (ApplicationState !== _type.ApplicationState.Modify) {
                    _notifications.Notify.Alert(jsonResponse);
                }
                else if (ApplicationState === _type.ApplicationState.Modify) {
                    _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
                }
                if (ApplicationState != _type.ApplicationState.Modify) {
                    const responseObj = jsonResponse;
                    (yield _storage.CacheStorage).SetDataToLocalStorage(responseObj, moduleCacheKey, moduleCacheKey, 30);
                }
                return jsonResponse;
            }))
                .catch(error => {
                toastr.error(error, "Error");
                _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
            });
        });
    }
    static SaveApiWithFile(url, token, DataToSave) {
        return __awaiter(this, void 0, void 0, function* () {
            _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(true);
            return yield fetch(url, {
                method: 'POST',
                headers: {
                    "UserName": token.UN,
                    "Authorization": token.TK,
                    'Content-Type': 'application/json; charset=utf-8',
                    "TokenSessionId": token.SessionId,
                    "TokenScopeId": token.ScopeId,
                    "ClientId": token.ClientId,
                    "UserId": token.UserId,
                },
                body: JSON.stringify(DataToSave),
            })
                .then((response) => __awaiter(this, void 0, void 0, function* () {
                return response.json();
                if (Number(response.status) >= 500 && Number(response.status) <= 599) {
                    throw new Error(`Code: ${response.status} Authentication error!`);
                }
                if (Number(response.status) >= 400 && Number(response.status) <= 499) {
                    throw new Error(`Code: ${response.status} Service not found!`);
                }
                if (!response.ok) {
                    throw new Error(`Code: ${response.status} Something went wrong!`);
                }
                return response.json();
            }))
                .then((jsonResponse) => __awaiter(this, void 0, void 0, function* () {
                _notifications.Notify.Alert(jsonResponse);
                return jsonResponse;
            }))
                .catch(error => {
                toastr.error(error, "Error");
                _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
            });
        });
    }
    static SaveApi(url, token, DataToSave) {
        return __awaiter(this, void 0, void 0, function* () {
            _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(true);
            return yield fetch(url, {
                method: 'POST',
                headers: {
                    "UserName": token.UN,
                    "Authorization": token.TK,
                    'Content-Type': 'application/json; charset=utf-8',
                    "TokenSessionId": token.SessionId,
                    "TokenScopeId": token.ScopeId,
                    "ClientId": token.ClientId,
                    "UserId": token.UserId,
                },
                body: JSON.stringify(DataToSave),
            })
                .then((response) => __awaiter(this, void 0, void 0, function* () {
                return response.json();
            }))
                .then((jsonResponse) => __awaiter(this, void 0, void 0, function* () {
                _notifications.Notify.Alert(jsonResponse);
                return jsonResponse;
            }))
                .catch(error => {
                toastr.error(error, "Error");
                _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
            });
        });
    }
    static DeleteApi(url, token) {
        return __awaiter(this, void 0, void 0, function* () {
            _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(true);
            return yield fetch(url, {
                method: 'GET',
                headers: {
                    "UserName": token.UN,
                    "Authorization": token.TK,
                    'Content-Type': 'application/json; charset=utf-8',
                    "TokenSessionId": token.SessionId,
                    "TokenScopeId": token.ScopeId,
                },
            })
                .then((response) => __awaiter(this, void 0, void 0, function* () {
                return response.json();
                if (Number(response.status) >= 500 && Number(response.status) <= 599) {
                    throw new Error(`Code: ${response.status} Authentication error!`);
                }
                if (Number(response.status) >= 400 && Number(response.status) <= 499) {
                    throw new Error(`Code: ${response.status} Service not found!`);
                }
                if (!response.ok) {
                    throw new Error(`Code: ${response.status} Something went wrong!`);
                }
                return response.json();
            }))
                .then((jsonResponse) => __awaiter(this, void 0, void 0, function* () {
                _notifications.Notify.Alert(jsonResponse);
                return jsonResponse;
            }))
                .catch(error => {
                toastr.error(error, "Error");
                _moduleFunctions.ApplyEffectOnModuleSpecific.SpinnerLoading(false);
            });
        });
    }
}
//# sourceMappingURL=ApiMethod.js.map