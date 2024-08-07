import * as _type from "../CustomType/customTypes"
export const IdentityController = function(){
    //it will read token config from layout page.
    const ReadTokenFromLayoutPage = function(element:HTMLInputElement):_type.StorageData<_type.TokenObject>|undefined{
        if (element.hasAttribute('SchoolName') &&  element.hasAttribute('Token') && element.hasAttribute('UserId'))
        {
            let jsTokenObject:_type.StorageData<_type.TokenObject> = { 
                Data : {
                            UN: element!.getAttribute('SchoolName')!,
                            TK: element!.getAttribute('Token')!,
                            Id: element!.getAttribute('UserId')!,
                            SessionId:element!.getAttribute('SessionId')!,
                            ScopeId:element!.getAttribute('ScopeId')!,
                            UserId:element!.getAttribute('UserId')!,
                            ClientId:element!.getAttribute('ClientId')!,
                            SetupEnvironment:element!.getAttribute('SetupEnvironment')! as _type.Environment,
                        },
                Expiry: 60,
                Id:"Token",
                
            };
            return jsTokenObject;
        }
            return undefined;
    }
    return{
        ReadToken:ReadTokenFromLayoutPage
    }
}();