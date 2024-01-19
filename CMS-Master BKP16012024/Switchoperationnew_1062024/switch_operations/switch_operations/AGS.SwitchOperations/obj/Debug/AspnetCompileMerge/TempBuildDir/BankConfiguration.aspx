<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="BankConfiguration.aspx.cs" Inherits="AGS.SwitchOperations.BankConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">
    <%-- <div id="BankConfigure">
        <div class="box-primary">
            <div class="box-header with-border">
                <i class="fa fa-credit-card"></i>
                <!-- start sheetal change card details to  Set card Limit-->
                <h4 class="box-title">Bank Configure :</h4>
                <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
                <!-- /.box-tools -->
            </div>




            <div class="box-body">

            <div class="box box-default">
            <div class="box-header with-border">
                <i class="fa fa-credit-card"></i>
                <!-- start sheetal change card details to  Set card Limit-->
                <h4 class="box-title">Bank Configure :</h4>
                <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
                <!-- /.box-tools -->
            </div>


                <div class="box-body" id="tblbank">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>CustomerID:</label>
                                    <div class="col-sm-8">
                                        <input type="text" class="form-control" maxlength="20" runat="server" name="BankName" id="txtBankName" required="required" />
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorCustomerID" runat="server"
                                            ControlToValidate="txtCustomerID" ErrorMessage="Please provide CustomerID ." 
                                            Display="Dynamic">
                                        </asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>CardNo:</label>
                                    <div class="col-sm-8">
                                        <input type="text" class="form-control" maxlength="20" runat="server" name="BankCode" id="txtBankCode" required="required" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>NewBinPrefix:</label>
                                    <div class="col-sm-8">
                                        <input type="text" class="form-control" maxlength="20" runat="server" name="SystemID" id="txtSystemID" required="required" />                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">

                            <div class="col-md-4">
                                <div class="form-group">
                                    <div class="col-sm-4"></div>
                                    <div class="col-sm-4">
                                        <div class="col-md-7">
                                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSave" Text="Save" OnClick="btnSave_Click" />
                                        </div>
                                    </div>
                                    <div class="col-sm-4"></div>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
         </div>--%>

</asp:Content>
