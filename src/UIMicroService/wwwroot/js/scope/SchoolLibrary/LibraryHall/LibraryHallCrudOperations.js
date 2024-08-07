//Document ready execution
$(document).ready(async function () {
    await clearCache();
    jsFormObject = {};
    $("#LayoutUl li").removeClass("active");
    $('#LibraryLI').addClass('active treeview');
    $('#SchoolLibrary-LibraryModule a').css({
        'color': 'darkblue',
        'font-weight': 'bold'
    }); 
    jsFormObject.cacheKey = 'LibraryModule';
    jsFormObject.apiModule = 'Library';
    jsFormObject.sessionId = `Libraries_${scopeTokenData.id}`;
    jsFormObject.FillIndex = FillIndex;
    jsFormObject.useCache = true;
    apiUrlToGetAll = `https://localhost/ApiGateway/${jsFormObject.apiModule}/getall?useCache=false`;
    apiUrlToGetById = `https://localhost/ApiGateway/${jsFormObject.apiModule}/get??useCache=false&id=`;
    jsFormObject.apiUrlToGetAll = apiUrlToGetAll;
    jsFormObject.apiUrlToGetById = apiUrlToGetById;
    //Initial populate the table
    PopulateItemsTable();
});


//pushing data to model fields for editing
async function FillModelFormForEditingRecord(recordData) {

    $("#Name").val(recordData.name);
    $("#Id").val(recordData.id);
}

//table specific function for populate index page
async function FillIndex(data) {
    let hrefid = "#";
    let hrefdel = "#";
    let delIcon = "#";
    $.each(data.Result, function (key, val) {
        let body = "<tr>";
        hrefid = "'#modal-default' class='open-Model'";
        hrefdel = "class='delete-Record'";
        delIcon = "<i class='ion ion-trash-b btn btn-warning' style='font-size:20px;'></i>";
        body += `<td> <a href=${hrefid} data-id=${val.id}  </a>${val.name}</td>`;
        body += `<td> ${val.location}</td>`;
        if (val.isEditable === true) {
            body += `<td> <a href='#' ${hrefdel} apiName='Library' data-id=${val.id} &nbsp&nbsp</a> ${delIcon} </td>`;
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

//Adding or updating record to datatable and cache
async function addRowToIndex(data, newValue, id) {

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

    //store updated data inside cache
    return await SetDataToLocalStorage(jsFormObject.cacheKey, jsFormObject.sessionId, data, 20);
}
