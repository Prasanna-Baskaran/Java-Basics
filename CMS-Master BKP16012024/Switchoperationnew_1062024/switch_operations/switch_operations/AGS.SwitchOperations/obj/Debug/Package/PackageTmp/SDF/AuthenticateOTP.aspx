<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="AuthenticateOTP.aspx.cs" Inherits="AGS.SwitchOperations.SDF.AuthenticateOTP" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <!-- Meta, title, CSS, favicons, etc. -->
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title></title>
    
    <script type = "text/javascript" >  
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()",10000);
        window.onunload = function () { null };
          
        
    </script> 

    <!-- Bootstrap -->
    <link href="../Styles/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <%--<link href="Styles/font-awesome.min.css" rel="stylesheet">--%>
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../Scripts/crypto-js.js"></script>
    <!-- Animate.css -->
    <link href="../Styles/animate.css/animate.min.css" rel="stylesheet" />
    <!-- Custom Theme Style -->
    <link href="../Styles/custom.css" rel="stylesheet" />
    <%-- Loader --%>
    <link href="../Styles/Loader.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/Loader.js"></script>
    <script type="text/javascript" src="../../Scripts/RSA.js"></script>
    <script type="text/javascript" src="../Scripts/JsCommon.js"></script>
    <script>
        $(document).ready(function () {
            startTime();
            //var otp = Generateotp();
            //while (otp<=100000)
            //    otp = Generateotp();
            //$('#Passwordcheck').val(otp);
            //alert("generated Otp is: " + otp);
            //alert("Password check value is" + $('#Passwordcheck').val());
        });
    </script>
        <script type="text/javascript">
            function startTime() {
                const dat = new Date();
                document.getElementById('Header').innerHTML=dat;
                setTimeout(startTime, 1000);
            }
            function FunShowLoader() {
                if ($('#Asplbl').val() != "") {
                    $('.shader').fadeIn();
                    return true;
                }
            }
            function FunOTPcheck() {
                var $pass = $('#Asplbl').val();
                var v = 123456;
                var s = ($pass ^ v);
                $('#Asplbl').val(s);
                
            }
        </script>
<%--    <script >
        //function otptimerout() {
        //    alert("Otp is expired. Click OK to continue.. ");
        //    $('#Asplbl').val(null);
        //    var otp = Generateotp();
        //    alert("otp is:  " + otp);
        //    $('#Passwordcheck').val(otp);
        //    return false;
        //}
        //function Validateotp() {
        //    var otp = $('#Passwordcheck').val();
        //    var password1 = $('#Asplbl').val();
        //    if ($('#Asplbl').val())
        //    if (otp != "" && password1 != "") {
        //        if (otp == password1) {
        //            return true;
        //        }
        //        else {
                     
        //            $('#Asplbl').val(null);
        //            return false;

        //        }
        //    }
        //    else {
        //        return false;
        //    }
           
        //}
    </script>--%>
   
</head>
<body runat="server" style="background-color:#ddd">
    <div style="width:100vw; height:100vh;">
           <div id="Header" style="display:flex; align-items:center; padding:1rem; justify-content:flex-end;">
        <p style="font-size:25px; padding:2rem 2rem;"></p>
    </div>
         <div style=" height:70vh; display:flex; align-items:center; justify-content:center; background-color:#fff">
             <div style="text-align:center">
                 <asp:Image ID="Logo" runat="server" ImageUrl="~/Images/SDB_LOGO.png" />
                 <div id="loginErrorMsg" class="alert alert-error hide" runat="server"/>
                 <div > 
                     <form id="form1" runat="server">
                     <div style="padding:1rem">
                           <p style="display:inline-block; width:170px">One Time Password</p>
                          <asp:TextBox runat="server" Text="Enter OTP here" ID="Asplbl" TextMode="Password" AutoCompleteType="Disabled" ValidateRequestMode="Enabled" MaxLength="6" ></asp:TextBox>
                     </div>

                         <div style="display:flex; justify-content:center">
                               <p>
                           <asp:HiddenField ID="Passwordcheck" runat="server" />
                             </p>
                              <asp:Button ID="Button1" runat="server" OnClientClick="return FunOTPcheck()" onClick="Button1_Click"  Text="Submit" />
                             <asp:Button ID="Button3" runat="server" OnClick="Resend_otp" Text="Resend OTP" />
                         </div>
       <div id="resenddiv" runat="server" style=" margin-top:1rem">
              </div>
            
                         </form>
                 </div>

             </div>

         </div>
       
    </div>
 
   
</body>
</html>


