//public variables for the form specific

let cacheKeyLibrary = 'LibraryModule';
let cacheKeyRoom = 'RoomModule';
let sessionId = `Rooms_${scopeTokenData.id}`;
let apiModuleLibrary = "Library";
let apiModuleRoom = "Room";
let libId = '';
let roomId = '';
let apiUrlToGetAllLibrary = `https://localhost/ApiGateway/${apiModuleLibrary}/getall`;
let apiUrlToGetAllRooms = `https://localhost/ApiGateway/${apiModuleRoom}/getall?libraryId=${libId}`;
let apiUrlToRoomGetById = `https://localhost/ApiGateway/${apiModuleRoom}/get?libraryId=${libId}&roomId=${roomId}`;

//===end pubic vars=====================================================

//let clearCache = async function clearCacheData() {

//    await AdminLayout.clear().then(function () {
//        // Run this code once the database has been entirely deleted.
//        SetDataToLocalStorage('Config', "Application Configuration", defaultValue);
//    }).catch(function (err) {
//        // This code runs if there were any errors
//        console.log(err);
//    });

//}
//Document ready execution
$(document).ready(async function () {

    $("#LayoutUl li").removeClass("active");
    $('#LibraryLI').addClass('active treeview');
    $('#Room-LibraryModule a').css({
        'color': 'darkblue',
        'font-weight': 'bold'
    });

    jsFormObject.apiUrlToGetAllLibrary = apiUrlToGetAllLibrary;
    jsFormObject.cacheKeyLibrary = cacheKeyLibrary;
    jsFormObject.cacheKeyRoom = cacheKeyRoom;
    jsFormObject.sessionId = sessionId;
    jsFormObject.FillIndex = fillRoomIndex;
    jsFormObject.useCache = true;
    jsFormObject.fillLibrarySelectList = fillDataInLibrarySelectList;
    //retrive cache value for making api calls
    let configData = await GetConfigData('Config');

    backroundCache = configData.UseRedisCache;

    //Initial populate the table
    await PopulateLibrary(jsFormObject, $('#SelectedLibraries'));
});

let RefreshRoomIndexWithoutCache = async function refreshRoomIndexWithoutCache() {
    $('#indexDataTable').DataTable().destroy();
    $('#indexDataTable tbody').empty();

    await PopulateLibrary(jsFormObject, $('#SelectedLibraries'));
}

//populate library data from database in library select list box
let PopulateLibrary = async function populateLibrary(obj, selectListObject) {

    let cacheData = await GetDataFromLocalStorage(obj.cacheKeyLibrary, obj.sessionId, obj.useCache);

    if (cacheData !== null) {
        obj.fillLibrarySelectList(cacheData, selectListObject);
        selectListObject.focus();
        return;
    }

    window.$.ajax({
        url: `https://localhost/ApiGateway/${apiModuleLibrary}/getall?useCache=${backroundCache}`,
        type: "GET",
        datatype: "json",
        headers: {
            "School": jsTokenObject.schoolName,
            "Authorization": jsTokenObject.token
        },
        beforeSend: function () {
            showLoader();
        },
        success: function (data) {

            if (data.status === "Success") {
                //Storing data to client browser
                SetDataToLocalStorage(obj.cacheKeyLibrary, obj.sessionId, data, 20);
                obj.fillLibrarySelectList(data, selectListObject);
                selectListObject.focus();
            }
        },
        error(err) {
            console.log(err);
            toastr.error('Oops! something went wrong!', "Error!");
            hideLoader();
        },
        complete: function () {
            hideLoader();
        }
    });

};


//When library name changed on index page
$('#SelectedLibraries').on('change', async function (e) {

    $('#indexDataTable').DataTable().destroy();
    $('#indexDataTable tbody').empty();

    PopulateItemsOnRoomTable(this.value, jsFormObject);
});


//On page load, it will populate the records
let PopulateItemsOnRoomTable = async function populateItemsOnRoomTable(libraryId, obj) {

    //if libraryId is null then do not proceed
    if (libraryId === '' || libraryId === '-1') {
        return;
    }

    //Get cache data
    let cacheData = await GetDataFromLocalStorage(`${obj.cacheKeyRoom}_${libraryId}`, obj.sessionId, obj.useCache);

    if (cacheData != null) {
        fillRoomIndex(cacheData, $("#SelectedLibraries"));
        window.$("#SelectedLibraries").focus();
        return;
    }

    $.ajax({
        url: `https://localhost/ApiGateway/room/getAll?useCache=${backroundCache}&libraryId=${libraryId}`,
        type: "GET",
        datatype: "application/json",
        headers: {
            'School': jsTokenObject.schoolName,
            'Authorization': jsTokenObject.token,
            'Content-Type': 'application/json; charset=utf-8'
        },
        beforeSend: function () {
            showLoader();
        },
        success: function (data, status) {

            if (data.status === "Success") {

                //Storing data to client browser
                SetDataToLocalStorage(`${obj.cacheKeyRoom}_${libraryId}`, obj.sessionId, data, 20);
                fillRoomIndex(data, $("#SelectedLibraries"));
                window.$("#SelectedLibraries").focus();
            }
        },
        error(err) {
            console.log(err);
            $(window).ready(hideLoader);
            toastr.error('Oops! something went wrong!', "Error!");
            // Strongly recommended: Hide loader after 20 seconds, even if the page hasn't finished loading
            setTimeout(hideLoader, 20 * 1000);
        },
        complete: function () {
            $(window).ready(hideLoader);
            setTimeout(hideLoader, 20 * 1000);
        }
    });
}



//it will fill data to library select list - from cache or server
let fillDataInLibrarySelectList = async function fillLibraryDataInSelectOption(data, selectElement) {

    selectElement.empty();

    selectElement
        .append($("<option></option>")
            .attr("value", -1)
            .text("Select Library"));
    $.each(data.Result,
        function (key, val) {

            selectElement
                .append($("<option></option>")
                    .attr("value", val.id)
                    .text(val.name));
        });
}

//============== new imp
let fillRoomIndex = async function fillRoomIndex(data) {
    $.each(data.Result,
        function (key, val) {

            let body = "<tr>";
            hrefid = "#modal-default class='open-RoomModel'";
            hrefdel = "class='delete-RoomRecord'";
            hrefEdit = "class='open-RoomModel'";
            editIcon = "<i class='ion ion-edit btn btn-info' style='font-size:20px;'></i>";
            delIcon = "<i class='ion ion-trash-b btn btn-warning' style='font-size:20px;'></i>";
            body += "<td> <a href= " +
                hrefid +
                " data-id='" +
                val.id +
                "' data-libraryid='" +
                val.libraryId +
                "'>" +
                val.name +
                "</a></td>";
            /*body += "<td>" + libName + "</td>";*/
            if (val.isEditable == true) {
                body += "<td> <a href='#' " +
                    hrefEdit +
                    " data-id='" +
                    val.id +
                    "' data-libraryid='" +
                    val.libraryId +
                    "' >&nbsp&nbsp" +
                    editIcon +
                    "</a>" +
                    "<a href='#' " +
                    hrefdel +
                    " data-id='" +
                    val.id +
                    "' data-libraryid='" +
                    val.libraryId +
                    "' >&nbsp&nbsp" +
                    delIcon +
                    "</a>"
                "</td>";
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
};

$("#newRecord").click(function () {
    changeButtonCRUDState();
    PopulateLibrary(jsFormObject, $("#Libraries"));
    $('#Name').prop('disabled', true);
});

//When library name changed on index page
$('#Libraries').on('change', async function (e) {

    if (this.value === '-1') {
        $('#Name').prop('disabled', true);
        return;
    }
    else {
        $('#Name').prop('disabled', false);

    }
    $('#LibraryId').val(this.value);


});

//common submit form for all the modules
//it will get called on submit click - while click on side panel save button
$('#modelFormRoom').submit(async function (e) {
    showLoader();
    e.preventDefault();
    $("#LibraryId").val($('#Libraries').find(":selected").val());

    let id = getFormElementsData($(this)).Id;
    let formData = JSON.stringify(getFormElementsData($(this)));
    let urlPath = `https://localhost/ApiGateway/${apiModuleRoom}/save?useCache=${backroundCache}`;
    if (id !== '') {
        urlPath = `https://localhost/ApiGateway/${apiModuleRoom}/update?id=${id}&useCache=${backroundCache}`;
    }

    //passing object for saving or updating record to db
    let objectToProcess = {};
    objectToProcess.urlPathToSave = urlPath;
    objectToProcess.formData = formData;
    objectToProcess.cacheKeyLibrary = cacheKeyLibrary;
    objectToProcess.cacheKeyRoom = cacheKeyRoom;
    objectToProcess.sessionId = `Rooms_${scopeTokenData.id}`;
    objectToProcess.apiUrlToGetAll = `https://localhost/ApiGateway/${apiModuleRoom}/getall?libraryId=${libId}`;
    objectToProcess.fnTableFill = fillRoomIndex;
    await submitForm(objectToProcess);
});

//submit form
 async function submitForm(obj) {
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

                //Close model
                $('#modal-default').modal('toggle');
                $('#indexDataTable').DataTable().destroy();
                $('#indexDataTable tbody').empty();
                //add to cache

                //Reading previously stored value and then update it if update process
                //start otherwise add new record
                let libId = $("#LibraryId").val();
                obj.libId = libId;
                printLog(`${obj.cacheKeyRoom}_${libId}`, "id check");
                GetDataFromLocalStorage(`${obj.cacheKeyRoom}_${libId}`, obj.sessionId, obj.useCache)
                    .then(function (cacheData) {
                        if (cacheData) {
                            addRowToIndex(cacheData, data.Result, data.Result.id, obj);
                            obj.fnTableFill(cacheData);

                        } else {
                            PopulateItemsOnRoomTable(libId, obj);
                        }
                    })
                    .catch(e => {
                        console.log(e.message);
                    }); // Received a function as a value

                let selectedLib = $('#Libraries').find(":selected").text();
                $("#SelectedLibraries").find("option:contains(" + selectedLib + ")").attr('selected', 'selected');

                //Reset Form
                $('#modelFormRoom').each(function () {
                    this.reset();
                    $('#Id').val("");
                });
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

let addRowToIndex = async function AddRowToIndex(cacheData, ajaxData, id, obj) {

    let isFound = false;
    $.each(cacheData.Result, function (key, val) {
        if (val.id === id) {
            //update condition
            val.name = ajaxData.name;
            val.isEditable = true;
            isFound = true;
        }
    });
    if (!isFound) {
        //new record condition
        ajaxData.isEditable = true;
        cacheData.Result.push(ajaxData);
    }
    printLog(`${obj.cacheKeyRoom}_${obj.libId}`, "check here");
    //store newly updated data inside cache
    return await SetDataToLocalStorage(`${obj.cacheKeyRoom}_${obj.libId}`, obj.sessionId, cacheData, 20);
}


//Open for edit record
$(document).on("click", ".open-RoomModel", async function () {
    let recordId = $(this).attr("data-id"); //for record
    let libraryId = $(this).attr("data-libraryid"); //for record


    showLoader();
    $('#modal-default').modal('show');
    changeButtonCRUDState();
    loadRoomRecord(recordId, libraryId);

});

//Load a record for editing when model open
let loadRoomRecord = async function LoadRoomRecord(id, libraryId) {

    $.ajax({
        type: 'GET',
        datatype: "json",
        url: `https://localhost/ApiGateway/room/get?id=${id}&LibraryId=${libraryId}`,
        headers: {
            'School': jsTokenObject.schoolName,
            'Authorization': jsTokenObject.token,
            'Content-Type': 'application/json; charset=utf-8'
        },
        success: function (result) {
            //check response status
            if (result.status === 'Success') {
                $("#Name").val(result.Result.name);
                $("#Id").val(result.Result.id);
                $("#LibraryId").val(libraryId);
                $("#Libraries").empty();

                //Populate library data on model
                /*PopulateLibrary(jsFormObject, $("#Libraries"));*/

                //$("#Libraries")
                //    .append($("<option></option>")
                //        .attr("value", -1)
                //        .text("Select Library"));


                //iterate foreach loop with select option dropdown
                $("#SelectedLibraries option").each(function () {
                    console.log('Text:-' + this.text + '  Value:-' + this.value);
                    if (this.value === libraryId) {
                        $("#Libraries").append($("<option selected></option>")
                            .attr("value", this.value)
                            .text(this.text));

                    } else {
                        $("#Libraries").append($("<option></option>")
                            .attr("value", this.value)
                            .text(this.text));

                    }
                });

                $('#Libraries').prop('disabled', true);
                $("#Libraries").attr("title", "Moving room to another library is not permitted!");


                if (result.Result.isEditable === false) {
                    $("#saveLi").css("display", "none");
                    $("#cancelLi").css("display", "block");
                }
                hideLoader();

                //$("#IsEditable").val(result.Result.isEditable);
                //let selectedLib = $('#SelectedLibraries').find(":selected").text();
                
                //$("#Libraries").find("option:contains(" + selectedLib + ")").attr('selected', 'selected');

                //let selectedLibId = $('#Libraries').find(":selected").val();
                //printLog(selectedLibId);
                //if (selectedLibId === undefined) {
                //    $("#Libraries").val($("#Libraries option:first").val());
                //    $("#LibraryId").val($('#Libraries').find(":selected").val());

                //} else {
                //    $("#LibraryId").val(selectedLibId);
                //}

            }

            //hide new record button
            //show update record button
        },
        error: function (req, status, error) {
            console.log(req);
            console.log(status);
            console.log(error);
            ////display error message
            toastr.warning("Opps! something went wrong!", "Error");
            hideLoader();
        }
    });
};


//Delete record
$(document).on("click", ".delete-RoomRecord", async function () {
    let recordId = $(this).data('id');
    let trSequence = $(this);
    let libId = $('#SelectedLibraries').find(":selected").val();

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
                deleteRoomData(trSequence, `https://localhost/ApiGateway/room/delete?useCache=${backroundCache}&id=${recordId}&libraryId=${libId}`,libId);
                
            }
        });
});

let deleteRoomData = function DeleteRoomData(trSequence, apiUrl,libraryId) {
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
            
            //Finding data from browser cache
            GetDataFromLocalStorage(`${cacheKeyRoom}_${libraryId}`, sessionId, jsFormObject.useCache)
                .then(function (cacheData) {
                    
            
                    if (cacheData) {
                        cacheData.Result = cacheData.Result.filter(e => e.id !== ajaxData.Result.id);
            
                        SetDataToLocalStorage(`${cacheKeyRoom}_${libraryId}`, sessionId, cacheData, 20);
                        if (cacheData.Result.length > 0) {
                            fillRoomIndex(cacheData);
                            $('.searchLabel').focus();
                            return;
                        }
                    }
                });
            //check response status
            hideLoader();

            if (ajaxData.status === "Success") {
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

////public varibales for the form specific
//let cacheKey = 'LibraryRoomModule';
//let cachekeyForLibraryList = 'LibraryModule';
//let sessionIdForLibraryList = `Libraries_${scopeTokenData.id}`;
//let sessionIdForRoomList = `Rooms_In_Library_${scopeTokenData.id}`;

//let sessionId = `LibraryRoom_${scopeTokenData.id}`;
//let apiLibModule = "library";
//let apiModule = "room";
//let apiUrlToGetAll = `https://localhost/ApiGateway/${apiModule}/getall?useCache=false`;
//let apiUrlToGetById = `https://localhost/ApiGateway/${apiModule}/get??useCache=false&id=`;

//let jsFormObject = {};

////===end pubic vars=====================================================

////Attached to index document for room
//$(document).ready(async function () {

//    $("#LayoutUl li").removeClass("active");
//    $('#LibraryLI').addClass('active treeview');
//    $('#Room-LibraryModule a').css({
//        'color': 'darkblue',
//        'font-weight': 'bold'
//    });

//    jsFormObject.apiUrlToGetAll = apiUrlToGetAll;
//    jsFormObject.cacheKey = cacheKey;
//    jsFormObject.sessionId = sessionId;
//    jsFormObject.FillIndex = fillRoomIndex;
//    jsFormObject.useCache = true;
//    let configData = await GetConfigData('Config');
//    backroundCache = configData.UseRedisCache;

//    PopulateLibraryData($("#SelectedLibraries"));
//});


////When library name changed on model
//$('#Libraries').on('change', function (e) {
//    $('#LibraryId').val(this.value);

//});

//let RefreshRoomIndexWithoutCache = async function RefreshIndexWithoutCache() {
//    jsFormObject.apiUrlToGetAll = apiUrlToGetAll;
//    jsFormObject.cacheKey = cacheKey;
//    jsFormObject.sessionId = sessionId;
//    jsFormObject.FillIndex = fillRoomIndex;
//    jsFormObject.useCache = false;
//    let configData = await GetConfigData('Config');
//    backroundCache = configData.UseRedisCache;
//    $('#indexDataTable').DataTable().destroy();
//    $('#indexDataTable tbody').empty();

//    PopulateLibraryData($("#SelectedLibraries"));
//    jsFormObject.useCache = true;
//}

////On page load, it will populate the library records
//let PopulateLibraryData = async function PopulateLibraryData(selectedElement) {
//    //selectElement.empty();

//    //Finding data from browser cache

//    let indexData = await GetDataFromLocalStorage(cachekeyForLibraryList, sessionIdForLibraryList, jsFormObject.useCache);
//    if (indexData != null) {
//        fillDataInLibrarySelectList(indexData, selectedElement);
//        $('.searchLabel').focus();
//        return;
//    }

//    $.ajax({
//        url: `https://localhost/ApiGateway/library/getall?useCache=${backroundCache}`,
//        type: "GET",
//        datatype: "application/json",
//        headers: {
//            'School': jsTokenObject.schoolName,
//            'Authorization': jsTokenObject.token,
//            'Content-Type': 'application/json; charset=utf-8'
//        },
//        beforeSend: function () {
//            showLoader();
//        },
//        success: function (data) {
//            if (data.status === 'Success') {
//                SetDataToLocalStorage(cachekeyForLibraryList, sessionIdForLibraryList, data.Result, 20);
//                fillDataInLibrarySelectList(data.Result, selectedElement);
//            }
//        },
//        error(err) {
//            console.log(err);
//            $(window).ready(hideLoader);
//            toastr.error('Oops! something went wrong!', "Error!");
//            // Strongly recommended: Hide loader after 20 seconds, even if the page hasn't finished loading
//            setTimeout(hideLoader, 20 * 1000);
//        },
//        complete: function () {
//            $(window).ready(hideLoader);
//            setTimeout(hideLoader, 20 * 1000);
//        }
//    });
//}

////it will fill data to library select list - from cache or server
//let fillDataInLibrarySelectList = function fillLibraryDataInSelectOptioin(data, selectElement) {
//    selectElement.empty();

//    selectElement
//        .append($("<option></option>")
//            .attr("value", -1)
//            .text("Select Library"));
//    $.each(data, function (key, val) {

//        selectElement
//            .append($("<option></option>")
//                .attr("value", val.id)
//                .text(val.name));
//    });

//    let selectedLib = $('#SelectedLibraries').find(":selected").text();
//    $("#Libraries").find("option:contains(" + selectedLib + ")").attr('selected', 'selected');

//    let selectedLibId = $('#SelectedLibraries').find(":selected").val();
//    if (selectedLibId === '') {
//        $("#Libraries").val($("#Libraries option:first").val());
//        $("#LibraryId").val($('#Libraries').find(":selected").val());

//    } else {
//        $("#LibraryId").val(selectedLibId);
//    }

//}
//let changeButtonCRUDState = function (advanceForm) {
//    $('#newRecordLi').css("display", "none");
//    $('#cancelLi').css("display", "block");
//    $('#saveLi').css("display", "block");

//    //Reset Form
//    if (advanceForm === undefined) {
//        $('#modelFormRoom').each(function () {
//            this.reset();
//        });

//    }
//};










////edit record icon click
////$(document).on("click", ".edit-RoomRecord", function () {
////    let recordId = $(this).attr("data-id"); //for record
////    let libraryId = $(this).attr("data-libraryid"); //for record
////    printLog(recordId,"dddd")
////    showLoader();
////    $('#modal-default').modal('show');
////    changeButtonCRUDState();
////    loadRoomRecord(recordId, libraryId);
////});

////Save or update record.
//$('#modelFormRoom').submit(async function (e) {

//    showLoader();
//    e.preventDefault();
//    let id = getFormElementsData($(this)).Id;
//    let formData = JSON.stringify(getFormElementsData($(this)));

//    if ($("#LibraryId").val() === '') {
//        toastr.error('Please select library!', 'Error!');
//        hideLoader();
//        return;
//    }

//    let urlPath = 'https://localhost/ApiGateway/room/save';
//    if (id != '') {
//        urlPath = 'https://localhost/ApiGateway/room/update?id=' + id;
//    }

//    $.ajax({
//        type: 'POST',
//        url: urlPath,
//        data: formData,
//        headers: {
//            'School': jsTokenObject.schoolName,
//            'Authorization': jsTokenObject.token,
//            'Content-Type': 'application/json; charset=utf-8'
//        },
//        success: function (ajaxData) {
//            //Display message
//            displayAlert(ajaxData);
//            //check response status
//            if (ajaxData.status === 'Success') {

//                let selectedLibId = $('#Libraries').find(":selected").val();


//                GetDataFromLocalStorage(cacheKey + '_' + selectedLibId, sessionIdForRoomList, jsFormObject.useCache)
//                    .then(function (cacheData) {

//                        if (cacheData) {

//                            $('#indexDataTable').DataTable().destroy();
//                            $('#indexDataTable tbody').empty();


//                            //come here
//                            addRowToRoomIndex(ajaxData, cacheData.Result, ajaxData.Result.id);


//                            SetDataToLocalStorage(cacheKey + '_' + selectedLibId, sessionIdForRoomList, cacheData, 20);

//                            fillRoomIndex(cacheData);

//                            //Reset Form

//                            ////Close model
//                            $('#modal-default').modal('toggle');
//                            resetRoomButtonState();
//                            hideLoader();

//                        } else {
//                            printLog("No cache data", "local cache");

//                        }
//                    })
//                    .catch(e => {
//                        console.log(e.message);
//                    }); // Received a function as a value

//            }

//            //hide loader on success or fail status


//        },
//        error: function (req, status, error) {
//            console.log(req);
//            console.log(status);
//            console.log(error);
//            ////display error message
//            toastr.warning("Opps! Something went wrong!", "Error");
//            hideLoader();

//        }, complete: function () {
//            $(window).ready(hideLoader);
//            setTimeout(hideLoader, 20 * 1000);
//        }
//    });
//});
//let resetRoomButtonState = function resetRoomButtonState() {
//    $('#modelFormRoom').each(function () {
//        this.reset();
//    });
//    $('#Id').val("");
//    $("#newRecordLi").css("display", "block");
//    $("#saveLi").css("display", "none");
//    $("#cancelLi").css("display", "none");
//    //Reset form
//    //Reseting previously pressed key
//    keyPressStatus = "";

//}
////Adding or upading record to cache
//let addRowToRoomIndex = async function fillRoomIndex(ajaxData, cacheData, id) {

//    let isFound = false;

//    $.each(cacheData, function (key, val) {


//        if (val.id === id) {
//            val.name = ajaxData.Result.name;
//            val.isEditable = true;
//            isFound = true;
//        }
//    });
//    if (!isFound) {
//        ajaxData.Result.isEditable = true;
//        cacheData.push(ajaxData.Result);
//    }

//    ajaxData.Result = cacheData;

//}


////On page load, it will populate the records
//let PopulateItemsOnRoomTable = async function PopulateItemsOnRoomTable(libraryId) {
//    //let libraryId = $('#SelectedLibraries').find(":selected").val()
//    //if libraryId is null then do not proceed
//    if (libraryId === "" || libraryId === '-1') {
//        return;
//    }
//    let recordStoredBefore = false;

//    //Finding data from browser cache
//    let indexData = await GetDataFromLocalStorage(cacheKey + '_' + libraryId, sessionIdForRoomList, jsFormObject.useCache);

//    if (indexData != null) {

//        indexData.Result = indexData.Result.filter(e => e.libraryId === libraryId);

//        if (indexData.Result.length > 0) {
//            fillRoomIndex(indexData);
//            $('.searchLabel').focus();
//            return;
//        }
//    }

//    $.ajax({
//        url: `https://localhost/ApiGateway/room/getAll?useCache=${backroundCache}&libraryId=${libraryId}`,
//        type: "GET",
//        datatype: "application/json",
//        headers: {
//            'School': jsTokenObject.schoolName,
//            'Authorization': jsTokenObject.token,
//            'Content-Type': 'application/json; charset=utf-8'
//        },
//        beforeSend: function () {
//            showLoader();
//        },
//        success: function (data, status) {

//            if (data.status === "Success") {
//                //if new data list has records, push that records to application cache
//                if (data.recordCount > 0) {
//                    SetDataToLocalStorage(cacheKey + '_' + libraryId, sessionIdForRoomList, data, 20);
//                    useCache = true;
//                    fillRoomIndex(data);
//                }
//            }
//        },
//        error(err) {
//            console.log(err);
//            $(window).ready(hideLoader);
//            toastr.error('Oops! something went wrong!', "Error!");
//            // Strongly recommended: Hide loader after 20 seconds, even if the page hasn't finished loading
//            setTimeout(hideLoader, 20 * 1000);
//        },
//        complete: function () {
//            $(window).ready(hideLoader);
//            setTimeout(hideLoader, 20 * 1000);
//        }
//    });
//}

////When library name changed on index page
//$('#SelectedLibraries').on('change', function (e) {
//    if (this.value === '') {
//        return;
//    }
//    $('#indexDataTable').DataTable().destroy();
//    $('#indexDataTable tbody').empty();
//    /*$('#indexDataTable').DataTable().clear();*/

//    PopulateItemsOnRoomTable(this.value);
//});


////On new record click
//$("#newRecord").click(function () {
//    useCache = true;
//    PopulateLibraryData($("#Libraries"));

//    changeButtonCRUDState(true);
//});


////============== new imp
//let fillRoomIndex = function fillRoomIndex(data) {
//    $.each(data.Result, function (key, val) {

//        let body = "<tr>";
//        hrefid = "#modal-default class='open-RoomModel'";
//        hrefdel = "class='delete-RoomRecord'";
//        hrefEdit = "class='open-RoomModel'";
//        editIcon = "<i class='ion ion-edit btn btn-info' style='font-size:20px;'></i>";
//        delIcon = "<i class='ion ion-trash-b btn btn-warning' style='font-size:20px;'></i>";
//        body += "<td> <a href= " + hrefid + " data-id='" + val.id + "' data-libraryid='" + val.libraryId + "'>" + val.name + "</a></td>";
//        /*body += "<td>" + libName + "</td>";*/
//        if (val.isEditable == true) {
//            body += "<td> <a href='#' " + hrefEdit +
//                " data-id='" +
//                val.id + "' data-libraryid='" + val.libraryId +
//                "' >&nbsp&nbsp" + editIcon +
//                "</a>" +
//                "<a href='#' " + hrefdel +
//                " data-id='" +
//                val.id + "' data-libraryid='" + val.libraryId +
//                "' >&nbsp&nbsp" +
//                delIcon +
//                "</a>"
//            "</td>";
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
//    $('.searchLabel').focus();
//}