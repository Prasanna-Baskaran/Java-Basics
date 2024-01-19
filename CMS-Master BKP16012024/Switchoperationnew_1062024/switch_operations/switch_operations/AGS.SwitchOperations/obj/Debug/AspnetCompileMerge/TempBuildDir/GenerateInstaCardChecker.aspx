<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="GenerateInstaCardChecker.aspx.cs" Inherits="AGS.SwitchOperations.GenerateInstaCardChecker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
               <asp:HiddenField ID="hdnAllSelectedValues" runat="server" />
                <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
                <asp:HiddenField ID="hdnRequestIDs" runat="server" />
         <asp:Panel ID="pnlCustomerDtl" runat="server">
    <div class="box-header with-border">
                <i class="fa"></i>
                <h4 class="box-title">Insta Cards Authorization :</h4>
                <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
                <!-- /.box-tools -->
            </div>

      <div id="DivResult">
                          <div class="row" id="DivResultMsg">
                <div class="callout callout-info" style="margin-bottom: 0!important;">
                <h4><i class="fa fa-info"></i>Information :</h4>
                <label runat="server" name="Name" id="LblResult" readonly="readonly" class="text-center" />

                </div>
             </div>
                    <div class="row">
                     <%--   <div class="auto-style1">--%>
                            <div class="box-primary">
                                <!-- /.box-header -->
                                <div class="box-body no-padding">
                                    <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                        <div class="col-md-12">
                                            <div class="x_panel">
                                                <div>
                                                    <div id="SelectAllDiv">
                                                        <input id="select_all" runat="server" type="checkbox" />
                                                        <label runat="server" name="Name_select_all" id="LBLselect_all" readonly="readonly"> SelectAll</label>                                             
                                                    </div>
                                                    <div class="clearfix"></div>
                                                </div>

                                                <div class="x_content">

                                                    <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%;">
                                                    </table>
                                                </div>
                                                <asp:Button runat="server" ID="BtnSave" Text="Approve" OnClick="BtnSave_Click" class="btn-div btn-style btn btn-primary" OnClientClick="return $('.shader').fadeIn();" />
                                                <%--<asp:Button runat="server" ID="BtnReject" Text="Reject" style="height: 26px" OnClick="return funGetResult(2)" OnClientClick="return $('.shader').fadeIn();" />--%><%-- BtnReject_Click--%>
                                           <input type="button" value="Reject" onclick="funGetResult(2)" id="BtnReject" class="btn-div btn-style btn btn-primary" />
                                                                                            <%--<asp:Button runat="server" ID="Button1" Text="Reject" style="height: 26px" OnClick="return funGetResult(2)" OnClientClick="return $('.shader').fadeIn();" />--%><%-- BtnReject_Click--%>
    
                                            
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        <%--</div>--%>
                    </div>
                                  

<%--                 <div class="row" id="DivResultMsg">
                <div class="callout callout-info" style="margin-bottom: 0!important;">
                <h4><i class="fa fa-info"></i>Information :</h4>

                </div>
             </div>--%>
        </div>


          <div id="RejectConfirmationModal" class="modal fade" data-backdrop="static" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content -->
                <div class="modal-content" style="border-radius: 4px;">

                    <div class="modal-header">
                        <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                        <h4 id="myLargeRejectModal" class="modal-title" style="font-weight: bold">Confirmation</h4>
                    </div>
                    <%--//start Diksha--%>
                    <!--Display validation msg ------------------------------------------------------------------------->
        <%--            <div class="pad margin no-print" id="RejecterrormsgDiv" style="display: none">
                        <div class="callout callout-info" style="margin-bottom: 0!important;">
                            <h4><i class="fa fa-info"></i>Information :</h4>
                            <span id="SpnRejectErrorMsg" class="text-center"></span>
                        </div>
                    </div>--%>
                    <div class="modal-body">

               <%--         <div class="row">

                            <label for="inputName" class="col-xs-5 control-label"><span style="color: red;">*</span>Do you want to reject this request?:</label>
                            <div class="col-sm-6">
                                <input type='radio' name='IsConfirm' value='1' />Yes
                        &nbsp;&nbsp;<input type='radio' name='IsConfirm' value='2' aria-label="Close" data-dismiss="modal" onclick="funCancelModal()" />No
                            </div>
                        </div>--%>
                    <%--    <div class="row">
                            <br />
                        </div>--%>
                        <div class="row" id="remarkDiv">
                            <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>Reason:</label>
                            <div class="col-sm-7">
                                <asp:TextBox runat="server" ID="txtRejectReson" CssClass="form-control" TextMode="MultiLine" MaxLength="50" onkeypress="return onlyAlphabets(event,this);"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <br />
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" style="margin-right: 10px;" type="button" onclick="funCancelModal()"><span aria-hidden="true">CANCEL</span></button>
                            </div>
                            <div class="col-md-6">
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Confirm" ID="Reject_Btn" OnClick="Reject_Btn_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



 <%-- Response Msg --%>
    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Insta cards Authorization.</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <%--<asp:Label ID="LblMessage" runat="server"></asp:Label>--%>
                      <label runat="server" name="Name" id="LblMessage" readonly="readonly"/>
                </div>
                <div class="modal-footer">
                    <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
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

          </asp:Panel>

    <script>
        $(document).ready(function () {
            //Start   Dynamic datatable bind
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');

            //Hide checkbox when request is  accepted/Rejected

            //$('input[type=checkbox][formstatus="File generated"]').attr('disabled', true)

            //$('input[type=checkbox][formstatus="Authorised"]').attr('disabled', true)
            //$('input[type=checkbox][formstatus="Rejected"]').attr('disabled', true)


            $('input[type=checkbox][formstatus=1]').attr('disabled', true)
            //If record found 
    <%--        if ($("#<%=hdnResultStatus.ClientID%>").val() == "1") {

                $("#DivResult").show()

                //If user has Accept right
                if ($.inArray("C", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                    $('#AuthDiv').show();
                    $('#SelectAllDiv').show()


                }
                else {
                    $('#SelectAllDiv').hide()
                    $('#ModalAuthDiv').hide()

                }


            }
            else {

                $("#DivResult").hide()
            }--%>





            //For Data Table
            if ($("#datatable-buttons tbody tr").length > 0) {
           

                //TableManageButtons.init();
                var handleDataTableButtons = function () {
                    if ($("#datatable-buttons").length) {
                        $("#datatable-buttons").DataTable({
                            "info": true,

                            scrollX: true
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

            }
            //End   Dynamic datatable bind

            ////Hide FormStatusID
            //$("#datatable-buttons tr :nth-child(6)").hide()





            // ************* Select All function *******
            $('[id$="select_all"]').click(function (event) {  //"select all" change 

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
                    $('[id$="select_all"]').prop('checked', true);
                }
                else {
                    $('[id$="select_all"]').prop('checked', false);
                }
            })
            //$('[id$="BtnReject"]').click(function () {
            //    debugger;
            //    var allids = "";
            //    $('#datatable-buttons tbody').find('tr').each(function () {
            //        if ($(this).find('input[type="checkbox"]:checked').length > 0) {
            //            var id = $(this).find('td').next().html() //+ "|" + $(this).find('td:eq(2)').text() + "|" + $(this).find('td:eq(3)').text();
            //            if (allids == "") {
            //                allids = id;
            //            } else {
            //                allids = allids + "," + id;
            //            }
            //        }
            //    });
           
            //    if (allids == '') {
            //        document.getElementById('phPageBody_LblMessage').innerHTML = 'Please check at least one checkbox to reject.'
            //        FunShowMsg();
            //        return false;
            //    }
            //    else {
            //        $('[id$="hdnAllSelectedValues"]').val(allids);
            //    }
            //});

            $('[id$="BtnSave"]').click(function () {
                debugger;
                var allids = "";
                $('#datatable-buttons tbody').find('tr').each(function () {
                    if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                        var id = $(this).find('td').next().html() //+ "|" + $(this).find('td:eq(2)').text() + "|" + $(this).find('td:eq(3)').text();
                        if (allids == "") {
                            allids = id;
                        } else {
                            allids = allids + "," + id;
                        }
                    }
                });

                if (allids == '') {
                    document.getElementById('phPageBody_LblMessage').innerHTML = 'Please check at least one checkbox to approve.'
                    FunShowMsg();
                    return false;
                }
                else {
                    $('[id$="hdnAllSelectedValues"]').val(allids);
                }
                //FunShowMsg();
            });



      

            $('#phPageBody_Reject_Btn').click(function () {
                var errrorTextPD = 'Please provide ';
                var errorFieldsPD = '';
                //if (!($('input[name="IsConfirm"]').is(':checked'))) {
                //    errorFieldsPD = errorFieldsPD + (errorFieldsPD != '' ? '<b>,' : '<b>') + ' option(Yes/No) </b> ';
                //}

                if ($('#phPageBody_txtRejectReson').val() == "") {
                    errorFieldsPD = errorFieldsPD + (errorFieldsPD != '' ? '<b>,' : '<b>') + ' Reason</b> ';
                }


                if (errorFieldsPD != '') {
                    document.getElementById('phPageBody_LblMessage').innerHTML = errrorTextPD + errorFieldsPD;
                    FunShowMsg();


                    //$('#SpnRejectErrorMsg').html(errrorTextPD + errorFieldsPD);
                    //$('#RejecterrormsgDiv').show();
                    return false;
                }
                else {
                    errorFieldsPD = '';
                    errrorTextSectionPD = '';
                    errrorTextPD = '';
                    $('.shader').fadeIn();
                    //$('#errormsgDiv').hide();
                }

            });



        });
        //$('#memberModal').modal({
        //    backdrop: 'static',
        //    keyboard: false
        //})

        function funGetResult(FormstatusID) {


            if ($('#datatable-buttons tbody input[type=checkbox]:checked').length == 0) {
                document.getElementById('phPageBody_LblMessage').innerHTML = "Please select Card Requests to accept/reject";
                FunShowMsg();
                return false;
            }
            else {
                document.getElementById('phPageBody_LblMessage').innerHTML = "";
                //$('#errormsgDiv').hide();
       <%--         $("#<%=hdnFormStatusID.ClientID%>").val(FormstatusID);
                $("#<%=hdnReqType.ClientID%>").val($($('#datatable-buttons tbody input[type=checkbox]:checked')[0]).attr('reqTypeId'));--%>

                    var ArrayIds = [];
                    $('#datatable-buttons tbody input[type=checkbox]:checked').each(function (i) {
                        ArrayIds[i] = $(this).attr("ReqID");

                    });
                    //alert(val.join(","))

                    $("#<%=hdnAllSelectedValues.ClientID%>").val(ArrayIds.join(","))

                //for Reject request
                if (FormstatusID == "2") {

                    // $('[id$="txtRejectReson"]').attr('required', true)
                    $('#RejectConfirmationModal').modal('show');

                }
                else {
                    $('.shader').fadeIn();
                }
                return true;
            }
        }

        function FunShowinfo()
        {
            $('[id$="DivResultMsg"]').show();
        }

        function FunHideinfo() {
            $('[id$="DivResultMsg"]').hide();
        }

        function FunShowMsg() {
            $('.shader').fadeOut();
            $('#memberModal').modal('show');
        }
        //function Hidemodel() {
        //    //7-11
        //    $('#memberModal').modal('hide');
        //}
     

        //Cancel Reject Confirmation modal
        function funCancelModal() {

            //$('[id$="txtRejectReson"]').attr('required', false)
            $('input[name="IsConfirm"]').attr("checked", false)
            $('#phPageBody_txtRejectReson').val('')
            $('input[name="IsConfirm"]').val(0)
        }
        </script>

</asp:Content>
