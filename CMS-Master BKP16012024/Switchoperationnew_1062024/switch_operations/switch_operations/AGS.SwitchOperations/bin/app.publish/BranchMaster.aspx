<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="BranchMaster.aspx.cs" Inherits="AGS.SwitchOperations.BranchMaster" %>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">
    <link href="Styles/responsive.bootstrap.css" rel="stylesheet" />

    <script src="Scripts/dataTables.responsive.min.js"></script>
    <script src="Scripts/responsive.bootstrap.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">

    <input type="hidden" name="ResultStatus" id="hdnResultStatus" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnSystems" runat="server" />
    <asp:HiddenField runat="server" ID="hdnAccessCaption" />
    <asp:HiddenField runat="server" ID="LoginUserSystems" />
    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">Branch Detail </h2>
        </div>

        <%--//divresult--%>
        <div class="box-body" id="DivResult">
            <div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
                    <%--start sheetal data-target="#AddEditModal" property added to btnaddnew--%>
                    <input type="button" class="btn btn-primary pull-right" id="BtnAddNew" data-targrt="#AddEditModal" value="Add New" onclick="funAddNew()" />
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
    <div id="AddEditModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px; width: 797px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">Branch Details</h4>
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
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>Branch Code:</label>
              
                                <div class="col-md-7">
                                    <input type="Text" class="form-control" id="txt_Branchcode" runat="server" placeholder="Branch Code" maxlength="10" autocomplete="off" onkeypress="return FunChkIsNumber(event)" />
                                </div>

                          </div>

                          </div>
                         <div class="row">
                        <br />
                    </div>

                         <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>Branch Name:</label>
                            <div class="col-md-7">
                                <input type="text" class="form-control" id="txt_Branchname" runat="server" placeholder="Enter Branch Name" maxlength="20" onkeypress="return FunChkAlphaNumeric(event)" autocomplete="off" />

                            </div>
                        </div>




                       <div class="col-md-6" hidden style="border-spacing: 5px">
                            <input type="text" class="form-control" id="txt_Branchid" runat="server" />
                        </div>
                    </div>

               
                    <div class="row">
                        <br />
                    </div>
                       <div class="row">
                         <div class="col-md-6">
                         <label for="inputName" class="col-sm-5 control-label"><span style="color: red;">*</span>ACTIVE:</label>
                            <div class="col-md-7 align">
                                <asp:CHECKBOX ID="ACTIVE" runat="server" RepeatDirection="Horizontal" CssClass="flat" Style="" CellSpacing="1" CellPadding="1">
                                </asp:CHECKBOX>
                            </div>
                         </div>
                           </div>

                        <div class="row">
                        <br />
                    </div>

                    <div class="row">

                        <div class="col-sm-3">
                        </div>

                        <div class="col-sm-6 text-center">
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="SAVE" Style="width: 69px" ID="AddBtn" OnClick="add_Click" />
                            <button aria-label="Close" data-dismiss="modal" class="btn btn-primary" onclick="funCancelModal()" type="button"><span aria-hidden="true">CANCEL</span></button>
                            <input type="button" id="BtnReset" aria-label="Reset" class="btn btn-primary" value="Reset" onclick="fnreset();" />
                        </div>

                        <div class="col-sm-3">
                        </div>
                    </div>
                </div>
            </div>

        </div>

    </div>

    <!-- ErrorMsg start -->
    <div id="ErrorModal" class="modal fade bs-example-modal-lg " tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" data-backdrop="static">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Branch Details </h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                    <button aria-label="Close" class="btn btn-primary pull-right" type="button" onclick="funClearErrmsg()">OK</button>
                </div>
            </div>
        </div>
    </div>


    <!-- ErrorMsg  end-->
    <script>
        $(document).ready(function () {
            $(function () {
                $('[data-toggle="tooltip"]').tooltip()
            });

            //Add UserPrefix
        <%--    var UserPrefix= '<%=HttpContext.Current.Session["UserPrefix"]%>''
            $('#spnUserPrefix').html(UserPrefix)--%>
            $("#spnUserPrefix").html('<%=HttpContext.Current.Session["UserPrefix"]%>')



            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');
            //$("#datatable-buttons tr :nth-child(8)").hide();
            //$("#datatable-buttons tr :nth-child(9)").hide();
            //$("#datatable-buttons tr :nth-child(10)").hide();
            //$("#datatable-buttons tr :nth-child(11)").hide();
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

                //$('#datatable').dataTable();
                //$('#datatable-keytable').DataTable({
                //    keys: true
                //});

                //$('#datatable-responsive').DataTable();

                //$('#datatable-scroller').DataTable({
                //    ajax: "js/datatables/json/scroller-demo.json",
                //    deferRender: true,
                //    scrollY: 380,
                //    scrollCollapse: true,
                //    scroller: true
                //});



                TableManageButtons.init();
                $('#datatable-buttons tbody input[type=button][isedit=0][value=Edit]').hide()
            }

            //$("#phPageBody_SystemList").find("td").css("padding-right", "10px")



        });
    </script>
    <script>
        $("#<%=AddBtn.ClientID %>").click(function () {
            var errrorTextPD = 'Please provide :  ';
            var errorFieldsPD = '';
            var Branchid = $('[id$="txt_Branchid"]').val();

            if (Branchid == "" || Branchid == null) {

                if ($("#phPageBody_txt_Branchname").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, Branch Name</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Branch Name</b> ';
                    }
                    $('#<%=txt_Branchname.ClientID%>').focus();
                }

               

                if ($("#phPageBody_txt_Branchcode").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, Branch code</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Branch code</b> ';
                    }
                    $('#<%=txt_Branchcode.ClientID%>').focus();
                }

                if ($("#phPageBody_ACTIVE").prop('checked') == false)
                    //if ($("[id$=ACTIVE]:checked").length == "0")
                    {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, ACTIVE</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>ACTIVE</b> ';
                    }
                    $('#<%=ACTIVE.ClientID%>').focus();
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
                    $('#errormsgDiv').hide();

             
                }

            }

            else {


                if ($("#phPageBody_txt_Branchname").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, Branch Name</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Branch Name</b> ';
                    }
                    $('#<%=txt_Branchname.ClientID%>').focus();
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
                      $('#errormsgDiv').hide();
                    $('.shader').fadeIn();
                }


            }

        });

    </script>
    <script>

        function funAddNew() {

            $('[id$="txt_Branchname"]').val('');
            $('[id$="txt_Branchcode"]').val(''); 
            $('[id$="txt_Branchid"]').val('');
            //$('id$="ACTIVE"').prop('checked', false);
            $('[id$="txt_Branchcode"]').prop('readonly', false);
            $("[id*=ACTIVE] input").removeAttr("checked")
            $('#AddEditModal').modal('show');
        }
    </script>

    <script>
        function funedit(obj) {

            $('[id$="txt_Branchname"]').val('');
            $('[id$="txt_Branchcode"]').val('');
            $('[id$="txt_Branchid"]').val('');
            //$('id$="ACTIVE"').prop('checked', false);

            var sBranchName = $(obj).attr('BranchName');
            var sBranchCode = $(obj).attr('BranchCode');
            var sBranchId = $(obj).attr('id');
            var ACTIVE = $(obj).attr('ACTIVE');

            $('[id$="txt_Branchname"]').val(sBranchName);
            $('[id$="txt_Branchcode"]').val(sBranchCode);
            $('[id$="txt_Branchid"]').val(sBranchId);
            $('[id$="txt_Branchcode"]').prop('readonly', true);
            if (ACTIVE == 'True')
            {
                $('#<%=ACTIVE.ClientID%>').prop('checked', true);
                //$('id$="ACTIVE"').prop('checked', true);
            } else {
                $('#<%=ACTIVE.ClientID%>').prop('checked', false);
            }
            //$("[id$=ACTIVE]").prop("checked", "checked");
            
            //$($(obj).attr('ACTIVE').split(",")).each(function (i, item) {
            //    $("table[id*=ACTIVE] input[value=" + $.trim(item) + "]").prop("checked", "checked");
            //});
            $('#AddEditModal').modal('show');
            $('#BtnReset').hide();
        }
    </script>
    <script>
        function FunShowMsg() {

            $('#ErrorModal').modal('show');

        }


        

    </script>

     <script>
         function fnreset() {

            $('#ErrorModal').modal('hide');
            $('[id$="txt_Branchname"]').val('');
            $('[id$="txt_Branchcode"]').val('');
            $('[id$="txt_Branchcode"]').prop('readonly', false);
            $('#<%=ACTIVE.ClientID%>').prop('checked', false);
        }
  </script>

    <script>
        function funClearErrmsg() {
            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                $('#AddEditModal').modal('show');
       
                $('#ErrorModal').modal('hide');
            }
            else {
                $('#ErrorModal').modal('hide');
                $('[id$="txt_Branchname"]').val('');
                $('[id$="txt_Branchcode"]').val('');
                $('[id$="txt_Branchid"]').val('');
                $('[id$="txt_Branchcode"]').prop('readonly', false);
                $('#<%=ACTIVE.ClientID%>').prop('checked', false);
                $('#AddEditModal').modal('hide');

            }
        }
    </script>

    <script>

        function funCancelModal() {

            $('#errormsgDiv').hide();
            $('[id$="txt_Branchname"]').val('');
            $('[id$="txt_Branchcode"]').val('');
            $('[id$="txt_Branchid"]').val('');
            $('[id$="txt_Branchcode"]').prop('readonly', false);
            $('#<%=ACTIVE.ClientID%>').prop('checked', false);
            $('#AddEditModal').modal('hide');

        }
    </script>

</asp:Content>

