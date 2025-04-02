<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="ProdMaster.aspx.cs" Inherits="Admin_ProdMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Product Report :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        <br />
        <br />

        <div class="filter-section">
            <div class="row">
                <div class="col-md-4">
                    <label for="txtSearchFromDate">From Date  :</label>
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
                    <label for="txtSearchProductName">Enter Product Name  :</label>
                    <asp:TextBox ID="txtSearchProductName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-7">
                    <label for="txtSearchContentName">Enter Content Name  :</label>
                    <asp:TextBox ID="txtSearchContentName" runat="server" CssClass="form-control" />
                </div>
            </div>
            <br />
            <div class="row">

                <div class="col-md-4">
                    <label for="txtSearchBrandName">Enter Brand Name  :</label>
                    <asp:TextBox ID="txtSearchBrandName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label for="ddlSearchCategory">Select Category  :</label>
                    <asp:DropDownList ID="ddlSearchCategory" runat="server" CssClass="dropdown-custom">
                        <asp:ListItem Text="--All Category--" Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="ddlSearchIsActive">Select Status  :</label>
                    <asp:DropDownList ID="ddlSearchIsActive" runat="server" CssClass="dropdown-custom">
                        <asp:ListItem Text="Select All" Value="2" />
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>
            
            <br />
            <br />
            <div class="text-center">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add New" PostBackUrl="~/Admin/AddProd.aspx" />&nbsp;
            </div>
        </div>
            <br />
            <br />
        <div class="col-md-12" style="overflow: auto; width: 2000px;">
            <!-- GridView Section -->
            <asp:GridView ID="GridViewProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="PROD_ID" CssClass="table-view" AllowPaging="true"
                EmptyDataText="No data found !" OnPageIndexChanging="GridViewProducts_PageIndexChanging" PageSize="5" PagerSettings-Mode="Numeric">

                <Columns>
                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <a id="btnEdit" href='<%#Eval("PROD_ID", "AddProd.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                        </ItemTemplate>
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="PROD_NM" HeaderText="Product">
                        <ItemStyle Width="450px" />
                        <HeaderStyle Width="450px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Category" HeaderText="Category">
                        <ItemStyle Width="500px" />
                        <HeaderStyle Width="500px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Brand Name" HeaderText="Brand">
                        <ItemStyle Width="200px" />
                        <HeaderStyle Width="200px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="SHELF_NO" HeaderText="Shelf">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="MANUFACTURER" HeaderText="Manufacturer">
                        <ItemStyle Width="300px" />
                        <HeaderStyle Width="300px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="CONTENT" HeaderText="Content">
                        <ItemStyle Width="200px" />
                        <HeaderStyle Width="200px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="UNIT" HeaderText="Unit">
                        <ItemStyle Width="100px" />
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="TAX_NM" HeaderText="Tax(%)">
                        <ItemStyle Width="100px" />
                        <HeaderStyle Width="100px" />
                    </asp:BoundField>

                    <asp:CheckBoxField DataField="IS_ACTIVE" HeaderText="Active">
                        <ItemStyle Width="100px" />
                        <HeaderStyle Width="100px" />
                    </asp:CheckBoxField>

                    <asp:BoundField DataField="DATE" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}">
                        <ItemStyle Width="150px" />
                        <HeaderStyle Width="150px" />
                    </asp:BoundField>

                    <%-- <asp:BoundField DataField="DESCRIPTION" HeaderText="Description">
                <ItemStyle Width="250px" />
                <HeaderStyle Width="250px" />
            </asp:BoundField> --%>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

