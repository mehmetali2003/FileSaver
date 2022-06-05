
String.prototype.isNullOrEmpty = function () {
    return !(this && this.length > 0) || this == "&nbsp;";
};

function FreezeScreen()
{
    scroll(0, 0);
    var outerPane = document.getElementById('FreezePane');
    if (outerPane) outerPane.className = 'FreezePaneOn';
}

function FreezeScreenOff() {
    scroll(0, 0);
    var outerPane = document.getElementById('FreezePane');
    if (outerPane) outerPane.className = 'FreezePaneOff';
}

function makeVisible(controlID) {
    document.getElementById(controlID).style.visibility = 'visible';
}

function validatePersonData(gridViewClientID) {
    try {
        var gv = document.getElementById(gridViewClientID);
        var rowCount = gv.rows.length - 1; //minus the header row
        var firstNameColumnIndex = 1;
        var surnameColumnIndex = 2
        var ageColumnIndex = 3;
        var sexColumnIndex = 4;
        var mobileColumnIndex = 5;
        var activeColumnIndex = 6;
        var message = "";
        for (var i = 1; i < rowCount; i++) {
            var row = gv.rows[i];
            var firstName = row.cells[firstNameColumnIndex].innerHTML;
            var surname = row.cells[surnameColumnIndex].innerHTML;
            var age = row.cells[ageColumnIndex].innerHTML;
            var sex = row.cells[sexColumnIndex].innerHTML;
            var mobile = row.cells[mobileColumnIndex].innerHTML;
            var active = row.cells[activeColumnIndex].innerHTML;

            if (firstName.isNullOrEmpty()) {
                message += `Firstname in row: ${i + 1} column: ${firstNameColumnIndex + 1} must be filled! \n`;
            }

            if (surname.isNullOrEmpty()) {
                message += `Surname in row: ${i + 1} column: ${surnameColumnIndex + 1} must be filled! \n`;
            }

            if (isNaN(age)) {
                message += `Age in row: ${i + 1} column: ${ageColumnIndex + 1} must be numeric! \n`;
            }

            if (sex.isNullOrEmpty()) {
                message += `Sex in row: ${i + 1} column: ${sexColumnIndex + 1} must be filled! \n`;
            } else if (!['M', 'F'].includes(sex)) {
                message += `Sex in row: ${i + 1} column: ${sexColumnIndex + 1} must be 'M' or 'F'! \n`;
            }

            if (mobile.isNullOrEmpty()) {
                message += `Mobile in row: ${i + 1} column: ${mobileColumnIndex + 1} must be filled! \n`;
            } else if (isNaN(mobile)) {
                message += `Mobile in row: ${i + 1} column: ${mobileColumnIndex + 1} must be numeric! \n`;
            }

            if (active.isNullOrEmpty()) {
                message += `Active in row: ${i + 1} column: ${activeColumnIndex + 1} must be filled! \n`;
            } else if (!['TRUE', 'FALSE'].includes(active)) {
                message += `Active in row: ${i + 1} column: ${activeColumnIndex + 1} must be 'TRUE' or 'FALSE'! \n`;
            }
        }

        if (message.length > 0) {
            message = "Please follow directions below! \n" + message;
            alert(message);
            return false;
        }
        return true;
    }
    catch (e) {
        return false;
    }
}


/// FREEZE EFFECT in processing ... 
function FreezeScreen()
//http://www.4guysfromrolla.com/webtech/100406-1.shtml
{
    scroll(0, 0);
    var outerPane = document.getElementById('FreezePane');
    if (outerPane) outerPane.className = 'FreezePaneOn';
}

function FreezeScreenOff() {
    scroll(0, 0);
    var outerPane = document.getElementById('FreezePane');
    if (outerPane) outerPane.className = 'FreezePaneOff';
}


