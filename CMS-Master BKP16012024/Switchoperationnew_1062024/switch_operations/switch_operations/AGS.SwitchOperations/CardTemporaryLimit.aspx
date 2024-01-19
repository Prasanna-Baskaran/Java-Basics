<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="~/CardTemporaryLimit.aspx.cs" Inherits="AGS.SwitchOperations.CardTemporaryLimit" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <link href="Styles/DatePickerBootStrap.css" rel="stylesheet" />
    <script src="Scripts/jquery.bootstrap.wizard.min.js"></script>
    <script src="Scripts/bootstrap-DatePicker.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    
    <asp:HiddenField ID="hdnAccessCaption" runat="server" />

    <div id="rootwizard" class="tabbable tabs-left">

        <div class="box-primary">
            <div class="box-header with-border">
                <i class="fa fa-credit-card"></i>
                <!-- start sheetal change card details to  Set card Limit-->
                <h4 class="box-title">Set Card limit :</h4>
                <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
                <!-- /.box-tools -->
            </div>
            <!--Display validation msg ------------------------------------------------------------------------->
                    <div class="pad margin no-print" id="errormsgDiv" style="display: none">
                        <div class="callout callout-info" style="margin-bottom: 0!important;">
                            <h4><i class="fa fa-info"></i>Information :</h4>
                            <span id="SpnErrorMsg" class="text-center"></span>
                        </div>
                    </div>

            <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
                <div class="modal-dialog modal-md">
                    <div class="modal-content" style="border-radius: 6px">
                        <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                            <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                            <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Set Temporary Limit Of Card</h4>
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

            <div class="box-body" id="SearchDiv">
                <div class="form-horizontal">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Card No:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="20" runat="server" required="required" name="CardNO" id="txtCardNo" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>From Date:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control datepicker" maxlength="20" required="required" runat="server" name="FromDate" id="txtFromDate" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>To Date:</label><%--//class="datepicker maxDateToday hasDatepicker//form-control datepicker--%>
                                <div class="col-sm-8">
                                    <input type="text"  maxlength="20" required="required" runat="server" name="ToDate" id="txtToDate" class="form-control datepicker" onchange="return ValidateEndDate();" /><%--class="form-control datepicker"--%>
                                </div>
                            </div>
                        </div>

                        <%--<div class="col-md-4">
                            <div class="form-group">
                                <div class="col-sm-4"></div>

                                <div class="col-sm-4">
                                    <div class="col-md-7">--%>
                        <%--<asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" OnClientClick="return FunValidation();" />--%>
                        <%--</div>
                                </div>
                                <div class="col-sm-4"></div>
                            </div>
                        </div>--%>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Per Day Limit:</label>
                                <div class="col-sm-7">
                                    <input type="text" class="form-control" maxlength="12" runat="server" required="required" name="PerDayLimit" id="txtPerDayLimit" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Per Transaction Limit:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="12" runat="server" required="required" name="PerTxnLimit" id="txtPerTxnLimit" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Per Day Count:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="2" runat="server" required="required" name="PerDayCount" id="txtPerDayCount" onkeypress="return FunChkIsNumber(event)" />
                                     
                                </div>
                            </div>
                        </div>
                        
                        
                        <%--<div class="col-md-4">
                            <div class="form-group">
                                <div class="col-sm-4"></div>

                                <div class="col-sm-4">
                                    <div class="col-md-7">--%>
                        <%--<asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" OnClientClick="return FunValidation();" />--%>
                        <%--</div>
                                </div>
                                <div class="col-sm-4"></div>
                            </div>
                        </div>--%>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>OverallLimit:</label>
                                <div class="col-sm-7">
                                    <input type="text" class="form-control" maxlength="12" runat="server" required="required" name="OverallLimit" id="txtOverallLimit" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>OverallCount:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" maxlength="2" runat="server" required="required" name="OverallCount" id="txtOverallCount" onkeypress="return FunChkIsNumber(event)" />
                                </div>
                            </div>
                        </div>
                        </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" OnClientClick="return FunValidation()" CssClass="btn-div btn-style btn btn-primary" Style="border: 0.5px solid #fff; border-radius: 4px; margin-top: -55px; margin-left: 84%; margin-right: auto;" />

        <div class="pull-right">
            <AGS:usrButtons runat="server" ID="userBtns" />
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


    

    <script>
        //Validation on Search Button
        function FunValidation() {
            
            if ($('#phPageBody_txtCardNo').val() == "") {
                $('#SpnErrorMsg').html('Please provide  CardNo');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_txtFromDate').val() == "") {
                $('#SpnErrorMsg').html('Please select From date');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_txtToDate').val() == "") {
                $('#SpnErrorMsg').html('Please select To date');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_txtPerDayLimit').val() == "") {
                $('#SpnErrorMsg').html('Please provide Per day limit');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_txtPerTxnLimit').val() == "") {
                $('#SpnErrorMsg').html('Please provide Per transaction limit');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_txtPerDayCount').val() == "") {
                $('#SpnErrorMsg').html('Please provide Per day count');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_txtOverallCount').val() == "") {
                $('#SpnErrorMsg').html('Please provide overall count');
                $('#errormsgDiv').show();
                return false;
            }
            if ($('#phPageBody_txtOverallLimit').val() == "") {
                $('#SpnErrorMsg').html('Please provide overall limit');
                $('#errormsgDiv').show();
                return false;
            }
            if (Date.parse($("[id$='txtFromDate']").val()) > Date.parse($("[id$='txtToDate']").val())) 
            {
                $('#SpnErrorMsg').html('Todate should be greater than Fromdate date');
                $('#errormsgDiv').show();
                return false;
            }
            if (parseInt($("[id$='txtPerDayLimit']").val()) < parseInt($("[id$='txtPerTxnLimit']").val())) {
                $('#SpnErrorMsg').html('Per day limit should be greater than Per transaction limit');
                $('#errormsgDiv').show();
                return false;
            }
            if (parseInt($("[id$='txtOverallLimit']").val()) < parseInt($("[id$='txtPerDayLimit']").val())) {
                $('#SpnErrorMsg').html('OverallLimit should be greater than PerDayLimit');
                $('#errormsgDiv').show();
                return false;
            }
            if (parseInt($("[id$='txtOverallCount']").val()) < parseInt($("[id$='txtPerDayCount']").val())) {
                $('#SpnErrorMsg').html('OverallCount should be greater than PerDayCount');
                $('#errormsgDiv').show();
                return false;
            }

            //try {
            //    var d1 = frm.txtFrmDate.value.substr(0, 2);
            //    var m1 = frm.txtFrmDate.value.substr(3, 2);
            //    var y1 = frm.txtFrmDate.value.substr(6, 4);
            //    var StrDate = m1 + "/" + d1 + "/" + y1;

            //    var d2 = frm.txtToDate.value.substr(0, 2);
            //    var m2 = frm.txtToDate.value.substr(3, 2);
            //    var y2 = frm.txtToDate.value.substr(6, 4);
            //    var EndDate = m2 + "/" + d2 + "/" + y2;

            //    var startDate = new Date(StrDate);
            //    var endDate = new Date(EndDate);
            //    if (startDate > endDate) {
            //        alert('To date should be greater than From date.');
            //        frm.txtFrmDate.value = '';
            //        frm.txtToDate.value = '';
            //        frm.txtFrmDate.focus();
            //        return false;
            //    }
            //} catch (e) { alert(e.Description); }

            else {
                $('#errormsgDiv').hide();
                $('.shader').fadeIn();
            }
        }
    </script>

    <script>


        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');
            //window.location.reload();

        }

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
        $("#phPageBody_userBtns_btnSubmit_U").click(function () {
            ErrorCount = 0
            var errrorTextDD = 'Please provide ';
            var errrorTextSectionDD = ' in Documentation.';
            var errorFieldsDD = '';

            if (($('#phPageBody_PhotoUpload').val() != "") || ($('#phPageBody_SignatureUpload').val() != "") || ($('#phPageBody_IDProofUpload').val() != "")) {
                var Photoext = $('#phPageBody_PhotoUpload').val().split('.').pop().toLowerCase();

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
            }
        });
    </script>

    <script>
        $(document).ready(function () {

           // $('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true, endDate: new Date() });
            $('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true, startDate: new Date() });
            //$('.datepicker').attr('min')

            //$(".datepicker").datepicker({
            //    startDate: new Date()
            //});

        });
    </script>
    <script>
    function ValidateEndDate() {
        //var startDate = $("[id$='txtFromDate']").val();
        //var endDate = $("[id$='txtToDate']").val();
        if ($("[id$='txtFromDate']").val() != '' && $("[id$='txtToDate']").val() !='') {
            if (Date.parse($("[id$='txtFromDate']").val()) > Date.parse($("[id$='txtToDate']").val())) {
               $("[id$='txtToDate']").val('');
               alert("Todate should be greater than Fromdate date");
               return false;
           }
       }
       return true;
  }
        </script>
    <%--<script type="text/javascript">
        $(function () {
            $("#txtFromDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                yearRange: new Date().getFullYear().toString() + ':' + new Date().getFullYear().toString(),
                onClose: function (selectedDate) {
                    $("#txtToDate").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#txtToDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                yearRange: new Date().getFullYear().toString() + ':' + new Date().getFullYear().toString(),
                onClose: function (selectedDate) {
                    $("#txtFromDate").datepicker("option", "maxDate", selectedDate);
                }
            });
        });
</script>--%>
    <%--<script type="text/javascript">
        $(function () {
            $('.datepicker').datepicker();
            $("#txtToDate").datepicker({
                numberOfMonths: 12,
                on: ('change', function () {
                    var date = Date.parse($(this).val());
                    if (date < Date.now()) {
                        alert('Selected date must be greater than today date');
                        $(this).val('');
                    }
                })
            });
            //$("#txtToDate").datepicker({
            //    numberOfMonths: 12,
            //    onSelect: function (selected) {
            //        var dt = new Date(selected);
            //        dt.setDate(dt.getDate() - 1);
            //        $("#txtFromDate").datepicker("option", "maxDate", dt);
            //    }
            //});
        });
</script>--%>
</asp:Content>
