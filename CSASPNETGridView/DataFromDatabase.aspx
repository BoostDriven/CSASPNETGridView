<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataFromDatabase.aspx.cs" Inherits="CSASPNETGridView.GridView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gvPerson" 
                       runat="server"
         AutoGenerateColumns="False" 
                DataKeyNames="PersonID" 
                BackColor="White"
                BorderColor="#3366CC"
                BorderWidth="1px"
                CellPadding="4" 
                OnPageIndexChanging="gvPerson_PageIndexChanging" 
                OnRowCancelingEdit="gvPerson_RowCancelingEdit" 
                OnRowDeleting="gvPerson_RowDeleting" 
                OnRowEditing="gvPerson_RowEditing" 
                OnRowUpdating="gvPerson_RowUpdating" 
                OnSorting="gvPerson_Sorting" OnRowDataBound="gvPerson_RowDataBound">
                <Columns>
                    <asp:CommandField ShowEditButton="true" />
                    <asp:CommandField ShowDeleteButton="true" />
                    <asp:BoundField DataField="PersonID" HeaderText="PersonID" ReadOnly="true" SortExpression="PersonID" />
                    <asp:TemplateField HeaderText="LastName" SortExpression="LastName">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtLName" runat="server" Text='<%# Bind("LastName") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblLName" runat="server" Text='<%# Bind("LastName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="FirstName" SortExpression="FirstName">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFName" runat="server" Text='<%# Bind("FirstName") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblFName" runat="server" Text='<%# Bind("FirstName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="true" ForeColor="#CCFF99" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" /> 
            </asp:GridView>
            <br />
            <asp:LinkButton ID="lbtnAdd" OnClick="lbtnAdd_Click" runat="server">
                AddNew
            </asp:LinkButton>
            <br />
            <br />
            <asp:Panel ID="pnlAdd" Visible="false" runat="server">
                
                Last Name: <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                <br />
                <br />
                First Name: <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                <br />
                <br />

                <asp:LinkButton ID="lbtnSubmit" OnClick="lbtnSubmit_Click" runat="server">
                    Submit
                </asp:LinkButton>
                &nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lbtnCancel" OnClick="lbtnCancel_Click" runat="server">
                    Cancel
                </asp:LinkButton>
            </asp:Panel>

            
            
        </div>
    </form>
</body>
</html>
