<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddTax.aspx.cs" Inherits="Admin_AddTax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/style.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Add GST Category</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        <br/><br/>
        <!-- Add Tax Section -->
        <div class="form-group">
            <label for="txtTaxName">GST Category</label>
            <asp:TextBox ID="txtTaxName" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <label for="txtPercentage">Percentage (%)</label>
            <asp:TextBox ID="txtPercentage" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <label for="txtDescription">Description</label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group form-check">
            <label class="form-check-label" for="chkIsActive">Is Active</label>
            <asp:CheckBox ID="chkIsActive" runat="server" CssClass="checkboxlist-custom" />
        </div>
        <br />
        <div class="text-center">
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click" />&nbsp;
    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="btnEdit_Click" />
        </div>
    </div>
</asp:Content>

