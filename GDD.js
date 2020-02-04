//<![CDATA[
var keyesc = false;
var display = true;
var currentTabIndex = 0;
var arrTextArea = new Array();
var selferror = 0;

function getAttrib(obj, att) {
    return obj.getAttribute(att);
}

function getTxtId(obj) {
    return obj.name.split("$")[3];
}

function getControl(txtId) {
    return document.getElementById("ctl00_CPH_MI_" + txtId);
}

function getTableId(obj) {
    while (obj.parentNode.id.toString().indexOf("TBL") == -1) {
        obj = obj.parentNode;
    }

    return obj.parentNode.id;
}

function getTable(obj) {
    return document.getElementById(getTableId(obj));
}

function getColName(obj) {
    if (obj.className.match(new RegExp("(^|\\s)d(\\s|$)")))
        return "description";

    if (obj.className.match(new RegExp("(^|\\s)w(\\s|$)")))
        return "weight";

    if (obj.className.match(new RegExp("(^|\\s)v(\\s|$)")))
        return "value";

    return "";
}

function getPreviousTxtId(obj, previousIndex) {
    var arr = obj.name.split("$");
    var num = arr[3].substring(3, 7) - previousIndex;
    var index = ((num <= 9)) ? "0" + (num) : num;
    var txtId = "ctl" + index;
    if (getTableId(obj) == getTableId(getControl(txtId))) return txtId;
    return obj.id.toString().substring(23, 30);
}

function getNextTxtId(obj, nextIndex) {
    var arr = obj.name.split("$");
    var num = arr[3].substring(3, 7) - 0 + nextIndex;
    var index = ((num <= 9)) ? "0" + num : num;
    var txtId = "ctl" + index;
    if (getTableId(obj) == getTableId(getControl(txtId))) return txtId;
    return obj.id.toString().substring(23, 30);
}

function validPreviousLine(obj) {
    var ctrl = obj.name.split("$");
    if (getAttrib(obj, "firstLine") == "True")
        return true;

    if (getColName(obj) == "description")
        return validField(obj, 1) && validField(obj, 2) && validField(obj, 3);

    if (getColName(obj) == "weight")
        return validField(obj, 2) && validField(obj, 3) && validField(obj, 4);

    if (getColName(obj) == "value")
        if (getAttrib(obj, "resultOnly")) return validField(obj, 1);
        else return validField(obj, 3) && validField(obj, 4) && validField(obj, 5);
}


function validField(obj, index) {
    var ctrl = getControl(getPreviousTxtId(obj, index));

    if (ctrl != null) {
        var colName = getColName(ctrl);
        switch (colName) {
            case "description":
                return ctrl.value != "";

            case "weight":
                return ctrl.value != 0 && ctrl.value >= 5 && ctrl.value <= 100;

            case "value":
                return ctrl.value != "";
        }
    }
}

function validPreviousFields(obj) {
    if (getColName(obj) == "description")
        return true;

    if (getColName(obj) == "weight")
        return validField(obj, 1);

    if (getColName(obj) == "value") {
        if (getAttrib(obj, "resultOnly") == "True") {
            if (getAttrib(obj, "firstLine") == "True") return true;
            else { return validField(obj, 1) };
        }
        else { return validField(obj, 1) && validField(obj, 2); }
    }
}

function completeAllResults(obj) {
    var tbl = getTable(obj);
    if (TotalWeight(obj) != 100) return false;
    if (!validPreviousFields(obj) || !validPreviousLine(obj)) return false;

    for (var i = 2; i < (tbl.rows.length - 2) ; i++) {
        if (getAttrib(obj, "resultOnly") == "True") {
            if (tbl.rows[i].getElementsByTagName("input")[0].value == "")
                return false;
        }
        else {
            if (tbl.rows[i].getElementsByTagName("input")[0].value != "" && tbl.rows[i].getElementsByTagName("input")[1].value != ""
                && tbl.rows[i].getElementsByTagName("input")[2].value == "")
                return false;

            if (tbl.rows[i].getElementsByTagName("input")[0].value == "" && tbl.rows[i].getElementsByTagName("input")[1].value != ""
                && tbl.rows[i].getElementsByTagName("input")[2].value != "")
                return false;

            if (tbl.rows[i].getElementsByTagName("input")[0].value != "" && tbl.rows[i].getElementsByTagName("input")[1].value == ""
                && tbl.rows[i].getElementsByTagName("input")[2].value != "")
                return false;
        }
    }
    return true;
}

function completeAllTextArea() {

    var txt = document.all.tabCFF.getElementsByTagName("TextArea");
    for (var i = 0; i < (txt.length) ; i++) {
        if (!txt[i].readOnly && txt[i].value.trim() == "")
            return false;
    }
    return true;
}

function getCompetitionColor(e) {
    var checkedCount = 0;
    for (var i = 0; i < (e.children[0].children[1].cells.length) ; i++) {
        var name = e.children[0].children[1].children[i].children[0].name;
        var opt = document.getElementsByName(name);
        var checked = false;
        var s = e.children[0].children[0].children[0].innerText;
        for (var j = 0; j < opt.length; j++) {
            if (opt[j].checked == true)
                checked = true;
        }
        if (!checked && !e.children[0].children[1].children[i].children[0].disabled) { checkedCount += 1; }
    }

    if (checkedCount == 0) return "g";
    if (checkedCount == e.children[0].children[1].cells.length) return "r"
    else return "y";
}

function getImprovementPlanColor(e) {
    var emptyCount = 0;
    var tblId = e.parentElement.parentElement.parentElement.parentElement.id;
    var tbl = document.getElementById(tblId);
    var txt = tbl.getElementsByTagName("TextArea");
    for (var i = 0; i < txt.length; i++) {
        if (
            (((txt[i].value.trim().replace(/[&\/\\#'",+$~%.:()*?<>{}]/g, '') == "") || (txt[i].value.trim().replace(/[&\/\\#'",+$~%.:()*?<>{}]/g, '').length < 3 && (txt[i].value.trim() != "N/A")))
                )
        )
            emptyCount++;
    }
    let length = txt.length;
    txt = tbl.getElementsByTagName("select");
    let last;
    for (var i = 0; i < (txt.length) ; i++) {
        if (
            !txt[i].disabled
            && (txt[i].selectedIndex == 0 || (i > 0 && last.value == txt[i].value))
           ) {
            emptyCount++;
        }
        last = txt[i];
    }
    length += txt.length;

    if (emptyCount == 0) return "g";
    if (emptyCount == length) return "r"
    else return "y";
}


function isComplete(tabIndex) {
    var tblNum = 0;
    var tbl = document.getElementById("ctl00_CPH_MI_TBL" + tblNum);
    var jefeGrupo = document.getElementById("ctl00_CPH_jefeGroup").value;
    console.log(jefeGrupo);

    switch (tabIndex) {
        case 0:
            while (tbl != null) {
                if (tbl.rows[tbl.rows.length - 2].cells[1].innerText == "" && jefeGrupo!=8)
                    return false;
                tblNum += 1;
                tbl = document.getElementById("ctl00_CPH_MI_TBL" + tblNum)
            }
            return true;

        case 1:
            return completeAllCompetitions();
        case 2:
            return validateIPAllTextBoxs();
    }
}

function managementResult(n) {
    var l = "";
    if (n == null) return "";

    if (n >= 0 && n <= 0.799) {
        return "1";
    } else if (n <= 0.849) {
        return "2";
    } else if (n <= 0.929) {
        return "3";
    } else if (n <= 0.979) {
        return "4";
    } else
        return "5";
}

function femsaCompetition(result) {
    switch (result) {
        case "1":
            return 0;
        case "2":
            return 0.845;
        case "3":
            return 0.92;
        case "4":
            return 0.97;
        case "5":
            return 1;
    }
}

function finalResult(n) {
    var l = "";

    if (n >= 0 && n <= 0.7999) {
        l = "1";
    } else if (n <= 0.8999) {
        l = "2";
    } else if (n <= 0.9499) {
        l = "3";
    } else if (n <= 0.9950) {
        l = "4";
    } else
        l = "5";

    return l;
}

function TotalWeight(obj) {
    var tbl = getTable(obj);
    var tot = 0;
    for (var i = 2; i < (tbl.rows.length - 2) ; i++)
        if (getAttrib(obj, "resultOnly") == "True")
            tot += parseInt(tbl.rows[i].cells[1].innerText);
        else {
            if (tbl.rows[i].getElementsByTagName("input")[1].value != "")
                tot += parseInt(tbl.rows[i].getElementsByTagName("input")[1].value);
        }
    return tot;
}

function completeAllCompetitions() {
    var count = 1;
    var cell = document.getElementById("ctl00_CPH_CFF_C1");
    while (cell != null) {
        if (cell.outerHTML.indexOf("g") == -1)
            return false;

        count += 1;
        cell = document.getElementById("ctl00_CPH_CFF_C1" + count);
    }
    return completeAllTextArea();
}

function completeOtherFields(obj) {
    var colName = getColName(obj);
    if (getAttrib(obj, "resultOnly") == "True")
        return true;

    switch (colName) {
        case "description":
            return getControl(getNextTxtId(obj, 1)).value != "" ||
                    getControl(getNextTxtId(obj, 2)).value != "";
        case "weight":
            return getControl(getPreviousTxtId(obj, 1)).value != "" ||
                    getControl(getNextTxtId(obj, 1)).value != "";
        case "value":
            return getControl(getPreviousTxtId(obj, 1)).value != "" ||
                    getControl(getPreviousTxtId(obj, 1)).value != "";
    }
}

function eraseResult(obj) {
    var tbl = getTable(obj);
    tbl.rows[tbl.rows.length - 2].cells[1].innerText = "";
    var txt = document.getElementById(tbl.id + "_l");
    txt.value = "";
}

function calculateFinalResult(obj) {
    var pond = 0;
    var letter = "";
    var result = 0;
    var tot = 0;
    var tbl = getTable(obj);
    for (var i = 2; i < (tbl.rows.length - 2) ; i++) {
        if (getAttrib(obj, "resultOnly") == "True") {
            pond = (tbl.rows[i].cells[1].innerText * 0.01);
            letter = (tbl.rows[i].getElementsByTagName("input")[0].value * 0.01);
        }
        else {
            pond = (tbl.rows[i].getElementsByTagName("input")[1].value * 0.01);
            letter = (tbl.rows[i].getElementsByTagName("input")[2].value * 0.01);
        }

        if (pond != 0 || pond != "") {
            result = managementResult(letter);
            var fc = femsaCompetition(result);
            tot += letter * pond;
        }
    }
    tbl.rows[tbl.rows.length - 2].cells[1].innerText = finalResult(tot);
    var txt = document.getElementById(tbl.id + "_l");
    txt.value = finalResult(tot) + "_" + tot.toString();
}

function validateCurrentTab() {
    if (!isComplete(currentTabIndex)) {
        switch (currentTabIndex) {
            case 0:
                {
                    revealModal('modalPageMoreInfo', "Debe completar todos los campos de [Indicadores de Gesti\u00f3n]");
                    break;
                }
            case 1:
                {
                    revealModal('modalPageMoreInfo', "Debe completar todos los campos de [Competencias / Comentarios]");
                    break;
                }
            case 2:
                {
                    revealModal('modalPageMoreInfo', "Debe completar todos los campos de [Plan de Mejora]");
                    break;
                }
        }
        return false;
    }
    return true;
}

function validateAllTabs() {
    if (!isComplete(0)) {
        revealModal('modalPageMoreInfo', "Debe completar todos los campos de [Indicadores de Gesti\u00f3n]");
        return false;
    }
    if (!isComplete(1)) {
        revealModal('modalPageMoreInfo', "Debe completar todos los campos de [Competencias / Comentarios]");
        return false;
    }
    if (!isComplete(2)) {
        revealModal('modalPageMoreInfo', "Debe completar todos los campos de [Plan de Mejora]");
        return false;
    }
    return true;
}

function _a(e, o) {
    e.nextSibling.nextSibling.readOnly = false;
    e.nextSibling.nextSibling.nextSibling.nextSibling.nextSibling.nextSibling.readOnly = true;
    e.nextSibling.nextSibling.focus();
    e.nextSibling.nextSibling.select();

}

function _r(e, o) {
    e.nextSibling.nextSibling.readOnly = false;
    e.previousSibling.previousSibling.readOnly = true;
    e.nextSibling.nextSibling.focus();
    e.nextSibling.nextSibling.select();
}

function _i(obj) { obj.style.backgroundColor = "#F7F8E0"; }
function o(obj) { obj.style.backgroundColor = "#ffffff"; }

function _k(e, o) {

    validateTxtByControl(o);
    var obj = $("ctl00_CPH_btnSend");

    if (obj != null) {
        obj.disabled = !validateIPTextBoxs();
    }
    e.srcElement.parentElement.parentElement.parentElement.parentElement.rows[7].cells[1].className = getImprovementPlanColor(e.srcElement);
    $("ctl00_CPH_t3").value = 1;

}

function _o(e, o) {
    e.style.backgroundColor = "#ffffff";
    validateTxtByControl(o);
}

function validateTxtByControl(o) {

    var obj = $("ctl00_CPH_PM_" + o);
    var index = obj.id.split("_")[4];
    var txt = obj.getElementsByTagName("TextArea");
    for (var i = 0; i < (txt.length) ; i++) {
        if (
            !txt[i].readOnly
            && (((txt[i].value.trim().replace(/[&\/\\#'",+$~%.:()*?<>{}]/g, '') == "") || (txt[i].value.trim().replace(/[&\/\\#'",+$~%.:()*?<>{}]/g, '').length < 3))
                || (txt[i].value.trim() == "N/A"))
        ) {
            arrTextArea[index] = false;
            return false;
        }
    }
    txt = obj.getElementsByTagName("select");
    let last;
    for (var i = 0; i < (txt.length) ; i++) {
        if (
            !txt[i].disabled
            && (txt[i].selectedIndex == 0 || (i > 0 && last.value == txt[i].value))
           ) {
            arrTextArea[index] = false;
            return false;
        }
        last = txt[i];
    }
    arrTextArea[index] = true;
    return true;

}

function validateIPTextBoxs() {
    if (arrTextArea.length != getTablesCount())
        return false;

    for (var i = 0; i < (arrTextArea.length) ; i++)
        if (arrTextArea[i] == false)
            return false;

    return true;
}

function validateIPAllTextBoxs() {
    var tablesCount = getTablesCount();
    for (var i = 0; i < tablesCount; i++) {
        var obj = $("ctl00_CPH_PM_tbl_" + i.toString());
        var txt = obj.getElementsByTagName("TextArea");
        for (var j = 0; j < (txt.length) ; j++) {
            if (!txt[j].readOnly && txt[j].value.trim() == "" && txt[j].getAttribute("class") != "Seg") {
                return false;
            }
        }
    }
    return true;
}

function getTablesCount() {
    var index = 0;
    var obj = $("ctl00_CPH_PM_tbl_0");
    while (obj != null) {
        index++;
        obj = document.getElementById("ctl00_CPH_PM_tbl_" + index);
    }
    return index;
}

function c(obj, flag) {
    if (flag) {
        var index = obj.parentElement.parentElement.parentElement.firstElementChild.firstElementChild.firstElementChild.rows.length - 1;
        obj.parentElement.parentElement.parentElement.firstElementChild.firstElementChild.firstElementChild.rows[index].cells[0].className = getCompetitionColor(obj);
    };
    $("ctl00_CPH_t2").value = 1;
}

function f(obj) {
    var colName = getColName(obj);
    obj.style.backgroundColor = "#F7F8E0";
    if (colName == "weight" || colName == "value") obj.select();
}

function kIP(obj) {
    $("ctl00_CPH_t2").value = 1;
}

function kSeg(obj) {
    $("ctl00_CPH_t5").value = 1;
}

function k(e) {
    keynum = event.keyCode;
    if (e.type != "paste") {
        if (keynum == null) return true;
        if (keynum == 13) return false;
        if (keynum == 27) { keyesc = true; return true };
    }
    var obj = e.srcElement;

    if (!validPreviousLine(obj) || !validPreviousFields(obj) || !completeOtherFields(obj) && TotalWeight(obj) >= 100)
        event.keyCode = 0;

    var chr = String.fromCharCode(keynum);
    var num = /^([0-9])*$/.test(chr);
    var colName = getColName(obj);

    if ((colName == "weight" || colName == "value") && !num)
        event.keyCode = 0;

    $("ctl00_CPH_t1").value = 1;
}

function b(obj) {
    var flag = true;
    obj.style.backgroundColor = "#ffffff";
    if (keyesc) { keyesc = false; return true };
    if (getColName(obj) == "weight" && validPreviousLine(obj) && validPreviousFields(obj) && flag && (obj.value < 5 || obj.value > 100)) {
        obj.focus();
        obj.select();
        var msg = "";
        if (obj.value < 5) revealModal('modalPageMoreInfo', "La ponderaci\u00f3n no puede ser menor al 5%.");
        if (obj.value > 100) revealModal('modalPageMoreInfo', "La ponderaci\u00f3n no puede ser mayor al 100%");
        eraseResult(obj);
        flag = false;
        return false;
    }

    if (getColName(obj) == "value" && validPreviousLine(obj) && validPreviousFields(obj) && flag && display && getAttrib(obj, "resultOnly") == "False" && obj.value == "") {
        obj.focus();
        obj.select();
        display = false;
        flag = false;
        revealModal('modalPageMoreInfo', "Debe ingresar un valor para poder continuar.");
        eraseResult(obj);
        return false;
    }

    if (getColName(obj) == "weight" && display && flag && TotalWeight(obj) > 100) {
        obj.focus();
        obj.select();
        display = false;
        flag = false;
        revealModal('modalPageMoreInfo', "La ponderaci\u00f3n no puede ser mayor al 100%");
        eraseResult(obj);
        return false;
    }

    display = true;

    if (getAttrib(obj, "resultOnly") == "True" && obj.value == "")
        eraseResult(obj);

    if (TotalWeight(obj) != 100)
        eraseResult(obj);

    if (getColName(obj) == "description" && getControl(getNextTxtId(obj, 1)).value != "" && getControl(getNextTxtId(obj, 2)).value != "" && obj.value == "")
        eraseResult(obj);

    if (completeAllResults(obj))
        calculateFinalResult(obj);

    return true;
}

function beforeSelectEvent(index, text) {
    var cancel = false;
    currentTabIndex = index;
    //return true;

    if ((text.indexOf("Evaluador") == 0 ||
        text.indexOf("Concurrente") == 0 ||
        text.indexOf("Doble Reporte") == 0) ||
        text.indexOf("Jefe RRHH") == 0) { return true;; }

    if (index == 0) { return true; }

    if (index == 1) {
        cancel = isComplete(0);
        if (!cancel) { revealModal('modalPageMoreInfo', "Debe completar todos los campos de [Indicadores de Gesti\u00f3n]") };
    }

    if (index == 2) {
        cancel = isComplete(0) && isComplete(1);
        if (!cancel) { revealModal('modalPageMoreInfo', "Debe completar todos los campos de [Competencias / Comentarios]") };
    }

    if (index == 3) { return true; }

    return cancel;
}

function switchViews(obj, row) {
    var div = document.getElementById(obj);
    var img = document.getElementById('img' + obj);

    if (div.style.display == "none") {
        div.style.display = "inline";
        if (row == 'alt') {
            img.src = "img/exp.gif";
            mce_src = "img/exp.gif";
        }
        else {
            img.src = "img/exp.gif";
            mce_src = "img/exp.gif";
        }
        img.alt = "Ocultar detalles";
    }
    else {
        div.style.display = "none";
        if (row == 'alt') {
            img.src = "img/col.gif";
            mce_src = "img/col.gif";
        }
        else {
            img.src = "img/col.gif";
            mce_src = "img/col.gif";
        }
        img.alt = "Mostrar detalles";
    }
}


var _rulesAdded = false;
var currentDiv;
var isIE6 = /msie|MSIE 6/.test(navigator.userAgent);

function revealModal(divId, msg) {
    currentDiv = divId;
    $(divId).style.display = $('modalBackground').style.display = 'block';

    $('MsgText').innerText = msg;

    if (isIE6) {
        var type = $('hideType').value;

        if (type == 'iframe')
            $('modalIframe').style.display = 'block';

        if (type == 'replace')
            ReplaceSelectsWithSpans();
    }


    OnWindowResize();

    if (window.attachEvent)
        window.attachEvent('onresize', OnWindowResize);

    else if (window.addEventListener)
        window.addEventListener('resize', OnWindowResize, false);
    else
        window.onresize = OnWindowResize;

    if (document.all)
        document.documentElement.onscroll = OnWindowResize;
}


function revealModalNotButton(divId, msg) {
    currentDiv = divId;
    $(divId).style.display = $('modalBackground').style.display = 'block';

    $('MsgTextNotButton').innerText = msg;

    if (isIE6) {
        var type = $('hideType').value;

        if (type == 'iframe')
            $('modalIframe').style.display = 'block';

        if (type == 'replace')
            ReplaceSelectsWithSpans();
    }


    OnWindowResize();

    if (window.attachEvent)
        window.attachEvent('onresize', OnWindowResize);

    else if (window.addEventListener)
        window.addEventListener('resize', OnWindowResize, false);
    else
        window.onresize = OnWindowResize;

    if (document.all)
        document.documentElement.onscroll = OnWindowResize;
}


function OnWindowResize() {
    var left = isIE6 ? document.documentElement.scrollLeft : 0;
    var top = isIE6 ? ((document.documentElement.scrollTop == 0) ? 1 : document.documentElement.scrollTop) : 0;
    var div = moveDiv(currentDiv, top, left);
}



function moveDiv(divId, top, left) {
    var div = $(divId);
    if (div) {
        div.style.left = Math.max((left + (GetWindowWidth() - div.offsetWidth) / 2), 0) + 'px';
        div.style.top = Math.max((top + (GetWindowHeight() - div.offsetHeight) / 2), 0) + 'px';
    }
}

function hideModal(divId) {
    $(divId).style.display = $('modalBackground').style.display = 'none';

    if (document.all) {
        var type = $('hideType').value;

        if (type == 'iframe')
            $('modalIframe').style.display = 'none';

        if (type == 'replace')
            RemoveSelectSpans();
    }

    if (window.detachEvent)
        window.detachEvent('onresize', OnWindowResize);
    else if (window.removeEventListener)
        window.removeEventListener('resize', OnWindowResize, false);
    else
        window.onresize = null;

    if ($(divId).innerText.trim() == "La Evaluaci\u00f3n se envi\u00f3 exitosamente!")
    { window.location.href = "Default.aspx"; }

    if ($(divId).innerText.trim().indexOf("Corrobore que todos los campos se encuentren completos") >= 0) {
        location.replace(window.location.href);
    }
}

function RemoveSelectSpans() {
    var selects = document.getElementsByTagName('select');
    for (var i = 0; i < selects.length; i++) {
        var select = selects[i];

        if (select.clientWidth == 0 || select.clientHeight == 0 ||
              select.nextSibling == null || select.nextSibling.className != 'selectReplacement')
        { continue; }



        select.parentNode.removeChild(select.nextSibling);
        select.style.display = select.cachedDisplay;
    }
}

function ReplaceSelectsWithSpans() {
    var selects = document.getElementsByTagName('select');
    for (var i = 0; i < selects.length; i++) {
        var select = selects[i];

        if (select.clientWidth == 0 || select.clientHeight == 0)
        { continue; }

        var span = document.createElement('span');
        span.style.height = (select.clientHeight - 4) + 'px';
        span.style.width = (select.clientWidth - 6) + 'px';
        span.style.display = 'inline-block';
        span.style.border = '1px solid rgb(200, 210, 230)';
        span.style.padding = '1px 0 0 4px';
        span.style.position = 'relative';
        span.style.top = '1px';
        span.className = 'selectReplacement';
        span.innerHTML = select.options[select.selectedIndex].innerHTML;
        select.cachedDisplay = select.style.display;
        select.style.display = 'none';
        select.parentNode.insertBefore(span, select.nextSibling);
    }
}

function AddStyleRules() {
    if (_rulesAdded) return;

    _rulesAdded = true;

    var stylesheet = document.styleSheets[document.styleSheets.length - 1];
    if (!document.all) {
        InsertCssRule(stylesheet, '#modalBackground', 'position: fixed; height: 100%; width: 100%; left: 0; top: 0;');
        InsertCssRule(stylesheet, '#modalWindow', 'position: fixed; left: 0; top: 0;');
    }
    else {
        InsertCssRule(stylesheet, '#modalBackground',
                'position: absolute; ' +
                'left: expression(ignoreMe = document.documentElement.scrollLeft + "px"); ' +
                'top: expression(ignoreMe = document.documentElement.scrollTop + "px");' +
                'width: expression(document.documentElement.clientWidth + "px"); ' +
                'height: expression(document.documentElement.clientHeight + "px");');



        InsertCssRule(stylesheet, '#modalWindow',
                'position: absolute; ' +
                'left: expression(ignoreMe = document.documentElement.scrollLeft + "px"); ' +
                'top: expression(ignoreMe = document.documentElement.scrollTop + "px");');
    }
}

function InsertCssRule(stylesheet, selector, rule) {
    if (stylesheet.addRule) {
        stylesheet.addRule(selector, rule, stylesheet.rules.length);
        return stylesheet.rules.length - 1;
    }
    else {
        stylesheet.insertRule(selector + ' {' + rule + '}', stylesheet.cssRules.length);
        return stylesheet.cssRules.length - 1;
    }
}

/* utiltiy functions */

function GetWindowWidth() {
    var width =
        document.documentElement && document.documentElement.clientWidth ||
        document.body && document.body.clientWidth ||
        document.body && document.body.parentNode && document.body.parentNode.clientWidth ||
        0;
    return width;
}

function GetWindowHeight() {
    var height =
        document.documentElement && document.documentElement.clientHeight ||
        document.body && document.body.clientHeight ||
        document.body && document.body.parentNode && document.body.parentNode.clientHeight ||
        0;
    return height;
}

function $(id) {
    return document.getElementById(id);
}
var ext
function p(parameter) {
    //  window.showModalDialog("PrintPage.htm", parameter, "dialogHeight: 400px; dialogWidth: 600px; edge: Raised; center:Yes; help: no; resizable=No; status: no");
    // **************** 12 linea modificada al original
    window.showModalDialog("PrintReport.aspx?EvaluationId=" + parameter, parameter, "dialogHeight: 800PX; dialogWidth: 700px; edge: Raised; center:Yes; help: no; resizable=Yes; status: no");
    //    setTimeout('__doPostBack();', 10000, 'ctl00_CPH_UP', parameter);
    __doPostBack('ctl00_CPH_UP', parameter);
    //revealModalNotButton('modalPageMoreInfoNotButton', "Record\u00e1 reunirte con tu colaborador para darle la devoluci\u00f3n de su evaluaci\u00f3n. La devoluci\u00f3n es una instancia muy importante del proceso, ya que a tu colaborador le permitir\u00e1 conocer sus fortalezas y \u00e1reas de oportunidad, contribuyendo, de manera progresiva, a una optimizaci\u00f3n de su desempe\u00f1o. Es importante, es un compromiso de todos!");
    setTimeout(function () { alert("Record\u00e1 reunirte con tu colaborador para darle la devoluci\u00f3n de su evaluaci\u00f3n. La devoluci\u00f3n es una instancia muy importante del proceso, ya que a tu colaborador le permitir\u00e1 conocer sus fortalezas y \u00e1reas de oportunidad, contribuyendo, de manera progresiva, a una optimizaci\u00f3n de su desempe\u00f1o. Es importante, es un compromiso de todos!") }, 1000);

}

//function s(parameter) {
//    if(validateAllTabs()) {
//        __doPostBack('ctl00_CPH_btnSave', parameter); 
////        __doPostBack('ctl00_CPH_UP', parameter);          
//        revealModal('modalPageMoreInfo', "La Evaluaci\u00f3n se envi\u00f3 exitosamente!");
//    }
//}

//\u00e1 -> á 
//\u00e9 -> é 
//\u00ed -> í 
//\u00f3 -> ó 
//\u00fa -> ú 
//\u00c1 -> Á 
//\u00c9 -> É 
//\u00cd -> Í 
//\u00d3 -> Ó 
//\u00da -> Ú 
//\u00f1 -> ñ
//\u00d1 -> Ñ

function hasSupport() {
    if (typeof hasSupport.support != "undefined")
        return hasSupport.support;

    var ie55 = /msie 5\.[56789]/i.test(navigator.userAgent);

    hasSupport.support = (typeof document.implementation != "undefined" &&
		document.implementation.hasFeature("html", "1.0") || ie55)

    if (ie55) {
        document._getElementsByTagName = document.getElementsByTagName;
        document.getElementsByTagName = function (sTagName) {
            if (sTagName == "*")
                return document.all;
            else
                return document._getElementsByTagName(sTagName);
        };
    }
    return hasSupport.support;
}


function WebFXTabPane(el, bUseCookie) {
    if (!hasSupport() || el == null) return;

    this.element = el;
    this.element.tabPane = this;
    this.pages = [];
    this.selectedIndex = null;
    this.useCookie = bUseCookie != null ? bUseCookie : true;
    this.element.className = this.classNameTag + " " + this.element.className;
    this.tabRow = document.createElement("div");
    this.tabRow.className = "tab-row";
    el.insertBefore(this.tabRow, el.firstChild);

    var cs = el.childNodes;
    var tabIndex = 0;


    switch (document.getElementById("ctl00_CPH_pId").value) {
        case "2":
            if (cs.length > 2) { tabIndex = 1 };
            break;
        case "3":
            if (el.parentNode.id == "tabCFF" && el.childNodes[2].className == "tab-page" && cs.length == 4) { tabIndex = 2 }
            if (el.parentNode.id == "tabCFF" && el.childNodes[2].className == "tab-page" && cs.length == 5) { tabIndex = 2 }
            break;
            // ********************** 11 linea agregada al original          
        case "5":
            if (el.parentNode.id == "tabCFF" && el.childNodes[5].className == "tab-page" && cs.length == 6)
                tabIndex = 3
            else
                tabIndex = 1;
    }
    this.selectedIndex = tabIndex;

    var n;
    for (var i = 0; i < cs.length; i++) {
        if (cs[i].nodeType == 1 && cs[i].className == "tab-page") {
            this.addTabPage(cs[i]);
        }
    }
}

WebFXTabPane.prototype.classNameTag = "dynamic-tab-pane-control";

WebFXTabPane.prototype.setSelectedIndex = function (n) {
    if (this.selectedIndex != n) {
        if (this.selectedIndex != null && this.pages[this.selectedIndex] != null)
            this.pages[this.selectedIndex].hide();
        this.selectedIndex = n;
        this.pages[this.selectedIndex].show();

        if (this.useCookie)
            WebFXTabPane.setCookie("webfxtab_" + this.element.id, n); // session cookie
    }
};

WebFXTabPane.prototype.getSelectedIndex = function () {
    return this.selectedIndex;
};

WebFXTabPane.prototype.addTabPage = function (oElement) {
    if (!hasSupport()) return;

    if (oElement.tabPage == this)
        return oElement.tabPage;

    var n = this.pages.length;
    var tp = this.pages[n] = new WebFXTabPage(oElement, this, n);
    tp.tabPane = this;

    this.tabRow.appendChild(tp.tab);
    if (n == this.selectedIndex) {
        tp.show();
    }
    else
        tp.hide();

    return tp;
};

WebFXTabPane.prototype.dispose = function () {
    this.element.tabPane = null;
    this.element = null;
    this.tabRow = null;

    for (var i = 0; i < this.pages.length; i++) {
        this.pages[i].dispose();
        this.pages[i] = null;
    }
    this.pages = null;
};


WebFXTabPane.setCookie = function (sName, sValue, nDays) {
    var expires = "";
    if (nDays) {
        var d = new Date();
        d.setTime(d.getTime() + nDays * 24 * 60 * 60 * 1000);
        expires = "; expires=" + d.toGMTString();
    }

    document.cookie = sName + "=" + sValue + expires + "; path=/";
};

WebFXTabPane.getCookie = function (sName) {
    var re = new RegExp("(\;|^)[^;]*(" + sName + ")\=([^;]*)(;|$)");
    var res = re.exec(document.cookie);
    return res != null ? res[3] : null;
};

WebFXTabPane.removeCookie = function (name) {
    setCookie(name, "", -1);
};


function WebFXTabPage(el, tabPane, nIndex) {
    if (!hasSupport() || el == null) return;

    this.element = el;
    this.element.tabPage = this;
    this.index = nIndex;

    var cs = el.childNodes;
    for (var i = 0; i < cs.length; i++) {
        if (cs[i].nodeType == 1 && cs[i].className == "tab") {
            this.tab = cs[i];
            break;
        }
    }

    var a = document.createElement("A");
    this.aElement = a;
    a.href = "#";
    a.onclick = function () { return false; };
    while (this.tab.hasChildNodes())
        a.appendChild(this.tab.firstChild);
    this.tab.appendChild(a);

    var oThis = this;
    this.tab.onclick = function () { oThis.select(); };
    this.tab.onmouseover = function () { WebFXTabPage.tabOver(oThis); };
    this.tab.onmouseout = function () { WebFXTabPage.tabOut(oThis); };
}

WebFXTabPage.prototype.show = function () {
    var el = this.tab;
    var s = el.className + " selected";
    s = s.replace(/ +/g, " ");
    el.className = s;

    this.element.style.display = "block";
};

WebFXTabPage.prototype.hide = function () {
    var el = this.tab;
    var s = el.className;
    s = s.replace(/ selected/g, "");
    el.className = s;

    this.element.style.display = "none";
};


WebFXTabPage.prototype.select = function () {
    var txt = this.tab.innerText;
    var i = this.index;
    if (!beforeSelectEvent(this.index, txt)) { return false };
    var t = document.getElementById("ctl00_tab");
    if ((txt.indexOf("Indicadores de Gesti") == 0 || txt.indexOf("Competencias") == 0 || txt.indexOf("Agenda de") == 0 || txt.indexOf("Auto evaluaci") == 0)) { t.value = i; }
    this.tabPane.setSelectedIndex(this.index);

};

WebFXTabPage.prototype.dispose = function () {
    this.aElement.onclick = null;
    this.aElement = null;
    this.element.tabPage = null;
    this.tab.onclick = null;
    this.tab.onmouseover = null;
    this.tab.onmouseout = null;
    this.tab = null;
    this.tabPane = null;
    this.element = null;
};

WebFXTabPage.tabOver = function (tabpage) {
    var el = tabpage.tab;
    var s = el.className + " hover";
    s = s.replace(/ +/g, " ");
    el.className = s;
};

WebFXTabPage.tabOut = function (tabpage) {
    var el = tabpage.tab;
    var s = el.className;
    s = s.replace(/ hover/g, "");
    el.className = s;
};


function setupAllTabs() {
    if (!hasSupport()) return;

    var all = document.getElementsByTagName("*");
    var l = all.length;
    var tabPaneRe = /tab\-pane/;
    var tabPageRe = /tab\-page/;
    var cn, el;
    var parentTabPane;

    for (var i = 0; i < l; i++) {
        el = all[i]
        cn = el.className;

        if (cn == "") continue;

        if (tabPaneRe.test(cn) && !el.tabPane)
            new WebFXTabPane(el);

        else if (tabPageRe.test(cn) && !el.tabPage &&
				tabPaneRe.test(el.parentNode.className)) {
            el.parentNode.tabPane.addTabPage(el);
        }
    }
}

function disposeAllTabs() {
    if (!hasSupport()) return;

    var all = document.getElementsByTagName("*");
    var l = all.length;
    var tabPaneRe = /tab\-pane/;
    var cn, el;
    var tabPanes = [];

    for (var i = 0; i < l; i++) {
        el = all[i]
        cn = el.className;

        if (cn == "") continue;

        if (tabPaneRe.test(cn) && el.tabPane)
            tabPanes[tabPanes.length] = el.tabPane;
    }

    for (var i = tabPanes.length - 1; i >= 0; i--) {
        tabPanes[i].dispose();
        tabPanes[i] = null;
    }
}

if (typeof window.addEventListener != "undefined")
    window.addEventListener("load", setupAllTabs, false);
else if (typeof window.attachEvent != "undefined") {
    window.attachEvent("onload", setupAllTabs);
    window.attachEvent("onunload", disposeAllTabs);
}
else {
    if (window.onload != null) {
        var oldOnload = window.onload;
        window.onload = function (e) {
            oldOnload(e);
            setupAllTabs();
        };
    }
    else
        window.onload = setupAllTabs;
}

document.onkeydown = checkKeys
function checkKeys() {
    if (event.keyCode == 13) {
        return false;
    }
    if (event.ctrlKey && event.keyCode == 78) {
        event.keyCode = 0;
        return false;
    }

}


function setBtnSend() {
    var obj = $("ctl00_CPH_btnSend");
    if (obj != null) {
        $("ctl00_CPH_btnSend").disabled = !validateIPAllTextBoxs();
    }
}

function setBtnSendError() {
    var obj = $("ctl00_CPH_btnSend");
    if (obj != null) {
        $("ctl00_CPH_btnSend").disabled = false;
    }
}


function InitializeRequestHandler(sender, args) {

    if (prm.get_isInAsyncPostBack()) {
        args.set_cancel(true);
        //        pbQueue.push(args.get_postBackElement().id);
        //        argsQueue.push(document.forms[0].__EVENTARGUMENT.value);
    }

    if (sender._postBackSettings.sourceElement.id == "ctl00_CPH_btnSend") {
        if (!validateAllTabs()) {
            args.set_cancel(true);
        }
    }
}

function SetSelfError(i) {
    selferror = i;
}

function EndRequestHandler(sender, args) {
    if (pbQueue.length > 0) {
        __doPostBack(pbQueue.shift(), argsQueue.shift());
    }

    var error = 0;
    if (document.all.tabCFF != null) {
        var txt = document.all.tabCFF.getElementsByTagName("TextArea");
        var opt = document.all.tabCFF.getElementsByTagName("input");

        for (var i = 0; i < (txt.length) ; i++) {
            if (!txt[i].readOnly && ((txt[i].value.trim().replace(/[&/\\#'", +$~%.:() *?<>{ }]/g, '').length < 3) || (txt[i].value.trim().indexOf("En caso de") > -1))) {
                error++;
                if ((txt[i].previousSibling.previousSibling != null && txt[i].previousSibling.previousSibling.checked != undefined && !txt[i].previousSibling.previousSibling.checked) || (txt[i].previousSibling != null && txt[i].previousSibling.checked != undefined && !txt[i].previousSibling.checked))
                    error--;
            }
        }

        //if (error > 0) {
        //    for (var i = 0; i < (txt.length) ; i++)
        //        if (opt[i].type == "checkbox" && !opt[i].checked)
        //            error--;
        //}
    }

    if (sender._postBackSettings.sourceElement.id == "ctl00_CPH_btnSendSelf") {
        if (selferror == 1) {
            revealModal('modalPageMoreInfo', "Todos los campo a excepci\u00f3n de las tareas destacadas son obligatorios. Intente nuevamente luego de completarlos.");
        }
        else {
            revealModal('modalPageMoreInfo', "La Evaluaci\u00f3n se envi\u00f3 exitosamente!");
        }
    }

    if (sender._postBackSettings.sourceElement.id == "ctl00_CPH_btnSend") {
        if (error == 0)
            revealModal('modalPageMoreInfo', "La Evaluaci\u00f3n se envi\u00f3 exitosamente!");
        else if (error == 1) {
            setBtnSendError();
            revealModal('modalPageMoreInfo', "Hubo " + error + " error en las evaluaciones al realizar el env\u00edo. Corrobore que todos los campos se encuentren completos.\nAdicionalmente, recuerde que los campos de texto deben contener al menos 3 caracteres, exceptuando caracteres especiales.");
        }
        else {
            setBtnSendError();
            revealModal('modalPageMoreInfo', "Hubo " + error + " errores en las evaluaciones al realizar el env\u00edo. Corrobore que todos los campos se encuentren completos.\nAdicionalmente, recuerde que los campos de texto deben contener al menos 3 caracteres, exceptuando caracteres especiales.");
        }
    }

    if (sender._postBackSettings.sourceElement.id == "ctl00_CPH_btnSave") {
        setBtnSend();
        revealModal('modalPageMoreInfo', "Los datos se grabaron exitosamente.");
    }


}

function Redirect(url) {
    window.location = url;
}

//]]>
