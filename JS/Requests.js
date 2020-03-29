//Url to the backend
//Data for post
//Callback to another script
//Async true/false
//Get/Post
function serverRequest(url, jsonData, callbackFunction, asyncStatus, ajaxType) {

    $.ajax({
        type: ajaxType,
        url: url,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonData),
        dataType: "json",
        success: function (result) {
            if (callbackFunction != null && typeof callbackFunction != 'undefined') {

                //Run a callback function
                callbackFunction(parseToJson(result.d));

            }

        },
        error: function (result) {

            //Crash :(
            alert('error occured, check console');
            alert(result.responseText);
            console.log(result.responseText)
        },
        async: asyncStatus
    });
}

//Parse the object to json so i can work easily with it
function parseToJson(data) {
    return JSON.parse(data)
}