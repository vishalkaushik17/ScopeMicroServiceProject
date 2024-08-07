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
exports.ScopeStorage = void 0;
var _localforage = require("./tsStorage");
var BackgroundCache;
var ScopeStorage = /** @class */ (function () {
    function ScopeStorage() {
        var _this = this;
        this.DataOperation = {
            Write: function (data) { return __awaiter(_this, void 0, void 0, function () {
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0: return [4 /*yield*/, _localforage.CacheStorage];
                        case 1:
                            (_a.sent()).SetDataToLocalStorage(data.Data, data.Id, data.Key, data.Duration);
                            return [2 /*return*/];
                    }
                });
            }); },
            Read: function (key) { return __awaiter(_this, void 0, void 0, function () {
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0: return [4 /*yield*/, _localforage.CacheStorage];
                        case 1: return [2 /*return*/, (_a.sent()).GetDataFromLocalStorage(key)];
                    }
                });
            }); },
            Clean: function () { return __awaiter(_this, void 0, void 0, function () {
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0: return [4 /*yield*/, _localforage.CacheStorage];
                        case 1:
                            (_a.sent()).ClearCache();
                            return [2 /*return*/];
                    }
                });
            }); }
        };
        ScopeStorage.SetInitConfig();
    }
    ScopeStorage.GetInstance = function () {
        if (this.instance) {
            console.warn('storage instance already created!!!');
            return this.instance;
        }
        this.instance = new ScopeStorage();
        return this.instance;
    };
    ScopeStorage.SetInitConfig = function () {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, _localforage.CacheStorage];
                    case 1:
                        (_a.sent()).SetDefaultData();
                        return [2 /*return*/];
                }
            });
        });
    };
    //Storing token inside local forage
    ScopeStorage.prototype.SetToken = function (token) {
        return __awaiter(this, void 0, void 0, function () {
            var tokenData;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        tokenData = {
                            Data: token,
                            Id: "IO",
                            Duration: 40,
                            Key: "IO"
                        };
                        return [4 /*yield*/, _localforage.CacheStorage];
                    case 1:
                        (_a.sent()).SetToken(tokenData);
                        return [2 /*return*/];
                }
            });
        });
    };
    //Get Token
    ScopeStorage.prototype.GetToken = function (key) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.DataOperation.Read(key)];
                    case 1: 
                    //creating tokenData
                    return [2 /*return*/, _a.sent()];
                }
            });
        });
    };
    return ScopeStorage;
}());
exports.ScopeStorage = ScopeStorage;
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
