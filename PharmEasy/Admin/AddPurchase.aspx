<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddPurchase.aspx.cs" Inherits="Admin_AddPurchase" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Purchase Entry</title>
   
    <script type="text/javascript">
        $(document).ready(function () {
            function moveToNextField(currentField, nextField) {
                $(currentField).on('keydown', function (e) {
                    if (e.key === 'Enter' || e.key === 'Tab') {
                        e.preventDefault();
                        $(nextField).focus();
                    }
                });
            }

            // Field navigation
            moveToNextField('#<%= txtSalesmanPassword.ClientID %>', '#<%= txtSupplierName.ClientID %>');
            moveToNextField('#<%= txtSupplierName.ClientID %>', '#<%= txtInvoiceNo.ClientID %>');
            moveToNextField('#<%= txtInvoiceNo.ClientID %>', '#<%= txtDate.ClientID %>');
            moveToNextField('#<%= txtDate.ClientID %>', '#<%= ddlPaymentMode.ClientID %>');

            $('#<%= gvProductDetails.ClientID %> input[type="text"]').on('keydown', function (e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    $(this).closest('td').next('td').find('input').focus();
                }
            });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="purchase-container">
        <h4 class="boxed-heading">Purchase :-</h4>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        <br>
       <br />
        <div class="row">
            <div class="col-md-2">
                <label for="txtSalesmanPassword">Salesman Password :</label>
                <asp:TextBox ID="txtSalesmanPassword" runat="server" CssClass="form-control" TabIndex="1" />
            </div>

            <div class="col-md-4">
                <label for="txtSupplierName">Supplier Name :</label>
                <asp:TextBox ID="txtSupplierName" runat="server" CssClass="form-control" TabIndex="2" />
            </div>
            <div class="col-md-2">
                <label for="txtInvoiceNo">Invoice No :</label>
                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" OnTextChanged="txtInvoiceNo_TextChanged" AutoPostBack="true" TabIndex="3" />
            </div>
            <div class="col-md-2">
                <label for="ddlPaymentMode">Payment Mode :</label>
                <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="dropdown-custom" TabIndex="4">
                    <asp:ListItem Text="Cash" Value="Cash" />
                    <asp:ListItem Text="Credit" Value="Credit" />
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label for="txtDate">Purchase Date :</label>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Text='<%# DateTime.Now.ToString("dd-mm-yyyy") %>' ReadOnly="true" TabIndex="5" />
            </div>
        </div>

        <div class="product-details">
            <asp:GridView ID="gvProductDetails" runat="server" AutoGenerateColumns="False" CssClass="grid-view">
                <Columns>
                    <asp:TemplateField HeaderText="Product Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="input-text" Text='<%# Bind("ProductName") %>' AutoPostBack="true" OnTextChanged="txtProductName_TextChanged" TabIndex="6"></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender
                                ID="AutoCompleteExtender1"
                                runat="server"
                                TargetControlID="txtProductName"
                                ServiceMethod="GetProductNames"
                                MinimumPrefixLength="2"
                                CompletionInterval="500"
                                EnableCaching="true"
                                CompletionSetCount="10">
                            </ajaxToolkit:AutoCompleteExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Batch No.">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBatchNo" runat="server" CssClass="input-text-small" Text='<%# Bind("BatchNo") %>' TabIndex="7"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <asp:TextBox ID="txtExpiry" runat="server" CssClass="input-text-small" Text='<%# Bind("Expiry") %>' TabIndex="8" placeholder="MM/YY"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="MRP">
                        <ItemTemplate>
                            <asp:TextBox ID="txtMRP" runat="server" CssClass="input-text-small" Text='<%# Bind("MRP") %>' TabIndex="9"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:TextBox ID="txtRate" runat="server" CssClass="input-text-small" Text='<%# Bind("Rate") %>' TabIndex="10"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQty" runat="server" CssClass="input-text-small" Text='<%# Bind("Qty") %>' OnTextChanged="txtQty_TextChanged" AutoPostBack="true" TabIndex="11"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Free">
                        <ItemTemplate>
                            <asp:TextBox ID="txtFree" runat="server" CssClass="input-text-small" Text='<%# Bind("Free") %>' TabIndex="12"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sch Disc">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSchDisc" runat="server" CssClass="input-text-small" Text='<%# Bind("SchDisc") %>' TabIndex="13"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GST (%)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtGST" runat="server" CssClass="input-text-small" Text='<%# Bind("GST") %>' placeholder="0%" ReadOnly="true" TabIndex="14" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="input-text-small" ReadOnly="true" Text='<%# Bind("Amount") %>' TabIndex="15"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Button ID="btnAddProduct" runat="server" CssClass="btn-add-product" Text="Add Product" OnClick="btnAddProduct_Click" TabIndex="16" />

        </div>

        <div class="totals">
            <div class="row">
                <div class="col-md-4">
                    <label for="txtNarration">Narration :</label>
                    <asp:TextBox ID="txtNarration" runat="server" CssClass="form-control" TabIndex="17" />
                </div>
                <div class="col-md-2">
                    <label for="txtPartyDiscount">Party Discount :</label>
                    <asp:TextBox ID="txtPartyDiscount" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtPartyDiscount_TextChanged" TabIndex="18" />
                </div>
                <div class="col-md-2">
                    <label for="txtSchDiscount">Sch. Discount :</label>
                    <asp:TextBox ID="txtSchDiscount" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-md-2">
                    <label for="txtTotalDiscount">Total Discount :</label>
                    <asp:TextBox ID="txtTotalDiscount" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-md-2">
                    <label for="txtGrossAmt">Gross Amount :</label>
                    <asp:TextBox ID="txtGrossAmt" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-md-2">
                    <label for="txtTotalGST">Total GST :</label>
                    <asp:TextBox ID="txtTotalGST" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
                <div class="col-md-2">
                    <label for="txtNetAmt">Net Amount :</label>
                    <asp:TextBox ID="txtNetAmt" runat="server" CssClass="form-control" ReadOnly="true" />
                </div>
            </div>
        </div>

        <div class="text-center">
            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click" TabIndex="19" />
            <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="btnEdit_Click" TabIndex="20" />
        </div>
    </div>
</asp:Content>
