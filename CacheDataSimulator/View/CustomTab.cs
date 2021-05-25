using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CacheDataSimulator.View
{
    public partial class CustomTab : UserControl
    {
        public static string Type;
        public CustomTab()
        {
            InitializeComponent();
        }

        public void SetTemplateDT(DataTable dt)
        {
            TemplateDG.DataSource = dt;
        }

        public void SetSelectedRow(string searchData, string type)
        {
            TemplateDG.ClearSelection();
            if (type == "TextSegment")
            {
                int rowIndex = -1;
                DataGridViewRow row = TemplateDG.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells["Address"].Value.ToString().Equals(searchData))
                    .First();
                rowIndex = row.Index;
                TemplateDG.Rows[rowIndex].Selected = true;
            }
        }

        public void SetRegisterEditable(string variant)
        {
            Type = variant;
            if(TemplateDG.Columns.Count > 0 )
            {
                TemplateDG.Columns[0].ReadOnly = true;
                if (variant == "Register")
                {
                    TemplateDG.Columns[1].ReadOnly = false;
                } else
                {
                    TemplateDG.Columns[1].ReadOnly = true;
                    if (TemplateDG.Columns.Count > 2)
                        TemplateDG.Columns[2].ReadOnly = true;
                }
            }
        }

        public void SetColumnWidth()
        {
            TemplateDG.Columns[0].Width = 100;
            TemplateDG.Columns[1].Width = 100;
            if(TemplateDG.Columns.Count > 2)
                TemplateDG.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void TemplateDG_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (TemplateDG.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().StartsWith("0x"))
            {
                Regex rx = new Regex(@"\A[A-Fa-f0-9]+\Z");
                string temp = TemplateDG.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Replace("0x","");
                if ((!rx.IsMatch(temp)) || (temp.Length != 8))
                {
                    MessageBox.Show("Invalid Input: Must be a 32bit HEX digit starting with 0x", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TemplateDG.CancelEdit();
                }
            }
            else
            {
                MessageBox.Show("Invalid Input: Must be a 32bit HEX digit starting with 0x", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TemplateDG.CancelEdit();
            }
        }
    }
}
