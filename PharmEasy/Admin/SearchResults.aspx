<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="SearchResults.aspx.cs" Inherits="Admin_SearchResults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Search Results</h5>

        <!-- Dropdown to select search type (e.g., Patient or Product) -->
        <asp:DropDownList
            ID="ddlSearchType"
            runat="server"
            AutoPostBack="True"
            OnSelectedIndexChanged="ddlSearchType_SelectedIndexChanged"
            Style="border: 2px solid #ccc; padding: 5px 10px; font-size: 16px; background-color: #fff; border-radius: 4px; width: 250px; display: block; margin: 0 auto;">
            <asp:ListItem Value="Patient" Text="Search by Patient" />
            <asp:ListItem Value="Product" Text="Search by Product" />
        </asp:DropDownList>

        <!-- Label to display messages (e.g., no results found) -->
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" />


        <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="False" CssClass="table-view">
            <Columns>

                <asp:BoundField DataField="PATIENT_NAME" HeaderText="Patient Name" Visible="False" />
                <asp:BoundField DataField="ADDRESS" HeaderText="Address" Visible="False" />
                <asp:BoundField DataField="MOBILE" HeaderText="Mobile" Visible="False" />
                <asp:BoundField DataField="DR_NAME" HeaderText="Doctor Name" Visible="False" />
                <asp:BoundField DataField="CITY" HeaderText="City" Visible="False" />
                <asp:BoundField DataField="BALANCE" HeaderText="Balance" Visible="False" />
                <asp:BoundField DataField="PROD_NM" HeaderText="Product Name" Visible="False" />
              
                <asp:BoundField DataField="BRAND_NAME" HeaderText="Brand Name" Visible="False" />
                <asp:BoundField DataField="SHELF_NO" HeaderText="Shelf No" Visible="False" />
                <asp:BoundField DataField="MANUFACTURER" HeaderText="Manufacturer" Visible="False" />
                <asp:BoundField DataField="CONTENT" HeaderText="Content" Visible="False" />
                <asp:BoundField DataField="EXPIRY" HeaderText="Expiry" Visible="False" />
                <asp:BoundField DataField="AVAIL_STOCK" HeaderText="Available Stock" Visible="False" />

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

