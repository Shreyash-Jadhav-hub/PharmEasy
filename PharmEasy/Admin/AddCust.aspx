<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddCust.aspx.cs" Inherits="Admin_AddCust" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      <script type="text/javascript">
        function validateForm() {
            var patientName = document.getElementById('<%= txtPatientName.ClientID %>').value.trim();
            var address = document.getElementById('<%= txtAddress.ClientID %>').value.trim();
            var mobile = document.getElementById('<%= txtMobile.ClientID %>').value.trim();
            var doctorName = document.getElementById('<%= txtDoctorName.ClientID %>').value.trim();
            var city = document.getElementById('<%= txtCity.ClientID %>').value.trim();

            if (!patientName || !address || !mobile || !doctorName || !city) {
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="category-container">
        <div class="row">
            <div class="col-md-12">
                <h5 class="boxed-heading">Add New Customer :-</h5>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtPatientName">Patient Name :</label>
                    <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control" placeholder="Enter Patient Name" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtAddress">Address :</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Enter Address" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtMobile">Mobile :</label>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Enter Mobile Number" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtDoctorName">Doctor Name :</label>
                    <asp:TextBox ID="txtDoctorName" runat="server" CssClass="form-control" placeholder="Enter Doctor's Name" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label for="txtCity">Doctor's City :</label>
                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="Enter City" />
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="text-center">
                <asp:Button ID="btnAddCustomer" runat="server" Text="Add Customer" CssClass="btn btn-primary" OnClick="btnAddCustomer_Click" />
                 <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="btnEdit_Click" />
            </div>
        </div>
    </div>
</asp:Content>

