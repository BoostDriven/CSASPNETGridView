using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace CSASPNETGridView
{
    public partial class GridView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // The Page is accessed for the first time
            if (!IsPostBack)
            {
                gvPerson.AllowPaging = true;
                gvPerson.PageSize = 15;
                gvPerson.AllowSorting = true;

                // Initialize the sorting expression
                ViewState["SortExpression"] = "PersonID ASC";

                // Populate the GridView
                BindGridView();
            }
        }

        private void BindGridView()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                DataSet dsPerson = new DataSet();
                string selectCmd = "SELECT PersonID,LastName,FirstName FROM Person";
                SqlDataAdapter da = new SqlDataAdapter(selectCmd, conn);
                conn.Open();
                da.Fill(dsPerson, "Person");
                DataView dvPerson = dsPerson.Tables["Person"].DefaultView;
                dvPerson.Sort = ViewState["SortExpression"].ToString();

                gvPerson.DataSource = dvPerson;
                gvPerson.DataBind();
            }
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            lbtnAdd.Visible = false;
            pnlAdd.Visible = true;
        }

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO Person ( LastName, FirstName ) VALUES ( @LastName, @FirstName )";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = txtLastName.Text;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = txtFirstName.Text;

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            BindGridView();

            txtLastName.Text = "";
            txtFirstName.Text = "";

            lbtnAdd.Visible = true;
            pnlAdd.Visible = false;
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

        protected void gvPerson_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Make sure the current GridViewRow is a data row
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    // Add client-side confirmation when deleting.
                    ((LinkButton)e.Row.Cells[1].Controls[0]).Attributes["onclick"] = "if(!confirm('Are you certain you want to delete this person?')) return false;";
                }
            }
        }

        protected void gvPerson_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the index of the new display page.
            gvPerson.PageIndex = e.NewPageIndex;

            // Rebind grid to show data in the new page.
            BindGridView();
        }

        protected void gvPerson_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Exit edit mode.
            gvPerson.EditIndex = -1;

            // Rebind grid to show data in view mode.
            BindGridView();

            // Show the Add Button
            lbtnAdd.Visible = true;
        }

        protected void gvPerson_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM Person WHERE PersonID = @PersonID";
                cmd.CommandType = CommandType.Text;
                string strPersonID = gvPerson.Rows[e.RowIndex].Cells[2].Text;
                cmd.Parameters.Add("@PersonID", SqlDbType.Int).Value = strPersonID;
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            BindGridView();
        }

        protected void gvPerson_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Make the GridView control into edit mode for the selected row
            gvPerson.EditIndex = e.NewEditIndex;

            BindGridView();

            lbtnAdd.Visible = false;
        }

        protected void gvPerson_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // Set the command text 
                // SQL statement or the name of the stored procedure  
                cmd.CommandText = "UPDATE Person SET LastName = @LastName, FirstName = @FirstName WHERE PersonID = @PersonID";


                // Set the command type 
                // CommandType.Text for ordinary SQL statements;  
                // CommandType.StoredProcedure for stored procedures. 
                cmd.CommandType = CommandType.Text;

                // Get the PersonID of the selected row. 
                string strPersonID = gvPerson.Rows[e.RowIndex].Cells[2].Text;
                string strLastName = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtLName")).Text;
                string strFirstName = ((TextBox)gvPerson.Rows[e.RowIndex].FindControl("txtFName")).Text;

                // Append the parameters.
                cmd.Parameters.Add("@PersonID", SqlDbType.Int).Value = strPersonID;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = strLastName;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = strFirstName;

                conn.Open();

                cmd.ExecuteNonQuery();
            }

            gvPerson.EditIndex = -1;
            BindGridView();
            lbtnAdd.Visible = true;
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
            else {
                ViewState["SortExpression"] = e.SortExpression + " " + "ASC";
            }

            BindGridView();
        }


    }
}