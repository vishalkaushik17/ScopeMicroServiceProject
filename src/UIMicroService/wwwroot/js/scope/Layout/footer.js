

let toggleModel = function (element) {
    
    let isHidden = $(element).is(":hidden");
    if (isHidden) {
        $('#logModelbox').attr('style', 'display:block !Important');
    }
    else {
        $('#logModelbox').attr('style', 'display:none !Important');
    }
    $('#logModelbox').prop('hidden', !isHidden);
    $('#logmodal-default').modal('toggle');
    $('#logModelbox').modal('toggle');
}
let HideLogModel = function () {
    $('#logModelbox').prop('hidden',true);
    $('#logmodal-default').modal('toggle');
}
