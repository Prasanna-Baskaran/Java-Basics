<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true"
    CodeBehind="Registration.aspx.cs" Inherits="AGS.SwitchOperations.Registration" %>


<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">

    <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
    <script src="Scripts/bootstrap-DatePicker.js"></script>
    <link href="Styles/DatePickerBootStrap.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">
    <div class="row">
        <div class="col-md-12">
            <asp:Panel ID="pnlEnrolment" runat="server"></asp:Panel>
            <div class="box-primary">
                <div class="box-header with-border">
                    <i class="fa fa-list"></i>
                    <h3 class="box-title">Customer Registration</h3>
                    <div class="box-tools pull-right">
                    </div>
                </div>
                <%--Progress START--%>
                <div class="box-body">
                    <!------------HIDDEN FIELDS-------------------------------->
                    <input type="hidden" name="ResultStatus" id="hdnResultStatus" runat="server" />

                    <div id="rootwizard" class="tabbable tabs-left">
                        <div id="bar" class="progress">
                            <div class="progress-bar progress-bar-success" id="divProgress" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>
                        </div>

                        <div class="navbar">
                            <div class="navbar-inner">
                                <div class="container">
                                    <ul class="nav nav-tabs">
                                        <li><a href="#PersonalInfo" data-toggle="tab" id="TabC1">Card Details</a></li>
                                        <li><a href="#CustAddress" data-toggle="tab" id="TabC2">Customer Address</a></li>
                                    </ul>
                                </div>
                            </div>
                        </div>


                        <!--Display validation msg ------------------------------------------------------------------------->
                        <div class="pad margin no-print" id="errormsgDiv" style="display: none">
                            <div class="callout callout-info" style="margin-bottom: 0!important;">
                                <h4><i class="fa fa-info"></i>Information :</h4>
                                <span id="SpnErrorMsg" class="text-center"></span>
                            </div>
                        </div>

                        <!--All Tabs section start here ----------------------------------------------------------------------------------------------------------------------------------------------->
                        <div class="box-primary">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="tab-content">
                                        <!-- TAB1  -->
                                        <div id="PersonalInfo" class="tab-pane">
                                            <div class="">
                                                <div class="box-header">
                                                    <h2 class="box-title">Card Details </h2>
                                                </div>
                                                <div class="form-horizontal">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>CIF ID:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" maxlength="16" runat="server" placeholder="CIF ID"
                                                                        name="CIFID" id="txtCifId" required="required"
                                                                        onkeypress="return FunChkIsNumber(event)" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Creation Date:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control datepicker1" placeholder="CIF Creation Date" runat="server" id="txtCIFCreationDate" maxlength="10"/>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Bin Prefix:</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList runat="server" ID="DdlBinPrefix" CssClass="form-control">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Customer Name:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" maxlength="100" placeholder="Customer Name"
                                                                        runat="server" name="CustomerName" id="txtCustomerName"
                                                                        onkeypress="return onlyAlphabets(event)" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Name On Card:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" maxlength="100" runat="server" name="NameOnCard" placeholder="Name On Card"
                                                                        id="txtNameOnCard" required="required" onkeypress="return onlyAlphabets(event)" />
                                                                </div>
                                                            </div>
                                                        </div>


                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Mothers Name:</label>
                                                                <div class="col-sm-7">

                                                                    <input type="text" class="form-control" maxlength="100" runat="server" name="txtMotherName" placeholder="Mothers Maiden Name"
                                                                        id="txtMotherName" onkeypress="return onlyAlphabets(event)" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">

                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Account No:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" runat="server"
                                                                        id="txtAccountNo" maxlength="16" onkeypress="return FunChkIsNumber(event)" required="required" placeholder="Account No." />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Account Type:</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList runat="server" ID="DdlAccountType" CssClass="form-control">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Date of Birth:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control datepicker" placeholder="Date of Birth" runat="server" id="DOB" maxlength="10" />
                                                                </div>
                                                            </div>
                                                        </div>  
                                                    </div>

                                                    <div class="row">
                                                            
                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Branch Code:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" runat="server" placeholder="Branch Code"
                                                                        id="txtBranchCode" onkeypress="return FunChkAlphaNumeric(event)" required="required" maxlength="4" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Pan Number:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" placeholder="Pan Number" runat="server" onkeypress="return FunChkAlphaNumeric(event)"
                                                                        id="txtPanNumber" maxlength="10" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Fourth Line Embossing:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" runat="server" placeholder="Fourth Line Embossing"
                                                                        id="txtForthLineEmbossing" onkeypress="return FunChkAlphaNumeric(event)"  maxlength="20" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">

                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Aadhar No:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" runat="server"
                                                                        id="TxtAadharNo" onkeypress="return FunChkAlphaNumeric(event)"  placeholder="Aadhar No" maxlength="12" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--//Customer Address Tab2--%>
                                        <div class="tab-pane" id="CustAddress">
                                            <div class="">
                                                <div class="box-header">
                                                    <h3 class="box-title">Customer Address</h3>
                                                </div>
                                                <div class="form-horizontal">
                                                    <div class="row">

                                                        <div class="col-md-6">
                                                            <div class="form-group">

                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Address 1:</label>

                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtAddress1" placeholder="Address 1" runat="server"
                                                                        maxlength="50" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Address 2:</label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="col-xs-4">
                                                                <input type="text" class="form-control" runat="server" id="txtAddress2" placeholder="Address 2"
                                                                    maxlength="50" onkeypress="return FunChkAlphaNumeric(event)" />
                                                            </div>

                                                            <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Address 3:</label>
                                                            <div class="col-xs-4">
                                                                <input type="text" class="form-control" id="txtAddress3" runat="server" maxlength="50" placeholder="Address 3"
                                                                    onkeypress="return FunChkAlphaNumeric(event)" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">

                                                        <div class="col-md-6">
                                                            <div class="form-group">

                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>City:</label>

                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtCity" runat="server" placeholder="City" onkeypress="return onlyAlphabets(event)" maxlength="40" />
                                                                </div>
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>State:</label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="col-xs-4">
                                                                <input type="text" class="form-control" id="txtState" runat="server" placeholder="State"
                                                                    maxlength="20" onkeypress="return onlyAlphabets(event)" />
                                                            </div>

                                                            <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Pincode:</label>
                                                            <div class="col-xs-4">
                                                                <input type="text" class="form-control" id="txtPincode" runat="server" placeholder="Pincode"
                                                                    maxlength="6" onkeypress="return FunChkIsNumber(event,this);" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">

                                                        <div class="col-md-6">
                                                            <div class="form-group">

                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Country:</label>

                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtCountry" runat="server"
                                                                        placeholder="Country" maxlength="100" onkeypress="return onlyAlphabets(event,this);" />
                                                                </div>
                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Country Code:</label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="col-xs-4">
                                                                <input type="text" class="form-control" runat="server" id="txtCountryCode" placeholder="Country Code"
                                                                    onkeypress="return FunChkIsNumber(event)" maxlength="4" />
                                                            </div>

                                                            <label for="inputName" class="col-xs-4 control-label">STD Code:</label>
                                                            <div class="col-xs-4">
                                                                <input type="text" class="form-control" runat="server" id="txtStdCode" placeholder="STD Code"
                                                                    onkeypress="return FunChkIsNumber(event)" maxlength="4" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">

                                                        <div class="col-md-6">
                                                            <div class="form-group">

                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Mobile No:</label>

                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtMobileNo" maxlength="10"
                                                                        placeholder="Mobile No."
                                                                        onkeypress="return FunChkIsNumber(event)" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Email:</label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">


                                                            <!-- end sheetal-->
                                                            <div class="col-xs-4">
                                                                <input type="email" class="form-control" placeholder="Email Id" runat="server" id="txtEmailId" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <ul class="pager wizard">
                            <li class="previous first"><a href="#">First</a></li>
                            <li class="previous"><a href="#">Previous</a></li>
                            <li class="next last"><a href="#">Last</a></li>
                            <li class="next"><a href="#">Next</a></li>
                        </ul>

                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn-div btn-style btn btn-primary"
                            Style="border: 0.5px solid #fff; border-radius: 4px; margin-top: -6px; margin-left: 94%; margin-right: auto;" />

                        <div class="pull-right">
                            <AGS:usrButtons runat="server" ID="userBtns" />
                        </div>
                    </div>

                </div>

                <div id="shader" class="shader">
                    <div class="loadingContainer">
                        <div id="divLoading3">
                            <div id="loader-wrapper">
                                <div id="loader"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Modal HTML -->
                <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
                    <div class="modal-dialog modal-md">
                        <div class="modal-content" style="border-radius: 6px">
                            <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                                <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="CancelModal();"><span aria-hidden="true">×</span></button>
                                <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Customer Registration</h4>
                            </div>
                            <div class="modal-body" id="msgBody">
                                <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                            </div>
                            <div class="modal-footer">
                                <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button" onclick="CancelModal();"><span aria-hidden="true">OK</span></button>
                            </div>
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
            </div>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            //$('.datepickerNepal').nepaliDatePicker();
            $('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true, endDate: new Date() });
  
            $('.datepicker1').datepicker({ format: "dd/mm/yyyy", autoclose: true, startDate: new Date() });
    
        });
    </script>
    <script>
        $(document).ready(function () {
            $('#divError').hide();
            $('#rootwizard').bootstrapWizard({
                'tabClass': 'nav nav-tabs',
                onTabShow: function (tab, navigation, index) {
                    var $total = navigation.find('li').length;
                    var $current = index + 1;
                    var $percent = ($current / $total) * 100;

                    $('#rootwizard .progress-bar').css({ width: $percent + '%' });
                    $('#rootwizard .progress-bar').text("Step " + (index + 1) + " of 2");

                    if (index <= 3) {
                        $('#divProgress').removeClass().addClass("progress-bar progress-bar-danger progress-bar-striped active");
                    }
                    if (index >= 4) {
                        $('#divProgress').removeClass().addClass("progress-bar progress-bar-warning progress-bar-striped active");
                    }
                    if (index >= 6) {
                        $('#divProgress').removeClass().addClass("progress-bar progress-bar-success progress-bar-striped active");
                    }
                    if (index >= 8) {
                        $('#divProgress').removeClass().addClass("progress-bar progress-bar-success progress-bar-striped active");
                    }
                }
            });
            window.prettyPrint && prettyPrint()
        });
    </script>

    <script>
        var errortab = '';
        //var errordesc = '';

        $("[id$='btnSubmit']").click(function () {
            
            var errrorTextPD = 'Please provide ';
            var errrorTextSectionPD = ' in Card Details.';
            var errorFieldsPD = '';

            if ($("[id$='txtCifId']").val() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {

                    errorFieldsPD = errorFieldsPD + '<b>, Cif Id</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Cif Id</b> ';
                }
            }
            //if (($("[id$='txtCIFCreationDate']").val() == '')) {
            //    errortab = '1';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, CIF Creation Date</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>CIF Creation Date</b> ';
            //    }

            //}
            if ($("[id$='txtCustomerName']").val() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Customer Name</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Customer Name</b> ';
                }
            }
            if ($("[id$='txtNameOnCard']").val() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Name On Card</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Name On Card</b> ';
                }
            }

            if ($("[id$='DdlBinPrefix']").val() == '0') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Bin</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Bin</b> ';
                }
            }

            //if ($("[id$='txtMotherName']").val() == '') {
            //    errortab = '1';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Mother Maiden Name</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Mother Maiden Name</b> ';
            //    }
            //}

            if ($("[id$='txtAccountNo']").val() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Account No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Account No</b> ';
                }
            }
            if ($("[id$='DdlAccountType']").val() == '0') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Account Type</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Account type</b> ';
                }
            }

            if ($("[id$='txtBranchCode']").val() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Branch Code</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Branch Code</b> ';
                }
            }

            //if ($("[id$='txtPanNumber']").val() == '') {
            //    errortab = '1';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Pan Number</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Pan Number</b> ';
            //    }
            //}
            //if ($("[id$='txtForthLineEmbossing']").val() == '') {
            //    errortab = '1';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Forth Line Embossing</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Forth Line Embossing</b> ';
            //    }
            //}
            //if ($("[id$='TxtAadharNo']").val() == '') {
            //    errortab = '1';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Aadhar No</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Aadhar No</b> ';
            //    }
            //}


            //errordesc = errrorTextPD + errorFieldsPD + errrorTextSectionPD;


            if (errorFieldsPD != '') {
                $('#SpnErrorMsg').html(errrorTextPD + errorFieldsPD + errrorTextSectionPD);
                $('#errormsgDiv').show();
                if (errortab == '1') {
                    $("#TabC1").click();
                    $('#<%=txtCifId.ClientID%>').focus();
                }

                errorFieldsPD = '';
                errrorTextSectionPD = '';
                errrorTextPD = '';
                //ErrorCount = 5
                return false;
            }

            //if ((ErrorCount == 0)) {
            //    $('.shader').show();
            //}




            errrorTextPD = 'Please provide ';
            errrorTextSectionPD = ' in Customer Address.';
            errorFieldsPD = '';

            if ($("[id$='txtAddress1']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {

                    errorFieldsPD = errorFieldsPD + '<b>, Address1</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Address1</b> ';
                }
            }

            //if ($("[id$='txtAddress2']").val() == '') {
            //    errortab = '2';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Address2</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Address2</b> ';
            //    }
            //}
            //if (($("[id$='txtAddress3']").val() == '')) {
            //    errortab = '2';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Address3</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Address3</b> ';
            //    }

            //}

            if ($("[id$='txtCity']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, City</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>City</b> ';
                }
            }

            //if ($("[id$='txtMotherName']").val() == '') {
            //    errortab = '2';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Mother Maiden Name</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Mother Maiden Name</b> ';
            //    }
            //}

            if ($("[id$='txtState']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, State</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>State</b> ';
                }
            }

            if ($("[id$='txtPincode']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Pincode</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Pincode</b> ';
                }
            }

            if ($("[id$='txtCountry']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Country</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Country</b> ';
                }
            }

            if ($("[id$='txtCountryCode']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Country Code</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Country Code</b> ';
                }
            }
            //if ($("[id$='txtStdCode']").val() == '') {
            //    errortab = '2';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Std Code</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Std Code</b> ';
            //    }
            //}
            if ($("[id$='txtMobileNo']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Mobile No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Mobile No</b> ';
                }
            }
            //if ($("[id$='txtEmailId']").val() == '') {
            //    errortab = '2';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Email Id</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Email Id</b> ';
            //    }
            //}


            if ($('[id$="txtEmailId"]').val().trim().length > 0) {
                if (eml.test($('[id$="txtEmailId"]').val().trim()) == false) {

                    $('[id$="txtEmailId"]').focus();
                    if (errorFieldsAD != '') {
                        errorFieldsAD = errorFieldsAD + '<b>, valid email Address</b> ';
                    }
                    else {
                        errorFieldsAD = errorFieldsAD + '<b> valid email address</b> ';
                    }
                }
            }



            if (errorFieldsPD != '') {
                $('#SpnErrorMsg').html(errrorTextPD + errorFieldsPD + errrorTextSectionPD);
                $('#errormsgDiv').show();
                if (errortab == '2') {
                    $("#TabC2").click();
                    $('#<%=txtAddress1.ClientID%>').focus();
                }

                errorFieldsPD = '';
                errrorTextSectionPD = '';
                errrorTextPD = '';
                //ErrorCount = 5
                return false;
            }

            //if ((ErrorCount == 0)) {
            //    $('.shader').show();
            //}








        });


     <%--   $("[id$='btnSubmit']").click(function () {

            var errrorTextPD = 'Please provide ';
            var errrorTextSectionPD = ' in Customer Address.';
            var errorFieldsPD = '';

            if ($("[id$='txtAddress1']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {

                    errorFieldsPD = errorFieldsPD + '<b>, Address1</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Address1</b> ';
                }
            }

            if ($("[id$='txtAddress2']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Address2</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Address2</b> ';
                }
            }
            //if (($("[id$='txtAddress3']").val() == '')) {
            //    errortab = '2';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Address3</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Address3</b> ';
            //    }

            //}

            if ($("[id$='txtCity']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, City</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>City</b> ';
                }
            }

            if ($("[id$='txtMotherName']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Mother Maiden Name</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Mother Maiden Name</b> ';
                }
            }

            if ($("[id$='txtState']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, State</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>State</b> ';
                }
            }

            if ($("[id$='txtPincode']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Pincode</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Pincode</b> ';
                }
            }

            if ($("[id$='txtCountry']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Country</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Country</b> ';
                }
            }

            if ($("[id$='txtCountryCode']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Country Code</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Country Code</b> ';
                }
            }
            //if ($("[id$='txtStdCode']").val() == '') {
            //    errortab = '2';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Std Code</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Std Code</b> ';
            //    }
            //}
            if ($("[id$='txtMobileNo']").val() == '') {
                errortab = '2';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Mobile No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Mobile No</b> ';
                }
            }
            //if ($("[id$='txtEmailId']").val() == '') {
            //    errortab = '2';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Email Id</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Email Id</b> ';
            //    }
            //}
            
            if (errorFieldsPD != '') {
                $('#SpnErrorMsg').html(errrorTextPD + errorFieldsPD + errrorTextSectionPD);
                $('#errormsgDiv').show();
                if (errortab == '2') {
                    $("#TabC2").click();
                    $('#<%=txtAddress1.ClientID%>').focus();
                }

                errorFieldsPD = '';
                errrorTextSectionPD = '';
                errrorTextPD = '';
                ErrorCount = 5
                return false;
            }

            if ((ErrorCount == 0)) {
                $('.shader').show();
            }
        });--%>

        $("#phPageBody_userBtns_btnSubmit_U").click(function () {


        });


    </script>


    <script>
        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');

            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                $('input[type="text"]').val('');
            }
        }

        <%--function CancelModal() {
                if ($('#phPageBody_hdnResultStatus').val() == 1) {
                    fnreset(null, true);
                    $("#<%=ddlINSTID.ClientID %>").val(0);
                $("#<%=DdlProduct.ClientID %>").val(0);
            }--%>
        //}

    </script>
</asp:Content>
