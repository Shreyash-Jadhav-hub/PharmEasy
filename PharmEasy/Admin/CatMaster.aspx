<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="CatMaster.aspx.cs" Inherits="Admin_CatMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Category Reports :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        <!-- Filter/Search Section -->
         <br /> <br />
        <div class="filter-section">
           
            <div class="row">
                <div class="col-md-4">
                    <label for="txtSearchFromDate">From Date :</label>
                     <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-4">
                    <label for="txtSearchToDate">To Date  :</label>
                    <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
            </div>
         <br /> 
            <div class="row">
                <div class="col-md-4">
                    <label for="txtSearchCategoryName">Enter Category Name :</label>
                    <asp:TextBox ID="txtSearchCategoryName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label for="ddlSearchParentCategory">Select Parent Category :</label>
                    <asp:DropDownList ID="ddlSearchParentCategory" runat="server" CssClass="dropdown-custom">
                        <asp:ListItem Text="--All Category--" Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label for="ddlSearchIsActive">Select Status :</label>
                    <asp:DropDownList ID="ddlSearchIsActive" runat="server" CssClass="dropdown-custom ">
                        <asp:ListItem Text="Select All" Value="2" />
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>
             <br /> <br /> 
            <div class="text-center">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add New" PostBackUrl="~/Admin/AddCat.aspx" />&nbsp;

            </div>
        </div><br /> <br /> 
        <!-- GridView Section -->
        <asp:GridView ID="GridViewCategories" runat="server" AutoGenerateColumns="False" DataKeyNames="CAT_ID" CssClass="table-view" AllowPaging="true"
            EmptyDataText="No data found !" OnPageIndexChanging="GridViewCategories_PageIndexChanging" PageSize="5" PagerSettings-Mode="Numeric">
            <Columns>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <a id="btnEdit" href='<%#Eval("CAT_ID", "AddCat.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CAT_NM" HeaderText="Category Name" />
                <asp:BoundField DataField="PARENT_CAT" HeaderText="Parent Category" />
                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                <asp:CheckBoxField DataField="IS_ACTIVE" HeaderText="Active" />
                <asp:BoundField DataField="DATE" HeaderText="Date" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

