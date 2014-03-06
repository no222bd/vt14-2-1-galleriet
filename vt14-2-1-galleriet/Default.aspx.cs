using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace vt14_2_1_galleriet
{
    public partial class Default : System.Web.UI.Page
    {
        Model.Gallery gallery;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if suerystring exists and display original image
            if (Request.QueryString["imageID"] != null)
            {
                var imageID = Request.QueryString["imageID"];

                ShownImage.ImageUrl = String.Format("~/Content/Images/{0}", imageID);
                ImageBox.Visible = true;
            }
            
            gallery = new Model.Gallery();
        }
        
        public IEnumerable<System.IO.FileInfo> Repeater_GetData()
        {
            return gallery.GetImageNames();
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    //Image upload and thumbnail creation
                    string imageID = gallery.SaveImage(FileUpload.FileContent, FileUpload.FileName);
                    Response.Redirect(String.Format("~/UploadSuccess.html?imageID={0}", imageID));
                }
                catch
                {
                    var custVal = new CustomValidator
                    {
                        IsValid = false,
                        ErrorMessage = "Ett fel inträffade när filen skulle överföras"
                    };

                    Page.Validators.Add(custVal);
                }
            }
        }

        protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Set CssClass of currently show thumbnail
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var fileInfo = (FileInfo)e.Item.DataItem;
                var hyperLink = (HyperLink)e.Item.FindControl("ThumbHyperLink");
                if(fileInfo.Name == Request.QueryString["imageID"])
                {
                    hyperLink.CssClass = "currentImage";
                }
            }
        }
    }
}