<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="SalesMaster.aspx.cs" Inherits="Admin_SalesMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <h5 class="boxed-heading">Sales Report :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
             <br />
     <br />
        <!-- Filter/Search Section -->
        <div class="filter-section row">
            <div class="form-group col-md-4">
                <label for="txtFromDate">From Date  :</label>
                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <div class="form-group col-md-4">
                <label for="txtToDate">To Date  :</label>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
        </div>
        <div class="filter-section row">
            <div class="form-group col-md-3">
                <label for="txtSearchBillNo">Enter Bill Number  :</label>
                <asp:TextBox ID="txtSearchBillNo" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group col-md-5">
                <label for="ddlSearchPatientName">Select Patient Name  :</label>
                <asp:DropDownList ID="ddlSearchPatientName" runat="server" CssClass="dropdown-custom">
                    <asp:ListItem Text="--All Patients--" Value="0" />
                </asp:DropDownList>
            </div>
            <div class="form-group col-md-3">
                <label for="ddlSearchPaymentMode">Select Payment Mode  :</label>
                <asp:DropDownList ID="ddlSearchPaymentMode" runat="server" CssClass="dropdown-custom">
                    <asp:ListItem Text="--All Modes--" Value="0" />
                    <asp:ListItem Text="Cash" Value="Cash" />
                    <asp:ListItem Text="Credit" Value="Credit" />
                 
                </asp:DropDownList>
            </div>
        </div>
             <br />
     <br />
        <div class="text-center col-md-12">
            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />&nbsp;
               
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add New" PostBackUrl="~/Admin/AddSales.aspx" />&nbsp;
           
        </div>
           <br />
           <br />
        <!-- GridView Section -->
        <asp:GridView ID="GridViewSales" runat="server" AutoGenerateColumns="False" DataKeyNames="SR_NO" CssClass="table-view"
            EmptyDataText="No data found!"  OnPageIndexChanging="GridViewSales_PageIndexChanging"  AllowPaging="true" PageSize="5" PagerSettings-Mode="Numeric">
            <Columns>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <a id="btnEdit" href='<%#Eval("SR_NO", "AddSales.aspx?id={0}") %>' class="btn btn-primary">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BILL_NO" HeaderText="Bill Number" />
                <asp:BoundField DataField="PATIENT_NAME" HeaderText="Patient Name" />
                <asp:BoundField DataField="PAYMENT_MODE" HeaderText="Payment Mode" />
                <asp:BoundField DataField="GROSS_AMOUNT" HeaderText="Gross Amount" DataFormatString="{0:C}" />
                <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="Total Amount" DataFormatString="{0:C}"/>
                <asp:BoundField DataField="DATE" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />

            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

