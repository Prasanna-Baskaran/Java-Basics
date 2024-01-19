<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="ViewEditCustDetailAll.aspx.cs" Inherits="AGS.SwitchOperations.ViewEditCustDetailAll" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <link href="Styles/DatePickerBootStrap.css" rel="stylesheet" />
    <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
    <script src="Scripts/bootstrap-DatePicker.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnAccDetails" runat="server" />
    <asp:HiddenField ID="hdnAlertAccList" runat="server" />
    <asp:HiddenField ID="hdnLinkedAccList" runat="server" />
    <asp:HiddenField ID="hdnCardDetails" runat="server" />
    <asp:HiddenField ID="hdnAllSelectedValues" runat="server" />

    <asp:HiddenField runat="server" ID="hdnTblAuthId" />
    <asp:HiddenField runat="server" ID="hdnCardNo" />
    <asp:HiddenField runat="server" ID="hdnCustomerID" />
    <asp:HiddenField runat="server" ID="hdnBankCustId" />
    <asp:HiddenField runat="server" ID="hdnacclinkData" />

    <asp:HiddenField runat="server" ID="hdnFlagId" />
    <asp:HiddenField runat="server" ID="hdnFormStatusID" />
    <asp:HiddenField runat="server" ID="hdnUserRoleID" />
    <asp:HiddenField runat="server" ID="hdnShowAuthButton" />
    <asp:HiddenField runat="server" ID="HiddenField1" />

    <asp:HiddenField runat="server" ID="hdnCurrTabID" />
    <asp:HiddenField runat="server" ID="hdnInstaEdit" />
    <%--added for ATPCM-759--%>

    <asp:Button runat="server" ID="hdnViewBtn" Text="Search" OnClick="btnView_Click" Style="display: none;" />



    <asp:Panel ID="pnlEnrolment" runat="server">

        <div class="row" id="SearchCustomer">
            <div class="">
                <div class="box-header with-border">
                    <i class="fa fa-list"></i>
                    <h3 class="box-title">View/Edit Customer Details</h3>
                    <div class="box-tools pull-right">
                    </div>
                </div>
            </div>
            <div class="box-body">
                <div class="form-horizontal">
                    <!--Display validation msg ------------------------------------------------------------------------->
                    <div class="pad margin no-print" id="ValidateMsgDiv" style="display: none">
                        <div class="callout callout-info" style="margin-bottom: 0!important;">
                            <h4><i class="fa fa-info"></i>Information :</h4>
                            <span id="SpnValidMsg" class="text-center"></span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Card No:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="SearchMobile" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Name:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="100" runat="server" name="CustomerName" id="txtSearchName" onkeypress="return onlyAlphabets(event,this);" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Mobile No:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="15" runat="server" name="SearchMobile" id="txtSearchMobile" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Account No:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="AccountNo" id="txtAccountNoView" />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">CIF No:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="cif" id="txtCIF" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputNIC" class="col-xs-4 control-label">NIC No:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="nic" id="txtNIC" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputNIC" class="col-xs-4 control-label">Name On Card:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="NameOnCard" id="txtsearchNameOnCard" />
                                </div>
                            </div>
                        </div>


                    </div>
                    <div class="row">
                        <div class="col-md-4"></div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <div class="col-sm-4"></div>
                                <div class="col-sm-4">
                                    <asp:Button runat="server" CssClass="btn btn-primary"
                                        ID="btnSearchCustomer" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunValidation()" />
                                </div>
                                <div class="col-sm-4"></div>
                            </div>
                        </div>
                        <div class="col-md-4"></div>
                    </div>
                </div>
                <%--//divresult--%>
                <div class="box-body">

                    <div class="row" id="DivResultMsg">
                        <div class="col-md-6">
                            <h4>
                                <label maxlength="20" runat="server" name="Name" id="LblResult" readonly="readonly" />
                            </h4>
                        </div>
                    </div>
                    <div class="row" id="divResult">


                        <div class="col-md-12" id="DivTable">
                            <div class="">
                                <!-- /.box-header -->
                                <div class="box-body no-padding">

                                    <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                        <div class="col-md-12">
                                            <div class="x_panel">
                                                <div>
                                                    <div id="SelectAllDiv">
                                                        <input id="select_all" type="checkbox" /><span>Select All</span>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                </div>
                                                <div class="x_content">
                                                    <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%">
                                                    </table>
                                                    <%--<div class="dataTables_paginate paging_simple_numbers" id="datatable-buttons_paginate"></div>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="box-body" id="DivCardResult">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="box-primary">
                                        <!-- /.box-header -->
                                        <!-- start sheetal for carddetails  -->
                                        <div class="box-body no-padding">
                                            <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                                <div class="col-md-12">
                                                    <div class="x_panel">
                                                        <div class="">
                                                            <div class="clearfix"></div>
                                                        </div>
                                                        <div class="x_content">
                                                            <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%">
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row" id="AuthDiv">
                            <div class="col-md-4"></div>
                            <div class="col-md-4">
                            </div>
                            <div class="col-md-4"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



        <div class="row" id="EnrolmentDiv" style="display: none">
            <div class="col-md-12">
                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                <div class="">
                    <div class="box-header with-border">
                        <i class="fa fa-list"></i>
                        <h3 class="box-title">Customer Information</h3>
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

                            <div class="row">
                                <div class="">
                                    <div class="col-md-12">
                                        <div class="tab-content">
                                            <!-- TAB1  -->
                                            <div id="PersonalInfo" class="tab-pane">
                                                <div class="">
                                                    <div class="box-header">
                                                        <h3 class="box-title">Card Details </h3>
                                                    </div>
                                                    <div class="form-horizontal">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">CIF ID:</label>
                                                                    <div class="col-sm-7">

                                                                        <input type="text" class="form-control" maxlength="100" placeholder="CIF ID"
                                                                            runat="server" name="CIFID" id="txtCifId"
                                                                            onkeypress="return FunChkAlphaNumeric(event)" />
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Customer Name:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" maxlength="100" placeholder="Customer Name"
                                                                            runat="server" name="CustomerName" id="txtCustomerName"
                                                                            onkeypress="return FunChkAlphaNumeric(event)" />
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Name On Card:</label>
                                                                    <div class="col-sm-7">

                                                                        <input type="text" class="form-control" maxlength="100" placeholder="Name on card"
                                                                            runat="server" name="NameOnCard" id="txtNameOnCard"
                                                                            onkeypress="return FunChkAlphaNumeric(event)" disabled="disabled" />

                                                                    </div>
                                                                </div>
                                                            </div>



                                                        </div>




                                                        <div class="row">
                                                            <%--                  <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Date of Birth:</label>
                                                                    <div class="col-sm-7">
                                                                     
                                                                        <input type="text" class="form-control" placeholder="DOB" runat="server" id="txtDOB"  maxlength="10" />
                                                                    </div>
                                                                </div>
                                                            </div>--%>

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Date of Birth:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control datepicker" runat="server" id="DOB" />
                                                                    </div>
                                                                </div>
                                                            </div>



                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Branch Code:</label>
                                                                    <div class="col-sm-7">

                                                                        <input type="text" class="form-control" maxlength="11" placeholder="Branch Code"
                                                                            runat="server" name="BranchCode" id="txtBranchCode"
                                                                            onkeypress="return FunChkAlphaNumeric(event)" />

                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Gender:</label>
                                                                    <div class="col-sm-7">
                                                                        <select id="dropGender" runat="server" class="form-control">
                                                                            <option>Male</option>
                                                                            <option>Female</option>
                                                                        </select>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                        </div>



                                                        <div class="row">

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>NIC No:</label>
                                                                    <div class="col-sm-7">

                                                                        <input type="text" class="form-control" maxlength="20" placeholder="NIC No"
                                                                            runat="server" name="AadharNo" id="TxtAadharNo"
                                                                            onkeypress="return FunChkAlphaNumeric(event)" />

                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Language Preference:</label>
                                                                    <div class="col-sm-7">
                                                                        <asp:DropDownList ID="DDLLanguage" runat="server" class="form-control">
                                                                        </asp:DropDownList>

                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Mothers Name:</label>
                                                                    <div class="col-sm-7">

                                                                        <input type="text" class="form-control" maxlength="100" placeholder="Mothers Maiden Name"
                                                                            runat="server" name="MotherName" id="txtMotherName"
                                                                            onkeypress="return FunChkAlphaNumeric(event)" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Card No:</label>
                                                                    <div class="col-sm-7">

                                                                        <input type="text" class="form-control" maxlength="16" placeholder="Card No"
                                                                            runat="server" name="MotherName" disabled="disabled" readonly="readonly" id="txtCardNo"/>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <%--                   <div class="row">

                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Account No:</label>
                                                                    <div class="col-sm-7">

                                                                        <input type="text" class="form-control" maxlength="20" placeholder="Account No."
                                                                            runat="server" name="AccountNo" id="txtAccountNo"
                                                                            onkeypress="return FunChkAlphaNumeric(event)" />
                                                                    </div>
                                                                </div>
                                                            </div>


                                                       
                                                        </div>--%>

                                                        <div>
                                                            <div class="box-header">
                                                                <h3 class="box-title">Account Linking</h3>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-sm-7">
                                                                <table id="datatable-buttonsAcclink" class="table table-striped table-bordered" style="width: 60%">
                                                                </table>
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

                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Address Line 1:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtAddress1" placeholder="Address Line 1" runat="server"
                                                                            maxlength="100" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Address Line 2:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtAddress2" placeholder="Address Line 2"
                                                                        maxlength="100" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Address Line 3:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtAddress3" runat="server" maxlength="100" placeholder="Address Line 3"
                                                                        onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>City:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtCity" runat="server" placeholder="City" onkeypress="return FunChkAlphaNumeric(event)" maxlength="100" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>State:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtState" runat="server" placeholder="State"
                                                                        maxlength="100" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Pincode:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtPincode" runat="server" placeholder="Pincode"
                                                                        maxlength="6" onkeypress="return FunChkIsNumber(event);" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Country:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtCountry" runat="server" placeholder="Country" minlength="3" maxlength="100" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Country Code:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtCountryCode" placeholder="Country Code"
                                                                        onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label">STD Code:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtStdCode" placeholder="STD Code"
                                                                        onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label"><%--<span style="color: red;">*</span>--%>Mobile No:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" runat="server" id="txtMobileNo" maxlength="15"
                                                                            placeholder="Mobile No."
                                                                            onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>

                                                                    <label for="inputName" class="col-xs-4 control-label">Email:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
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
                        </div>
                        <div class="box-footer footer_fixed">
                            <ul class="pager wizard">
                                <li class="previous first"><a href="#">First</a></li>
                                <li class="previous"><a href="#">Previous</a></li>
                                <li class="next last"><a href="#">Last</a></li>
                                <li class="next"><a href="#">Next</a></li>
                            </ul>

                            <%-- <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn-div btn-style btn btn-primary" Style="border: 0.5px solid #fff; border-radius: 4px; margin-top: -55px; display: block; margin-left: 50%; margin-right: auto;" />--%>

                            <div id="ButtonsDiv" style="display: none" class="">

                                <div class="">
                                    <%--<asp:Button ID="btnAccept" runat="server" Text="Accept" CssClass="btn btn-primary"
                                                OnClientClick="return funShowLoader();" />
                                            &nbsp;                      
                                        <input type="button" id="btnReject" value="Reject" class="btn btn-primary" />
                                            &nbsp;--%>

                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary" OnClientClick="return FunEditClick()" />
                                    <%--<asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />--%>
                                    <input type="button" class="btn btn-primary" id="btnUpdate" value="Update" style="float: right; margin-right: 10px; visibility: hidden" />

                                    <%--<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn-div btn-style btn btn-primary"
                                            Style="border: 0.5px solid #fff; border-radius: 4px; margin-top: -55px; margin-left: 50%; margin-right: auto;" />--%>

                                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                                    <%--OnClientClick="fnreset(null, true)" --%>
                                </div>

                                <div class="pull-right">
                                    <AGS:usrButtons runat="server" ID="userBtns" />
                                </div>
                            </div>
                        </div>

                    </div>
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
                        <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                        <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Customer Registration</h4>
                    </div>
                    <div class="modal-body" id="msgBody">
                        <label for="username" class="control-label" id="lblResult" style="font-weight: normal"></label>
                        <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                    </div>
                    <div class="modal-footer">
                        <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>


    </asp:Panel>

    <%--************************** SCRIPTS *************************--%>
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
        $(document).ready(function () {
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());

            $("#<%=hdnTransactionDetails.ClientID %>").val('');

            //if ($("#datatable-buttons tbody tr").length > 0) {
            //    $('#divResult').show();
            //}
            //else {
            //    $('#divResult').hide();
            //}



            if ($("#datatable-buttons tbody tr").length > 0) {
                //For Data Table
                var handleDataTableButtons = function () {
                    if ($("#datatable-buttons").length) {
                        $("#datatable-buttons").DataTable({
                            dom: "Bfrtip",

                            responsive: true
                        });
                    }
                };

                TableManageButtons = function () {
                    "use strict";
                    return {
                        init: function () {
                            handleDataTableButtons();
                        }
                    };
                }();
                TableManageButtons.init();
                $('#divResult').show();
            }
            else {
                $('#divResult').hide();
            }





            $('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true, endDate: new Date() });
            $('.datepickerFu').datepicker({ format: "dd/mm/yyyy", autoclose: true });

            $('#datatable-buttons tbody').find('input[type=checkbox][formstatus=2]').attr('disabled', true)
            $('#datatable-buttons tbody').find('input[type=checkbox][formstatus=1]').attr('disabled', true)

            if ($('#<%=hdnShowAuthButton.ClientID%>').val() == "1") {
                if ($("#datatable-buttons tbody tr").length > 0) {
                    $('#SelectAllDiv').show();
                    $('#AuthDiv').show();
                }
            }
            else {
                $('#SelectAllDiv').hide();
                $('#AuthDiv').hide();
            }

            // ************* Select All function *******
            $("#select_all").click(function (event) {

                //select all checkboxes
                if ($(this).is(":checked")) {
                    $('#datatable-buttons tbody input[type=checkbox]:not(:disabled)').prop("checked", 'true')
                }
                else {
                    $('#datatable-buttons tbody input[type=checkbox]').prop("checked", false)
                }
            });

            //$('#datatable-buttons tbody input[type=checkbox]').change(function () {

            //    if ($('#datatable-buttons tbody input[type=checkbox]').length == $('#datatable-buttons tbody input[type=checkbox]:checked').length) {
            //        $("#select_all").prop('checked', true);
            //    }
            //    else {
            //        $("#select_all").prop('checked', false);
            //    }
            //})


            $('#datatable-buttonsAcclink').html($("#<%=hdnAccDetails.ClientID %>").val());
            $("#<%=hdnAccDetails.ClientID %>").val('');
            //$(function () {
            //    $('[data-toggle="tooltip"]').tooltip()
            //});
            $('input[type=checkbox][formstatus=1]').attr('disabled', true)
            //check user has Edit rights

            //if ($("#datatable-buttonsAcclink tbody tr").length > 0) {

            //    var handleDataTableButtons = function () {
            //        if ($("#datatable-buttonsAcclink").length) {
            //            $("#datatable-buttonsAcclink").DataTable({
            //                "info": true,

            //                scrollX: true
            //            });
            //        }
            //    };

            //    TableManageButtons = function () {
            //        "use strict";
            //        return {
            //            init: function () {
            //                handleDataTableButtons();
            //            }
            //        };
            //    }();


            //    TableManageButtons.init();

            //}

            // ************* find Common Element function *******
            function findCommonElement(array1, array2) {

                // Loop for array1 
                for (let i = 0; i < array1.length; i++) {

                    // Loop for array2 
                    for (let j = 0; j < array2.length; j++) {

                        // Compare the element of each and 
                        // every element from both of the 
                        // arrays 
                        if (array1[i] === array2[j]) {

                            $("#<%=hdnAlertAccList.ClientID%>").val(array1[i]);
                            // Return if common element found 
                            return true;
                        }
                    }
                }

                // Return if no common element exist 
                return false;
            }

            // ************* find Uncommon Element function *******
            function findUncommonElement(array1, array2) {

                // Loop for array1 
                for (let i = 0; i < array1.length; i++) {

                    // Loop for array2 
                    for (let j = 0; j < array2.length; j++) {

                        // Compare the element of each and 
                        // every element from both of the 
                        // arrays 
                        if (array1[i] === array2[j]) {
                        }
                        else {

                            $("#<%=hdnAlertAccList.ClientID%>").val(array1[i]);
                            // Return if common element found 
                            return true;
                        }
                    }
                }

                // Return if no common element exist 
                return false;
            }


            // ************* Select All function *******
            $('[id$="select_all"]').click(function (event) {  //"select all" change 

                //select all checkboxes
                if ($(this).is(":checked")) {
                    $('#datatable-buttonsAcclink tbody input[type=checkbox]:not(:disabled)').prop("checked", 'true')
                }
                else {
                    $('#datatable-buttonsAcclink tbody input[type=checkbox]').prop("checked", false)
                }
            });


            $('#datatable-buttonsAcclink tbody input[type=checkbox]').change(function () {

                if ($('#datatable-buttonsAcclink tbody input[type=checkbox]').length == $('#datatable-buttonsAcclink tbody input[type=checkbox]:checked').length) {
                    $('[id$="select_all"]').prop('checked', true);
                }
                else {
                    $('[id$="select_all"]').prop('checked', false);
                }
            })


            function AccountLinkVal() {
                debugger;
                var allids = "", allqualifiers = "", allAcc = "";
                var LinkedAcc = $("#<%=hdnLinkedAccList.ClientID%>").val();
               var hdnAlertAccList = $("#<%=hdnAlertAccList.ClientID%>").val();
                $('#datatable-buttonsAcclink tbody').find('tr').each(function () {
                    if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                        var id = $(this).find('td').next().html() //+ "|" + $(this).find('td:eq(2)').text() + "|" + $(this).find('td:eq(3)').text();
                        var qualifier = $(this).find('td:last select')[0].value;
                        var accNo = $(this).find('td:nth-child(2)').text();
                        if (allids == "") {
                            allids = id + "|" + qualifier;
                        } else {
                            allids = allids + "," + id + "|" + qualifier;
                        }
                        if (allqualifiers == "") {
                            allqualifiers = qualifier;
                        } else {
                            allqualifiers = allqualifiers + "," + qualifier;
                        }
                        if (allAcc == "") {
                            allAcc = accNo;
                        } else {
                            allAcc = allAcc + "," + accNo;
                        }
                    }
                });

                if (allids == '') {
                    document.getElementById('phPageBody_LblMessage').innerHTML = 'Please check at least one checkbox to approve.'
                    FunShowMsg();
                    $('.shader').fadeOut();
                    return false;
                }
                else if (checkIfArrayIsUnique(allqualifiers.split(','))) {
                    document.getElementById('phPageBody_LblMessage').innerHTML = 'Please select different account qualifier.'
                    FunShowMsg();
                    $('.shader').fadeOut();
                    return false;
                }
                //else if (findCommonElement(allAcc.split(','), LinkedAcc.split(','))) {
                //    document.getElementById('phPageBody_LblMessage').innerHTML = 'Selected account ' + hdnAlertAccList + ' is already linked.'
                //    FunShowMsg();
                //    $('.shader').fadeOut();
                //    return false;
                //}
                else {
                    $('[id$="hdnAllSelectedValues"]').val(allids);
                }
                //FunShowMsg();
            }











        });
    </script>

    <script>

        function FunEditClick() {
   <%--         $("#<%=hdnFlagId.ClientID%>").val('2')--%>
            $("#<%=hdnFlagId.ClientID%>").val($("#<%=hdnInstaEdit.ClientID%>").val())


            $('[id$="btnUpdate"]').css("visibility", "visible")
            // $("#phPageBody_btnUpdate").css("display", "")
            $("#phPageBody_btnEdit").css("display", "none")
            $('#phPageBody_dropGender').attr("disabled", false);

            $('[id$="btnUpdate_U"]').css("display", "")
            //$('[id$="btnEdit_U"]').css("display", "none")

            $('#SearchCustomer').css("display", "none");
            $('#OtherCardDiv').css("display", "");
            $('#EnrolmentDiv').css("display", "");
            $('#ButtonsDiv').css("display", "");


            $('.datepicker').prop('readonly', false);
            //$('input[type="text"]').removeAttr('disabled');
            $('[id$="txtCifId"]').prop('readonly', false);
            $('[id$="txtCustomerName"]').prop('readonly', false);
            $('[id$="txtBranchCode"]').prop('readonly', false);
            $('[id$="TxtAadharNo"]').prop('readonly', false);
            $('[id$="DOB"]').prop('readonly', false);
            $('[id$="txtNameOnCard"]').prop('readonly', false);
            $('[id$="txtAddress1"]').prop('readonly', false);
            $('[id$="txtAddress2"]').prop('readonly', false);
            $('[id$="txtAddress3"]').prop('readonly', false);
            $('[id$="txtCity"]').prop('readonly', false);
            $('[id$="txtState"]').prop('readonly', false);
            $('[id$="txtPincode"]').prop('readonly', false);
            $('[id$="txtCountry"]').prop('readonly', false);
            $('[id$="txtCountryCode"]').prop('readonly', false);
            $('[id$="txtStdCode"]').prop('readonly', false);
            $('[id$="MobileNo"]').prop('readonly', false);
            $('[id$="txtMotherName"]').prop('readonly', false);

            $("#<%=DDLLanguage.ClientID%>").prop('disabled', false);

            $('[id$="txtEmailId"]').prop('readonly', false);
            $('#id_div_acc *').prop('readonly', false);

            //hide accept and reject buttons
            if ($("#phPageBody_hdnFormStatusID").val() == "1") {
                //$("#phPageBody_btnAccept").css("display", "none")
                //$("#phPageBody_btnReject").css("display", "none")
                $('[id$="btnAccept_U"]').css("display", "none")
            }



            //FunOperation();
            return false;
        }

    </script>

    <script>
        //Validation on Search Button
        function FunValidation() {
            var CardNo = $('#<%=txtSearchCardNo.ClientID%>').val()
            var Mobile = $('#<%=txtSearchMobile.ClientID%>').val()
            var Name = $('#<%=txtSearchName.ClientID%>').val()
            var AccountNo = $('#<%=txtAccountNoView.ClientID%>').val()
            var CIFNo = $('#<%=txtCIF.ClientID%>').val()
            var NicNo = $('#<%=txtNIC.ClientID%>').val()
            var NameOnCard = $('#<%=txtsearchNameOnCard.ClientID%>').val()


            if ((CardNo == "") && (Mobile == "") && (AccountNo == "") && (Name == "") && (CIFNo == "") && (NicNo == "") && (NameOnCard == "")) {
                $('#SpnValidMsg').html('Please provide Card No /Mobile No /Name /Account No /CIFNo /NicNo /Name On Card ');
                $('#ValidateMsgDiv').show();
                $('#divResult').hide();
                return false;
            }

            //else if ((CIFNo == "") && (NicNo == "")) {
            //    $('#SpnValidMsg').html('Please provide either CIF No or NIC No ');
            //    $('#ValidateMsgDiv').show();
            //    $('#divResult').hide();
            //    return false;
            //}
            //else if ((CIFNo != "") && (NicNo != ""))
            //{
            //    $('#SpnValidMsg').html('Please provide either CIF No or NIC No ');
            //    $('#ValidateMsgDiv').show();
            //    $('#divResult').hide();
            //    return false;
            //}
            else {
                $('#errormsgDiv').hide();
                $('#divResult').show();
                $('.shader').fadeIn();
                return true;

            }

        }
    </script>

    <%-- ************************** Financial details *************** --%>
    <script>




        function checkIfArrayIsUnique(myArray) {
            for (var i = 0; i < myArray.length; i++) {
                for (var j = 0; j < myArray.length; j++) {
                    if (i != j) {
                        if (myArray[i] == myArray[j]) {
                            return true; // means there are duplicate values
                        }
                    }
                }
            }
            return false; // means there are no duplicate values.
        }
    </script>



    <%----************************* Address details ****************************** --%>
    <script>

        $('[id$="btnUpdate"]').click(function () {


            var errrorTextFID = 'Please set ';
            var errrorTextSectionFID = ' in Customer Details.';
            var errorFieldsAD = '';

            if ($('[id$="txtNameOnCard"]').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, Name on card</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>Name on card</b> ';
                }
                $('[id$="txtNameOnCard"]').focus();

            }

            if ($('[id$="txtMotherName"]').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, Mother Name</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>Mother Name</b> ';
                }
                $('#<%=txtMotherName.ClientID%>').focus();
            }

            var allids = "", allqualifiers = "", allAcc = "";
            var LinkedAcc = $("#<%=hdnLinkedAccList.ClientID%>").val();
            var hdnAlertAccList = $("#<%=hdnAlertAccList.ClientID%>").val();
            $('#datatable-buttonsAcclink tbody').find('tr').each(function () {
                /*if ($(this).find('input[type="checkbox"]:checked').length > 0)*/ {
                    var id = $(this).find("#txtAcNo").val();// $(this).find('td').next().value; //+ "|" + $(this).find('td:eq(2)').text() + "|" + $(this).find('td:eq(3)').text();
                    var qualifier = $(this).find('td:last select')[0].value;
                    var accNo = $(this).find('td:nth-child(2)').text();
                    if (id != "") {
                        if (allids == "") {
                            allids = id + "|" + qualifier;
                        } else {
                            allids = allids + "," + id + "|" + qualifier;
                        }
                    }
                    if (allqualifiers == "") {
                        allqualifiers = qualifier;
                    } else {
                        allqualifiers = allqualifiers + "," + qualifier;
                    }
                    if (allAcc == "") {
                        allAcc = accNo;
                    } else {
                        allAcc = allAcc + "," + accNo;
                    }
                }
            });

            if (allids == '' /*&& !$('#datatable-buttonsAcclink tbody').find('tr input[type="checkbox"]').prop('disabled')*/) {


                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, at least one Account no to Account Linking.</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>at least one Account no to Account Linking</b> ';
                }


                //document.getElementById('phPageBody_LblMessage').innerHTML = 'Please check at least one checkbox to approve.'
                //FunShowMsg();
                //$('.shader').fadeOut();
                //return false;
            }
            else if (checkIfArrayIsUnique(allqualifiers.split(','))) {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, different account qualifier.</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>different account qualifier</b> ';
                }

                //document.getElementById('phPageBody_LblMessage').innerHTML = 'Please select different account qualifier.'
                //FunShowMsg();
                //$('.shader').fadeOut();
                //return false;
            }
            //else if (findCommonElement(allAcc.split(','), LinkedAcc.split(','))) {
            //    document.getElementById('phPageBody_LblMessage').innerHTML = 'Selected account ' + hdnAlertAccList + ' is already linked.'
            //    FunShowMsg();
            //    $('.shader').fadeOut();
            //    return false;
            //}
            else {
                $('[id$="hdnAllSelectedValues"]').val(allids);
            }









            if (errorFieldsAD != '') {
                $('#SpnErrorMsg').html(errrorTextFID + errorFieldsAD + errrorTextSectionFID);
                $('#errormsgDiv').show();
                $("#TabC1").click();

                errorFieldsAD = '';
                errrorTextSectionFID = '';
                errrorTextFID = '';
                //ErrorCount = 2;

                return false;
            }




            var errrorTextAD = 'Please provide ';
            var errrorTextSectionAD = ' in Address Details.';
            var errorFieldsAD = '';
            var eml = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;


            if ($('[id$="txtAddress1"]').val().trim() == '') {
                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, Address1</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>Address1</b> ';
                }
            }


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


            if ($('[id$="txtCity"]').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, City </b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>City</b> ';
                }
            }

            if ($('[id$="txtState"]').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, State</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>State</b> ';
                }
            }

            if ($('[id$="txtPincode"]').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, Pincode</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>Pincode</b> ';
                }
            }
            if ($('[id$="txtCountry"]').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, Country</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>Country</b> ';
                }
            }
            else if ($('[id$="txtCountry"]').val().trim().length < 3) {

                    if (errorFieldsAD != '') {
                        errorFieldsAD = errorFieldsAD + '<b>, valid Country (min 3 character)</b> ';
                    }
                    else {
                        errorFieldsAD = errorFieldsAD + '<b>valid Country (min 3 character)</b> ';
                    }
                }
            if ($('[id$="txtCountryCode"]').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, Country Code</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>Country Code</b> ';
                }
            }


            if ($('[id$="txtMobileNo"]').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, Mobile No</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>Mobile No</b> ';
                }
            }


            if (errorFieldsAD != '') {
                $('#SpnErrorMsg').html(errrorTextAD + errorFieldsAD + errrorTextSectionAD);
                $('#errormsgDiv').show();
                $("#TabC2").click();
                errorFieldsAD = '';
                errrorTextSectionAD = '';
                errrorTextAD = '';
                //ErrorCount = 4;
                return false;
            }



            $('#errormsgDiv').hide();
            var _RegistrationDet = {


                CardNo: $("[id$='txtCardNo']").val(),
                CIFID: $("[id$='txtCifId']").val(),
                CustomerName: $("[id$='txtCustomerName']").val(),
                CustomerPreferredName: $("[id$='txtNameOnCard']").val(),
                MothersMaidenName: $("[id$='txtMotherName']").val(),
                Address1: $("[id$='txtAddress1']").val(),
                Address2: $("[id$='txtAddress2']").val(),
                Address3: $("[id$='txtAddress3']").val(),
                City: $("[id$='txtCity']").val(),
                State: $("[id$='txtState']").val(),
                PinCode: $("[id$='txtPincode']").val(),
                Country: $("[id$='txtCountry']").val(),
                DOB: $("[id$='DOB']").val(),
                CountryDialcode: $("[id$='txtCountryCode']").val(),
                STDCode: $("[id$='txtStdCode']").val(),
                MobileNo: $("[id$='txtMobileNo']").val(),
                EmailId: $("[id$='txtEmailId']").val(),
                BranchCode: $("[id$='txtBranchCode']").val(),
                Aadhaar: $("[id$='TxtAadharNo']").val(),
                Gender: $("#<%=dropGender.ClientID %>").val(),
                CustLanguage: $("#<%=DDLLanguage.ClientID %> option:selected").text(),
                AuthId: $("#<%=hdnTblAuthId.ClientID %>").val(),
                AllSelectedValues: $("#<%=hdnAllSelectedValues.ClientID %>").val()

            }
            $('.shader').fadeIn();
            $.ajax({
                type: "POST",
                url: 'ViewEditCustDetailAll.aspx/btnUpdate',
                data: {},
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                data: JSON.stringify({ '_RegistrationDet': _RegistrationDet }),
                //data: "{Inputdata:" + JSON.stringify(Inputdata) + "}",
                success: function (data) {
                    var objdata = $.parseJSON(data.d);
                    if (objdata.split('|')[0] == '1') {
                        $('#lblResult').text(objdata.split('|')[1]);
                        $('input[type="text"]').val('');
                        $('#EnrolmentDiv').css("display", "none");
                        $('#SearchCustomer').css("display", "");
                        $('input[type="text"]').prop('readonly', false);

                    } else {

                        $('#lblResult').text(objdata.split('|')[1]);
                        //$('#SearchCustomer').css("display", "none");
                        //$('#EnrolmentDiv').css("display", "");
                        //$('#ButtonsDiv').css("display", "");
                        //$('#id_div_acc').css("display", "");


                     //   $('#datatable-buttonsAcclink').html($("#<%=hdnacclinkData.ClientID %>").val());


                        //$("#phPageBody_btnEdit").css("display", "none");
                    }
                    $('.shader').fadeOut();
                    $('#memberModal').modal('show');

                    //now iterate through this object's contents and load your gridview
                },
                failure: function (data) {
                    met
                    var objdata = $.parseJSON(data.d);
                    $('#lblResult').text("Customer details are not saved.");
                    $('.shader').fadeOut();

                    $('#memberModal').modal('show');
                    //$('#SearchCustomer').css("display", "none");
                    //$('#EnrolmentDiv').css("display", "");
                    //$('#ButtonsDiv').css("display", "");

                    //$("#phPageBody_btnEdit").css("display", "none");
                }

            });




        });


    </script>


    <%--//Message Box--%>
    <script>
        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');

            //on success
            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                $('input[type="text"]').val('');

            }
            //on update fail
            else if ($('#phPageBody_hdnResultStatus').val() == 2) {
                $('#SearchCustomer').css("display", "none");
                $('#EnrolmentDiv').css("display", "");
                $('#ButtonsDiv').css("display", "");
                //$('#id_div_acc').css("display", "");


                $('#datatable-buttonsAcclink').html($("#<%=hdnacclinkData.ClientID %>").val());


                $("#phPageBody_btnEdit").css("display", "none")
            }


        }
    </script>

    <script>
        //Reject Customer Click
        function FunReject() {

            $('#phPageBody_txtRejectReson').val('');
            $('#remarkDiv').css("display", "none");
            $('#phPageBody_Reject_Btn').css("display", "none");
            $('input[name=IsConfirm]').removeAttr('disabled');
            $('input[name=IsConfirm]').attr('checked', false);
            //$('input[name=IsConfirm]').val(0);
            $('#RejectConfirmationModal').modal('show');
            $('[id$="txtRejectReson"]').attr('required', false)
        }

        //Remove required field of reject reason
        function funCancelModal() {
            $('[id$="txtRejectReson"]').attr('required', false)
            $('#RejectConfirmationModal').modal('hide')
        }

        //remove required field of txtRejectReason
        function funRemoveRequired() {
            $('[id$="txtRejectReson"]').attr('required', false)
        }


        //Fun for checkbox validation 
        function funGetSelectedCustomers(FormstatusID) {
            if ($('#datatable-buttons tbody input[type=checkbox]:checked').length == 0) {
                $('#SpnValidMsg').html("Please select Card Requests to accept/reject");
                $('#ValidateMsgDiv').show();
                return false;
            }
            else {
                $('#SpnValidMsg').html('');
                $('#ValidateMsgDiv').hide();
                $("#<%=hdnFormStatusID.ClientID%>").val(FormstatusID);


                var ArrayIds = [];
                $('#datatable-buttons tbody input[type=checkbox]:checked').each(function (i) {
                    ArrayIds[i] = $(this).attr("custid");

                });
                //alert(val.join(","))

                $("#<%=hdnCustomerID.ClientID%>").val(ArrayIds.join(","))
                $("#<%=hdnBankCustId.ClientID%>").val(ArrayIds.join(","))

                //for Reject request
                if (FormstatusID == "2") {

                    $('[id$="txtRejectReson"]').attr('required', true)
                    $('#RejectConfirmationModal').modal('show');
                    $('input[name="IsConfirm"]').attr("checked", false)
                    $('#phPageBody_txtRejectReson').val('')
                    //$('input[name="IsConfirm"]').val(0)

                }
                else {
                    $('.shader').fadeIn();
                }

                return true;
            }
        }

    </script>


    <%-- Show Loader --%>
    <script>
        function funShowLoader() {
            $('.shader').fadeIn();
            return true;
        }
    </script>

    <script>
        function funViewClick(obj) {

            var CustomerID = $(obj).attr('custid');
            var AuthId = $(obj).attr('ID');
            var cardno = $(obj).attr('CardNo');

            $("[id$='hdnCustomerID']").val(CustomerID);
            $("[id$='hdnTblAuthId']").val(AuthId);
            $("[id$='hdnCardNo']").val(cardno);

            $('.shader').fadeIn();
            document.getElementById("phPageBody_hdnViewBtn").click();
        }
    </script>

    <script>
        function FunOperation() {

            // View Customer
            if ($("#phPageBody_hdnFlagId").val() == "1") {
                $('#SearchCustomer').css("display", "none");
                $('#EnrolmentDiv').css("display", "");
                $('#ButtonsDiv').css("display", "");
                $('input[type="text"]').prop('readonly', true);
                $('input[type="email"]').prop('readonly', true);
                $('#phPageBody_dropGender').attr("disabled", true);
                $("#<%=DDLLanguage.ClientID%>").prop('disabled', true);
                //$('[id$="DDLLanguage"]').prop('readonly', true);
                $('#id_div_acc *').prop('readonly', true);
                $("#phPageBody_btnUpdate").css("display", "none")
                $('[id$="btnUpdate_U"]').css("display", "none")
                //Readonly date textbox
                $("#<%=DOB.ClientID%>").removeClass("datepicker")
                $('#datatable-buttonsAcclink .form-control,#datatable-buttonsAcclink .checkbox').prop('readonly', true).attr("disabled", true);

            }
            //Edit Customer
            else if ($("#phPageBody_hdnFlagId").val() == "2") {

                $("#phPageBody_btnUpdate").css("display", "")
                $("#phPageBody_btnEdit").css("display", "none")
                $('#phPageBody_dropGender').attr("disabled", false);

                $('[id$="btnUpdate_U"]').css("display", "")
                //$('[id$="btnEdit_U"]').css("display", "none")

                $('#SearchCustomer').css("display", "none");
                $('#OtherCardDiv').css("display", "");
                $('#EnrolmentDiv').css("display", "");
                $('#ButtonsDiv').css("display", "");

                //$('input[type="text"]').removeAttr('disabled');

                $('input[type="email"]').prop('readonly', false);
                $('input[type="text"]:not(#<%=txtNameOnCard.ClientID%>)').prop('readonly', false);

                //$('[id$="txtCifId"]').prop('readonly', true);
                //$('[id$="DOB"]').prop('readonly', true);
                //$('[id$="txtAccountNo"]').prop('readonly', true);



                $("#<%=DOB.ClientID%>").addClass("datepicker")

                //$('[id$="txtRejectReson"]').attr('required', false)

                //$("#FileUploadDiv").css("display", "");
                //$("#SpnNote").css("display", "");

                // date texbox

                //hide accept and reject buttons
                if ($("#phPageBody_hdnFormStatusID").val() == "1") {
                    //$("#phPageBody_btnAccept").css("display", "none")
                    //$("#phPageBody_btnReject").css("display", "none")
                    $('[id$="btnAccept_U"]').css("display", "none")
                }
                //$('#' + $("#phPageBody_hdnCurrTabID").val() + '').click()
            }
            else if ($("#phPageBody_hdnFlagId").val() == "3") {

                $("#phPageBody_btnUpdate").css("display", "")
                $("#phPageBody_btnEdit").css("display", "none")
                $('#phPageBody_dropGender').attr("disabled", false);

                $('[id$="btnUpdate_U"]').css("display", "")
                //$('[id$="btnEdit_U"]').css("display", "none")

                $('#SearchCustomer').css("display", "none");
                $('#OtherCardDiv').css("display", "");
                $('#EnrolmentDiv').css("display", "");
                $('#ButtonsDiv').css("display", "");

                //$('input[type="text"]').removeAttr('disabled');

                $('input[type="email"]').prop('readonly', false);
                $('input[type="text"]').prop('readonly', false);

                //$('[id$="txtCifId"]').prop('readonly', true);
                //$('[id$="txtCIFCreationDate"]').prop('readonly', true);
                //$('[id$="DOB"]').prop('readonly', true);
                //$('[id$="txtAccountNo"]').prop('readonly', true);
                //$('[id$="txtForthLineEmbossing"]').prop('readonly', true);
                //$('[id$="tXtAccountType"]').prop('readonly', true);



                //$('[id$="txtRejectReson"]').attr('required', false)

                //$("#FileUploadDiv").css("display", "");
                //$("#SpnNote").css("display", "");

                // date texbox

                $("#<%=DOB.ClientID%>").addClass("datepicker")
                //hide accept and reject buttons
                if ($("#phPageBody_hdnFormStatusID").val() == "1") {
                    //$("#phPageBody_btnAccept").css("display", "none")
                    //$("#phPageBody_btnReject").css("display", "none")
                    $('[id$="btnAccept_U"]').css("display", "none")
                }
                //$('#' + $("#phPageBody_hdnCurrTabID").val() + '').click()
            }



        }
    </script>



</asp:Content>
