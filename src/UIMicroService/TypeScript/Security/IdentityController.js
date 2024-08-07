"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.IdentityController = void 0;
exports.IdentityController = function () {
    //it will read token config from layout page.
    var ReadTokenFromLayoutPage = function (element) {
        if (element.hasAttribute('SchoolName') && element.hasAttribute('Token') && element.hasAttribute('UserId')) {
            var jsTokenObject = {
                Data: {
                    SN: element.getAttribute('SchoolName'),
                    TK: element.getAttribute('Token'),
                    Id: element.getAttribute('UserId')
                },
                Duration: 60,
                Id: "Token",
                Key: "Token"
            };
            return jsTokenObject;
        }
        return undefined;
    };
    return {
        ReadToken: ReadTokenFromLayoutPage
    };
}();
