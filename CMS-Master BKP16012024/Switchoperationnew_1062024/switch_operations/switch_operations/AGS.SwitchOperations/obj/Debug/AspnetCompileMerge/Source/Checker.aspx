<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="Checker.aspx.cs" Inherits="AGS.SwitchOperations.Checker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnAllSelectedValues" runat="server" />
    <asp:HiddenField ID="hdnTransactionDetails" runat="server" />
    <asp:Button runat="server" ID="BtnSave" Text="Approve" OnClick="BtnSave_Click" />
      <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">Checker Master </h2>
            <asp:DropDownList OnSelectedIndexChanged="ProcessType_SelectedIndexChanged" ID="ddlProcessType" runat="server">
            </asp:DropDownList>
        </div>

        <div class="box-body" id="DivResult">
            <%--<div class="row">
                <div class="col-md-6"></div>
                <div class="col-md-6">
                    <input type="button" class="btn btn-primary pull-right" id="BtnAddNew" value="Add New" onclick="funAddNew()" />
                </div>
            </div>--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="box-primary">
                        <!-- /.box-header -->
                        <div class="box-body no-padding">
                            <div class="row" style="padding: 0px 30px; padding-top: 15px;">
                                <div class="col-md-12">
                                    <div class="x_panel">
                                        <div class="">
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="x_content">
                                            <table id="datatable-buttons" class="table table-striped table-bordered" style="width:100%">
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
     <%-- Response Msg --%>
    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Checker</h4>
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
     <script>

         $(document).ready(function () {

            $('#datatable-buttons').html($('[id$="hdnTransactionDetails"]').val());
            $('#datatable-buttons').val('');
            var handleDataTableButtons = function () {
                if ($("#datatable-buttons").length) {
                    $("#datatable-buttons").DataTable({
                        dom: "Bfrtip",
                        //responsive: true
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
            $('#datatable-buttons').find('tr:first').find('td:first').html('<input id="checkboxhead" type="checkbox" class="" value="">');
            $('#checkboxhead').click(function () {
                var ss = $(this).is(':checked');
                //if (ss) {
                //    $('#datatable-buttons').find('.CHECK').prop('checked', true);
                //}
                //else
                //{
                //    $('#datatable-buttons').find('.CHECK').prop('checked', false);
                //}
                
                $('#datatable-buttons tbody').find('tr').each(function () {
                    if (ss) {
                       // alert('b');
                        $(this).find('input[type="checkbox"]').prop('checked', true);
                        //$(this).closest('tr').find('input[type="checkbox"]').attr('checked', true);
                        //var clsName = $(this).closest('tr').find('input[type="checkbox"]').attr('class');
                        //alert(clsName);
                       // $(this).find('.CHECK').attr('checked', true);
                    } else {
                       // alert('c');
                        $(this).find('input[type="checkbox"]').prop('checked', false);
                    }
                    
                });
            });
            $('[id$="BtnSave"]').click(function () {
                var allids = "";
                $('#datatable-buttons tbody').find('tr').each(function () {
                    if ($(this).find('input[type="checkbox"]:checked').length > 0) {
                        var id = $(this).find('td').next().html();
                        if (allids == "") {
                            allids = id;
                        } else {
                            allids = allids + "," + id;
                        }
                    } 
                });

                $('[id$="hdnAllSelectedValues"]').val(allids);
            });

         });
         function FunShowMsg() {
             //  setTimeout($('#myModal').modal('show'),2000);
             $('#memberModal').modal('show');
         }


    </script>
   
</asp:Content>
