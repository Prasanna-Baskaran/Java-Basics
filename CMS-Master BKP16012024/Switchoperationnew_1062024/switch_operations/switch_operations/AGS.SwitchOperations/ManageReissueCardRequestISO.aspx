<%@ Page Title="" Language="C#" MasterPageFile="~/SwitchOperationSite.Master" AutoEventWireup="true" CodeBehind="ManageReissueCardRequestISO.aspx.cs" Inherits="AGS.SwitchOperations.ManageReissueCardRequestISO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phPageHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="phPageBody" runat="server">

    <div id="CardReissue">
        <div class="box-primary">
            <div class="box-header with-border">
                <i class="fa fa-credit-card"></i>
                <!-- start sheetal change card details to  Set card Limit-->
                <h4 class="box-title">Manage Card Reissue :</h4>
                <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                </div>
                <!-- /.box-tools -->
            </div>

            <div class="box-body">
                <div class="box-body" id="SearchDiv">
                    <div class="form-horizontal">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label"> CustomerID:</label>
                                    <div class="col-sm-8">
                                        <input type="text" class="form-control" maxlength="20" runat="server" name="Name" id="txtCustomerID"  />
                                        
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
                                        <input type="text" class="form-control" maxlength="20" runat="server" name="CardNo" id="txtCardNo" required="required" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>NewBinPrefix:</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="ddlNewBinPrefix" AppendDataBoundItems="true" CssClass="form-control" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label for="inputName" class="col-xs-4 control-label"><span style="color: red;">*</span>HoldRespCode:</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="ddlHoldResponseCode" AppendDataBoundItems="true" runat="server" CssClass="form-control" >
                                            <asp:ListItem Selected="True" Text="Select" Value="--Select--"></asp:ListItem>
                                            <asp:ListItem Text="Lost" Value="41" />
                                            <asp:ListItem Text="Stolen" Value="43" />
                                            <%--<asp:ListItem Selected="True" Text="Lost" Value="41"></asp:ListItem>
                                                <asp:ListItem Text="Stolan" Value="43"></asp:ListItem>--%>
                                        </asp:DropDownList>

                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlHoldResponseCode" runat="server"
                                            ControlToValidate="ddlHoldResponseCode" ErrorMessage="Please provide HoldResponseCode ." 
                                            Display="Dynamic">
                                        </asp:RequiredFieldValidator>

                                        --%>

                                        <%--<input type="text" class="form-control" maxlength="20" runat="server" name="HoldRSPCode" id="txtHoldRSPCode" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                        </div>
                        <div class="row">

                            <div class="col-md-4">
                                <div class="form-group">
                                    <div class="col-sm-4"></div>
                                    <div class="col-sm-4">
                                        <div class="col-md-7">
                                            <asp:Button runat="server" CssClass="btn btn-primary pull-right" ID="btnSave" Text="Save" OnClick="btnSave_Click" OnClientClick="return FunValidation();" />
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

    <div id="memberModal" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
        <div class="modal-dialog modal-md">
            <div class="modal-content" style="border-radius: 6px">
                <div class="modal-header box-primary" style="border-top-right-radius: 6px; border-top-left-radius: 6px">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button"><span aria-hidden="true">×</span></button>
                    <h4 id="myLargeModalLabel" class="modal-title" style="font-weight: bold">Manage Card Reissue</h4>
                </div>
                <div class="modal-body" id="msgBody">
                    <asp:Label ID="LblMessage" runat="server"></asp:Label>
                    <asp:Label ID="LblCardRPANMessage" runat="server"></asp:Label>
                </div>
                <div class="modal-footer">
                    <button aria-label="Close" data-dismiss="modal" id="BtnModalClose" class="btn btn-primary" type="button"><span aria-hidden="true">OK</span></button>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>


    <script>
        function FunShowMsg() {
            //  setTimeout($('#myModal').modal('show'),2000);
            $('#memberModal').modal('show');
        }
    </script>
</asp:Content>
