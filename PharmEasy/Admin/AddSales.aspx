<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="AddSales.aspx.cs" Inherits="Admin_AddSales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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

            moveToNextField('#<%= txtSalesmanPassword.ClientID %>', '#<%= txtBillNo.ClientID %>');
            moveToNextField('#<%= txtBillNo.ClientID %>', '#<%= txtDate.ClientID %>');
            moveToNextField('#<%= txtDate.ClientID %>', '#<%= ddlPaymentMode.ClientID %>');
            moveToNextField('#<%= ddlPaymentMode.ClientID %>', '#<%= gvProductDetails.ClientID %> input[type="text"]:first');
            moveToNextField('#<%= gvProductDetails.ClientID %> input[type="text"]:last', '#<%= btnAddProduct.ClientID %>');



            // Handle Enter key in GridView textboxes
            $('#<%= gvProductDetails.ClientID %> input[type="text"]').on('keydown', function (e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    $(this).closest('td').next('td').find('input').focus();
                }
            });

            // Initial focus on Salesman Password field
            $('#<%= txtSalesmanPassword.ClientID %>').focus();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="purchase-container">
        <h5 class="boxed-heading">Sales Bill :-</h5>
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />
        <br />
        <br />
        <div class="row">
            <div class="col-md-2">
                <label for="txtSalesmanPassword">Salesman Password :</label>
                <asp:TextBox ID="txtSalesmanPassword" runat="server" CssClass="form-control"  OnTextChanged="txtSalesmanPassword_TextChanged" AutoPostBack="true" TabIndex="1" />
            </div>
            <div class="col-md-3">
                <label for="txtSalesmanName">Salesman Name :</label>
                <asp:TextBox ID="txtSalesmanName" runat="server" CssClass="form-control" ReadOnly="true" TabIndex="27" />
            </div>
            <div class="col-md-2">
                <label for="ddlPaymentMode">Payment Mode :</label>
                <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="dropdown-custom" TabIndex="4">
                    <asp:ListItem Text="Cash" Value="Cash" />
                    <asp:ListItem Text="Credit" Value="Credit" />
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label for="txtBillNo">Bill Number :</label>
                <asp:TextBox ID="txtBillNo" runat="server" CssClass="form-control" ReadOnly="true" TabIndex="2" />
            </div>

            <div class="col-md-3">
                <label for="txtDate">Billing Date :</label>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" ReadOnly="true" TabIndex="3" />
            </div>

        </div>
        <div class="row">
            <div class="col-md-3">
                <label for="txtPatientName">Patient Name</label>
                <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control" TabIndex="17" />
            </div>
            <div class="col-md-3">
                <label for="txtAddress">Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TabIndex="18" />
            </div>
            <div class="col-md-2">
                <label for="txtPhoneNo">Phone No.</label>
                <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control" TabIndex="19" />
            </div>
            <div class="col-md-2">
                <label for="txtDrName">Dr. Name</label>
                <asp:TextBox ID="txtDrName" runat="server" CssClass="form-control" TabIndex="20" />
            </div>
            <div class="col-md-2">
                <label for="txtCity">City</label>
                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" TabIndex="21" />
            </div>
        </div>

        <div class="product-details">
            <asp:GridView ID="gvProductDetails" runat="server" AutoGenerateColumns="False" CssClass="grid-view">
                <Columns>
                    <asp:TemplateField HeaderText="Product Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="input-text" Text='<%# Bind("ProductName") %>' AutoPostBack="true" OnTextChanged="txtProductName_TextChanged"></asp:TextBox>
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
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUnit" runat="server" CssClass="input-text-small" Text='<%# Bind("Unit") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Batch No.">
                        <ItemTemplate>
                            <asp:TextBox ID="txtBatchNo" runat="server" CssClass="input-text-small" Text='<%# Bind("BatchNo") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <asp:TextBox ID="txtExpiry" runat="server" CssClass="input-text-small" Text='<%# Bind("Expiry") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="MRP">
                        <ItemTemplate>
                            <asp:TextBox ID="txtMRP" runat="server" CssClass="input-text-small" Text='<%# Bind("MRP") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQty" runat="server" CssClass="input-text-small" Text='<%# Bind("Qty") %>' OnTextChanged="txtQty_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GST (%)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtGST" runat="server" CssClass="input-text-small" Text='<%# Bind("GST") %>' placeholder="0%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="input-text-small" ReadOnly="true" Text='<%# Bind("Amount") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Button ID="btnAddProduct" runat="server" CssClass="btn-add-product" Text="Add Product" OnClick="btnAddProduct_Click" />
            <asp:Label ID="Label1" runat="server" CssClass="error-message"></asp:Label>
        </div>

        <div class="totals">
            <div class="row mt-">
                <div class="col-md-3">
                    <label for="txtNarration">Narration</label>
                    <asp:TextBox ID="txtNarration" runat="server" CssClass="form-control" TabIndex="17" />
                </div>
                <div class="col-md-1">
                    <label for="txtDiscountPercent">Disc %</label>
                    <asp:TextBox ID="txtDiscountPercent" runat="server" CssClass="form-control"  OnTextChanged="txtDiscountPercent_TextChanged" AutoPostBack="true" TabIndex="24" />
                </div>
                <div class="col-md-2">
                    <label for="txtDiscount">Discount</label>
                    <asp:TextBox ID="txtTotalDiscount" runat="server" CssClass="form-control" ReadOnly="true"  />
                </div>
                <div class="col-md-2">
                    <label for="txtGrossAmount">Gross Amount</label>
                    <asp:TextBox ID="txtGrossAmount" runat="server" CssClass="form-control" ReadOnly="true" TabIndex="22" />
                </div>
                <div class="col-md-2">
                    <label for="txtTotalGST">Total GST</label>
                    <asp:TextBox ID="txtTotalGST" runat="server" CssClass="form-control" ReadOnly="true" TabIndex="23" />
                </div>
                <div class="col-md-2">
                    <label for="txtTotalAmount">Total Amount</label>
                    <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" ReadOnly="true" TabIndex="25" />
                </div>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-md-6 text-center">
                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" OnClick="btnAdd_Click" />
                 <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="btnEdit_Click" />
            </div>
        </div>
    </div>
</asp:Content>

