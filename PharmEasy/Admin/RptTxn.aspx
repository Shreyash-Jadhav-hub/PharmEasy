<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="RptTxn.aspx.cs" Inherits="Admin_RptTxn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Transaction Detail Report :-</h5>
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
                <label for="ddlPayType">Payment Type</label>
                <asp:DropDownList ID="ddlPayType" runat="server" CssClass="dropdown-custom">
                    <asp:ListItem Text="All" Value="" />
                    <asp:ListItem Text="Credit" Value="Credit" />
                    <asp:ListItem Text="Debit" Value="Debit" />

                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div class="col-md-4">
            <label for="ddlName">Name :</label>
            <asp:DropDownList ID="ddlName" runat="server" CssClass="dropdown-custom">
            </asp:DropDownList>
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
            <asp:GridView
                ID="gvTransactionDetails"
                runat="server"
                CssClass="table-view"
                AutoGenerateColumns="false"
                AllowPaging="true"
                PageSize="5"
                EmptyDataText="No data found!"
                OnPageIndexChanging="gvTransactionDetails_PageIndexChanging"
                PagerSettings-Mode="Numeric">
                <Columns>
                    <asp:BoundField DataField="TRANS_ID" HeaderText="Transaction No." />
                    <asp:BoundField DataField="PatientName" HeaderText="Name" />
                    <asp:BoundField DataField="CR" HeaderText="Credit" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="DR" HeaderText="Debit" DataFormatString="{0:C}"/>
                    <asp:BoundField DataField="PAYTYPE" HeaderText="Payment Type" />
                    <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                    <asp:BoundField DataField="DATE" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="BALANCE" HeaderText="Balance" DataFormatString="{0:C}"/>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

