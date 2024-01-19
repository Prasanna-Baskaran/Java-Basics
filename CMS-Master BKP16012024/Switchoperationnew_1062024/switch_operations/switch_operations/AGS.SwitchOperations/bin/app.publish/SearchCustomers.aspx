<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="SearchCustomers.aspx.cs" Inherits="AGS.SwitchOperations.SearchCustomers" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">
    <link href="Styles/DatePickerBootStrap.css" rel="stylesheet" />
    <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
    <script src="Scripts/bootstrap-DatePicker.js"></script>

    <script>
        <%--//Maker Login
        if($('#<%=hdnUserRoleID.ClientID%>').val()=="3")
        {

        }
            //checker Login
        else if($('#<%=hdnUserRoleID.ClientID%>').val()=="4")
        {

        }--%>

</script>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">

    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />

    <asp:HiddenField ID="hdnCardDetails" runat="server" />


    <asp:HiddenField runat="server" ID="hdnCustomerID" />
    <asp:HiddenField runat="server" ID="hdnBankCustId" />

    <asp:HiddenField runat="server" ID="hdnFlagId" />
    <asp:HiddenField runat="server" ID="hdnFormStatusID" />
    <asp:HiddenField runat="server" ID="hdnUserRoleID" />
    <asp:HiddenField runat="server" ID="hdnShowAuthButton" />
    <!-- 069-->
    <!-- start sheetal to set hidden field for current tab-->
    <asp:HiddenField runat="server" ID="hdnCurrTabID" />

    <asp:Button runat="server" ID="hdnViewBtn" Text="Search" OnClick="btnView_Click" Style="display: none;" />



    <asp:Panel ID="pnlEnrolment" runat="server">

        <div class="row" id="SearchCustomer">
            <div class="">
                <div class="box-header with-border">
                    <i class="fa fa-list"></i>
                    <h3 class="box-title">Search Customer</h3>
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
                                <label for="inputName" class="col-xs-4 control-label">Application No:</label>
                                <div class="col-sm-8">
                                    <%--<asp:DropDownList runat="server" ID="ddlApplicationNo" CssClass="form-control" Style="display: none"></asp:DropDownList>--%>
                                    <input type="text" class="form-control" maxlength="17" runat="server" name="ApplNo" id="txtSearchApplNo" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Name:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="15" runat="server" name="CustomerName" id="txtSearchName" onkeypress="return onlyAlphabets(event,this);" />
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
                                <label for="inputName" class="col-xs-4 control-label">Date Of Birth:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control datepicker" maxlength="15" runat="server" name="DOB_AD" id="txtSearchDOB_AD" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Created Date:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control datepicker" maxlength="15" runat="server" name="CreatedDate" id="txtSearchCreatedDate" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label">Card No:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" name="SearchMobile" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>


                    </div>

                    <div class="row">
                        <div class="col-md-4"></div>
                        <div class="col-md-4"></div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <div class="col-sm-4"></div>
                                <div class="col-sm-4">
                                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnSearchCustomer" Text="Search" OnClick="btnSearch_Click" 
                                        OnClientClick="return FunValidation()" />
                                </div>
                                <div class="col-sm-4"></div>
                            </div>
                        </div>
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
                                <div class="col-md-4">
                                    <asp:Button ID="Button1" runat="server" Text="Accept" CssClass="btn-div btn-style btn btn-primary" OnClientClick="return funGetSelectedCustomers(1)" OnClick="btnAccept_Click" />
                                </div>
                                <div class="col-md-4">
                                    <%--<asp:Button ID="Button2" runat="server" Text="Reject" CssClass="btn-div btn-style btn btn-primary" OnClientClick="return funGetResult(2)" OnClick="AcceptRejectCardOpsRequests" />--%>
                                    <input type="button" value="Reject" onclick="return funGetSelectedCustomers(2)" id="BtnRejectCustomers" class="btn-div btn-style btn btn-primary" />
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn-div btn-style btn btn-primary" OnClientClick="fnreset(null, true)" OnClick="Page_Load" />
                                </div>
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
                                            <li><a href="#PersonalInfo" data-toggle="tab" id="TabC1">Personal Info</a></li>
                                            <li><a href="#CustAddress" data-toggle="tab" id="TabC2">Address</a></li>
                                            <li><a href="#FamilyDtl" data-toggle="tab" id="TabC3">Family Details</a></li>
                                            <li><a href="#OccupationDtl" data-toggle="tab" id="TabC4">Occupational Data</a></li>
                                            <li><a href="#FinalcialDtl" data-toggle="tab" id="TabC5">Financial Info</a></li>
                                            <li><a href="#CardStatement" data-toggle="tab" id="TabC6">Card Details</a></li>
                                            <li><a href="#RefferenceDiv" data-toggle="tab" id="TabC7">References</a></li>
                                            <li><a href="#DocumentationDiv" data-toggle="tab" id="TabC8">Documentation</a></li>
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
                                                        <h2 class="box-title">PERSONAL INFORMATION OF THE APPLICANT </h2>
                                                    </div>
                                                    <div class="form-horizontal">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>First Name:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" maxlength="15" runat="server" name="FirstName" 
                                                                            id="txtFirstName" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Middle Name:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" maxlength="15" runat="server" name="MiddleName" id="txtMiddleName" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Last Name:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" maxlength="15" runat="server" name="LastName" id="txtLastName" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Date of Birth:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control datepicker" runat="server" id="DOB_AD" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Mobile No:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" runat="server" id="txtMobileNo" maxlength="12" onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Nationality:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" maxlength="15" runat="server" id="txtNationality" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Sex:</label>
                                                                    <div class="col-sm-7">
                                                                        <asp:DropDownList runat="server" ID="Gender" CssClass="form-control">
                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Male" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="Female" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="Other" Value="3"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Marital Status:</label>
                                                                    <div class="col-sm-7">
                                                                        <asp:DropDownList runat="server" ID="MaritalStatus" CssClass="form-control">
                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Single" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="Married" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="Other" Value="3"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <%--<div class="col-md-4">
                                                        <div class="form-group">
                                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Identification No:</label>
                                                            <div class="col-sm-7">
                                                                <input type="text" class="form-control"  runat="server" id="txtIdentificationNo" maxlength="20" />
                                                            </div>
                                                        </div>
                                                    </div>--%>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Passport No/Identification No:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" runat="server" id="txtPassportNo" maxlength="20" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Issue Date:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control datepicker" placeholder="" runat="server" id="txtIssuedate" maxlength="10" />
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
                                                        <h3 class="box-title">Permanent Address Details</h3>
                                                    </div>
                                                    <div class="form-horizontal">
                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">P.O. Box:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtPO_Box_P" runat="server" maxlength="15" onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>House No:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtHouseNo_P" maxlength="15" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>
                                                                <%-- start sheetal--%>
                                                                <%-- Make House No,Street Name,City,District,Email  text boxes mandatory Change By Sheetal--%>

                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Street Name:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtStreetName_P" runat="server" maxlength="15" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">Tole:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtTole_P" runat="server" maxlength="15" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">Ward No:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtWardNo_P" runat="server" maxlength="15" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>City:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtCity_P" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>District:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtDistrict_P" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">Phone1:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtPhone1_P" onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label">Phone2:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtPhone2_P" onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">FAX:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" runat="server" id="txtFAX_P" maxlength="15" onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">Mobile:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtMobile_P" maxlength="12" onkeypress="return FunChkIsNumber(event)" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Email:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="email" class="form-control" runat="server" id="txtEmail_P" />
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>

                                                    <div class="box-header">
                                                        <h3 class="box-title">Correspondence Address Details</h3>
                                                    </div>
                                                    <div class="form-horizontal" id="CorresAddrsDiv">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-5 control-label"><span style="color: red;">*</span>Is Same As Permanent Address:</label>
                                                                    <div class="col-xs-6">
                                                                        <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="IsSameAsPermAddrs">
                                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                        </asp:RadioButtonList>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">P.O. Box:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtPO_Box_C" runat="server" maxlength="15" onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">House No:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtHouseNo_C" maxlength="15" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label">Street Name:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtStreetName_C" runat="server" maxlength="15" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">Tole:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtTole_C" runat="server" maxlength="15" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">Ward No:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtWardNo_C" runat="server" maxlength="15" onkeypress="return FunChkAlphaNumeric(event)" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label">City:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtCity_C" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">District:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtDistrict_C" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">Phone1:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtPhone1_C" onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label">Phone2:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtPhone2_C" onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">FAX:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" runat="server" id="txtFAX_C" maxlength="15" onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">Mobile:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtMobile_C" maxlength="12" onkeypress="return FunChkIsNumber(event)" />
                                                                </div>

                                                                <label for="inputName" class="col-xs-4 control-label">Email:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="email" class="form-control" runat="server" id="txtEmail_C" />
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>

                                                </div>
                                            </div>
                                            <%--//Family Details--%>
                                            <div class="tab-pane" id="FamilyDtl">
                                                <div class="">
                                                    <div class="box-header">
                                                        <h3 class="box-title">Family Details</h3>
                                                    </div>
                                                    <div class="form-horizontal">

                                                        <div class="row">
                                                            <%--1st ROW--%>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-sm-3 control-label">Spouse Name :</label>
                                                                    <div class="col-sm-8">
                                                                        <input type="text" class="form-control" id="txtSpouseName" runat="server" maxlength="25" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-3 control-label">Mother's Name :</label>
                                                                    <div class="col-sm-8">
                                                                        <input type="text" class="form-control" id="txtMotherName" runat="server" maxlength="25" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <%--2nd ROW--%>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputEmail" class="col-sm-3 control-label">Father's Name :</label>
                                                                    <div class="col-sm-8">
                                                                        <input type="text" class="form-control" id="txtFatherName" runat="server" maxlength="25" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-3 control-label">Grandfather's Name :</label>
                                                                    <div class="col-sm-8">
                                                                        <input type="text" class="form-control" id="txtGrandFatherName" maxlength="25" runat="server" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                            <%--//Occupation detail--%>
                                            <div class="tab-pane" id="OccupationDtl">
                                                <div class="">
                                                    <div class="box-header">
                                                        <h3 class="box-title">Occupation Details</h3>
                                                    </div>
                                                    <div class="form-horizontal">
                                                        <div class="row">
                                                            <%--1st ROW--%>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-sm-4 control-label">Occupation:</label>
                                                                    <div class="col-sm-7">
                                                                        <asp:DropDownList runat="server" ID="DdlProffessionType" CssClass="form-control">
                                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                            <asp:ListItem Value="1">Salaried</asp:ListItem>
                                                                            <asp:ListItem Value="2">Self Employed</asp:ListItem>
                                                                            <asp:ListItem Value="3">Retired</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-sm-4 control-label">Work for:</label>
                                                                    <div class="col-sm-7">
                                                                        <asp:DropDownList runat="server" ID="ddlOrgzTypeID" CssClass="form-control">
                                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                            <asp:ListItem Value="1">Govt.</asp:ListItem>
                                                                            <asp:ListItem Value="2">Public Companies</asp:ListItem>
                                                                            <asp:ListItem Value="3">Private Business</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <%--2nd ROW--%>
                                                            <div class="col-md-6" id="ProffesiontypeDiv">
                                                                <div class="form-group">
                                                                    <label for="inputEmail" class="col-sm-4 control-label">Profession:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" placeholder="" id="txtProffesion" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6" style="display: none" id="PreviousEmpDiv">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Previous Employment:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" placeholder="" id="txtPreviousEmployment" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <%--3rd ROW --%>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputEmail" class="col-sm-4 control-label">Designation :</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" placeholder="" id="txtDesignation" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Nature of Business:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" placeholder="" id="txtBusinessType" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Employed/Business Since:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control datepicker" placeholder="" runat="server" id="txtWorkSince" />
                                                                    </div>
                                                                </div>
                                                            </div>


                                                        </div>
                                                    </div>

                                                    <div class="box-header">
                                                        <h3 class="box-title">Employer / Business Address</h3>
                                                    </div>
                                                    <div class="form-horizontal">
                                                        <div class="row">

                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">P.O. Box:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtPO_Box_O" runat="server" maxlength="15" onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">Street Name:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">

                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtStreetName_O" runat="server" maxlength="15" />
                                                                </div>
                                                                <label for="inputName" class="col-xs-4 control-label">City:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" id="txtCity_O" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">District:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" id="txtDistrict_O" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">Phone1:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">

                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtPhone1_O" onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                </div>
                                                                <label for="inputName" class="col-xs-4 control-label">Phone2:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtPhone2_O" onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <div class="form-group">

                                                                    <label for="inputName" class="col-xs-4 control-label">FAX:</label>

                                                                    <div class="col-xs-4">
                                                                        <input type="text" class="form-control" runat="server" id="txtFAX_O" maxlength="15" onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>
                                                                    <label for="inputName" class="col-xs-4 control-label">Mobile:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">

                                                                <div class="col-xs-4">
                                                                    <input type="text" class="form-control" runat="server" id="txtMobile_O" maxlength="12" onkeypress="return FunChkIsNumber(event)" />
                                                                </div>
                                                                <label for="inputName" class="col-xs-4 control-label">Email:</label>
                                                                <div class="col-xs-4">
                                                                    <input type="email" class="form-control" runat="server" id="txtEmail_O" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--//Financial Details--%>
                                            <div class="tab-pane" id="FinalcialDtl">
                                                <div class="">
                                                    <div class="box-header">
                                                        <h3 class="box-title">Financial Details</h3>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <table class="table table-striped table-bordered">
                                                                <tr>
                                                                    <th>Income</th>
                                                                    <th>Amount</th>
                                                                </tr>
                                                                <tr>
                                                                    <!-- start sheetal to call function on onkeyup to autocalculate the salary -->
                                                                    <td>Annual Salary</td>
                                                                    <td>
                                                                        <input type="text" class="form-control" id="txtAnnualSal" runat="server" maxlength="15" onkeyup="add();" onkeypress="return FunChkIsNumber(event)" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Annual Bonus/Incentive</td>
                                                                    <td>
                                                                        <input type="text" class="form-control" id="txtAnnualIncentive" runat="server" maxlength="15" onkeyup="add();" onkeypress="return FunChkIsNumber(event)" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Annual Business Income</td>
                                                                    <td>
                                                                        <input type="text" class="form-control" runat="server" id="txtAnnualIncome" maxlength="15" onkeyup="add();" onkeypress="return FunChkIsNumber(event)" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Rental Income</td>
                                                                    <td>
                                                                        <input type="text" class="form-control" runat="server" id="txtRentalIncome" maxlength="15" onkeyup="add();" onkeypress="return FunChkIsNumber(event)" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Agriculture</td>
                                                                    <td>
                                                                        <input type="text" class="form-control" runat="server" id="txtAgriculture" maxlength="15" onkeyup="add();" onkeypress="return FunChkIsNumber(event)" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Income (specify)</td>
                                                                    <td>
                                                                        <input type="text" class="form-control" runat="server" id="txtIncome" maxlength="15" onkeyup="add();" onkeypress="return FunChkIsNumber(event)" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Total Annual Income</td>
                                                                    <td>
                                                                        <input type="text" class="form-control" runat="server" id="txtTotalIncome" maxlength="15" onkeypress="return FunChkIsNumber(event)" /></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>

                                                    <div class="form-horizontal">
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-6 control-label">Name of your Principal Bank:</label>
                                                                    <div class="col-sm-6">
                                                                        <input type="text" class="form-control" runat="server" id="txtPrincipleBankName" maxlength="30" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputEmail" class="col-sm-6 control-label">Account with PRABHU :</label>
                                                                    <div class="col-sm-5">
                                                                        <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="IsPrabhuAccount">
                                                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="row" id="PhrabhuBankDtlDiv">
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-6 control-label">Account No:</label>
                                                                    <div class="col-sm-6">
                                                                        <input type="text" class="form-control" runat="server" id="txtAccountNo" maxlength="20" onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-6 control-label">Branch Code:</label>
                                                                    <div class="col-sm-6">
                                                                        <input type="text" class="form-control" runat="server" id="txtPrabhuBranch" maxlength="20" onkeypress="return FunChkIsNumber(event)" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-6 control-label">Account Type:</label>
                                                                    <div class="col-sm-6">
                                                                        <asp:DropDownList runat="server" ID="DdlAccountType" CssClass="form-control">
                                                                            <%--   <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Savings" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="Current" Value="2"></asp:ListItem>--%>
                                                                            <%-- <asp:ListItem Text="Fixed" Value="3"></asp:ListItem>
                                                                            <asp:ListItem Text="Loan" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Text="Others" Value="5"></asp:ListItem>--%>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <%--//Card Statement--%>
                                            <div class="tab-pane" id="CardStatement">
                                                <div class="">
                                                    <div class="box-header">
                                                        <h3 class="box-title">Card Details</h3>
                                                    </div>
                                                    <div class="form-horizontal">
                                                        <div class="row" style="display: none">
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-6 control-label"><span style="color: red;">*</span>Card Type:</label>
                                                                    <div class="col-sm-5">
                                                                        <asp:DropDownList ID="DdlCardType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-6 control-label"><span style="color: red;">*</span>Institution ID:</label>
                                                                    <div class="col-sm-5">
                                                                        <asp:DropDownList ID="ddlINSTID" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                        </div>


                                                        <div class="row" style="display: none">

                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Product:</label>
                                                                    <div class="col-sm-8">
                                                                        <asp:DropDownList ID="DdlProduct" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Name On Card:</label>
                                                                    <div class="col-sm-8">
                                                                        <input type="text" class="form-control" id="txtNameOnCard" runat="server" maxlength="22" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row" style="display: none">
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Branch:</label>
                                                                    <div class="col-sm-8">
                                                                        <asp:DropDownList ID="DdlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">BIN:</label>
                                                                    <div class="col-sm-8">
                                                                        <asp:DropDownList ID="DdlBin" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="row" style="display: none">
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Statement Delivery:</label>
                                                                    <div class="col-sm-8">
                                                                        <asp:DropDownList runat="server" ID="StatementBy" CssClass="form-control">
                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="Collect" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="Email" Value="2"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <!--start sheetal new div added contains lable and textfield -->
                                                            <div id="DivtohideEmail">
                                                                <div class="col-md-6">
                                                                    <div class="form-group">
                                                                        <label for="inputName" class="col-xs-4 control-label">Email:</label>
                                                                        <div class="col-sm-8">
                                                                            <input type="email" class="form-control" id="txtEmailForStatement" runat="server" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>

                                                    
                                                     <table id="datatable-carddetails" class="table table-striped table-bordered" style="width: 100%">
                                                     </table>
                                                </div>
                                            </div>

                                            <%--//Refference--%>
                                            <div class="tab-pane" id="RefferenceDiv">
                                                <div class="">
                                                    <div class="box-header">
                                                        <h2 class="box-title">Reference 1 </h2>
                                                    </div>

                                                    <div class="form-horizontal">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Name:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" id="txtRefName1" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Designation:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" id="txtDesignation1_R" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Phone:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" id="txtPhone1_R" runat="server" onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="box-header">
                                                        <h2 class="box-title">Reference 2 </h2>
                                                    </div>

                                                    <div class="form-horizontal">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Name:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" id="txtRefName2" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Designation:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" id="txtDesignation2_R" runat="server" maxlength="15" onkeypress="return onlyAlphabets(event,this);" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Phone:</label>
                                                                    <div class="col-sm-7">
                                                                        <input type="text" class="form-control" id="txtPhone2_R" runat="server" onkeypress="return FunChkIsNumber(event)" maxlength="12" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <%--//Documentation--%>
                                            <div class="tab-pane" id="DocumentationDiv">
                                                <div class="">
                                                    <div class="box-header">
                                                        <h2 class="box-title">Documentation </h2>
                                                    </div>

                                                    <div class="form-horizontal">
                                                        <div class="row" id="HeadingDiv">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" id="lblPhoto" class="col-xs-4 control-label">Photo:</label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label for="inputName" id="lblSignature" class="col-xs-5 control-label">Signature:</label>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <label for="inputName" id="lblIdproof" class="col-xs-5 control-label">ID-Proof:</label>
                                                            </div>
                                                        </div>
                                                        <div class="row" id="FileUploadDiv">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label for="inputName" class="col-xs-4 control-label">Upload Photo:</label>
                                                                    <div class="col-sm-7">
                                                                        <asp:FileUpload runat="server" ID="PhotoUpload" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label for="inputName" class="col-xs-5 control-label">Upload Signature:</label>
                                                                <div class="col-sm-6">
                                                                    <asp:FileUpload runat="server" ID="SignatureUpload" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <label for="inputName" class="col-xs-5 control-label">Upload ID-Proof:</label>
                                                                <div class="col-sm-6">
                                                                    <asp:FileUpload runat="server" ID="IDProofUpload" Style="width: 200px;" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <div class="col-sm-4"></div>
                                                                    <div class="col-sm-6">
                                                                        <img id="PhotoImg" runat="server" style="width: 100px; height: 100px" align="left" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <div class="col-sm-5"></div>
                                                                    <div class="col-sm-6">
                                                                        <img id="SignatureImg" runat="server" style="width: 100px; height: 100px" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <div class="col-sm-5"></div>
                                                                    <div class="col-sm-6">
                                                                        <img id="IDProofImg" runat="server" style="width: 100px; height: 100px" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>


                                                        <div class="row">

                                                            <div class="col-md-11">
                                                                <div class="form-group">
                                                                    <span id="SpnNote" style="margin-left: 28px; color: red">*Uploading file Should be [.jpg, .jpeg , .gif , .png].File Size should not be more than 100 KB </span>
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
                                    <div class="" style="display: none">
                                        <div class="">
                                            <asp:Button ID="btnAccept" runat="server" Text="Accept" CssClass="btn btn-primary  " OnClick="btnAccept_Click" 
                                                OnClientClick="return funShowLoader();" />
                                            &nbsp;                      
                                        <input type="button" id="btnReject" value="Reject" class="btn btn-primary" onclick="FunReject();" />
                                            &nbsp;<asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
                                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" />

                                            &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="Page_Load" OnClientClick="fnreset(null, true)" />
                                        </div>
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
                        <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                    </div>
                    <div class="modal-footer">
                        <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                    </div>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>

        <!-- /.modal-content for Reject  card limit -->

        <div id="RejectConfirmationModal" class="modal fade in" data-keyboard="false" data-backdrop="static" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content -->
                <div class="modal-content" style="border-radius: 4px;">

                    <div class="modal-header">
                        <h4 id="myLargeModaldelete" class="modal-title" style="font-weight: bold">Confirmation</h4>
                    </div>
                    <div class="modal-body">
                        <div>
                            <div class="row">
                                <div class="col-sm-6"><b>Do you want to reject Customer?</b></div>
                                <div class="col-sm-5" id="radioBtnDiv">

                                    <input type='radio' name='IsConfirm' value='1' />Yes
                                &nbsp;&nbsp;<input type='radio' name='IsConfirm' value='2' aria-label="Close" data-dismiss="modal" onclick="funCancelModal()" />No

                                </div>
                            </div>

                            <div class="row" style="border-bottom: 1px solid #f7f3f5; padding: 5px; display: none" id="remarkDiv">
                                <div class="col-sm-6"><b>Reason</b><span style="color: red;">*</span></div>
                                <div class="col-sm-5">

                                    <asp:TextBox runat="server" CssClass="form-control" ID="txtRejectReson" TextMode="MultiLine" MaxLength="50" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-sm-6">

                                    <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" onclick="funCancelModal()" style="margin-right: 10px;" type="button"><span aria-hidden="true">CANCEL</span></button>

                                </div>
                                <div class="col-sm-5">
                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Confirm" ID="Reject_Btn" OnClick="btnReject_Click" OnClientClick="return funShowLoader();" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </asp:Panel>

    <%--************************** SCRIPTS *************************--%>
    <script>
        $(document).ready(function () {
            $('#phPageBody_txtTotalIncome').prop('readonly', true)
            $('[id$="DdlAccountType"]').attr('disabled', 'disabled');

            $('#phPageBody_txtAccountNo').prop("readonly", true)

            if ($('input[id*=IsSameAsPermAddrs]:checked').val() == "1") {
                $('#CorresAddrsDiv input[type="text"]').prop("readonly", true)
                $('#CorresAddrsDiv input[id="phPageBody_txtEmail_C"]').prop("readonly", true)
            }
            else {
                $('#CorresAddrsDiv input[type="text"]').prop("readonly", false)
                $('#CorresAddrsDiv input[id="phPageBody_txtEmail_C"]').prop("readonly", false)
            }

            $('#divError').hide();
            $('#rootwizard').bootstrapWizard({
                'tabClass': 'nav nav-tabs',
                onTabShow: function (tab, navigation, index) {
                    var $total = navigation.find('li').length;
                    var $current = index + 1;
                    var $percent = ($current / $total) * 100;

                    $('#rootwizard .progress-bar').css({ width: $percent + '%' });
                    $('#rootwizard .progress-bar').text("Step " + (index + 1) + " of 8");

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

            <%--if ($('#<%=hdnTransactionDetails.ClientID %>').val() == '')
            {
                //$('#divResult').hide();
            }--%>

            $("#<%=hdnTransactionDetails.ClientID %>").val('');

            if ($("#datatable-buttons tbody tr").length > 0) {
                $('#divResult').show();
            }
            else {
                $('#divResult').hide();
            }

            $('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true, endDate: new Date() });

            //hide formstatusid column
            //$("#datatable-buttons tr :nth-child(6)").hide()



            //Start 21/02/17
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
            $("#select_all").click(function (event) {  //"select all" change 

                //select all checkboxes
                if ($(this).is(":checked")) {
                    $('#datatable-buttons tbody input[type=checkbox]:not(:disabled)').prop("checked", 'true')
                }
                else {
                    $('#datatable-buttons tbody input[type=checkbox]').prop("checked", false)
                }
            });

            $('#datatable-buttons tbody input[type=checkbox]').change(function () {

                if ($('#datatable-buttons tbody input[type=checkbox]').length == $('#datatable-buttons tbody input[type=checkbox]:checked').length) {
                    $("#select_all").prop('checked', true);
                }
                else {
                    $("#select_all").prop('checked', false);
                }
            })
            //End 21/02/17


        });
    </script>

    <script>

</script>

    <script>
        $(document).ready(function () {
            $('#TabC6').click(function () {
                //start sheetal for carddetails table
                    $('#datatable-carddetails').html($("[id$='hdnCardDetails']").val());
                    if ($("#datatable-carddetails tbody tr").length > 0) {
                        //For Data Table
                        var handleDataTableCardDetails = function () {
                            if ($("#datatable-carddetails").length) {
                                $("#datatable-carddetails").DataTable({
                                    dom: "Bfrtip",
                                    responsive: true
                                });
                            }
                        };
                        TableManageCardDetails = function () {
                            "use strict";
                            return {
                                init: function () {
                                    handleDataTableCardDetails();
                                }
                            };
                        }();
                        TableManageCardDetails.init();
                    }
            });
        });
    </script>
    <%--//Click Function--%>
    <script>

        <%--//User control  buttons click--%>
                $(document).ready(function () {

                    usrbtn.btnAddClick = function () {

                    }

                    usrbtn.btnEditClick = function () {

                        //alert('You pressed EDIT');
                        $('.shader').fadeIn();
                        $("#phPageBody_hdnCurrTabID").val($('a[aria-expanded="true"]').attr('id'));
                    }

                    usrbtn.btnRejectClick = function () {

                        FunReject();
                        return false;

                    }


                });
                var ErrorCount = 0;
                $("[id$='IsSameAsPermAddrs']").click(function () {
                    //alert('1')
                    //if ($('input[name=IsSameAsPermAddrs]:checked').val() == "1") {
                    if ($("[id*=IsSameAsPermAddrs] input:checked").val() == "1") {
                        //   alert('1');

                        $('#phPageBody_txtPO_Box_C').val($('#phPageBody_txtPO_Box_P').val())
                        $('#phPageBody_txtHouseNo_C').val($('#phPageBody_txtHouseNo_P').val())
                        $('#phPageBody_txtStreetName_C').val($('#phPageBody_txtStreetName_P').val())
                        $('#phPageBody_txtTole_C').val($('#phPageBody_txtTole_P').val())
                        $('#phPageBody_txtWardNo_C').val($('#phPageBody_txtWardNo_P').val())
                        $('#phPageBody_txtCity_C').val($('#phPageBody_txtCity_P').val())
                        $('#phPageBody_txtDistrict_C').val($('#phPageBody_txtDistrict_P').val())
                        $('#phPageBody_txtPhone1_C').val($('#phPageBody_txtPhone1_P').val())
                        $('#phPageBody_txtPhone2_C').val($('#phPageBody_txtPhone2_P').val())
                        $('#phPageBody_txtFAX_C').val($('#phPageBody_txtFAX_P').val())
                        $('#phPageBody_txtMobile_C').val($('#phPageBody_txtMobile_P').val())
                        $('#phPageBody_txtEmail_C').val($('#phPageBody_txtEmail_P').val())
                        $('#phPageBody_txtFAX_C').val($('#phPageBody_txtFAX_P').val())
                        $('#phPageBody_txtFAX_C').val($('#phPageBody_txtFAX_P').val())
                        $('#CorresAddrsDiv input[type="text"]').prop("readonly", true)
                        $('#CorresAddrsDiv input[id="phPageBody_txtEmail_C"]').prop("readonly", true)
                    }
                    else {
                        // alert('2');

                        $('#phPageBody_txtPO_Box_C').val('')
                        $('#phPageBody_txtHouseNo_C').val('')
                        $('#phPageBody_txtStreetName_C').val('')
                        $('#phPageBody_txtTole_C').val('')
                        $('#phPageBody_txtWardNo_C').val('')
                        $('#phPageBody_txtCity_C').val('')
                        $('#phPageBody_txtDistrict_C').val('')
                        $('#phPageBody_txtPhone1_C').val('')
                        $('#phPageBody_txtPhone2_C').val('')
                        $('#phPageBody_txtFAX_C').val('')
                        $('#phPageBody_txtMobile_C').val('')
                        $('#phPageBody_txtEmail_C').val('')
                        $('#phPageBody_txtFAX_C').val('')
                        $('#phPageBody_txtFAX_C').val('')
                        $('#CorresAddrsDiv input[type="text"]').prop("readonly", false)
                        $('#CorresAddrsDiv input[id="phPageBody_txtEmail_C"]').prop("readonly", false)
                    }
                })
    </script>
    <script>
        //Validation on Search Button
        function FunValidation() {

            var AppNo = $('#<%=txtSearchApplNo.ClientID%>').val()
            var Mobile = $('#<%=txtSearchMobile.ClientID%>').val()
            var DOB = $('#<%=txtSearchDOB_AD.ClientID%>').val()
            var Name = $('#<%=txtSearchName.ClientID%>').val()
            var CreatedDate = $('#<%=txtSearchCreatedDate.ClientID%>').val()
            var CardNo = $('#<%=txtSearchCardNo.ClientID%>').val()



            if ((AppNo == "") && (Mobile == "") && (DOB == "") && (Name == "") && (CreatedDate == "") && (CardNo == "")) {
                $('#SpnValidMsg').html('Please provide Application No /Mobile No /Date of Birth/Name/Created Date /Card No ');
                $('#ValidateMsgDiv').show();
                $('#divResult').hide();
                return false;
            }

            else {
                $('#errormsgDiv').hide();
                $('#divResult').show();
                //$('input[type="text"]').val('');
                //$('#phPageBody_txtAddress').val('');
                // showLoader();
                $('.shader').fadeIn();
                return true;

            }

        }
    </script>

    <script>
        function funViewClick(obj) {
            //var tds = $(obj).parent().parent().find('td');
            //var CustomerID = $(tds[0]).text();
            //var formStatusid = $(tds[5]).text();
            var CustomerID = $(obj).attr('custid');
            var BankCustomerId = $(obj).attr('BankCustId');

            var formStatusid = $(obj).attr('formstatusid');
            $('[id$="txtRejectReson"]').attr('required', false)

            $("[id$='hdnCustomerID']").val(CustomerID);
            $("[id$='hdnBankCustId']").val(BankCustomerId);

            $('.shader').fadeIn();
            document.getElementById("phPageBody_hdnViewBtn").click();


        }
    </script>
    <script>
        function FunOperation() {

            //View Customer
            if ($("#phPageBody_hdnFlagId").val() == "1") {

                $('#SearchCustomer').css("display", "none");
                $('#OtherCardDiv').css("display", "none");
                $('#EnrolmentDiv').css("display", "");
                $('#ButtonsDiv').css("display", "");
                $('input[type="text"]').prop('readonly', true);
                $('input[type="email"]').prop('readonly', true);
                $('[id$="VehicleType"]').attr('disabled', 'disabled');
                $('[id$="DdlProffessionType"]').attr('disabled', 'disabled');
                $('[id$="DdlCardType"]').attr('disabled', 'disabled');
                $('[id$="ddlINSTID"]').attr('disabled', 'disabled');
                $('[id$="DdlProduct"]').attr('disabled', 'disabled');
                $('[id$="Gender"]').attr('disabled', 'disabled');
                $('[id$="MaritalStatus"]').attr('disabled', 'disabled');
                $('[id$="ddlOrgzTypeID"]').attr('disabled', 'disabled');
                $('[id$="DdlAccountType"]').attr('disabled', 'disabled');
                $('[id$="StatementBy"]').attr('disabled', 'disabled');
                $('input[type="radio"]').attr('disabled', 'disabled');

                if ($("#phPageBody_hdnFormStatusID").val() == "1") {
                    $("#phPageBody_btnAccept").css("display", "none")
                    $("#phPageBody_btnReject").css("display", "none")
                    $('[id$="btnAccept_U"]').css("display", "none")
                }
                //bug 067
                // start sheetal condition to hide user define reject button
                if ($("#phPageBody_hdnFormStatusID").val() == "2") {
                    $("#phPageBody_btnAccept").css("display", "none")
                    $("#phPageBody_btnReject").css("display", "none")
                    $('[id$="btnReject_U"]').css("display", "none")
                }


                $('[id$="txtRejectReson"]').attr('required', false)
                $("#phPageBody_btnUpdate").css("display", "none")
                $('[id$="btnUpdate_U"]').css("display", "none")
                $("#FileUploadDiv").css("display", "none");
                $("#SpnNote").css("display", "none");

                //Readonly date textbox
                $("#<%=DOB_AD.ClientID%>").removeClass("datepicker")
                $("#<%=txtIssuedate.ClientID%>").removeClass("datepicker")
                $("#<%=txtWorkSince.ClientID%>").removeClass("datepicker")
                //start diksha
                $("#HeadingDiv").show()

            }
            //Edit Customer
            else if ($("#phPageBody_hdnFlagId").val() == "2") {
                if ($("#phPageBody_hdnFormStatusID").val() == "1") {
                    $("#phPageBody_btnAccept").css("display", "none")
                    $("#phPageBody_btnReject").css("display", "none")
                    $('[id$="btnAccept_U"]').css("display", "none")
                }
                //start diksha
                $("#HeadingDiv").hide()
                // start sheetal condition to hide user define reject button
                if ($("#phPageBody_hdnFormStatusID").val() == "2") {
                    $("#phPageBody_btnAccept").css("display", "none")
                    $("#phPageBody_btnReject").css("display", "none")
                    $('[id$="btnReject_U"]').css("display", "none")
                }
                //$("#phPageBody_btnUpdate").css("display", "")
                //$("#phPageBody_btnEdit").css("display", "none")

                $('[id$="btnUpdate_U"]').css("display", "")
                $('[id$="btnEdit_U"]').css("display", "none")
                $('#SearchCustomer').css("display", "none");
                $('#OtherCardDiv').css("display", "");
                $('#EnrolmentDiv').css("display", "");
                $('#ButtonsDiv').css("display", "");
                $('input[type="text"]').removeAttr('disabled');
                $('input[type="email"]').prop('readonly', false);
                $('[id$="VehicleType"]').removeAttr("disabled");
                $('[id$="VehicleType"]').AutoPostBack = true;
                $('[id$="DdlProffessionType"]').removeAttr('disabled');
                $('[id$="DdlCardType"]').removeAttr('disabled');
                $('[id$="DdlProduct"]').removeAttr('disabled');
                $('[id$="ddlINSTID"]').removeAttr('disabled');
                $('input[type="radio"]').removeAttr('disabled');
                //$('[id$="txtFirstName"]').attr('required', true)
                //$('[id$="txtMiddleName"]').attr('required', true)
                //$('[id$="txtLastName"]').attr('required', true)
                //$('[id$="txtNationality"]').attr('required', true)
                //$('[id$="txtPassportNo"]').attr('required', true)
                //$('[id$="txtMobileNo"]').attr('required', true)

                $('[id$="txtRejectReson"]').attr('required', false)

                $("#FileUploadDiv").css("display", "");
                $("#SpnNote").css("display", "");

                // date texbox
                $("#<%=DOB_AD.ClientID%>").addClass("datepicker")
                $("#<%=txtIssuedate.ClientID%>").addClass("datepicker")
                $("#<%=txtWorkSince.ClientID%>").addClass("datepicker")

                //hide accept and reject buttons
                if ($("#phPageBody_hdnFormStatusID").val() == "1") {
                    //$("#phPageBody_btnAccept").css("display", "none")
                    //$("#phPageBody_btnReject").css("display", "none")
                    $('[id$="btnAccept_U"]').css("display", "none")
                }
                $('#' + $("#phPageBody_hdnCurrTabID").val() + '').click()



                if ($('input[id*=IsSameAsPermAddrs]:checked').val() == "1") {
                    $('#CorresAddrsDiv input[type="text"]').prop("readonly", true)
                    $('#CorresAddrsDiv input[id="phPageBody_txtEmail_C"]').prop("readonly", true)
                }
                else {
                    $('#CorresAddrsDiv input[type="text"]').prop("readonly", false)
                    $('#CorresAddrsDiv input[id="phPageBody_txtEmail_C"]').prop("readonly", false)
                }


            }

        }
    </script>

    <%--//Validation Msg--%>

    <%--******************************* Documentation *************************** --%>
    <script>
        //start sheetal 
        //change the id of submit button as #phPageBody_userBtns_btnUpdate_U"
        $("#phPageBody_userBtns_btnUpdate_U").click(function () {
            var errrorTextDD = 'Please provide ';
            var errrorTextSectionDD = ' in Documentation.';
            var errorFieldsDD = '';

            //if ($('#phPageBody_PhotoImg').prop('src') == '') {
            //    if ($('#phPageBody_PhotoUpload').val() != '') {
            //        var Photoext = $('#phPageBody_PhotoUpload').val().split('.').pop().toLowerCase();
            //        if (($.inArray(Photoext, ['gif', 'png', 'jpg', 'jpeg']) == -1)) {

            //            errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Photo</b> ';
            //        }
            //        else if ((phPageBody_PhotoUpload.files[0].size / 1024) > 100) {

            //            errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Photo (Size exceed)</b> ';
            //        }
            //    }
            //    else {
            //        errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Photo</b> ';
            //    }
            //}
            //else
            if ($('#phPageBody_PhotoUpload').val() != '') {
                var Photoext = $('#phPageBody_PhotoUpload').val().split('.').pop().toLowerCase();
                if (($.inArray(Photoext, ['gif', 'png', 'jpg', 'jpeg']) == -1)) {

                    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Photo</b> ';
                }
                else if ((phPageBody_PhotoUpload.files[0].size / 1024) > 100) {

                    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Photo (Size exceed)</b> ';
                }
            }


            //if ($('#phPageBody_SignatureImg').prop('src') == '') {
            //    if ($('#phPageBody_SignatureUpload').val() != '') {
            //        var Sigext = $('#phPageBody_SignatureUpload').val().split('.').pop().toLowerCase();
            //        if ($.inArray(Sigext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            //            errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Signature</b> ';
            //        }
            //        else if ((phPageBody_SignatureUpload.files[0].size / 1024) > 100) {

            //            errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Signature (Size exceeds)</b> ';
            //        }
            //    }
            //    else {
            //        errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Signature</b> ';
            //    }
            //}
            //else
            if ($('#phPageBody_SignatureUpload').val() != '') {
                var Sigext = $('#phPageBody_SignatureUpload').val().split('.').pop().toLowerCase();
                if ($.inArray(Sigext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Signature</b> ';
                }
                else if ((phPageBody_SignatureUpload.files[0].size / 1024) > 100) {
                    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid Signature (Size exceeds)</b> ';
                }
            }

            //if ($('#phPageBody_IDProofImg').prop('src') == '') {
            //    if ($('#phPageBody_IDProofUpload').val() != '') {
            //        var Sigext = $('#phPageBody_IDProofUpload').val().split('.').pop().toLowerCase();
            //        if ($.inArray(Sigext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            //            errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid ID Proof</b> ';
            //        }
            //        else if ((phPageBody_IDProofUpload.files[0].size / 1024) > 100) {

            //            errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid ID Proof (Size exceeds)</b> ';
            //        }
            //    }
            //    else {
            //        errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid ID Proof</b> ';
            //    }
            //}
            //else
            if ($('#phPageBody_IDProofUpload').val() != '') {
                var Sigext = $('#phPageBody_IDProofUpload').val().split('.').pop().toLowerCase();
                if ($.inArray(Sigext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid ID Proof</b> ';
                }
                else if ((phPageBody_IDProofUpload.files[0].size / 1024) > 100) {
                    errorFieldsDD = errorFieldsDD + (errorFieldsDD != '' ? '<b>,' : '<b>') + ' valid ID Proof (Size exceeds)</b> ';
                }
            }

            if (errorFieldsDD != '') {
                $('#SpnErrorMsg').html(errrorTextDD + errorFieldsDD + errrorTextSectionDD);
                $('#errormsgDiv').show();
                $("#TabC8").click();
                errorFieldsDD = '';
                errrorTextSectionDD = '';
                errrorTextDD = '';
                ErrorCount = 1;
                return false;
            }
        });
    </script>

    <%-- ************************** Financial details *************** --%>
    <script>
        $("#phPageBody_userBtns_btnUpdate_U").click(function () {
            var errrorTextFID = 'Please provide ';
            var errrorTextSectionFID = ' in Financial Details.';
            var errorFieldsFID = '';
            //for Debit system
            if (<%=HttpContext.Current.Session["SystemID"]%>=="2") {
                if ($('#phPageBody_DdlAccountType') == 0) {
                    if (errorFieldsFID != '') {
                        errorFieldsFID = errorFieldsFID + '<b>, Account type</b> ';
                    }
                    else {
                        errorFieldsFID = errorFieldsFID + '<b>Account type</b> ';
                    }

                }
                if ($('#<%=txtAccountNo.ClientID%>').val().trim() == '') {
                    if (errorFieldsFID != '') {
                        errorFieldsFID = errorFieldsFID + '<b>, Account No</b> ';
                    }
                    else {
                        errorFieldsFID = errorFieldsFID + '<b>Account No</b> ';
                    }
                }
            }
            if (errorFieldsFID != '') {
                $('#SpnErrorMsg').html(errrorTextFID + errorFieldsFID + errrorTextSectionFID);
                $('#errormsgDiv').show();
                $("#TabC5").click();

                errorFieldsFID = '';
                errrorTextSectionFID = '';
                errrorTextFID = '';
                ErrorCount = 2;
                return false;
            }

        });
    </script>
    <%----****************************** Statement *******************************--%>

    <script>
        //start sheetal
        $("#phPageBody_userBtns_btnUpdate_U").click(function () {

            var errrorTextSD = 'Please provide ';
            var errrorTextSectionSD = ' in Card Details .';
            var errorFieldsSD = '';
            var eml = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

         <%--   if ($("#phPageBody_DdlCardType").val() == "0") {
                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Card Type</b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Card Type</b> ';
                }
                $('#<%=DdlCardType.ClientID%>').focus();
            }--%>

            <%--if ($("#phPageBody_DdlProduct").val() == "0") {
                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Product</b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Product</b> ';
                }
                $('#<%=DdlProduct.ClientID%>').focus();
            }--%>

          <%--  if ($("#phPageBody_ddlINSTID").val() == "0") {
                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Institution ID</b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Institution ID</b> ';
                }
                $('#<%=ddlINSTID.ClientID%>').focus();
            }--%>

           <%-- if ($("#<%=txtNameOnCard.ClientID %>").val() == "") {
                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Name On Card</b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Name On Card</b> ';
                }
                $("#<%=txtNameOnCard.ClientID %>").focus();

            }--%>


            //if ($('input[id*=StatementBy]:checked').length == 0) {
<%--            if ($("#phPageBody_StatementBy").val() == "0") {

                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Statement delivery </b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Statement delivery </b> ';
                }
                $('#<%=StatementBy.ClientID%>').focus();

            }--%>
            <%--else if ($("#phPageBody_StatementBy").val() == "2") {
                if (eml.test($.trim($("#<%=txtEmailForStatement.ClientID %>").val())) == false) {
                    $("#<%=txtEmailForStatement.ClientID %>").focus();
                    if (errorFieldsSD != '') {
                        errorFieldsSD = errorFieldsSD + '<b>, valid email Address.</b> ';
                    }
                    else {
                        errorFieldsSD = errorFieldsSD + '<b> valid email address.</b> ';
                    }
                }
            }--%>

            if (errorFieldsSD != '') {
                $('#SpnErrorMsg').html(errrorTextSD + errorFieldsSD + errrorTextSectionSD);
                $('#errormsgDiv').show();
                $("#TabC6").click();

                errorFieldsSD = '';
                errrorTextSectionSD = '';
                errrorTextSD = '';
                ErrorCount = 3;
                return false;
            }
        });
    </script>


    <%----************************* Address details ****************************** --%>
    <script>
        //start sheetal
        $("#phPageBody_userBtns_btnUpdate_U").click(function () {
            var errrorTextAD = 'Please provide ';
            var errrorTextSectionAD = ' in Address Details.';
            var errorFieldsAD = '';
            var eml = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;


            if ($('#<%=txtHouseNo_P.ClientID%>').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, House No</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>House No</b> ';
                }
                $('#<%=txtHouseNo_P.ClientID%>').focus();

            }

            if ($('#<%=txtStreetName_P.ClientID%>').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, Street Name</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>Street Name</b> ';
                }
                $('#<%=txtStreetName_P.ClientID%>').focus();
            }

            if ($('#<%=txtCity_P.ClientID%>').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, City Name</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>City Name</b> ';
                }

            }
            if ($('#<%=txtDistrict_P.ClientID%>').val().trim() == '') {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, District Name</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>District Name</b> ';
                }
            }
            if ($('#<%=txtEmail_P.ClientID%>').val().trim() == '') {

                $("#<%=txtEmail_P.ClientID %>").focus();
                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, e-Mail</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>e-Mail</b> ';
                }
            }

            else if (eml.test($.trim($("#<%=txtEmail_P.ClientID %>").val())) == false) {

                $("#<%=txtEmail_P.ClientID %>").focus();
                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, valid email Address</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b> valid email address</b> ';
                }
            }
            if ($('input[id*=IsSameAsPermAddrs]:checked').length == 0) {
                //if ($("#phPageBody_IsSameAsPermAddrs").is(":not(':checked')") == true) {

                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, Correspondence Address</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>Correspondance Address</b> ';
                }
            }
            //start sheetal
            //to give validation when click on radio button as yes and clear any fields use else instead of else if
            //else if ($('input[id*=IsSameAsPermAddrs]:checked').val() == "0") {
            else {
                if ($('#<%=txtHouseNo_C.ClientID%>').val().trim() == '') {

                    if (errorFieldsAD != '') {
                        errorFieldsAD = errorFieldsAD + '<b>, House No </b> ';
                    }
                    else {
                        errorFieldsAD = errorFieldsAD + '<b>House No</b> ';
                    }
                }

                if ($('#<%=txtStreetName_C.ClientID%>').val().trim() == '') {

                    if (errorFieldsAD != '') {
                        errorFieldsAD = errorFieldsAD + '<b>, Street Name</b> ';
                    }
                    else {
                        errorFieldsAD = errorFieldsAD + '<b>Street Name</b> ';
                    }
                }

                if ($('#<%=txtCity_C.ClientID%>').val().trim() == '') {

                    if (errorFieldsAD != '') {
                        errorFieldsAD = errorFieldsAD + '<b>, City Name</b> ';
                    }
                    else {
                        errorFieldsAD = errorFieldsAD + '<b>City Name</b> ';
                    }
                }
                if ($('#<%=txtDistrict_C.ClientID%>').val().trim() == '') {

                    if (errorFieldsAD != '') {
                        errorFieldsAD = errorFieldsAD + '<b>, District Name</b> ';
                    }
                    else {
                        errorFieldsAD = errorFieldsAD + '<b>District Name</b> ';
                    }
                }

                if (eml.test($.trim($("#<%=txtEmail_C.ClientID %>").val())) == false) {

                    $("#<%=txtEmail_C.ClientID %>").focus();
                    if (errorFieldsAD != '') {
                        errorFieldsAD = errorFieldsAD + '<b>, valid email Address.</b> ';
                    }
                    else {
                        errorFieldsAD = errorFieldsAD + '<b> valid email address.</b> ';
                    }
                }
            }

            if (errorFieldsAD != '') {
                $('#SpnErrorMsg').html(errrorTextAD + errorFieldsAD + errrorTextSectionAD);
                $('#errormsgDiv').show();
                $("#TabC2").click();
                errorFieldsAD = '';
                errrorTextSectionAD = '';
                errrorTextAD = '';
                ErrorCount = 4;
                return false;
            }

        });


    </script>


    <%--  *******************************Personal details Validation **********************************--%>
    <script>
        var errortab = '';
        //start sheetal
        $("#phPageBody_userBtns_btnUpdate_U").click(function () {
            var errrorTextPD = 'Please provide ';
            var errrorTextSectionPD = ' in Personal Info.';
            var errorFieldsPD = '';



            if ($('#<%=txtFirstName.ClientID%>').val().trim() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {

                    errorFieldsPD = errorFieldsPD + '<b>, First Name</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>First Name</b> ';
                }
            }

           <%-- if ($('#<%=txtMiddleName.ClientID%>').val().trim() == '') {

                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Middle Name</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Middle Name</b> ';
                }
            }--%>

            if ($('#<%=txtLastName.ClientID%>').val().trim() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Last Name</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Last Name</b> ';
                }
            }
            if (($('#<%=DOB_AD.ClientID%>').val().trim() == '')) {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Date Of Birth</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Date Of Birth</b> ';
                }

            }

            if (($('#<%=txtMobileNo.ClientID%>').val().length != '10')) {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, valid Mobile No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b> valid Mobile No</b> ';
                }
            }

            if ($('#<%=txtNationality.ClientID%>').val().trim() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Nationality</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Nationality</b> ';
                }
            }
            if ($("#phPageBody_Gender").val() == "0") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Gender</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Gender</b> ';
                }
            }
            if ($('#<%=txtIssuedate.ClientID%>').val().trim() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Date of Issue</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Date of Issue</b> ';
                }
            }

            //if ($("#phPageBody_MaritalStatus").val() == "0") {
            //    errortab = '1';
            //    if (errorFieldsPD != '') {
            //        errorFieldsPD = errorFieldsPD + '<b>, Marital Status</b> ';
            //    }
            //    else {
            //        errorFieldsPD = errorFieldsPD + '<b>Marital Status</b> ';
            //    }

            //}
            if ($('#<%=txtPassportNo.ClientID%>').val().trim() == '') {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Passport No</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Passport No/IdentificationNo</b> ';
                }
            }


            if (errorFieldsPD != '') {
                $('#SpnErrorMsg').html(errrorTextPD + errorFieldsPD + errrorTextSectionPD);
                $('#errormsgDiv').show();
                if (errortab == '1') {
                    $("#TabC1").click();
                    $('#<%=txtFirstName.ClientID%>').focus();

                }
                ErrorCount = 6;
                return false;
            }

            errorFieldsPD = '';
            errrorTextSectionPD = '';
            errrorTextPD = '';

            //$('.shader').fadeIn(); 
            //-------------------------------------------------
            if ((ErrorCount == 0)) {
                //$('#phPageBody_userBtns_btnSubmit_U').prop("disabled",true)
                $('.shader').show();
            }
        });
    </script>



    <%--//---------------------------------------------------%>


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

        //Confirm rejection        
        //$("[name$='IsConfirm']").click(function () {
        $("input[name='IsConfirm']", $('#radioBtnDiv')).change(function () {


            if ($("input:radio[name='IsConfirm']:checked").val() == "1") {

                $('#remarkDiv').css("display", "")
                $('#phPageBody_Reject_Btn').css("display", "")
                $('[id$="txtRejectReson"]').attr('required', true)
            }
            else {
                $('[id$="txtRejectReson"]').attr('required', false)
            }
        });

        //Remove required field of reject reason
        function funCancelModal() {
            $('[id$="txtRejectReson"]').attr('required', false)
            $('#RejectConfirmationModal').modal('hide')
        }

        //function funCancel(Para) {

        //    fnreset(null, true)
        //    $('[id$="txtRejectReson"]').attr('required', false)

        //}
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

    <!--start sheetal auto income calculate  add function call on onfocus of total annual income txt -->
    <script>
        function add() {

            var txt1 = (document.getElementById('<%= txtAnnualSal.ClientID %>').value) == '' ? 0 : (document.getElementById('<%= txtAnnualSal.ClientID %>').value)
            var txt2 = (document.getElementById('<%=txtAnnualIncentive.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtAnnualIncentive.ClientID%>').value);
            var txt3 = (document.getElementById('<%=txtAnnualIncome.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtAnnualIncome.ClientID%>').value);
            var txt4 = (document.getElem    entById('<%=txtRentalIncome.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtRentalIncome.ClientID%>').value);
            var txt5 = (document.getElementById('<%=txtAgriculture.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtAgriculture.ClientID%>').value);
            var txt6 = (document.getElementById('<%=txtIncome.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtIncome.ClientID%>').value);

            var result = parseInt(txt1) + parseInt(txt2) + parseInt(txt3) + parseInt(txt4) + parseInt(txt5) + parseInt(txt6);
            if (!isNaN(result) || (result != 0)) {


                document.getElementById('<%=txtTotalIncome.ClientID%>').value = result;
            }
            else {
                document.getElementById('<%=txtTotalIncome.ClientID%>').value = ''
            }

        }
    </script>

    <!--start sheetal to hide textbox on dropdown click for collect for card detail tab -->
    <script>
        $('#<%=StatementBy.ClientID %>').change(function () {
            if ($('#phPageBody_StatementBy').val() == "1") {
                $("#DivtohideEmail").css('display', 'none');
            }
            else {
                $("#DivtohideEmail").show();
            }
        });
    </script>

    <%-- Show Loader --%>
    <script>
        function funShowLoader() {
            $('.shader').fadeIn();
            return true;
        }
    </script>

</asp:Content>

