<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddCat.aspx.cs" Inherits="Admin_AddCat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Create New Category :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        <br/><br/>
        <div class="row">
        <!-- Add Category Section -->
        <div class="col-md-4">
            <label for="ddlParentCategory">Parent Category :</label>
            <asp:DropDownList ID="ddlParentCategory" runat="server" CssClass="dropdown-custom">
            </asp:DropDownList>
           
        </div>
        <div class="col-md-4">
            <label for="txtCategoryName">Category Name :</label>
            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" />
        </div>
        <div class="col-md-4">
            <label for="txtDescription">Description :</label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" />
        </div>
        </div><br/>
        <div class="form-group form-check">
            <label class="form-check-label" for="chkIsActive">Is Active :</label>
            <asp:CheckBox ID="chkIsActive" runat="server" CssClass="checkboxlist-custom" />
        </div>
        <br />
        <div class="text-center">
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click" />&nbsp;
        <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="btnEdit_Click" />
        </div>
    </div>
</asp:Content>

