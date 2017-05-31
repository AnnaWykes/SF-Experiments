﻿

/* This function calls the ActorBackendController's HTTP GET method to get the number of actors in the ActorBackendService */
function getActorCount() {
    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        
        if (http.readyState === 4) {
            end = new Date().getTime();
       
            if (http.status < 400) {
            
                returnData = JSON.parse(http.responseText);
                if (returnData) {
                    countDisplay.innerHTML = returnData.count;
                    updateFooter(http, (end - start));
                }
            } else {
             
                updateFooter(http, (end - start));
            }
        }
    };
    
    start = new Date().getTime();
    alert();
    http.open("GET", "/api/ActorBackendService/?c=" + start);
    http.send();
}

/* This function calls the ActorBackendController's HTTP POST method to create a new actor in the ActorBackendService */
function newActor() {
    var http = new XMLHttpRequest();

    http.onreadystatechange = function () {
        if (http.readyState === 4) {
            end = new Date().getTime();
            if (http.status < 400) {
                returnData = JSON.parse(http.responseText);
                if (returnData) {
                    updateFooter(http, (end - start));
                }
            } else {
                updateFooter(http, (end - start));
            }
        }
    };
    start = new Date().getTime();
    http.open("POST", "/api/ActorBackendService/?c=" + start);
    http.send();
}

/*This function hides the footer*/
function toggleFooter(option) {
    var footer = document.getElementById('footer');
    switch (option) {
        case 0:
            footer.classList = 'footer hidden';
            break;
        case 1:
            footer.classList = 'footer';
            break;
    }
}

/*This function puts HTTP result in the footer */
function updateFooter(http, timeTaken) {
    toggleFooter(1);
    if (http.status < 299) {
        statusPanel.className = 'panel panel-success';
        statusPanelHeading.innerHTML = http.status + ' ' + http.statusText;
        statusPanelBody.innerHTML = 'Result returned in ' + timeTaken.toString() + ' ms from ' + http.responseURL;
    }
    else {
        statusPanel.className = 'panel panel-danger';
        statusPanelHeading.innerHTML = http.status + ' ' + http.statusText;
        statusPanelBody.innerHTML = http.responseText;
    }
}

function handleEnter() {
    var pathName = document.location.pathname.substring(6);
    switch (pathName) {
        case "Stateless":
            onkeyup = function (e) {
                if (e.keyCode === 13) {
                    getStatelessBackendCount();
                    return false;
                }
            };
            break;
        case "Stateful":
            keyInput.onkeyup = function (e) {
                if (e.keyCode === 13) {
                    addStatefulBackendServiceKeyValuePair();
                    return false;
                }
            };
            valueInput.onkeyup = function (e) {
                if (e.keyCode === 13) {
                    addStatefulBackendServiceKeyValuePair();
                    return false;
                }
            };
            break;
    }
}


