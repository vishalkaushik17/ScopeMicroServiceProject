"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.CacheStorage = void 0;
var localforage = require("localforage");
// Using config()
localforage.config({
    driver: [
        localforage.INDEXEDDB,
        localforage.LOCALSTORAGE,
        localforage.WEBSQL
    ],
    name: 'ScopeERP',
    version: 1.0,
    size: 4980736,
    storeName: 'ScopeERPData',
    description: 'storing data at client browser for faster operations.'
});
//instance of localforage
var AdminLayout = localforage.createInstance({
    name: "ScopeERP",
    storeName: "ScopeERPData"
});
//storing default value in browser cache
var DefaultValue = {
    AppVersion: '1.0',
    AppName: 'Scope ERP Software',
    CompanyName: 'Coding Company',
    BuildVersion: '1.0',
    UseRedisCache: true
};
exports.CacheStorage = function () {
    return __awaiter(this, void 0, void 0, function () {
        var SetDataToLocalStorage, GetConfigData, GetDataFromLocalStorage, SetDefaultData, ClearCache, SetToken, GetToken;
        var _a;
        return __generator(this, function (_b) {
            switch (_b.label) {
                case 0:
                    SetDataToLocalStorage = function (value, id, key, mins) {
                        return __awaiter(this, void 0, void 0, function () {
                            var now, item;
                            return __generator(this, function (_a) {
                                now = new Date();
                                item = {
                                    Data: value,
                                    Id: id,
                                    Expiry: now.getTime() + (mins * 60000)
                                };
                                return [2 /*return*/, AdminLayout.setItem(key, item)
                                        .then(function (result) {
                                        if (result) {
                                            return result;
                                        }
                                        else {
                                            console.log("No cache data", "setItem");
                                            return null;
                                        }
                                    })
                                        .catch(function (e) {
                                        console.log(e.message);
                                    })]; // Received a function as a value
                            });
                        });
                    };
                    GetConfigData = function (key) {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, GetDataFromLocalStorage(key)];
                                    case 1: return [2 /*return*/, _a.sent()];
                                }
                            });
                        });
                    };
                    GetDataFromLocalStorage = function (key, UseBrowserCache) {
                        if (UseBrowserCache === void 0) { UseBrowserCache = true; }
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        if (UseBrowserCache === false) {
                                            return [2 /*return*/, null];
                                        }
                                        return [4 /*yield*/, AdminLayout.getItem(key).then(function (dataFromCache) {
                                                if (dataFromCache) {
                                                    var data = dataFromCache;
                                                    var now = new Date();
                                                    if (now.getTime() > data.Duration) {
                                                        localStorage.removeItem(key);
                                                        return undefined;
                                                    }
                                                    return data;
                                                }
                                                return undefined;
                                            }).then(function (result) {
                                                if (result) {
                                                    return result;
                                                }
                                                return undefined;
                                            })
                                                .catch(function (err) {
                                                console.log(err);
                                            })];
                                    case 1: return [2 /*return*/, _a.sent()];
                                }
                            });
                        });
                    };
                    SetDefaultData = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                SetDataToLocalStorage(DefaultValue, "Application Configuration", 'Config', 10);
                                return [2 /*return*/];
                            });
                        });
                    };
                    ClearCache = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                AdminLayout.clear().then(function () {
                                    SetDataToLocalStorage(DefaultValue, "Application Configuration", 'Config', 10);
                                }).catch(function (err) {
                                    console.log(err);
                                });
                                return [2 /*return*/];
                            });
                        });
                    };
                    SetToken = function (token) {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
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
                                return [2 /*return*/];
                            });
                        });
                    };
                    GetToken = function (key) {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, GetDataFromLocalStorage(key)];
                                    case 1: return [2 /*return*/, _a.sent()];
                                }
                            });
                        });
                    };
                    _a = {
                        SetDataToLocalStorage: SetDataToLocalStorage,
                        GetConfigData: GetConfigData
                    };
                    return [4 /*yield*/, GetDataFromLocalStorage];
                case 1: return [2 /*return*/, (_a.GetDataFromLocalStorage = _b.sent(),
                        _a.SetDefaultData = SetDefaultData,
                        _a.ClearCache = ClearCache,
                        _a.SetToken = SetToken,
                        _a.GetToken = GetToken,
                        _a)];
            }
        });
    });
}();
