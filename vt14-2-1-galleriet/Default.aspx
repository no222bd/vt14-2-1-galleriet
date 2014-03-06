<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="vt14_2_1_galleriet.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Galleriet</title>
    <link rel="stylesheet" href="~/Content/style.css" />
</head>
<body>
    <div id="container">
        <header>
            <h1>Galleriet</h1>
        </header>
        <div id="content">
            <form id="form1" runat="server">

                <%--Display of original image--%>
                <div id="ImageBox" runat="server" visible="False">
                    <asp:Image ID="ShownImage" runat="server" />
                </div>

                <%--List thumbnails--%>
                <div id="Thumbnails">
                    <asp:Repeater ID="Repeater" runat="server" ItemType="System.IO.FileInfo" SelectMethod="Repeater_GetData" OnItemDataBound="Repeater_ItemDataBound">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <asp:HyperLink ID="ThumbHyperLink" runat="server"
                                    NavigateUrl='<%# "~/Default.aspx?imageID=" + Item.Name %>'
                                    ImageUrl='<%# "~/Content/Images/Thumbnails/" + Item.Name %>' />
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                
                <%--Fileupload--%>
                <fieldset>
                    <legend>Ladda upp bild</legend>
                    
                    <asp:FileUpload ID="FileUpload" runat="server" />
                    <asp:Button ID="SendButton" runat="server" Text="Skicka bild" OnClick="SendButton_Click" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ErrorMessage="En fil måste väljas" ControlToValidate="FileUpload" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator" runat="server" ErrorMessage="Endast bilder av typerna gif, jpeg eller png är tillåtna" ControlToValidate="FileUpload" ValidationExpression="^.+\.(jpg|JPG|gif|GIF|png|PNG)$" Display="None"></asp:RegularExpressionValidator>
                    <asp:ValidationSummary ID="ValidationSummary" runat="server" DisplayMode="List" CssClass="ValSummary" />
                </fieldset>

            </form>
        </div>
        <!-- content -->
        <footer>
            <p>Nils-Jakob Olsson, no222bd, WP2012 Distans</p>
        </footer>
    </div>
    <!-- container -->
</body>
</html>
