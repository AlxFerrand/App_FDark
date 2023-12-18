var categorySelect = document.getElementById("categorySelect")
var contentSelect = document.getElementById("contentSelect")
var dataTypeSelect = document.getElementById("dataType")
var snapCardDiv = document.getElementById("snapCard")
var form = document.getElementById("formVideo")

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

if (categorySelect.value != "") {
    GetContentList()
}

function showSnap() {
    var formData = new FormData(form);
    var xhrContent = new XMLHttpRequest();
    xhrContent.open("POST", "/Links/GetSnapCard", true)
    xhrContent.setRequestHeader("enctype", "multipart/form-data")
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
