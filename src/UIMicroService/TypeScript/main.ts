import * as _security from "./Security/utils"
import * as _IdentityController from "./Security/IdentityController"
import * as _storage from "./BrowserStorage/tsStorage"
import * as _type from "./CustomType/customTypes"
import * as _spa from "./UIComponents/SPALink"
import * as _schoolLibraryModule from "./Modules/Library/SchoolLibraryIndex"
import * as _authorModule from "./Modules/Library/AuthorIndex"
import * as _utils from "./Security/utils"
import DataTable from 'datatables.net';
import 'datatables.net-bs';

var UseBrowserCache = true;


let AppSuccessLogin = function(){
    //On successful login token will be saved in browser storage.
    const OnSuccessLogin = async function(userToken:_type.StorageData<_type.TokenObject>){
        _utils.GenericFunctionality.HtmlLogs(`AppSuccessLogin.OnSuccessLogin`, `userToken value`, userToken);
            (await _storage.CacheStorage).SetDefaultData();
            (await _storage.CacheStorage).SetToken(userToken);
            _spa.JSLayoutPageComponent.FindSPANavigationLinks();
    };
    const SetDefaultAppConfig = async function(){
        (await _storage.CacheStorage).SetDefaultData();
    }
    return{
        OnSuccessLogin: OnSuccessLogin
    }
}();

//On successful login it will get executed, and read token config.
let tokenConfig = document.getElementById('TokenConfig')! as HTMLInputElement;

if (tokenConfig) 
{
    //reading token and setting in localforage 
    let userToken:_type.StorageData<_type.TokenObject> = _IdentityController.IdentityController.ReadToken(tokenConfig)!;
    if (userToken && userToken.Data.Id !== '' && userToken.Data.TK !== '' && userToken.Data.SessionId !=''){
        AppSuccessLogin.OnSuccessLogin(userToken);
        
        //adding SetupEnvironment in browser cookie
        _utils.GenericFunctionality.addItemToCookie("Environment",userToken.Data.SetupEnvironment,1);
        
        //setting setup environment to handle trace operations and others which are dependent on SetupEnvironment
        _utils.GenericFunctionality.Environment = userToken.Data.SetupEnvironment;
    }       
}