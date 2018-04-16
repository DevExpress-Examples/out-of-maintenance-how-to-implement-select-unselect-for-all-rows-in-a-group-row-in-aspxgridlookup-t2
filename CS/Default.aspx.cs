using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web;

public partial class _Default : System.Web.UI.Page {

	protected void lookup_Init(object sender, EventArgs e) {
		ASPxGridLookup gl = (ASPxGridLookup)sender;
		gl.GridView.CustomCallback += GridView_CustomCallback;
	}

	protected void checkBox_Load(object sender, EventArgs e) {
		ASPxCheckBox checkBox = (ASPxCheckBox)sender;
		GridViewGroupRowTemplateContainer c = (GridViewGroupRowTemplateContainer)checkBox.NamingContainer;
		checkBox.ClientSideEvents.CheckedChanged = string.Format("function(s, e){{ console.log('CheckedChanged'); lookup.GetGridView().PerformCallback('{0};' + s.GetChecked()); }}", c.VisibleIndex);
		checkBox.Checked = GetChecked(c);
	}
	protected string GetCaptionText(GridViewGroupRowTemplateContainer container) {
		string captionText = !string.IsNullOrEmpty(container.Column.Caption) ? container.Column.Caption : container.Column.FieldName;
		return string.Format("{0} : {1} {2}", captionText, container.GroupText, container.SummaryText);
	}
	protected bool GetChecked(GridViewGroupRowTemplateContainer c) {
		for (int i = 0; i < c.Grid.GetChildRowCount(c.VisibleIndex); i++) {
			bool isRowSelected = c.Grid.Selection.IsRowSelectedByKey(c.Grid.GetChildDataRow(c.VisibleIndex, i)["ProductID"]);
			if (!isRowSelected)
				return false;
		}
		return true;
	}

	void GridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e) {
		ASPxGridView grid = (ASPxGridView)sender;
		string[] parameters = e.Parameters.Split(';');
		int index;
		if (int.TryParse(parameters[0], out index)) {
			bool isGroupRowSelected = bool.Parse(parameters[1]);
			for (int i = 0; i < grid.GetChildRowCount(index); i++) {
				DataRow row = grid.GetChildDataRow(index, i);
				grid.Selection.SetSelectionByKey(row["ProductID"], isGroupRowSelected);
			}
		}
	}
}
