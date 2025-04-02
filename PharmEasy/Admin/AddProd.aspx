<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddProd.aspx.cs" Inherits="Admin_AddProd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Create New Product  :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
      <br /> <br />
        <div class="row">
            <div class="col-md-6">
                <label for="txtProductName">Product Name  :</label>
                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control"  OnTextChanged="txtProductName_TextChanged"  AutoPostBack="true"/>
            </div>
            <div class="col-md-6">
                <label for="ddlCategory">Category  :</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropdown-custom">
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="txtContent">Content  :</label>
            <asp:TextBox ID="txtContent" runat="server" CssClass="form-control" />
        </div>
        <div class="row">
            <div class="col-md-3">
                <label for="ddlBrand">Brand  :</label>
                <asp:DropDownList ID="ddlBrand" runat="server" CssClass="dropdown-custom">
                    <asp:ListItem Text="Select Brand" Value="" />
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label for="txtCompanyName">Manufacturer  :</label>
                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-2">
                <label for="txtType">Packing  :</label>
                <asp:TextBox ID="txtType" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-2">
                <label for="txtShelfNo">Shelf No  :</label>
                <asp:TextBox ID="txtShelfNo" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-2">
                <label for="ddlGST">GST  :</label>
                <asp:DropDownList ID="ddlGST" runat="server" CssClass="dropdown-custom">
                    <asp:ListItem Text="Select GST" Value="" />
                </asp:DropDownList>
            </div>
        </div>
         <br />
        <div class="row">
            <div class="col-md-4 form-check">
                <label class="form-check-label" for="chkIsActive">Is Active  :  </label>
                <asp:CheckBox ID="chkIsActive" runat="server" CssClass="checkboxlist-custom" />
            </div>
        </div>
        <br />
        <div class="text-center">
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click" />
            <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="btnEdit_Click" />
        </div>
    </div>

</asp:Content>

