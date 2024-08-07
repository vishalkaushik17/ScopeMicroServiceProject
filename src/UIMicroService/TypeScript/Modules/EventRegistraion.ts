import Swal, { SweetAlertOptions } from 'sweetalert2';// const swal = swals as any;
import * as _type from "../CustomType/customTypes";
import * as _utils from "../Security/utils"
import * as _apiModule from "../ApiMethods/ApiMethod"
import * as _storage from "../BrowserStorage/tsStorage"
import * as _moduleFunctions from "./ModuleFunctions";
import * as $ from 'jquery';
//import 'jquery-serializetojson';

//import { event } from 'jquery';
// import * as jQuery from 'jquery';
// import * as jSerial from 'jquery-serializetojson';

// declare var $: any;
// declare let global: any;
// global.jQuery = $;

export class EventBinding {

    //finding controls on form and bind respective operation event on them.
    public static async FindElementsAndBindEventsOfModuleForm(clsObject: object | any, objPrimary: object | any): Promise<void> {

        _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `Binding event on required elements started!.`, '');

        const submitSaveBtn = document.querySelector('#submit-btn')! as HTMLAnchorElement;
        submitSaveBtn.addEventListener(`click`, async function (e) {
            _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `Clicked event triggered to submit a form!.`, '');
            e.preventDefault;
            _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `Form validation started!.`, '');
            let retVal = await EventBinding.ValidateForm(clsObject, objPrimary);
            if (retVal === true) {
                _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `Form validation completed!, form data submitted.`, '');
            } else {
                _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `Form validation failed!`, '');

            }
            return false;
        })



        // const homeDashBoardLink = document.querySelector(`a[partial_nav_link][href='#/ui/Dashboard']`)! as HTMLAnchorElement;
        // if (homeDashBoardLink) {
        //     homeDashBoardLink.addEventListener('click', function () {
        //         const getAllLiOnAsideBar = Array.from(document.querySelectorAll(`li.treeview a span`)!);
        //         getAllLiOnAsideBar.forEach((element) => {
        //             let el = element as HTMLSpanElement;
        //             el.style.color = `darkolivegreen`;
        //             el.style.fontWeight = `normal`;
        //         })
        //         const DashboardLi = document.querySelector(`#DashboardLi span`)! as HTMLLIElement;
        //         DashboardLi.style.color = `#0f6cbd`;
        //         DashboardLi.style.fontWeight = `bold`;
        //     });
        //     const liTrViewA = document.querySelectorAll(`ul.sidebar-menu li.treeview a`);
        //     liTrViewA.forEach((element) => {
        //         let el = element as HTMLSpanElement;
        //         el.style.color = `darkolivegreen`;
        //         el.style.fontWeight = `normal`;
        //     })


        // }
        const refreshIndexBtn = document.querySelector(`#refreshIndex`)! as HTMLButtonElement;
        const modalWindow = document.querySelector(`#modal-default`)! as HTMLButtonElement;

        if (refreshIndexBtn) {
            refreshIndexBtn.addEventListener(`click`, function () {
                _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `RefreshIndex button - Refresh record clicked!.`, '');
                _moduleFunctions.GenericModuleFunctions.RefreshPageWithoutBrowserCache(objPrimary, clsObject)
            });
        }

        // modalWindow.addEventListener('keypress', function (event) {
        //     event.preventDefault;
        //     
        //     if (event.key === "Enter") { // escape key maps to keycode `27`
        //         const btnSave = document.querySelector(`#SaveRecord`)! as HTMLAnchorElement;
        //         if (btnSave) {
        //             btnSave.click();
        //         }
        //     }
        //     //it will prevent refresh the page on enter key 
        //     return false;
        // });

        // modalWindow.addEventListener('keyup', function (event) {
        //     event.preventDefault;
        //     
        //     if (event.key === "Escape") { // escape key maps to keycode `27`
        //         const btnCancle = document.querySelector(`#CancelRecord`)! as HTMLAnchorElement;
        //         if (btnCancle) {
        //             btnCancle.click();
        //         }
        //     }
        //     return false;
        // });

        //Find New Button
        const addNewRecordButton = document.querySelector('#InsertNewRecord')! as HTMLAnchorElement;
        if (addNewRecordButton) {

            _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `InsertNewRecord clicked!.`, '');
            addNewRecordButton.addEventListener('click', EventBinding.AddNewRecord);

        }

        const cancelRecordButton = document.querySelector('#CancelRecord')! as HTMLAnchorElement;
        if (cancelRecordButton) {
            _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `CancelRecord clicked!.`, '');

            cancelRecordButton.addEventListener('click', EventBinding.CancelRecord);
        }
        const cancelXRecordButton = document.querySelector('#model-close-x')! as HTMLButtonElement;
        if (cancelXRecordButton) {
            _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `model-close-x clicked!.`, '');
            cancelXRecordButton.addEventListener('click', EventBinding.CancelRecord);
        }

        const saveRecordButton = document.querySelector('#SaveRecord')! as HTMLAnchorElement;
        if (saveRecordButton) {
            saveRecordButton.addEventListener('click', function () {
                _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `saveRecordButton clicked!.`, '');
                EventBinding.SaveRecord()
                return false;
            });
        }

        const dialogElement = document.querySelector(`#modal-default`)! as HTMLDialogElement;
        if (dialogElement) {
            //when modal display on dom, this event get executed,
            //this event is used for setting up focus on first element.
            dialogElement.addEventListener(`shown.bs.modal`, EventBinding.ShowModalPopup);
            _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `ShowModalPopup clicked!.`, '');

            jQuery(dialogElement)
                .on('shown.bs.modal', EventBinding.ShowModalPopup);
        }

        //image control
        // const btnUploadImage = document.querySelector('#UploadImage')! as HTMLInputElement;
        // if (btnUploadImage) {
        //     //saveRecordButton.addEventListener('click',EventBinding.SaveRecord.bind(saveRecordButton,objPrimary,clsObject));
        //     btnUploadImage.addEventListener('change', function () {
        //         _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `UploadImage image chanced!.`, '');

        //         //EventBinding.changeImage(this)
        //         // const productImage = document.querySelector('#ProductImage')! as HTMLInputElement;
        //         // const imagePreview = document.querySelector('#ImagePreview')! as HTMLImageElement;
        //         // ;
        //         // productImage.value =  EventBinding.getBase64Image(imagePreview);
        //     });
        // }
        _utils.GenericFunctionality.HtmlLogs(`FindElementsAndBindEventsOfModuleForm`, `Binding event on required elements completed!.`, '');

    }
    // public static async changeImage(input) {
    //     _utils.GenericFunctionality.HtmlLogs(`changeImage`, `Execution started!.`, '');

    //     var reader;
    //     const imagePreview = document.querySelector('#ImagePreview')! as HTMLImageElement;

    //     if (input.files && input.files[0]) {
    //         reader = new FileReader();

    //         reader.onload = function (e) {

    //             imagePreview.src = e.target.result;
    //             imagePreview.ariaValueText = `product image`;
    //             //const productImage = document.querySelector('#ProductImage')! as HTMLInputElement;

    //             //productImage.value = EventBinding.getBase64Image(imagePreview);
    //         }

    //         reader.readAsDataURL(input.files[0]);
    //     }
    //     _utils.GenericFunctionality.HtmlLogs(`changeImage`, `Execution completed!.`, '');

    //     // var c = document.createElement('canvas') as HTMLCanvasElement;
    //     // c.height = imagePreview.naturalHeight;
    //     // c.width = imagePreview.naturalWidth;
    //     // var ctx = c.getContext('2d')!;
    //     // ctx.drawImage(imagePreview, 0, 0, c.width, c.height);
    //     // var base64String = c.toDataURL();
    //     // productImage.value = base64String;

    // }

    public static getBase64Image(img) {
        var canvas = document.createElement("canvas")!;
        canvas.width = img.width;
        canvas.height = img.height;
        var ctx = canvas.getContext("2d")!;
        ctx.drawImage(img, 0, 0);
        let dataURL = canvas.toDataURL("image/png")!;
        return dataURL;//?.replace(/^data:image\/?[A-z]*;base64,/)!;
    }
    public static async UploadImage(imageControlId: string) {
        const imageElement = document.querySelector(`#${imageControlId}}`)! as HTMLImageElement;
        if (imageElement) {
            imageElement.src = ""
        }
    }

    public static async GetApplicationState(): Promise<_type.ApplicationState> {

        const saveBtn = document.querySelector(`#SaveRecord`)! as HTMLAnchorElement;
        if (saveBtn && saveBtn.hasAttribute(`applicationstate`)) {
            return saveBtn.getAttribute(`applicationstate`) as _type.ApplicationState;
        }
        else (saveBtn && !saveBtn.hasAttribute(`applicationstate`))
        {
            EventBinding.SetApplicationState(_type.ApplicationState.Index);
        }
        return _type.ApplicationState.Index;
    }

    public static async SetApplicationState(applicationState: _type.ApplicationState) {
        const saveBtn = document.querySelector(`#SaveRecord`)! as HTMLAnchorElement;
        if (saveBtn) {
            saveBtn.setAttribute(`applicationstate`, applicationState);
        }
    }

    private static async AddNewRecord(): Promise<void> {
        //reset button state as per the operation

        await EventBinding.SetApplicationState(_type.ApplicationState.New);
        await EventBinding.ResetOperationButtons(await EventBinding.GetApplicationState());
    }
    private static async CancelRecord(): Promise<void> {
        //reset button state as per the operation
        await EventBinding.SetApplicationState(_type.ApplicationState.Cancel);
        await EventBinding.ResetOperationButtons(await EventBinding.GetApplicationState());
    }

    private static async ShowModalPopup() {
        // console.log("dialogElement");
        const firstInputElementOnDialogElement = document.querySelector(`.first`)! as HTMLInputElement;
        firstInputElementOnDialogElement.focus();
    }

    //it will validate the form before submit.
    private static async ValidateForm(clsPrimary: object | any, objPrimary: object | any): Promise<boolean> {
        _utils.GenericFunctionality.HtmlLogs('ValidateForm', 'Form Validation started', '')
        const modelForm = document.querySelector('#modelForm')! as HTMLFormElement | any;
        const alertDialog = document.querySelector(`#errorAlert`)! as HTMLDivElement;
        const alertMessage = document.querySelector(`#alertMessage`)! as HTMLDivElement;
        let formValidated = true;

        //checking input field is required. if value is null or not set, set focus on that element.
        for (var i = 0; i < modelForm.elements.length; i++) {

            if (modelForm.elements[i].value === '' && modelForm.elements[i].hasAttribute('required')) {

                if (modelForm.elements[i].tagName === `INPUT` || modelForm.elements[i].tagName === `TEXTAREA`) {
                    alertMessage.innerText = `${(modelForm.elements[i] as HTMLInputElement).nextElementSibling?.nextElementSibling?.innerHTML} is required!`
                } else {
                    alertMessage.innerText = `${modelForm.elements[i].name} is required!`
                }

                modelForm.elements[i].focus();

                formValidated = false;
                break;
            }
            let value = modelForm.elements[i].value;

            if (value.toUpperCase() === 'N/A') {
                modelForm.elements[i].value = value.toUpperCase();
            }

            //checking element has pattern or not.
            if (modelForm.elements[i].hasAttribute('pattern')) {
                //get pattern of the element
                let pattern = new RegExp(modelForm.elements[i].getAttribute('pattern'));

                _utils.GenericFunctionality.HtmlLogs('ValidateForm', `pattern found for data field ${modelForm.elements[i].Id}`, pattern.source)
                //checking pattern with given value.
                if (!pattern.test(value) && value.toUpperCase() !== 'N/A' && value !== '') {

                    alertMessage.innerText = `${modelForm.elements[i].getAttribute('customMessage') ?? ``}`;
                    modelForm.elements[i].focus();
                    formValidated = false;
                    break;
                }
            }
        }

        if (!formValidated) {
            _utils.GenericFunctionality.HtmlLogs('ValidateForm', 'form input is not valid,returning to UI.', '')
            alertDialog.style.display = `block`;
            alertDialog.setAttribute(`aria-hidden`, `true`);
            return Promise.resolve(formValidated);
        }
        //  const modalForm = document.getElementById("modal-default")! as HTMLDialogElement;

        _utils.GenericFunctionality.HtmlLogs('ValidateForm', 'Form Successfully validated on UI', '')

        //Read modelForm elements from DOM.
        let allElements = document.querySelector('#modelForm')! as HTMLFormElement;

        //here we are getting all elements which are required to submit.
        let elementToSubmit = allElements.querySelectorAll(`:not([dontsubmit])`)!;

        debugger;
        //creating form with required elements
        const form = new FormData(allElements);

        //for adding element value to submit form.
        const formObject = {};

        //let finalFormToSubmit:any =elementToSubmit;
        let nodeListArray: any = Array.from(elementToSubmit);
        let ValueToAddOnPropertyObject: _type.Primitive;

        form.forEach((value, key) => {

            ValueToAddOnPropertyObject = null;
            let property = nodeListArray.find(o => o.name === key);

            //here we have to check type for the element and add the respective value.
            if (property && property.type === 'checkbox') {
                if (property.checked === true) {
                    ValueToAddOnPropertyObject = true;
                    // _utils.GenericFunctionality.CreateNestedObject(formObject, key, true);
                } else {
                    ValueToAddOnPropertyObject = false;
                }
            }
            //double check this condition when radio is on DOM 
            if (property && property.type === 'radio') {
                if (property.checked === true) {
                    ValueToAddOnPropertyObject = property.value as _type.Primitive;
                }
            }
            else {
                if (property === undefined) {
                    ValueToAddOnPropertyObject = null;
                } else {
                    if (typeof ValueToAddOnPropertyObject !== 'boolean') {
                        ValueToAddOnPropertyObject = value as _type.Primitive;
                    }
                }
            }
            if (property !== undefined) {
                _utils.GenericFunctionality.CreateNestedObject(formObject, key, ValueToAddOnPropertyObject);

                //removing non required properties from the formObject.
                _utils.GenericFunctionality.removeNotRequiredProperties(formObject, property, key, ValueToAddOnPropertyObject);
            }
        });

        let apiResponse: _type.ApiResponse<object>;
        let url = '';
        const RecordId = (document.querySelector(`#Id`)! as HTMLInputElement).value;
        _utils.GenericFunctionality.HtmlLogs('ValidateForm', 'ApplicationState', await EventBinding.GetApplicationState())
        //for building respective url to perform action
        //SaveApi record
        switch (await EventBinding.GetApplicationState()) {
            case _type.ApplicationState.Modify:
                url = _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/update`, objPrimary.UseBrowserCache, ['id'], [RecordId])
                break;
            case _type.ApplicationState.New:
            default:
                url = _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/save`, objPrimary.UseBrowserCache)
                break;
        }

        _utils.GenericFunctionality.HtmlLogs(`ValidateForm`, `Post Data type`, objPrimary.ApiType);
        if (objPrimary.ApiType === _type.ApiType.FormData) {
            _utils.GenericFunctionality.HtmlLogs(`ValidateForm`, `Converting json object to FormData`, formObject);
            const formData = new FormData();
            //appending formObject to FormData to submit data with file support.
            _utils.GenericFunctionality.appendFormData(formData, formObject);
            apiResponse = await _apiModule.ApiFunctionality.SaveApiWithFile<_type.ApiResponse<object>>(url, (await EventBinding.GetApiToken()).Data, formData);
        } else {
            apiResponse = await _apiModule.ApiFunctionality.SaveApi<_type.ApiResponse<object>>(url, (await EventBinding.GetApiToken()).Data, formObject);
        }



        let logAdded: boolean = false;
        if (apiResponse && apiResponse.status === _type.ResponseStatus.Success) {
            let logsModal = document.querySelector(`#logs`)! as HTMLDivElement;
            if (logsModal) {
                if (apiResponse?.log) {

                    logsModal.innerHTML = logsModal.innerHTML + apiResponse.log;
                }

                logAdded = true;
            }

            let browserData = (await _storage.CacheStorage).GetDataFromLocalStorage(`${objPrimary.CacheKey}|${objPrimary.ClientId}`, objPrimary.UseBrowserCache) as Promise<_type.StorageData<_type.ApiResponse<object[] | any[]>>>;

            if (browserData) {
                switch (await EventBinding.GetApplicationState()) {
                    case _type.ApplicationState.Index:
                    case _type.ApplicationState.New:
                        (await browserData).Data.Result.push(await apiResponse.Result);
                        (await browserData).Data.recordCount = (await browserData).Data.Result.length;
                        break;
                    case _type.ApplicationState.Save:
                    case _type.ApplicationState.Modify:
                        {
                            (await browserData).Data.Result = (await browserData).Data.Result.map(record => {
                                if (record.id === RecordId) {
                                    record = (apiResponse.Result);
                                    return record;
                                }
                                return record;
                            })
                            break;
                        }
                    case _type.ApplicationState.Delete:
                        break;
                }
                let logsModal = document.querySelector(`#logs`)! as HTMLDivElement;
                if (logsModal && !logAdded) {

                    logsModal.innerHTML = logsModal.innerHTML + apiResponse.log;
                }
                browserData.then(async (newData) => {

                    (await (_storage.CacheStorage)).SetDataToLocalStorage(newData.Data, objPrimary.CacheKey, `${objPrimary.CacheKey}|${objPrimary.ClientId}`, 30);
                    // if (newData?.recordCount > 0) {
                    //     await clsPrimary.FillDataOnIndexTable((responseData.Result));
                    // }

                    await clsPrimary.FillDataOnIndexTable(newData.Data.Result);
                });

            }
            await EventBinding.SetApplicationState(_type.ApplicationState.Save);
            await EventBinding.ResetOperationButtons(await EventBinding.GetApplicationState());

            const btnCancle = document.querySelector(`#CancelRecord`)! as HTMLAnchorElement;
            if (btnCancle) {

                btnCancle.click();
            }
        }
        //else {
        //     toastr.error("Something went wrong!", "Error");
        // }
        alertDialog.style.display = `none`;
        _utils.GenericFunctionality.HtmlLogs('ValidateForm', 'Execution completed', '')
        return Promise.resolve(formValidated);
    }
    //to submit form, trigger click event
    private static SaveRecord(): boolean {
        _utils.GenericFunctionality.HtmlLogs(`SaveRecord`, `Triggering click event to submit form!.`, '');
        const submitBtnForFormSave = document.querySelector(`#submit-btn`)! as HTMLButtonElement;
        submitBtnForFormSave.click();
        // const modalForm = document.querySelector('#modelForm')! as HTMLFormElement;
        // modalForm.submit();

        return false;

    }

    public static async GetApiToken(): Promise<_type.StorageData<_type.TokenObject>> {
        return (await (_storage.CacheStorage)).GetToken("Token").then(bToken => bToken as _type.StorageData<_type.TokenObject>);
    }
    public static async ResetOperationButtons(operation: _type.ApplicationState): Promise<void> {

        //Get element for record operations
        const saveLiElement = document.querySelector('#saveLi')! as HTMLLIElement;
        const cancelLiElement = document.querySelector('#cancelLi')! as HTMLLIElement;
        const newRecordLiElement = document.querySelector('#newRecordLi')! as HTMLLIElement;
        const modalForm = document.querySelector('#modelForm')! as HTMLFormElement;

        switch (operation) {
            case _type.ApplicationState.Index:
            case _type.ApplicationState.New:
                saveLiElement.style.display = 'block';
                cancelLiElement.style.display = 'block';
                newRecordLiElement.style.display = 'none';
                modalForm.reset();
                _moduleFunctions.GenericModuleFunctions.ResetFormData()
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
                _moduleFunctions.GenericModuleFunctions.ResetFormData()
                break;
            case _type.ApplicationState.Save:
                saveLiElement.style.display = 'none';
                cancelLiElement.style.display = 'none';
                newRecordLiElement.style.display = 'block';
                modalForm.reset();
                _moduleFunctions.GenericModuleFunctions.ResetFormData()
                break;

            default:
                break;
        }
        _utils.GenericFunctionality.HtmlLogs(`ResetOperationButtons`, 'Application status', operation);

    }
    public static async FindAndRemoveEventsOfIndexTable(): Promise<void> {

        //finding and binding event listener for delete button
        const dataDeleteBtn = Array.from(document.querySelectorAll('[index-delete-data-id]')! as NodeList);
        dataDeleteBtn.forEach((e) => {
            EventBinding.recreateNode(e);
        });

        const dataEditBtn = Array.from(document.querySelectorAll('[index-edit-data-id]')! as NodeList);
        dataEditBtn.forEach((e) => {
            EventBinding.recreateNode(e);
        });
        _utils.GenericFunctionality.HtmlLogs(`FindAndRemoveEventsOfIndexTable`, 'recreateNode for binding edit and delete event!', `[index-delete-data-id] & [index-edit-data-id]`);
    }
    public static async FindAndBindEventsOfIndexTable(objPrimary: object, clsObject: object): Promise<void> {
        //finding and binding event listener for delete button
        const dataDeleteBtn = Array.from(document.querySelectorAll('[index-delete-data-id]')! as NodeList);
        dataDeleteBtn.forEach((e) => {
            e.addEventListener('click', function (this) {
                EventBinding.BindDeleteEvent(this, objPrimary);
                let eElement = e as HTMLButtonElement;
                _utils.GenericFunctionality.HtmlLogs(`BindDeleteEvent`, `event registered for Delete Id`, eElement.getAttribute('index-delete-data-id'));

            });

        });

        const dataEditlink = Array.from(document.querySelectorAll('[index-edit-data-id]')! as NodeList);
        dataEditlink.forEach((e) => {
            e.addEventListener('click', function (this) {
                EventBinding.BindEditEvent(this, objPrimary, clsObject)
                let eElement = e as HTMLButtonElement;
                _utils.GenericFunctionality.HtmlLogs(`BindEditEvent`, `event registered for Edit Id`, eElement.getAttribute('index-edit-data-id'));

            });
        });

    }
    private static recreateNode(el, withChildren?) {
        if (withChildren) {
            el.parentNode.replaceChild(el.cloneNode(true), el);
        }
        else {
            var newEl = el.cloneNode(false);
            while (el.hasChildNodes()) newEl.appendChild(el.firstChild);
            el.parentNode.replaceChild(newEl, el);
        }
    }
    //this function is resposible to edit the record.
    private static async BindEditEvent(e, objPrimary: object | any, clsObject: object | any): Promise<void> {
        EventBinding.SetApplicationState(_type.ApplicationState.Modify);
        let recordId = e.getAttribute('index-edit-data-id');
        if (objPrimary.AdditionalFunctionalityOnBeforeEditEntry) {
            objPrimary.AdditionalFunctionalityOnBeforeEditEntry();
        }
        _utils.GenericFunctionality.HtmlLogs(`BindEditEvent`, `event registered for Edit Id`, recordId);
        objPrimary.RecordId = recordId;
        const url = _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/get`, objPrimary.UseBrowserCache, [`id`], [objPrimary.RecordId])
        _utils.GenericFunctionality.HtmlLogs(`BindEditEvent`, `Model Primary Object`, objPrimary);
        await _moduleFunctions.GenericModuleFunctions.GetModelData(url, await EventBinding.GetApplicationState(), objPrimary, clsObject)
        //it will run before edit record.

        if (objPrimary.AdditionalFunctionalityOnAfterEditEntry) {
            objPrimary.AdditionalFunctionalityOnAfterEditEntry();
        }

    }
    private static BindDeleteEvent(e, objPrimary: object | any) {
        objPrimary.ApplicationState = _type.ApplicationState.Delete;
        let deleteRecordId = e.getAttribute('index-delete-data-id');
        _utils.GenericFunctionality.HtmlLogs(`BindDeleteEvent`, `Delete button clicked with : [index-delete-data-id]`, deleteRecordId);
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
            } as SweetAlertOptions).then(async (result) => {
                if (result.value) {
                    const url = _utils.GenericFunctionality.buildUrl(`${objPrimary.BaseUrl}/${objPrimary.ApiModule}/delete`, objPrimary.UseBrowserCache, ['id'], [deleteRecordId])

                    // const delResponse = await clsObject.DeleteDataUsingApi(url);

                    let apiResponse = await _apiModule.ApiFunctionality.DeleteApi<_type.ApiResponse<object | any>>(url, (await EventBinding.GetApiToken()).Data);
                    let browserData: any;
                    if (apiResponse && apiResponse.status === _type.ResponseStatus.Success) {
                        //remove element from index table
                        currentRow.parentNode.removeChild(currentRow);

                        //when data available on api server, get data from browser storage and delete that and store again in browser storage.
                        //${token.then(result=>{result.Data.ClientId.ClientId})}
                        browserData = await (await _storage.CacheStorage).GetDataFromLocalStorage(`${objPrimary.CacheKey}|${objPrimary.ClientId}`, objPrimary.UseBrowserCache) as Promise<_type.StorageData<_type.ApiResponse<object[] | any[]>>>;

                        //once transaction is completed at backend, now we have to read browser cache and remove the selected data and store again.
                        if (await browserData) {
                            let response = await browserData as _type.StorageData<_type.ApiResponse<object[] | any>>;
                            let newRecord = response.Data.Result.filter(e =>
                                e.id != apiResponse.Result.id);

                            response.Data.Result = newRecord;

                            //setting record count
                            response.Data.recordCount = newRecord.length;
                            (await _storage.CacheStorage).SetDataToLocalStorage(response.Data, objPrimary.CacheKey, `${objPrimary.CacheKey}|${objPrimary.ClientId}`, 30)
                            let logsModal = document.querySelector(`#logs`)! as HTMLDivElement;


                            //adding log on log history.
                            if (logsModal) {
                                logsModal.innerHTML = logsModal.innerHTML + apiResponse.log;
                            }

                        }
                    }
                    // else {
                    //     toastr.error("Something went wrong!", "Error");
                    // }
                }
            });
        }
    }
}