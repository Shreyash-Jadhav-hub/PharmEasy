<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddBrand.aspx.cs" Inherits="Admin_AddBrand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <div class="category-container">
      <h5 class="boxed-heading">Create New Brand :-</h5>
      <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />

      <!-- Add Unit Section -->
      <div class="form-group">
          <label for="txtBrandName">Brand Name  :</label>
          <asp:TextBox ID="txtBrandName" runat="server" CssClass="form-control" />
      </div>
      <div class="form-group">
          <label for="txtDescription">Description  :</label>
          <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
      </div>
      <div class="form-group form-check">
          <label class="form-check-label" for="chkIsActive">Is Active  :</label>
          <asp:CheckBox ID="chkIsActive" runat="server" CssClass="checkboxlist-custom" />
      </div>
      <br />
      <div class="text-center">
          <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click" />&nbsp;
          <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="btnEdit_Click" />
      </div>
  </div>
</asp:Content>

