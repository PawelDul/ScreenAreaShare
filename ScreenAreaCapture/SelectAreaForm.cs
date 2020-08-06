using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenAreaCapture
{
    public partial class SelectAreaForm : Form
    {
        private enum CursPos
        {

            WithinSelectionArea = 0,
            OutsideSelectionArea,
            TopLine,
            BottomLine,
            LeftLine,
            RightLine,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }

        private enum ClickAction
        {

            NoClick = 0,
            Dragging,
            Outside,
            TopSizing,
            BottomSizing,
            LeftSizing,
            TopLeftSizing,
            BottomLeftSizing,
            RightSizing,
            TopRightSizing,
            BottomRightSizing
        }

        private ClickAction CurrentAction;
        private bool LeftButtonDown = false;
        private bool RectangleDrawn = false;

        private Point ClickPoint = new Point();
        private Point CurrentTopLeft = new Point();
        private Point CurrentBottomRight = new Point();
        private Point DragClickRelative = new Point();

        private int RectangleHeight = new int();
        private int RectangleWidth = new int();

        private Graphics graphics;
        private Pen MyPen = new Pen(Color.Black, 1);
        private Pen EraserPen = new Pen(Color.FromArgb(255, 255, 192), 1);
        public MainForm InstanceRef { get; set; }

        public SelectAreaForm()
        {
            InitializeComponent();
            this.MouseDown += new MouseEventHandler(OnMouseClick);
            this.MouseDoubleClick += new MouseEventHandler(OnMouseDoubleClick);
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            graphics = this.CreateGraphics();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                e = null;
            }

            base.OnMouseClick(e);
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (RectangleDrawn && (CursorPosition() == CursPos.WithinSelectionArea || CursorPosition() == CursPos.OutsideSelectionArea))
            {
                InstanceRef.OnAreaSelected(CurrentTopLeft, CurrentBottomRight, this);
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetClickAction();
                LeftButtonDown = true;
                ClickPoint = new Point(System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y);

                if (RectangleDrawn)
                {
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    DragClickRelative.X = Cursor.Position.X - CurrentTopLeft.X;
                    DragClickRelative.Y = Cursor.Position.Y - CurrentTopLeft.Y;
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            RectangleDrawn = true;
            LeftButtonDown = false;
            CurrentAction = ClickAction.NoClick;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (LeftButtonDown && !RectangleDrawn)
            {
                DrawSelection();
            }

            if (RectangleDrawn)
            {
                CursorPosition();
                if (CurrentAction == ClickAction.Dragging)
                {
                    DragSelection();
                }

                if (CurrentAction != ClickAction.Dragging && CurrentAction != ClickAction.Outside)
                {
                    ResizeSelection();
                }
            }
        }

        private CursPos CursorPosition()
        {
            if (Cursor.Position.X > CurrentTopLeft.X - 10 && Cursor.Position.X < CurrentTopLeft.X + 10 && ((Cursor.Position.Y > CurrentTopLeft.Y + 10) && (Cursor.Position.Y < CurrentBottomRight.Y - 10)))
            {

                Cursor = Cursors.SizeWE;
                return CursPos.LeftLine;

            }
            if (Cursor.Position.X >= CurrentTopLeft.X - 10 && Cursor.Position.X <= CurrentTopLeft.X + 10 && ((Cursor.Position.Y >= CurrentTopLeft.Y - 10) && (Cursor.Position.Y <= CurrentTopLeft.Y + 10)))
            {

                Cursor = Cursors.SizeNWSE;
                return CursPos.TopLeft;

            }
            if (Cursor.Position.X >= CurrentTopLeft.X - 10 && Cursor.Position.X <= CurrentTopLeft.X + 10 && ((Cursor.Position.Y >= CurrentBottomRight.Y - 10) && (Cursor.Position.Y <= CurrentBottomRight.Y + 10)))
            {

                Cursor = Cursors.SizeNESW;
                return CursPos.BottomLeft;

            }
            if (Cursor.Position.X > CurrentBottomRight.X - 10 && Cursor.Position.X < CurrentBottomRight.X + 10 && (Cursor.Position.Y > CurrentTopLeft.Y + 10) && Cursor.Position.Y < CurrentBottomRight.Y - 10)
            {

                Cursor = Cursors.SizeWE;
                return CursPos.RightLine;

            }
            if (Cursor.Position.X >= CurrentBottomRight.X - 10 && Cursor.Position.X <= CurrentBottomRight.X + 10 && (Cursor.Position.Y >= CurrentTopLeft.Y - 10) && Cursor.Position.Y <= CurrentTopLeft.Y + 10)
            {

                Cursor = Cursors.SizeNESW;
                return CursPos.TopRight;

            }
            if (Cursor.Position.X >= CurrentBottomRight.X - 10 && Cursor.Position.X <= CurrentBottomRight.X + 10 && (Cursor.Position.Y >= CurrentBottomRight.Y - 10) && Cursor.Position.Y <= CurrentBottomRight.Y + 10)
            {

                Cursor = Cursors.SizeNWSE;
                return CursPos.BottomRight;

            }
            if (Cursor.Position.Y > CurrentTopLeft.Y - 10 && (Cursor.Position.Y < CurrentTopLeft.Y + 10) && Cursor.Position.X > CurrentTopLeft.X + 10 && Cursor.Position.X < CurrentBottomRight.X - 10)
            {

                Cursor = Cursors.SizeNS;
                return CursPos.TopLine;

            }
            if (Cursor.Position.Y > CurrentBottomRight.Y - 10 && (Cursor.Position.Y < CurrentBottomRight.Y + 10) && Cursor.Position.X > CurrentTopLeft.X + 10 && Cursor.Position.X < CurrentBottomRight.X - 10)
            {

                Cursor = Cursors.SizeNS;
                return CursPos.BottomLine;

            }
            if (
                Cursor.Position.X >= CurrentTopLeft.X + 10 && Cursor.Position.X <= CurrentBottomRight.X - 10 && Cursor.Position.Y >= CurrentTopLeft.Y + 10 && Cursor.Position.Y <= CurrentBottomRight.Y - 10)
            {

                Cursor = Cursors.Hand;
                return CursPos.WithinSelectionArea;

            }

            Cursor = Cursors.No;
            return CursPos.OutsideSelectionArea;
        }

        private void SetClickAction()
        {

            switch (CursorPosition())
            {
                case CursPos.BottomLine:
                    CurrentAction = ClickAction.BottomSizing;
                    break;
                case CursPos.TopLine:
                    CurrentAction = ClickAction.TopSizing;
                    break;
                case CursPos.LeftLine:
                    CurrentAction = ClickAction.LeftSizing;
                    break;
                case CursPos.TopLeft:
                    CurrentAction = ClickAction.TopLeftSizing;
                    break;
                case CursPos.BottomLeft:
                    CurrentAction = ClickAction.BottomLeftSizing;
                    break;
                case CursPos.RightLine:
                    CurrentAction = ClickAction.RightSizing;
                    break;
                case CursPos.TopRight:
                    CurrentAction = ClickAction.TopRightSizing;
                    break;
                case CursPos.BottomRight:
                    CurrentAction = ClickAction.BottomRightSizing;
                    break;
                case CursPos.WithinSelectionArea:
                    CurrentAction = ClickAction.Dragging;
                    break;
                case CursPos.OutsideSelectionArea:
                    CurrentAction = ClickAction.Outside;
                    break;
            }

        }

        private void ResizeSelection()
        {
            switch (CurrentAction)
            {
                case ClickAction.LeftSizing when Cursor.Position.X < CurrentBottomRight.X - 10:
                    //Erase the previous rectangle
                    graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = Cursor.Position.X;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    break;
                case ClickAction.TopLeftSizing:
                {
                    if (Cursor.Position.X < CurrentBottomRight.X - 10 && Cursor.Position.Y < CurrentBottomRight.Y - 10)
                    {
                        //Erase the previous rectangle
                        graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                        CurrentTopLeft.X = Cursor.Position.X;
                        CurrentTopLeft.Y = Cursor.Position.Y;
                        RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                        RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                        graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    }

                    break;
                }
                case ClickAction.BottomLeftSizing:
                {
                    if (Cursor.Position.X < CurrentBottomRight.X - 10 && Cursor.Position.Y > CurrentTopLeft.Y + 10)
                    {
                        //Erase the previous rectangle
                        graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                        CurrentTopLeft.X = Cursor.Position.X;
                        CurrentBottomRight.Y = Cursor.Position.Y;
                        RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                        RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                        graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    }

                    break;
                }
                case ClickAction.RightSizing:
                {
                    if (Cursor.Position.X > CurrentTopLeft.X + 10)
                    {
                        //Erase the previous rectangle
                        graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                        CurrentBottomRight.X = Cursor.Position.X;
                        RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                        graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    }

                    break;
                }
                case ClickAction.TopRightSizing:
                {
                    if (Cursor.Position.X > CurrentTopLeft.X + 10 && Cursor.Position.Y < CurrentBottomRight.Y - 10)
                    {
                        //Erase the previous rectangle
                        graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                        CurrentBottomRight.X = Cursor.Position.X;
                        CurrentTopLeft.Y = Cursor.Position.Y;
                        RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                        RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                        graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    }

                    break;
                }
                case ClickAction.BottomRightSizing:
                {
                    if (Cursor.Position.X > CurrentTopLeft.X + 10 && Cursor.Position.Y > CurrentTopLeft.Y + 10)
                    {
                        //Erase the previous rectangle
                        graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                        CurrentBottomRight.X = Cursor.Position.X;
                        CurrentBottomRight.Y = Cursor.Position.Y;
                        RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                        RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                        graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    }

                    break;
                }
                case ClickAction.TopSizing:
                {
                    if (Cursor.Position.Y < CurrentBottomRight.Y - 10)
                    {
                        //Erase the previous rectangle
                        graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                        CurrentTopLeft.Y = Cursor.Position.Y;
                        RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                        graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    }

                    break;
                }
                case ClickAction.BottomSizing:
                {
                    if (Cursor.Position.Y > CurrentTopLeft.Y + 10)
                    {
                        //Erase the previous rectangle
                        graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                        CurrentBottomRight.Y = Cursor.Position.Y;
                        RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                        graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    }

                    break;
                }
            }
        }

        private void DragSelection()
        {
            //Ensure that the rectangle stays within the bounds of the screen

            //Erase the previous rectangle
            graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

            if (Cursor.Position.X - DragClickRelative.X > 0 && Cursor.Position.X - DragClickRelative.X + RectangleWidth < System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width)
            {

                CurrentTopLeft.X = Cursor.Position.X - DragClickRelative.X;
                CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;

            }
            else
                //Selection area has reached the right side of the screen
                if (Cursor.Position.X - DragClickRelative.X > 0)
            {

                CurrentTopLeft.X = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - RectangleWidth;
                CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;

            }
            //Selection area has reached the left side of the screen
            else
            {

                CurrentTopLeft.X = 0;
                CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;

            }

            if (Cursor.Position.Y - DragClickRelative.Y > 0 && Cursor.Position.Y - DragClickRelative.Y + RectangleHeight < System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
            {

                CurrentTopLeft.Y = Cursor.Position.Y - DragClickRelative.Y;
                CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;

            }
            else
                //Selection area has reached the bottom of the screen
                if (Cursor.Position.Y - DragClickRelative.Y > 0)
            {

                CurrentTopLeft.Y = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - RectangleHeight;
                CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;

            }
            //Selection area has reached the top of the screen
            else
            {

                CurrentTopLeft.Y = 0;
                CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;

            }

            //Draw a new rectangle
            graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

        }

        private void DrawSelection()
        {

            this.Cursor = Cursors.Arrow;

            //Erase the previous rectangle
            graphics.DrawRectangle(EraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);

            //Calculate X Coordinates
            if (Cursor.Position.X < ClickPoint.X)
            {

                CurrentTopLeft.X = Cursor.Position.X;
                CurrentBottomRight.X = ClickPoint.X;

            }
            else
            {

                CurrentTopLeft.X = ClickPoint.X;
                CurrentBottomRight.X = Cursor.Position.X;

            }

            //Calculate Y Coordinates
            if (Cursor.Position.Y < ClickPoint.Y)
            {

                CurrentTopLeft.Y = Cursor.Position.Y;
                CurrentBottomRight.Y = ClickPoint.Y;

            }
            else
            {

                CurrentTopLeft.Y = ClickPoint.Y;
                CurrentBottomRight.Y = Cursor.Position.Y;

            }

            //Draw a new rectangle
            graphics.DrawRectangle(MyPen, CurrentTopLeft.X, CurrentTopLeft.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);

        }
    }
}
