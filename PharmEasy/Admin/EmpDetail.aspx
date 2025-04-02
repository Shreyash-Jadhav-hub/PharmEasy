<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="EmpDetail.aspx.cs" Inherits="Admin_EmpDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Employee Details :-</h5>
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
                    <label for="txtSearchUsername">Enter Username :</label>
                    <asp:TextBox ID="txtSearchUsername" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label for="txtSearchName">Enter Employee Name :</label>
                    <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-3">
                    <label for="ddlSearchRole">Select Role :</label>
                    <asp:DropDownList ID="ddlSearchRole" runat="server" CssClass="dropdown-custom">
                        <asp:ListItem Text="--All Roles--" Value="0" />
                        <asp:ListItem Text="Admin" Value="Admin" />
                        <asp:ListItem Text="Emplyoee" Value="Emplyoee" />
                    </asp:DropDownList>
                </div>

            </div>
            <br />
            <br />
            <div class="text-center">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;   
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add New" PostBackUrl="~/Admin/AddEmp.aspx" />&nbsp;
            </div>
        </div>
        <br />
        <br />
        <!-- GridView Section -->
        <asp:GridView ID="GridViewEmployees" runat="server" AutoGenerateColumns="False" DataKeyNames="UID" CssClass="table-view" AllowPaging="true"
            EmptyDataText="No data found !" OnPageIndexChanging="GridViewEmployees_PageIndexChanging" PageSize="5" PagerSettings-Mode="Numeric">
            <Columns>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <a id="btnEdit" href='<%#Eval("UID", "AddEmp.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="USERNAME" HeaderText="Username" />
                <asp:BoundField DataField="ROLE" HeaderText="Role" />
                <asp:BoundField DataField="NAME" HeaderText="Name" />
                <asp:BoundField DataField="MOBILE" HeaderText="Mobile" />
                <asp:BoundField DataField="ADDRESS" HeaderText="Address" />
              <%--  <asp:CheckBoxField DataField="IS_ACTIVE" HeaderText="Active" />--%>
                <asp:BoundField DataField="DATE" HeaderText="Joining Date" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

