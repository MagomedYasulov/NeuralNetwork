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

            if (result == null)
                return;

            var output = Math.Round(result.First(),2);

            Healthy.Visible = true;
            Sick.Visible = true;

            double healtyPercent = output * 100;
            double sickPercent = (1 - output) * 100;

            Healthy.Text = $"Здоров - {healtyPercent}%";
            Sick.Text = $"Болен - {sickPercent}%";
        }

        private void healthy_Click(object sender, EventArgs e)
        {

        }

        private void Sick_Click(object sender, EventArgs e)
        {

        }
    }
}
