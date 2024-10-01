using System;
using System.Drawing;

namespace DDSWeb
{
    public partial class graphics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";
            Bitmap b = new Bitmap("d:\\tmp\\finger.jpg");
            b.Save(this.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            b.Dispose();
        }
    }
}
