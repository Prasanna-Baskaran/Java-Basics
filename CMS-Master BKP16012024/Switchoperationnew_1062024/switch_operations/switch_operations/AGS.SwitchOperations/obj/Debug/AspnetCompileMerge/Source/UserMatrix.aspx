<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="UserMatrix.aspx.cs" Inherits="AGS.SwitchOperations.UserMatrix" %>

<%@ Register Src="~/UserControls/UserButtons.ascx" TagPrefix="AGS" TagName="usrButtons" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#datatable-buttons').html($("#<%=hdnData.ClientID %>").val());
            if ($("#<%=hdnData.ClientID %>").val() != "") {
                $('#DivResult').show();
                $('#datatable-buttons').dataTable({
                    "bSort": false,
                    "scrollX": false,
                    "scrollY": "50vh",
                    "paging": false,
                    "bFilter": false
                });
            }
            else {
                $('#DivResult').hide();
            }

            $("#<%=hdnData.ClientID %>").val('');



            //for highlight Main menu
            $('#datatable-buttons tbody tr[parentneumonic="0"]').css("background", "#d3d3d3")
            //Unchecked Main menu checkbox
            $('#datatable-buttons tbody tr[parentneumonic="0"]').find('input:checkbox').prop("checked", false)

            //hide main menu All checkbox
            //$('#datatable-buttons tbody [parentneumonic="0"] [accesscode="2"]').hide()

            //on main menu checkbox checked event checked all corresp submenu 
            $('#datatable-buttons tbody tr[parentneumonic="0"] input[type="checkbox"]').off().on('click', function () {
                if ($(this).prop('checked'))
                    $('#datatable-buttons tbody tr[parentneumonic="' + $(this).parent().parent().attr('optionneumonic') + '"] input[accesscode="' + $(this).attr('accesscode') + '"]:not(:disabled)').prop('checked', 'checked');
                else
                    $('#datatable-buttons tbody tr[parentneumonic="' + $(this).parent().parent().attr('optionneumonic') + '"] input[accesscode="' + $(this).attr('accesscode') + '"]:not(:disabled)').prop('checked', false);
            })

            //for all checkbox of menu row checked event
            $('#datatable-buttons tbody tr[parentneumonic="0"] input[accesscode="2"]').change(function () {
                //var result = $(this); alert(result)
                if ($(this).prop('checked'))
                    $('#datatable-buttons tbody tr[parentneumonic="' + $(this).parent().parent().attr('optionneumonic') + '"],[optionneumonic="' + $(this).parent().parent().attr('optionneumonic') + '"]').find('td input:checkbox:not(:disabled)').prop('checked', 'checked')
                else
                    $('#datatable-buttons tbody tr[parentneumonic="' + $(this).parent().parent().attr('optionneumonic') + '"],[optionneumonic="' + $(this).parent().parent().attr('optionneumonic') + '"]').find('td input:checkbox:not(:disabled)').prop('checked', false)
            })

            $('#<%= dvRights.ClientID %>').find('table tbody tr td:not(:last-child) input:checkbox').change(function () {
                var enablecount = $(this).parent().parent().find('td:not(:last-child) input:checkbox:enabled').length;
                var checkedcount = $(this).parent().parent().find('td:not(:last-child) input:checkbox:checked').length;

                //row wise selection
                if (enablecount == checkedcount)
                    $(this).parent().parent().find('td:last input:checkbox').prop('checked', true);
                else
                    $(this).parent().parent().find('td:last input:checkbox').prop('checked', false);
                //column wise selection
                var enableColcount = $('#datatable-buttons tbody tr[parentneumonic="' + $(this).parent().parent().attr('parentneumonic') + '"]').find('input[accesscode="' + $(this).attr('accesscode') + '"]:not(:disabled)').length
                var checkedColcount = $('#datatable-buttons tbody tr[parentneumonic="' + $(this).parent().parent().attr('parentneumonic') + '"]').find('input[accesscode="' + $(this).attr('accesscode') + '"]:checkbox:checked').length
                if (enableColcount == checkedColcount)
                    $('#datatable-buttons tbody tr[optionneumonic="' + $(this).parent().parent().attr('parentneumonic') + '"]').find('input[accesscode="' + $(this).attr('accesscode') + '"]').prop("checked", "checked")
                else
                    $('#datatable-buttons tbody tr[optionneumonic="' + $(this).parent().parent().attr('parentneumonic') + '"]').find('input[accesscode="' + $(this).attr('accesscode') + '"]').prop("checked", false)

            });
            $('#phPageBody_userBtns_btnSave_U').click(function () {

                if (($("#phPageBody_ddlRole").val() == "0")) {
                    var errrorTextPD = 'Please select :  ';
                    var errorFieldsPD = '';
                    if ($("#phPageBody_ddlRole").val() == "0") {
                        errortab = '1';

                        if (errorFieldsPD != '') {
                            errorFieldsPD = errorFieldsPD + '<b>,User </b> ';

                        }
                        else {
                            errorFieldsPD = errorFieldsPD + '<b>User </b> ';

                        }
                        $('#<%=ddlRole.ClientID%>').focus();

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
                }
                else {
                    var rights = [];
                    $('#<%= dvRights.ClientID %>').find('table tbody tr[parentneumonic!=0]').each(function (a, b) {
                        var optionneumonic = $(this).attr('optionneumonic');
                        var accessrights = '';
                        $(this).find('td input:checkbox').each(function (p, q) {
                            if ($(this).prop('checked'))
                                accessrights += $(this).attr('accesscode') + ',';
                        })
                        rights.push({ optionneumonic: optionneumonic, accessrights: accessrights });
                    })
                    $('#<%=hdnData.ClientID %>').val(JSON.stringify(rights));
                    $('.shader').fadeIn();

                }





       <%--         var rights = [];
                $('#<%= dvRights.ClientID %>').find('table tbody tr').each(function (a, b) {
                     var optionneumonic = $(this).attr('optionneumonic');
                     var accessrights = '';
                     $(this).find('td input:checkbox').each(function (p, q) {
                         if ($(this).prop('checked'))
                             accessrights += $(this).attr('accesscode') + ',';
                     })
                     rights.push({ optionneumonic: optionneumonic, accessrights: accessrights });
                 })
                 $('#<%=hdnData.ClientID %>').val(JSON.stringify(rights));--%>

            });

            $('#<%= dvRights.ClientID %>').find('table tbody tr td:last-child input:checkbox').change(function () {
                $(this).parent().parent().find('input:checkbox:enabled').prop('checked', $(this).prop('checked'));
            });

            $('#<%= dvRights.ClientID %>').find('table tbody tr td:not(:last-child) input:checkbox').change(function () {
                funcheckallenablechecked($(this));
            });

            $('#<%= dvRights.ClientID %>').find('table tbody tr td:not(:last-child) input:checkbox').each(function () {
                funcheckallenablechecked($(this));
            });

        });

        function funcheckallenablechecked(ctrl) {
            var enablecount = $(ctrl).parent().parent().find('td:not(:last-child) input:checkbox:enabled').length;
            var checkedcount = $(ctrl).parent().parent().find('td:not(:last-child) input:checkbox:checked').length;
            if (enablecount != 0 && enablecount == checkedcount)
                $(ctrl).parent().parent().find('td:last input:checkbox').prop('checked', true);
            else
                $(ctrl).parent().parent().find('td:last input:checkbox').prop('checked', false);
        }

        function FunShowMsg() {

            $('#ErrorModal').modal('show');

        }
    </script>

</asp:Content>

<%--<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnData" runat="server" />
    <input type="hidden" name="ResultStatus" id="hdnResultStatus" runat="server" />
    <asp:HiddenField runat="server" ID="hdnAccessCaption" />


    <%-- <div id="dvRights" runat="server">
    </div>--%>
    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">User Rights </h2>
        </div>
        <div class="box-body" id="dvRights" runat="server">
            <!--Display validation msg ------------------------------------------------------------------------->
            <div class="pad margin no-print" id="errormsgDiv" style="display: none">
                <div class="callout callout-info" style="margin-bottom: 0!important;">
                    <h4><i class="fa fa-info"></i>Information :</h4>
                    <span id="SpnErrorMsg" class="text-center"></span>
                </div>
            </div>

            <label for="inputName"><span style="color: red;"></span>Select User:</label>
            <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged"></asp:DropDownList>
            <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click" class="btn btn-primary pull-right" style="display:none" />
            <div class="pull-right">
                <AGS:usrButtons runat="server" ID="userBtns" />
            </div>
            

            <div class="row" id="DivResult">
                <div class="col-md-12">
                    <div class="box-primary">
                        <!-- /.box-header -->
                        <div class="box-body no-padding">
                            <div class="row" >
                                <div class="col-md-12">
                                    <div class="x_panel" style="padding:5px">
                                        <div>
                                            <div class="clearfix"></div>
                                        </div>


                                        <div class="x_content">
                                            <table id="datatable-buttons" class="table table-bordered" style="width: 100%">
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
        <div id="shader" class="shader">
            <div class="loadingContainer">
                <div id="divLoading3">
                    <div id="loader-wrapper">
                        <div id="loader"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- ErrorMsg start -->
        <div id="ErrorModal" class="modal fade bs-example-modal-lg " tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
            <div class="modal-dialog modal-md">
                <div class="modal-content" style="border-radius: 6px">
                    <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                        <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                        <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">User Rights </h4>
                    </div>
                    <div class="modal-body" id="msgBody">
                        <label for="username" class="control-label" id="strresult" runat="server" style="font-weight: normal"></label>
                        <button aria-label="Close" data-dismiss="modal" class="btn btn-primary pull-right" type="button"><span aria-hidden="true">OK </span></button>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
