using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicalSystem
{
    public partial class MedicalSystem : Form
    {
        public MedicalSystem()
        {
            InitializeComponent();
        }

        private void EnterData_Click(object sender, EventArgs e)
        {
            var enterdataForm = new EnterData();
            var result = enterdataForm.ShowForm();
        }
    }
}
