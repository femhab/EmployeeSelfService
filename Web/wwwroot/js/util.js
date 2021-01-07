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

function ProcessLoanField(valueInput) {
    if (valueInput == 2) {
        ShowLoanSecondLevelApproval();
    }
    if (valueInput == 3) {
        ShowLoanThirdLevelApproval();
    }
    if (valueInput == 4) {
        ShowLoanFourthLevelApproval();
    }
    if (valueInput == 5) {
        ShowLoanFifthLevelApproval();
    }
    if (valueInput == 6) {
        ShowLoanSixthLevelApproval();
    }
    if (valueInput == 7) {
        ShowLoanSeventhLevelApproval();
    }
    if (valueInput == 8) {
        ShowLoanEightLevelApproval();
    }
    if (valueInput == 9) {
        ShowLoanNinethLevelApproval();
    }
    if (valueInput == 10) {
        ShowLoanTenthLevelApproval();
    }
    if (valueInput > 10) {
        ShowLoanSecondLevelApproval();
    }
}

function ProcessAppraisalField(valueInput) {
    if (valueInput == 2) {
        ShowAppraisalSecondLevelApproval();
    }
    if (valueInput == 3) {
        ShowAppraisalThirdLevelApproval();
    }
    if (valueInput == 4) {
        ShowAppraisalFourthLevelApproval();
    }
    if (valueInput == 5) {
        ShowAppraisalFifthLevelApproval();
    }
    if (valueInput == 6) {
        ShowAppraisalSixthLevelApproval();
    }
    if (valueInput == 7) {
        ShowAppraisalSeventhLevelApproval();
    }
    if (valueInput == 8) {
        ShowAppraisalEightLevelApproval();
    }
    if (valueInput == 9) {
        ShowAppraisalNinethLevelApproval();
    }
    if (valueInput == 10) {
        ShowAppraisalTenthLevelApproval();
    }
    if (valueInput > 10) {
        ShowAppraisalSecondLevelApproval();
    }
}

function ProcessTransferField(valueInput) {
    if (valueInput == 2) {
        ShowTransferSecondLevelApproval();
    }
    if (valueInput == 3) {
        ShowTransferThirdLevelApproval();
    }
    if (valueInput == 4) {
        ShowTransferFourthLevelApproval();
    }
    if (valueInput == 5) {
        ShowTransferFifthLevelApproval();
    }
    if (valueInput == 6) {
        ShowTransferSixthLevelApproval();
    }
    if (valueInput == 7) {
        ShowTransferSeventhLevelApproval();
    }
    if (valueInput == 8) {
        ShowTransferEightLevelApproval();
    }
    if (valueInput == 9) {
        ShowTransferNinethLevelApproval();
    }
    if (valueInput == 10) {
        ShowTransferTenthLevelApproval();
    }
    if (valueInput > 10) {
        ShowTransferSecondLevelApproval();
    }
}

function ProcessTrainingField(valueInput) {
    if (valueInput == 2) {
        ShowTrainingSecondLevelApproval();
    }
    if (valueInput == 3) {
        ShowTrainingThirdLevelApproval();
    }
    if (valueInput == 4) {
        ShowTrainingFourthLevelApproval();
    }
    if (valueInput == 5) {
        ShowTrainingFifthLevelApproval();
    }
    if (valueInput == 6) {
        ShowTrainingSixthLevelApproval();
    }
    if (valueInput == 7) {
        ShowTrainingSeventhLevelApproval();
    }
    if (valueInput == 8) {
        ShowTrainingEightLevelApproval();
    }
    if (valueInput == 9) {
        ShowTrainingNinethLevelApproval();
    }
    if (valueInput == 10) {
        ShowTrainingTenthLevelApproval();
    }
    if (valueInput > 10) {
        ShowTrainingSecondLevelApproval();
    }
}

function ProcessAdvanceField(valueInput) {
    if (valueInput == 2) {
        ShowAdvanceSecondLevelApproval();
    }
    if (valueInput == 3) {
        ShowAdvanceThirdLevelApproval();
    }
    if (valueInput == 4) {
        ShowAdvanceFourthLevelApproval();
    }
    if (valueInput == 5) {
        ShowAdvanceFifthLevelApproval();
    }
    if (valueInput == 6) {
        ShowAdvanceSixthLevelApproval();
    }
    if (valueInput == 7) {
        ShowAdvanceSeventhLevelApproval();
    }
    if (valueInput == 8) {
        ShowAdvanceEightLevelApproval();
    }
    if (valueInput == 9) {
        ShowAdvanceNinethLevelApproval();
    }
    if (valueInput == 10) {
        ShowAdvanceTenthLevelApproval();
    }
    if (valueInput > 10) {
        ShowAdvanceSecondLevelApproval();
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
function ShowLoanSecondLevelApproval() {
    document.getElementById('tableloan2').removeAttribute("hidden");
    document.getElementById('tableloan3').setAttribute("hidden", true);
    document.getElementById('tableloan4').setAttribute("hidden", true);
    document.getElementById('tableloan5').setAttribute("hidden", true);
    document.getElementById('tableloan6').setAttribute("hidden", true);
    document.getElementById('tableloan7').setAttribute("hidden", true);
    document.getElementById('tableloan8').setAttribute("hidden", true);
    document.getElementById('tableloan9').setAttribute("hidden", true);
    document.getElementById('tableloan10').setAttribute("hidden", true);
}
function ShowTransferSecondLevelApproval() {
    document.getElementById('tabletransfer2').removeAttribute("hidden");
    document.getElementById('tabletransfer3').setAttribute("hidden", true);
    document.getElementById('tabletransfer4').setAttribute("hidden", true);
    document.getElementById('tabletransfer5').setAttribute("hidden", true);
    document.getElementById('tabletransfer6').setAttribute("hidden", true);
    document.getElementById('tabletransfer7').setAttribute("hidden", true);
    document.getElementById('tabletransfer8').setAttribute("hidden", true);
    document.getElementById('tabletransfer9').setAttribute("hidden", true);
    document.getElementById('tabletransfer10').setAttribute("hidden", true);
}
function ShowAppraisalSecondLevelApproval() {
    document.getElementById('tableappraisal2').removeAttribute("hidden");
    document.getElementById('tableappraisal3').setAttribute("hidden", true);
    document.getElementById('tableappraisal4').setAttribute("hidden", true);
    document.getElementById('tableappraisal5').setAttribute("hidden", true);
    document.getElementById('tableappraisal6').setAttribute("hidden", true);
    document.getElementById('tableappraisal7').setAttribute("hidden", true);
    document.getElementById('tableappraisal8').setAttribute("hidden", true);
    document.getElementById('tableappraisal9').setAttribute("hidden", true);
    document.getElementById('tableappraisal10').setAttribute("hidden", true);
}
function ShowTrainingSecondLevelApproval() {
    document.getElementById('tabletraining2').removeAttribute("hidden");
    document.getElementById('tabletraining3').setAttribute("hidden", true);
    document.getElementById('tabletraining4').setAttribute("hidden", true);
    document.getElementById('tabletraining5').setAttribute("hidden", true);
    document.getElementById('tabletraining6').setAttribute("hidden", true);
    document.getElementById('tabletraining7').setAttribute("hidden", true);
    document.getElementById('tabletraining8').setAttribute("hidden", true);
    document.getElementById('tabletraining9').setAttribute("hidden", true);
    document.getElementById('tabletraining10').setAttribute("hidden", true);
}
function ShowAdvanceSecondLevelApproval() {
    document.getElementById('tableadvance2').removeAttribute("hidden");
    document.getElementById('tableadvance3').setAttribute("hidden", true);
    document.getElementById('tableadvance4').setAttribute("hidden", true);
    document.getElementById('tableadvance5').setAttribute("hidden", true);
    document.getElementById('tableadvance6').setAttribute("hidden", true);
    document.getElementById('tableadvance7').setAttribute("hidden", true);
    document.getElementById('tableadvance8').setAttribute("hidden", true);
    document.getElementById('tableadvance9').setAttribute("hidden", true);
    document.getElementById('tableadvance10').setAttribute("hidden", true);
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
function ShowLoanThirdLevelApproval() {
    document.getElementById('tableloan2').removeAttribute("hidden");
    document.getElementById('tableloan3').removeAttribute("hidden");
    document.getElementById('tableloan4').setAttribute("hidden", true);
    document.getElementById('tableloan5').setAttribute("hidden", true);
    document.getElementById('tableloan6').setAttribute("hidden", true);
    document.getElementById('tableloan7').setAttribute("hidden", true);
    document.getElementById('tableloan8').setAttribute("hidden", true);
    document.getElementById('tableloan9').setAttribute("hidden", true);
    document.getElementById('tableloan10').setAttribute("hidden", true);
}
function ShowTransferThirdLevelApproval() {
    document.getElementById('tabletransfer2').removeAttribute("hidden");
    document.getElementById('tabletransfer3').removeAttribute("hidden");
    document.getElementById('tabletransfer4').setAttribute("hidden", true);
    document.getElementById('tabletransfer5').setAttribute("hidden", true);
    document.getElementById('tabletransfer6').setAttribute("hidden", true);
    document.getElementById('tabletransfer7').setAttribute("hidden", true);
    document.getElementById('tabletransfer8').setAttribute("hidden", true);
    document.getElementById('tabletransfer9').setAttribute("hidden", true);
    document.getElementById('tabletransfer10').setAttribute("hidden", true);
}
function ShowAppraisalThirdLevelApproval() {
    document.getElementById('tableappraisal2').removeAttribute("hidden");
    document.getElementById('tableappraisal3').removeAttribute("hidden");
    document.getElementById('tableappraisal4').setAttribute("hidden", true);
    document.getElementById('tableappraisal5').setAttribute("hidden", true);
    document.getElementById('tableappraisal6').setAttribute("hidden", true);
    document.getElementById('tableappraisal7').setAttribute("hidden", true);
    document.getElementById('tableappraisal8').setAttribute("hidden", true);
    document.getElementById('tableappraisal9').setAttribute("hidden", true);
    document.getElementById('tableappraisal10').setAttribute("hidden", true);
}
function ShowTrainingThirdLevelApproval() {
    document.getElementById('tabletraining2').removeAttribute("hidden");
    document.getElementById('tabletraining3').removeAttribute("hidden");
    document.getElementById('tabletraining4').setAttribute("hidden", true);
    document.getElementById('tabletraining5').setAttribute("hidden", true);
    document.getElementById('tabletraining6').setAttribute("hidden", true);
    document.getElementById('tabletraining7').setAttribute("hidden", true);
    document.getElementById('tabletraining8').setAttribute("hidden", true);
    document.getElementById('tabletraining9').setAttribute("hidden", true);
    document.getElementById('tabletraining10').setAttribute("hidden", true);
}
function ShowAdvanceThirdLevelApproval() {
    document.getElementById('tableadvance2').removeAttribute("hidden");
    document.getElementById('tableadvance3').removeAttribute("hidden");
    document.getElementById('tableadvance4').setAttribute("hidden", true);
    document.getElementById('tableadvance5').setAttribute("hidden", true);
    document.getElementById('tableadvance6').setAttribute("hidden", true);
    document.getElementById('tableadvance7').setAttribute("hidden", true);
    document.getElementById('tableadvance8').setAttribute("hidden", true);
    document.getElementById('tableadvance9').setAttribute("hidden", true);
    document.getElementById('tableadvance10').setAttribute("hidden", true);
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
function ShowLoanFourthLevelApproval() {
    document.getElementById('tableloan2').removeAttribute("hidden");
    document.getElementById('tableloan3').removeAttribute("hidden");
    document.getElementById('tableloan4').removeAttribute("hidden");
    document.getElementById('tableloan5').setAttribute("hidden", true);
    document.getElementById('tableloan6').setAttribute("hidden", true);
    document.getElementById('tableloan7').setAttribute("hidden", true);
    document.getElementById('tableloan8').setAttribute("hidden", true);
    document.getElementById('tableloan9').setAttribute("hidden", true);
    document.getElementById('tableloan10').setAttribute("hidden", true);
}
function ShowAppraisalFourthLevelApproval() {
    document.getElementById('tableappraisal2').removeAttribute("hidden");
    document.getElementById('tableappraisal3').removeAttribute("hidden");
    document.getElementById('tableappraisal4').removeAttribute("hidden");
    document.getElementById('tableappraisal5').setAttribute("hidden", true);
    document.getElementById('tableappraisal6').setAttribute("hidden", true);
    document.getElementById('tableappraisal7').setAttribute("hidden", true);
    document.getElementById('tableappraisal8').setAttribute("hidden", true);
    document.getElementById('tableappraisal9').setAttribute("hidden", true);
    document.getElementById('tableappraisal10').setAttribute("hidden", true);
}
function ShowTransferFourthLevelApproval() {
    document.getElementById('tabletransfer2').removeAttribute("hidden");
    document.getElementById('tabletransfer3').removeAttribute("hidden");
    document.getElementById('tabletransfer4').removeAttribute("hidden");
    document.getElementById('tabletransfer5').setAttribute("hidden", true);
    document.getElementById('tabletransfer6').setAttribute("hidden", true);
    document.getElementById('tabletransfer7').setAttribute("hidden", true);
    document.getElementById('tabletransfer8').setAttribute("hidden", true);
    document.getElementById('tabletransfer9').setAttribute("hidden", true);
    document.getElementById('tabletransfer10').setAttribute("hidden", true);
}
function ShowTrainingFourthLevelApproval() {
    document.getElementById('tabletraining2').removeAttribute("hidden");
    document.getElementById('tabletraining3').removeAttribute("hidden");
    document.getElementById('tabletraining4').removeAttribute("hidden");
    document.getElementById('tabletraining5').setAttribute("hidden", true);
    document.getElementById('tabletraining6').setAttribute("hidden", true);
    document.getElementById('tabletraining7').setAttribute("hidden", true);
    document.getElementById('tabletraining8').setAttribute("hidden", true);
    document.getElementById('tabletraining9').setAttribute("hidden", true);
    document.getElementById('tabletraining10').setAttribute("hidden", true);
}
function ShowAdvanceFourthLevelApproval() {
    document.getElementById('tableadvance2').removeAttribute("hidden");
    document.getElementById('tableadvance3').removeAttribute("hidden");
    document.getElementById('tableadvance4').removeAttribute("hidden");
    document.getElementById('tableadvance5').setAttribute("hidden", true);
    document.getElementById('tableadvance6').setAttribute("hidden", true);
    document.getElementById('tableadvance7').setAttribute("hidden", true);
    document.getElementById('tableadvance8').setAttribute("hidden", true);
    document.getElementById('tableadvance9').setAttribute("hidden", true);
    document.getElementById('tableadvance10').setAttribute("hidden", true);
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
function ShowLoanFifthLevelApproval() {
    document.getElementById('tableloan2').removeAttribute("hidden");
    document.getElementById('tableloan3').removeAttribute("hidden");
    document.getElementById('tableloan4').removeAttribute("hidden");
    document.getElementById('tableloan5').removeAttribute("hidden");
    document.getElementById('tableloan6').setAttribute("hidden", true);
    document.getElementById('tableloan7').setAttribute("hidden", true);
    document.getElementById('tableloan8').setAttribute("hidden", true);
    document.getElementById('tableloan9').setAttribute("hidden", true);
    document.getElementById('tableloan10').setAttribute("hidden", true);
}
function ShowAppraisalFifthLevelApproval() {
    document.getElementById('tableappraisal2').removeAttribute("hidden");
    document.getElementById('tableappraisal3').removeAttribute("hidden");
    document.getElementById('tableappraisal4').removeAttribute("hidden");
    document.getElementById('tableappraisal5').removeAttribute("hidden");
    document.getElementById('tableappraisal6').setAttribute("hidden", true);
    document.getElementById('tableappraisal7').setAttribute("hidden", true);
    document.getElementById('tableappraisal8').setAttribute("hidden", true);
    document.getElementById('tableappraisal9').setAttribute("hidden", true);
    document.getElementById('tableappraisal10').setAttribute("hidden", true);
}
function ShowTransferFifthLevelApproval() {
    document.getElementById('tabletransfer2').removeAttribute("hidden");
    document.getElementById('tabletransfer3').removeAttribute("hidden");
    document.getElementById('tabletransfer4').removeAttribute("hidden");
    document.getElementById('tabletransfer5').removeAttribute("hidden");
    document.getElementById('tabletransfer6').setAttribute("hidden", true);
    document.getElementById('tabletransfer7').setAttribute("hidden", true);
    document.getElementById('tabletransfer8').setAttribute("hidden", true);
    document.getElementById('tabletransfer9').setAttribute("hidden", true);
    document.getElementById('tabletransfer10').setAttribute("hidden", true);
}
function ShowTrainingFifthLevelApproval() {
    document.getElementById('tabletraining2').removeAttribute("hidden");
    document.getElementById('tabletraining3').removeAttribute("hidden");
    document.getElementById('tabletraining4').removeAttribute("hidden");
    document.getElementById('tabletraining5').removeAttribute("hidden");
    document.getElementById('tabletraining6').setAttribute("hidden", true);
    document.getElementById('tabletraining7').setAttribute("hidden", true);
    document.getElementById('tabletraining8').setAttribute("hidden", true);
    document.getElementById('tabletraining9').setAttribute("hidden", true);
    document.getElementById('tabletraining10').setAttribute("hidden", true);
}
function ShowAdvanceFifthLevelApproval() {
    document.getElementById('tableadvance2').removeAttribute("hidden");
    document.getElementById('tableadvance3').removeAttribute("hidden");
    document.getElementById('tableadvance4').removeAttribute("hidden");
    document.getElementById('tableadvance5').removeAttribute("hidden");
    document.getElementById('tableadvance6').setAttribute("hidden", true);
    document.getElementById('tableadvance7').setAttribute("hidden", true);
    document.getElementById('tableadvance8').setAttribute("hidden", true);
    document.getElementById('tableadvance9').setAttribute("hidden", true);
    document.getElementById('tableadvance10').setAttribute("hidden", true);
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
function ShowLoanSixthLevelApproval() {
    document.getElementById('tableloan2').removeAttribute("hidden");
    document.getElementById('tableloan3').removeAttribute("hidden");
    document.getElementById('tableloan4').removeAttribute("hidden");
    document.getElementById('tableloan5').removeAttribute("hidden");
    document.getElementById('tableloan6').removeAttribute("hidden");
    document.getElementById('tableloan7').setAttribute("hidden", true);
    document.getElementById('tableloan8').setAttribute("hidden", true);
    document.getElementById('tableloan9').setAttribute("hidden", true);
    document.getElementById('tableloan10').setAttribute("hidden", true);
}
function ShowAppraisalSixthLevelApproval() {
    document.getElementById('tableappraisal2').removeAttribute("hidden");
    document.getElementById('tableappraisal3').removeAttribute("hidden");
    document.getElementById('tableappraisal4').removeAttribute("hidden");
    document.getElementById('tableappraisal5').removeAttribute("hidden");
    document.getElementById('tableappraisal6').removeAttribute("hidden");
    document.getElementById('tableappraisal7').setAttribute("hidden", true);
    document.getElementById('tableappraisal8').setAttribute("hidden", true);
    document.getElementById('tableappraisal9').setAttribute("hidden", true);
    document.getElementById('tableappraisal10').setAttribute("hidden", true);
}
function ShowTransferSixthLevelApproval() {
    document.getElementById('tabletransfer2').removeAttribute("hidden");
    document.getElementById('tabletransfer3').removeAttribute("hidden");
    document.getElementById('tabletransfer4').removeAttribute("hidden");
    document.getElementById('tabletransfer5').removeAttribute("hidden");
    document.getElementById('tabletransfer6').removeAttribute("hidden");
    document.getElementById('tabletransfer7').setAttribute("hidden", true);
    document.getElementById('tabletransfer8').setAttribute("hidden", true);
    document.getElementById('tabletransfer9').setAttribute("hidden", true);
    document.getElementById('tabletransfer10').setAttribute("hidden", true);
}
function ShowTrainingSixthLevelApproval() {
    document.getElementById('tabletraining2').removeAttribute("hidden");
    document.getElementById('tabletraining3').removeAttribute("hidden");
    document.getElementById('tabletraining4').removeAttribute("hidden");
    document.getElementById('tabletraining5').removeAttribute("hidden");
    document.getElementById('tabletraining6').removeAttribute("hidden");
    document.getElementById('tabletraining7').setAttribute("hidden", true);
    document.getElementById('tabletraining8').setAttribute("hidden", true);
    document.getElementById('tabletraining9').setAttribute("hidden", true);
    document.getElementById('tabletraining10').setAttribute("hidden", true);
}
function ShowAdvanceSixthLevelApproval() {
    document.getElementById('tableadvance2').removeAttribute("hidden");
    document.getElementById('tableadvance3').removeAttribute("hidden");
    document.getElementById('tableadvance4').removeAttribute("hidden");
    document.getElementById('tableadvance5').removeAttribute("hidden");
    document.getElementById('tableadvance6').removeAttribute("hidden");
    document.getElementById('tableadvance7').setAttribute("hidden", true);
    document.getElementById('tableadvance8').setAttribute("hidden", true);
    document.getElementById('tableadvance9').setAttribute("hidden", true);
    document.getElementById('tableadvance10').setAttribute("hidden", true);
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
function ShowLoanSeventhLevelApproval() {
    document.getElementById('tableloan2').removeAttribute("hidden");
    document.getElementById('tableloan3').removeAttribute("hidden");
    document.getElementById('tableloan4').removeAttribute("hidden");
    document.getElementById('tableloan5').removeAttribute("hidden");
    document.getElementById('tableloan6').removeAttribute("hidden");
    document.getElementById('tableloan7').removeAttribute("hidden");
    document.getElementById('tableloan8').setAttribute("hidden", true);
    document.getElementById('tableloan9').setAttribute("hidden", true);
    document.getElementById('tableloan10').setAttribute("hidden", true);
}
function ShowAppraisalSeventhLevelApproval() {
    document.getElementById('tableappraisal2').removeAttribute("hidden");
    document.getElementById('tableappraisal3').removeAttribute("hidden");
    document.getElementById('tableappraisal4').removeAttribute("hidden");
    document.getElementById('tableappraisal5').removeAttribute("hidden");
    document.getElementById('tableappraisal6').removeAttribute("hidden");
    document.getElementById('tableappraisal7').removeAttribute("hidden");
    document.getElementById('tableappraisal8').setAttribute("hidden", true);
    document.getElementById('tableappraisal9').setAttribute("hidden", true);
    document.getElementById('tableappraisal10').setAttribute("hidden", true);
}
function ShowTransferSeventhLevelApproval() {
    document.getElementById('tabletransfer2').removeAttribute("hidden");
    document.getElementById('tabletransfer3').removeAttribute("hidden");
    document.getElementById('tabletransfer4').removeAttribute("hidden");
    document.getElementById('tabletransfer5').removeAttribute("hidden");
    document.getElementById('tabletransfer6').removeAttribute("hidden");
    document.getElementById('tabletransfer7').removeAttribute("hidden");
    document.getElementById('tabletransfer8').setAttribute("hidden", true);
    document.getElementById('tabletransfer9').setAttribute("hidden", true);
    document.getElementById('tabletransfer10').setAttribute("hidden", true);
}
function ShowTrainingSeventhLevelApproval() {
    document.getElementById('tabletraining2').removeAttribute("hidden");
    document.getElementById('tabletraining3').removeAttribute("hidden");
    document.getElementById('tabletraining4').removeAttribute("hidden");
    document.getElementById('tabletraining5').removeAttribute("hidden");
    document.getElementById('tabletraining6').removeAttribute("hidden");
    document.getElementById('tabletraining7').removeAttribute("hidden");
    document.getElementById('tabletraining8').setAttribute("hidden", true);
    document.getElementById('tabletraining9').setAttribute("hidden", true);
    document.getElementById('tabletraining10').setAttribute("hidden", true);
}
function ShowAdvanceSeventhLevelApproval() {
    document.getElementById('tableadvance2').removeAttribute("hidden");
    document.getElementById('tableadvance3').removeAttribute("hidden");
    document.getElementById('tableadvance4').removeAttribute("hidden");
    document.getElementById('tableadvance5').removeAttribute("hidden");
    document.getElementById('tableadvance6').removeAttribute("hidden");
    document.getElementById('tableadvance7').removeAttribute("hidden");
    document.getElementById('tableadvance8').setAttribute("hidden", true);
    document.getElementById('tableadvance9').setAttribute("hidden", true);
    document.getElementById('tableadvance10').setAttribute("hidden", true);
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
function ShowLoanEightLevelApproval() {
    document.getElementById('tableloan2').removeAttribute("hidden");
    document.getElementById('tableloan3').removeAttribute("hidden");
    document.getElementById('tableloan4').removeAttribute("hidden");
    document.getElementById('tableloan5').removeAttribute("hidden");
    document.getElementById('tableloan6').removeAttribute("hidden");
    document.getElementById('tableloan7').removeAttribute("hidden");
    document.getElementById('tableloan8').removeAttribute("hidden");
    document.getElementById('tableloan9').setAttribute("hidden", true);
    document.getElementById('tableloan10').setAttribute("hidden", true);
}
function ShowAppraisalEightLevelApproval() {
    document.getElementById('tableappraisal2').removeAttribute("hidden");
    document.getElementById('tableappraisal3').removeAttribute("hidden");
    document.getElementById('tableappraisal4').removeAttribute("hidden");
    document.getElementById('tableappraisal5').removeAttribute("hidden");
    document.getElementById('tableappraisal6').removeAttribute("hidden");
    document.getElementById('tableappraisal7').removeAttribute("hidden");
    document.getElementById('tableappraisal8').removeAttribute("hidden");
    document.getElementById('tableappraisal9').setAttribute("hidden", true);
    document.getElementById('tableappraisal10').setAttribute("hidden", true);
}
function ShowTransferEightLevelApproval() {
    document.getElementById('tabletransfer2').removeAttribute("hidden");
    document.getElementById('tabletransfer3').removeAttribute("hidden");
    document.getElementById('tabletransfer4').removeAttribute("hidden");
    document.getElementById('tabletransfer5').removeAttribute("hidden");
    document.getElementById('tabletransfer6').removeAttribute("hidden");
    document.getElementById('tabletransfer7').removeAttribute("hidden");
    document.getElementById('tabletransfer8').removeAttribute("hidden");
    document.getElementById('tabletransfer9').setAttribute("hidden", true);
    document.getElementById('tabletransfer10').setAttribute("hidden", true);
}
function ShowTrainingEightLevelApproval() {
    document.getElementById('tabletraining2').removeAttribute("hidden");
    document.getElementById('tabletraining3').removeAttribute("hidden");
    document.getElementById('tabletraining4').removeAttribute("hidden");
    document.getElementById('tabletraining5').removeAttribute("hidden");
    document.getElementById('tabletraining6').removeAttribute("hidden");
    document.getElementById('tabletraining7').removeAttribute("hidden");
    document.getElementById('tabletraining8').removeAttribute("hidden");
    document.getElementById('tabletraining9').setAttribute("hidden", true);
    document.getElementById('tabletraining10').setAttribute("hidden", true);
}
function ShowAdvanceEightLevelApproval() {
    document.getElementById('tableadvance2').removeAttribute("hidden");
    document.getElementById('tableadvance3').removeAttribute("hidden");
    document.getElementById('tableadvance4').removeAttribute("hidden");
    document.getElementById('tableadvance5').removeAttribute("hidden");
    document.getElementById('tableadvance6').removeAttribute("hidden");
    document.getElementById('tableadvance7').removeAttribute("hidden");
    document.getElementById('tableadvance8').removeAttribute("hidden");
    document.getElementById('tableadvance9').setAttribute("hidden", true);
    document.getElementById('tableadvance10').setAttribute("hidden", true);
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
function ShowLoanNinethLevelApproval() {
    document.getElementById('tableloan2').removeAttribute("hidden");
    document.getElementById('tableloan3').removeAttribute("hidden");
    document.getElementById('tableloan4').removeAttribute("hidden");
    document.getElementById('tableloan5').removeAttribute("hidden");
    document.getElementById('tableloan6').removeAttribute("hidden");
    document.getElementById('tableloan7').removeAttribute("hidden");
    document.getElementById('tableloan8').removeAttribute("hidden");
    document.getElementById('tableloan9').removeAttribute("hidden");
    document.getElementById('tableloan10').setAttribute("hidden", true);
}
function ShowAppraisalNinethLevelApproval() {
    document.getElementById('tableappraisal2').removeAttribute("hidden");
    document.getElementById('tableappraisal3').removeAttribute("hidden");
    document.getElementById('tableappraisal4').removeAttribute("hidden");
    document.getElementById('tableappraisal5').removeAttribute("hidden");
    document.getElementById('tableappraisal6').removeAttribute("hidden");
    document.getElementById('tableappraisal7').removeAttribute("hidden");
    document.getElementById('tableappraisal8').removeAttribute("hidden");
    document.getElementById('tableappraisal9').removeAttribute("hidden");
    document.getElementById('tableappraisal10').setAttribute("hidden", true);
}
function ShowTransferNinethLevelApproval() {
    document.getElementById('tabletransfer2').removeAttribute("hidden");
    document.getElementById('tabletransfer3').removeAttribute("hidden");
    document.getElementById('tabletransfer4').removeAttribute("hidden");
    document.getElementById('tabletransfer5').removeAttribute("hidden");
    document.getElementById('tabletransfer6').removeAttribute("hidden");
    document.getElementById('tabletransfer7').removeAttribute("hidden");
    document.getElementById('tabletransfer8').removeAttribute("hidden");
    document.getElementById('tabletransfer9').removeAttribute("hidden");
    document.getElementById('tabletransfer10').setAttribute("hidden", true);
}
function ShowTrainingNinethLevelApproval() {
    document.getElementById('tabletraining2').removeAttribute("hidden");
    document.getElementById('tabletraining3').removeAttribute("hidden");
    document.getElementById('tabletraining4').removeAttribute("hidden");
    document.getElementById('tabletraining5').removeAttribute("hidden");
    document.getElementById('tabletraining6').removeAttribute("hidden");
    document.getElementById('tabletraining7').removeAttribute("hidden");
    document.getElementById('tabletraining8').removeAttribute("hidden");
    document.getElementById('tabletraining9').removeAttribute("hidden");
    document.getElementById('tabletraining10').setAttribute("hidden", true);
}
function ShowAdvancerNinethLevelApproval() {
    document.getElementById('tableadvance2').removeAttribute("hidden");
    document.getElementById('tableadvance3').removeAttribute("hidden");
    document.getElementById('tableadvance4').removeAttribute("hidden");
    document.getElementById('tableadvance5').removeAttribute("hidden");
    document.getElementById('tableadvance6').removeAttribute("hidden");
    document.getElementById('tableadvance7').removeAttribute("hidden");
    document.getElementById('tableadvance8').removeAttribute("hidden");
    document.getElementById('tableadvance9').removeAttribute("hidden");
    document.getElementById('tableadvance10').setAttribute("hidden", true);
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
function ShowLoanTenthLevelApproval() {
    document.getElementById('tableloan2').removeAttribute("hidden");
    document.getElementById('tableloan3').removeAttribute("hidden");
    document.getElementById('tableloan4').removeAttribute("hidden");
    document.getElementById('tableloan5').removeAttribute("hidden");
    document.getElementById('tableloan6').removeAttribute("hidden");
    document.getElementById('tableloan7').removeAttribute("hidden");
    document.getElementById('tableloan8').removeAttribute("hidden");
    document.getElementById('tableloan9').removeAttribute("hidden");
    document.getElementById('tableloan10').removeAttribute("hidden");
}
function ShowAppraisalTenthLevelApproval() {
    document.getElementById('tableappraisal2').removeAttribute("hidden");
    document.getElementById('tableappraisal3').removeAttribute("hidden");
    document.getElementById('tableappraisal4').removeAttribute("hidden");
    document.getElementById('tableappraisal5').removeAttribute("hidden");
    document.getElementById('tableappraisal6').removeAttribute("hidden");
    document.getElementById('tableappraisal7').removeAttribute("hidden");
    document.getElementById('tableappraisal8').removeAttribute("hidden");
    document.getElementById('tableappraisal9').removeAttribute("hidden");
    document.getElementById('tableappraisal10').removeAttribute("hidden");
}
function ShowTransferTenthLevelApproval() {
    document.getElementById('tabletransfer2').removeAttribute("hidden");
    document.getElementById('tabletransfer3').removeAttribute("hidden");
    document.getElementById('tabletransfer4').removeAttribute("hidden");
    document.getElementById('tabletransfer5').removeAttribute("hidden");
    document.getElementById('tabletransfer6').removeAttribute("hidden");
    document.getElementById('tabletransfer7').removeAttribute("hidden");
    document.getElementById('tabletransfer8').removeAttribute("hidden");
    document.getElementById('tabletransfer9').removeAttribute("hidden");
    document.getElementById('tabletransfer10').removeAttribute("hidden");
}
function ShowTrainingTenthLevelApproval() {
    document.getElementById('tabletraining2').removeAttribute("hidden");
    document.getElementById('tabletraining3').removeAttribute("hidden");
    document.getElementById('tabletraining4').removeAttribute("hidden");
    document.getElementById('tabletraining5').removeAttribute("hidden");
    document.getElementById('tabletraining6').removeAttribute("hidden");
    document.getElementById('tabletraining7').removeAttribute("hidden");
    document.getElementById('tabletraining8').removeAttribute("hidden");
    document.getElementById('tabletraining9').removeAttribute("hidden");
    document.getElementById('tabletraining10').removeAttribute("hidden");
}
function ShowAdvanceTenthLevelApproval() {
    document.getElementById('tableadvance2').removeAttribute("hidden");
    document.getElementById('tableadvance3').removeAttribute("hidden");
    document.getElementById('tableadvance4').removeAttribute("hidden");
    document.getElementById('tableadvance5').removeAttribute("hidden");
    document.getElementById('tableadvance6').removeAttribute("hidden");
    document.getElementById('tableadvance7').removeAttribute("hidden");
    document.getElementById('tableadvance8').removeAttribute("hidden");
    document.getElementById('tableadvance9').removeAttribute("hidden");
    document.getElementById('tableadvance10').removeAttribute("hidden");
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