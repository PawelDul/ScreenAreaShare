using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WindowsFormsApp1;

namespace ScreenAreaShare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Graphics captureGraphics;
        private System.Drawing.Point topLeftPoint;
        private System.Drawing.Size captureSize;
        private System.Drawing.Rectangle captureRectangle;
        private Bitmap bitmap;
        private Int32Rect rect;
        private BitmapData pixels;
        private int bufferSize;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            captureGraphics.CopyFromScreen(topLeftPoint.X, topLeftPoint.Y, 0, 0, captureSize);
            captureGraphics.FillEllipse(System.Drawing.Brushes.Red, System.Windows.Forms.Control.MousePosition.X - 5 - topLeftPoint.X, System.Windows.Forms.Control.MousePosition.Y - 5 - topLeftPoint.Y, 10, 10);

            var bm = (WriteableBitmap)pbScreenDisplay.Source;

            try
            {
                pixels = bitmap.LockBits(captureRectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                bm.Lock();
                bm.WritePixels(rect, pixels.Scan0, bufferSize, pixels.Stride);
                bm.Unlock();
            }
            finally
            {
                bitmap.UnlockBits(pixels);
            }            
        }

        private void bSelectArea_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            //pass points somehow here
            var p = pbScreenDisplay.Cursor;
            var frm = new SelectAreaForm();
            frm.InstanceRef = this;
            frm.Show();
            bSelectArea.Visibility = Visibility.Hidden;
        }

        public void OnAreaSelected(System.Drawing.Point topLeft, System.Drawing.Point bottomRight, SelectAreaForm frm)
        {
            topLeftPoint = topLeft;
            captureSize = new System.Drawing.Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            rect = new Int32Rect(0, 0, captureSize.Width, captureSize.Height);
            captureRectangle = new Rectangle(0, 0, captureSize.Width, captureSize.Height);
            bufferSize = captureSize.Width * captureSize.Height * 4;
            frm.Close();

            this.Show();

            //prepare
            this.bitmap = new Bitmap(captureSize.Width, captureSize.Height);
            this.captureGraphics = Graphics.FromImage(bitmap);
            pbScreenDisplay.Source = new WriteableBitmap(captureSize.Width, captureSize.Height, 96, 96, PixelFormats.Bgra32, null);

            //resize
            var width = (decimal)Application.Current.MainWindow.Width;
            var pictureWidth = (decimal)gAll.ActualWidth;
            var height = (decimal)Application.Current.MainWindow.Height;
            var pictureHeight = (decimal)gAll.ActualHeight;
            var areaWidth = (decimal)captureSize.Width;
            var areaHeigth = (decimal)captureSize.Height;

            var widthPadding = (width - pictureWidth);
            var heightPadding = (height - pictureHeight);

            var areaRatio = areaWidth / areaHeigth;
            var newPictureHeight = pictureWidth / areaRatio;
            Application.Current.MainWindow.Height = Convert.ToInt32(decimal.Round(newPictureHeight + heightPadding));

            //start            
            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 40);
            dispatcherTimer.Start();
        }
    }
}
