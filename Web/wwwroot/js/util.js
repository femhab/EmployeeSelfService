function ProcessField(valueInput) {
    if (valueInput == 2) {
        ShowSecondLevelApproval();
    }
    if (valueInput == 3) {
        ShowThirdLevelApproval();
    }
    if (valueInput == 4) {
        ShowFourthLevelApproval();
    }
    if (valueInput == 5) {
        ShowFifthLevelApproval();
    }
    if (valueInput == 6) {
        ShowSixthLevelApproval();
    }
    if (valueInput == 7) {
        ShowSeventhLevelApproval();
    }
    if (valueInput == 8) {
        ShowEightLevelApproval();
    }
    if (valueInput == 9) {
        ShowNinethLevelApproval();
    }
    if (valueInput == 10) {
        ShowTenthLevelApproval();
    }
    if (valueInput > 10) {
        ShowSecondLevelApproval();
    }
}

function ShowSecondLevelApproval() {
    document.getElementById('table2').removeAttribute("hidden");
    document.getElementById('table3').setAttribute("hidden", true);
    document.getElementById('table4').setAttribute("hidden", true);
    document.getElementById('table5').setAttribute("hidden", true);
    document.getElementById('table6').setAttribute("hidden", true);
    document.getElementById('table7').setAttribute("hidden", true);
    document.getElementById('table8').setAttribute("hidden", true);
    document.getElementById('table9').setAttribute("hidden", true);
    document.getElementById('table10').setAttribute("hidden", true);
}
function ShowThirdLevelApproval() {
    document.getElementById('table2').removeAttribute("hidden");
    document.getElementById('table3').removeAttribute("hidden");
    document.getElementById('table4').setAttribute("hidden", true);
    document.getElementById('table5').setAttribute("hidden", true);
    document.getElementById('table6').setAttribute("hidden", true);
    document.getElementById('table7').setAttribute("hidden", true);
    document.getElementById('table8').setAttribute("hidden", true);
    document.getElementById('table9').setAttribute("hidden", true);
    document.getElementById('table10').setAttribute("hidden", true);
}
function ShowFourthLevelApproval() {
    document.getElementById('table2').removeAttribute("hidden");
    document.getElementById('table3').removeAttribute("hidden");
    document.getElementById('table4').removeAttribute("hidden");
    document.getElementById('table5').setAttribute("hidden", true);
    document.getElementById('table6').setAttribute("hidden", true);
    document.getElementById('table7').setAttribute("hidden", true);
    document.getElementById('table8').setAttribute("hidden", true);
    document.getElementById('table9').setAttribute("hidden", true);
    document.getElementById('table10').setAttribute("hidden", true);
}
function ShowFifthLevelApproval() {
    document.getElementById('table2').removeAttribute("hidden");
    document.getElementById('table3').removeAttribute("hidden");
    document.getElementById('table4').removeAttribute("hidden");
    document.getElementById('table5').removeAttribute("hidden");
    document.getElementById('table6').setAttribute("hidden", true);
    document.getElementById('table7').setAttribute("hidden", true);
    document.getElementById('table8').setAttribute("hidden", true);
    document.getElementById('table9').setAttribute("hidden", true);
    document.getElementById('table10').setAttribute("hidden", true);
}
function ShowSixthLevelApproval() {
    document.getElementById('table2').removeAttribute("hidden");
    document.getElementById('table3').removeAttribute("hidden");
    document.getElementById('table4').removeAttribute("hidden");
    document.getElementById('table5').removeAttribute("hidden");
    document.getElementById('table6').removeAttribute("hidden");
    document.getElementById('table7').setAttribute("hidden", true);
    document.getElementById('table8').setAttribute("hidden", true);
    document.getElementById('table9').setAttribute("hidden", true);
    document.getElementById('table10').setAttribute("hidden", true);
}
function ShowSeventhLevelApproval() {
    document.getElementById('table2').removeAttribute("hidden");
    document.getElementById('table3').removeAttribute("hidden");
    document.getElementById('table4').removeAttribute("hidden");
    document.getElementById('table5').removeAttribute("hidden");
    document.getElementById('table6').removeAttribute("hidden");
    document.getElementById('table7').removeAttribute("hidden");
    document.getElementById('table8').setAttribute("hidden", true);
    document.getElementById('table9').setAttribute("hidden", true);
    document.getElementById('table10').setAttribute("hidden", true);
}
function ShowEightLevelApproval() {
    document.getElementById('table2').removeAttribute("hidden");
    document.getElementById('table3').removeAttribute("hidden");
    document.getElementById('table4').removeAttribute("hidden");
    document.getElementById('table5').removeAttribute("hidden");
    document.getElementById('table6').removeAttribute("hidden");
    document.getElementById('table7').removeAttribute("hidden");
    document.getElementById('table8').removeAttribute("hidden");
    document.getElementById('table9').setAttribute("hidden", true);
    document.getElementById('table10').setAttribute("hidden", true);
}
function ShowNinethLevelApproval() {
    document.getElementById('table2').removeAttribute("hidden");
    document.getElementById('table3').removeAttribute("hidden");
    document.getElementById('table4').removeAttribute("hidden");
    document.getElementById('table5').removeAttribute("hidden");
    document.getElementById('table6').removeAttribute("hidden");
    document.getElementById('table7').removeAttribute("hidden");
    document.getElementById('table8').removeAttribute("hidden");
    document.getElementById('table9').removeAttribute("hidden");
    document.getElementById('table10').setAttribute("hidden", true);
}
function ShowTenthLevelApproval() {
    document.getElementById('table2').removeAttribute("hidden");
    document.getElementById('table3').removeAttribute("hidden");
    document.getElementById('table4').removeAttribute("hidden");
    document.getElementById('table5').removeAttribute("hidden");
    document.getElementById('table6').removeAttribute("hidden");
    document.getElementById('table7').removeAttribute("hidden");
    document.getElementById('table8').removeAttribute("hidden");
    document.getElementById('table9').removeAttribute("hidden");
    document.getElementById('table10').removeAttribute("hidden");
}

function stringToDate(str) {
    var date = str.split("/"),
        m = date[1],
        d = date[0],
        y = date[2];
    return (new Date(y + "/" + m + "/" + d));
}

function dateToString(date) {
    //var date = new Date(str),
    mnth = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
    return [day, mnth, date.getFullYear()].join("/");
}

function getNextBusinessDay(date) {
    // Copy date so don't affect original
    date = new Date(+date);
    // Add days until get not Sat or Sun
    do {
        date.setDate(date.getDate() + 1);
    } while (!(date.getDay() % 6))
    return date;
}