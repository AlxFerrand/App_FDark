function PageBack() {
    window.history.back();
}

function ShowPicture(linkId) {
    var picturesModal = document.getElementById("PicturesModal")
    let xhrImages = new XMLHttpRequest
    xhrImages.open("POST", "/Home/GetPicturesModal?linkId=" + linkId)
    xhrImages.send()
    xhrImages.onload = () => {
        if (xhrImages.readyState == 4 && xhrImages.status == 200) {
            if (xhrImages.response !== null) {
                picturesModal.innerHTML = xhrImages.response
                $("#imageModal").modal("show");
            } else {
                alert("Resource introuvable")
            }
        } else {
            alert("Error: " + xhrImages.readyState +" : "+ xhrImages.status)
        }
    }
}

var index = 0
function RightImage() {
    var mainImage = document.getElementById("mainImage")
    if (index < length) {
        mainImage.src = "/img/" + images[index + 1]
    }
}

function LeftImage() {
    var mainImage = document.getElementById("mainImage")
    if (index > 0) {
        mainImage.src = "/img/" + images[index - 1]
    }

}
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