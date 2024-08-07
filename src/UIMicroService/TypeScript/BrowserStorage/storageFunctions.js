let backgroundCache = true;
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

/**Setting up instance of the local storage. */
let AdminLayout = localforage.createInstance({
    name: "ScopeERP",
    storeName: "ScopeERPData"
});


/**Storing default value for application. */
let DefaultValue = {
    AppVersion: '1.0',
    AppName: 'Scope ERP Software',
    CompanyName: 'Coding Company',
    BuildVersion: '1.0',
    UseRedisCache: true
}
/** Browser storage operations. */
export const BrowserStorage = {
    /** Store data in browser cache. */
    SetDataToLocalStorage: async function (value, id, key, mins) {
        const now = new Date();
        const item = {
            data: value,
            id: id,
            expiry: now.getTime() + (mins * 60000)
        };
        return await AdminLayout.setItem(key, item)
            .then(function (result) {

                if (result) {
                    return result;
                } else {
                    printLog("No cache data", "setItem");
                    return null;
                }
            })
            .catch(e => {
                console.log(e.message);
            }); // Received a function as a value

    },
    /** Get config data from local storage. */
    GetConfigData: async function (key) {
        return await AdminLayout.getItem(key).then(function (data) {
            if (data) {
                const now = new Date();
                if (data.expiry !== 'NaN' && now.getTime() > data.expiry) {
                    localStorage.removeItem(key);
                    return null;
                }
                return data;

            } else {
                return null;
            }
        }).catch(function (err) {
            console.log(err);
        });
    },
    /** Get data from local storage. */
    GetDataFromLocalStorage: async function (key, id, UseBrowserCache=true) {
        if (UseBrowserCache === false) {
            return null;
        }
        return AdminLayout.getItem(key).then(function (data) {
            console.warn(key + ' : '+ data);
            if (data) {
                const now = new Date();
                if ((data.expiry !== 'NaN' && now.getTime() > data.expiry) || data.id !== id) {
                    localStorage.removeItem(key);
                    return null;
                }
                return data;
            } else {

                return null;
            }
        }).catch(function (err) {
            console.log(err);
        });
    },
    /** Storing Application configuration related data. */
    SetDefaultData: async function () {
        await this.SetDataToLocalStorage(DefaultValue, "Application Configuration", 'Config', 10);
        let configData = await this.GetConfigData('Config');
        backgroundCache = configData.UseRedisCache;
    },
    /** Clear cache data. */
    ClearCache: async function () {
        await AdminLayout.clear().then(() => {

            this.SetDataToLocalStorage(DefaultValue, "Application Configuration", 'Config', 10);
        }).catch(function (err) {
            console.log(err);
        });
    },
    SetToken: async function (token) {
        await this.SetDataToLocalStorage(token.Data, token.Id, token.Key, token.Duration);
    },
    GetToken: async function (key,id,UseBrowserCache=true) {
        return await this.GetConfigData(key);
    },
}
