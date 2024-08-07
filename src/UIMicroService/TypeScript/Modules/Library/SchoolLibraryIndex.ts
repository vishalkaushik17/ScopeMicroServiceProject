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
const objModuleName: _type.ModuleSpecificObjectType =
{
    ClientId: `Default`,
    ApplicationState: _type.ApplicationState.Index,
    CacheKey: "LibraryModule",
    ApiModule: 'Library',
    ApiType:_type.ApiType.Json,
    UseBrowserCache: true,
    UseRedisCache: true,
    BaseUrl: `https://localhost/ApiGateway`,
    RecordId: ``,
    AsideBarLink: `SchoolLibrary-LibraryModule`,
    AsideBarModuleId: `LibraryLI`, //aside bar link id to dispaly active
    IndexTitle: `Libraries`,
    ApplyCssOnModule: _moduleFunctions.ApplyEffectOnModuleSpecific,
    DbTarget: _type.TargetDataBase.ClientSpecific, // from which database, data should come
    UploadFile: false,

}
//change aside bar li and ul element colors on load


export class clsIndexPage {
    public static templateLayoutForIndex(templateRecord: string): void {
        let template: string = `<div class='col-md-12'>${templateRecord}</div>`;
        const pageContent = document.querySelector(`#indexDataTable`)! as HTMLDivElement;
        pageContent.innerHTML = template;
    }
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
            templateRow = templateRow.replace(`<<RECORDTITLE>>`, record.name)
            templateRow = templateRow.replace(`<<CRUDICON>>`, record.isEditable ? `${editIcon}  ${delIcon}` : '')
            templateRow = templateRow.replace(`<<EDITRECORDID>>`, record.isEditable ? `${record.id}` : '')
            templateRow = templateRow.replace(`<<DELETERECORDID>>`, record.isEditable ? `${record.id}` : '')
            templateRow += `<p class="text-blue">Location :&nbsp;<span class="text-black">${record.location}</span></p>`;
            templateRow += `<p class="text-blue">Description :&nbsp;<span class="text-black">${record.description}</span></p>`;
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
            _events.EventBinding.FindAndRemoveEventsOfIndexTable();
            _events.EventBinding.FindAndBindEventsOfIndexTable(objModuleName, clsIndexPage);
        });

    }
}

//self executable iife function.
const iifeIndexPageFunction = function () {
    const Init = async function () {
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `Auto init starts for page.`, '');
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

        //binding events on init
        await _events.EventBinding.FindElementsAndBindEventsOfModuleForm(clsIndexPage, objModuleName);
        objModuleName.ApplyCssOnModule = _moduleFunctions.ApplyEffectOnModuleSpecific.ApplyCssOnModule(objModuleName.AsideBarModuleId, objModuleName.AsideBarLink);
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `objModuleName`, objModuleName);
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `Auto init execution completed!.`, '');
    }
    return {
        Init: Init
    }
}();
iifeIndexPageFunction.Init();   
