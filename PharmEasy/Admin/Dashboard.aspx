<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Admin_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="purchase-container">
        <div class="row g-4">
            <div class="col-sm-6 col-xl-3">
                <div class="rounded d-flex align-items-center justify-content-between p-4" style="background: linear-gradient(to right, #28a745, #17a2b8);">
                    <i class="fa fa-chart-line fa-3x text-white"></i>
                    <div class="ms-3">
                        <p class="mb-2 text-white">Daily Sales:</p>
                        <h6 class="mb-0 text-white">
                            <asp:Literal ID="litDailySales" runat="server"></asp:Literal>
                        </h6>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-xl-3">
                <div class="bg-light rounded d-flex align-items-center justify-content-between p-4" style="background: linear-gradient(to right, #6f42c1, #e83e8c);">
                    <i class="fa fa-chart-bar fa-3x text-white"></i>
                    <div class="ms-3">
                        <p class="mb-2 text-white">Monthly Sales:</p>
                        <h6 class="mb-0 text-white">
                            <asp:Literal ID="litMonthlySales" runat="server"></asp:Literal>
                        </h6>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-xl-3">
                <div class="rounded d-flex align-items-center justify-content-between p-4" style="background: linear-gradient(to right, #28a745, #17a2b8);">
                    <i class="fa fa-exchange-alt fa-3x text-white"></i>
                    <div class="ms-3">
                        <p class="mb-2 text-white">Today's Transactions:</p>
                        <h6 class="mb-0 text-white">
                            <asp:Literal ID="litTotalBalance" runat="server"></asp:Literal>
                        </h6>
                    </div>
                </div>
            </div>


            <div class="col-sm-6 col-xl-3">
                <div class="bg-light rounded d-flex align-items-center justify-content-between p-4" style="background: linear-gradient(to right, #6f42c1, #e83e8c);">
                    <i class="fa fa-dollar-sign fa-3x text-white"></i>
                    <div class="ms-3">
                        <p class="mb-2 text-white">Total Revenue:</p>
                        <h6 class="mb-0 text-white">
                            <asp:Literal ID="litTotalRevenue" runat="server"></asp:Literal>
                        </h6>
                    </div>
                </div>
            </div>
        </div>


        <!-- Recent Sales Start -->
        <div class="container-fluid pt-4 px-4">
            <div class="bg-light text-center rounded p-4" style="background: linear-gradient(to left, #d3d3d3, #F7F9F2);">
                <div class="d-flex align-items-center justify-content-between mb-4">
                    <h6 class="mb-0">Recent Sales</h6>
                    <a href="SalesList.aspx">Show All</a>
                </div>
                <asp:GridView ID="gvRecentSales" runat="server" CssClass="table text-start align-middle table-bordered table-hover mb-0" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="SR. No">
                            <ItemTemplate>
                                <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DATE" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="false" />
                        <asp:BoundField DataField="BILL_NO" HeaderText="Bill Number" />
                        <asp:BoundField DataField="PATIENT_NAME" HeaderText="Patient Name" />
                        <asp:BoundField DataField="PAYMENT_MODE" HeaderText="Payment Mode" />
                        <asp:BoundField DataField="TOTAL_AMOUNT" HeaderText="Bill Amount" DataFormatString="{0:C}" HtmlEncode="false" />
                        <asp:BoundField DataField="BALANCE" HeaderText="Balance" DataFormatString="{0:C}" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" Text="Detail" CommandArgument='<%# Eval("BILL_NO") %>' OnCommand="btnDetail_Command" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Recent Sales End -->


        <!-- Widgets Start -->
        <div class="container-fluid pt-4 px-4">
            <div class="row g-4">

                <div class="col-sm-12 col-md-6 col-xl-6">
                    <div class="h-100 bg-light rounded p-4">
                        <div class="d-flex align-items-center justify-content-between mb-4">
                            <h6 class="mb-0">Calender</h6>

                        </div>
                        <div id="calender"></div>
                    </div>
                </div>
                <!-- To Do List -->
                <div class="col-sm-12 col-md-6 col-xl-5">
                    <div class="h-100 bg-light rounded p-4">
                        <div class="d-flex align-items-center justify-content-between mb-4">
                            <h6 class="mb-0">To Do List</h6>
                            <a href="#" id="linkShowAll">Show All</a>
                        </div>
                        <div class="d-flex mb-2">
                            <asp:TextBox ID="txtTask" runat="server" CssClass="form-control bg-transparent" Placeholder="Enter task"></asp:TextBox>
                            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary ms-2" Text="Add" OnClick="btnAdd_Click" />
                        </div>
                        <asp:Repeater ID="rptTasks" runat="server">
                            <ItemTemplate>
                                <div class="d-flex align-items-center border-bottom py-2">
                                    <asp:CheckBox ID="chkTask" runat="server" Checked='<%# Eval("IsCompleted") %>' OnCheckedChanged="chkTask_CheckedChanged" AutoPostBack="True" />
                                    <div class="w-100 ms-3">
                                        <div class="d-flex w-100 align-items-center justify-content-between">
                                            <span><%# Eval("TaskDescription") %></span>
                                            <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-sm" Text="Delete" CommandArgument='<%# Eval("TaskID") %>' OnCommand="btnDelete_Command" />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
        <!-- Widgets End -->
    </div>
</asp:Content>

