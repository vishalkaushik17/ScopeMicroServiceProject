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
import * as _events from "./EventRegistraion";
import * as _utils from "../Security/utils";
import * as _apiModule from "../ApiMethods/ApiMethod";
let _userCache = true;
export class ApplyEffectOnModuleSpecific {
    static ApplyCssOnModule(moduleId, innerModuleId) {
        var _a, _b;
        (_a = document.querySelectorAll('.active')) === null || _a === void 0 ? void 0 : _a.forEach((activeElement) => { activeElement.classList.remove('active'); });
        (_b = document.querySelectorAll('.ActiveSideBarLiSpan')) === null || _b === void 0 ? void 0 : _b.forEach((activeElement) => { activeElement.classList.remove('ActiveSideBarLiSpan'); });
        document.querySelectorAll(`.selectedBarIcon`).forEach(element => {
            element.classList.remove(`selectedBarIcon`);
        });
        document.querySelectorAll(`#LayoutUl li`).forEach((item) => {
            var _a, _b, _c, _d, _e, _f, _g, _h, _j;
            item.classList.remove("active");
            if (item.id === "LayoutUl") {
                item.classList.add("active");
                item.classList.add("treeview");
            }
            else {
                item.classList.remove("active");
            }
            if (item.id === innerModuleId) {
                (_a = document.querySelector(`#${moduleId} a span`)) === null || _a === void 0 ? void 0 : _a.classList.add(`ActiveSideBarLiSpan`);
                (_b = item.querySelector(`a`)) === null || _b === void 0 ? void 0 : _b.classList.add("ActiveAsideBarALink");
                (_c = item.querySelector(`a`)) === null || _c === void 0 ? void 0 : _c.classList.add("selectedBarIcon");
                (_f = (_e = (_d = item.parentElement) === null || _d === void 0 ? void 0 : _d.parentElement) === null || _e === void 0 ? void 0 : _e.querySelector(`a i`)) === null || _f === void 0 ? void 0 : _f.classList.add("selectedBarIcon");
                (_h = (_g = item.parentElement) === null || _g === void 0 ? void 0 : _g.querySelector(`a i`)) === null || _h === void 0 ? void 0 : _h.classList.add("selectedBarIcon");
            }
            else {
                (_j = item.querySelector(`a`)) === null || _j === void 0 ? void 0 : _j.classList.remove("ActiveAsideBarALink");
            }
        });
    }
    static SpinnerLoading(effect) {
        const spinner = document.querySelector('#LoadSpinner');
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
    static SetIndexPageTitle(objPrimary) {
        return __awaiter(this, void 0, void 0, function* () {
            const indexPageTitle = document.querySelector(`#indexPageTitle`);
            indexPageTitle.textContent = objPrimary.IndexTitle;
            indexPageTitle.innerHTML = indexPageTitle.textContent + ` <small>Index</small>`;
        });
    }
    static RefreshPageWithoutCache(objPrimary, clsPrimary) {
        return __awaiter(this, void 0, void 0, function* () {
            (yield _storage.CacheStorage).ClearModuleKeys([objPrimary.CacheKey]);
            (yield _storage.CacheStorage).SetDefaultData();
            _events.EventBinding.SetApplicationState(_type.ApplicationState.Index);
            let checkBrowserCache = objPrimary.UseCache;
            objPrimary.UseCache = false;
            const url = yield _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/getall`, objPrimary.UseCache);
            yield GenericModuleFunctions.GetModelData(url, yield _events.EventBinding.GetApplicationState(), objPrimary, clsPrimary);
            if (checkBrowserCache) {
                objPrimary.UseCache = true;
            }
            else {
                objPrimary.UseCache = false;
            }
        });
    }
    static ResetFormData() {
        return __awaiter(this, void 0, void 0, function* () {
            const form = document.querySelector('#modelForm');
            const idElement = document.querySelector('#Id');
            if (idElement) {
                idElement.value = ``;
            }
            form.reset();
        });
    }
    static ReflectOnProperty(elementType, value) {
        let reflectionType;
        let reflectionOn = elementType.getAttribute(`reflectionOn`);
        if (reflectionOn) {
            reflectionType = document.getElementById(reflectionOn);
        }
        switch (reflectionType === null || reflectionType === void 0 ? void 0 : reflectionType.tagName) {
            case "IMG":
                reflectionType['src'] = value;
                break;
            default:
                break;
        }
    }
    static GetModelData(url, ApplicationState, objPrimary, clsPrimary) {
        return __awaiter(this, void 0, void 0, function* () {
            var _a;
            let response;
            if (objPrimary.UseCache) {
                if (ApplicationState !== _type.ApplicationState.Modify) {
                    let data = yield (yield _storage.CacheStorage).GetDataFromLocalStorage(objPrimary.CacheKey, objPrimary.UseCache);
                    if (data && (yield data).Data.id === null && (yield data).Data.Result.length > 0) {
                        response = (yield data);
                    }
                    else {
                        response = yield _apiModule.ApiFunctionality.GetApi(url, (yield _events.EventBinding.GetApiToken()).Data, objPrimary.CacheKey, ApplicationState);
                    }
                }
                else {
                    response = yield _apiModule.ApiFunctionality.GetApi(url, (yield _events.EventBinding.GetApiToken()).Data, objPrimary.CacheKey, ApplicationState);
                }
            }
            else {
                response = yield _apiModule.ApiFunctionality.GetApi(url, (yield _events.EventBinding.GetApiToken()).Data, objPrimary.CacheKey, ApplicationState);
                if (ApplicationState === _type.ApplicationState.Index && objPrimary.UseCache) {
                    response = (yield (yield _storage.CacheStorage).GetDataFromLocalStorage(objPrimary.CacheKey, objPrimary.UseCache));
                }
            }
            if (!response) {
                toastr.error("Server communication error!", "Error");
            }
            else {
                switch (ApplicationState) {
                    case _type.ApplicationState.Modify:
                        const form = new FormData(document.querySelector('#modelForm'));
                        [...form].forEach((element) => {
                            if (response.Result.hasOwnProperty(_utils.GenericFunctionality.toCamelCase(element[0]))) {
                                const propertyName = _utils.GenericFunctionality.toCamelCase(element[0]);
                                let elementOfForm = document.querySelector(`#${element[0]}`);
                                switch (elementOfForm.tagName) {
                                    case "INPUT":
                                        let InputElement = document.querySelector(`#${element[0]}`);
                                        InputElement.value = response.Result[_utils.GenericFunctionality.toCamelCase(element[0])];
                                        GenericModuleFunctions.ReflectOnProperty(elementOfForm, InputElement.value);
                                        break;
                                    case "TEXTAREA":
                                        let TextAreaElement = document.querySelector(`#${element[0]}`);
                                        TextAreaElement.value = response.Result[_utils.GenericFunctionality.toCamelCase(element[0])];
                                        GenericModuleFunctions.ReflectOnProperty(elementOfForm, TextAreaElement.value);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        });
                        yield _events.EventBinding.ResetOperationButtons(ApplicationState);
                        break;
                    case _type.ApplicationState.Index:
                    default:
                        if (objPrimary.UseCache && response && ((_a = response === null || response === void 0 ? void 0 : response.Data) === null || _a === void 0 ? void 0 : _a.Result)) {
                            yield clsPrimary.FillDataOnIndexTable((response.Data.Result));
                        }
                        else {
                            yield clsPrimary.FillDataOnIndexTable((response.Result));
                        }
                        break;
                }
                let logsModal = document.querySelector(`#logs`);
                if (logsModal) {
                    if (response === null || response === void 0 ? void 0 : response.Data) {
                        logsModal.innerHTML = logsModal.innerHTML + response.Data.log;
                    }
                    else {
                        logsModal.innerHTML = logsModal.innerHTML + response.log;
                    }
                }
            }
        });
    }
}
//# sourceMappingURL=ModuleFunctions.js.map