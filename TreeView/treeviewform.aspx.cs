using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TreeView
{
    public partial class treeviewform : System.Web.UI.Page
    {
        static int count = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DataTable dt = this.GetData("SELECT Name,Code as Id from oubr where Remarks is not null");
                this.PopulateTreeView(dt, count, null);
            }
        }

        private void PopulateTreeView(DataTable dtParent, int parentId, TreeNode treeNode)
        {
            foreach (DataRow row in dtParent.Rows)
            {
                TreeNode child = new TreeNode
                {
                    Text = row["Name"].ToString(),
                    Value = row["Id"].ToString()
                };
                if (parentId == 0)
                {
                    TreeView1.Nodes.Add(child);
                    DataTable dtChild = this.GetData("SELECT distinct  U_DepartmentName as 'Name',U_DepartmentID as 'Id' FROM [@ohem] WHERE U_locationID = " + child.Value);
                    count = 1;
                    PopulateTreeView(dtChild, count, child);
                }
                else if (parentId == 1)
                {
                    //TreeView1.Nodes.Add(child);
                    treeNode.ChildNodes.Add(child);

                    DataTable dtChild = this.GetData("select distinct U_SubDept as 'name',U_SubDeptID as 'id' from [@OHEM] where U_flgActive='y'and  U_DepartmentID = " + child.Value);
                    count = 2;
                    PopulateTreeView(dtChild, count, child);
                }else if(parentId==2){

                    treeNode.ChildNodes.Add(child);

                    DataTable dtChild = this.GetData("select Top(10) CONCAT(U_FIrstName,'',U_LastName) as name,DocEntry as id from [@OHEM] where U_flgActive='y'and U_SubDeptID =" + child.Value);
                    count = 3;
                    PopulateTreeView(dtChild, count, child);
                }else if(parentId==3){

                    treeNode.ChildNodes.Add(child);

                    DataTable dtChild = this.GetData("select distinct U_PositionName  as 'name',DocEntry as 'id' from [@OHEM] where  DocEntry=" + child.Value);
                    count = 4;
                    PopulateTreeView(dtChild, count, child);
                }
                else
                {
                   // treeNode.ChildNodes.Add(child);
                    treeNode.ChildNodes.Add(child);
                }
            }
        }

        private DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            string constr = "Data Source=192.168.1.8,1433;Network Library=DBMSSOCN;Initial Catalog=FF_Steel_test;Persist Security Info=True;User ID=sa;Password=Abacus@123;Connection Timeout=0";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                    }
                }
                return dt;
            }
        }
    }
}