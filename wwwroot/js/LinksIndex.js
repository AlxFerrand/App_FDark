function refreshLinks() {
    var formData = new FormData(form);
    var xhrLinks = new XMLHttpRequest();
    xhrLinks.open("POST", "/Links/Index", true)
    xhrLinks.send(formData)
    xhrLinks.onload = () => {
        if (xhrLinks.readyState == 4 && xhrLinks.status == 200) {
            document.documentElement.innerHTML = xhrLinks.response
        } else {
            alert("Error: ")
        }
    }
}
function SortLinks(sortOrder) {
    var inputSortOrder = document.getElementById('sortOrder')
    inputSortOrder.value = sortOrder
    refreshLinks()
}