using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSharpAppsecure
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OracleData.DBAccess DBConn = new OracleData.DBAccess();
            //DBConn.BeginTransaction();
            if (DBConn.ConnectToDataRepository())
            {
                // DBConn.GetDataViaProcedure("REMEDYFM.PROC_TEST");
                DBConn.CreateMyOracleDataReader("SELECT * FROM REMEDYFM.CRNCY");
                System.Data.DataTable tbl = DBConn.GetDataViaPackage("REMEDYFM.DA_EXPENSE.READALL");
                lbData.Items.Clear();

                foreach (System.Data.DataRow row in tbl.Rows)
                {
                    lbData.Items.Add(row.ItemArray[0] + ",  " + row["RELEASE_NUM"]+ ",  " + row.ItemArray[4]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Int32[] cc = { 29, 283, 38, 23, 2203, 15, 20, 38, 2, 9, 128, 28, 18 };
            OracleData.BinaryTreeOrder.TreeOrdering(cc);
            Console.WriteLine("Binary Search:");
            for (int i = cc.Length - 1; i >= 0; i--)
            {
                Console.Write(cc[i] + ", ");
            }
            Console.WriteLine();
        }
    }
}
