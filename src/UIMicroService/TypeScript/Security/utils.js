"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.InsertNodeDynamicly = exports.decipher = exports.cipher = void 0;
var _type = require("../CustomType/customTypes");
var cipher = function (salt) {
    var textToChars = function (text) { return text.split('').map(function (c) { return c.charCodeAt(0); }); };
    var byteHex = function (n) { return ("0" + Number(n).toString(16)).substr(-2); };
    var applySaltToChar = function (code) { return textToChars(salt).reduce(function (a, b) { return a ^ b; }, code); };
    return function (text) { return text.split('')
        .map(textToChars)
        .map(applySaltToChar)
        .map(byteHex)
        .join(''); };
};
exports.cipher = cipher;
var decipher = function (salt) {
    var textToChars = function (text) { return text.split('').map(function (c) { return c.charCodeAt(0); }); };
    var applySaltToChar = function (code) { return textToChars(salt).reduce(function (a, b) { return a ^ b; }, code); };
    return function (encoded) { return encoded.match(/.{1,2}/g)
        .map(function (hex) { return parseInt(hex, 16); })
        .map(applySaltToChar)
        .map(function (charCode) { return String.fromCharCode(charCode); })
        .join(''); };
};
exports.decipher = decipher;
exports.InsertNodeDynamicly = function () {
    var IsFileAlreadyRendered = function (moduleName, tag) {
        var moduleNodes = document.querySelectorAll(tag);
        var moduleNodesArray = Array.from(moduleNodes);
        var listOfAvailableTags = moduleNodesArray.filter(function (value, index) {
            if (tag == _type.Tag.css) {
                var cssTag = value;
                if (cssTag.href.toLowerCase().indexOf(moduleName.toLowerCase()) > -1) {
                    var newLinkTag = document.createElement(tag);
                    newLinkTag.href = moduleNodes[index].href;
                    document.head.appendChild(newLinkTag);
                    document.head.removeChild(newLinkTag);
                    return true;
                }
            }
            else if (tag == _type.Tag.script) {
                var jsTag = value;
                if (jsTag.src.toLowerCase().indexOf(moduleName.toLowerCase()) > -1) {
                    var newLinkTag = document.createElement(tag);
                    newLinkTag.src = moduleNodes[index].src;
                    document.body.appendChild(newLinkTag);
                    document.body.removeChild(newLinkTag);
                    return true;
                }
            }
            return false;
        });
        //if module node is available in DOM then return true
        if (listOfAvailableTags.length > 0) {
            return true;
        }
        return false;
    };
    var InsertNodeElement = function (url, elementTagType, location, version) {
        var scriptTag = document.createElement(elementTagType);
        scriptTag.src = "".concat(url, "?v=").concat(version);
        scriptTag.defer = true;
        scriptTag.type = "module";
        //scriptTag.onload = implementationCode;
        // scriptTag.onreadystatechange = implementationCode;
        location.appendChild(scriptTag);
        return true;
    };
    var loadJS = function (url, location, partialViewName, version) {
        var isNodeAlreadyExists = IsFileAlreadyRendered(partialViewName, _type.Tag.script);
        if (!isNodeAlreadyExists) {
            InsertNodeElement(url, _type.Tag.script, location, version);
        }
    };
    var loadCss = function (url, location, partialViewName, version) {
        var isNodeAlreadyExists = IsFileAlreadyRendered(partialViewName, _type.Tag.css);
        if (!isNodeAlreadyExists) {
            InsertNodeElement(url, _type.Tag.css, location, version);
        }
    };
    // const InitDynamicFileFunction = function(functionCallBack)
    // {
    //   let fun= window[functionCallBack];
    //   if(typeof fun !== 'function')
    //   {
    //     return;
    //   }
    //   (fun as any).apply = window;
    //   (fun as any).Init();
    //   console.log("school library executed!");
    // }
    return {
        loadJS: loadJS,
        loadCss: loadCss,
        // InitDynamicFileFunction:InitDynamicFileFunction
    };
}();
// export const cipher = function(salt:string):any {
//     const textToChars = text => text.split('').map(c => c.charCodeAt(0));
//     const byteHex = n => ("0" + Number(n).toString(16)).substr(-2);
//     const applySaltToChar = code => textToChars(salt).reduce((a, b) => a ^ b, code);
//     return function (text) {
//         text.split('')
//         .map(textToChars)
//         .map(applySaltToChar)
//         .map(byteHex)
//         .join('')
//     };
// }
// export const decipher = function (salt: string): any {
//     const textToChars = text => text.split('').map(c => c.charCodeAt(0));
//     const applySaltToChar = code => textToChars(salt).reduce((a, b) => a ^ b, code);
//     return encoded => encoded.match(/.{1,2}/g)
//         .map(hex => parseInt(hex, 16))
//         .map(applySaltToChar)
//         .map(charCode => String.fromCharCode(charCode))
//         .join('');
// }
