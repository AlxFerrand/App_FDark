var categorySelect = document.getElementById("categorySelect")
var contentSelect = document.getElementById("contentSelect")
var dataTypeSelect = document.getElementById("dataType")

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
function ShowForm() {
    if (document.getElementById("dataType").value == "video") {
        document.getElementById("label").style.display = 'block';
        document.getElementById("picture").style.display = 'none';
        document.getElementById("url").style.display = 'block';
        document.getElementById("description").style.display = 'block';
        document.getElementById("extension").style.display = 'block';

    }
    if (document.getElementById("dataType").value == "site") {
        document.getElementById("label").style.display = 'block';
        document.getElementById("picture").style.display = 'block';
        document.getElementById("url").style.display = 'block';
        document.getElementById("description").style.display = 'block';
        document.getElementById("extension").style.display = 'block';

    }
    if (document.getElementById("dataType").value == "img") {
        document.getElementById("label").style.display = 'block';
        document.getElementById("picture").style.display = 'block';
        document.getElementById("url").style.display = 'none';
        document.getElementById("description").style.display = 'block';
        document.getElementById("extension").style.display = 'block';

    }
    if (document.getElementById("dataType").value == "text") {
        document.getElementById("label").style.display = 'block';
        document.getElementById("picture").style.display = 'none';
        document.getElementById("url").style.display = 'none';
        document.getElementById("description").style.display = 'block';
        document.getElementById("extension").style.display = 'block';

    }
    if (document.getElementById("dataType").value == "") {
        document.getElementById("label").style.display = 'none';
        document.getElementById("picture").style.display = 'none';
        document.getElementById("url").style.display = 'none';
        document.getElementById("description").style.display = 'none';
        document.getElementById("extension").style.display = 'none';
        document.getElementById("contentSelectDiv").style.display = 'none';
    }
}

if (dataTypeSelect.value != "Null") {
    ShowForm()
}

if (categorySelect.value != "") {
    document.getElementById("contentSelectDiv").style.display = 'block';
    GetContentList()


} else {
    document.getElementById("contentSelectDiv").style.display = 'none';
}