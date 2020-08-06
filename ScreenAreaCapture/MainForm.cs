using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenAreaCapture
{
    public partial class MainForm : Form
    {
        private Point topLeftPoint;
        private Size captureSize;
        private Graphics captureGraphics;
        private VirtualDesktopManager vdm;
        NewWindow _nw;
        private bool _isWindowMoved = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            vdm = new VirtualDesktopManager();
        }

        public void OnAreaSelected(Point topLeft, Point bottomRight, SelectAreaForm frm)
        {
            topLeftPoint = topLeft;
            captureSize = new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);

            frm.Close();

            this.Show();
            StartCapturing();
        }

        private void StartCapturing()
        {
            var bitmap = new Bitmap(captureSize.Width, captureSize.Height);
            pbScreenDisplay.Image = bitmap;
            this.captureGraphics = Graphics.FromImage(bitmap);

            tPicture.Start();

            //resize
            var width = (decimal)this.Width;
            var pictureWidth = (decimal)pbScreenDisplay.Width;
            var height = (decimal)this.Height;
            var pictureHeight = (decimal)pbScreenDisplay.Height;
            var areaWidth = (decimal)captureSize.Width;
            var areaHeigth = (decimal)captureSize.Height;

            var widthPadding = (width - pictureWidth);
            var heightPadding = (height - pictureHeight);

            var widthRatio = pictureWidth / areaWidth;
            var areaRatio = areaWidth / areaHeigth;
            var newPictureHeight = pictureWidth / areaRatio;
            this.Height = Convert.ToInt32(decimal.Round(newPictureHeight + heightPadding));
        }

        private void tPicture_Tick(object sender, EventArgs e)
        {
            captureGraphics.CopyFromScreen(topLeftPoint.X, topLeftPoint.Y, 0, 0, captureSize);
            captureGraphics.FillEllipse(Brushes.Red, Cursor.Position.X - 5 - topLeftPoint.X, Cursor.Position.Y - 5 - topLeftPoint.Y, 10, 10);
            pbScreenDisplay.Refresh();
        }

        private void startCapture_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frm = new SelectAreaForm();
            frm.InstanceRef = this;
            frm.Show();
            startCapture.Visible = false;
        }

        private async void VDCheckTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (vdm.IsWindowOnCurrentVirtualDesktop(Handle)) return;
                if (_isWindowMoved) return;
                using (_nw = new NewWindow())
                {
                    _nw.Show(null);
                    vdm.MoveWindowToDesktop(Handle, vdm.GetWindowDesktopId(_nw.Handle));
                    _isWindowMoved = true;
                }
            }
            catch
            {
                //This will fail due to race conditions as currently written on occassion
            }
        }
    }


}
