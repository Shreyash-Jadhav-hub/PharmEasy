<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="PurchaseMaster.aspx.cs" Inherits="Admin_PurchaseMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="purchase-container">
        <h5 class="boxed-heading">Purchase Report  :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        <!-- Filter/Search Section -->
        </br>
        </br>
        <div class="filter-section">
            <div class="row">

                <div class="col-md-3">
                    <label for="txtSearchFromDate">From Date :</label>
                    <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-3">
                    <label for="txtSearchToDate">To Date :</label>
                    <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <label for="ddlSearchSupplierName">Select Supplier Name :</label>
                    <asp:DropDownList ID="ddlSearchSupplierName" runat="server" CssClass="form-control">
                        <asp:ListItem Text="--All Suppliers--" Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label for="txtSearchInvoiceNo">Enter Invoice Number :</label>
                    <asp:TextBox ID="txtSearchInvoiceNo" runat="server" CssClass="form-control" />
                </div>

                <div class="col-md-2">
                    <label for="ddlSearchPaymentMode">Select Paytype :</label>
                    <asp:DropDownList ID="ddlSearchPaymentMode" runat="server" CssClass="form-control">
                        <asp:ListItem Text="--Both--" Value="0" />
                        <asp:ListItem Text="Cash" Value="Cash" />
                        <asp:ListItem Text="Credit" Value="Credit" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="text-center">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;
               
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add New" PostBackUrl="~/Admin/AddPurchase.aspx" />&nbsp;
           
            </div>
        </div>
        <!-- GridView Section -->
        <asp:GridView ID="GridViewPurchases" runat="server" AutoGenerateColumns="False" DataKeyNames="PUR_ID" CssClass="table-view" AllowPaging="true"
            EmptyDataText="No data found!" OnPageIndexChanging="GridViewPurchases_PageIndexChanging" PageSize="5" PagerSettings-Mode="Numeric">
            <Columns>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <a id="btnEdit" href='<%#Eval("PUR_ID", "AddPurchase.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="INVOICE_NO" HeaderText="Invoice Number" />
                <asp:BoundField DataField="SUPPLIER_NM" HeaderText="Supplier Name" />
                <asp:BoundField DataField="TTL_SCH_DISC" HeaderText="Total Scheme Discount" DataFormatString="{0:C}" />
                <asp:BoundField DataField="PARTY_DISC" HeaderText="Party Discount" DataFormatString="{0:C}" />
                <asp:BoundField DataField="TTL_DISC" HeaderText="Total Discount" DataFormatString="{0:C}" />
                <asp:BoundField DataField="TTL_GST" HeaderText="Total GST" DataFormatString="{0:C}" />
                <asp:BoundField DataField="GROSS_AMT" HeaderText="Gross Amount" DataFormatString="{0:C}" />
                <asp:BoundField DataField="NET_AMT" HeaderText="Net Amount" DataFormatString="{0:C}" />
                <asp:BoundField DataField="PAYTYPE" HeaderText="Payment Type" />
                <asp:BoundField DataField="DATE" HeaderText="Purchase Date" DataFormatString="{0:d}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

