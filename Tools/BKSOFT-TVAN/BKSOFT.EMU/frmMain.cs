using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BKSOFT.EMU
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    textBox1.Text = openFileDialog.FileName;
                }
            }
        }

        private void btPost_Click(object sender, EventArgs e)
        {
            // Send GPHD
            //GPHDHelper.SendXML(textBox1.Text);
            ThreadPost pthe = new ThreadPost(textBox1.Text);
            pthe.Start();
        }

        private void btTVan_Click(object sender, EventArgs e)
        {
            // Send TVAN
            TVanInfo info = new TVanInfo();
            info.ApiUrl = ConfigurationManager.AppSettings["TVAN_Url"];
            info.ApiTaxCode = ConfigurationManager.AppSettings["TVAN_TaxCode"];
            info.ApiUserName = ConfigurationManager.AppSettings["TVAN_UserName"];
            info.ApiPassword = ConfigurationManager.AppSettings["TVAN_PassWord"];

            // Send hoa don cap ma
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(textBox1.Text);

            // Send Tvan
            TVANHelper tvan = new TVANHelper(info);
            tvan.TVANSendData("api/invoice/send", xmlDoc.OuterXml);
        }
    }
}
