<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="DomainChecker.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACS Domain Checker</title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
        }
        .table-responsive {
table {
        width: 100%; /* Set the width of the table */
        table-layout:fixed;
        border-collapse: collapse; /* Collapse table borders */
        border: 2px solid #000; /* Set border for the table */
    }

    th, td {
        border: 1px solid #000; /* Set border for table cells */
        padding: 2px; /* Add padding to table cells */
        overflow-wrap: normal; /* Wrap long words to the next line */
    }
}

    </style>
</head>
<body style="width=100%">
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2" style="border-width: 0px; border-style: hidden; background-color: #E5E5E5">
                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="24pt" Text="Domain Checker"></asp:Label>
                        <br />
                        &nbsp;<asp:TextBox ID="DomainTextBox" runat="server" BorderColor="#CCCCCC" Height="26px" Width="30%"></asp:TextBox>
&nbsp;<asp:Button ID="Button1" runat="server" BackColor="#0066CC" Font-Bold="True" ForeColor="White" Height="28px" OnClick="Button1_Click" Text="Lookup" Width="85px" />
                        <br />
&nbsp;<button runat="server" id="copyButton" onclick="copyToClipboard()" visible="false">Copy table to clipboard</button>
                    </td>
                </tr>
            
                </table>
                        <asp:Label ID="Label2" runat="server" Visible="False"></asp:Label>
                        <div class="table-responsive" id="RecordsDiv" runat="server"></div>
                        
                         

        </div>
    </form>
    <script>
        function copyToClipboard() {
            var range = document.createRange();
            range.selectNode(document.getElementById("RecordsDiv"));
            window.getSelection().removeAllRanges(); // Clear current selection
            window.getSelection().addRange(range); // Add the range to the selection
            document.execCommand('copy'); // Copy the selected content
            window.getSelection().removeAllRanges(); // Clear the selection after copying
        }
    </script>
</body>
</html>
