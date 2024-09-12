<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="checkEligibility.aspx.cs" Inherits="DomainChecker.checkEligibility"  Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">

        .auto-style1 {
            width: 100%;
        }
                .table-responsive {
table {
        width: 50; /* Set the width of the table */
        table-layout:fixed;
        position:center;
        border-collapse: collapse; /* Collapse table borders */
        border: 2px solid #000; /* Set border for the table */
    }
        th, td {
            border: 1px solid black;
            border-collapse: collapse;
            padding: 10px;
            text-align: left;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2" style="border-width: 0px; border-style: hidden; background-color: #E5E5E5">
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="24pt" Text="Quota Eligibility"></asp:Label>
                        <br />
                        &nbsp;<asp:TextBox ID="DomainTextBox" runat="server" BorderColor="#CCCCCC" Height="26px" Width="30%"></asp:TextBox>
                        &nbsp;<asp:Button ID="Button1" runat="server" BackColor="#0066CC" Font-Bold="True" ForeColor="White" Height="28px" OnClick="Button1_Click" Text="Lookup" Width="85px" />
                        <br />
                        &nbsp;
                        <button id="copyButton" runat="server" onclick="copyToClipboard()" visible="false">
                            Copy table to clipboard
                        </button>
                    </td>
                </tr>
            </table>
        </div>
         <div class="table-responsive" id="RecordsDiv" runat="server"></div>
        <br />
        <asp:Label ID="Label2" runat="server"></asp:Label>
    </form>
</body>
</html>
