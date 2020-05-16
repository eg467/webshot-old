using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webshot.Controls
{
    public partial class LargeImageViewer : UserControl
    {
        public LargeImageViewer()
        {
            InitializeComponent();
        }

        private bool _constrainImageWidth;

        public bool ConstrainImageWidth
        {
            get => _constrainImageWidth;
            set
            {
                _constrainImageWidth = value;
                ScaleImage();
            }
        }

        /// <summary>
        /// How fast autoscroll scrolls the image.
        /// </summary>
        public int AutoScrollImageSpeed { get; set; }

        /// <summary>
        /// How long (in ms) to delay autoscrolling after first displaying an image.
        /// </summary>
        public int AutoScrollImageDelay { get; set; }

        public bool AutoscrollImage
        {
            get => this.timerAutoscroll.Enabled;
            set { this.timerAutoscroll.Enabled = value; }
        }

        /// <summary>
        /// Use with <see cref="AutoScrollImageDelay"/> to delay autoscrolling
        /// immediately after switching images.
        /// </summary>
        private DateTime _lastImageChange = DateTime.MinValue;

        public Image Image
        {
            get => this.picImage.Image;
            set
            {
                _lastImageChange = DateTime.Now;
                this.picImage.Image?.Dispose();
                this.picImage.Image = value;
                ImageLocation = Point.Empty;
                ScaleImage();
                ResizeScrollbars();
            }
        }

        private Point _mouseDownLocation;

        public const int MinScale = 10;
        public const int MaxScale = 500;
        private int _imageScale = 100;

        /// <summary>
        /// How large the image should be relative to its original size (100 = 100%).
        /// </summary>
        public int ImageScale
        {
            get => _imageScale;
            set
            {
                if (value < MinScale || value > MaxScale)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(ImageScale),
                        value,
                        $"The scale must be between {MinScale} and {MaxScale}.");
                }
                _imageScale = value;
                ScaleImage();
            }
        }

        private void ResizeScrollbars()
        {
            var extraPic = Size.Subtract(
                this.picImage.Size,
                this.pnlPic.ClientSize);

            var scrollMargin = new Size(
                extraPic.Width > 0 ? this.hscrollImg.LargeChange : 0,
                extraPic.Height > 0 ? this.vscrollImg.LargeChange : 0);

            var maxScroll = Size.Add(extraPic, scrollMargin);

            this.vscrollImg.Maximum = Math.Max(0, maxScroll.Height);
            this.hscrollImg.Maximum = Math.Max(0, maxScroll.Width);
        }

        private void Picture_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor = Cursors.NoMove2D;
                _mouseDownLocation = e.Location;
            }
        }

        private void Picture_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.picImage.Image != null && e.Button == MouseButtons.Left)
            {
                MoveImage(e.X - _mouseDownLocation.X, e.Y - _mouseDownLocation.Y);
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void ScaleImage()
        {
            if (this.picImage.Image == null)
            {
                return;
            }
            var origImgSize = this.picImage.Image.Size;

            var scale = this.ImageScale / 100.0;

            var scaledSize = new Size(
                (int)Math.Round(scale * origImgSize.Width),
                (int)Math.Round(scale * origImgSize.Height));

            if (this.ConstrainImageWidth
                && scaledSize.Width > this.pnlPic.ClientSize.Width)
            {
                var containerScale =
                    (double)this.pnlPic.ClientSize.Width / scaledSize.Width;

                scaledSize = new Size(
                    (int)Math.Floor(containerScale * scaledSize.Width),
                    (int)Math.Floor(containerScale * scaledSize.Height));
            }

            this.picImage.Size = scaledSize;
            ResizeScrollbars();
            // Moves the image to origin to avoid invalid positions.
            // TODO: Move to closest valid location if already scrolled.
            ImageLocation = Point.Empty;
        }

        private void MoveImage(int dX = 0, int dY = 0)
        {
            var translation = new Size(dX, dY);
            ImageLocation = Point.Add(ImageLocation, translation);
        }

        private Point ImageLocation
        {
            get => this.picImage.Location;
            set
            {
                var constrainedLocation = new Point(
                    Utils.InRange(
                        value.X,
                        this.pnlPic.ClientSize.Width - this.picImage.Width,
                        0),
                    Utils.InRange(
                        value.Y,
                        this.pnlPic.ClientSize.Height - this.picImage.Height,
                        0));

                if (this.picImage.Location == constrainedLocation)
                {
                    return;
                }

                this.picImage.Location = constrainedLocation;
                this.vscrollImg.Value = -constrainedLocation.Y;
                this.hscrollImg.Value = -constrainedLocation.X;
            }
        }

        private void VscrollImg_Scroll(object sender, ScrollEventArgs e)
        {
            ImageLocation = new Point(ImageLocation.X, -this.vscrollImg.Value);
        }

        private void HscrollImg_Scroll(object sender, ScrollEventArgs e)
        {
            ImageLocation = new Point(-this.hscrollImg.Value, ImageLocation.Y);
        }

        private void PnlPic_MouseWheel(object sender, MouseEventArgs e)
        {
            MoveImage(dY: e.Delta);
        }

        private void PnlPic_Resize(object sender, EventArgs e)
        {
            if (ConstrainImageWidth)
            {
                ScaleImage();
            }
            ResizeScrollbars();
        }

        private void LargeImageViewer_Load(object sender, EventArgs e)
        {
            this.pnlPic.MouseWheel += PnlPic_MouseWheel;
        }

        private void TimerAutoscroll_Tick(object sender, EventArgs e)
        {
            var timeEllapsed = DateTime.Now.Subtract(_lastImageChange);

            // Perform autoscroll
            if (this.picImage.Image != null
            && timeEllapsed.TotalMilliseconds >= AutoScrollImageDelay
            && this.vscrollImg.Value < this.vscrollImg.Maximum)
            {
                MoveImage(dY: -this.AutoScrollImageSpeed);
            }
        }
    }
}