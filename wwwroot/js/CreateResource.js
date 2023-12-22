var categorySelect = document.getElementById("categorySelect")
var dataTypeSelect = document.getElementById("dataType")
var snapButton = document.getElementById("snapButton")

function ShowForm() {
    if (document.getElementById("dataType").value == "video") {
        document.getElementById("label").style.display = 'block';
        document.getElementById("picture").style.display = 'none';
        document.getElementById("url").style.display = 'block';
        document.getElementById("description").style.display = 'block';
        document.getElementById("extension").style.display = 'block';
        snapButton.style.display = 'block';

    }
    if (document.getElementById("dataType").value == "site") {
        document.getElementById("label").style.display = 'block';
        document.getElementById("picture").style.display = 'block';
        document.getElementById("url").style.display = 'block';
        document.getElementById("description").style.display = 'block';
        document.getElementById("extension").style.display = 'block';
        snapButton.style.display = 'block';

    }
    if (document.getElementById("dataType").value == "img") {
        document.getElementById("label").style.display = 'block';
        document.getElementById("picture").style.display = 'block';
        document.getElementById("url").style.display = 'none';
        document.getElementById("description").style.display = 'block';
        document.getElementById("extension").style.display = 'block';
        snapButton.style.display = 'block';

    }
    if (document.getElementById("dataType").value == "text") {
        document.getElementById("label").style.display = 'block';
        document.getElementById("picture").style.display = 'none';
        document.getElementById("url").style.display = 'none';
        document.getElementById("description").style.display = 'block';
        document.getElementById("extension").style.display = 'block';
        snapButton.style.display = 'block';

    }
    if (document.getElementById("dataType").value == "") {
        document.getElementById("label").style.display = 'none';
        document.getElementById("picture").style.display = 'none';
        document.getElementById("url").style.display = 'none';
        document.getElementById("description").style.display = 'none';
        document.getElementById("extension").style.display = 'none';
        document.getElementById("contentSelectDiv").style.display = 'none';
        snapButton.style.display = 'none';
    }
}

if (categorySelect.value != "") {
    document.getElementById("contentSelectDiv").style.display = 'block';
    GetContentList()
} else {
    document.getElementById("contentSelectDiv").style.display = 'none';
}

if (dataTypeSelect.value != "Null") {
    ShowForm()
}
