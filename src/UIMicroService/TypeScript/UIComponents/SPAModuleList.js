"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SPAModules = void 0;
var _utils = require("../Security/utils");
var _type = require("../CustomType/customTypes");
var SPAModules = function (AssetsStaticPath, ModuleName, isCssModuleAvailable, isJsModuleAvailable, jsVersion, cssVersion) {
    switch (ModuleName) {
        case _type.ModuleName.iifeIndexPageFunction:
            if (isJsModuleAvailable)
                _utils.InsertNodeDynamicly.loadJS("".concat(AssetsStaticPath, "/scripts/").concat(ModuleName.toLowerCase(), ".min.js"), document.body, ModuleName, jsVersion);
            if (isCssModuleAvailable)
                _utils.InsertNodeDynamicly.loadCss("".concat(AssetsStaticPath, "/styles/").concat(ModuleName.toLowerCase(), ".css"), document.head, ModuleName, cssVersion);
            break;
        case _type.ModuleName.iifeIndexPageFunction:
            break;
        default:
            break;
    }
};
exports.SPAModules = SPAModules;
