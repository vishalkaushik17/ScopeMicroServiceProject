//public varibales for the form specific

let cacheKey = 'MediaTypeModule';
let sessionId = `MediaTypes_${scopeTokenData.id}`;
let apiModule = "MediaType";
let apiUrlToGetAll = `https://localhost/ApiGateway/${apiModule}/getall`;
let apiUrlToGetById = `https://localhost/ApiGateway/${apiModule}/get?id=`;

let jsFormObject = {};
//===end pubic vars=====================================================

//Document ready execution
$(document).ready(async function () {
    $("#LayoutUl li").removeClass("active");
    $('#LibraryLI').addClass('active treeview');
    $('#MediaType-LibraryModule a').css({
        'color': 'darkblue',
        'font-weight': 'bold'
    });
    jsFormObject.apiUrlToGetAll = apiUrlToGetAll;
    jsFormObject.cacheKey = cacheKey;
    jsFormObject.sessionId = sessionId;
    jsFormObject.FillIndex = FillIndex;
    jsFormObject.useCache = true;

    //Initial populate the table
    PopulateItemsTable();
});


//pushing data to model fields for editing
let FillModelFormForEditingRecord = async function fillModelForEditing(recordData) {

    $("#Name").val(recordData.name);
    $("#Id").val(recordData.id);

}

//table specific function for populate index page
let FillIndex = async function FillIndex(data) {
    let hrefid = "#";
    let hrefdel = "#";
    let delIcon = "#";
    $.each(data.Result, function (key, val) {
        let body = "<tr>";
        hrefid = "'#modal-default' class='open-Model'";
        hrefdel = "class='delete-Record'";
        delIcon = "<i class='ion ion-trash-b btn btn-warning' style='font-size:20px;'></i>";
        body += `<td> <a href=${hrefid} data-id=${val.id}  </a>${val.name}</td>`;
        
        if (val.isEditable === true) {
            body += `<td> <a href='#' ${hrefdel} apiName='MediaType' data-id=${val.id} &nbsp&nbsp</a> ${delIcon} </td>`;
        } else {
            body += "<td> <a href='#'></a></td>";
        }
        body += "</tr>";

        $("#indexDataTable tbody").append(body);
    });
    $("#indexDataTable").DataTable().draw(); // always redraw
    $('#indexDataTable_filter > label > input').addClass("searchRecords");
    $('#indexDataTable_filter > label').addClass("searchLabel");
    $('#indexDataTable_length > label').addClass("searchLabel");
}

//Adding or updating record to cache
let addRowToIndex = async function AddRowToIndex(data, newValue, id) {

    let isFound = false;
    $.each(data.Result, function (key, val) {
        if (val.id === id) {
            //update condition
            val.name = newValue.name;
            val.isEditable = true;
            isFound = true;
        }
    });
    if (!isFound) {
        //new record condition
        newValue.isEditable = true;
        data.Result.push(newValue);
    }

    //store newly updated data inside cache
    return await SetDataToLocalStorage(cacheKey,sessionId, data, 20);
}

////public varibales for the form specific
//let jsFormObject = {};
//let cacheKey = "";
//let apiUrlToGetAll = 'https://localhost/ApiGateway/mediatype/getall';
//let apiUrlToGetById = 'https://localhost/ApiGateway/mediatype/get?id=';
////When index page get ready it will execute
//$(document).ready(function () {
//    $("#LayoutUl li").removeClass("active");
//    $('#LibraryLI').addClass('active treeview');
//    $('#MediaType-LibraryModule a').css({'color':'darkblue',
//        'font-weight': 'bold'
//    });
//    await PopulateItemsTable();
//});


////Populate index for page
//let PopulateItemsTable = async function populateItemsTable() {
//    cacheKey = `${scopeTokenData.id}_MediaTypes_Index`;
//    jsFormObject.apiUrlToGetAll = apiUrlToGetAll;
//    jsFormObject.cacheKey = cacheKey;
//    jsFormObject.FillIndex = FillIndex;

//    PopulateItemsOnIndex(jsFormObject);
//}

////Open for edit record
////$(document).on("click", ".open-Model",async function () {
////    let recordId = $(this).data('id');
////    //alert(myBookId);
////    showLoader();
////    //Load data for edit
////    changeButtonCRUDState();
////    let obj = {};
////    obj.apiUrlToGetById = apiUrlToGetById;
////    obj.FillModelFormForEditingRecord;
////    obj.id = recordId;
////    loadData(obj);
////    $('#modal-default').modal('show');
////});

////pushing data to model fields for editing
//let FillModelFormForEditingRecord = function FillModelForEditing(recordData) {
    
//    $("#Name").val(recordData.name);
//    $("#Id").val(recordData.id);

//}

////Delete record
//$(document).on("click", ".delete-Record", function () {
//    let recordId = $(this).data('id');
//    let trSequence = $(this);
//    swal({
//        title: "Are you sure?",
//        text: "Once deleted, you will not be able to recover this record!",
//        icon: "warning",
//        buttons: true,
//        dangerMode: true,
//    })
//        .then((willDelete) => {
//            if (willDelete) {
//                showLoader();
//                deleteData(trSequence, `https://localhost/ApiGateway/mediatype/delete?id=${recordId}`);

//                //To delete value from cache, we are making it true;
//                useCache = true;
//                let indexData = GetDataFromLocalStorage(cacheKey);

//                //removing record as per recordId from array
//                indexData.Result = indexData.value.Result.filter(e => e.id !== recordId);

//                //Again storing it to cache
//                SetDataToLocalStorage(cacheKey, indexData, 20);

//                hideLoader();
//            } 
//        });
//});


////it will get called on submit click - while click on side panel save button
//$('#modelForm').submit(function (e) {
//    showLoader();
//    e.preventDefault();
//    let id = getFormElementsData($(this)).Id;
//    let formData = JSON.stringify(getFormElementsData($(this)));
//    let urlPath = 'https://localhost/ApiGateway/mediatype/save';
//    if (id != '') {
//        urlPath = `https://localhost/ApiGateway/mediatype/update?id=${id}`;
//    }

//    //passing object for saving or updating record to db
//    let objecToProcess = {};
//    objecToProcess.urlPathToSave = urlPath;
//    objecToProcess.formData = formData;
//    objecToProcess.cacheKey = cacheKey;
//    objecToProcess.apiUrlToGetAll = 'https://localhost/ApiGateway/mediatype/getall';
//    objecToProcess.fnTableFill = FillIndex;
//    submitForm(objecToProcess);
//});


////table specific function for populate index page
//let FillIndex = function FillIndex(data) {
//    let hrefid = "#";
//    let hrefdel = "#";
//    let delIcon = "#";

//    $.each(data.Result, function (key, val) {
//        let body = "<tr>";
//        hrefid = "'#modal-default' class='open-Model'";
//        hrefdel = "class='delete-Record'";
//        delIcon = "<i class='ion ion-trash-b btn btn-warning' style='font-size:20px;'></i>";
//        body += `<td> <a href=${hrefid} data-id=${val.id} </a>${val.name}</td>`;
//        if (val.isEditable === true) {
//            body += `<td> <a href='#' ${hrefdel} data-id=${val.id} &nbsp&nbsp</a> ${delIcon} </td>`;
//        } else {
//            body += "<td> <a href='#'></a></td>";
//        }
//        body += "</tr>";

//        $("#indexDataTable tbody").append(body);
//    });
//    $("#indexDataTable").DataTable().draw(); // always redraw
//    $('#indexDataTable_filter > label > input').addClass("searchRecords");
//    $('#indexDataTable_filter > label').addClass("searchLabel");
//    $('#indexDataTable_length > label').addClass("searchLabel");


//}
////Adding or upading record to cache
//let addRowToIndex = function FillMediaTypeIndex(data,newValue,id) {
    
//     let isFound = false;
//    $.each(data.Result, function (key, val) {
//        if (val.id === id) {
//            val.name = newValue.name;
//            val.isEditable = true;
//            isFound = true;
//        }
//    });
//    if (!isFound) {
//        newValue.isEditable = true;
//        data.Result.push(newValue);
//    }
//    SetDataToLocalStorage(cacheKey, data, 20);

//}
