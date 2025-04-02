<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="CustDetail.aspx.cs" Inherits="Admin_CustDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Customer Details :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        <!-- Filter/Search Section -->
        <br />
        <br />
        <div class="filter-section">
            <div class="row">
                <div class="col-md-4">
                    <label for="txtSearchFromDate">From Date :</label>
                    <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-4">
                    <label for="txtSearchToDate">To Date :</label>
                    <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <label for="txtSearchPatientName">Enter Patient Name :</label>
                    <asp:TextBox ID="txtSearchPatientName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label for="txtSearchMobile">Enter Mobile :</label>
                    <asp:TextBox ID="txtSearchMobile" runat="server" CssClass="form-control" />
                </div>
            </div>
            <br />
            <br />
            <div class="text-center">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;   
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add New" PostBackUrl="~/Admin/AddCust.aspx" />&nbsp;
            </div>
        </div>
        <br />
        <br />
        <!-- GridView Section -->
        <asp:GridView ID="GridViewCustomers" runat="server" AutoGenerateColumns="False" DataKeyNames="PATIENT_ID" CssClass="table-view" AllowPaging="true"
            EmptyDataText="No data found !" OnPageIndexChanging="GridViewCustomers_PageIndexChanging" PageSize="5" PagerSettings-Mode="Numeric">
            <Columns>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <a id="btnEdit" href='<%#Eval("PATIENT_ID", "AddCust.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PATIENT_NAME" HeaderText="Patient Name" />
                <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
                <asp:BoundField DataField="MOBILE" HeaderText="Mobile" />
                <asp:BoundField DataField="DR_NAME" HeaderText="Doctor Name" />
                <asp:BoundField DataField="CITY" HeaderText="City" />
                <asp:BoundField DataField="BALANCE" HeaderText="Balance" DataFormatString="{0:C}" HtmlEncode="false" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

