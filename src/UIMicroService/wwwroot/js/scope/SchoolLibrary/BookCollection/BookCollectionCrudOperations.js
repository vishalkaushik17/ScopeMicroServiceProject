//public varibales for the form specific

cacheKey = 'BookCollectionModule';
sessionId = `BookCollections_${scopeTokenData.id}`;
apiModule = "BookCollection";
apiUrlToGetAll = `https://localhost/ApiGateway/${apiModule}/getall?useCache=false`;
apiUrlToGetById = `https://localhost/ApiGateway/${apiModule}/get??useCache=false&id=`;
jsFormObject = {};

//===end pubic vars=====================================================

//Document ready execution
$(document).ready(async function () {
    
    $("#LayoutUl li").removeClass("active");
    $('#LibraryLI').addClass('active treeview');
    $('#BookCollection-LibraryModule a').css({
        'color': 'darkblue',
        'font-weight': 'bold'
    });
    jsFormObject.apiUrlToGetAll = apiUrlToGetAll;
    jsFormObject.cacheKey = cacheKey;
    jsFormObject.sessionId = sessionId;
    jsFormObject.FillIndex = fillBookCollectionIndex;
    jsFormObject.useCache = true;


    //Initial populate the table
    PopulateItemsTable();
});

//Open for edit record
$(document).on("click", ".open-BookCollectionModel", async function () {
    let recordId = $(this).data('id');
    //alert(myBookId);
    showLoader();
    //Load data for edit

    changeButtonCRUDState();
    let obj = {};
    obj.apiUrlToGetById = apiUrlToGetById;
    obj.id = recordId;
    loadBookCollectionData(obj);

});
//Load a record for editing
let loadBookCollectionData = async function loadBookCollectionData(obj) {
    $.ajax({
        type: 'GET',
        datatype: "json",
        url: `${obj.apiUrlToGetById}${obj.id}`,
        headers: {
            "School": jsTokenObject.schoolName,
            "Authorization": jsTokenObject.token,
            'Content-Type': 'application/json; charset=utf-8'
        },
        success: function (data) {
            //check response status

            if (data.status === 'Success') {
                hideLoader();
                $('#modal-default').modal('show');
                //it will populate records on model for editing
                FillBookCollectionModelFormForEditing(data.Result);
            }

            //hide new record button
            //show update record button
            if (data.Result.isEditable === false) {
                $("#saveLi").css("display", "none");
                $("#cancelLi").css("display", "block");
            }
        },
        error: function (req, status, error) {
            console.log(req);
            console.log(status);
            console.log(error);
            ////display error message
            toastr.warning("Opps! something went wrong!", "Error");
        },
        complete: function () {
            hideLoader();
        }
    });
};


//pushing data to model fields for editing
let FillBookCollectionModelFormForEditing = async function fillModelForEditing(recordData) {

    $("#Name").val(recordData.name);
    $("#Abbreviation").val(recordData.abbreviation);
    $("#Id").val(recordData.id);

}


//Adding or updating record to cache
let addRowToBookCollectionIndex = async function addRowToBookCollectionIndex(data, newValue, id) {

    let isFound = false;
    $.each(data.Result, function (key, val) {
        if (val.id === id) {
            //update condition
            val.name = newValue.name;
            val.abbreviation = newValue.abbreviation;
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
let fillBookCollectionIndex = function fillBookCollectionIndex(data) {
    let hrefid = "#";
    let hrefdel = "#";
    let delIcon = "#";
    $.each(data.Result, function (key, val) {
        let body = "<tr>";
        hrefid = "'#modal-default' class='open-BookCollectionModel'";
        hrefdel = "class='delete-BookCollectionRecord'";
        delIcon = "<i class='ion ion-trash-b btn btn-warning' style='font-size:20px;'></i>";
        body += `<td> <a href=${hrefid} data-id=${val.id} </a>${val.name}</td>`;
        body += `<td> ${val.abbreviation}</td>`;
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



//common submit form for all the modules
//it will get called on submit click - while click on side panel save button
$('#modelFormBookCollection').submit(async function (e) {
    showLoader();
    e.preventDefault();

    let id = getFormElementsData($(this)).Id;
    let formData = JSON.stringify(getFormElementsData($(this)));
    let urlPath = `https://localhost/ApiGateway/${apiModule}/save?useCache=${backroundCache}`;
    if (id != '') {
        urlPath = `https://localhost/ApiGateway/${apiModule}/update?id=${id}&useCache=${backroundCache}`;
    }

    //passing object for saving or updating record to db
    let objecToProcess = {};
    objecToProcess.urlPathToSave = urlPath;
    objecToProcess.formData = formData;
    objecToProcess.cacheKey = cacheKey;
    objecToProcess.id = sessionId;
    objecToProcess.apiUrlToGetAll = `https://localhost/ApiGateway/${apiModule}/getall?useCache=${backroundCache}`;
    objecToProcess.fnTableFill = fillBookCollectionIndex;
    await submitBookCollectionModelForm(objecToProcess);
});
let submitBookCollectionModelForm = async function submitBookCollectionModelForm(obj) {
    $.ajax({
        type: 'POST',
        url: obj.urlPathToSave,
        data: obj.formData,
        headers: {
            "School": jsTokenObject.schoolName,
            "Authorization": jsTokenObject.token,
            'Content-Type': 'application/json; charset=utf-8'
        }, beforeSend: function () {
            showLoader();
        },
        success: function (data) {
            //Display message
            displayAlert(data);
            //check response status
            if (data.status === 'Success') {
                let val = data.Result;
                //Close model
                $('#modal-default').modal('toggle');
                $('#indexDataTable').DataTable().destroy();
                $('#indexDataTable tbody').empty();

                //Reding previously stored value and then update it if update process 
                //start otherwise add new record
                GetDataFromLocalStorage(obj.cacheKey, obj.id,obj.useCache)
                    .then(function (result) {

                        if (result) {

                            addRowToBookCollectionIndex(result, val, data.Result.id);

                            obj.fnTableFill(result);

                        } else {
                            printLog("No cache data", "local cache");

                        }
                    })
                    .catch(e => {
                        console.log(e.message);
                    }); // Received a function as a value

                //Reset Form
                $('#modelForm').each(function () {
                    this.reset();
                });
                $('#Id').val("");
                $('#Name').val("");
                $('#Abbreviation').val("");
                resetButtonState();
            }

            //hide loader on success or fail status
            hideLoader();

        },
        error: function (req, status, error) {
            console.log(req);
            console.log(status);
            console.log(error);
            ////display error message
            toastr.warning("Opps! Something went wrong!", "Error");
        },
        complete: function () {
            hideLoader();
        }
    });
};


//Delete record
$(document).on("click",
    ".delete-BookCollectionRecord",
    async function () {
        let recordId = $(this).data('id');

        let trSequence = $(this);
        swal({
            title: "Are you sure?",
            text: "Once deleted, you will not be able to recover this record!",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    showLoader();
                    deleteBookCollectionData(trSequence, `https://localhost/ApiGateway/${apiModule}/delete?id=${recordId}&useCache=${backroundCache}`);

                    //To delete value from cache, we are making it true;



                }
            });
    });


//Common crud operations
//Delete Data
let deleteBookCollectionData = async function deleteBookCollectionData(trSequence, apiUrl) {
    $.ajax({
        type: 'GET',
        datatype: "json",
        url: apiUrl,
        headers: {
            'School': jsTokenObject.schoolName,
            'Authorization': jsTokenObject.token,
            'Content-Type': 'application/json; charset=utf-8'
        },
        success: function (ajaxData) {
            
            //check response status
            hideLoader();
            if (ajaxData.status === "Success") {
                //Finding data from browser cache
                GetDataFromLocalStorage(`${cacheKey}`, sessionId, jsFormObject.useCache)
                    .then(function (cacheData) {
                        
                        if (cacheData) {
                            cacheData.Result = cacheData.Result.filter(e => e.id !== ajaxData.Result.id);

                            SetDataToLocalStorage(`${cacheKey}`, sessionId, cacheData, 20);
                            if (cacheData.Result.length > 0) {
                                fillBookCollectionIndex(cacheData);
                                $('.searchLabel').focus();
                                return;
                            }
                        } 
                    });

                swal("Ohh! Your record has been deleted!", {
                    icon: "success",
                    timer: 3000
                });

                let table = $('#indexDataTable').DataTable();
                table
                    .row(trSequence.parents('tr'))
                    .remove()
                    .draw();

            }
            else {
                swal("Ohh! " + ajaxData.message, {
                    icon: "error"
                });
            }
        },
        error: function (req, status, error) {
            console.log(req);
            console.log(status);
            console.log(error);
            ////display error message
            toastr.warning(error, "Error!");
            hideLoader();
        },
        complete: function () {
            hideLoader();
        }

    });
};
