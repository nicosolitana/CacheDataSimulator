using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CacheDataSimulator.View
{
    public partial class CustomTab : UserControl
    {
        public CustomTab()
        {
            InitializeComponent();
        }

        public void SetTemplateDT(DataTable dt)
        {
            TemplateDG.DataSource = dt;
        }

        public void SetColumnWidth()
        {
            TemplateDG.Columns[0].Width = 100;
            TemplateDG.Columns[1].Width = 100;
            if(TemplateDG.Columns.Count > 2)
                TemplateDG.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }
    }
}
