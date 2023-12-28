var categorySelect = document.getElementById("categorySelect")
var contentSelect = document.getElementById("contentSelect")
var modalContent = document.getElementById("modalContent")
var selectedFiles = document.getElementById("selectedFiles")
var imagesInput = document.getElementById("imagesInput")
var form = document.getElementById("form")
var snapCardDiv = document.getElementById("snapCard")
var dataType = document.getElementById("dataType")

function GetContentList() {
    var liste;
    contentSelect.options.length = 0;
    contentSelect.options[contentSelect.options.length] = new Option("All", 0);
    if (categorySelect.value == 0) {
        document.getElementById("contentSelectDiv").style.display = 'none';
    } else {
        let xhrContent = new XMLHttpRequest
        xhrContent.open("GET", "/Links/GetContentList?catId=" + categorySelect.value)
        xhrContent.send()
        xhrContent.responseType = "Json"
        xhrContent.onload = () => {
            if (xhrContent.readyState == 4 && xhrContent.status == 200) {
                liste = JSON.parse(xhrContent.responseText)
                for (var obj of liste) {
                    contentSelect.options[contentSelect.options.length] = new Option(obj.Name, obj.Id);
                };
                document.getElementById("contentSelectDiv").style.display = 'block';
                contentSelect.value = contentSelected
            } else {
                alert("Error: ")
            }
        }
    }
}

function loadFiles() {
    var xhrFiles = new XMLHttpRequest();
    xhrFiles.open("GET", "/Home/GetImagesList")
    xhrFiles.send()
    xhrFiles.onload = () => {
        if (xhrFiles.readyState == 4 && xhrFiles.status == 200) {
            modalContent.innerHTML = xhrFiles.response;
            preSelectedFiles()
        } else {
            alert("Error: ")
        }
    }
}

function preSelectedFiles() {
    if (imagesInput.value !== "") {
        let listOfImages = imagesInput.value.split(',')
        listOfImages.forEach(ssChaine => {
            document.getElementById(ssChaine).style.border = "solid"
        })
        selectedFiles.value = imagesInput.value + ",";
    }
}

function selectFile(file) {
    if (dataType.value == "site") {
        var selectedFilesValue = selectedFiles.value;
        if (selectedFilesValue !== "") {
            document.getElementById(selectedFilesValue.replace(",", "")).style.border = "none"
        }      
        document.getElementById(file).style.border = "solid rgb(171, 73, 209)"
        selectedFiles.value = file;

    } else {
        var selectedFilesValue = selectedFiles.value;
        if (selectedFilesValue.includes(file)) {
            selectedFilesValue = selectedFilesValue.replace(file + ",", "")
            document.getElementById(file).style.border = "none"
            selectedFiles.value = selectedFilesValue;

        } else {
            selectedFilesValue = selectedFilesValue + file + ","
            document.getElementById(file).style.border = "solid rgb(171, 73, 209)"
            selectedFiles.value = selectedFilesValue;
        }
    }
    
}

function cleanFiles() {
    selectedFiles.value = "";
}

function saveFiles() {
    var selection = selectedFiles.value;
    if (selection.endsWith(",")) {
        selection = selection.slice(0, selection.lastIndexOf(","));
    }
    document.getElementById("imagesInput").value = selection;
}
function saveFilesDb(file) {
    if (dataType.value == "site") {
        var selection = file;
    } else {
        var selection = selectedFiles.value + file;
    }
    if (selection.endsWith(",")) {
        selection = selection.slice(0, selection.lastIndexOf(","));
    }
    document.getElementById("imagesInput").value = selection;
    $("#fileModal").modal("hide");
}

function showSnap() {
    var formData = new FormData(form);
    var xhrContent = new XMLHttpRequest();
    xhrContent.open("POST", "/Links/GetSnapCard", true)
    xhrContent.send(formData)
    xhrContent.onload = () => {
        if (xhrContent.readyState == 4 && xhrContent.status == 200) {
            snapCardDiv.innerHTML = xhrContent.response;
            snapCardDiv.style.display = 'block';
        } else {
            alert("Error: ")
        }
    }
}

function AddNewFiles() {
    var formData = new FormData(document.getElementById("addFilesForm"))
    var xhrAddFile = new XMLHttpRequest();
    xhrAddFile.open("POST", "/Links/AddPictureFiles", true)
    xhrAddFile.setRequestHeader("enctype", "multipart/form-data")
    xhrAddFile.send(formData)
    xhrAddFile.onload = () => {
        if (xhrAddFile.readyState == 4 && xhrAddFile.status == 200) {
            alert(xhrAddFile.response)
            loadFiles()
        } else {
            alert("Error: ")
        }
    }
}