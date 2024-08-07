//jwt info
let jsTokenObject = {
    schoolName: '',
    token: '',
    id: ''
};



//cache key for get/set info in local forage
let cacheKey = '';

//boolean 
let useCache = true ;
let keyPressStatus = '';
let backroundCache = true;

//storing table related information for calling ajax and cache related operations.
let jsFormObject = {};


let cacheKeyLibrary = '';
let cacheKeyRoom = '';
let cacheKeySection = '';
let sessionId = '';
let apiModuleLibrary = '';
let apiModuleRoom = '';
let apiModuleSection = '';
let libId = '';
let roomId = '';
let sectionId = '';
let apiUrlToGetAllLibrary = '';
let apiUrlToGetAllRoom = '';
let apiUrlToGetAllSection = '';
let apiUrlToSectionGetById = '';


    
//Reading form elements
$("#modal-default").on('keydown', (event) => {
    if (event.key === 'Escape') {
        //Close model
        $('#modal-default').modal('toggle');
        resetButtonState();

    }
});
//For insert new record pupup shortcut key setup
$(window).keydown(function (event) {
    let nKey = 105; //i
    let Nkey = 73; //I
    if (keyPressStatus === "N") {
        return;
    }

    if (event.ctrlKey && (event.which === nKey || event.which === Nkey)) {
        $('#newRecord').trigger('click');
        if (keyPressStatus === "") {
            keyPressStatus = "N";
        }

        changeButtonCRUDState();
        event.preventDefault();
    }
});

//For saving new record shortcut key setup
$(window).keydown(function (event) {
    let nKey = 115; //i
    let Nkey = 83; //I

    if (event.ctrlKey && (event.which === nKey || event.which === Nkey)) {

        $('#submit-btn').trigger('click');
        event.preventDefault();
    }
    if (event.ctrlKey && event.altKey && (event.which === nKey || event.which === Nkey)) {

        $('#submit-btn').trigger('click');
        event.preventDefault();
    }
});

//Set focus on first element of the model
$('#modal-default').on('shown.bs.modal', function () {
    $('.first').focus();
})

let resetButtonState =  function () {
    $("#newRecordLi").css("display", "block");
    $("#saveLi").css("display", "none");
    $("#cancelLi").css("display", "none");
    //Reset form
    //Reseting previously pressed key
    keyPressStatus = "";
    $('#modelForm').each(function () {
        this.reset();
    });
    $('#Id').val("");

};
$(document).ready(async function () {

    //Common button event handling for models
    $("#model-close-x").click(function () {
        resetButtonState();
    });

    //On new record click
    $("#newRecord").click(function () {
         changeButtonCRUDState();
    });
    //Common button event handling for models
    $("#cancelRecord").click(function () {
         resetButtonState();
    });
});

//it will read form elements and set serialize array for that form element
let getFormElementsData =  function getFormData($form) {
    let unindexed_array = $form.serializeArray();
    let indexed_array = {};
    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });
    return  indexed_array;
};

let displayAlert = async function displayAlertMessage(object, type) {
    toastr.options = {
        "closeButton": true,
        "newestOnTop": false,
        "progressBar": true,
        //"positionClass": "toast-bottom-center",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    if (type === "Index") {
        toastr.success('Record populated : ' + object.recordCount, object.status);
        return null;
    }
    if (object.messageType == "Information" || object.messageType == "Message")
        toastr.success(object.message, object.status);

    if (object.messageType == "Error" || object.messageType == "Exception")
        toastr.error(object.message, object.status);

    if (object.messageType == "Warning" || object.messageType == "ModelState" || object.messageType == "DefaultRecordWarning") {
        if (object.messageType == "ModelState") {
            let strError = "";
            object.modelStateErrors.forEach(function (item) {
                strError += item + '<br>';
            });
            toastr.warning(strError, object.status);
            return null;
        }
        toastr.error(object.message, object.status);
    }

};

//Data table binding
let bindDataTable = async function BindItemTable(table) {
    myTable = table.DataTable({
        "deferRender": true,
        "paging": true,
        "lengthChange": false,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "sDom": 'lfrtip'
    });
    return await myTable;
}
//Hiding loader
let showLoader = function showLoader() {
    $('#loading').show();
};
let hideLoader = function hideLoader() {
    $('#loading').hide();
};

let printLog = function PrintLog(msg, title) {

    console.log(`===========${title}=============`);
    console.log(msg);
    console.log("===========<End>=============");
};

//Used for pulling data from cache or server
//if (useCache === undefined) {
//    printLog("x1");
//    let useCache = true;

//} else {
//    printLog("x2");
//    useCache = false;
//}


    //===================================Common Functionality============
//Populate index for page
let PopulateItemsTable = async function populateItemsTable() {
    jsFormObject.useCache = true;
    PopulateItemsOnIndex(jsFormObject);
}

//refresh index page from db without using  browser cache and redis cache
let RefreshIndexWithoutCache = async function refreshIndexWithoutCache() {
    await clearCache();
    jsFormObject.useCache = false;
    window.$('#indexDataTable').DataTable().destroy();
    window.$('#indexDataTable tbody').empty();

    PopulateItemsOnIndex(jsFormObject);
}

//Common for all index table
//Generic populate function for filling data in table on index page.
let PopulateItemsOnIndex = async function populateItemsOnIndex(obj) {

    let indexData = await GetDataFromLocalStorage(obj.cacheKey, obj.sessionId, obj.useCache);
    if (indexData != null) {
        await obj.fnTableFill(indexData);
        window.$('.searchLabel').focus();
        return;
    }

    window.$.ajax({
        url: `https://localhost/ApiGateway/${jsFormObject.apiModule}/getall?useCache=${backroundCache}`,
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

            //Storing data to client browser
            SetDataToLocalStorage(obj.cacheKey, obj.sessionId, data, 20);

            obj.FillIndex(data);
            window.$('.searchLabel').focus();

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
}
//end populate index function

//common submit form for all the modules
//it will get called on submit click - while click on side panel save button
$('#modelForm').submit(async function (e) {
    showLoader();
    e.preventDefault();

    let id = getFormElementsData($(this)).Id;
    let formData = JSON.stringify(getFormElementsData($(this)));
    let urlPath = `https://localhost/ApiGateway/${jsFormObject.apiModule}/save?useCache=${backroundCache}`;
    if (id != '') {
        urlPath = `https://localhost/ApiGateway/${jsFormObject.apiModule}/update?id=${id}&useCache=${backroundCache}`;
    }

    //passing object for saving or updating record to db
    let objecToProcess = {};
    objecToProcess.urlPathToSave = urlPath;
    objecToProcess.formData = formData;
    objecToProcess.cacheKey = cacheKey;
    objecToProcess.id = sessionId;
    objecToProcess.apiUrlToGetAll = `https://localhost/ApiGateway/${jsFormObject.apiModule}/getall?useCache=${backroundCache}`;
    objecToProcess.fnTableFill = FillIndex;
    await submitForm(objecToProcess);
});
let submitForm = async function SubmitForm(obj) {
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
                GetDataFromLocalStorage(obj.cacheKey, obj.id)
                    .then(function (result) {

                        if (result) {

                            addRowToIndex(result, val, data.Result.id);

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


//Load a record for editing
let loadData = async function loadDataforEditing(obj) {
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
                FillModelFormForEditingRecord(data.Result);
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

//Open for edit record
$(document).on("click", ".open-Model", async function () {
    let recordId = $(this).data('id');
    //alert(myBookId);
    showLoader();
    //Load data for edit

    changeButtonCRUDState();
    let obj = {};
    obj.apiUrlToGetById = apiUrlToGetById;
    obj.FillModelFormForEditingRecord;
    obj.id = recordId;
    loadData(obj);

});

let changeButtonCRUDState = async function (advanceForm) {
    $("#newRecordLi").css("display", "none");
    $("#cancelLi").css("display", "block");
    $("#saveLi").css("display", "block");

    //Reset Form
    if (advanceForm === undefined) {
        $('#modelForm').each(function () {
            this.reset();
        });
    }
};

//Delete record
$(document).on("click",
    ".delete-Record",
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
                    deleteData(trSequence, `https://localhost/ApiGateway/${jsFormObject.apiModule}/delete?id=${recordId}&useCache=${backroundCache}`);

                }
            });
    });


//Common crud operations
//Delete Data
let deleteData = async function DeleteData(trSequence, apiUrl) {
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
                                FillIndex(cacheData);
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


    //===== Common Functionality end =====================================
