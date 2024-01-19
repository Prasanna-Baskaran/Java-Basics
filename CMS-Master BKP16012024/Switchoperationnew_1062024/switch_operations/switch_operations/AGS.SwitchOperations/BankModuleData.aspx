<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="BankModuleData.aspx.cs" Inherits="AGS.SwitchOperations.BankModuleData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">

    <!--All Hiddenfields------------------------------------------------------------------------->
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnFlag" runat="server" />
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />

<!--Display validation msg ------------------------------------------------------------------------->
                    <div class="pad margin no-print" id="errormsgDiv" style="display: none">
                        <div class="callout callout-info" style="margin-bottom: 0!important;">
                            <h4><i class="fa fa-info"></i>Information :</h4>
                            <span id="SpnErrorMsg" class="text-center"></span>
                        </div>
                    </div>


    <asp:Panel ID="pnlModuleDataSearch" runat="server">
        <div style="font-size: 14px;">
            <div id="SearchModule">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>

                        <h4 class="box-title">Bank Module Details:</h4>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                        <!-- /.box-tools -->
                    </div>

                    

                    <div class="box-body">
                        
                        <div class="box-body" id="SearchDiv">
                            <div class="form-horizontal">
                                <div class="row">
                                    <%--<div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;"></span>ModuleName:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" runat="server" name="SearchModuleName" id="txtSearchModuleName" onkeypress="return onlyAlphabets(event);" maxlength="1000" />

                                            </div>
                                        </div>
                                    </div>--%>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>IssuerNo:</label>
                                            <div class="col-sm-8">
                                                <input type="text" class="form-control" maxlength="4" runat="server" name="SearchIssuerNo" id="txtSearchIssuerNo" onkeypress="return FunChkIsNumber(event);" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <div class="col-sm-4"></div>

                                            <div class="col-sm-3">
                                                <div class="col-md-8">
                                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" OnClientClick="return FunSearchValidation();" />
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                            <div class="col-md-8">
                                         <input type="button" class="btn btn-primary pull-right" id="BtnAddNew" data-targrt="#AddEditModal" value="Add New" onclick="funAddNew()"/>
                                            </div>
                                                </div>

                                            
                                                </div>
                                            <div class="col-sm-4"></div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>
                        <div class="row">
                                    <%--<div class="col-md-4">--%>
                                        <div class="form-group">
                                            <div class="col-sm-6"></div>
                                         <%--<div class="col-sm-4">--%>
                                            <div class="col-md-6">
                                    <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="Button1" Text="Reset" OnClick="btnReset_Click" />
                                            </div>
                                          </div>
                                        </div>
                                        <%--</div>
                            </div>--%>


                        <div class="row" id="DivResultMsg">
                            <div class="col-md-6">
                                <h4>
                                    <label maxlength="20" runat="server" name="Name" id="LblResult" readonly="readonly" />
                                </h4>
                            </div>
                        </div>

                        

                        

                    </div>
                </div>
            </div>
        
    </asp:Panel>
    
    <%--7-1-18--%>
    <div id="shader" class="shader">
        <div class="loadingContainer">
            <div id="divLoading3">
                <div id="loader-wrapper">
                    <div id="loader"></div>
                </div>
            </div>
        </div>
    </div>

    <%--<div>
        <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%">
        </table>
    </div>--%>

    <div class="box-primary">
    <div class="box-body" id="DivReslt">
    <div class="row">
                <div class="col-md-12">
                    <div class="box-primary">
                        <!-- /.box-header -->
                        <div class="box-body no-padding">
                            <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                <div class="col-md-12">
                                    <%--<div class="x_panel">
                                        <div>
                                            <div class="clearfix"></div>
                                        </div>--%>


                                       <%-- <div class="x_content">--%>
                                            <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%">
                                            </table>
                                       <%-- </div>--%>
                                    <%--</div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>


    <div id="AddEditModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px; width: 797px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">Bank Module Data</h4>
                </div>
                <%--7-1-18--%>
                <!--Display validation msg ------------------------------------------------------------------------->
                <div class="pad margin no-print" id="errormsgDivForModel" style="display: none">
                    <div class="callout callout-info" style="margin-bottom: 0!important;">
                        <h4><i class="fa fa-info"></i>Information :</h4>
                        <span id="SpnErrorMsgForModel" class="text-center"></span>
                    </div>
                </div>


                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>ModuleName:</label>
                            <div class="col-md-7">
                                <input type="text" class="form-control" id="txtModuleName" runat="server" placeholder="Enter Module Name" maxlength="1000" onkeypress="return onlyAlphabets(event);" />



                            </div>
                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>IssuerNo:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtIssuerNo" runat="server" placeholder="Enter Issuer No" maxlength="4" onkeypress="return FunChkIsNumber(event);" />

                            </div>
                        </div>


                    </div>

                    <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Frequency:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtFrequency" runat="server" placeholder="Enter Frequency" maxlength="4" onkeypress="return FunChkIsNumber(event);" />
                                
                            </div>

                        </div>
                        <div class="col-md-6">

                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>FrequencyUnit:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlFrequencyUnit" runat="server" class="form-control" Style="width: 100%">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="MI" Value="MI"></asp:ListItem>
                                    <asp:ListItem Text="HH" Value="HH"></asp:ListItem>

                                </asp:DropDownList>

                            </div>

                        </div>


                    </div>

                    <div class="row">
                        <br />
                    </div>


                    <div class="row">

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>EnableState:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlEnableState" runat="server" class="form-control" Style="width: 100%">
                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                </asp:DropDownList>

                            </div>

                        </div>

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Status:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="ddlStatus" runat="server" class="form-control" Style="width: 100%">
                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <%--<asp:ListItem Text="Current service is running" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Error in dll loade" Value="3"></asp:ListItem>--%>
                                    <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                </asp:DropDownList>

                            </div>

                        </div>
                        
                    </div>

                    <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>ClassName:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtClassName" runat="server" placeholder="Enter ClassName" maxlength="500" onkeypress="return GetclassName(event);" />

                            </div>
                        </div>


                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>DllPath:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txtDllPath" runat="server" placeholder="Enter DllPath" maxlength="500" onkeypress="return GetDllPath(event);" />
                            </div>

                        </div>
                      </div>


                    <div class="row">
                        <br />
                    </div>
                   
                      <div class="row">

                        <div class="col-sm-3">
                        </div>
<%--7-1-18--%>
                        <div class="col-sm-6 text-center">
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="SAVE" Style="width: 69px" ID="AddBtn" OnClick="add_Click" />
                            <button aria-label="Close" data-dismiss="modal" class="btn btn-primary" onclick="funCancelModal()" type="button"><span aria-hidden="true">CANCEL</span></button>
                            <input type="button" id="BtnReset" aria-label="Reset" class="btn btn-primary" value="RESET" onclick="FunClear();" />
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="DELETE" Style="width: 69px" ID="BtnDelete" OnClick="Delete_Click" OnClientClick="return confirm_user(); " />
                        </div>

                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </div>


    

    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Bank Module Data</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <asp:Label ID="LblMessage" runat="server"></asp:Label>
                    <%--<asp:Label ID="LblCardRPANMessage" runat="server"></asp:Label>--%>
                </div>
                <div class="modal-footer">
                    <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>

<script>
    $(document).ready(function () {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        });

        $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            //check user has Edit rights
            if ($.inArray("E", ($('#<%=hdnAccessCaption.ClientID%>').val().split(","))) > -1) {
                $('#datatable-buttons input[type="button"][value="Edit"]').show();
            }
            else {
                $('#datatable-buttons input[type="button"][value="Edit"]').hide();
            }
            //check user has Add rights
            if  ($("#<%=hdnFlag.ClientID %>").val() == "3" && ($.inArray("A", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1)) {
                $('#BtnAddNew').hide();
                

            }
            else if ($.inArray("A", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                $('#BtnAddNew').show();
            }
            
            
            //else {
            //    $('#BtnAddNew').hide();
            //}
        
            if ($("#datatable-buttons tbody tr").length > 0) {

                var handleDataTableButtons = function () {
                    if ($("#datatable-buttons").length) {
                        $("#datatable-buttons").DataTable({
                            dom: "Bfrtip",
                            buttons: [
                                {
                                    extend: "copy",
                                    className: "btn-sm"
                                },
                                {
                                    extend: "csv",
                                    className: "btn-sm"
                                },
                                {
                                    extend: "excel",
                                    className: "btn-sm"
                                },
                                {
                                    extend: "pdfHtml5",
                                    className: "btn-sm"
                                },
                                {
                                    extend: "print",
                                    className: "btn-sm"
                                },
                            ],
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
                $('#datatable-buttons tbody input[type=button][isedit=0][value=Edit]').hide()
            }

            $("#phPageBody_SystemList").find("td").css("padding-right", "10px")
    });

    </script>
    <%--7-1-18--%>
    <script>
    function FunClear()
        {
            //fnreset(null, true);
            //$('#SearchDiv').show();
            //$('#InfoDiv').hide();
            $('#errormsgDivForModel').hide();
            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtModuleName"]').val('');
            $('[id$="txtClassName"]').val('');
            $('[id$="txtDllPath"]').val('');
            $('[id$="txtFrequency"]').val('');
            $("#<%=ddlFrequencyUnit.ClientID%>")[0].selectedIndex = 0;
            //$('#<%=ddlFrequencyUnit.ClientID%>').val('0');
            $('#<%=ddlEnableState.ClientID%>').val('-1');
            $('#<%=ddlStatus.ClientID%>').val('-1');
                
        }
        </script>
    <script>

        function funAddNew() {

            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtModuleName"]').val('');
            
            <%--$('#<%=DDLUserRole.ClientID%> option:selected').text("--Select--");--%>
            $('[id$="txtFrequency"]').val('');
            $('[id$="txtClassName"]').val('');
            $('[id$="txtDllPath"]').val('');
            $('#<%=ddlFrequencyUnit.ClientID%>').val('0');
            $('#<%=ddlEnableState.ClientID%>').val('-1');
            $('#<%=ddlEnableState.ClientID%> option:selected').text("--Select--");
            $('#<%=ddlStatus.ClientID%>').val('-1');
            $('#<%=ddlStatus.ClientID%> option:selected').text("--Select--");

            $('#AddEditModal').modal('show');
            $('[id$="txtModuleName"]').attr("readonly", false);
            $('[id$="txtIssuerNo"]').attr("readonly", false);

            $("#<%=ddlEnableState.ClientID%>").attr("disabled", false)
            $('#BtnReset').show();
            $('#BtnDelete').hide();
            $('#<%=BtnDelete.ClientID%>').hide();
        }
    </script>

    <script>

        function funedit(obj) {

            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtModuleName"]').val('');
            $('[id$="txtFrequency"]').val('');
            $('#<%=ddlFrequencyUnit.ClientID%>').val('0');
            $('#<%=ddlEnableState.ClientID%>').val('-1');
            $('#<%=ddlEnableState.ClientID%> option:selected').text("--Select--");
            $('#<%=ddlStatus.ClientID%>').val('-1');
            $('#<%=ddlStatus.ClientID%> option:selected').text("--Select--");
            $('[id$="txtClassName"]').val('');
            $('[id$="txtDllPath"]').val('');
            //$("[id*=SystemList] input").removeAttr("checked")

            var sIssuerNo = $(obj).attr('IssuerNo');
            var sModuleName = $(obj).attr('ModuleName');
            var sClassName = $(obj).attr('ClassName');
            var sDllPath = $(obj).attr('DllPath');
            var sFrequency = $(obj).attr('Frequency');
            var sFrequencyUnit = $(obj).attr('FrequencyUnit');
            var sEnableState = $(obj).attr('EnableState');
            var sStatus = $(obj).attr('Status');


            $('[id$="txtIssuerNo"]').val(sIssuerNo);
            $('[id$="txtModuleName"]').val(sModuleName);
            $('[id$="txtClassName"]').val(sClassName);
            $('[id$="txtDllPath"]').val(sDllPath);
            //$("#phPageBody_DDLUserRole")[0].selectedIndex = suserrole;
            // $("#phPageBody_dropdown_status").val(suserstatus);
            $('[id$="txtFrequency"]').val(sFrequency);
            $("#<%=ddlFrequencyUnit.ClientID%>").val(sFrequencyUnit);
            $("#<%=ddlEnableState.ClientID%>").val(sEnableState);
            $("#<%=ddlStatus.ClientID%>").val(sStatus);
            //$('#txtCardProgram').attr('disabled', true);
            $('#BtnReset').hide();
            $('#AddEditModal').modal('show');
            $('[id$="txtModuleName"]').attr("readonly", true);
            $('[id$="txtIssuerNo"]').attr("readonly", true);

            $('#BtnReset').hide();
            $('#BtnDelete').show();
            $('#<%=BtnDelete.ClientID%>').show();
            //$('[id$="txt_username"]').attr('disabled', 'disabled');
            // $($(obj).attr('systemid').split(",")).each(function (i, item) {
            // $("table[id*=SystemList] input[value=" + $.trim(item) + "]").prop("checked", "checked");
            // });
            //$("#txtCardProgram").prop("readonly", true);
                    }
    </script>

    <script>
        function funCancelModal() {
            $('#errormsgDivForModel').hide();
            $('[id$="txtIssuerNo"]').val('');
            $('[id$="txtModuleName"]').val('');
            $('[id$="txtClassName"]').val('');
            $('[id$="txtDllPath"]').val('');
            $('[id$="txtFrequency"]').val('');
            $("#<%=ddlFrequencyUnit.ClientID%>")[0].selectedIndex = 0;
            //$('#<%=ddlFrequencyUnit.ClientID%>').val('0');
            $('#<%=ddlEnableState.ClientID%>').val('-1');
            $('#<%=ddlEnableState.ClientID%> option:selected').text("--Select--");
            $('#<%=ddlStatus.ClientID%>').val('-1');
            $('#<%=ddlStatus.ClientID%> option:selected').text("--Select--");
           $('#AddEditModal').modal('hide')

        }
    </script>
    
    <script>
        function confirm_user() {
            if (confirm("Are you sure you want to delete the Record") == true)
                return true;
            else
                return false;
        }


    </script>

    <script>
        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');
        }
    </script>
    <script>
        function Hidemodel() {
            //7-11
            $('#memberModal').modal('hide');
        }
    </script>
    <script>
        //to prevent model closing when click outside
        $('#memberModal').modal({
            backdrop: 'static',
            keyboard: false
        })
    </script>


    <script>
        //function to clear textboxes
        function FunClearAllTextValue() {

            var elements = [];
            elements = document.getElementsByClassName("form-control");

            for (var i = 2; i < elements.length; i++) {
                elements[i].value = "";
            }

        }
    </script>

    <script>
        //Validation on Search Button
        function FunSearchValidation() {

            var ModuleName = $('#phPageBody_txtSearchModuleName').val()
            var IssuerNo = $('#phPageBody_txtSearchIssuerNo').val()
            var Number = /^[0 - 9]$/;
            var alpha = /^[a-zA-Z ]+$/;
            //if (ModuleName == "") {
            //    $('#SpnErrorMsg').html('Please provide modulename for bank');
            //    $('#errormsgDiv').show();
            //    return false;
            //}
            if (IssuerNo == "") {
                $('#SpnErrorMsg').html('Please provide numeric IssuerNo');
                $('#errormsgDiv').show();
                return false;
            }


            else {
                $('#errormsgDiv').hide();
                $('.shader').fadeIn();
            }


        }


    </script>
    <%--7-1-18--%>
    <script>
    $("#<%=AddBtn.ClientID %>").click(function () {
            var errrorTextPD = 'Please provide :  ';
            var errorFieldsPD = '';

     if ($("#phPageBody_txtModuleName").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, Module Name</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Module Name</b> ';
                    }
                    $('#<%=txtModuleName.ClientID%>').focus();
                }

                 if ($("#phPageBody_txtIssuerNo").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Issuer No</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Issuer No</b> ';
                    }
                    $('#<%=txtIssuerNo.ClientID%>').focus();
                }

                 if ($("#phPageBody_txtFrequency").val() == "") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Frequency</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Frequency</b> ';

                    }

                    $('#<%=txtFrequency.ClientID%>').focus();
                 }

                 if ($("#phPageBody_ddlFrequencyUnit").val() == "0") {
                     errortab = '1';

                     if (errorFieldsPD != '') {
                         errorFieldsPD = errorFieldsPD + '<b>,Frequency Unit</b> ';

                     }
                     else {
                         errorFieldsPD = errorFieldsPD + '<b>Frequency Unit</b> ';

                     }
                     $('#<%=ddlFrequencyUnit.ClientID%>').focus();
                }


                 if ($("#phPageBody_ddlEnableState").val() == "-1") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Enable State</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Enable State</b> ';
                    }
                    $('#<%=ddlEnableState.ClientID%>').focus();
                }

                 if ($("#phPageBody_ddlStatus").val() == "-1") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Status</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Status</b> ';

                    }
                    $('#<%=ddlStatus.ClientID%>').focus();
                }

                 if ($("#phPageBody_txtClassName").val() == "") {
                     errortab = '1';
                     if (errorFieldsPD != '') {
                         errorFieldsPD = errorFieldsPD + '<b>,Class Name</b> ';
                     }
                     else {
                         errorFieldsPD = errorFieldsPD + '<b>Class Name</b> ';
                     }
                     $('#<%=txtClassName.ClientID%>').focus();
        }
                 if ($("#phPageBody_txtDllPath").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,DllPath</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>DllPath</b> ';
                    }
                    $('#<%=txtDllPath.ClientID%>').focus();
                  }

                 
                 //if ($('#phPageBody_txtIssuerNo').val() != $('#phPageBody_txtSearchIssuerNo').val()) {
                 //    $('#SpnErrorMsgForModel').html('Issuer no does not match with search');
                 //    $('#errormsgDivForModel').show();
                 //    return false;
                 //}

                  if (errorFieldsPD != '') {
                      $('#SpnErrorMsgForModel').html(errrorTextPD + errorFieldsPD);
                      $('#errormsgDivForModel').show();
                      return false;
                  }
                  else {
                      errorFieldsPD = '';
                      errrorTextSectionPD = '';
                      errrorTextPD = '';
                      $('#errormsgDivForModel').hide();

                     // var ArrayIds = [];
                      //$("[id*=SystemList] input:checked").each(function (i) {
                      //    ArrayIds[i] = $(this).val();


                      
                      //alert(val.join(","))
                      
                    $('.shader').fadeIn();
                }
              });

    </script>

    <script>
        function FunSaveValidation() {

            //function to validate save button
            var ModuleName = $('#phPageBody_txtModuleName').val()
            var IssuerNo = $('#phPageBody_txtIssuerNo').val()
            var ModuleNameSearch = $('#phPageBody_txtSearchModuleName').val()
            var IssuerNoSearch = $('#phPageBody_txtSearchIssuerNo').val()
            var Frequency = $('#phPageBody_ddlFrequency').val()
            var Number = /^[0 - 9]$/;
            var status = /^[1-3]$/;
            var alpha = /^[a-zA-Z ]+$/;
            //var Dll = /^[a-zA-Z .\:]$/;
            if (ModuleName == "") {
                $('#SpnErrorMsg').html('Please provide modulename for bank');
                $('#errormsgDiv').show();
                return false;
            }
            if (ModuleName != ModuleNameSearch) {
                $('#SpnErrorMsg').html('Module name does not match');
                $('#errormsgDiv').show();
                return false;
            }

            if (IssuerNo == "") {
                $('#SpnErrorMsg').html('Please provide numeric issuerNo');
                $('#errormsgDiv').show();
                return false;
            }


            if (IssuerNo != IssuerNoSearch) {
                $('#SpnErrorMsg').html('Issuerno does not match');
                $('#errormsgDiv').show();
                return false;

            }


            if ($('#phPageBody_ddlFrequency').val() == "0") {
                $('#SpnErrorMsg').html('please select frequency');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_ddlFrequencyUnit').val() == "0") {
                $('#SpnErrorMsg').html('please select frequencyunit');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_ddlEnablState').val() == "0") {
                $('#SpnErrorMsg').html('please select enable state');
                $('#errormsgDiv').show();
                return false;
            }
            //13-12-17
            if ($('#phPageBody_ddlStatus').val() == "0") {
                $('#SpnErrorMsg').html('please select Status');
                $('#errormsgDiv').show();
                return false;
            }
            if (status.test($.trim($('#phPageBody_txtStatus').val())) == false) {
                $('#SpnErrorMsg').html('Status should be numeric');
                $('#errormsgDiv').show();
                return false;
            }
            //if (Number.test($.trim($('#phPageBody_txtModifiedBy').val())) == false) {
            //    $('#SpnErrorMsg').html('ModifiedBy field should be numeric');
            //    $('#errormsgDiv').show();
            //    return false;
            //}
            //if (Dll.test($.trim($('#phPageBody_txtDllPath').val())) == false) {
            //    $('#SpnErrorMsg').html('please enter proper dll path');
            //    $('#errormsgDiv').show();
            //    return false;
            //}
            else {
                $('#errormsgDiv').hide();

            }
        }

    </script>


</asp:Content>

