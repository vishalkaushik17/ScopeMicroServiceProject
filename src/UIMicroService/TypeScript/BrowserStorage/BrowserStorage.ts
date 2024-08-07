import * as _localforage from "./tsStorage";
import * as _type from "../CustomType/customTypes";

let BackgroundCache: boolean;


export class ScopeStorage {
    private static instance: ScopeStorage;
    private constructor() {
        ScopeStorage.SetInitConfig();
    }

    public static GetInstance(){
        if (this.instance) {
            console.warn('storage instance already created!!!');
            return this.instance;
        }
        this.instance = new ScopeStorage();
        return this.instance;
    }

    private static async SetInitConfig() {
        (await _localforage.CacheStorage).SetDefaultData();
    }
    //Storing token inside local forage
    public async SetToken(token: _type.TokenObject) {
        //creating tokenData
        let tokenData: _type.StorageData<_type.TokenObject> = {
            Data: token,
            Id: "IO",
            Expiry: 40,
            //  Key: "IO"
        }
        ;(await _localforage.CacheStorage).SetToken(tokenData);
    }
    //Get Token
    public async GetToken(key:string) {
        //creating tokenData
        return await this.DataOperation.Read(key);
    }

    public DataOperation = {
        Write: async (data: _type.StorageData<object>) => {
            (await _localforage.CacheStorage).SetDataToLocalStorage(data.Data, data.Id,data.Id, data.Expiry)
        },
        Read: async (key: string) => {
            return (await _localforage.CacheStorage).GetDataFromLocalStorage(key)
        },
        Clean: async () =>  {
             (await _localforage.CacheStorage).ClearCache();
        }
    }
}
//export class ScopeStorage {
//    private static instance: ScopeStorage;
//    private constructor() {

//    }

//    GetInstance(): ScopeStorage {
//        if (ScopeStorage.instance) {
//            return ScopeStorage.instance;
//        }
//        ScopeStorage.instance = new ScopeStorage();
//        return ScopeStorage.instance;

//    }
//    //Storing token inside local forage
//    static async SetToken(token: _type.TokenObject) {
//        //creating tokenData
//        let tokenData: _type.StorageData = {
//            Data: token,
//            Id: "Token",
//            Duration: 40,
//            Key: "Token"
//        }
//        _localforage.BrowserStorage.SetToken(tokenData);
//    }
//    static LocalStorage = {
//        Write: async (data: _type.StorageData) => {
//            _localforage.BrowserStorage.SetDataToLocalStorage(data.Data, data.Id, data.Key, data.Duration)
//        },
//        Read: async (key: string) => {
//            return _localforage.BrowserStorage.GetConfigData(key)
//        },
//        Clean: async () => {
//            return _localforage.BrowserStorage.ClearCache();
//        }


//    }
//}





////class with getter/setter and constructor
//class testClass {
//    private firstName: string;
//    constructor(name: string) {
//        this.firstName = name;
//        /*super()*/ /*use super to call base constroctor class*/
//    }
//    get Name() {
//        if (this.firstName) {
//            return this.firstName;
//        }
//        throw new Error('name not defined');
//    }
//    set Name(value: string) {
//        if (!value) {
//            throw new Error('Value not defined!');
//        }
//        this.firstName = value;
//        return;
//    }
//}

//const temp = new testClass('vishal');

//temp.Name; //getter
//temp.Name = "abcd"; //setter




