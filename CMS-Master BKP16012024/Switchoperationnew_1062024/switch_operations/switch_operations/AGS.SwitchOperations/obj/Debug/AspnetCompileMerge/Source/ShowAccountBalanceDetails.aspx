<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="ShowAccountBalanceDetails.aspx.cs" Inherits="AGS.SwitchOperations.ShowAccountBalanceDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <asp:HiddenField ID="hdnAccountBalDetails" runat="server" />
    <div class="box-primary">
        <div class="box-header with-border">
            <i class="fa fa-list"></i>
            <h2 class="box-title">Account Balance Details</h2>

        </div>
        <!--Display validation msg ------------------------------------------------------------------------->
        <div class="pad margin no-print" id="errormsgDiv" style="display: none">
            <div class="callout callout-info" style="margin-bottom: 0!important;">
                <h4><i class="fa fa-info"></i>Information :</h4>
                <span id="SpnErrorMsg" class="text-center"></span>
            </div>
        </div>

        <div class="box-body" id="DivResult">
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group">
                        <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>CardNo:</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" maxlength="30" runat="server" name="SearchCardNo" id="txtSearchCardNo" onkeypress="return FunChkIsNumber(event);" />
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="form-group">
                        <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSearch"  OnClientClick="return FunSearchValidation();" Text="Search" OnClick="btnSearch_Click" />
                    </div>
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
                                        <div class="">
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

    <script>
        $(document).ready(function () {
            $('#datatable-buttons').html($('[id$="hdnAccountBalDetails"]').val());
        });

        function FunSearchValidation() {
            var CardNo = $('#phPageBody_txtSearchCardNo').val()

            if (CardNo == "") {
                $('#SpnErrorMsg').html('Please Provide Card Number');
                $('#errormsgDiv').show();
                $('#datatable-buttons').html('');
                return false;
            }

            else {
                $('#errormsgDiv').hide();
                $('.shader').fadeIn();
            }
        }
    </script>
</asp:Content>
