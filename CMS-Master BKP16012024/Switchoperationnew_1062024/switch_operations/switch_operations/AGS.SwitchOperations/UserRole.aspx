<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="UserRole.aspx.cs" Inherits="AGS.SwitchOperations.UserRole" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">
    <input type="hidden" name="ResultStatus" id="hdnResultStatus" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:HiddenField ID="hdnMenudetails" runat="server" />
    <asp:HiddenField ID="hdnAllSelectedValues" runat="server" />
    <asp:HiddenField runat="server" ID="hdnAccessCaption"/>


    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">User Role Master </h2>
        </div>

        <%--//divresult--%>
        <div class="box-body" id="DivResult">
            <div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
                    <%--start sheetal data-target="#AddEditModal" property added to btnaddnew--%>
                     <%--<asp:Button runat="server" class="btn btn-primary pull-right" ID="BtnAddNew" data-target="#AddEditModal" Text="Add New" OnClick="btnAddNew_Click" />--%>
                    <input type="button" class="btn btn-primary pull-right" id="BtnAddNew" data-target="AddEditModal" value="Add New" onclick="funAddNew()" />
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
                                            <table id="datatable-buttons" class="table table-striped table-bordered" style="width: 100%;">
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
            <div class="modal-content" style="border-radius: 4px;">

                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button" onclick="funCancelModal()"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeEdit" class="modal-title" style="font-weight: bold">User Role</h4>
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
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;"></span>User Role:</label>
                        <div class="col-sm-7">
                            <input type="text" class="form-control" id="txtUserRole" runat="server" placeholder="Enter User Role" maxlength="30" onkeypress="return onlyAlphabets(this,event)" autocomplete="off" />

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
                                            <table id="datatable-buttonsOpt" class="table table-striped table-bordered" style="width: 80%;">
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

                     
                        <div class="col-sm-7" hidden>
                            <input type="text" class="form-control" id="txt_userid" runat="server" />
                        </div>
                    </div>

                    <div class="row">
                        <br />
                    </div>



                    <div class="row">
                        <div class="col-sm-3">
                        </div>

                        <div class="col-sm-6  text-center">
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="SAVE" Style="width: 69px" ID="AddBtn" OnClick="add_Click" />
                            <button aria-label="Close" class="btn btn-primary" onclick="funCancelModal()" type="button"><span aria-hidden="true">CANCEL</span></button>
                            <input type="button" aria-label="Reset" class="btn btn-primary" value="Reset" onclick="fnreset();" style="display:none" />
                        </div>
                        <div class="col-sm-3">
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
                    <%--<button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>--%>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">UserRole Master</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <label for="username" class="control-label" id="LblMsg" runat="server" style="font-weight: normal"></label>
                    <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" type="button" onclick="FunShowAddModal()"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
    </div>
    <!-- ErrorMsg  end-->

    <script>
        $(document).ready(function () {

            $('#datatable-buttons').html($("#<%=hdnTransactionDetails.ClientID %>").val());
            $("#<%=hdnTransactionDetails.ClientID %>").val('');
            //$("#datatable-buttons tr :nth-child(2)").hide();
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
                        "bSort" : false,
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
            $('input[type=button][isdefault=1][value=Edit]').hide()
        }
        });
    </script>
    <script>

        




        $("#<%=AddBtn.ClientID %>").click(function () {
            var errrorTextPD = 'Please provide ';
            var errorFieldsPD = '';

            if ($("#phPageBody_txtUserRole").val() == "") {
                errortab = '1';
                if (errorFieldsPD != '') {
                    errorFieldsPD = errorFieldsPD + '<b>, User Role</b> ';
                }
                else {
                    errorFieldsPD = errorFieldsPD + '<b>User Role</b> ';
                }
                $('#<%=txtUserRole.ClientID%>').focus();
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
                //document.getElementById('phPageBody_LblMessage').innerHTML = 'Please check at least one checkbox to approve.'
                //FunShowMsg();
                //return false;

                $('#SpnErrorMsg').html('Please check at least one checkbox to approve.');
                $('#errormsgDiv').show();
                return false;
            }
            else {
                $('[id$="hdnAllSelectedValues"]').val(allids);
            }


        });
    </script>



    <script>

        function funedit(obj) {

            //var tds = $(btn).parent().parent().find('td');
            //var sUserRole = $(tds[0]).text();
            //var SUserID = $(tds[1]).text();
            var SUserID = $(obj).attr('id');
            $.ajax({
                type: "POST",
                url: "UserRole.aspx/FunGetUserRoleDetails",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                context: document.body,
                cache: "false",
                async: "false",
                data: "{'roleid':'" + SUserID + "'}",
                error: function (X,R) {
                    alert("Request: " + X.toString() + " ::: " + R.toString());
                },
                //success: OnSuccess,
                success: function (response) {
                    var resp = response.d;
                     resp = resp.split(',');
                    if (resp == '') {

                    }
                    else {

                        var sUserRole = $(obj).attr('userrole');
                      

                        $.each(resp, function (i, val) {

                           
                            $('[id$="txt_userid"]').val(SUserID);
                            //$('#datatable-buttons tbody input[type=checkbox]:not(:checked)').each(function (i) {
                            //    if ($(this).attr("ReqID") == val)
                            //    {
                            //        $(this).prop('checked', true);
                            //    }

                            //});



                            $('#datatable-buttons tbody').find('tr').each(function () {

                                $('[id$="txtUserRole"]').val(sUserRole);
                                if ($(this).find('input[type="checkbox"]').attr("ReqID") == val) {
                                    //$(this).prop('checked', true);
                                    $(this).find('input[type="checkbox"]').prop('checked', true);
                                }
                                $(this).find('input[type="checkbox"]').prop('checked', true);

                                //if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                                //    var id = $(this).find('td').next().html() //+ "|" + $(this).find('td:eq(2)').text() + "|" + $(this).find('td:eq(3)').text();
                                //    if (allids == "") {
                                //        allids = id;
                                //    } else {
                                //        allids = allids + "," + id;
                                //    }
                                //}
                            });



                            //$("input[type='checkbox']").attr("reqid='" + i + "'").prop('checked', true);
                            //$("input[type='checkbox' attr('ReqID')=" + val + "]").prop('checked', true);
                            //$(obj).attr('reqid')
                        });
                        $('#AddEditModal').modal('show');
                        //for (var i = 0; i < resp.length; i++)
                        //{


                        //}
                    }
                }
            }); 

            //function OnSuccess(response) {
            //    alert(response);
            //}

            
         
        }
    </script>
    <script>
        function FunShowMsg() {

            //if ($('#phPageBody_hdnResultStatus').val() == "1") {

                $('#ErrorModal').modal('show');
                //$('#AddEditModal').modal('show');
            //}
            //else {


            //}
        }
    </script>

    <script>

        function funCancelModal() {

            $('#errormsgDiv').hide();

            $('#AddEditModal').modal('hide')
            ////fnreset(null, true);
        }

        function FunShowAddModal() {
            if ($('#phPageBody_hdnResultStatus').val() == "1") {
                $('#AddEditModal').modal('show')
            }
            else
            {
                fnreset(null, true);
                $('#AddEditModal').modal('hide')
            }
        }

    </script>

    <script>

        
        <%--$("#<%=BtnAddNew.ClientID %>").click(function () {

            $('#errormsgDiv').hide();
            $('#phPageBody_txtUserRole').val('');
            $('#phPageBody_hdnID').val('');

            $('#datatable-buttons').html($("#<%=hdnMenudetails.ClientID %>").val());
            $("#<%=hdnMenudetails.ClientID %>").val('');
            //$('input[type=checkbox][formstatus=1]').attr('disabled', true)
            //For Data Table
            if ($("#datatable-buttons tbody tr").length > 0) {

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

            $('#AddEditModal').modal('show');

        });--%>


       function funAddNew() {
            $('#errormsgDiv').hide();
            $('#phPageBody_txtUserRole').val('');
            $('#phPageBody_hdnID').val('');
          
            if ($("#<%=hdnMenudetails.ClientID %>").val() != '')
            {
                $('#datatable-buttonsOpt').html($("#<%=hdnMenudetails.ClientID %>").val());
                $("#<%=hdnMenudetails.ClientID %>").val('');

                if ($("#datatable-buttonsOpt tbody tr").length > 0) {
                    var handleDataTableButtonsOpt = function () {
                        if ($("#datatable-buttonsOpt").length) {
                            $("#datatable-buttonsOpt").DataTable({
                                dom: "Bfrtip",
                                "bSort": false,
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
                                handleDataTableButtonsOpt();
                            }
                        };
                    }();

                    TableManageButtons.init();

                }

            }
         
       
            //For Data Table
            //if ($("#datatable-buttonsOpt tbody tr").length > 0) {

            //    var handleDataTableButtonsOpt = function () {
            //        if ($("#datatable-buttonsOpt").length) {
            //            $("#datatable-buttonsOpt").DataTable({
            //                "info": true,

            //                scrollX: true,
            //                responsive: true
            //            });
            //        }
            //    };

            //    TableManageButtonsOpt = function () {
            //        "use strict";
            //        return {
            //            init: function () {
            //                handleDataTableButtonsOpt();
            //            }
            //        };
            //    }();


            //    TableManageButtonsOpt.init();

            //}






            //For Data Table
           









            $('#AddEditModal').modal('show');
        }
    </script>

    <%--  <script>
        function FunShowMsg()
        {
            $('#ErrorModal').modal('show')
        }
    </script>--%>
</asp:Content>
