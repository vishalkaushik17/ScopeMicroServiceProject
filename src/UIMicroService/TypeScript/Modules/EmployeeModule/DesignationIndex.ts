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


// var recordId:string = '0';
const objModuleName: _type.ModuleSpecificObjectType = {
    DictionaryObjectForApiModuleCall:{},
    ClientId: `Default`,
    ApplicationState: _type.ApplicationState.Index,
    CacheKey: "DesignationModule",
    ApiModule: 'EmployeeDesignation',
    ApiType:_type.ApiType.Json,
    UseBrowserCache: true,
    UseRedisCache: true,
    BaseUrl: `https://localhost/ApiGateway`,
    RecordId: ``,
    AsideBarLink: `EmployeeDesignation-MasterModule`,
    AsideBarModuleId: `EmployeeLi`, //aside bar link id to dispaly active
    IndexTitle: `Designations`,
    ApplyCssOnModule: _moduleFunctions.ApplyEffectOnModuleSpecific,
    DbTarget: _type.TargetDataBase.ClientSpecific, // from which database, data should come
    UploadFile: true,
    //AdditionalApiModules: [{ ApiModule: "EmployeeDepartment", CacheKey: "DepartmentModule", FieldName: "Department", FieldNameReflectedId: `DepartmentId`, ResultPropertyName: "name", ResultPropertyId: "id", FieldType: _type.ElementType.LIST }],
    AdditionalFunctionalityOnNewEntry: RunAdditionalFunctionalityOnNewEntry,
    FetchAdditionalDataOnIndexPage:FetchAdditionalDataOnIndexPage,
    AdditionalEventListenerFunctionality: AdditionalEventListenerFunctionality,
    AdditionalFunctionalityOnAfterEditEntry:AdditionalFunctionalityOnAfterEditEntry,

}

/**Run additional functionality on this module when edit button clicked */
async function AdditionalFunctionalityOnAfterEditEntry(){
    _utils.GenericFunctionality.HtmlLogs(`AdditionalFunctionalityOnAfterEditEntry`, `Execution started!`,'');
    
    let departmentDropDown = document.querySelector(`#Department`)! as HTMLSelectElement;
    let departmentId = document.querySelector(`#DepartmentId`)! as HTMLSelectElement;
    _utils.GenericFunctionality.HtmlLogs(`AdditionalFunctionalityOnAfterEditEntry`, `Setting dropdown list using value`,departmentId.value);
    _utils.GenericFunctionality.SetSelectedValue(departmentDropDown, departmentId.value);
    _utils.GenericFunctionality.HtmlLogs(`AdditionalFunctionalityOnAfterEditEntry`, `Execution end!`,'');
   
}

/** Add additional functionality on this page to register event on difference types of elements, which are not common on 
 * all modules or perform other kind of task.  It will get loaded on iife execution.*/
async function AdditionalEventListenerFunctionality() {
    _utils.GenericFunctionality.HtmlLogs(`AdditionalEventListenerFunctionality`, `Execution started!`,'');

    let departmentListElement = document.getElementById('Department')! as HTMLSelectElement;
    let hiddenDepartmentId = document.getElementById('DepartmentId')! as HTMLInputElement;
    departmentListElement.addEventListener("change", function (this) {
        _utils.GenericFunctionality.HtmlLogs(`AdditionalEventListenerFunctionality`, `event registered : change!`,'');
        _utils.GenericFunctionality.HtmlLogs(`AdditionalEventListenerFunctionality`, `departmentId`,this.value);
        hiddenDepartmentId.value = this.value;
    });

    let isDepartmentHeadElement = document.getElementById(`IsHeadPosition`)! as HTMLInputElement;

    if (isDepartmentHeadElement){
        isDepartmentHeadElement.addEventListener("click", async function (this) {
            
            let AllowedSeatsElement = document.getElementById(`AllowedSeats`)! as HTMLInputElement;
            if (isDepartmentHeadElement.checked){
                AllowedSeatsElement.value = '1';
                AllowedSeatsElement.readOnly =true;
            }else{
                if (await _events.EventBinding.GetApplicationState() !== _type.ApplicationState.Modify ){
                    AllowedSeatsElement.value = '0';
                }
                AllowedSeatsElement.readOnly =false;
            }
        });
    
    }
    _utils.GenericFunctionality.HtmlLogs(`AdditionalEventListenerFunctionality`, `Execution end!`,'');

}
/** Function get executed when new entry button get clicked. */
async function RunAdditionalFunctionalityOnNewEntry() {

    _utils.GenericFunctionality.HtmlLogs(`RunAdditionalFunctionalityOnNewEntry`, `Execution started!`,'');
    const departmentUrl = _utils.GenericFunctionality.buildUrl(`${objModuleName.BaseUrl}/${objModuleName.DictionaryObjectForApiModuleCall![_type.ApiModuleKey.Department]}/getall`, objModuleName.UseRedisCache)
    let Data = await _moduleFunctions.GenericModuleFunctions.GetAdditionalModelData(departmentUrl, await _events.EventBinding.GetApplicationState(), objModuleName, objModuleName.DictionaryObjectForApiModuleCall![_type.ApiModuleKey.Department],true);
    let departmentListElement = document.querySelector('#Department')! as HTMLSelectElement;
    //remove option list from dropdown list
    _utils.GenericFunctionality.HtmlLogs(`RunAdditionalFunctionalityOnNewEntry`, `Removing existing options from department dropdown!`,'');

    _utils.GenericFunctionality.removeOptions(departmentListElement);

    //create a default option in dropdown
    // let el = document.createElement("option");
    // el.textContent = "Select Department";
    // el.value = ``;
    // departmentListElement.appendChild(el);
    _utils.GenericFunctionality.addDefaultRecordOnSelectElement("Select Department",departmentListElement)
    _utils.GenericFunctionality.HtmlLogs(`RunAdditionalFunctionalityOnNewEntry`, `adding new records to department dropdown!`,'');

    
    _utils.GenericFunctionality.HtmlLogs(`RunAdditionalFunctionalityOnNewEntry`, `Execution completed!`,'');

}

/** Function get executed before index page get loaded. It will also get executed on refresh index page button clicked.*/
async function FetchAdditionalDataOnIndexPage() {
    
    let apiDictionaryForDepartment = objModuleName.DictionaryObjectForApiModuleCall![_type.ApiModuleKey.Department];
    _utils.GenericFunctionality.HtmlLogs(`FetchAdditionalDataOnIndexPage`, `Execution started!`,'');
    const departmentUrl = _utils.GenericFunctionality.buildUrl(`${objModuleName.BaseUrl}/${apiDictionaryForDepartment.ApiModule}/getall`, objModuleName.UseRedisCache)
    let Data = await _moduleFunctions.GenericModuleFunctions.GetAdditionalModelData(departmentUrl, await _events.EventBinding.GetApplicationState(), objModuleName, apiDictionaryForDepartment!,false);
    let departmentListElement = document.querySelector('#Department')! as HTMLSelectElement;
    //remove option list from dropdown list
    _utils.GenericFunctionality.HtmlLogs(`FetchAdditionalDataOnIndexPage`, `Removing existing options from department dropdown!`,'');

    _utils.GenericFunctionality.removeOptions(departmentListElement);
    _utils.GenericFunctionality.addDefaultRecordOnSelectElement(`Select Department`,departmentListElement);

    
    _utils.GenericFunctionality.HtmlLogs(`FetchAdditionalDataOnIndexPage`, `adding new records to department dropdown!`,'');

    Data.Result.forEach(element => {

        let el = document.createElement("option");
        el.textContent = element.name;
        el.value = element.id;
        departmentListElement.appendChild(el);
    });
    _utils.GenericFunctionality.HtmlLogs(`FetchAdditionalDataOnIndexPage`, `Execution completed!`,'');

}

export class clsIndexPage {

    //module specific data table binding
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
            templateRow = templateRow.replace(`<<RECORDTITLE>>`, record.name  +(record.isHeadPosition === true ? ` <span><strong style='background:green;color:white;border:1px solid;padding:0.1em;'> HEAD OF THE DEPARTMENT </strong></span>`: " Normal"));
            templateRow = templateRow.replace(`<<CRUDICON>>`, record.isEditable ? `${editIcon}  ${delIcon}` : '')
            templateRow = templateRow.replace(`<<EDITRECORDID>>`, record.isEditable ? `${record.id}` : '')
            templateRow = templateRow.replace(`<<DELETERECORDID>>`, record.isEditable ? `${record.id}` : '')
            templateRow += `<p class="text-blue">Department :&nbsp;<span class="text-black">${record.department.name}</span></p>`;
            // if (record.isHeadPosition){
            //     templateRow += `<br><span><strong style='background:green;color:white;border:1px solid;'> HEAD OF THE DEPARTMENT</strong></span>`;
            // }
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

    //first function which executed on page load.
    const Init = async function () {
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `Auto init starts for page.`, '');
        objModuleName.DictionaryObjectForApiModuleCall = _apiModule.ApiFunctionality.InitApiDictionary(objModuleName.DictionaryObjectForApiModuleCall);
        let token = await (await _storage.CacheStorage).GetToken("Token").then(bToken => bToken as _type.StorageData<_type.TokenObject>);
        objModuleName.ClientId = token.Data.ClientId;
        //setting document title and page title
        _moduleFunctions.GenericModuleFunctions.SetIndexPageTitle(objModuleName);

        //set default application state
        _events.EventBinding.SetApplicationState(_type.ApplicationState.Index);

        //kind of operational behavior for cache
        const GetAppBehavior = await (await _storage.CacheStorage).GetConfigData(`Config`);
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `GetAppBehavior`, GetAppBehavior);

        //getting token for application api transactions.
        if (GetAppBehavior) {
            const AppBehavior = GetAppBehavior as _type.StorageData<_type.AppConfigType>;
            objModuleName.UseBrowserCache = AppBehavior.Data.UseBrowserCache;
            objModuleName.UseRedisCache = AppBehavior.Data.UseRedisCache;
        }

        //build getall records url as per objModuleName object properties.
        const url = _utils.GenericFunctionality.buildUrl(`${objModuleName.BaseUrl}/${objModuleName.ApiModule}/getall`, objModuleName.UseRedisCache)

        //fetching records and setting browser cache and filling index page.
        //inside function we are calling clsIndexPage.FillDataOnIndexTable using reference and filling index page.
        await _moduleFunctions.GenericModuleFunctions.GetModelData(url, await _events.EventBinding.GetApplicationState(), objModuleName, clsIndexPage);

        //register event listener based on index page extra elements or form.cshtml extra elements
        if (objModuleName.AdditionalEventListenerFunctionality) {

            await objModuleName.AdditionalEventListenerFunctionality();
        }


        //it will run before index fetch record.
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
