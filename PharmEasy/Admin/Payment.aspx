<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Payment.aspx.cs" Inherits="Admin_Payment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Payment Entry</title>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="category-container">
        <!-- Centered Heading -->
        <h5 class="boxed-heading">Payment :-</h5>

        <!-- Date Row -->
        <div class="row mb-3">
            <div class="col-md-10"></div>
            <div class="col-md-2">
                <label for="txtDate" class="form-label">Date  :</label>
                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Text='<%# DateTime.Now.ToString("dd-MM-yyyy") %>' ReadOnly="true" />
            </div>
        </div>

        <!-- Receiver and Amount Row -->
        <div class="row mb-3">
            <!-- Receiver Field -->
            <!-- Receiver Field -->
            <div class="col-md-6">
                <label for="txtReceiver" class="form-label">Receiver  :</label>
                <asp:TextBox ID="txtReceiver" runat="server" CssClass="form-control" placeholder="Enter Receiver Name"
                     AutoPostBack="true" OnTextChanged="txtReceiver_TextChanged"></asp:TextBox>

                <ajaxToolkit:AutoCompleteExtender
                    ID="AutoCompleteExtenderReceiver"
                    runat="server"
                    TargetControlID="txtReceiver"
                    ServiceMethod="GetReceiverNames"
                    MinimumPrefixLength="2"
                    CompletionInterval="500"
                    EnableCaching="true"
                    CompletionSetCount="10">
                </ajaxToolkit:AutoCompleteExtender>
            </div>
            <div class="col-md-2"></div>
            <!-- Amount Field -->
            <div class="col-md-4 d-flex justify-content-end">
                <div class="w-50">
                    <label for="txtAmount" class="form-label">Amount  :</label>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="Rs. 00.00" />
                </div>
            </div>
        </div>

        <!-- Payment Type Row -->
        <div class="row mb-3">
            <div class="col-md-12">
                <div class="d-flex align-items-center">
                    <label for="rbCash" class="form-label mr-4">Payment Type  :</label>
                    <div class="form-check mr-4">
                        <asp:RadioButton ID="rbCash" runat="server" GroupName="PaymentType" />
                        <label for="rbCash" class="form-check-label">Cash</label>
                    </div>
                    <div class="form-check">
                        <asp:RadioButton ID="rbCredit" runat="server" GroupName="PaymentType" />
                        <label for="rbCredit" class="form-check-label">Credit</label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Description Row -->
        <div class="row mb-3">
            <div class="col-md-12">
                <label for="txtDescription" class="form-label">Description  :</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-md-12 text-center">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-success" />
            </div>
        </div>
        <br />

        <!-- Buttons Row -->
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />&nbsp;
                <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-secondary ml-2" OnClick="btnView_Click" />&nbsp;
                <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="btnEdit_Click" />
            </div>
        </div>
    </div>
</asp:Content>

