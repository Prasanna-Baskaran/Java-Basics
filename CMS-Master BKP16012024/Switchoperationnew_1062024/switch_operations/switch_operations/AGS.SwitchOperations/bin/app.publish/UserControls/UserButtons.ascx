<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserButtons.ascx.cs" Inherits="AGS.SwitchOperations.UserControls.UserButtons" %>
<script>
    var usrbtn = usrbtn || {};
    usrbtn.btnAddClick = function () { };
    usrbtn.btnEditClick = function () { };
    usrbtn.btnRejectClick = function () { };
    usrbtn.btnCancelClick = function () { };
    usrbtn.btnFirstClick = function () { };
    usrbtn.btnNextClick = function () { };
    usrbtn.btnPreviousClick = function () { };
    usrbtn.btnLastClick = function () { };
    usrbtn.btnViewClick = function () { };
    usrbtn.btnSaveClick = function () { };
    usrbtn.btnSubmitClick = function () { };
    usrbtn.btnDeleteClick = function () { };
    //24/03/17
    usrbtn.btnProcessClick = function () { };
    usrbtn.btnAcceptClick = function () { };
    //Start Sheetal
    usrbtn.btnUpdateClick = function () { };

    

    
    $(document).ready(function () {
        $('[id$="btnAdd_U"]').click(function () {
            return usrbtn.btnAddClick();
        });
        $('[id$="btnEdit_U"]').click(function () {
            return usrbtn.btnEditClick();
        });
        $('[id$="btnReject_U"]').click(function () {
            return usrbtn.btnRejectClick();
        });
        $('[id$="btnCancel_U"]').click(function () {
            return usrbtn.btnCancelClick();
        });
        $('[id$="btnUpdate_U"]').click(function () {
            return usrbtn.btnUpdateClick();
        });
        $('[id$="btnDelete_U"]').click(function () {
            return usrbtn.btnDeleteClick();
        });
        $('[id$="btnProcess_U"]').click(function () {
            return usrbtn.btnProcessClick();
        });
        $('[id$="btnSubmit_U"]').click(function () {
            return usrbtn.btnSubmitClick();
        });

        $('[id$="btnSave_U"]').click(function () {
            return usrbtn.btnSaveClick();
        });

        //$('[id$="btnView_U"]').click(function () {
        //    return usrbtn.btnViewClick();
        //});
        $('[id$="btnLast_U"]').click(function () {
            return usrbtn.btnLastClick();
        });

        $('[id$="btnPrevious_U"]').click(function () {
            return usrbtn.btnPreviousClick();
        });

        $('[id$="btnFirst_U"]').click(function () {
            return usrbtn.btnFirstClick();
        });

        $('[id$="btnNext_U"]').click(function () {
            return usrbtn.btnNextClick();
        });

        $('[id$="btnAccept_U"]').click(function () {
            return usrbtn.btnAcceptClick();
        });



    });
    

</script>
<div>
    <asp:Button runat="server" ID="btnAdd_U"  Text="Add" Visible="false" OnClick="btnAdd_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnEdit_U" Text="Edit" Visible="false" OnClick="btnEdit_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnProcess_U" Text="Process" Visible="false" OnClick="btnProcess_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnFirst_U" Text="First" Visible="false" OnClick="btnFirst_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnPrevious_U" Text="Previous" Visible="false" OnClick="btnPrevious_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnNext_U" Text="Next" Visible="false" OnClick="btnNext_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnLast_U" Text="Last" Visible="false" OnClick="btnLast_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnAccept_U" Text="Accept" Visible="false" OnClick="btnAccept_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnReject_U" Text="Reject" Visible="false" OnClick="btnReject_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnView_U" Text="View" Visible="false" OnClick="btnView_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnSave_U" Text="Save" Visible="false" OnClick="btnSave_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnUpdate_U" Text="Update" Visible="false" OnClick="btnUpdate_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnSubmit_U" Text="Submit" Visible="false" OnClick="btnSubmit_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnDelete_U" Text="Delete" Visible="false" OnClick="btnDelete_Click" CssClass="btn btn-primary" />
    <asp:Button runat="server" ID="btnCancel_U" Text="Cancel" Visible="false" OnClick="btnCancel_Click" CssClass="btn btn-primary" />
    
</div>
