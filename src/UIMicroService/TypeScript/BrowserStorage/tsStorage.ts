import * as _security from "../Security/utils"
import * as localforage from "localforage";
import * as _type from "../CustomType/customTypes";
import * as _utils from "../Security/utils";
// Using config()
localforage.config({
    driver: [
        localforage.INDEXEDDB,
        localforage.LOCALSTORAGE,
        localforage.WEBSQL
    ],
    name: 'ScopeERP',     // These fields
    version: 1.0,      // are totally optional
    size: 4980736, // Size of database, in bytes. WebSQL-only for now.
    storeName: 'ScopeERPData', // Should be alphanumeric, with underscores.
    description: 'storing data at client browser for faster operations.'
});

//instance of localforage
let AdminLayout = localforage.createInstance({
    name: "ScopeERP",
    storeName: "ScopeERPData"
});


//storing default value in browser cache
let DefaultValue: _type.AppConfigType = {
    AppVersion: '1.0',
    AppName: 'Scope ERP Software',
    CompanyName: 'Coding Company',
    BuildVersion: '1.0',
    UseRedisCache: true,
    UseBrowserCache: true
}

export const CacheStorage = async function () {
    const SetDataToLocalStorage = async function (value: string | object, id: string, key: string, mins: number) {
        const now = new Date();
        const item: _type.StorageData<string | object | any> = {
            Data: value,
            Id: id,
            Expiry: now.getTime() + (mins * 60000)
        };
        item.Data.log = "";
        return AdminLayout.setItem(key, item)
            .then(function (result) {
                if (result) {
                    result.Data.log = '';
                    _utils.GenericFunctionality.HtmlLogs(`SetDataToLocalStorage - Data stored in browser! Key:`, key, `${JSON.stringify(result)}`);
                    return result;
                } else {
                    _utils.GenericFunctionality.HtmlLogs(`SetDataToLocalStorage - Unable to store data in browser result object is null!`, key, `${JSON.stringify(result)}`);

                    return null;
                }
            })
            .catch(e => {
                console.log(e.message);
                _utils.GenericFunctionality.HtmlLogs(`SetDataToLocalStorage - Error while storing data on browser cache`, 'Error', e.message);

            }); // Received a function as a value

    };
    const GetConfigData = async function (key: string): Promise<_type.StorageData<object> | unknown> {
        return await GetDataFromLocalStorage(key)
    };


    /**
     * Get data from browser storage.
     * @param key Read data from specific key
     * @param UseBrowserCache Boolean value for reading data from browser storage, Eg. true : read data from browser storage, false: dont read key
     * @returns Return Promise<T|any> type.
     */
    const GetDataFromLocalStorage = async function <T>(key: string, UseBrowserCache: boolean = true, callWithoutLog: boolean = false): Promise<T | any> {
        try {
            if (UseBrowserCache === false) {
                return null;
            }
            const bwStorageResponse = await AdminLayout.getItem<Promise<T>>(key);
            if (bwStorageResponse) {
                const data = bwStorageResponse as any;
                const now = new Date();
                if (now.getTime() > data.Expiry) {
                    AdminLayout.removeItem(key);
                    return null;
                }
                if (!callWithoutLog)
                    _utils.GenericFunctionality.HtmlLogs(`GetDataFromLocalStorage - Data fetched from browser! Key`, key, `${JSON.stringify(data)}`);
                return Promise.resolve(<T>data);
            }
            else {
                return null;
            }

        } catch (err) {
            // This code runs if there were any errors.
            return <T>Promise.reject(_type.PromiseState.Error);
        }

    };

    const SetDefaultData = async function () {
        _utils.GenericFunctionality.HtmlLogs(`SetDefaultData`, `Setting default Application Configuration Data on browser cache!.`, DefaultValue);
        SetDataToLocalStorage(DefaultValue, "Application Configuration", 'Config', 10);
        // let configData = GetConfigData('Config')! as _type.AppConfigType;
        // backgroundCache = configData.UseRedisCache;

    };

    const ClearCache = async function () {
        AdminLayout.clear().then(() => {
            SetDataToLocalStorage(DefaultValue, "Application Configuration", 'Config', 10);
        }).catch(function (err) {
            console.log(err);
        });
    };
    const ClearModuleKeys = async function (keys: string[]) {
        _utils.GenericFunctionality.HtmlLogs(`ClearModuleKeys`, `Clearing browser cache!.`, '');
        keys.forEach((key) => {
            localStorage.removeItem(key);
        })
    };
    /**
     * Store Token object to browser storage.
     * @param token Token object type.
     * @returns void.
     */
    const SetToken = async function (token: _type.StorageData<_type.TokenObject>): Promise<void> {
        //const myCipher = _security.cipher('codingcompany.in')
        //clear cache first, and save token
        //ClearCache();
        //encripting token object
        // let userToken: _type.TokenObject = {
        //     TK: myCipher(token.TK),
        //     SN: myCipher(token.SN),
        //     Id: myCipher(token.Id),
        // } 
        SetDataToLocalStorage(token.Data, 'Token', 'Token', 60);
        SetDefaultData();

    };
    const GetToken = async function (key: string) {
        return await GetDataFromLocalStorage(key);
        // let userToken: _type.TokenObject = {
        //     TK: myCipher(token.TK),
        //     SN: myCipher(token.SN),
        //     Id: myCipher(token.Id),
        // } 

    };

    return {
        SetDataToLocalStorage: SetDataToLocalStorage,
        GetConfigData: GetConfigData,
        GetDataFromLocalStorage: GetDataFromLocalStorage,
        SetDefaultData: SetDefaultData,
        ClearCache: ClearCache,
        ClearModuleKeys: ClearModuleKeys,
        SetToken: SetToken,
        GetToken: GetToken
    }
}();