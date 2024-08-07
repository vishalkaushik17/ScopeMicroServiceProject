cacheKeyLibrary = 'LibraryModule';
cacheKeyRoom = 'RoomModule';
cacheKeySection = 'SectionModule';
sessionId = `Sections_${scopeTokenData.id}`;
apiModuleLibrary = "Library";
apiModuleRoom = "Room";
apiModuleSection = "Section";
libId = '';
roomId = '';
sectionId = '';
apiUrlToGetAllLibrary = `https://localhost/ApiGateway/${apiModuleLibrary}/getall`;
apiUrlToGetAllRoom = `https://localhost/ApiGateway/${apiModuleRoom}/getall?libraryId=${libId}`;
apiUrlToGetAllSection = `https://localhost/ApiGateway/${apiModuleSection}/getall?roomid=${roomId}`;
apiUrlToSectionGetById = `https://localhost/ApiGateway/${apiModuleRoom}/get?roomId=${roomId}&sectionid=${sectionId}`;

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
    $('#Section-LibraryModule a').css({
        'color': 'darkblue',
        'font-weight': 'bold'
    });

    jsFormObject.apiUrlToGetAllLibrary = apiUrlToGetAllLibrary;
    jsFormObject.cacheKeyLibrary = cacheKeyLibrary;
    jsFormObject.cacheKeyRoom = cacheKeyRoom;
    jsFormObject.sessionId = sessionId;
    jsFormObject.FillSectionTable = fillSectionTable;
    jsFormObject.useCache = true;
    jsFormObject.fillLibrarySelectList = fillDataInLibrarySelectList;
    //retrive cache value for making api calls
    let configData = await GetConfigData('Config');

    backroundCache = configData.UseRedisCache;
    $('#SelectedRoomsOnIndex').prop('disabled', true);
    //Initial populate the table
    await FetchLibraryRecords(jsFormObject, $('#SelectedLibrariesOnIndex'));
});

//on refresh button it will execute to perform task without cache
let RefreshSectionIndexWithoutCache = async function refreshSectionIndexWithoutCache() {
    $('#indexDataTable').DataTable().destroy();
    $('#indexDataTable tbody').empty();
    $('#SelectedLibrariesOnIndex').empty();
    $('#SelectedRoomsOnIndex').prop('disabled', true);
    $('#SelectedRoomsOnIndex').empty();
    $('#SelectedRoomsOnIndex')
        .append($("<option></option>")
            .attr("value", -1)
            .text("Select Room"));
    //alert(0);
    await FetchLibraryRecords(jsFormObject, $('#SelectedLibrariesOnIndex'));
}

//When library name changed on index page , change rooms
$('#SelectedLibrariesOnIndex').on('change', async function (e) {

    if (jsFormObject.libId === '' || jsFormObject.libId === '-1') {
        $('#SelectedRoomsOnIndex').prop('disabled', true);
        return;
    }
    jsFormObject.libId = $('#SelectedLibrariesOnIndex').val();
    $('#SelectedRoomsOnIndex').empty();
    $('#SelectedRoomsOnIndex')
        .append($("<option></option>")
            .attr("value", -1)
            .text("Select Room"));

    FetchRoomRecords(jsFormObject, $("#SelectedRoomsOnIndex"));
});

//When library name changed on index page , change rooms
$('#Libraries').on('change', async function (e) {
    $('#Rooms').empty();
    $('#Rooms')
        .append($("<option></option>")
            .attr("value", -1)
            .text("Select Room"));
    $('#Name').prop('disabled', true);
    if (this.value === '' || this.value === '-1') {
        $('#Rooms').prop('disabled', true);
        return;
    }
    jsFormObject.libId = $('#Libraries').val();
    $('#LibraryId').val(this.value);
    FetchRoomRecords(jsFormObject, $("#Rooms"));
   

});

//on model if roow change assign room id 
$('#Rooms').on('change', async function (e) {

    if (this.value === '-1') {
        $('#Name').prop('disabled', true);
        return;
    } 

    $('#Name').prop('disabled', false);
    $('#RoomsId').val(this.value);
    
});


//When room name changed on index page , update index table
$('#SelectedRoomsOnIndex').on('change', async function (e) {

    $('#indexDataTable').DataTable().destroy();
    $('#indexDataTable tbody').empty();


    jsFormObject.apiUrlToGetAllLibrary = apiUrlToGetAllLibrary;
    jsFormObject.cacheKeyLibrary = cacheKeyLibrary;
    jsFormObject.cacheKeyRoom = cacheKeyRoom;
    jsFormObject.sessionId = sessionId;
    jsFormObject.FillSectionTable = fillSectionTable;
    jsFormObject.useCache = true;
    jsFormObject.fillLibrarySelectList = fillDataInLibrarySelectList;
    //retrive cache value for making api calls
    let configData = await GetConfigData('Config');

    backroundCache = configData.UseRedisCache;

    jsFormObject.libId = $('#SelectedLibrariesOnIndex').val();
    jsFormObject.roomId = $('#SelectedRoomsOnIndex').val();
    
    FetchSectionRecords(this.value, jsFormObject.libId, jsFormObject);
});

//populate library data from database in library select list box
let FetchLibraryRecords = async function FetchLibraryRecords(obj, selectListObject) {

    let cacheData = await GetDataFromLocalStorage(obj.cacheKeyLibrary, obj.sessionId, obj.useCache);

    if (cacheData !== null) {
        if (cacheData.recordCount === 0) {
            toastr.warning(data.message, "Warning");
            $('#SelectedLibrariesOnIndex').prop('disabled', true);
            return;
        }
        obj.fillLibrarySelectList(cacheData, selectListObject, "Select Library");
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
                if (data.recordCount === 0) {
                    toastr.warning(data.message, "Warning");
                    $('#SelectedLibrariesOnIndex').prop('disabled', true);
                    return;
                }
                //Storing data to client browser
                SetDataToLocalStorage(obj.cacheKeyLibrary, obj.sessionId, data, 20);
                obj.fillLibrarySelectList(data, selectListObject,"Select Library");
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



//populate room data from database in room select list box
let FetchRoomRecords = async function fetchRoomRecords(obj, selectListObject) {

    let cacheData = await GetDataFromLocalStorage(`${obj.cacheKeyRoom}_${obj.libId}`, obj.sessionId, obj.useCache);
    
    if (cacheData !== null) {
        
        if (cacheData.recordCount === 0) {
            toastr.warning(data.message, "Warning");
            selectListObject.prop('disabled', true);
            return;
        }
        obj.fillLibrarySelectList(cacheData, selectListObject, "Select Room");
        selectListObject.prop('disabled', false);
        selectListObject.focus();
        return;
    }

    window.$.ajax({
        url: `https://localhost/ApiGateway/${apiModuleRoom}/getall?useCache=${backroundCache}&libraryId=${obj.libId}`,
        type: "GET",
        datatype: "json",
        headers: {
            "School": jsTokenObject.schoolName,
            "Authorization": jsTokenObject.token
        },
        beforeSend: function () {
            showLoader();
        },
        success: function (ajaxData) {

            if (ajaxData.status === "Success") {
                if (ajaxData.recordCount === 0) {
                    toastr.warning(ajaxData.message, "Warning");
                    selectListObject.prop('disabled', true);
                    return;
                }
                //Storing data to client browser
                SetDataToLocalStorage(`${obj.cacheKeyRoom}_${obj.libId}`, obj.sessionId, ajaxData, 20);
                selectListObject.prop('disabled', false);
                obj.fillLibrarySelectList(ajaxData, selectListObject,"Select Room");
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



//On page load, it will populate the records
let FetchSectionRecords = async function fetchSectionRecords(roomId,libraryId, obj) {

    //if libraryId is null then do not proceed
    if (libraryId === '-1' || roomId === '-1') {
        return;
    }


    //Get cache data
    let cacheData = await GetDataFromLocalStorage(`${cacheKeySection}_${roomId}`, sessionId, useCache);
    if (cacheData != null) {
        if (cacheData.recordCount > 0) {
            fillSectionTable(cacheData);
        }
        return;
    }

    $.ajax({
        url: `https://localhost/ApiGateway/section/getAll?useCache=${backroundCache}&roomId=${roomId}`,
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
        success: function (ajaxData, status) {
            
            if (ajaxData.status === "Success") {
                
                if (ajaxData.recordCount > 0) {
                    //Storing data to client browser
                    SetDataToLocalStorage(`${cacheKeySection}_${roomId}`, sessionId, ajaxData, 20);
                    fillSectionTable(ajaxData);
                    window.$("#SelectedLibraries").focus();

                }
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
let fillDataInLibrarySelectList = async function fillLibraryDataInSelectOption(data, selectElement,firstValue) {

    selectElement.empty();

    selectElement
        .append($("<option></option>")
            .attr("value", -1)
            .text(firstValue));
    $.each(data.Result,
        function (key, val) {

            selectElement
                .append($("<option></option>")
                    .attr("value", val.id)
                    .text(val.name));
        });
}

//============== new imp
let fillSectionTable = async function fillSectionTable(data) {
    
    $.each(data.Result, function (key, val) {
       

        let body = "<tr>";
        hrefid = "#modal-default";
        hrefdel = "class='delete-SectionRecord'";
        hrefEdit = "class='edit-RecordSection'";
        editIcon = "<i class='ion ion-edit btn btn-info' style='font-size:20px;'></i>";
        delIcon = "<i class='ion ion-trash-b btn btn-warning' style='font-size:20px;'></i>";
        body += `<td> <a href= '${hrefid}' class='open-ModelSection' data-Id='${val.id}' data-libraryId='${val.libraryId}' data-roomId='${val.roomId}'> ${val.name}</a></td>`;
        /*body += "<td>" + libName + "</td>";*/
        if (val.isEditable === true) {
            body += `<td> <a href='#' ${hrefEdit} data-Id='${val.id}' data-libraryId='${val.libraryId
                }' data-roomId='${val.roomId}' &nbsp&nbsp ${editIcon} </a>
                    <a href='#' ${hrefdel} data-Id='${val.id}' data-libraryId='${val.libraryId}' data-roomId='${val.roomId}'> &nbsp&nbsp
                        ${delIcon} </a></td>`;
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


//When library name changed on index page
$('#Libraries').on('change', async function (e) {

    if (this.value === '-1') {
        $('#Rooms').prop('disabled', true);
        return;
    }
    else {
        $('#Rooms').prop('disabled', false);

    }
    $('#LibraryId').val(this.value);

});

//common submit form for all the modules
//it will get called on submit click - while click on side panel save button
$('#modelFormSection').submit(async function (e) {
    showLoader();
    e.preventDefault();
    $("#LibraryId").val($('#Libraries').find(":selected").val());
    $("#RoomId").val($('#Rooms').find(":selected").val());

    let id = getFormElementsData($(this)).Id;
    let formData = JSON.stringify(getFormElementsData($(this)));
    let urlPath = `https://localhost/ApiGateway/${apiModuleSection}/save?useCache=${backroundCache}`;
    if (id !== '') {
        urlPath = `https://localhost/ApiGateway/${apiModuleSection}/update?id=${id}&useCache=${backroundCache}`;
    }

    //passing object for saving or updating record to db
    let objectToProcess = {};
    objectToProcess.urlPathToSave = urlPath;
    objectToProcess.formData = formData;
    objectToProcess.cacheKeyLibrary = cacheKeyLibrary;
    objectToProcess.cacheKeyRoom = cacheKeyRoom;
    objectToProcess.cacheKeySection = cacheKeySection;
    objectToProcess.sessionId = `Sections_${scopeTokenData.id}`;
    objectToProcess.apiUrlToGetAll = `https://localhost/ApiGateway/${apiModuleSection}/getall?useCache=${backroundCache}&RoomId=${$("#RoomId").val()}`;
    objectToProcess.FillSectionTable = fillSectionTable;
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
                let roomId = $("#RoomId").val();
                obj.libId = libId;
                obj.roomId = roomId;
                
                GetDataFromLocalStorage(`${cacheKeySection}_${roomId}`, sessionId, obj.useCache)
                    .then(function (cacheData) {
                        if (cacheData) {

                            addRowToIndex(cacheData, data.Result, data.Result.id, obj);
                            obj.FillSectionTable(cacheData);

                        } else {
                            FetchSectionRecords(roomId,libId, obj);
                        }
                    })
                    .catch(e => {
                        console.log(e.message);
                    }); // Received a function as a value

                let selectedLib = $('#Libraries').find(":selected").text();
                let selectedRoom = $('#Rooms').find(":selected").text();
                $("#SelectedLibrariesOnIndex").find("option:contains(" + selectedLib + ")").attr('selected', 'selected');
                $("#SelectedRoomsOnIndex").find("option:contains(" + selectedRoom + ")").attr('selected', 'selected');

                //Reset Form
                $('#modelFormRoom').each(function () {
                    this.reset();
                });
                $('#Id').val("");
                $('#Name').val("");
                $('#LibraryId').val("");
                $('#RoomId').val("");
    
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
    
    //store newly updated data inside cache
    return await SetDataToLocalStorage(`${cacheKeySection}_${obj.roomId}`, obj.sessionId, cacheData, 20);
}


$("#newRecord").click(function () {
    changeButtonCRUDState();
    /*FetchLibraryRecords(jsFormObject, $("#Libraries"));*/
    /*$('#Name').prop('disabled', true);*/
    changeButtonCRUDState();
    //loadRoomRecord(recordId, libraryId);
    let libraryId = $("#SelectedLibrariesOnIndex").val();
    let roomId = $("#SelectedRoomsOnIndex").val();
    $('#Name').prop('disabled', true);
    
    $("#Libraries").empty();
    $("#Rooms").empty();
    if (libraryId !== '-1' && roomId !== '-1') {
        $('#Name').prop('disabled', false);
    }
   
    loadLibraryAndRoomOnModel(libraryId, roomId);

});

//Open for edit record
$(document).on("click", ".open-ModelSection", async function () {
    
    let recordId = $(this).attr("data-id"); //for record
    let roomId = $(this).attr("data-roomId"); //for record
    let libraryId = $(this).attr("data-libraryId"); //for record

    /*let libraryId = $("#SelectedRoomsOnIndex").val();*/
    showLoader();
    $('#modal-default').modal('show');
    changeButtonCRUDState();
    
    $("#Libraries").empty();
    $("#Rooms").empty();
    loadLibraryAndRoomOnModel(libraryId,roomId);
    loadSectionRecordForEdit(recordId, roomId, libraryId);
    $('#Name').focus();
    

});

//it will load library records and room records on model when new button get clicked.
let loadLibraryAndRoomOnModel = async function (libraryId, roomId) {
    $("#SelectedLibrariesOnIndex option").each(function () {
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
    $("#SelectedRoomsOnIndex option").each(function () {
        if (this.value === roomId) {
            $("#Rooms").append($("<option selected></option>")
                .attr("value", this.value)
                .text(this.text));

        } else {
            $("#Rooms").append($("<option></option>")
                .attr("value", this.value)
                .text(this.text));
        }

    });
}
//Load a record for editing when model open
let loadSectionRecordForEdit = async function loadSectionRecordForEdit(id, roomId, libraryId) {

    $.ajax({
        type: 'GET',
        datatype: "json",
        url: `https://localhost/ApiGateway/section/get?id=${id}&roomid=${roomId}`,
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
                $("#RoomId").val(roomId);
                $("#Libraries").empty();
                $("#Rooms").empty();

                //Populate library data on model
                loadLibraryAndRoomOnModel(libraryId, roomId);
                $('#Libraries').prop('disabled', true);
                $('#Rooms').prop('disabled', true);
                $("#Libraries").attr("title", "Moving room to another library is not permitted!");
                $("#Rooms").attr("title", "Moving room to another room is not permitted!");


                if (result.Result.isEditable === false) {
                    $("#saveLi").css("display", "none");
                    $("#cancelLi").css("display", "block");
                }
                hideLoader();
                $("#Name").focus();
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
$(document).on("click", ".delete-SectionRecord", async function () {
    let recordId = $(this).data('id');
    let trSequence = $(this);
    
    let roomId = $(this).attr("data-roomId"); //for record 

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
                deleteSectionData(trSequence, `https://localhost/ApiGateway/section/delete?useCache=${backroundCache}&id=${recordId}&roomid=${roomId}`,recordId,roomId);

            }
        });
});

let deleteSectionData = function deleteSectionData(trSequence, apiUrl, recordId, roomId) {
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
                GetDataFromLocalStorage(`${cacheKeySection}_${roomId}`, sessionId, jsFormObject.useCache)
                    .then(function (cacheData) {


                        if (cacheData) {
                            cacheData.Result = cacheData.Result.filter(e => e.id !== recordId);

                            SetDataToLocalStorage(`${cacheKeySection}_${roomId}`, sessionId, cacheData, 20);
                            if (cacheData.Result.length > 0) {
                                fillSectionTable(cacheData);
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


//    //Default behaviour
//    $("#SelectedRoomsOnIndex").prop('disabled', true);

//    //fetch records from cache if available or fetch from api
//    useCache = true;
//    InsertDataToLibrarySelectListElement($("#SelectedLibrariesOnIndex"));
//});

//let changeButtonCRUDState = function (advanceForm) {
//    $("#newRecordLi").css("display", "none");
//    $("#cancelLi").css("display", "block");
//    $("#saveLi").css("display", "block");

//    //Reset Form
//    if (advanceForm === undefined) {
//        $('#modelForm').each(function () {
//            this.reset();
//        });

//    }
//};

////Load a record for editing when model open
//let loadData = function LoadData(id, libraryId, roomId) {

//    $.ajax({
//        type: 'GET',
//        datatype: "json",
//        url: `https://localhost/ApiGateway/section/get?id=${id}&RoomId=${roomId}&LibraryId=${libraryId}`,
//        headers: {
//            'School': jsTokenObject.schoolName,
//            'Authorization': jsTokenObject.token,
//            'Content-Type': 'application/json; charset=utf-8'
//        },
//        success: function (result) {

//            //check response status
//            if (result.status === 'Success') {
//                $("#Name").val(result.Result.name);
//                $("#Id").val(result.Result.id);
//                $("#LibraryId").val(libraryId);
//                $("#RoomId").val(roomId);
//                $("#Libraries").empty();

//                InsertDataToLibrarySelectListElement($("#Libraries"));

//                $('#Libraries').prop('disabled', true);
//                $('#Rooms').prop('disabled', true);
//                $("#Libraries").attr("title", "Moving section to another library is not permitted!");
//                $("#Rooms").attr("title", "Moving section to another room is not permitted!");

//                //$("#IsEditable").val(result.Result.isEditable);
//                hideLoader();
//                //hide new record button
//                //show update record button
//                if (result.Result.isEditable === false) {
//                    $("#saveLi").css("display", "none");
//                    $("#cancelLi").css("display", "block");
//                }
//            }

//        },
//        error: function (req, status, error) {
//            console.log(req);
//            console.log(status);
//            console.log(error);
//            ////display error message
//            toastr.warning("Opps! something went wrong!", "Error");
//            hideLoader();
//        }
//    });
//};

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

//    //let selectedLib = $('#SelectedLibrariesOnIndex').find(":selected").text();
//    //$("#Libraries").find("option:contains(" + selectedLib + ")").attr('selected', 'selected');

//    //let selectedLibId = $('#SelectedLibrariesOnIndex').find(":selected").val();
//    //if (selectedLibId === '') {
//    //    $("#Libraries").val($("#Libraries option:first").val());
//    //    $("#LibraryId").val($('#Libraries').find(":selected").val());

//    //} else {
//    //    $("#LibraryId").val(selectedLibId);
//    //}

//}


////it will fill data to room select list
//let fillDataInRoomSelectList = function FillDataInRoomSelectList(data, selectElement) {
//    selectElement.empty();
//    selectElement
//        .append($("<option></option>")
//            .attr("value", -1)
//            .text("Select Room"));
//    $.each(data, function (key, val) {
//        selectElement
//            .append($("<option></option>")
//                .attr("value", val.id)
//                .text(val.name));
//    });
//}

////Open for edit record
//$(document).on("click", ".open-Model", function () {
//    let recordId = $(this).attr("data-Id"); //for record 
//    let libraryId = $(this).attr("data-libraryId"); //for record 
//    let roomId = $(this).attr("data-roomId"); //for record 
//    showLoader();
//    $('#modal-default').modal('show');
//    changeButtonCRUDState();
//    loadData(recordId, libraryId, roomId);

//});

////Delete record
//$(document).on("click", ".delete-Record", function () {
//    let recordId = $(this).data('id');
//    let trSequence = $(this);
//    let libId = $('#SelectedLibrariesOnIndex').find(":selected").val();
//    let roomId = $('#SelectedRoomsOnIndex').find(":selected").val();
//    debugger;
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
//                deleteData(trSequence, 'https://localhost/ApiGateway/section/delete?id=' + recordId + '&libraryId=' + libId + '&roomId=' + roomId);
//                let table = $('#indexDataTable').DataTable();
//                hideLoader();
//            }
//        });
//});

////edit record icon click
//$(document).on("click", ".edit-Record", function () {
//    let recordId = $(this).attr("data-Id"); //for record 
//    let libraryId = $(this).attr("data-libraryId"); //for record 
//    let roomId = $(this).attr("data-roomId"); //for record 
//    showLoader();
//    $('#modal-default').modal('show');
//    changeButtonCRUDState();
//    loadData(recordId, libraryId, roomId);
//});
//let fillDataInIndex = function FillDataInIndex(data) {
//    $.each(data, function (key, val) {


//        let body = "<tr>";
//        hrefid = "#modal-default";
//        hrefdel = "class='delete-Record'";
//        hrefEdit = "class='edit-Record'";
//        editIcon = "<i class='ion ion-edit btn btn-info' style='font-size:20px;'></i>";
//        delIcon = "<i class='ion ion-trash-b btn btn-warning' style='font-size:20px;'></i>";
//        body += `<td> <a href= '${hrefid}' class='open-Model' data-Id='${val.id}' data-libraryId='${val.libraryId}' data-roomId='${val.roomId}'> ${val.name}</a></td>`;
//        /*body += "<td>" + libName + "</td>";*/
//        if (val.isEditable === true) {
//            body += `<td> <a href='#' ${hrefEdit} data-Id='${val.id}' data-libraryId='${val.libraryId
//                }' data-roomId='${val.roomId}' &nbsp&nbsp ${editIcon} </a> 
//                    <a href='#' ${hrefdel} data-Id='${val.id}' data-libraryId='${val.libraryId}' data-roomId='${val.roomId}'> &nbsp&nbsp 
//                        ${delIcon} </a></td>`;
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
////Save or update record.
//$('#modelForm').submit(function (e) {
//    showLoader();
//    e.preventDefault();
//    let id = getFormElementsData($(this)).Id;
//    let formData = JSON.stringify(getFormElementsData($(this)));

//    //if ($("#LibraryId").val() === '') {
//    //    toastr.error('Please select library!', 'Error!');
//    //    hideLoader();
//    //    return;
//    //}
//    //if ($("#RoomId").val() === '') {
//    //    toastr.error('Please select Room!', 'Error!');
//    //    hideLoader();
//    //    return;
//    //}

//    let urlPath = 'https://localhost/ApiGateway/section/save';
//    if (id != '') {
//        urlPath = 'https://localhost/ApiGateway/section/update?id=' + id;
//    }
//    $.ajax({
//        type: 'POST',
//        url: urlPath,
//        data: formData,
//        headers: {
//            'School': jsTokenObject.schoolName,
//            'Authorization': jsTokenObject.token,
//            'Content-Type': 'application/json; charset=utf-8'
//        }
//        ,
//        success: function (result) {
//            //Display message
//            displayAlert(result);
//            //check response status
//            if (result.status === 'Success') {
//                //Close model
//                $('#modal-default').modal('toggle');

//                $('#indexDataTable').DataTable().destroy();
//                $('#indexDataTable tbody').empty();

//                // location.reload();
//                let libId = $('#LibraryId').val();
//                let roomId = $('#RoomId').val();


//                useCache = true;
//                let indexData = GetDataFromLocalStorage(`${scopeTokenData.id}_${libId}_${roomId}_Section_SelectList`);

//                printLog(indexData.value, "result");
//                if (indexData === null) {
//                    SetDataToLocalStorage(`${scopeTokenData.id}_${$('#LibraryId').val()}_${$('#RoomId').val()}_Section_SelectList`,
//                        newValue,
//                        20);

//                } else {
//                    addRowToIndex(indexData, result, result.Result.id);

//                }



//                useCache = true;
//                $("#SelectedRoomsOnIndex").trigger("change");
//                //Reset Form
//                $('#modelForm').each(function () {
//                    this.reset();
//                    $('#Id').val("");
//                    $('#LibraryId').val("");
//                    $('#RoomId').val("");
//                });
//                resetButtonState();
//            }

//            //hide loader on success or fail status
//            hideLoader();

//        },
//        error: function (req, status, error) {
//            console.log(req);
//            console.log(status);
//            console.log(error);
//            ////display error message
//            toastr.warning("Opps! Something went wrong!", "Error");
//            hideLoader();

//        }
//    });
//});

////Adding or upading record to cache
//let addRowToIndex = function FillIndex(cacheData, newValue, id) {


//    let isFound = false;
//    if (cacheData != null) {
//        $.each(cacheData.value,
//            function (key, val) {
//                if (val.id === id) {
//                    val.name = newValue.Result.name;
//                    val.isEditable = true;
//                    isFound = true;
//                    printLog(value, "found data");
//                }
//            });

//        if (!isFound) {
//            printLog(cacheData.value, "not found");
//            newValue.isEditable = true;
//            cacheData?.value.push(newValue);
//        }

//        SetDataToLocalStorage(`${scopeTokenData.id}_${newValue.Result.libraryId}_${newValue.Result.roomId}_Section_SelectList`,
//            cacheData.value,
//            20);
//    } else {
//        SetDataToLocalStorage(`${scopeTokenData.id}_${newValue.Result.libraryId}_${newValue.Result.roomId}_Section_SelectList`,
//            newValue,
//            20);

//    }
//}


////On page load, it will populate the records when room element change on index page
//let FetchDataForTable = function FetchDataForTable(libraryId, roomId) {


//    let indexData = GetDataFromLocalStorage(`${scopeTokenData.id}_${libraryId}_${roomId}_Section_SelectList`);

//    if (indexData != null) {
//        fillDataInIndex(indexData.value);
//        return;
//    }

//    $.ajax({
//        url: "https://localhost/ApiGateway/section/getAll?libraryId=" + libraryId + "&roomId=" + roomId,
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

//            if (data.status === 'Success' && data.recordCount > 0) {
//                SetDataToLocalStorage(`${scopeTokenData.id}_${libraryId}_${roomId}_Section_SelectList`, data.Result, 20);
//                fillDataInIndex(data.Result);
//            } else if (data.recordCount === 0) {
//                $('#indexDataTable').DataTable().destroy();
//                $('#indexDataTable tbody').empty();
//                toastr.warning('Oops! no record found!', "Warning!");
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
//        },
//    });

//}
////when library records needed it get called and fill the records in library select list
//let InsertDataToLibrarySelectListElement = function insertDataToLibrarySelectListElement(selectElement) {

//    $('#Rooms').prop('disabled', true);
//    //Finding data from browser cache
//    let indexData = GetDataFromLocalStorage(`${scopeTokenData.id}_Library_SelectList`);

//    if (indexData != null) {
//        fillDataInLibrarySelectList(indexData.value, selectElement);
//        selectElement.focus();
//        return;
//    }

//    //if data is not available in cache, api call get hit and fetch data from server.
//    $.ajax({
//        url: "https://localhost/ApiGateway/library/getAll",
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

//            if (data.status === "Success") {
//                //data get stored in cache for library select list,
//                // we are storing data.Result in cache for each object
//                if (data.recordCount > 0) {
//                    SetDataToLocalStorage(`${scopeTokenData.id}_Library_SelectList`, data.Result, 20);
//                    fillDataInLibrarySelectList(data.Result, selectElement);
//                    selectElement.focus();
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

////On page load, it will populate the rooms records
//let FetchDataFromCacheApiForRoomElementOnIndexPage = function fetchDataFromCacheApiForRoomElementOnIndexPage(selectElement, libraryId) {

//    //Get data from browser cache
//    let indexData = GetDataFromLocalStorage(`${scopeTokenData.id}_${libraryId}_Room_SelectList`);

//    if (indexData != null) {
//        selectElement.prop('disabled', false);
//        fillDataInRoomSelectList(indexData.value, selectElement);

//        //if all ok then break the process
//        return;
//    }

//    //if cache is empty then fetch from api
//    $.ajax({
//        url: "https://localhost/ApiGateway/room/getAll?libraryId=" + libraryId,
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
//            if (data.status === "Success" && data.recordCount > 0) {
//                selectElement.prop('disabled', false);

//                //Store data to cache
//                SetDataToLocalStorage(`${scopeTokenData.id}_${libraryId}_Room_SelectList`, data.Result, 20);

//                //fill the data in room select list                
//                fillDataInRoomSelectList(data.Result, selectElement);
//            } else if (data.recordCount === 0) {
//                toastr.warning('Oops! No rooms defined for given library!', "Wawrning!");
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
//            hideLoader();
//            $(window).ready(hideLoader);
//            setTimeout(hideLoader, 20 * 1000);
//        }
//    });

//}

////when no record found, set default behaviour of index page and room element for index page
//let setDefaultState = function SetDefaultState(roomElement) {

//    AddDefaultRecordToRoomElement(roomElement);

//    $('#SelectedRoomsOnIndex').prop('disabled', true);
//    $('#indexDataTable').DataTable().destroy();
//    $('#indexDataTable tbody').empty();

//}

////add default behavior of room select list element
//AddDefaultRecordToRoomElement = function addDefaultRecordToRoomElement(roomElement) {
//    //clear the previously filled data from room element
//    roomElement.empty();

//    //add default record
//    roomElement
//        .append($("<option></option>")
//            .attr("value", -1)
//            .text("Select Room"));

//}
////When library name changed on index page
//$('#SelectedLibrariesOnIndex').on('change',
//    function (e) {
//        printLog(this.value);
//        //on change of library select list element, populate the data in room select list element
//        //on index page
//        setDefaultState($("#SelectedRoomsOnIndex"));

//        //return if this.value is empty or -1
//        if (this.value === '' || this.value === '-1') {
//            return;
//        }

//        //proceed if value exists
//        AddDefaultRecordToRoomElement($("#SelectedRoomsOnIndex"));
//        //use cache to fill data in room select element on index page
//        useCache = true;
//        FetchDataFromCacheApiForRoomElementOnIndexPage($("#SelectedRoomsOnIndex"), this.value);

//    });


//CleanIndexPage = function cleanIndexPage() {
//    $('#indexDataTable').DataTable().destroy();
//    $('#indexDataTable tbody').empty();

//}
////When room name changed on index page
//$('#SelectedRoomsOnIndex').on('change', function (e) {

//    if (this.value === '' || this.value === '-1' || $('#SelectedLibrariesOnIndex').val() === '' || $('#SelectedLibrariesOnIndex').val() === '-1') {
//        CleanIndexPage();
//        return;
//    }

//    useCache = true;
//    FetchDataForTable($('#SelectedLibrariesOnIndex').val(), this.value);
//});

////When library name changed on model
//$('#Libraries').on('change', function (e) {
//    printLog(this.value, "change");
//    //Get the id of selected library element
//    $('#LibraryId').val(this.value);
//    AddDefaultRecordToRoomElement($('Rooms'));

//    if (this.value === '' || this.value === '-1') {
//        //if value is null then return
//        $('#Rooms').prop('disabled', true);
//        return;
//    }

//    //try to get from cache and fill the room select list
//    useCache = true;
//    FetchDataFromCacheApiForRoomElementOnIndexPage($("#Rooms"), this.value);

//});
////$('#Libraries').on('click', function (e) {
////    $('#LibraryId').val(this.value);
////    FetchDataFromCacheApiForRoomElementOnIndexPage($("#Rooms"),this.value);

////});
////When room data get changed on model
//$('#Rooms').on('change', function (e) {
//    $('#RoomId').val(this.value);
//});


////On new record click populate library data for model
//$("#newRecord").click(function () {

//    InsertDataToLibrarySelectListElement($("#Libraries"));
//    changeButtonCRUDState(true);
//});


