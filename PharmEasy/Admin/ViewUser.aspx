<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ViewUser.aspx.cs" Inherits="Admin_ViewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="profile-container">
       <h5 class="boxed-heading">User Detail :-</h5>
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label><br /><br />
       <%-- <div class="form-group">
            <label for="lblUID">UserID :</label>
            <asp:Label ID="lblUID" runat="server" CssClass="form-control"></asp:Label>
        </div>--%>
        <div class="form-group">
            <label for="lblUsername">Username :</label>
            <asp:Label ID="lblUsername" runat="server" CssClass="form-control"></asp:Label>
        </div>
        <div class="form-group">
            <label for="lblPassword">Password :</label>
            <asp:Label ID="lblPassword" runat="server" CssClass="form-control"></asp:Label>
        </div>
        <div class="form-group">
            <label for="lblRole">Role :</label>
            <asp:Label ID="lblRole" runat="server" CssClass="form-control"></asp:Label>
        </div>
        <div class="form-group">
            <label for="lblName">Name :</label>
            <asp:Label ID="lblName" runat="server" CssClass="form-control"></asp:Label>
        </div>
        <div class="form-group">
            <label for="lblDate">Joining Date :</label>
            <asp:Label ID="lblDate" runat="server" CssClass="form-control"></asp:Label>
        </div>
        <div class="form-group">
            <label for="lblMobile">Mobile :</label>
            <asp:Label ID="lblMobile" runat="server" CssClass="form-control"></asp:Label>
        </div>
        <div class="form-group">
            <label for="lblAddress">Address :</label>
            <asp:Label ID="lblAddress" runat="server" CssClass="form-control"></asp:Label>
        </div>
    </div>
</asp:Content>

