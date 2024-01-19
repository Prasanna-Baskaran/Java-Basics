<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="InstitutionMaster.aspx.cs" Inherits="AGS.SwitchOperations.InstitutionMaster" %>


<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">
    <!------------HIDDEN FIELDS-------------------------------->
    <input type="hidden" name="ResultStatus" id="hdnResultStatus" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <input type="hidden" name="ID" id="hdnID" runat="server" />
    <asp:HiddenField runat="server" ID="hdnAccessCaption" />

    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">Institution Master </h2>
        </div>


        <%--//divresult--%>
        <div class="box-body" id="DivResult">
            <div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
                    <%--start sheetal data-target="#AddEditModal" property added to btnaddnew--%>
                    <input type="button" class="btn btn-primary pull-right" id="BtnAddNew" data-target="#AddEditModal" value="Add New" onclick="funAddNew()" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="box-primary">
                        <!-- /.box-header -->
                        <div class="box-body no-padding">
                            <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                <div class="col-md-12">
                                    <div class="x_panel">
                                        <div>
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

    </div>

    <!-- Modal HTML -->
    <%-- Response Msg --%>
    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Institution Master</h4>
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

    <%-- Edit Modal --%>
    <!--start sheetal data backdrop property added as static to show model even click on window -->
    <div id="AddEditModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModaldelete" class="modal-title" style="font-weight: bold">Institution Information</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <label for="inputName" class="col-xs-4 control-label">InstitutionID:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="txtINSTID" runat="server" maxlength="20" onkeypress="return onlyAlphabets(event,this);" />
                        </div>
                    </div>
                    <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <label for="inputName" class="col-xs-4 control-label">Institution Description:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="txtINSTDesc" runat="server" maxlength="30" onkeypress="return onlyAlphabets(event,this);" />
                        </div>
                    </div>
                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" onclick="funCancelModal()" style="margin-right: 10px;" type="button"><span aria-hidden="true">CANCEL</span></button>
                        </div>
                        <div class="col-sm-5">
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="SAVE" Style="width: 63px" ID="SaveBtn" OnClick="btnSave_Click" OnClientClick="return funShowLoader();" />
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>




    <%---************************** SCRIPT *************************----%>
    <script>
        $(document).ready(function () {
            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');
            //check  user has Edit right
            if ($.inArray("E", ($('#<%=hdnAccessCaption.ClientID%>').val().split(","))) > -1) {
                $('#datatable-buttons input[type="button"][value="Edit"]').show();
            }
            else {
                $('#datatable-buttons input[type="button"][value="Edit"]').hide();
            }
            //check user has Add rights
            if ($.inArray("A", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                $('#BtnAddNew').show();
            }
            else {
                $('#BtnAddNew').hide();
            }


        });
    </script>
    <%-- Datatable fun----%>
    <script>
        $(document).ready(function () {
            //For Data Table

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

                $('#datatable').dataTable();
                $('#datatable-keytable').DataTable({
                    keys: true
                });

                $('#datatable-responsive').DataTable();

                $('#datatable-scroller').DataTable({
                    ajax: "js/datatables/json/scroller-demo.json",
                    deferRender: true,
                    scrollY: 380,
                    scrollCollapse: true,
                    scroller: true
                });

                var table = $('#datatable-fixed-header').DataTable({
                    fixedHeader: true
                });

                TableManageButtons.init();
            }
        });
    </script>


    <%----************************ Click Function ************************--%>
    <script>
    
    </script>


    <%--******************************** //Message Box *********************--%>
    <script>
        //response msg
        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');

            //on success
            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                $('input[type="text"]').val('');

            }
                //on update fail
            else if ($('#phPageBody_hdnResultStatus').val() == 2) {
                $('#AddEditModal').css("display", "");
                $('#DivResult').css("display", "none");
            }


        }

        //Add New Click
        function funAddNew() {
            $('#phPageBody_txtINSTID').val('');

            $('#phPageBody_txtINSTDesc').val('');

            $('#phPageBody_hdnID').val('');

            $('[id$="txtINSTID"]').attr('required', true)
            $('[id$="txtINSTDesc"]').attr('required', true)

            $('#AddEditModal').modal('show')
        }

        //Edit click
        function funEditClick(obj) {
            //var tds = $(obj).parent().parent().find('td');
            //var ID = $(tds[0]).text();
            //var INSTID = $(tds[1]).text();
            //var INSTIDesc = $(tds[2]).text();

            var ID = $(obj).attr('id');
            var INSTID = $(obj).attr('instid');
            var INSTIDesc = $(obj).attr('instiddesc');

            $('#phPageBody_txtINSTID').val(INSTID);

            $('#phPageBody_txtINSTDesc').val(INSTIDesc);

            $('#phPageBody_hdnID').val(ID);

            $('[id$="txtINSTID"]').attr('required', true)
            $('[id$="txtINSTDesc"]').attr('required', true)
            $('#AddEditModal').modal('show')
        }

        //Cancel edit modal
        function funCancelModal() {
            $('[id$="txtINSTID"]').attr('required', false)
            $('[id$="txtINSTDesc"]').attr('required', false)
            $('#AddEditModal').modal('hide')
        }
    </script>
    <%-- Show Loader --%>
    <script>
        function funShowLoader() {
            if ($('#phPageBody_txtINSTID').val() != "" && $('#phPageBody_txtINSTDesc').val() != "") {
                $('.shader').fadeIn();
                return true;
            }
            
        }
    </script>

</asp:Content>
