<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="CourierMaster.aspx.cs" Inherits="AGS.SwitchOperations.CourierMaster" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">

    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField runat="server" ID="hdnAccessCaption" />
    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">Courier Details </h2>
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
    <div id="shader" class="shader">
        <div class="loadingContainer">
            <div id="divLoading3">
                <div id="loader-wrapper">
                    <div id="loader"></div>
                </div>
            </div>
        </div>
    </div>


    <%-- Edit Modal --%>
    <!--start sheetal data backdrop property added as static to show model even click on window -->
    <div id="AddEditModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px; width: 797px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">Courier Details</h4>
                </div>

                <!--Display validation msg ------------------------------------------------------------------------->
                <div class="pad margin no-print" id="errormsgDiv" style="display: none">
                    <div class="callout callout-info" style="margin-bottom: 0!important;">
                        <h4><i class="fa fa-info"></i>Information :</h4>
                        <span id="SpnErrorMsg" class="text-center"></span>
                    </div>
                </div>

                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Courier Name:</label>
                            <div class="col-md-7">
                                <input type="text" class="form-control" id="txt_couriername" runat="server" placeholder="" maxlength="20" onkeypress="return onlyAlphabets(this,event)" autocomplete="off" />

                            </div>
                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Office :</label>
                            <div class="col-md-7">
                                <input type="text" class="form-control" id="txt_officename" runat="server" placeholder="" maxlength="20" onkeypress="return onlyAlphabets(this,event)" autocomplete="off" />

                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <br />
                    </div>

                    <div class="row">

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Contact Number:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txt_mobileno" runat="server" placeholder="" maxlength="10" onkeypress="return FunChkIsNumber(event)" autocomplete="off" />

                            </div>
                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;"></span>Status</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="dropdown_status" runat="server" class="form-control" Style="width: 100%">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="In-Active" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>

                        <div class="col-md-6" hidden>

                            <input type="text" class="form-control" id="txt_courierid" runat="server" />
                        </div>


                    </div>

                    <div class="row">
                        <br />
                    </div>
                    <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <div class="col-sm-6">
                            <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" onclick="funCancelModal()" style="margin-right: 10px;" type="button"><span aria-hidden="true">CANCEL</span></button>
                        </div>
                        <div class="col-sm-5">
                            <asp:Button runat="server" CssClass="btn btn-primary pull-left" Text="SAVE" Style="width: 69px" ID="AddBtn" OnClick="add_Click" />
                        </div>

                    </div>
                </div>
            </div>

        </div>
    </div>

    <!-- ErrorMsg start -->
    <div id="ErrorModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">User Details Master</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                    <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" type="button"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
    </div>
    <!-- ErrorMsg  end-->
    <script>
        $(document).ready(function () {

            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');
            //$("#datatable-buttons tr :nth-child(5)").hide();
            //$("#datatable-buttons tr :nth-child(6)").hide();
            ////check user has Add rights
            if ($.inArray("A", $('#<%=hdnAccessCaption.ClientID%>').val().split(",")) > -1) {
                $('#BtnAddNew').show();
            }
            else {
                $('#BtnAddNew').hide();
            }

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
    <script>
        $("#<%=AddBtn.ClientID %>").click(function () {
            var errrorTextPD = 'Please provide ';
            var errorFieldsPD = '';

            if ($("#phPageBody_txt_couriername").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Courier Name</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Courier Name</b> ';
                }
                $('#<%=txt_couriername.ClientID%>').focus();
            }

            if ($("#phPageBody_txt_officename").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Office Name</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>Office Name</b> ';
                }
                $('#<%=txt_officename.ClientID%>').focus();
            }



            if ($("#phPageBody_txt_mobileno").val().length != "10") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>,valid Contact Number</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>valid Contact Number</b> ';
                }
                $('#<%=txt_mobileno.ClientID%>').focus();
            }

            if ($("#phPageBody_dropdown_status").val() == "0") {
                errortab = '1';

                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, Status</b> ';

                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b> Status</b> ';

                }

                $('#<%=dropdown_status.ClientID%>').focus();
            }


            if (errorFieldsPD != '') {
                $('#SpnErrorMsg').html(errrorTextPD + errorFieldsPD);
                $('#errormsgDiv').show();
                return false;
            }
            else {
                errorFieldsPD = '';
                errrorTextSectionPD = '';
                errrorTextPD = '';
                $('.shader').fadeIn();
                $('#errormsgDiv').hide();
            }
        });

    </script>
    <script>

        function funAddNew() {
            //alert('new');
            $('[id$="txt_couriername"]').val('');
            $('[id$="txt_officename"]').val('');
            $('[id$="txt_mobileno"]').val('');
            $('#<%=dropdown_status.ClientID%>').val('0');
            $('#AddEditModal').modal('show');

        }
    </script>
    <script>
        function FunShowMsg() {

            $('#ErrorModal').modal('show');
        }
    </script>
    <script>

        function funCancelModal() {


            $('#errormsgDiv').hide();
            $('[id$="txt_couriername"]').val('');
            $('[id$="txt_officename"]').val('');
            $('[id$="txt_mobileno"]').val('');
            $("#<%=dropdown_status.ClientID%>")[0].selectedIndex = 0;
            $('[id$="txt_courierid"]').val('');
            $('#AddEditModal').modal('hide')

        }
    </script>

    <script>
        function funedit(obj) {



            //var tds = $(btn).parent().parent().find('td');
            //var scouriername = $(tds[0]).text();
            //var sofficename = $(tds[1]).text();
            //var scontactno = $(tds[2]).text();
            //var sstatus = $(tds[4]).text();

            //var scourierid = $(tds[5]).text();

            var scouriername = $(obj).attr('couriername')
            var sofficename = $(obj).attr('office')
            var scontactno = $(obj).attr('contact')
            var sstatus = $(obj).attr('statusid')
            var scourierid = $(obj).attr('id')

            $('[id$="txt_couriername"]').val(scouriername);
            $('[id$="txt_officename"]').val(sofficename);
            $('[id$="txt_mobileno"]').val(scontactno);
            $("#phPageBody_dropdown_status").val(sstatus);
            $('[id$="txt_courierid"]').val(scourierid);

            $('#AddEditModal').modal('show');

        }
    </script>
    <script>
        function FunShowMsg() {

            $('#ErrorModal').modal('show');
        }
    </script>
    <%-- Show Loader --%>
    <script>
        function funShowLoader() {
            $('.shader').fadeIn();
            return true;
        }
    </script>

</asp:Content>
