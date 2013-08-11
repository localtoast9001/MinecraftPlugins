<%@ Page Language="C#" AutoEventWireup="true" Title="Console Host" Inherits="ConsoleHost.Web.Default" %>
<html>
<head>
    <title>Console Host</title>
    <style type="text/css">
        a.message span
        {
            font-family: monospace;
            white-space: pre;
        }
        
        a.message span:hover
        {
            background-color: #ccccff;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server" >
        <asp:Repeater runat="server" ID="Display" >
           <ItemTemplate>
            <a runat="server" class='message' title='<%# Eval("Time") %>'>
                <b runat="server" visible='<%# String.CompareOrdinal(Eval("Severity").ToString(),"Error") == 0 %>'>
                    <asp:Label runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Text")) %>' />
                </b>
                <asp:Label visible='<%# String.CompareOrdinal(Eval("Severity").ToString(),"Error") != 0 %>' runat="server" Text='<%# System.Web.HttpUtility.HtmlEncode(Eval("Text")) %>' />
            </a>
            <br />
            </ItemTemplate>
        </asp:Repeater>
        <asp:TextBox ID="CommandTextBox" runat="server" />
        <asp:Button ID="SubmitButton" runat="server" Text="Submit" OnClick="OnSubmitButtonClick" />
    </form>
</body>
</html>
