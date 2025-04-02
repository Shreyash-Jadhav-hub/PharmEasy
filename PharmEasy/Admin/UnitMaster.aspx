<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="UnitMaster.aspx.cs" Inherits="Admin_UnitMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="category-container">
        <h5 class="boxed-heading">Unit Reports :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
          <br /> <br />
        <div class="row">
            <div class="col-md-3">
                <label for="txtSearchUnitName">Enter Unit Name :</label>
                <asp:TextBox ID="txtSearchUnitName" runat="server" CssClass="form-control" />
            </div>
           <div class="col-md-3">
                <label for="ddlSearchIsActive">Select Status :</label>
                <asp:DropDownList ID="ddlSearchIsActive" runat="server"  CssClass="dropdown-custom">
                    <asp:ListItem Text="Select All" Value="2" />
                    <asp:ListItem Text="Active" Value="1" />
                    <asp:ListItem Text="Inactive" Value="0" />
                </asp:DropDownList>
            </div>
            <div class="col-md-2"></div>
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add New" PostBackUrl="~/Admin/AddUnit.aspx" />&nbsp;
        </div>  <br /> <br />
        <!-- GridView Section -->
        <asp:GridView ID="GridViewUnits" runat="server" AutoGenerateColumns="False" DataKeyNames="UNIT_ID" CssClass="table-view"
            EmptyDataText="No data found !">
            <Columns>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <a id="btnEdit" href='<%#Eval("UNIT_ID", "AddUnit.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="UNIT_NM" HeaderText="Unit Name" />
                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                <asp:CheckBoxField DataField="IS_ACTIVE" HeaderText="Active" />
                <asp:BoundField DataField="DATE" HeaderText="Date" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

