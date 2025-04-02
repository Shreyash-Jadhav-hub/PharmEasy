<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="RptStock.aspx.cs" Inherits="Admin_RptStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Stock Detail Report :-</h5>
        <br />
        <br />
        <div class="row filter-row">
            <div class="col-md-4">
                <label for="txtFromDate">From Date</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label for="txtToDate">To Date</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label for="ddlProduct">Product</label>
                <asp:DropDownList ID="ddlProduct" runat="server" CssClass="dropdown-custom">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <br />
        <div class="row">
            <div class="text-center">
                <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_Click" />
            </div>
        </div>
 <br />
<br />
 <div class="col-md-12" style="overflow: auto; width: 2000px;">
        <asp:GridView ID="gvStockDetails" runat="server" CssClass="table-view" AutoGenerateColumns="false"
            AllowPaging="true" EmptyDataText="No data found !" OnPageIndexChanging="gvStockDetails_PageIndexChanging" PageSize="5" PagerSettings-Mode="Numeric">
            <Columns>
                <asp:BoundField DataField="STOCK_ID" HeaderText="Stock No." />
                <asp:BoundField DataField="PUR_ID" HeaderText="Purchase No." />
                <%--<asp:BoundField DataField="PROD_ID" HeaderText="Product ID" />--%>
                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                 <asp:BoundField DataField="BATCH_NO" HeaderText="Batch No." />
                <asp:BoundField DataField="STOCK_IN" HeaderText="Stock In" />
                <asp:BoundField DataField="STOCK_OUT" HeaderText="Stock Out" />
                <asp:BoundField DataField="AVAIL_STOCK" HeaderText="Available Stock" />
               <asp:BoundField DataField="DATE" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />      
            </Columns>
        </asp:GridView>
    </div></div>
</asp:Content>

