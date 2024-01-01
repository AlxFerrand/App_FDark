
function tagCheck(tag) {
    var inputTag = document.getElementById("inputTag")
    inputTag.value = tag;
}


function PreConfirmUser(id, name,charactere) {
    document.getElementById("confirmeUserName").innerText = document.getElementById("confirmeUserName").innerText + name
    document.getElementById("confirmeUserCharactere").innerText = document.getElementById("confirmeUserCharactere").innerText + charactere
    document.getElementById("confirmeUserId").value = id
    $("#confirmModal").modal("show");
}
function ConfirmUser() {
    var id = document.getElementById("confirmeUserId").value
    let xhrConfirm = new XMLHttpRequest
    xhrConfirm.open("POST", "/Account/ConfirmeUser?id=" + id)
    xhrConfirm.send()
    xhrConfirm.onload = () => {
        if (xhrConfirm.readyState == 4 && xhrConfirm.status == 200) {
            if (xhrConfirm.response === "ok") {
                window.location.href = '/Account/Index'
            } else {
                alert("Error : " + xhrConfirm.response)
            }
        } else {
            alert("Error: ")
        }
    }
}

function PreDelete(id, name, charactere) {
    document.getElementById("deleteUserName").innerText = document.getElementById("deleteUserName").innerText + name
    document.getElementById("deleteUserCharactere").innerText = document.getElementById("deleteUserCharactere").innerText + charactere
    document.getElementById("deleteUserId").value = id
    $("#deleteModal").modal("show");
}

function DeleteUser() {
    var id = document.getElementById("deleteUserId").value
    let xhrDelete = new XMLHttpRequest
    xhrDelete.open("POST", "/Account/Delete?id="+id)
    xhrDelete.send()
    xhrDelete.onload = () => {
        if (xhrDelete.readyState == 4 && xhrDelete.status == 200) {
            if (xhrDelete.response === "ok") {
                window.location.href = '/Account/Index'
            } else {
                alert("Error : " + xhrDelete.response)
            }
        } else {
            alert("Error: ")
        }
    }
}

function PreEditPass(id, name, charactere) {
    document.getElementById("PassUserName").innerText = document.getElementById("PassUserName").innerText + name
    document.getElementById("PassUserCharactere").innerText = document.getElementById("PassUserCharactere").innerText + charactere
    document.getElementById("PassUserId").value = id
    $("#PassModal").modal("show");
}