//# region "IssueList 6"
// Author       :       Swaraj Bhagat
// Date         :       30.05.2014 4.13 PM
// Decsription  :       Added for RSA encryption.
//# endregion 


function EncryptRSA(str, n, ek) {
    var EncData = '';
    try {
        for (var i = 0; i < str.length; i++) {
            var EncChar = EncDycChar(str.charCodeAt(i), parseFloat(n), parseFloat(ek)).toString();
            if (EncChar.length < 12) {
                var strAppend = "000000000000";
                EncChar = strAppend.substr(0, (12 - EncChar.length)) + EncChar;
            }
            var EncString = '';
            for (var j = 0; j < EncChar.length; j++) {
                var ChunkEncChar = Math.floor(1 + (Math.random() * 8)).toString() + EncChar.substr(j, 3);
                EncString = EncString + String.fromCharCode(parseFloat(ChunkEncChar));
                j = j + 2;
            }
            EncData = EncData + EncString;
        }
        EncData = Merge(EncData);
    }
    catch (e) {
        EncData = '';
    }    
    return EncData;
}

function EncDycChar(val, mod, root) {
    var Rem = root % 2;
    var Quotient = (root - Rem) / 2;
    var value = 0;
    var intb = 1;
    for (var k = 0; k < Quotient; k++) {
        var inta = (val * val) % mod;
        intb = (intb * inta) % mod;
    }
    if (Rem == 1) {
        intb = (intb * val) % mod;
    }
    value = intb;

    return value;
}

function Merge(strChar) {
    var strMerge = '';
    for (var i = 0; i < strChar.length; i++) {
        var b = Math.floor((Math.random() * 2));
        strMerge = strMerge + RandomString(b) + strChar.charAt(i);
        if (i == (strChar.length - 1)) {
            strMerge = strMerge + RandomString(Math.floor((Math.random() * 2)));
        }
    }
    return strMerge;
}

function RandomString(n) {
    if (!n) {
        n = 0;
    }
    var text = '';
    //var possible = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()_=[]{}';

    for (var i = 0; i < n; i++) {
        text += String.fromCharCode(Math.floor(10000 + (Math.random() * 10000)));
    }

    return text;
}