<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="BrandMaster.aspx.cs" Inherits="Admin_BrandMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Brand Report:-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        <br />
        <br />
        <div class="filter-section">
            <div class="row">
                <div class="col-md-3">
                    <label for="txtSearchBrandName">Enter Brand Name :</label>
                    <asp:TextBox ID="txtSearchBrandName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-3">
                    <label for="ddlSearchIsActive">Select Status :</label>
                    <asp:DropDownList ID="ddlSearchIsActive" runat="server" CssClass="dropdown-custom">
                        <asp:ListItem Text="Select All" Value="2" />
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </div>
             <div class="col-md-2"></div>
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" PostBackUrl="~/Admin/AddBrand.aspx" />&nbsp;
            </div>
        </div>        <br />
        <br />
        <!-- GridView Section -->
        <asp:GridView ID="GridViewBrands" runat="server" AutoGenerateColumns="False" DataKeyNames="BRAND_ID" CssClass="table-view"
            EmptyDataText="No data found !">
            <Columns>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <a id="btnEdit" href='<%#Eval("BRAND_ID", "AddBrand.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BRAND_NM" HeaderText="Brand Name" />
                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                <asp:CheckBoxField DataField="IS_ACTIVE" HeaderText="Active" />
               <asp:BoundField DataField="DATE" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

