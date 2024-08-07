import * as _events from "../EventRegistraion"
import * as _moduleFunctions from "../ModuleFunctions";
import * as _apiModule from "../../ApiMethods/ApiMethod"
import * as _storage from "../../BrowserStorage/tsStorage"
import * as _type from "../../CustomType/customTypes";
import * as _utils from "../../Security/utils"
import * as _notifications from "../../UIComponents/Notifications/NotificationComponents"
import * as jQuery from 'jquery';
import Swal, { SweetAlertOptions } from 'sweetalert2';// const swal = swals as any;
import DataTable from 'datatables.net';
import 'datatables.net-bs';
import 'select2';
import 'js-datepicker';

// var recordId:string = '0';
const objModuleName: _type.ModuleSpecificObjectType = {
    ClientId: `Default`,
    ApplicationState: _type.ApplicationState.Index,
    CacheKey: "EmployeeModule",
    ApiModule: 'Employee',//it mean api url
    ApiType: _type.ApiType.Json, //it shows that form has some file sending kind of process attached eg. photo or excel.
    UseBrowserCache: true,
    UseRedisCache: true,
    BaseUrl: `https://localhost/ApiGateway`,
    RecordId: ``,
    AsideBarLink: `Employee-MasterModule`,
    AsideBarModuleId: `EmployeeLi`, //aside bar link id to dispaly active
    IndexTitle: `Employees`,
    ApplyCssOnModule: _moduleFunctions.ApplyEffectOnModuleSpecific,
    DbTarget: _type.TargetDataBase.ClientSpecific, // from which database, data should come
    UploadFile: true,
    AdditionalFunctionalityOnNewEntry: RunAdditionalFunctionalityOnNewEntry,
    FetchAdditionalDataOnIndexPage: FetchAdditionalDataOnIndexPage,
    AdditionalEventListenerFunctionality: AdditionalEventListenerFunctionality,
    AdditionalFunctionalityOnAfterEditEntry: AdditionalFunctionalityOnAfterEditEntry,
}

/**Run additional functionality on this module when edit button clicked */
async function AdditionalFunctionalityOnAfterEditEntry() {
    _utils.GenericFunctionality.HtmlLogs(`AdditionalFunctionalityOnAfterEditEntry`, `Execution started!`, '');

    let departmentDropDown = document.querySelector(`#Department`)! as HTMLSelectElement;
    let departmentId = document.querySelector(`#DepartmentId`)! as HTMLSelectElement;
    _utils.GenericFunctionality.HtmlLogs(`AdditionalFunctionalityOnAfterEditEntry`, `Setting dropdown list using value`, departmentId.value);
    _utils.GenericFunctionality.SetSelectedValue(departmentDropDown, departmentId.value);
    _utils.GenericFunctionality.HtmlLogs(`AdditionalFunctionalityOnAfterEditEntry`, `Execution end!`, '');

}

/** Add additional functionality on this page to register event on difference types of elements, which are not common on 
 * all modules or perform other kind of task.  It will get loaded on iife execution.*/
async function AdditionalEventListenerFunctionality() {
    _utils.GenericFunctionality.HtmlLogs(`AdditionalEventListenerFunctionality`, `Execution started!`, '');

    //make searchable dropdown
    //applying select2 dropdown effect on normal select list element.
    jQuery('#BloodGroup').select2({
        placeholder: "Select",
    });

    jQuery('#EmploymentType').select2({
        placeholder: "Select",
    });
    jQuery('#Citizenship').select2({
        placeholder: "Select",
    });
    jQuery('#Gender').select2({
        placeholder: "Select",
    });
    jQuery('#Religion').select2({
        placeholder: "Select",
    });
    jQuery('#MaritalStatus').select2({
        placeholder: "Select",
    });
    jQuery('#BloodGroup').select2({
        placeholder: "Select",
    });

    jQuery('#DesignationId').select2({
        placeholder: "Select",
    });
    $(function () {
        $("#DateOfBirthString").datepicker(
            {
                dateFormat: "DD-MMM-YYYY",
                minDate: _utils.GenericFunctionality.getFormattedDate(new Date())
            });
    });
    // jQuery('#EmploymentType').on('change', function() {
    //     console.log(this);
    //     const selectedValue = $(this).val();
    //     console.log('Selected value2:', selectedValue);

    // });

    // let designationListElement = document.getElementById('Designation')! as HTMLSelectElement;
    // let hiddenDepartmentId = document.getElementById('DesignationId')! as HTMLInputElement;
    // designationListElement.addEventListener("change", function (this) {
    //     _utils.GenericFunctionality.HtmlLogs(`AdditionalEventListenerFunctionality`, `event registered : change!`, '');
    //     _utils.GenericFunctionality.HtmlLogs(`AdditionalEventListenerFunctionality`, `departmentId`, this.value);
    //     hiddenDepartmentId.value = this.value;
    // });
    _utils.GenericFunctionality.HtmlLogs(`AdditionalEventListenerFunctionality`, `Execution end!`, '');

}

/** Function get executed when new entry button get clicked. */
async function RunAdditionalFunctionalityOnNewEntry() {

    _utils.GenericFunctionality.HtmlLogs(`RunAdditionalFunctionalityOnNewEntry`, `Execution started!`, '');
    const designationUrl = _utils.GenericFunctionality.buildUrl(`${objModuleName.BaseUrl}/${objModuleName.DictionaryObjectForApiModuleCall![_type.ApiModuleKey.Department]}/getall`, objModuleName.UseRedisCache)
    let Data = await _moduleFunctions.GenericModuleFunctions.GetAdditionalModelData(designationUrl, await _events.EventBinding.GetApplicationState(), objModuleName, objModuleName.DictionaryObjectForApiModuleCall![_type.ApiModuleKey.Department], true);
    let designationListElement = document.querySelector('#Department')! as HTMLSelectElement;
    //remove option list from dropdown list
    _utils.GenericFunctionality.HtmlLogs(`RunAdditionalFunctionalityOnNewEntry`, `Removing existing options from department dropdown!`, '');

    _utils.GenericFunctionality.removeOptions(designationListElement);

    //create a default option in dropdown
    // let el = document.createElement("option");
    // el.textContent = "Select Department";
    // el.value = ``;
    // designationListElement.appendChild(el);
    _utils.GenericFunctionality.addDefaultRecordOnSelectElement("Select Department", designationListElement)
    _utils.GenericFunctionality.HtmlLogs(`RunAdditionalFunctionalityOnNewEntry`, `adding new records to department dropdown!`, '');


    _utils.GenericFunctionality.HtmlLogs(`RunAdditionalFunctionalityOnNewEntry`, `Execution completed!`, '');

}

/** Function get executed before index page get loaded. It will also get executed on refresh index page button clicked.*/
async function FetchAdditionalDataOnIndexPage() {
    _utils.GenericFunctionality.HtmlLogs(`FetchAdditionalDataOnIndexPage`, `Execution started!`, '');

    try {
        //get object dictionary settings for designation key
        let apiDictionaryForDesignation = objModuleName.DictionaryObjectForApiModuleCall![_type.ApiModuleKey.Designation];
        let apiDictionaryForBank = objModuleName.DictionaryObjectForApiModuleCall![_type.ApiModuleKey.Bank];
        let apiDictionaryForEmployee = objModuleName.DictionaryObjectForApiModuleCall![_type.ApiModuleKey.Employee];
        debugger;
        //building url to fetch api data
        const designationUrl = _utils.GenericFunctionality.buildUrl(`${objModuleName.BaseUrl}/${apiDictionaryForDesignation.ApiModule}/getall`, objModuleName.UseRedisCache)
        const bankUrl = _utils.GenericFunctionality.buildUrl(`${objModuleName.BaseUrl}/${apiDictionaryForBank.ApiModule}/getall`, objModuleName.UseRedisCache)
        const employeeUrl = _utils.GenericFunctionality.buildUrl(`${objModuleName.BaseUrl}/${apiDictionaryForEmployee.ApiModule}/getall`, objModuleName.UseRedisCache)

        //Now passing dictionary settings and url to fetch data from api/browser cache
        debugger;
        let designationData = await _moduleFunctions.GenericModuleFunctions.GetAdditionalModelData(designationUrl, await _events.EventBinding.GetApplicationState(), objModuleName, apiDictionaryForDesignation!, false);
        let bankData = await _moduleFunctions.GenericModuleFunctions.GetAdditionalModelData(bankUrl, await _events.EventBinding.GetApplicationState(), objModuleName, apiDictionaryForBank!, false);
        let reportingHeadEmployeeData = await _moduleFunctions.GenericModuleFunctions.GetAdditionalModelData(employeeUrl, await _events.EventBinding.GetApplicationState(), objModuleName, apiDictionaryForEmployee!, false);

        let designationListElement = document.querySelector('#DesignationId')! as HTMLSelectElement;
        let bankListElement = document.querySelector('#BankId')! as HTMLSelectElement;
        let employeeListElement = document.querySelector('#ReportingHeadId')! as HTMLSelectElement;

        //checking element existence on DOM
        if (designationListElement) {
            _utils.GenericFunctionality.HtmlLogs(`FetchAdditionalDataOnIndexPage`, `adding new records to department dropdown!`, '');

            //as per browser cache /api data we are adding optons and to dropdown list.
            if (designationData.recordCount > 0) {
                jQuery("#DesignationId").append(`<option value="-1">Select</option>`);
                designationData.Result.forEach(element => {
                    jQuery("#DesignationId").append(`<option value="${element.id}">${element.name}</option>`);
                });
            } else {
                jQuery("#ReportingHeadId").append(`<option value="-1">No Data</option>`);
            }
        }

        //checking element existence on DOM
        if (bankListElement) {
            if (bankData.recordCount > 0) {
                jQuery("#BankId").append(`<option value="-1">Select</option>`);
                bankData.Result.forEach(element => {
                    jQuery("#BankId").append(`<option value="${element.id}">${element.name}</option>`);
                });
            } else {
                jQuery("#ReportingHeadId").append(`<option value="-1">No Data</option>`);
            }
        }

        //checking element existence on DOM
        if (employeeListElement) {

            if (reportingHeadEmployeeData.recordCount > 0) {
                jQuery("#ReportingHeadId").append(`<option value="-1">Select</option>`);
                reportingHeadEmployeeData.Result.forEach(element => {
                    jQuery("#ReportingHeadId").append(`<option value="${element.id}">${element.firstName} ${element.lastName}</option>`);
                });
            } else {
                jQuery("#ReportingHeadId").append(`<option value="-1">No Data</option>`);
            }
        }
    } catch (error) {
        _utils.GenericFunctionality.HtmlLogs(`FetchAdditionalDataOnIndexPage`, `Error!`, error);

    }
    _utils.GenericFunctionality.HtmlLogs(`FetchAdditionalDataOnIndexPage`, `Execution completed!`, '');
}






/** Index page class */
export class clsIndexPage {
    /**Function is resposible to fill the list of data on index page. */
    public static async FillDataOnIndexTable(result: object[] | any[]) {
        _utils.GenericFunctionality.HtmlLogs(`FillDataOnIndexTable`, `Binding data on index page and registering event on required elements`, result);
        _events.EventBinding.FindAndRemoveEventsOfIndexTable();
        jQuery('#indexDataTable').DataTable().destroy();
        jQuery('#indexDataTable tbody').empty();

        let firstItem: string = `<tr><td><div class="box box-solid box.box-solid-index-page"> <div class="box-header with-border index-page-item-main">
        <i class='index-box-header-icon'><ion-icon name="business-outline"> </ion-icon></i><h3 class="box-title"> &nbsp;${objModuleName.IndexTitle} :<span class="text-black"> <<RECORDTITLE>> </span></h3>
        <span class='pull-right'><<CRUDICON>></span>
        </div> <div class="box-body box-body-index-details">`;

        let templateRow = '';
        let hrefid = `'#modal-default' data-toggle='modal' class='open-Model'`;
        let editIcon = `<a href=${hrefid} index-edit-data-id='<<EDITRECORDID>>'> <i class='btn btn-info index-titile-crud-icon'><ion-icon name='create'></ion-icon></i></a>`;
        let delIcon = "<a href='#' index-delete-data-id='<<DELETERECORDID>>'> <i class='btn btn-warning index-titile-crud-icon'><ion-icon name='trash'></ion-icon></i></a>";


        result.forEach((record) => {
            templateRow += firstItem;
            templateRow = templateRow.replace(`<<RECORDTITLE>>`, `${record.firstName} ${record.middleName} ${record.lastName}`)
            templateRow = templateRow.replace(`<<CRUDICON>>`, record.isEditable ? `${editIcon}  ${delIcon}` : '')
            templateRow = templateRow.replace(`<<EDITRECORDID>>`, record.isEditable ? `${record.id}` : '')
            templateRow = templateRow.replace(`<<DELETERECORDID>>`, record.isEditable ? `${record.id}` : '')
            templateRow += `<span><strong> UID No:</strong> ${record.uidNo}&nbsp;</span>`;
            templateRow += `<br><span><strong> Contact No:</strong> ${record.contactNo} </span>`;

            templateRow += `</div></div></td></tr>`;
            jQuery("#indexDataTable tbody").append(templateRow);
            templateRow = '';
        })
        jQuery("#indexDataTable").DataTable().draw(); // always redraw
        jQuery('#indexDataTable_filter > label > input').addClass("searchRecords");
        jQuery('#indexDataTable_filter > label').addClass("searchLabel");
        _events.EventBinding.FindAndRemoveEventsOfIndexTable();
        _events.EventBinding.FindAndBindEventsOfIndexTable(objModuleName, clsIndexPage);
        //after new page load, bind event again
        jQuery('#indexDataTable').on('draw.dt', function () {
            //remove existing events on edit and delete button
            _events.EventBinding.FindAndRemoveEventsOfIndexTable();
            //re-attach events on edit and delete button.
            _events.EventBinding.FindAndBindEventsOfIndexTable(objModuleName, clsIndexPage);
        });
    }
}

//self executable iife function.
const iifeIndexPageFunction = function () {
    const Init = async function () {
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `Auto init starts for page.`, '');

        objModuleName.DictionaryObjectForApiModuleCall = _apiModule.ApiFunctionality.InitApiDictionary(objModuleName.DictionaryObjectForApiModuleCall);

        let token = await (await _storage.CacheStorage).GetToken("Token").then(bToken => bToken as _type.StorageData<_type.TokenObject>);
        objModuleName.ClientId = token.Data.ClientId;

        _moduleFunctions.GenericModuleFunctions.SetIndexPageTitle(objModuleName);
        //set default application state
        _events.EventBinding.SetApplicationState(_type.ApplicationState.Index);

        const GetAppBehavior = await (await _storage.CacheStorage).GetConfigData(`Config`);
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `GetAppBehavior`, GetAppBehavior);
        if (GetAppBehavior) {
            const AppBehavior = GetAppBehavior as _type.StorageData<_type.AppConfigType>;
            objModuleName.UseBrowserCache = AppBehavior.Data.UseBrowserCache;
            objModuleName.UseRedisCache = AppBehavior.Data.UseRedisCache;
        }

        const url = _utils.GenericFunctionality.buildUrl(`${objModuleName.BaseUrl}/${objModuleName.ApiModule}/getall`, objModuleName.UseRedisCache)
        await _moduleFunctions.GenericModuleFunctions.GetModelData(url, await _events.EventBinding.GetApplicationState(), objModuleName, clsIndexPage);

        //register event listener based on index page extra elements or form.cshtml extra elements
        if (objModuleName.AdditionalEventListenerFunctionality) {

            await objModuleName.AdditionalEventListenerFunctionality();
        }

        //applying additional functionality when page get loaded.
        if (objModuleName.FetchAdditionalDataOnIndexPage) {

            await objModuleName.FetchAdditionalDataOnIndexPage();
        }

        //binding events on init
        await _events.EventBinding.FindElementsAndBindEventsOfModuleForm(clsIndexPage, objModuleName);
        // objModuleName.AppyCssOnModule.ApplyCssOnModule("LibraryLI", objModuleName.AsideBarLink);
        objModuleName.ApplyCssOnModule = _moduleFunctions.ApplyEffectOnModuleSpecific.ApplyCssOnModule(objModuleName.AsideBarModuleId, objModuleName.AsideBarLink);
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `objModuleName`, objModuleName);
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `Auto init execution completed!.`, '');
    }
    return {
        Init: Init
    }
}();
iifeIndexPageFunction.Init();   
