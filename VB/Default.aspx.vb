Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports DevExpress.Web

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub lookup_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim gl As ASPxGridLookup = DirectCast(sender, ASPxGridLookup)
        AddHandler gl.GridView.CustomCallback, AddressOf GridView_CustomCallback
    End Sub

    Protected Sub checkBox_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim checkBox As ASPxCheckBox = DirectCast(sender, ASPxCheckBox)
        Dim c As GridViewGroupRowTemplateContainer = CType(checkBox.NamingContainer, GridViewGroupRowTemplateContainer)
        checkBox.ClientSideEvents.CheckedChanged = String.Format("function(s, e){{ console.log('CheckedChanged'); lookup.GetGridView().PerformCallback('{0};' + s.GetChecked()); }}", c.VisibleIndex)
        checkBox.Checked = GetChecked(c)
    End Sub
    Protected Function GetCaptionText(ByVal container As GridViewGroupRowTemplateContainer) As String
        Dim captionText As String = If((Not String.IsNullOrEmpty(container.Column.Caption)), container.Column.Caption, container.Column.FieldName)
        Return String.Format("{0} : {1} {2}", captionText, container.GroupText, container.SummaryText)
    End Function
    Protected Function GetChecked(ByVal c As GridViewGroupRowTemplateContainer) As Boolean
        For i As Integer = 0 To c.Grid.GetChildRowCount(c.VisibleIndex) - 1
            Dim isRowSelected As Boolean = c.Grid.Selection.IsRowSelectedByKey(c.Grid.GetChildDataRow(c.VisibleIndex, i)("ProductID"))
            If Not isRowSelected Then
                Return False
            End If
        Next i
        Return True
    End Function

    Private Sub GridView_CustomCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomCallbackEventArgs)
        Dim grid As ASPxGridView = DirectCast(sender, ASPxGridView)
        Dim parameters() As String = e.Parameters.Split(";"c)
        Dim index As Integer = Nothing
        If Integer.TryParse(parameters(0), index) Then
            Dim isGroupRowSelected As Boolean = Boolean.Parse(parameters(1))
            For i As Integer = 0 To grid.GetChildRowCount(index) - 1
                Dim row As DataRow = grid.GetChildDataRow(index, i)
                grid.Selection.SetSelectionByKey(row("ProductID"), isGroupRowSelected)
            Next i
        End If
    End Sub
End Class
