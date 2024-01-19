//allow numbers only 
function FunChkIsNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
function FunChkIsAmount(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 46)
    { return true; }
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
//allow alphanumeric 
function FunChkAlphaNumeric(e) {
    e = (e) ? e : window.event;
    var charCode = (e.which) ? e.which : e.keyCode;
    if (e.key === "Backspace")
    { return true }

    if ((charCode >= 48 && charCode <= 57) || (charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || (charCode == 32)) {
        return true;
    }
    return false;

    //if ((e.keyCode >= 48 && e.keyCode <= 57) || (e.keyCode >= 65 && e.keyCode <= 90) || (e.keyCode >= 97 && e.keyCode <= 122) || (e.keyCode == 32)) 
    //    return true;
    //return false;
}

//Allow decimal  numbers only 
function validateDec(key) {
    //getting key code of pressed key
    var keycode = (key.which) ? key.which : key.keyCode;
    //comparing pressed keycodes
    if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
        return false;
    }
    else {
        var parts = key.srcElement.value.split('.');
        if (parts.length > 1 && keycode == 46)
            return false;
        return true;
    }
}





//Allow alphabets only
function onlyAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }

        if (e.key === "Backspace")
        { return true }

        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

function FileNameFormat(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 95))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}
//sheetal
//function for class name allow alphabets and .
function GetclassName(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32 || (charCode == 46)))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}
//sheetal
//function to getdllpath . \ :

function GetDllPath(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else {
            return true;
        }

        if (((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)) || (charCode == 46 || charCode == 92 || charCode == 58))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

function GetIP(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 47 && charCode < 58) || (charCode == 46))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

function GetSourceSink(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 44))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}
function GetUser(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)  || (charCode == 64) || (charCode
        > 32 && charCode<58))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

function GetFolder(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}


//sheetal
//_=95
//function to get FileFolderpath  \ :
function GetFileFolderPath(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32 )|| (charCode == 92) || (charCode == 58))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}
//sheetal
//function to get GetHeaderCIF _ | 
function GetHeaderCIF(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 95) || (charCode == 124))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

//get sftp file folder path \
function GetSFTP_FolderPath(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 47))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

function IsAlphaOrIsNumeric(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else {
            return true;
        }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32) || (charCode==8))
            return true;
        if ((charCode > 47 && charCode < 58))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

//function to allow Capital letters only
function IsAlphaCapital(e) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else {
            return true;
        }
        if ((charCode > 64 && charCode < 91))
            return true;

        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}
//function to get userprefix
function GetUserPrefix(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else {
            return true;
        }
        if ((charCode > 64 && charCode < 91) || charCode == 95)
            return true;

        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}
//function to validate emailid

function ValidateEmail(mail) {
    if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(myForm.emailAddr.value)) {
        return (true)
    }
    //alert("You have entered an invalid email address!")
    return (false)
}
//validate ip address
function ValidateIPaddress(inputText) {
    var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
    if (inputText.value.match(ipformat)) {
        //document.form1.text1.focus();
        return true;
    }
    else {
        alert("You have entered an invalid IP address!");
        // document.form1.text1.focus();
        return false;
    }
}

function IPAddressKeyOnly(e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    if (keyCode != 46 && keyCode > 31 && (keyCode < 48 || keyCode > 57))
        return false;
    return true;
}

//function alpha(e, allow) {
//var email = '1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.@_'
//var bksp = 'backspace'
//var alt = 'alt'

//var k;
//k=document.all?parseInt(e.keyCode): parseInt(e.which);
//return (allow.indexOf(String.fromCharCode(k))!=-1);
//}
//onkeypress = "return alpha(event,email+bksp+alt)"

var pattern = /\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b/;
x = 46;
$('phPageBody_txtWinSCP_IP').keypress(function (e) {
    if (e.which != 8 && e.which != 0 && e.which != x && (e.which < 48 || e.which > 57)) {
        console.log(e.which);
        return false;
    }
}).keyup(function () {
    var this1 = $(this);
    if (!pattern.test(this1.val())) {
        $('#validate_ip').text('Not Valid IP');
        while (this1.val().indexOf("..") !== -1) {
            this1.val(this1.val().replace('..', '.'));
        }
        x = 46;
    } else {
        x = 0;
        var lastChar = this1.val().substr(this1.val().length - 1);
        if (lastChar == '.') {
            this1.val(this1.val().slice(0, -1));
        }
        var ip = this1.val().split('.');
        if (ip.length == 4) {
            $('#validate_ip').text('Valid IP');
        }
    }
});


function validate(email) {

    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    //var address = document.getElementById[email].value;
    if (reg.test(email) == false) {
        alert('Invalid Email Address');
        return (false);
    }
}

function checkEmail() {

    var email = document.getElementById('txtEmail');
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (!filter.test(email.value)) {
        alert('Please provide a valid email address');
        email.focus;
        return false;
    }
}
//To Reload
function Reload() {
    window.location.reload();
}
//Capital  first letter
jQuery.fn.capitalize = function () {
    $(this).keyup(function (event) {
        var box = event.target;
        var txt = $(this).val();
        var stringStart = box.selectionStart;
        var stringEnd = box.selectionEnd;
        $(this).val(txt.replace(/^(.)|(\s|\-)(.)/g, function ($word) {
            return $word.toUpperCase();
        }));
        box.setSelectionRange(stringStart, stringEnd);
    });

    return this;
}

function ajaxfunctioncall(pageurl, datainjson, funonsuccess, funonfailure) {
    //$.ajax({
    //    type: "POST",
    //    url: pageurl,
    //    data:datainjson,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: funonsuccess(),
    //    failure: funonfailure()
    //});

    $.ajax({
        type: 'POST',
        url: pageurl,
        context: document.body,
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: funonsuccess(),
        error: funonfailure()
    });
}

/*

 function(response) {
            alert(response.d);
        }

*/


fnreset = function (DivId, includehiddenfield) {
    if (includehiddenfield == null || includehiddenfield == undefined) includehiddenfield = false;
    if (DivId == null || DivId == undefined) DivId = ""; else DivId = "#" + DivId + ' ';
    $(DivId + "input").each(function () {
        if (includehiddenfield) {
            var typeObj = $(this).prop('type');
            if (typeObj == 'text' || typeObj == 'password' || typeObj == 'email') {

                $(this).val('');
            }
            return true;
        }
        else {
            if ($(this).css('display') != "none") {
                var typeObj = $(this).prop('type');
                if (typeObj == 'text' || typeObj == 'password' || typeObj == 'email') {
                    $(this).val('');
                }
                return true;
            }
        }
    });
    $("select").each(function () {
        if (includehiddenfield) {
            var typeObj = $(this).prop('type');
            if (typeObj == 'select-one') {
                $(this).val('0');
            }
            return true;
        }
        else {
            if ($(this).css('display') != "none") {
                var typeObj = $(this).prop('type');
                if (typeObj == 'select-one') {
                    $(this).val('0');
                }
                return true;
            }
        }
    });
    $("textarea").each(function () {
        if (includehiddenfield) {
            $(this).val('');
            return true;
        }
        else {
            if ($(this).css('display') != "none") {
                //var typeObj = $(this).prop('type');
                //if (typeObj == 'select-one') {
                //    $(this).val('0');
                //}
                $(this).val('');
                return true;
            }
        }
    });
}


function validateserver(lbl,div,msg) {
    if (msg != '') {
        $('#'+lbl).html(msg);
        $('#'+div).show();
    }
    else {
        $('#'+div).hide();
    }
}


function loginPageLoad(f) {

    $('#btnLogin').click(function () {

        var $pass = $('[id$="txtPwd"]').val();
        $('[id$="txtPwd"]').val(EncryptLocalAGS($pass));
        //var $UID = $('[id$="txtUsername"]').val();
        //$('[id$="txtUsername"]').val(EncryptLocalAGS($UID));
        //f.submit();
    });

    $('#txtUserLogin').keyup(function (event) {
        if (event.keyCode == 13) {
            $('#btnLogin').click();
        }
    });

    $('#txtUserPass').keyup(function (event) {
        if (event.keyCode == 13) {
            $('#btnLogin').click();
        }
    });

    $('#txtUserName').focus();
}

function EncryptLocalAGS(PlainTxt) {
    var key = $('[id$="hfPublicKey"]').val().split(';');
    if (key[0] != 0 && key[1] != 0) {
        return EncryptRSA(PlainTxt, key[0], key[1]);
    }
    else {
        return '';
    }
}