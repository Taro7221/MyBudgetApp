using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudgetApp
{
    public partial class Component3 : Component
    {
        public Component3()
        {
            InitializeComponent();
        }

        public Component3(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void cmb_category_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DataGrid_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
