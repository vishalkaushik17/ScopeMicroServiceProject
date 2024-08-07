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
let defaultValue = {
    AppVersion: '1.0',
    AppName: 'Scope ERP Software',
    CompanyName: 'Coding Company',
    BuildVersion: '1.0',
    UseRedisCache: true
}

//default document load for all modules
$(document).ready(async function () {
    //await clearCache();
    //Set default config data about application
    await SetDataToLocalStorage('Config', "Application Configuration", defaultValue);
    //retrieve cache value for making api calls
    let configData = await GetConfigData('Config');
    backroundCache = configData.UseRedisCache;
});

//Set session key
let SetDataToLocalStorage = async function setWithExpiry(key, id, value, mins) {
    const now = new Date();

    // `item` is an object which contains the original value
    // as well as the time when it's supposed to expire

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

};

//Get session key for config
let GetConfigData = async function getValue(key) {
    return await AdminLayout.getItem(key).then(function (data) {
        // This code runs once the value has been loaded
        // from the off-line store.
       
        if (data) {

            // `item` is an object which contains the original value
            // as well as the time when it's supposed to expire
            const now = new Date();
            if (data.expiry !== 'NaN' && now.getTime() > data.expiry) {

                // If the item is expired, delete the item from storage
                // and return null
                localStorage.removeItem(key);
                return null;
            }
            return Promise.resolve(data.data);


        } else {
            
            return null;
        }
    }).catch(function (err) {
        // This code runs if there were any errors
        console.log(err);
    });
}

//get session key
let GetDataFromLocalStorage = async function getValue(key, id, useCache) {

    
    //return null when cache is false, and when it is false then direct call to api
    if (useCache === false) {
        return null;
    }
    return await AdminLayout.getItem(key).then(function (returnResult) {
        // This code runs once the value has been loaded
        // from the off-line store.
        
        if (returnResult) {
            // `item` is an object which contains the original value
            // as well as the time when it's supposed to expire
            const now = new Date();
            if ((returnResult.expiry !== 'NaN' && now.getTime() > returnResult.expiry) || returnResult.id !== id) {
                // If the item is expired, delete the item from storage
                // and return null
                localStorage.removeItem(key);
                return null;
            }
            return Promise.resolve(returnResult.data);
        } else {
        
            return null;
        }
    }).catch(function (err) {
        // This code runs if there were any errors
        console.log(err);
    });
}

async function callback() {
    
    let configData = await GetConfigData('Config');
    backroundCache = configData.UseRedisCache;
}


//data populate on index page start here
//==========================================

async function clearCache() {    
    await AdminLayout.clear().then(function () {
        // Run this code once the database has been entirely deleted.
         SetDataToLocalStorage('Config', "Application Configuration", defaultValue);
    }).catch(function (err) {
        // This code runs if there were any errors
        console.log(err);
    });

}