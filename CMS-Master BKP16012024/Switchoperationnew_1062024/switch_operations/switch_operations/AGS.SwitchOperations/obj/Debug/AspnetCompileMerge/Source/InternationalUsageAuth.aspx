<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="InternationalUsageAuth.aspx.cs" Inherits="AGS.SwitchOperations.InternationalUsageAuth" %>
<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <style type="text/css">
        .auto-style1 {
            position: relative;
            min-height: 1px;
            float: left;
            width: 100%;
            padding-left: 10px;
            padding-right: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
         <asp:HiddenField ID="hdnAllSelectedValues" runat="server" />
                <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
                <asp:HiddenField ID="hdnRequestIDs" runat="server" />
     <asp:Panel ID="pnlCustomerDtl" runat="server">
    <div class="box-header with-border">
                <i class="fa"></i>
                <!-- start sheetal card operations changes to accept card request-->
                <h4 class="box-title">International Usage Authorization :</h4>
                <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
                <!-- /.box-tools -->
            </div>

      <div id="DivResult">

                    <div class="row">
                        <div class="auto-style1">
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
                                                <asp:Button runat="server" ID="BtnSave" Text="Approve" OnClick="BtnSave_Click" style="height: 26px" OnClientClick="return $('.shader').fadeIn();" />
                                                <asp:Button runat="server" ID="BtnReject" Text="Reject" style="height: 26px" OnClick="BtnReject_Click" OnClientClick="return $('.shader').fadeIn();" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                                  

                 <div class="row" id="DivResultMsg">
                <div class="callout callout-info" style="margin-bottom: 0!important;">
                <h4><i class="fa fa-info"></i>Information :</h4>
                <label runat="server" name="Name" id="LblResult" readonly="readonly" class="text-center" />
                    <label runat="server" name="Name" id="LblSuccess" readonly="readonly" class="text-center" />
                    <label runat="server" name="Name" id="LblFail" readonly="readonly" class="text-center" />
                </div>
             </div>
        </div>
 <%-- Response Msg --%>
    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">International Usage Authorization.</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <asp:Label ID="LblMessage" runat="server"></asp:Label>
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

            //$('input[type=checkbox][formstatus=1]').attr('disabled', true)

            //$('input[type=checkbox][formstatus=2]').attr('disabled', true)
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




            //$("#select_all").click(function (event) {  //"select all" change 
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
                    $("#select_all").prop('checked', true);
                }
                else {
                    $("#select_all").prop('checked', false);
                }
            })
            $('[id$="BtnReject"]').click(function () {
                debugger;
                var allids = "";
                $('#datatable-buttons tbody').find('tr').each(function () {
                    if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                        var id = $(this).find('td').next().html() + "|" + $(this).find('td:eq(2)').text() + "|" + $(this).find('td:eq(3)').text();
                        if (allids == "") {
                            allids = id;
                        } else {
                            allids = allids + "," + id;
                        }
                    }
                });
           
                if (allids == '') {
                    document.getElementById('phPageBody_LblMessage').innerHTML = 'Please check at least one checkbox to reject.'
                    FunShowMsg();
                    return false;
                }
                else {
                    $('[id$="hdnAllSelectedValues"]').val(allids);
                }
            });

            $('[id$="BtnSave"]').click(function () {
                debugger;
                var allids = "";
                $('#datatable-buttons tbody').find('tr').each(function () {
                    if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                        var id = $(this).find('td').next().html() + "|" + $(this).find('td:eq(2)').text() + "|" + $(this).find('td:eq(3)').text();
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
            });


        });
        function FunShowMsg() {
            $('.shader').fadeOut();
            $('#memberModal').modal('show');
        }
        
       
        </script>
    
</asp:Content>
