<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="CardProduction.aspx.cs" Inherits="AGS.SwitchOperations.FrmCardProduction" %>

<asp:Content ID="Content2" ContentPlaceHolderID="phPageHeader" runat="server">
    <%--<link href="CSS/jquery.multiselect.css" rel="stylesheet" />
    <link href="CSS/jquery.multiselect.filter.css" rel="stylesheet" />--%>
   <%-- <link href="CSS/nepali.datepicker.v2.1.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.0.0-alpha.6/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/dt/dt-1.10.13/datatables.min.css" />
    <script type="text/javascript" src="https://cdn.datatables.net/v/dt/dt-1.10.13/datatables.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.0.0-alpha.6/js/bootstrap.min.js"></script>--%>
    <%--<script src="plugins/jQuery/nepali.datepicker.v2.1.min.js"></script>--%>
    <%--<script src="plugins/jQuery/jquery.multiselect.filter.js"></script>--%>
    <%--<script src="jQuery/CardProduction.js"></script>--%>
    <script src="Scripts/bootstrap-DatePicker.js"></script>
    <link href="Styles/DatePickerBootStrap.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {

            

            $('#phPageBody_dvCardGrid').find('table thead td:first').append('<input type="checkbox" checked="false">')

            //$('#<%= dvCardGrid.ClientID%>').find('table').addClass('table table-striped table-bordered').dataTable();
            $('#<%= dvCardGrid.ClientID%>').find('table').addClass('display').dataTable();

            $('#<%= dvCardGrid.ClientID%>').find('table thead input[type=checkbox]').change(function (a, b) {
                if ($(this)[0].checked) {
                    $('#<%= dvCardGrid.ClientID%>').find('table tbody input[type=checkbox]').each(function () { $(this)[0].checked = true })
                }
                else {
                    $('#<%= dvCardGrid.ClientID%>').find('table tbody input[type=checkbox]').each(function () { $(this)[0].checked = false })
                }
            });

            $('#<%= dvCardGrid.ClientID%>').find('table tbody input[type=checkbox]').change(function (a, b) {
                cnt = $('#<%= dvCardGrid.ClientID%>').find('table tbody input[type=checkbox]').length;
                if ($(this)[0].checked) {
                    if (cnt == $('#<%= dvCardGrid.ClientID%>').find('table tbody input[type=checkbox]:checked').length) {
                        $('#<%= dvCardGrid.ClientID%>').find('table thead input[type=checkbox]').attr('checked', true)
                    }
                }
                else {
                    $('#<%= dvCardGrid.ClientID%>').find('table thead input[type=checkbox]').attr('checked', false);
                }
            });

            $('#<%=btnProcess1.ClientID %>').click(function () {
                var selectedcust = $('#<%= dvCardGrid.ClientID%>').find('table tbody input[type=checkbox]:checked');
                if (selectedcust.length == 0) {
                    alert('Please select customers');
                    return false;
                }
                else {
                    var custids = '';
                    var ArrayIds = [];
                    $(selectedcust).each(function (a,b) {
                        custids += $($(b).parent().siblings()[0]).text() + ',';
                      //  ArrayIds[a] = $($(b).parent().siblings()[0]).text();
                    });
                    $('#<%= hdnSelectedCustomers.ClientID %>').val(custids.substring(0, custids.length - 1));
                    //$('#<%= hdnSelectedCustomers.ClientID %>').val(ArrayIds.join(","));
                   
                    
                <%--$('#datatable-buttons tbody input[type=checkbox]:checked').each(function (i) {
                    ArrayIds[i] = $($(b).parent().siblings()[0]).text();

                });
                //alert(val.join(","))

                $("#<%=hdnSelectedCustomers.ClientID%>").val(ArrayIds.join(","))--%>

                }
            });

            $('#<%= btnAuthorise.ClientID %>').click(function () {
                var selectedcust = $('#<%= dvCardGrid.ClientID%>').find('table tbody input[type=checkbox]:checked');
                if (selectedcust.length == 0) {
                    alert('Please select customers');
                    return false;
                }
                else {
                    var custids = '';
                    $(selectedcust).each(function (a, b) {
                        custids += $($(b).parent().siblings()[0]).text() + ',';
                    });
                    $('#<%= hdnSelectedCustomers.ClientID %>').val(custids.substring(0, custids.length - 1));
                }
            });

            $('.datepicker').datepicker({ format: "dd/mm/yyyy", autoclose: true, endDate: new Date() });
            <%--$('#<%=btnDownload1.ClientID %>').click(function () {
                //$('#divDialog').html('').dialog({ modal: true });
            });--%>
        });

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="phPageBody" runat="server">
    <div id="divDialog"></div>
    <asp:HiddenField ID="hdnSelectedCustomers" runat="server" />
    <section class="content-header">
        <h1>Card Production         
            <small>Card Management</small>
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
            <li><a href="#">Card Management</a></li>
        </ol>
    </section>

    <div class="row">
        <div class="col-md-12">

            <div class="alert alert-warning alert-dismissable  hide" runat="server" id="divAlert">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                <h4><i class="icon fa fa-warning"></i>
                    <asp:Label ID="lblAlertMessageHeader" runat="server"></asp:Label>
                </h4>
                <asp:Label ID="lblAlertMessageText" runat="server"></asp:Label>
            </div>
            <asp:Panel ID="Panel1" runat="server">
                <div class="box-primary">
                    <div class="box-header with-border">
                        <i class="fa fa-credit-card"></i>
                        <h3 class="box-title">Card Management</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                        </div>
                        <!-- /.box-tools -->
                    </div>
                    <div class="box-body">
                        <div class="col-lg-12">
                            <div class=" col-lg-1 pull-left">
                                <asp:Button ID="btnProcess1" runat="server" CssClass="btn btn-primary form-control" OnClick="btnProcess1_Click" Text="Process" />
                                <asp:Button ID="btnAuthorise" runat="server" CssClass="btn btn-primary form-control" OnClick="btnAuthorise_Click" Text="Authorise" />
                            </div>
                            <div class="col-lg-1 pull-left">
                                <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary form-control" OnClick="btnReset_Click" Text="Reset" />
                            </div>
                            <div class="col-lg-2 pull-left">
                                <div class="col-lg-10" style="margin-right: 0px; padding-right: 0px">
                                    <input type="text" id="txtDate" aria-readonly="true" runat="server" class="datepicker form-control" style="margin-right: 0px;" />

                                </div>
                                <div class="col-lg-2" style="margin-left: 0px; padding-left: 0px">
                                    <asp:Button ID="btnSet" runat="server" CssClass="btn btn-primary form-control" Style="margin-left: 0px; border-radius: 0px 10px 10px 0px;" OnClick="btnSet_Click" Text=">" />
                                </div>
                            </div>
                            <div class="col-lg-2 pull-left">
                                <asp:DropDownList ID="ddlBatch" AutoPostBack="true" runat="server" CssClass="dropdown form-control" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <asp:Button ID="btnDownload1" runat="server" CssClass="col-lg-1 btn btn-primary" Style="width: auto" Text="Download" OnClick="btnDownload1_Click" />
                        </div>
                        <div runat="server" id="dvCardGrid" class="col-lg-12 " style="margin-top: 10px">
                        </div>
                 

                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>



</asp:Content>
