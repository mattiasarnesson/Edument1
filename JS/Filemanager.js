//Remember the current location on navigation
var historyStack = [];

//Undeclared defaults
var defaults;


function initFileManager(defaults) {


    //Set incomming defaults
    this.defaults = defaults;

    //Send the first request to the backend about the current base location
    var JSONDATA =  { 'path': '' }
    serverRequest("Default.aspx/" + defaults.requestBase, JSONDATA, renderTree, true, "POST");

    //Set some starters
    setStarters();
}

function renderTree(data) {

    renderLargeView(data);

    //Could render side view here, for preview of the file and size etc.
}

function setStarters() {

    historyStack.push(document.getElementById("current_navigation").innerHTML);

    //Event listeners
    registerEvent(document.getElementById("back_button"), "click", backAction);
    registerEvent(document.getElementById("add_button"), "click", addAction);
    registerEvent(document.getElementById("remove_button"), "click", removeAction);
}

function removeAction() {
    //Get all the classes for the remove elements

    var elementsToRemove = document.getElementsByClassName("selection");

    //Prompt
    var r = confirm(defaults.removeTextPart1 + " " + elementsToRemove.length + " " + defaults.removeTextPart2);
    if (r == true) {

        var paths = [];
        for (var i = 0; i < elementsToRemove.length; i++) {
            var singleElement = elementsToRemove[i];
            var path = singleElement.getAttribute("path");
            paths.push(path);

        }

        //Send the request to the server
        var JSONDATA = { 'paths': paths }
        serverRequest("Default.aspx/" + defaults.requestRemove, JSONDATA, removeFileCallback, true, "POST"); 
    }
}

function removeFileCallback(data) {

    if (data.Code == defaults.messageDeliveryOk) {
        clearViews();
        var JSONDATA = { 'path': document.getElementById("current_navigation").innerHTML }
        serverRequest("Default.aspx/" + defaults.requestBase, JSONDATA, renderTree, true, "POST");
    }
    else {
        //This will be the failed status
        alert(data.Message);
    }
}

function addAction() {
    clearAdd();
    renderAdd();
}


//Just some standard dom manipulation to render the view of the modal for the add file
function renderAdd() {
    //Render the header
    document.getElementById("modal-title").innerHTML = "Skapa fil/mapp"

    //Render the body
    var root = document.getElementById("modal-body");

    var titleInput = document.createElement("input");
    titleInput.classList.add("form-control");
    titleInput.type = "text";
    titleInput.placeholder = "Titel";
    titleInput.id = "addTitle"
    root.appendChild(titleInput)

    var selectInput = document.createElement("select");
    selectInput.id = "addType";
    selectInput.classList.add("form-control");

    //TXT
    var optionTxt = document.createElement("option");
    optionTxt.text = "TXT";
    optionTxt.value = "TXT";
    selectInput.options.add(optionTxt);

    //DIR
    var optionDir = document.createElement("option");
    optionDir.text = "DIR";
    optionDir.value = "DIR";
    selectInput.options.add(optionDir);

    root.appendChild(selectInput)

    //Render the footer button
    var footer = document.getElementById("modal-footer")

    var save = document.createElement("button");
    save.type = "button";
    save.id = "create_file_button";
    addMultipleClasses("btn,btn-primary", save);

    save.innerHTML = "SKAPA";
    footer.appendChild(save);

    registerEvent(document.getElementById("create_file_button"), "click", uploadFile);
}

function uploadFile() {

    //Create a call to the server to upload a file
    var JSONDATA = { 'path': document.getElementById("current_navigation").innerHTML, 'title': document.getElementById("addTitle").value, 'type': addType.value }
    serverRequest("Default.aspx/" + defaults.requestAdd, JSONDATA, createFileCallback, true, "POST");
}

function clearAdd() {

    //Clear the dom
    document.getElementById("modal-body").innerHTML = ""


    //Check for not empty
    if (document.getElementById("create_file_button") != null) {
        document.getElementById("create_file_button").parentNode.removeChild(document.getElementById("create_file_button"));
        unregisterEvent("create_file_button", "click", uploadFile);
    }
 
}

//This is a callback function
function createFileCallback(data) {

    //Hide the modal
    $('#exampleModal').modal('hide')

    if (data.Code == defaults.messageDeliveryOk) {

        //Create a call to the server to upload a file
        clearViews();
        var JSONDATA = { 'path': document.getElementById("current_navigation").innerHTML }
        serverRequest("Default.aspx/" + defaults.requestBase, JSONDATA, renderTree, true, "POST");
    }
    else {
        //This will be the failed status
        alert(data.Message);
    }

}

function backAction() {

    //Check for stack lenght
    if (historyStack.length == 1) {
        alert(defaults.backError);
        return;
    }

    //Clear the views
    clearViews();

    if (historyStack.length == 0) {
        //Send another request for the new items
        var JSONDATA = { 'path': '' }
        serverRequest("Default.aspx/" + defaults.requestBase, JSONDATA, renderTree, true, "POST");
    }
    else {
        //Pop the stack
        var item = historyStack.pop();

        if (item == document.getElementById("current_navigation").innerHTML) {

            //Send another request for the new items

            var additionalItem = historyStack.pop();

            var JSONDATA = { 'path': additionalItem }
            serverRequest("Default.aspx/" + defaults.requestBase, JSONDATA, renderTree, true, "POST");

            //Refresh the current dirr
            document.getElementById("current_navigation").innerHTML = additionalItem;
        }
        else {
            var JSONDATA = { 'path': item }
            serverRequest("Default.aspx/" + defaults.requestBase, JSONDATA, renderTree, true, "POST");

            //Refresh the current dirr
            document.getElementById("current_navigation").innerHTML = item;
        }
    }


}


function renderLargeView(data) {

    var root = document.getElementById("center_view_container");

    for (var i = 0; i < data.length; i++) {

        //an card div
        var cardDIV = document.createElement("div");
        addMultipleClasses("col-4,cursor-pointer,custom-border", cardDIV)
        root.appendChild(cardDIV);

        //Register some attributes for the card
        registerAttributes(data[i], cardDIV);

        //The name to the the top of the image
        var nameRender = document.createElement("h2")
        nameRender.innerHTML = cardDIV.getAttribute("name");
        cardDIV.appendChild(nameRender);

        //The icon
        var iconRender = document.createElement("i");
        addMultipleClasses("fa,fa-3x," + cardDIV.getAttribute("img"), iconRender);
        cardDIV.appendChild(iconRender);

        //Action for left double clicking
        registerEvent(cardDIV, "dblclick", openFolderOrFile);

        //Action for left clicking
        registerEvent(cardDIV, "click", markFile);

    }
}

function markFile(domElement) {

    //Loop all dom elements
    var arrayOfCards = document.getElementsByClassName("custom-border");
    for (var i = 0; i < arrayOfCards.length; i++) {
        removeMultipleClasses("border,border-primary,selection", arrayOfCards[i])
    }

    addMultipleClasses("border,border-primary,selection", domElement);
}

function registerAttributes(data, dom) {

    //Put all the values into some variables
    var name = data.Name;
    var path = data.Path;
    var extension = data.Extension;
    var img = data.Img;
    var size = data.Size;
    var dir = data.Dir;


    //Set some attributes on the dom, so that we can work with it
    dom.setAttribute("name", name);
    dom.setAttribute("path", path);
    dom.setAttribute("extension", extension);
    dom.setAttribute("img", img);
    dom.setAttribute("size", size);
    dom.setAttribute("dir", dir);
}

//Register som clicks and other stuff. 
function registerEvent(domElement, type, callbackFunction) {


    domElement.addEventListener(type, function () {

        //if callback
        if (callbackFunction != null && typeof callbackFunction != 'undefined') {

            callbackFunction(this);

        }
    })
}

//UnRegister som clicks and other stuff. 
function unregisterEvent(domElementID, type, method) {

    if (document.getElementById(domElementID) != null) {
        document.getElementById(domElementID).removeEventListener(type, method); 
    }

}

//Function that clears the views. For example when navigation occurs
function clearViews() {
    document.getElementById("center_view_container").innerHTML = "";
}

function openFolderOrFile(domElement) {
    //This is a folder. 
    if (domElement.getAttribute("dir") == "true") {
        clearViews();

        //Send another request for the new items
        var JSONDATA = { 'path': domElement.getAttribute("path") }
        serverRequest("Default.aspx/" + defaults.requestBase, JSONDATA, renderTree, true, "POST");

        //Refresh the current dirr
        document.getElementById("current_navigation").innerHTML = domElement.getAttribute("path");

        //Push current location to stack
        historyStack.push(domElement.getAttribute("path"));
    }
    //This is an item. Lets continue checking for the extension to see if we can open it
    else {

        //This will open TXT files etc,
    }
}

//A function that adds multiple classes instead of writing same row many times
function addMultipleClasses(classesString, domElement) {
    var splittedClasses = classesString.split(',');

    for (var i = 0; i < splittedClasses.length; i++) {
        domElement.classList.add(splittedClasses[i]);
    }

    return domElement;
}

//A function that adds removes classes instead of writing same row many times
function removeMultipleClasses(classesString, domElement) {
    var splittedClasses = classesString.split(',');

    for (var i = 0; i < splittedClasses.length; i++) {
        domElement.classList.remove(splittedClasses[i]);
    }

    return domElement;
}
