using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CSASPNETGridView
{
    public partial class DataInMemory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeDataSource();

                gvPerson.AllowPaging = true;
                gvPerson.PageSize = 10;
                gvPerson.AllowSorting = true;

                ViewState["SortExpression"] = "PersonID ASC";

                BindGridView();
            }
        }

        private void InitializeDataSource()
        {
            DataTable dtPerson = new DataTable();
            dtPerson.Columns.Add("PersonID");
            dtPerson.Columns.Add("LastName");
            dtPerson.Columns.Add("FirstName");

            dtPerson.Columns["PersonID"].AutoIncrement = true;
            dtPerson.Columns["PersonID"].AutoIncrementSeed = 1;
            dtPerson.Columns["PersonID"].AutoIncrementStep = 1;

            DataColumn[] dcKeys = new DataColumn[1];
            dcKeys[0] = dtPerson.Columns["PersonID"];
            dtPerson.PrimaryKey = dcKeys;

            dtPerson.Rows.Add(null, "Davolio", "Nancy");
            dtPerson.Rows.Add(null, "Fuller", "Andrew");
            dtPerson.Rows.Add(null, "Leverling", "Janet");
            dtPerson.Rows.Add(null, "Dodsworth", "Anne");
            dtPerson.Rows.Add(null, "Buchanan", "Steven");
            dtPerson.Rows.Add(null, "Suyama", "Michael");
            dtPerson.Rows.Add(null, "Callahan", "Laura");

            ViewState["dtPerson"] = dtPerson;
        }

        private void BindGridView()
        {
            if (ViewState["dtPerson"] != null)
            {
                DataTable dtPerson = (DataTable)ViewState["dtPerson"];
                DataView dvPerson = new DataView(dtPerson);
                dvPerson.Sort = ViewState["SortExpression"].ToString();

                gvPerson.DataSource = dvPerson;
                gvPerson.DataBind();
            }
        }

        protected void gvPerson_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // is normal state or an alternate row
                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    ((LinkButton)e.Row.Cells[1].Controls[0]).Attributes["onclick"] = "if(!confirm('Are you certain you want to delete this person ?')) return false; ";
                }
            }
        }

        protected void gvPerson_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Make the GridView control into edit mode for the selected row.
            gvPerson.EditIndex = e.NewEditIndex;

            BindGridView();

            lbtnAdd.Visible = false;
        }

        protected void gvPerson_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["dtPerson"] != null)
            {
                // Get table from ViewState
                DataTable dtPerson = (DataTable)ViewState["dtPerson"];

                string strPersonID = gvPerson.Rows[e.RowIndex].Cells[2].Text;

                // Get the row specified by the Primary Key value
                DataRow drPerson = dtPerson.Rows.Find(strPersonID);

                // remove row
                dtPerson.Rows.Remove(drPerson);

                BindGridView();
            }
        }

        protected void gvPerson_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPerson.EditIndex = -1;

            BindGridView();

            lbtnAdd.Visible = true;
        }

        protected void gvPerson_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPerson.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void gvPerson_Sorting(object sender, GridViewSortEventArgs e)
        {
            string[] strSortExpression = ViewState["SortExpression"].ToString().Split(' ');

            if (strSortExpression[0] == e.SortExpression)
            {
                if (strSortExpression[1] == "ASC")
                {
                    ViewState["SortExpression"] = e.SortExpression + " " + "DESC";
                }
                else
                {
                    ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
                }
            }
            else
            {
                ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
            }
        }

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            if (ViewState["dtPerson"] != null)
            {
                // Get table from ViewState
                DataTable dtPerson = (DataTable)ViewState["dtPerson"];
                dtPerson.Rows.Add(null, txtLastName.Text, txtFirstName.Text);

                BindGridView();
            }

            pnlAdd.Visible = false;
            lbtnAdd.Visible = true;
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            // Empty the TextBox controls.
            txtLastName.Text = "";
            txtFirstName.Text = "";

            // Show the Add button and hiding the Add panel.
            lbtnAdd.Visible = true;
            pnlAdd.Visible = false;
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            lbtnAdd.Visible = false;
            pnlAdd.Visible = true;
        }

        protected void gvPerson_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (ViewState["dtPerson"] != null)
            {
                DataTable dtPerson = (DataTable)ViewState["dtPerson"];

                string strPersonID = gvPerson.Rows[e.RowIndex].Cells[2].Text;
                DataRow drPerson = dtPerson.Rows.Find(strPersonID);
                drPerson["LastName"] = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtLastName")).Text;
                drPerson["FirstName"] = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtFirstName")).Text;

                gvPerson.EditIndex = -1;

                BindGridView();
                lbtnAdd.Visible = true;
            }
        }
    }
}