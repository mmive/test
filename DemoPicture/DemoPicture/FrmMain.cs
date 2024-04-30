using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoPicture
{
    public partial class FrmMain : Form
    {
        private string fileFilter="JPG文件|*.jpg|PNG文件|*.png|BMP文件|*.bmp|GIF文件|*.gif|ICON文件|*.ico";

        public FrmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 打开图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {

            this.fileDialog.Filter = fileFilter;
            this.fileDialog.Multiselect = true;
            this.fileDialog.CheckFileExists = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string[] fileNames = this.fileDialog.FileNames;
                foreach(string fileName in fileNames)
                {
                    Bitmap bmp = new Bitmap(fileName);
                    //保存图片名称
                    bmp.Tag = Path.GetFileNameWithoutExtension(fileName);
                    PictureBox box = new PictureBox();
                    box.Image = bmp;
                    box.Width = 105;
                    box.Height = 150;
                    box.BorderStyle = BorderStyle.FixedSingle;
                    box.Padding = new Padding(2);
                    this.flowPnl.Controls.Add(box);
                }
                this.txtFile.Text = Path.GetDirectoryName(fileNames[0]);

            }
        }

        /// <summary>
        /// 转换图片按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (this.cmbFilter.SelectedItem == null)
            {
                MessageBox.Show("没有选择要转换的图片类型");
                return;
            }
            
            if (this.flowPnl.Controls.Count == 0)
            {
                MessageBox.Show("没有要转换的图片");
                return;
            }
            
            string filter = this.cmbFilter.SelectedItem.ToString();
            string dir = string.Empty;
            if (this.folderDialog.ShowDialog() == DialogResult.OK)
            {
                dir = this.folderDialog.SelectedPath;
            }
            else
            {
                return;
            }
            foreach (PictureBox box in this.flowPnl.Controls)
            {
                convertImage(dir, filter, new Bitmap(box.Image) { Tag=box.Image.Tag});
            }

        }

        /// <summary>
        /// 转换图片
        /// </summary>
        private void convertImage(string dir, string filter,Bitmap bmp)
        {
            string filePath = Path.Combine(dir, string.Format("{0}.{1}", bmp.Tag.ToString(), filter.ToLower()));
            switch (filter)
            {
                case "JPG":
                    bmp.Save(filePath, ImageFormat.Jpeg);
                    break;
                case "PNG":
                    bmp.Save(filePath, ImageFormat.Png);
                    break;
                case "GIF":
                    bmp.Save(filePath, ImageFormat.Gif);
                    break;
                case "BMP":
                    bmp.Save(filePath, ImageFormat.Bmp);
                    break;
                case "ICO":
                    Stream stream = File.Create(filePath);
                    Icon icon = Icon.FromHandle(bmp.GetHicon());
                    icon.Save(stream);       //   save the icon
                    stream.Close();
                    break;
            }
        }
    }
}
