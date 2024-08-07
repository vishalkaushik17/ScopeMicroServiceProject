import * as _utils from "../Security/utils";
import * as _type from "../CustomType/customTypes";

export const SPAModules = function(AssetsStaticPath:string,ModuleName:_type.ModuleName,isCssModuleAvailable:boolean,isJsModuleAvailable:boolean,jsVersion:string,cssVersion:string){
if(isJsModuleAvailable)
_utils.InsertNodeDynamicly.loadJS(`${AssetsStaticPath}/scripts/${ModuleName.toLowerCase()}.min.js`,document.body,ModuleName,jsVersion);
if(isCssModuleAvailable)
_utils.InsertNodeDynamicly.loadCss(`${AssetsStaticPath}/styles/${ModuleName.toLowerCase()}.css`,document.head,ModuleName,cssVersion);

    // switch (ModuleName) {
    //     case  _type.ModuleName.DashBoard:
        
    //     if(isJsModuleAvailable)
    //         _utils.InsertNodeDynamicly.loadJS(`${AssetsStaticPath}/scripts/${ModuleName.toLowerCase()}.min.js`,document.body,ModuleName,jsVersion);
    //     if(isCssModuleAvailable)
    //         _utils.InsertNodeDynamicly.loadCss(`${AssetsStaticPath}/styles/${ModuleName.toLowerCase()}.css`,document.head,ModuleName,cssVersion);
        
    //     break;
    //     case _type.ModuleName.iifeIndexPageFunction:
    //         if(isJsModuleAvailable)
    //             _utils.InsertNodeDynamicly.loadJS(`${AssetsStaticPath}/scripts/${ModuleName.toLowerCase()}.min.js`,document.body,ModuleName,jsVersion);
    //         if(isCssModuleAvailable)
    //             _utils.InsertNodeDynamicly.loadCss(`${AssetsStaticPath}/styles/${ModuleName.toLowerCase()}.css`,document.head,ModuleName,cssVersion);
            
    //         break;
    //     case  _type.ModuleName.AuthorIndex:
        
    //         if(isJsModuleAvailable)
    //             _utils.InsertNodeDynamicly.loadJS(`${AssetsStaticPath}/scripts/${ModuleName.toLowerCase()}.min.js`,document.body,ModuleName,jsVersion);
    //         if(isCssModuleAvailable)
    //             _utils.InsertNodeDynamicly.loadCss(`${AssetsStaticPath}/styles/${ModuleName.toLowerCase()}.css`,document.head,ModuleName,cssVersion);
            
    //         break;
    
    //     default:
    //         break;
    // }
}