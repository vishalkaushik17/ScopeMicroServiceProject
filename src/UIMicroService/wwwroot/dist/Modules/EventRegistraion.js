var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import Swal from 'sweetalert2';
import * as _type from "../CustomType/customTypes";
import * as _utils from "../Security/utils";
import * as _apiModule from "../ApiMethods/ApiMethod";
import * as _storage from "../BrowserStorage/tsStorage";
import * as _moduleFunctions from "./ModuleFunctions";
export class EventBinding {
    static FindElementsAndBindEventsOfModuleForm(clsObject, objPrimary) {
        return __awaiter(this, void 0, void 0, function* () {
            const modelform = document.querySelector('#modelForm');
            const submitBtn = document.querySelector('#submit-btn');
            submitBtn.addEventListener(`click`, function (e) {
                return __awaiter(this, void 0, void 0, function* () {
                    e.preventDefault;
                    let retVal = yield EventBinding.ValidateForm(clsObject, objPrimary);
                    e.preventDefault;
                    return false;
                });
            });
            const refreshIndexBtn = document.querySelector(`#refreshIndex`);
            const modalWindow = document.querySelector(`#modal-default`);
            if (refreshIndexBtn) {
                refreshIndexBtn.addEventListener(`click`, function () {
                    _moduleFunctions.GenericModuleFunctions.RefreshPageWithoutCache(objPrimary, clsObject);
                });
            }
            const addNewRecordButton = document.querySelector('#InsertNewRecord');
            if (addNewRecordButton) {
                addNewRecordButton.addEventListener('click', EventBinding.AddNewRecord);
            }
            const cancelRecordButton = document.querySelector('#CancelRecord');
            if (cancelRecordButton) {
                cancelRecordButton.addEventListener('click', EventBinding.CancelRecord);
            }
            const cancelXRecordButton = document.querySelector('#model-close-x');
            if (cancelXRecordButton) {
                cancelXRecordButton.addEventListener('click', EventBinding.CancelRecord);
            }
            const saveRecordButton = document.querySelector('#SaveRecord');
            if (saveRecordButton) {
                saveRecordButton.addEventListener('click', function () {
                    EventBinding.SaveRecord();
                    return false;
                });
            }
            const dialogElement = document.querySelector(`#modal-default`);
            if (dialogElement) {
                dialogElement.addEventListener(`shown.bs.modal`, EventBinding.ShowModalPopup);
                jQuery(dialogElement)
                    .on('shown.bs.modal', EventBinding.ShowModalPopup);
            }
            const btnUploadImage = document.querySelector('#UploadImage');
            if (btnUploadImage) {
                btnUploadImage.addEventListener('change', function () {
                    EventBinding.changeImage(this);
                });
            }
        });
    }
    static changeImage(input) {
        return __awaiter(this, void 0, void 0, function* () {
            var reader;
            const imagePreview = document.querySelector('#ImagePreview');
            if (input.files && input.files[0]) {
                reader = new FileReader();
                reader.onload = function (e) {
                    imagePreview.src = e.target.result;
                    imagePreview.ariaValueText = `product image`;
                    const productImage = document.querySelector('#ProductImage');
                    ;
                    productImage.value = EventBinding.getBase64Image(imagePreview);
                };
                reader.readAsDataURL(input.files[0]);
            }
        });
    }
    static getBase64Image(img) {
        var canvas = document.createElement("canvas");
        canvas.width = img.width;
        canvas.height = img.height;
        var ctx = canvas.getContext("2d");
        ctx.drawImage(img, 0, 0);
        let dataURL = canvas.toDataURL("image/png");
        return dataURL;
    }
    static UploadImage(imageControlId) {
        return __awaiter(this, void 0, void 0, function* () {
            const imageElement = document.querySelector(`#${imageControlId}}`);
            if (imageElement) {
                imageElement.src = "";
            }
        });
    }
    static GetApplicationState() {
        return __awaiter(this, void 0, void 0, function* () {
            const saveBtn = document.querySelector(`#SaveRecord`);
            if (saveBtn && saveBtn.hasAttribute(`applicationstate`)) {
                return saveBtn.getAttribute(`applicationstate`);
            }
            else
                (saveBtn && !saveBtn.hasAttribute(`applicationstate`));
            {
                EventBinding.SetApplicationState(_type.ApplicationState.Index);
            }
            return _type.ApplicationState.Index;
        });
    }
    static SetApplicationState(applicationState) {
        return __awaiter(this, void 0, void 0, function* () {
            const saveBtn = document.querySelector(`#SaveRecord`);
            if (saveBtn) {
                saveBtn.setAttribute(`applicationstate`, applicationState);
            }
        });
    }
    static AddNewRecord() {
        return __awaiter(this, void 0, void 0, function* () {
            yield EventBinding.SetApplicationState(_type.ApplicationState.New);
            yield EventBinding.ResetOperationButtons(yield EventBinding.GetApplicationState());
        });
    }
    static CancelRecord() {
        return __awaiter(this, void 0, void 0, function* () {
            yield EventBinding.SetApplicationState(_type.ApplicationState.Cancel);
            yield EventBinding.ResetOperationButtons(yield EventBinding.GetApplicationState());
        });
    }
    static ShowModalPopup() {
        return __awaiter(this, void 0, void 0, function* () {
            const firstInputElementOnDialogElement = document.querySelector(`.first`);
            firstInputElementOnDialogElement.focus();
        });
    }
    static ValidateForm(clsPrimary, objPrimary) {
        return __awaiter(this, void 0, void 0, function* () {
            var _a, _b, _c;
            const modelForm = document.querySelector('#modelForm');
            const alertDialog = document.querySelector(`#errorAlert`);
            const alertMessage = document.querySelector(`#alertMessage`);
            let formValidated = true;
            for (var i = 0; i < modelForm.elements.length; i++) {
                if (modelForm.elements[i].value === '' && modelForm.elements[i].hasAttribute('required')) {
                    if (modelForm.elements[i].tagName === `INPUT` || modelForm.elements[i].tagName === `TEXTAREA`) {
                        alertMessage.innerText = `${(_b = (_a = modelForm.elements[i].nextElementSibling) === null || _a === void 0 ? void 0 : _a.nextElementSibling) === null || _b === void 0 ? void 0 : _b.innerHTML} is required!`;
                    }
                    else {
                        alertMessage.innerText = `${modelForm.elements[i].name} is required!`;
                    }
                    modelForm.elements[i].focus();
                    formValidated = false;
                    break;
                }
                if (modelForm.elements[i].hasAttribute('pattern')) {
                    let pattern = new RegExp(modelForm.elements[i].getAttribute('pattern'));
                    let value = modelForm.elements[i].value;
                    if (!pattern.test(value) && value !== 'N/A' && value !== '') {
                        alertMessage.innerText = `${(_c = modelForm.elements[i].getAttribute('customMessage')) !== null && _c !== void 0 ? _c : ``}`;
                        modelForm.elements[i].focus();
                        formValidated = false;
                        break;
                    }
                }
            }
            if (!formValidated) {
                alertDialog.style.display = `block`;
                alertDialog.setAttribute(`aria-hidden`, `true`);
                return Promise.resolve(formValidated);
            }
            const form = new FormData(document.querySelector('#modelForm'));
            function setNestedValue(obj, path, value) {
                const keys = path.split('.');
                let current = obj;
                for (let i = 0; i < keys.length - 1; i++) {
                    if (!current[keys[i]]) {
                        current[keys[i]] = {};
                    }
                    current = current[keys[i]];
                }
                current[keys[keys.length - 1]] = value;
            }
            const formObject = {};
            form.forEach((value, key) => {
                setNestedValue(formObject, key, value);
            });
            debugger;
            console.log(formObject);
            let apiResponse;
            let url = '';
            const RecordId = document.querySelector(`#Id`).value;
            switch (yield EventBinding.GetApplicationState()) {
                case _type.ApplicationState.Modify:
                    url = _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/update`, objPrimary.UseCache, ['id'], [RecordId]);
                    break;
                case _type.ApplicationState.New:
                default:
                    url = _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/save`, objPrimary.UseCache);
                    break;
            }
            apiResponse = yield _apiModule.ApiFunctionality.SaveApi(url, (yield EventBinding.GetApiToken()).Data, formObject);
            let logAdded = false;
            if (apiResponse && apiResponse.status === "Success") {
                let logsModal = document.querySelector(`#logs`);
                if (logsModal) {
                    if (apiResponse === null || apiResponse === void 0 ? void 0 : apiResponse.log) {
                        logsModal.innerHTML = logsModal.innerHTML + apiResponse.log;
                    }
                    logAdded = true;
                }
                let browserData = (yield _storage.CacheStorage).GetDataFromLocalStorage(objPrimary.CacheKey, objPrimary.UseCache);
                if (browserData) {
                    switch (yield EventBinding.GetApplicationState()) {
                        case _type.ApplicationState.Index:
                        case _type.ApplicationState.New:
                            (yield browserData).Data.Result.push(yield apiResponse.Result);
                            (yield browserData).Data.recordCount = (yield browserData).Data.Result.length;
                            break;
                        case _type.ApplicationState.Save:
                        case _type.ApplicationState.Modify:
                            {
                                (yield browserData).Data.Result = (yield browserData).Data.Result.map(record => {
                                    if (record.id === RecordId) {
                                        record = (apiResponse.Result);
                                        return record;
                                    }
                                    return record;
                                });
                                break;
                            }
                        case _type.ApplicationState.Delete:
                            break;
                    }
                    let logsModal = document.querySelector(`#logs`);
                    if (logsModal && !logAdded) {
                        logsModal.innerHTML = logsModal.innerHTML + apiResponse.log;
                    }
                    browserData.then((newData) => __awaiter(this, void 0, void 0, function* () {
                        (yield (_storage.CacheStorage)).SetDataToLocalStorage(newData.Data, objPrimary.CacheKey, objPrimary.CacheKey, 30);
                        yield clsPrimary.FillDataOnIndexTable(newData.Data.Result);
                    }));
                }
                yield EventBinding.SetApplicationState(_type.ApplicationState.Save);
                yield EventBinding.ResetOperationButtons(yield EventBinding.GetApplicationState());
                const btnCancle = document.querySelector(`#CancelRecord`);
                if (btnCancle) {
                    btnCancle.click();
                }
            }
            alertDialog.style.display = `none`;
            return Promise.resolve(formValidated);
        });
    }
    static SaveRecord() {
        const submitBtnForFormSave = document.querySelector(`#submit-btn`);
        submitBtnForFormSave.click();
        return false;
    }
    static GetApiToken() {
        return __awaiter(this, void 0, void 0, function* () {
            return (yield (_storage.CacheStorage)).GetToken("Token").then(bToken => bToken);
        });
    }
    static ResetOperationButtons(operation) {
        return __awaiter(this, void 0, void 0, function* () {
            const saveLiElement = document.querySelector('#saveLi');
            const cancelLiElement = document.querySelector('#cancelLi');
            const newRecordLiElement = document.querySelector('#newRecordLi');
            const modalForm = document.querySelector('#modelForm');
            switch (operation) {
                case _type.ApplicationState.Index:
                case _type.ApplicationState.New:
                    saveLiElement.style.display = 'block';
                    cancelLiElement.style.display = 'block';
                    newRecordLiElement.style.display = 'none';
                    modalForm.reset();
                    _moduleFunctions.GenericModuleFunctions.ResetFormData();
                    break;
                case _type.ApplicationState.Modify:
                    saveLiElement.style.display = 'block';
                    cancelLiElement.style.display = 'block';
                    newRecordLiElement.style.display = 'none';
                    break;
                case _type.ApplicationState.Cancel:
                    saveLiElement.style.display = 'none';
                    cancelLiElement.style.display = 'none';
                    newRecordLiElement.style.display = 'block';
                    modalForm.reset();
                    _moduleFunctions.GenericModuleFunctions.ResetFormData();
                    break;
                case _type.ApplicationState.Save:
                    saveLiElement.style.display = 'none';
                    cancelLiElement.style.display = 'none';
                    newRecordLiElement.style.display = 'block';
                    modalForm.reset();
                    _moduleFunctions.GenericModuleFunctions.ResetFormData();
                    break;
                default:
                    break;
            }
        });
    }
    static FindAndRemoveEventsOfIndexTable() {
        return __awaiter(this, void 0, void 0, function* () {
            const dataDeleteBtn = Array.from(document.querySelectorAll('[index-delete-data-id]'));
            dataDeleteBtn.forEach((e) => {
                EventBinding.recreateNode(e);
            });
            const dataEditlink = Array.from(document.querySelectorAll('[index-edit-data-id]'));
            dataEditlink.forEach((e) => {
                EventBinding.recreateNode(e);
            });
        });
    }
    static FindAndBindEventsOfIndexTable(objPrimary, clsObject) {
        return __awaiter(this, void 0, void 0, function* () {
            const dataDeleteBtn = Array.from(document.querySelectorAll('[index-delete-data-id]'));
            dataDeleteBtn.forEach((e) => {
                e.addEventListener('click', function () {
                    EventBinding.BindDeleteEvent(this, objPrimary);
                });
            });
            const dataEditlink = Array.from(document.querySelectorAll('[index-edit-data-id]'));
            dataEditlink.forEach((e) => {
                e.addEventListener('click', function () {
                    EventBinding.BindEditEvent(this, objPrimary, clsObject);
                });
            });
        });
    }
    static recreateNode(el, withChildren) {
        if (withChildren) {
            el.parentNode.replaceChild(el.cloneNode(true), el);
        }
        else {
            var newEl = el.cloneNode(false);
            while (el.hasChildNodes())
                newEl.appendChild(el.firstChild);
            el.parentNode.replaceChild(newEl, el);
        }
    }
    static BindEditEvent(e, objPrimary, clsObject) {
        return __awaiter(this, void 0, void 0, function* () {
            EventBinding.SetApplicationState(_type.ApplicationState.Modify);
            let recordId = e.getAttribute('index-edit-data-id');
            console.log(`edit id = ${recordId}`);
            objPrimary.RecordId = recordId;
            const url = _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/get`, objPrimary.UseCache, [`id`], [objPrimary.RecordId]);
            yield _moduleFunctions.GenericModuleFunctions.GetModelData(url, yield EventBinding.GetApplicationState(), objPrimary, clsObject);
        });
    }
    static BindDeleteEvent(e, objPrimary) {
        objPrimary.ApplicationState = _type.ApplicationState.Delete;
        let deleteRecordId = e.getAttribute('index-delete-data-id');
        const currentRow = e.closest(".box .box-solid").parentNode.parentNode;
        if (deleteRecordId) {
            Swal.fire({
                title: 'Are you sure?',
                text: 'You won\'t be able to revert this!',
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => __awaiter(this, void 0, void 0, function* () {
                if (result.value) {
                    const url = _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/delete`, objPrimary.UseCache, ['id'], [deleteRecordId]);
                    let apiResponse = yield _apiModule.ApiFunctionality.DeleteApi(url, (yield EventBinding.GetApiToken()).Data);
                    let browserData;
                    if (apiResponse && apiResponse.status === "Success") {
                        currentRow.parentNode.removeChild(currentRow);
                        browserData = (yield (yield _storage.CacheStorage).GetDataFromLocalStorage(objPrimary.CacheKey, objPrimary.UseCache));
                        if (yield browserData) {
                            let response = yield browserData;
                            let newRecord = response.Data.Result.filter(e => e.id != apiResponse.Result.id);
                            response.Data.Result = newRecord;
                            response.Data.recordCount = newRecord.length;
                            (yield _storage.CacheStorage).SetDataToLocalStorage(response.Data, objPrimary.CacheKey, objPrimary.CacheKey, 30);
                            let logsModal = document.querySelector(`#logs`);
                            if (logsModal) {
                                logsModal.innerHTML = logsModal.innerHTML + apiResponse.log;
                            }
                        }
                    }
                    else {
                        toastr.error("Something went wrong!", "Error");
                    }
                }
            }));
        }
    }
}
//# sourceMappingURL=EventRegistraion.js.map