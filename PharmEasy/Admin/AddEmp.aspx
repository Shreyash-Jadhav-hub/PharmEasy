<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddEmp.aspx.cs" Inherits="Admin_AddEmp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function validateForm() {
            var username = document.getElementById('<%= txtUsername.ClientID %>').value.trim();
            var password = document.getElementById('<%= txtPassword.ClientID %>').value.trim();
            var role = document.getElementById('<%= ddlRole.ClientID %>').value.trim();
            var name = document.getElementById('<%= txtName.ClientID %>').value.trim();
            var mobile = document.getElementById('<%= txtMobile.ClientID %>').value.trim();
            var address = document.getElementById('<%= txtAddress.ClientID %>').value.trim();

            if (!username || !password || !role || !name || !mobile || !address) {
                alert('All fields are required.');
                return false;
            }

            var mobilePattern = /^[0-9]{10}$/;
            if (!mobilePattern.test(mobile)) {
                alert('Invalid mobile number. Please enter a 10-digit mobile number.');
                return false;
            }

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <div class="row">
            <div class="col-md-12">
                <h5 class="boxed-heading">Add New Employee :-</h5>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtUsername">Username :</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtPassword">Password :</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label for="ddlRole">Role :</label>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="dropdown-custom">
                        <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                        <asp:ListItem Text="Employee" Value="Emplyoee"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtName">Name :</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter Full Name" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtMobile">Mobile :</label>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Enter Mobile Number" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtAddress">Address :</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Enter Address" />
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="text-center">
                <asp:Button ID="btnAddEmployee" runat="server" Text="Add Employee" CssClass="btn btn-primary" OnClick="btnAddEmployee_Click" />
            </div>
        </div>
    </div>
</asp:Content>

