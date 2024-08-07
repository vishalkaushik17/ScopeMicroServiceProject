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
    ApplicationState: _type.ApplicationState.Index,
    CacheKey: "", //for browser cache
    ApiModule: '', //this is api route name
    ApiType: _type.ApiType.Json,
    UseBrowserCache: true,
    UseRedisCache: true,
    BaseUrl: ``,
    RecordId: ``,
    AsideBarLink: `LayoutUl`, //aside bar link class
    AsideBarModuleId: `DashboardLi`, //aside bar link id to dispaly active
    IndexTitle: `Dashboard`, //title of the page
    ApplyCssOnModule: _moduleFunctions.ApplyEffectOnModuleSpecific,
    DbTarget: _type.TargetDataBase.CommonDB, // from which database, data should come
    UploadFile: false,
}
//change aside bar li and ul element colors on load

//self executable iife function.
const iifeIndexPageFunction = function () {
    const Init = async function () {
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `Auto init starts for page.`, '');
        let token = await (await _storage.CacheStorage).GetToken("Token").then(bToken => bToken as _type.StorageData<_type.TokenObject>);
        objModuleName.ClientId = token.Data.ClientId;

        document.querySelectorAll('.active')?.forEach((activeElement) => { activeElement.classList.remove('active'); });
        document.querySelectorAll('.ActiveSideBarLiSpan')?.forEach((activeElement) => { activeElement.classList.remove('ActiveSideBarLiSpan'); });
        document.querySelectorAll(`.selectedBarIcon`).forEach(element => {
            element.classList.remove(`selectedBarIcon`);
        });
        const dashboardUl = document.querySelector(`#LayoutUl`)! as HTMLUListElement;
        if (dashboardUl) {
            dashboardUl.querySelector(`a span`)?.classList.add(`ActiveSideBarLiSpan`);
            dashboardUl.querySelector(`a i`)?.classList.add(`selectedBarIcon`);
        }
        _utils.GenericFunctionality.HtmlLogs(`iifeIndexPageFunction`, `Auto init execution completed!.`, '');
    }
    return {
        Init: Init
    }
}();
iifeIndexPageFunction.Init();   
