using System;
using System.Windows.Forms;

namespace AesCrypto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cmdExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdConvert_Click(object sender, EventArgs e)
        {
            PocketKnife.Info o;
            String value;

            o = new PocketKnife.Info();

            value = txtPasswordDec.Text;
            if(checkValue(value).Equals("Ok"))
            {
                txtPasswordAes.Text = o.ConvertPasswordDesToAes(value);
            }
            
        }
        private string checkValue(String value)
        {
            string rt = "Ok";
            if (value.Trim().Length == 0)
            {
                MessageBox.Show("Value not found !", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            return rt;
        }

        //private void cmdKey_Click(object sender, EventArgs e)
        //{
        //    using( AesCryptoServiceProvider myAes   = new AesCryptoServiceProvider()){
        //        txtKey.Text = Convert.ToBase64String(myAes.Key);
        //        txtIV.Text = Convert.ToBase64String(myAes.IV);
        //    }
        //}

        private void cmdEncrypt_Click(object sender, EventArgs e)
        {
            PocketKnife.Info o;
            String value;

            o = new PocketKnife.Info();

            value = txtValue.Text;
            if (checkValue(value).Equals("Ok")) {
                txtEncryptValue.Text = o.EncryptPassword(value);
            }
        }

        //private void cmdDecrypt_Click(object sender, EventArgs e)
        //{
        //    PocketKnife.Info o;
        //    String value;

        //    o = new PocketKnife.Info();

        //    value = txtValue.Text;
        //    if(checkValue(value).Equals("Ok"))
        //    {
        //        txtDecryptValue.Text = o.DecryptPassword(value);
        //    }
        //}
    }
}
