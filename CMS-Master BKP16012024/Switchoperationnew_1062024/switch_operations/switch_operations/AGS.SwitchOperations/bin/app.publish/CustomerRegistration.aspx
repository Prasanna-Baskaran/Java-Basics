<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="CustomerRegistration.aspx.cs" Inherits="AGS.SwitchOperations.CustomerRegistration" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">

    <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
    <script src="Scripts/bootstrap-DatePicker.js"></script>
    <!-- start sheetal -->
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
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
                        <div class="box-primary">
                            <div class="row">
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
                                                                    <input type="text" class="form-control" maxlength="15" runat="server" name="FirstName" id="txtFirstName" required="required" onkeypress="return onlyAlphabets(event,this);" />
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
                                                                    <input type="text" class="form-control" maxlength="15" runat="server" name="LastName" id="txtLastName" required="required" onkeypress="return onlyAlphabets(event,this);" />
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
                                                                    <input type="text" class="form-control" runat="server" id="txtMobileNo" maxlength="12" onkeypress="return FunChkIsNumber(event)" required="required" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Nationality:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" maxlength="15" runat="server" id="txtNationality" required="required" onkeypress="return onlyAlphabets(event,this);" />
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
                                                                    <input type="text" class="form-control" runat="server" id="txtPassportNo" onkeypress="return FunChkAlphaNumeric(event)" required="required" maxlength="20" />
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
                                                                <%-- start sheetal--%>
                                                                <%-- Make House No,Street Name,City,District,Email  text boxes mandatory Change By Sheetal--%>
                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>House No:</label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="col-xs-4">
                                                                <input type="text" class="form-control" runat="server" id="txtHouseNo_P" maxlength="15" onkeypress="return FunChkAlphaNumeric(event)" />
                                                            </div>

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
                                                                    <input type="text" class="form-control" id="txtTole_P" runat="server" maxlength="15" />
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
                                                            <!-- end sheetal-->
                                                            <div class="col-xs-4">
                                                                <input type="email" class="form-control" runat="server" id="txtEmail_P" />
                                                            </div>
                                                        </div>
                                                    </div>


                                                </div>

                                                <div class="box-header">
                                                    <!-- start sheetal-->
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
                                                                    <input type="text" class="form-control" id="txtTole_C" runat="server" maxlength="15" />
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
                                                                    <input type="text" class="form-control" id="txtGrandFatherName" runat="server" maxlength="25" onkeypress="return onlyAlphabets(event,this);" />
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
                                                                   <%--     <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                        <asp:ListItem Text="Savings" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="Current" Value="2"></asp:ListItem>
                                                                        <asp:ListItem Text="Fixed" Value="3"></asp:ListItem>
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
                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Card Type:</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList ID="DdlCardType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Institution ID:</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList ID="ddlINSTID" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>
                                                    <div class="row">

                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Product:</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList ID="DdlProduct" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Name On Card:</label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" class="form-control" id="txtNameOnCard" runat="server" maxlength="22" onkeypress="return onlyAlphabets(event,this);" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row" style="display: none">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label">Branch:</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList ID="DdlBranch" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label">BIN:</label>
                                                                <div class="col-sm-7">
                                                                    <asp:DropDownList ID="DdlBin" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <div class="form-group">
                                                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Statement Delivery:</label>
                                                                <div class="col-sm-7">
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
                                                                    <div class="col-sm-7">
                                                                        <input type="email" class="form-control" id="txtEmailForStatement" runat="server" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--//Refference--%>
                                        <div class="tab-pane" id="RefferenceDiv">
                                            <div class="">
                                                <div class="box-header">
                                                    <!--start sheetal -->
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
                        <ul class="pager wizard">
                            <li class="previous first"><a href="#">First</a></li>
                            <li class="previous"><a href="#">Previous</a></li>
                            <li class="next last"><a href="#">Last</a></li>
                            <li class="next"><a href="#">Next</a></li>
                        </ul>

                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn-div btn-style btn btn-primary" Style="border: 0.5px solid #fff; border-radius: 4px; margin-top: -55px; display: none; margin-left: 50%; margin-right: auto;" />

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



    <%--//************************ Scripts ********************--%>
    <script>
        $(document).ready(function () {
            $('#phPageBody_txtTotalIncome').prop('readonly', true)

            // hide accountNo  and account type for remittance system
            if(<%=HttpContext.Current.Session["SystemID"]%>=="1")
            {
                $('[id$="DdlAccountType"]').attr('disabled', 'disabled');
                $('#phPageBody_txtAccountNo').prop("readonly",true)              
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
            //$('.datepickerNepal').nepaliDatePicker();
            $('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true,endDate: new Date() });
            
           
        });
        var ErrorCount=0;
        //For radiobutton check  events
        //$("input[name=IsSameAsPermAddrs]:radio").click(function () {
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
        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');

            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                $('input[type="text"]').val('');
            }

        }

        function CancelModal() {
            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                fnreset(null, true);
                $("#<%=ddlINSTID.ClientID %>").val(0);
                $("#<%=DdlProduct.ClientID %>").val(0);
            }
        }

    </script>

    <%-- ********************************** Click functions ***************************--%>
    <script>
      
        $("[id$='DdlProffessionType']").click(function () {
            if ($("#phPageBody_DdlProffessionType").val() == 1) {
                $('#Orgnizationdiv').css("display", "");
            }
            else {
                $('#Orgnizationdiv').css("display", "none");
            }

            if ($("#phPageBody_DdlProffessionType").val() == 3) {
                $('#PreviousEmpDiv').css("display", "");
            }
            else {
                $('#PreviousEmpDiv').css("display", "none");
            }
        });


        $("[id$='IsPrabhuAccount']").click(function () {
            if ($("[id*=IsPrabhuAccount] input:checked").val() == 1) {
                $('#PhrabhuBankDtlDiv').css("display", "");
            }
            else {
                $('#PhrabhuBankDtlDiv').css("display", "none");
            }
        });

    </script>

    <%--******************************* Documentation *************************** --%>
    <script>
       
        $("#phPageBody_userBtns_btnSubmit_U").click(function () {
            ErrorCount =0 
            var errrorTextDD = 'Please provide ';
            var errrorTextSectionDD = ' in Documentation.';
            var errorFieldsDD = '';
           
            if (($('#phPageBody_PhotoUpload').val() != "") || ($('#phPageBody_SignatureUpload').val() != "") || ($('#phPageBody_IDProofUpload').val() != "")) {
                var Photoext = $('#phPageBody_PhotoUpload').val().split('.').pop().toLowerCase();
                if (($.inArray(Photoext, ['gif', 'png', 'jpg', 'jpeg']) == -1)) {
                    if (errorFieldsDD != '') {
                        errorFieldsDD = errorFieldsDD + '<b>, valid Photo</b> ';
                    }
                    else {
                        errorFieldsDD = errorFieldsDD + '<b>valid Photo</b> ';
                    }
                }

                else if ((phPageBody_PhotoUpload.files[0].size / 1024) > 100) {
                    if (errorFieldsDD != '') {
                        errorFieldsDD = errorFieldsDD + '<b>, valid Photo (Size exceed)</b> ';
                    }
                    else {
                        errorFieldsDD = errorFieldsDD + '<b>valid Photo (Size exceed)</b> ';
                    }
                }



                var Sigext = $('#phPageBody_SignatureUpload').val().split('.').pop().toLowerCase();
                if ($.inArray(Sigext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                    if (errorFieldsDD != '') {
                        errorFieldsDD = errorFieldsDD + '<b>, valid Signature</b> ';
                    }
                    else {
                        errorFieldsDD = errorFieldsDD + '<b>valid Signature</b> ';
                    }
                }
                else if ((phPageBody_SignatureUpload.files[0].size / 1024) > 100) {
                    if (errorFieldsDD != '') {
                        errorFieldsDD = errorFieldsDD + '<b>, valid Signature (Size exceeds)</b> ';
                    }
                    else {
                        errorFieldsDD = errorFieldsDD + '<b>valid Signature (Size exceeds)</b> ';
                    }
                }


                var Sigext = $('#phPageBody_IDProofUpload').val().split('.').pop().toLowerCase();
                if ($.inArray(Sigext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                    if (errorFieldsDD != '') {
                        errorFieldsDD = errorFieldsDD + '<b>, valid ID proof</b> ';
                    }
                    else {
                        errorFieldsDD = errorFieldsDD + '<b>valid ID proof</b> ';
                    }
                }
                else if ((phPageBody_IDProofUpload.files[0].size / 1024) > 100) {
                    if (errorFieldsDD != '') {
                        errorFieldsDD = errorFieldsDD + '<b>, valid ID Proof (Size exceeds)</b> ';
                    }
                    else {
                        errorFieldsDD = errorFieldsDD + '<b>valid ID Proof(Size exceeds)</b> ';
                    }
                }



                if (errorFieldsDD != '') {
                    $('#SpnErrorMsg').html(errrorTextDD + errorFieldsDD + errrorTextSectionDD);
                    $('#errormsgDiv').show();
                    $("#TabC8").click();
                    errorFieldsDD = '';
                    errrorTextSectionDD = '';
                    errrorTextDD = '';
                    ErrorCount=1;
                    return false;
                }
                //else {
                //    IsClickDisable=true;
                //    //$('.shader').fadeIn();
                //}

            }
        });
    </script>


    <%----*************************************** Refference ****************************--%>

    <%--    <script>
        $("#<%=btnSubmit.ClientID %>").click(function () {
            var errrorTextRD = 'Please provide ';
            var errrorTextSectionRD = ' in Refferences.';
            var errorFieldsRD = '';



            if ($('#<%=txtRefName1.ClientID%>').val().trim() == '') {

                if (errorFieldsRD != '') {
                    errorFieldsRD = errorFieldsRD + '<b>, Name</b> ';
                }
                else {
                    errorFieldsRD = errorFieldsRD + '<b>Name</b> ';
                }
            }

            if ($('#<%=txtDesignation1_R.ClientID%>').val().trim() == '') {

                if (errorFieldsRD != '') {
                    errorFieldsRD = errorFieldsRD + '<b>, Designation</b> ';
                }
                else {
                    errorFieldsRD = errorFieldsRD + '<b>Designation</b> ';
                }
            }

            if ($('#<%=txtPhone1_R.ClientID%>').val().trim() == '') {

                if (errorFieldsRD != '') {
                    errorFieldsRD = errorFieldsRD + '<b>, Phone</b> ';
                }
                else {
                    errorFieldsRD = errorFieldsRD + '<b>Phone</b> ';
                }
            }
            if (errorFieldsRD != '') {
                $('#SpnErrorMsg').html(errrorTextRD + errorFieldsRD + errrorTextSectionRD);
                $('#errormsgDiv').show();
                $("#TabC7").click();
                errorFieldsRD = '';
                errrorTextSectionRD = '';
                errrorTextRD = '';
                return false;
            }

        });
    </script>--%>

    <%----****************************** Statement *******************************--%>

    <script>
        $("#phPageBody_userBtns_btnSubmit_U").click(function () {
            var errrorTextSD = 'Please provide ';
            var errrorTextSectionSD = ' in Card Details .';
            var errorFieldsSD = '';
            var eml = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

        <%--    if ($("#phPageBody_DdlCardType").val() == "0") {
                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Card Type</b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Card Type</b> ';
                }
                $("#<%=DdlCardType.ClientID %>").focus();
            }--%>

            if ($("#phPageBody_DdlProduct").val() == "0") {
                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Product</b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Product</b> ';
                }
                $("#<%=DdlProduct.ClientID %>").focus();
            }

            <%--if ($("#phPageBody_ddlINSTID").val() == "0") {
                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Institution ID</b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Institution ID</b> ';
                }
                $("#<%=ddlINSTID.ClientID %>").focus();
            }--%>

            if ($("#<%=txtNameOnCard.ClientID %>").val() == "") {
                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Name On Card</b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Name On Card</b> ';
                }
                $("#<%=txtNameOnCard.ClientID %>").focus();

            }

            //if ($('input[id*=StatementBy]:checked').length == 0) {
            if ($("#phPageBody_StatementBy").val() == "0") {

                if (errorFieldsSD != '') {
                    errorFieldsSD = errorFieldsSD + '<b>, Statement delivery </b> ';
                }
                else {
                    errorFieldsSD = errorFieldsSD + '<b>Statement delivery </b> ';
                }

                $("#<%=StatementBy.ClientID %>").focus();
            }
            else if ($("#phPageBody_StatementBy").val() == "2") {
                if (eml.test($.trim($("#<%=txtEmailForStatement.ClientID %>").val())) == false) {
                    $("#<%=txtEmailForStatement.ClientID %>").focus();
                    if (errorFieldsSD != '') {
                        errorFieldsSD = errorFieldsSD + '<b>, valid email Address.</b> ';
                    }
                    else {
                        errorFieldsSD = errorFieldsSD + '<b> valid email address.</b> ';
                    }
                }
            }

            if (errorFieldsSD != '') {
                $('#SpnErrorMsg').html(errrorTextSD + errorFieldsSD + errrorTextSectionSD);
                $('#errormsgDiv').show();
                $("#TabC6").click();
                errorFieldsSD = '';
                errrorTextSectionSD = '';
                errrorTextSD = '';
                ErrorCount=2
                return false;
            }
            //else
            //{IsClickDisable=true;}

        });
    </script>


    <%----************************************** Finantial Info *********************************--%>
    <script>
        $("#phPageBody_userBtns_btnSubmit_U").click(function () {
            var errrorTextFID = 'Please provide ';
            var errrorTextSectionFID = ' in Financial Details.';
            var errorFieldsFID = '';
            //for Debit system
            if(<%=HttpContext.Current.Session["SystemID"]%>=="2")
            {
                if ($('#phPageBody_DdlAccountType').val() == 0) {
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
                ErrorCount=3
                return false;
            }
            //else
            //{IsClickDisable=true;}

        });
    </script>



    <%----*************************** Occupation validation **************************--%>

    <%--<script>
          $("#phPageBody_userBtns_btnSubmit_U").click(function () {
            var errrorTextOD = 'Please provide ';
            var errrorTextSectionOD = ' in Occupational Details.';
            var errorFieldsOD = '';

            if ($("#phPageBody_DdlProffessionType").val() == "0") {

                if (errorFieldsOD != '') {
                    errorFieldsOD = errorFieldsOD + '<b>, Occupation </b> ';
                }
                else {
                    errorFieldsOD = errorFieldsOD + '<b>Occupation</b> ';
                }
            }
            else if ($("#phPageBody_DdlProffessionType").val() == "1") {
                if ($("#phPageBody_ddlOrgzTypeID").val() == "0") {
                    if (errorFieldsOD != '') {
                        errorFieldsOD = errorFieldsOD + '<b>, Work for </b> ';
                    }
                    else {
                        errorFieldsOD = errorFieldsOD + '<b>Work for</b> ';
                    }
                }
            }
        
            if (errorFieldsOD != '') {
                $('#SpnErrorMsg').html(errrorTextOD + errorFieldsOD + errrorTextSectionOD);
                $('#errormsgDiv').show();
                $("#TabC4").click();
                errorFieldsOD = '';
                errrorTextSectionOD = '';
                errrorTextOD = '';
                return false;
            }

        });
    </script>--%>



    <%----*************************** Family details ************************* --%>

    <%--<script>
        $("#phPageBody_userBtns_btnSubmit_U").click(function () {
            var errrorTextFD = 'Please provide ';
            var errrorTextSectionFD = ' in Family Details.';
            var errorFieldsFD = '';
            if ($('#<%=txtMotherName.ClientID%>').val().trim() == '') {

                if (errorFieldsFD != '') {
                    errorFieldsFD = errorFieldsFD + '<b>, Mother Name</b> ';
                }


                else {
                    errorFieldsFD = errorFieldsFD + '<b>Mother Name</b> ';
                }
            }



            if (errorFieldsFD != '') {
                $('#SpnErrorMsg').html(errrorTextFD + errorFieldsFD + errrorTextSectionFD);
                $('#errormsgDiv').show();
                $("#TabC3").click();
                $('#<%=txtMotherName.ClientID%>').focus();
                errorFieldsFD = '';
                errrorTextSectionFD = '';
                errrorTextFD = '';
                return false;
            }
        });
    </script>--%>

    <%----************************* Address details ****************************** --%>
    <script>
        $("#phPageBody_userBtns_btnSubmit_U").click(function () {
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
                    errorFieldsAD = errorFieldsAD + '<b>, valid email Address</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b>valid email Address</b> ';
                }
            }

            else if (eml.test($.trim($("#<%=txtEmail_P.ClientID %>").val())) == false) {

                $("#<%=txtEmail_P.ClientID %>").focus();
                if (errorFieldsAD != '') {
                    errorFieldsAD = errorFieldsAD + '<b>, valid email Address.</b> ';
                }
                else {
                    errorFieldsAD = errorFieldsAD + '<b> valid email address.</b> ';
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
                //to give validation when click on radio button as yes and clear any fields
                // else if ($('input[id*=IsSameAsPermAddrs]:checked').val() == "0") {
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
                //start sheetal
                // to give correct  validation message for  wrong email
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
                ErrorCount=4
                return false;
            }
            //else
            //    IsClickDisable=true;

        });


    </script>


    <%--  *******************************Personal details Validation **********************************--%>
    <script>
        var errortab = '';
        $("#phPageBody_userBtns_btnSubmit_U").click(function () {
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

            <%--if ($('#<%=txtMiddleName.ClientID%>').val().trim() == '') {

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
                    errorFieldsPD = errorFieldsPD + '<b>valid Mobile No</b> ';
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

            <%--if ($("#<%=Gender.ClientID %> :selected").val() == "") {--%>
            //if ($("#phPageBody_Gender ").is(":not(':checked')") == true) {
            //if ($('input[id*=Gender]:checked').length == 0) {
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

            //if ($("#phPageBody_MaritalStatus").is(":not(':checked')") == true) {
            //if ($('input[id*=MaritalStatus]:checked').length == 0) {
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

            //Residence type
            //if ($("#phPageBody_ResidenceType").is(":not(':checked')") == true) {


            if (errorFieldsPD != '') {
                $('#SpnErrorMsg').html(errrorTextPD + errorFieldsPD + errrorTextSectionPD);
                $('#errormsgDiv').show();
                if (errortab == '1') {
                    $("#TabC1").click();
                    $('#<%=txtFirstName.ClientID%>').focus();
                }

                errorFieldsPD = '';
                errrorTextSectionPD = '';
                errrorTextPD = '';
                ErrorCount=5
                return false;
            }
            //else
            //    IsClickDisable=true;
          

            //-------------------------------------------------
            if((ErrorCount==0))
            {
                //$('#phPageBody_userBtns_btnSubmit_U').prop("disabled",true)
                $('.shader').show();
            }

        });
        

    </script>

    <!--start sheetal auto income calculate  add function call on onfocus of total annual income txt -->
    <%-- <script>
        function add() {

            var txt1 = (document.getElementById('<%= txtAnnualSal.ClientID %>').value) == '' ? 0 : (document.getElementById('<%= txtAnnualSal.ClientID %>').value)
            var txt2 = (document.getElementById('<%=txtAnnualIncentive.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtAnnualIncentive.ClientID%>').value);
            var txt3 = (document.getElementById('<%=txtAnnualIncome.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtAnnualIncome.ClientID%>').value);
            var txt4 = (document.getElementById('<%=txtRentalIncome.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtRentalIncome.ClientID%>').value);
            var txt5 = (document.getElementById('<%=txtAgriculture.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtAgriculture.ClientID%>').value);
            var txt6 = (document.getElementById('<%=txtIncome.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtIncome.ClientID%>').value);

            var result = parseInt(txt1) + parseInt(txt2) + parseInt(txt3) + parseInt(txt4) + parseInt(txt5) + parseInt(txt6);
            if (!isNaN(result) || (result != 0)) {
                if (((document.getElementById('<%= txtAnnualSal.ClientID %>').value) <> '') || (document.getElementById('<%= txtAnnualIncentive.ClientID %>').value <> '') || (document.getElementById('<%= txtAnnualIncome.ClientID %>').value <> '')|| (document.getElementById('<%= txtRentalIncome.ClientID %>').value <> '') || (document.getElementById('<%= txtAgriculture.ClientID %>').value <> '') || (document.getElementById('<%= txtIncome.ClientID %>').value <> '') )
                {
                    document.getElementById('<%=txtTotalIncome.ClientID%>').value = result;
                }
                else
                {
                    document.getElementById('<%=txtTotalIncome.ClientID%>').value = '';
                }
            }
            else {
                document.getElementById('<%=txtTotalIncome.ClientID%>').value = '';
            }

        }
</script> --%>


    <script>
        function add() {

            var txt1 = (document.getElementById('<%= txtAnnualSal.ClientID %>').value) == '' ? 0 : (document.getElementById('<%= txtAnnualSal.ClientID %>').value)
            var txt2 = (document.getElementById('<%=txtAnnualIncentive.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtAnnualIncentive.ClientID%>').value);
            var txt3 = (document.getElementById('<%=txtAnnualIncome.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtAnnualIncome.ClientID%>').value);
            var txt4 = (document.getElementById('<%=txtRentalIncome.ClientID%>').value) == '' ? 0 : (document.getElementById('<%=txtRentalIncome.ClientID%>').value);
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



</asp:Content>
