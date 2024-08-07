



export var myapp = function () {


    var printme = function (message) {
        console.log(message);
    };
    return {
        printme: printme
    
    }
}();