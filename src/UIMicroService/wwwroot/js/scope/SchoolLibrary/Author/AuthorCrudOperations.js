//public varibales for the form specific

cacheKey = 'AuthorModule';
sessionId = `Authors_${scopeTokenData.id}`;
apiModule = "Author";
apiUrlToGetAll = `https://localhost/ApiGateway/${apiModule}/getall?useCache=false`;
apiUrlToGetById = `https://localhost/ApiGateway/${apiModule}/get??useCache=false&id=`;
jsFormObject = {};

//===end pubic vars=====================================================

//Document ready execution
$(document).ready(async function () {
    
    $("#LayoutUl li").removeClass("active");
    $('#LibraryLI').addClass('active treeview');
    $('#Author-LibraryModule a').css({
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
    return await SetDataToLocalStorage(cacheKey, sessionId, data, 20);
}

//table specific function for populate index page
let FillIndex = function FillIndex(data) {
    let hrefid = "#";
    let hrefdel = "#";
    let delIcon = "#";
    $.each(data.Result, function (key, val) {
        let body = "<tr>";
        hrefid = "'#modal-default' class='open-Model'";
        hrefdel = "class='delete-Record'";
        delIcon = "<i class='ion ion-trash-b btn btn-warning' style='font-size:20px;'></i>";
        body += `<td> <a href=${hrefid} data-id=${val.id} </a>${val.name}</td>`;
        if (val.isEditable === true) {
            body += `<td> <a href='#' ${hrefdel} data-id=${val.id} &nbsp&nbsp</a> ${delIcon} </td>`;
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

