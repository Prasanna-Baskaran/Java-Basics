<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="AGS.SwitchOperations.UserDetails" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="jQuery/PBLSite.js"></script>
</asp:Content>--%>
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
            <h2 class="box-title">User Detail </h2>
        </div>

        <%--//divresult--%>
        <div class="box-body" id="DivResult">
            <div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
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
    <!--start sheetal data backdrop property added as static to show model even click on window -->
    <div id="AddEditModal" class="modal fade" data-backdrop="static" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content -->
            <div class="modal-content" style="border-radius: 4px; width: 797px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">User Details</h4>
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
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>First Name:</label>
                            <div class="col-md-7">
                                <input type="text" class="form-control" id="txt_firstname" runat="server" placeholder="Enter First Name" maxlength="20" onkeypress="return onlyAlphabets(this,event)" autocomplete="off" />

                            </div>
                        </div>
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>Last Name:</label>
                            <div class="col-md-7">
                                <input type="text" class="form-control" id="txt_lastname" runat="server" placeholder="Enter Last Name" maxlength="20" onkeypress="return onlyAlphabets(this,event)" autocomplete="off" />

                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>User Status</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="dropdown_status" runat="server" class="form-control" Style="width: 100%">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div class="col-md-6">

                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>User Role:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="DDLUserRole" runat="server" class="form-control" Style="width: 100%">
                                </asp:DropDownList>

                            </div>

                        </div>


                    </div>

                    <div class="row">
                        <br />
                    </div>


                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>Mobile Number:</label>
                            <div class="col-md-7">
                                <input class="form-control" type="text" id="txt_mobileno" runat="server" placeholder="Enter Mobile Number" maxlength="10" onkeypress="return FunChkIsNumber(event)" autocomplete="off" />

                            </div>
                        </div>

                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>Email Id:</label>
                            <div class="col-md-7">
                                <input type="email" class="form-control" id="txt_emailid" runat="server" placeholder="Enter Email Id" maxlength="50" autocomplete="off" />
                            </div>

                        </div>
                    </div>
                     <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>User Name:</label>
                            <div class="col-sm-7">
                                <div class="col-sm-3" style="padding-top: 10px;">
                                    <span id="spnUserPrefix" ></span>
                                </div>
                                <div class="col-sm-9">
                                    <input type="Text" class="form-control" id="txt_username" runat="server" placeholder="UserName" maxlength="15" autocomplete="off" onkeypress="return FunChkAlphaNumeric(event)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6" style="display: none">
                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>System:</label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="DdlSystem" runat="server" class="form-control" Style="width: 100%"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-6">
                         <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>System:</label>
                            <div class="col-sm-7">
                                <asp:CheckBoxList ID="SystemList" runat="server" RepeatDirection="Horizontal" CssClass="flat" Style="" CellSpacing="5" CellPadding="5">
                                </asp:CheckBoxList>
                            </div>
                         </div>


                    </div>
                     <div class="row">
                        <br />
                    </div>

                    <div class="row">

                               <div class="col-md-6">

                            <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>Branch Code:</label>
                            <div class="col-md-7">
                                <asp:DropDownList ID="DDLBranch" runat="server" class="form-control" Style="width: 100%">
                                </asp:DropDownList>

                            </div>

                        </div>
                     </div>




                   



                    <div class="row">
                        <br />
                    </div>

                    <div class="row">
                        <div class="col-md-6">

                            <label for="inputName" class="col-md-5 control-label" id="lbl_password"><span style="color: red;">*</span>Set Password:</label>
                            <div class="col-md-7">
                                <div class="input-group">
                                    <input type="password" class="form-control" id="txt_password" runat="server" placeholder="Enter Password" maxlength="30" />
                                    <div id="PwdPolicy" class="input-group-addon text-danger"><i class="fa fa-info text-danger" data-toggle="tooltip" data-placement="bottom" title="Password Policy : Must be at least seven characters long, one upper case letter, one lower case letter, one number, one special character!"></i></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label id="lblNewPassword"></label>
                        </div>


                        <div class="col-md-6" hidden style="border-spacing: 5px">
                            <input type="text" class="form-control" id="txt_userid" runat="server" />
                        </div>

                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                            <label for="inputName" class="col-md-5 control-label" id="lbl_reentrypassword" onblur="Funblur();"><span style="color: red;">*</span>Confirm Password:</label>
                            <div class="col-md-7">
                                <input type="password" class="form-control" id="txt_retrypassword" runat="server" placeholder="Enter Password" maxlength="30" />
                            </div>
                        </div>

                    </div>
                    <br />
 <%--                   <div class="row">
                        <div class="col-md-6">
                         <label for="inputName" class="col-md-5 control-label"><span style="color: red;">*</span>System:</label>
                        <div class="col-md-7">
                            <asp:CheckBoxList ID="SystemList" runat="server" RepeatDirection="Horizontal" CssClass="flat" Style="" CellSpacing="5" CellPadding="5">
                            </asp:CheckBoxList>
                        </div>
                         </div>

                    </div>--%>

                    <div class="row">
                        <br />
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
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">User Details </h4>
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

            $("#phPageBody_SystemList").find("td").css("padding-right", "10px")



        });
    </script>
    <script>
        $("#<%=AddBtn.ClientID %>").click(function () {
            var errrorTextPD = 'Please provide :  ';
            var errorFieldsPD = '';
            var userid = $('[id$="txt_userid"]').val();

            if (userid == "" || userid == null) {

                if ($("#phPageBody_txt_firstname").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, First Name</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>First Name</b> ';
                    }
                    $('#<%=txt_firstname.ClientID%>').focus();
                }

                if ($("#phPageBody_txt_lastname").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, Last Name</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Last Name</b> ';
                    }
                    $('#<%=txt_lastname.ClientID%>').focus();
                }


                if ($("#phPageBody_dropdown_status").val() == "0") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,User Status</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>User Status</b> ';

                    }

                    $('#<%=dropdown_status.ClientID%>').focus();
                }
                if ($("#phPageBody_DDLUserRole").val() == "0") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,User Role</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>User Role</b> ';

                    }
                    $('#<%=DDLUserRole.ClientID%>').focus();
                }

                if ($("#phPageBody_DDLBranch").val() == "0") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Branch Code</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Branch Code</b> ';

                    }
                    $('#<%=DDLBranch.ClientID%>').focus();
                        }

                

                if ($("#phPageBody_txt_mobileno").val().length != "10") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Mobile No</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Mobile No</b> ';
                    }
                    $('#<%=txt_mobileno.ClientID%>').focus();
                }

                if ($("#phPageBody_txt_emailid").val() == "") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Email Id</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Email Id</b> ';

                    }
                    $('#<%=txt_emailid.ClientID%>').focus();
                }
                if ($("#phPageBody_txt_emailid").val() != "") {
                    errortab = '1';
                    var email = $("#phPageBody_txt_emailid").val();
                    if (!IsValidEmail(email)) {

                        errorFieldsPD = errorFieldsPD + '<b>Valid Mail Id</b> ';
                    }

                    $('#<%=txt_emailid.ClientID%>').focus();
                }

                if ($("#phPageBody_txt_username").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, User Name</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>User Name</b> ';
                    }
                    $('#<%=txt_username.ClientID%>').focus();
                }



                if ($("[id*=SystemList] input:checked").length == "0") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, System</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>System</b> ';
                    }
                    $('#<%=SystemList.ClientID%>').focus();
                }

                //if ($('#phPageBody_txt_password').val() != "") {
                    var strongRegex = new RegExp("^(?=.{7,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");

                    if (!(strongRegex.test($('#phPageBody_txt_password').val()))) {
                        var errorFieldsPD = '';
                        // $("#phPageBody_txt_password").style.borderColor = "#FF0000";

                        errorFieldsPD = errorFieldsPD + '<b> Valid Password according to passward policy</b> ';
                    }
                    else if ($('#phPageBody_txt_password').val() != $('#phPageBody_txt_retrypassword').val()) {
                        var errorFieldsPD = '';
                        // $("#phPageBody_txt_password").style.borderColor = "#FF0000";

                        errorFieldsPD = errorFieldsPD + '<b>Same Password in Set Password And Confim Password</b> ';
                    }
                //}


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

                    var ArrayIds = [];
                    $("[id*=SystemList] input:checked").each(function (i) {
                        ArrayIds[i] = $(this).val();

                    });
                    //alert(val.join(","))
                    $("#<%=hdnSystems.ClientID%>").val(ArrayIds.join(","))
                }

            }

            else {


                if ($("#phPageBody_txt_firstname").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, First Name</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>First Name</b> ';
                    }
                    $('#<%=txt_firstname.ClientID%>').focus();
                }

                if ($("#phPageBody_txt_lastname").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, Last Name</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Last Name</b> ';
                    }
                    $('#<%=txt_lastname.ClientID%>').focus();
                }

                if ($("#phPageBody_dropdown_status").val() == "0") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,User Status</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>User Status</b> ';

                    }

                    $('#<%=dropdown_status.ClientID%>').focus();
                 }

                 if ($("#phPageBody_DDLUserRole").val() == "0") {
                     errortab = '1';

                     if (errorFieldsPD != '') {
                         errorFieldsPD = errorFieldsPD + '<b>,User Role</b> ';

                     }
                     else {
                         errorFieldsPD = errorFieldsPD + '<b>User Role</b> ';

                     }
                     $('#<%=DDLUserRole.ClientID%>').focus();
                }


                 if ($("#phPageBody_DDLBranch").val() == "0") {
                     errortab = '1';

                     if (errorFieldsPD != '') {
                         errorFieldsPD = errorFieldsPD + '<b>,Branch Code</b> ';

                     }
                     else {
                         errorFieldsPD = errorFieldsPD + '<b>Branch Code</b> ';

                     }
                     $('#<%=DDLBranch.ClientID%>').focus();
                     }

                 
                if ($("#phPageBody_txt_mobileno").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Mobile No</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Mobile No</b> ';
                    }
                    $('#<%=txt_mobileno.ClientID%>').focus();
                }

                if ($("#phPageBody_txt_emailid").val() == "") {
                    errortab = '1';

                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>,Email Id</b> ';

                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>Email Id</b> ';

                    }
                    $('#<%=txt_emailid.ClientID%>').focus();
                }

                if ($("#phPageBody_txt_username").val() == "") {
                    errortab = '1';
                    if (errorFieldsPD != '') {
                        errorFieldsPD = errorFieldsPD + '<b>, User Name</b> ';
                    }
                    else {
                        errorFieldsPD = errorFieldsPD + '<b>User Name</b> ';
                    }
                    $('#<%=txt_username.ClientID%>').focus();
                  }

                  if ($('#phPageBody_txt_password').val() != "") {
                      var strongRegex = new RegExp("^(?=.{7,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");

                      if (!(strongRegex.test($('#phPageBody_txt_password').val()))) {
                          var errrorTextPD = 'Error:  ';
                          var errorFieldsPD = '';
                          // $("#phPageBody_txt_password").style.borderColor = "#FF0000";

                          errorFieldsPD = errorFieldsPD + '<b> Valid Password according to passward policy</b> ';
                      }
                      else if ($('#phPageBody_txt_password').val() != $('#phPageBody_txt_retrypassword').val()) {
                          var errrorTextPD = 'Error:  ';
                          var errorFieldsPD = '';
                          // $("#phPageBody_txt_password").style.borderColor = "#FF0000";

                          errorFieldsPD = errorFieldsPD + '<b> Password and Re-enter Password does not matched</b> ';
                      }
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

                      var ArrayIds = [];
                      $("[id*=SystemList] input:checked").each(function (i) {
                          ArrayIds[i] = $(this).val();


                      });
                      //alert(val.join(","))
                      $("#<%=hdnSystems.ClientID%>").val(ArrayIds.join(","))
                    $('.shader').fadeIn();
                }


            }

        });

    </script>
    <script>

        function funAddNew() {

            $('[id$="txt_firstname"]').val('');
            $('[id$="txt_lastname"]').val('');
            $('[id$="txt_username"]').val('');
            <%--$('#<%=DDLUserRole.ClientID%> option:selected').text("--Select--");--%>
            $('#<%=DDLUserRole.ClientID%>').val('0');
            $('#<%=DDLBranch.ClientID%>').val('0');
            
            $('[id$="txt_mobileno"]').val('');
            $('[id$="txt_emailid"]').val('');
            $('[id$="txt_userid"]').val('');
            $('[id$="txt_retrypassword"]').val('');
            $('[id$="txt_password"]').val('');
            $('#AddEditModal').modal('show');
            $('[id$="txt_password"]').show();
            $('[id$="txt_username"]').removeAttr('disabled');
            $('[id$="txt_retrypassword"]').show();
            $('[id$="lbl_password"]').show();
            $('#PwdPolicy').show();
            $('[id$="lbl_reentrypassword"]').show();
            $("[id*=SystemList] input").removeAttr("checked")
            $('#<%=DdlSystem.ClientID%>').val('0');
            $("#<%=DdlSystem.ClientID%>").attr("disabled", false)

        }
    </script>
    <script>
        function IsValidEmail(email) {
            var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            return expr.test(email);
        }
        function ValidateEmail() {

            var email = document.getElementById("txt_emailid").value;
            if (!IsValidEmail(email)) {

            }

        }
    </script>
    <script>
        function funedit(obj) {

            $('[id$="txt_firstname"]').val('');
            $('[id$="txt_lastname"]').val('');
            $('[id$="txt_username"]').val('');
            $('#<%=DDLUserRole.ClientID%> option:selected').text();
            $('#<%=DDLBranch.ClientID%> option:selected').text();
        
            //$('#<%=DDLUserRole.ClientID%>').val('0');
            $('#<%=DdlSystem.ClientID%>').val('0');
            $('[id$="txt_mobileno"]').val('');
            $('[id$="txt_emailid"]').val('');
            //$('#<%=dropdown_status.ClientID%> option:selected').text();
            $('[id$="txt_userid"]').val('');
            $('[id$="txt_retrypassword"]').val('');
            $('[id$="txt_password"]').val('');
            $("[id*=SystemList] input").removeAttr("checked")
            //var tds = $(btn).parent().parent().find('td');
            //var sfirstname = $(tds[0]).text();
            //var slastname = $(tds[1]).text();
            //var susername = $(tds[2]).text();
            ////  var suserrole = $(tds[3]).text();
            //var smobileno = $(tds[4]).text();
            //var semailid = $(tds[5]).text();
            //// var suserstatus = $(tds[6]).text();
            //var suserrole = $(tds[7]).text();
            //var suserstatus = $(tds[8]).text();
            //var suserid = $(tds[9]).text();
            //var systemId = $(tds[10]).text();
            var sfirstname = $(obj).attr('firstname');
            var slastname = $(obj).attr('lastname');
            var susername = $(obj).attr('username');
            //  var suserrole = $(tds[3]).text();
            var smobileno = $(obj).attr('mobile');
            var semailid = $(obj).attr('email');
            // var suserstatus = $(tds[6]).text();
            var suserrole = $(obj).attr('roleid');
            var sbranchcode = $(obj).attr('BranchCode');
            
            var suserstatus = $(obj).attr('activeid');
            var suserid = $(obj).attr('id');
            var systemId = $(obj).attr('systemid');
            //26/08           

            if (susername.indexOf('_') == -1) {
                susername = susername //// will not be triggered because susername has _..
            } else {
                susername = susername.split('_')[1];
            }

            $('[id$="txt_firstname"]').val(sfirstname);
            $('[id$="txt_lastname"]').val(slastname);
            $('[id$="txt_username"]').val(susername);

            //$("#phPageBody_DDLUserRole").find('option[value='+suserrole+']').attr('selected','selected');

            $("#phPageBody_DDLUserRole").val(suserrole);
            //$("#phPageBody_DDLBranch").text((sbranchcode == "" ? "--Select--" : sbranchcode));
            $("#phPageBody_DDLBranch option").each(function () {
                if ($(this).text() == (sbranchcode == "" ? "--Select--" : sbranchcode)) {
                    $(this).attr('selected', 'selected');
                }
            });


            $('[id$="txt_mobileno"]').val(smobileno);
            $('[id$="txt_emailid"]').val(semailid);
            $("#phPageBody_dropdown_status").val(suserstatus);
            $('[id$="txt_userid"]').val(suserid);
            $("#<%=DdlSystem.ClientID%>").val(systemId);
            $('#AddEditModal').modal('show');
            //$('[id$="txt_username"]').attr('disabled', 'disabled');
            $("#<%=DdlSystem.ClientID%>").attr("disabled", true)
            $('[id$="txt_password"]').show();
            $('[id$="txt_retrypassword"]').show();
            $('[id$="lbl_password"]').show();
            $('[id$="lbl_reentrypassword"]').show();
            $('#PwdPolicy').show();

            $($(obj).attr('systemid').split(",")).each(function (i, item) {
                $("table[id*=SystemList] input[value=" + $.trim(item) + "]").prop("checked", "checked");
            });


            $('#BtnReset').hide();
        }
    </script>
    <script>
        function FunShowMsg() {

            $('#ErrorModal').modal('show');

        }
    </script>
    <script>
        $('#txt_firstname').blur(function () {
            //alert("This input field has lost its focus.");
        });
    </script>
    <script>
        function funClearErrmsg() {
            if ($('#phPageBody_hdnResultStatus').val() == 1) {
                $('#AddEditModal').modal('show');
                $('[id$="txt_password"]').show();
                $('[id$="txt_retrypassword"]').show();
                $('[id$="lbl_password"]').show();
                $('[id$="lbl_reentrypassword"]').show();
                $('#PwdPolicy').show();
                $('#ErrorModal').modal('hide');
            }
            else {
                $('#ErrorModal').modal('hide');
                $('[id$="txt_firstname"]').val('');
                $('[id$="txt_lastname"]').val('');
                $('[id$="txt_username"]').val('');
                $('#<%=DDLUserRole.ClientID%> option:selected').text();
                $('#<%=DDLUserRole.ClientID%>').val('0');
                $('#<%=DDLBranch.ClientID%> option:selected').text();
                $('#<%=DDLBranch.ClientID%>').val('0');
                
                $('[id$="txt_mobileno"]').val('');
                $('[id$="txt_emailid"]').val('');
                $("#<%=dropdown_status.ClientID%>")[0].selectedIndex = 0;
                $('[id$="txt_userid"]').val('');
                $('[id$="txt_retrypassword"]').val('');
                $('[id$="txt_password"]').val('');
                $('#AddEditModal').modal('hide')

            }
        }
    </script>

    <script>

        function funCancelModal() {

            $('#errormsgDiv').hide();
            $('[id$="txt_firstname"]').val('');
            $('[id$="txt_lastname"]').val('');
            $('[id$="txt_username"]').val('');
            $('#<%=DDLUserRole.ClientID%> option:selected').text();
            $('#<%=DDLUserRole.ClientID%>').val('0');
            $('#<%=DDLBranch.ClientID%> option:selected').text();
            $('#<%=DDLBranch.ClientID%>').val('0');

            $('[id$="txt_mobileno"]').val('');
            $('[id$="txt_emailid"]').val('');
            $("#<%=dropdown_status.ClientID%>")[0].selectedIndex = 0;
            $('[id$="txt_userid"]').val('');
            $('[id$="txt_retrypassword"]').val('');
            $('[id$="txt_password"]').val('');
            $('#AddEditModal').modal('hide')
            $('#lblNewPassword').html('');
        }
    </script>
    <script>
        function FunGetMisMatchArrElements() {
            var LoginUserSystems = LoginUserSystems.val().split(',');


        }
    </script>

    <%--//passward policy--%>
    <script type="text/javascript">
        $("#<%=txt_password.ClientID %>").keyup(function (e) {
            if ($('[id$="txt_password"]').val()=='') { return true; }
            var strongRegex = new RegExp("^(?=.{7,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");
            var mediumRegex = new RegExp("^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z]PASSW[0-9]))).*$", "g");
            var enoughRegex = new RegExp("(?=.{6,}).*", "g");
            if (false == enoughRegex.test($(this).val())) {
                $(this).css({ 'color': 'red' });
                $('#lblNewPassword').html('Strength : Password is too short!');
                $('#lblNewPassword').removeClass().addClass('label label-danger');
            } else if (strongRegex.test($(this).val())) {

                $('#lblNewPassword').className = 'ok';
                $(this).css("color", "green");
                $('#lblNewPassword').html('Strength : Password is Strong!');
                $('#lblNewPassword').removeClass().addClass('label label-success');

            } else if (mediumRegex.test($(this).val())) {
                $(this).css("color", "orange");
                $('#lblNewPassword').html('Strength : Password is Medium!');
                $('#lblNewPassword').removeClass().addClass('label label-warning');
            } else {
                $(this).css("color", "red");
                $('#lblNewPassword').html('Strength : Password is Weak!');
                $('#lblNewPassword').removeClass().addClass('label label-danger');
            }
            return true;
        });
    </script>


</asp:Content>
