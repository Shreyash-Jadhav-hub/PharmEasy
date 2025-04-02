<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="TaxMaster.aspx.cs" Inherits="Admin_TaxMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Tax Report :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
   <br /> <br />
            <div class="row">
                <div class="col-md-3">
                    <label for="txtSearchTaxName">GST Category :</label>
                    <asp:TextBox ID="txtSearchTaxName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-3">
                    <label for="ddlSearchIsActive">Select Status :</label>
                    <asp:DropDownList ID="ddlSearchIsActive" runat="server" CssClass="dropdown-custom">
                        <asp:ListItem Text="Select all" Value="2" />
                        <asp:ListItem Text="Active" Value="1" />
                        <asp:ListItem Text="Inactive" Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-2"></div>
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add New" PostBackUrl="~/Admin/AddTax.aspx" />&nbsp;  
            </div>
       
          <br /> <br />
        <!-- GridView Section for Tax Management -->
        <asp:GridView ID="GridViewTaxes" runat="server" AutoGenerateColumns="False" DataKeyNames="TAX_ID" CssClass="table-view" EmptyDataText="No data found !">
            <Columns>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <a id="btnEdit" href='<%#Eval("TAX_ID", "AddTax.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TAX_NM" HeaderText="GST Category" />
                <asp:BoundField DataField="PERCENTAGE" HeaderText="Percentage (%)" />
                <asp:CheckBoxField DataField="IS_ACTIVE" HeaderText="Active" />
               <asp:BoundField DataField="DATE" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

