﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="RptReceipt.aspx.cs" Inherits="Admin_RptReceipt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="category-container">
        <h5 class="boxed-heading">Receipt Reports :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        
        <!-- Filter/Search Section -->
        <br /><br />
        <div class="filter-section">
            <div class="row">
                <div class="col-md-4">
                    <label for="txtSearchFromDate">From Date :</label>
                    <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-2"></div>
                <div class="col-md-4">
                    <label for="txtSearchToDate">To Date :</label>
                    <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-4">
                    <label for="txtSearchPayer">Payer :</label>
                    <asp:TextBox ID="txtSearchPayer" runat="server" CssClass="form-control" />
                    <ajaxToolkit:AutoCompleteExtender
                        ID="AutoCompleteExtenderPayer"
                        runat="server"
                        TargetControlID="txtSearchPayer"
                        ServiceMethod="GetPayerNames"
                        MinimumPrefixLength="2"
                        CompletionInterval="500"
                        EnableCaching="true"
                        CompletionSetCount="10" />
                </div>
                <div class="col-md-2"></div>
                <div class="col-md-4">
                    <label for="txtSearchDescription">Description :</label>
                    <asp:TextBox ID="txtSearchDescription" runat="server" CssClass="form-control" />
                </div>
            </div>
            <br />
            <!-- Receipt Type Row -->
            <div class="row mb-3">
                <div class="col-md-12">
                    <div class="d-flex align-items-center">
                        <label for="rbAll" class="form-label mr-4">Receipt Type :</label>
                        <div class="form-check mr-4">
                            <asp:RadioButton ID="rbAll" runat="server" GroupName="ReceiptType" Text="All" />
                        </div>
                        <div class="form-check mr-4">
                            <asp:RadioButton ID="rbCash" runat="server" GroupName="ReceiptType" Text="Cash" />
                        </div>
                        <div class="form-check">
                            <asp:RadioButton ID="rbCredit" runat="server" GroupName="ReceiptType" Text="Credit" />
                        </div>
                    </div>
                </div>
            </div>
            <br /><br />
            <div class="text-center">
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add New" PostBackUrl="~/Admin/Receipt.aspx" />&nbsp;
            </div>
        </div>
        <br /><br />
        <!-- GridView Section -->
        <asp:GridView ID="GridViewReceipts" runat="server" AutoGenerateColumns="False" DataKeyNames="RECEIPT_ID" CssClass="table-view" AllowPaging="true"
            EmptyDataText="No data found!" OnPageIndexChanging="GridViewReceipts_PageIndexChanging" PageSize="5" PagerSettings-Mode="Numeric">
            <Columns>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <a id="btnEdit" href='<%# Eval("RECEIPT_ID", "Receipt.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="PAYER" HeaderText="Payer" />
                <asp:BoundField DataField="DESCRIPTION" HeaderText="Description" />
                <asp:BoundField DataField="PAYTYPE" HeaderText="Payment Type" />
                <asp:BoundField DataField="AMOUNT" HeaderText="Amount" DataFormatString="{0:C}"/>
                <asp:BoundField DataField="DATE" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

