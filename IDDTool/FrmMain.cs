using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using IDDTool.IO.Compression;
using IDDTool.IO.Format;

namespace IDDTool
{
    public partial class FrmMain : Form
    {
        string CurrentFilePath;
        IDDContent CurrentFile;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void MenuOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OpenDlg = new OpenFileDialog())
            {
                OpenDlg.Filter = "Bayonetta IDD|*.idd";

                if (OpenDlg.ShowDialog() == DialogResult.OK && File.Exists(OpenDlg.FileName))
                {
                    CurrentFilePath = OpenDlg.FileName;
                    CurrentFile = IDD.Load(new FileStream(CurrentFilePath, FileMode.Open));

                    TextureImg.Image = null;
                    LstTextures.Items.Clear();
                    LstItems.Items.Clear();
                    NUDX.Value = 0;
                    NUDY.Value = 0;
                    NUDW.Value = 0;
                    NUDH.Value = 0;

                    for (int Index = 0; Index < CurrentFile.Textures.Count; Index++)
                    {
                        LstTextures.Items.Add(string.Format("Texture {0:D5}", Index));
                    }
                }
            }
        }

        private void MenuSave_Click(object sender, EventArgs e)
        {
            if (CurrentFile != null)
            {
                IDD.Save(new FileStream(CurrentFilePath, FileMode.Open), CurrentFile);
                MessageBox.Show(
                    "Changes saved successfully!",
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LstTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            TexturePanel.SuspendLayout();
            TextureImg.Image = null;
            LstItems.Items.Clear();
            NUDX.Value = 0;
            NUDY.Value = 0;
            NUDW.Value = 0;
            NUDH.Value = 0;

            if (LstTextures.SelectedIndex > -1)
            {
                IDDTexture Texture = CurrentFile.Textures[LstTextures.SelectedIndex];
                DisplayTexture(Texture);

                //Populate items
                LstItems.Items.Clear();
                LstItems.SuspendLayout();
                foreach (IDDTextureElement Element in Texture.Elements)
                {
                    LstItems.Items.Add(string.Format("Element {0:D5}", Element.Id));
                }

                LstItems.ResumeLayout();
            }

            TexturePanel.ResumeLayout();
        }

        private void DisplayTexture(IDDTexture Texture)
        {
            byte[] Data = Texture.TextureData;
            int Width = Texture.Resolution.Width;
            int Height = Texture.Resolution.Height;

            switch (Texture.Format)
            {
                case TextureFormat.DXT1: TextureImg.Image = DXT.DXT1_Decode(Data, Width, Height); break;
                case TextureFormat.DXT3: TextureImg.Image = DXT.DXT3_Decode(Data, Width, Height); break;
                case TextureFormat.DXT5: TextureImg.Image = DXT.DXT5_Decode(Data, Width, Height); break;
            }
        }

        private void LstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextureImg.Refresh();
        }

        Rectangle NewSizes;
        Rectangle ResizeRect;
        bool IsHoveringReisze;

        private void TextureImg_Paint(object sender, PaintEventArgs e)
        {
            if (LstItems.SelectedIndex > -1)
            {
                if (IsResizing)
                {
                    e.Graphics.DrawRectangle(Pens.Red, NewSizes);

                    NUDX.Value = NewSizes.X;
                    NUDY.Value = NewSizes.Y;
                    NUDW.Value = NewSizes.Width;
                    NUDH.Value = NewSizes.Height;
                }
                else
                {
                    Rectangle Region = GetElementRegion();

                    NUDX.Value = Region.X;
                    NUDY.Value = Region.Y;
                    NUDW.Value = Region.Width;
                    NUDH.Value = Region.Height;

                    e.Graphics.DrawRectangle(Pens.Blue, Region);
                    if (IsHoveringReisze) e.Graphics.DrawRectangle(Pens.Red, ResizeRect);
                }
            }
        }

        private Rectangle GetElementRegion()
        {
            IDDTexture Texture = CurrentFile.Textures[LstTextures.SelectedIndex];
            IDDTextureElement Element = Texture.Elements[LstItems.SelectedIndex];

            Rectangle Region = new Rectangle();
            Region.X = (int)(Element.X1 * Texture.Resolution.Width);
            Region.Y = (int)(Element.Y1 * Texture.Resolution.Height);
            Region.Width = (int)(Element.X2 * Texture.Resolution.Width) - Region.X;
            Region.Height = (int)(Element.Y2 * Texture.Resolution.Height) - Region.Y;

            return Region;
        }

        [Flags]
        enum ResizeCorner
        {
            None = 0,
            Top = 1,
            Left = 2,
            Bottom = 4,
            Right = 8,
            TopLeft = Top | Left,
            TopRight = Top | Right,
            BottomLeft = Bottom | Left,
            BottomRight = Bottom | Right
        }

        Point ResizeStart;
        bool IsResizing;
        ResizeCorner CurrentCorner;

        private void TextureImg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && LstTextures.SelectedIndex > -1 && !IsResizing)
            {
                IDDTexture Texture = CurrentFile.Textures[LstTextures.SelectedIndex];

                int Index = 0;
                foreach (IDDTextureElement Element in Texture.Elements)
                {
                    Rectangle Region = new Rectangle();
                    Region.X = (int)(Element.X1 * Texture.Resolution.Width);
                    Region.Y = (int)(Element.Y1 * Texture.Resolution.Height);
                    Region.Width = (int)(Element.X2 * Texture.Resolution.Width) - Region.X;
                    Region.Height = (int)(Element.Y2 * Texture.Resolution.Height) - Region.Y;

                    if (Region.Contains(e.Location))
                    {
                        LstItems.SelectedIndex = Index;
                        break;
                    }

                    Index++;
                }
            }
            else
            {
                if (LstItems.SelectedIndex > -1 && GetElementRegion().Contains(e.Location))
                {
                    ResizeStart = e.Location;
                    IsResizing = true;
                }
            }
        }

        private void TextureImg_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            IsHoveringReisze = false;
            IsResizing = false;
            TextureImg.Refresh();
        }

        private void TextureImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (LstItems.SelectedIndex > -1)
            {
                IDDTexture Texture = CurrentFile.Textures[LstTextures.SelectedIndex];
                IDDTextureElement Element = Texture.Elements[LstItems.SelectedIndex];

                int X1 = (int)(Element.X1 * Texture.Resolution.Width);
                int Y1 = (int)(Element.Y1 * Texture.Resolution.Height);
                int X2 = (int)(Element.X2 * Texture.Resolution.Width);
                int Y2 = (int)(Element.Y2 * Texture.Resolution.Height);

                if (IsResizing)
                {
                    int DiffX = e.X - ResizeStart.X;
                    int DiffY = e.Y - ResizeStart.Y;

                    if (CurrentCorner == ResizeCorner.None)
                    {
                        int W = X2 - X1;
                        int H = Y2 - Y1;

                        X1 += DiffX;
                        Y1 += DiffY;

                        NewSizes = new Rectangle(X1, Y1, W, H);
                    }
                    else
                    {
                        if ((CurrentCorner & ResizeCorner.Top) != 0) Y1 = Math.Min(Y1 + DiffY, Y2 - 1);
                        if ((CurrentCorner & ResizeCorner.Left) != 0) X1 = Math.Min(X1 + DiffX, X2 - 1);
                        if ((CurrentCorner & ResizeCorner.Bottom) != 0) Y2 = Math.Max(Y2 + DiffY, Y1 + 1);
                        if ((CurrentCorner & ResizeCorner.Right) != 0) X2 = Math.Max(X2 + DiffX, X1 + 1);

                        NewSizes = new Rectangle(X1, Y1, X2 - X1, Y2 - Y1);
                    }

                    TextureImg.Refresh();
                }
                else
                {
                    int W = X2 - X1;
                    int H = Y2 - Y1;

                    const int GS = 12; //Grip size
                    const int GS2 = GS * 2;
                    bool OldHoveringResize = IsHoveringReisze;
                    IsHoveringReisze = false;
                    CurrentCorner = ResizeCorner.None;
                    Point P = e.Location;

                    //Diamond
                    CheckIntersection(new Rectangle(X1 + GS, Y1, W - GS2, GS), P, Cursors.SizeNS, ResizeCorner.Top);
                    CheckIntersection(new Rectangle(X1, Y1 + GS, GS, H - GS2), P, Cursors.SizeWE, ResizeCorner.Left);
                    CheckIntersection(new Rectangle(X1 + GS, Y2 - GS, W - GS2, GS), P, Cursors.SizeNS, ResizeCorner.Bottom);
                    CheckIntersection(new Rectangle(X2 - GS, Y1 + GS, GS, H - GS2), P, Cursors.SizeWE, ResizeCorner.Right);

                    //Square
                    CheckIntersection(new Rectangle(X1, Y1, GS, GS), P, Cursors.SizeNWSE, ResizeCorner.TopLeft);
                    CheckIntersection(new Rectangle(X2 - GS, Y1, GS, GS), P, Cursors.SizeNESW, ResizeCorner.TopRight);
                    CheckIntersection(new Rectangle(X1, Y2 - GS, GS, GS), P, Cursors.SizeNESW, ResizeCorner.BottomLeft);
                    CheckIntersection(new Rectangle(X2 - GS, Y2 - GS, GS, GS), P, Cursors.SizeNWSE, ResizeCorner.BottomRight);

                    if (IsHoveringReisze || (IsHoveringReisze != OldHoveringResize)) TextureImg.Refresh();
                    if (!IsHoveringReisze)
                    {
                        if (new Rectangle(X1, Y1, W, H).Contains(e.Location))
                            Cursor = Cursors.SizeAll;
                        else
                            Cursor = Cursors.Default;
                    }
                }
            }
        }

        private void CheckIntersection(Rectangle Rect, Point Position, Cursor NewCursor, ResizeCorner Corner)
        {
            if (Rect.Contains(Position))
            {
                Cursor = NewCursor;
                CurrentCorner = Corner;
                ResizeRect = Rect;
                IsHoveringReisze = true;
            }
        }

        private void TextureImg_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsResizing)
            {
                //Update the bounds on the object
                if (LstItems.SelectedIndex > -1) SetNewSizes(NewSizes);
                IsResizing = false;
                TextureImg.Refresh();
            }
        }

        private void NUD_ValueChanged(object sender, EventArgs e)
        {
            if (LstItems.SelectedIndex > -1 && !IsResizing)
            {
                Rectangle Rect = new Rectangle();

                Rect.X = Convert.ToInt32(NUDX.Value);
                Rect.Y = Convert.ToInt32(NUDY.Value);
                Rect.Width = Convert.ToInt32(NUDW.Value);
                Rect.Height = Convert.ToInt32(NUDH.Value);

                SetNewSizes(Rect);
                TextureImg.Refresh();
            }
        }

        private void SetNewSizes(Rectangle NewRect)
        {
            IDDTexture Texture = CurrentFile.Textures[LstTextures.SelectedIndex];
            IDDTextureElement Element = Texture.Elements[LstItems.SelectedIndex];

            Element.X1 = (float)NewRect.X / Texture.Resolution.Width;
            Element.Y1 = (float)NewRect.Y / Texture.Resolution.Height;
            Element.X2 = (float)(NewRect.X + NewRect.Width) / Texture.Resolution.Width;
            Element.Y2 = (float)(NewRect.Y + NewRect.Height) / Texture.Resolution.Height;

            Texture.Elements.RemoveAt(LstItems.SelectedIndex);
            Texture.Elements.Insert(LstItems.SelectedIndex, Element);
            CurrentFile.Textures.RemoveAt(LstTextures.SelectedIndex);
            CurrentFile.Textures.Insert(LstTextures.SelectedIndex, Texture);
        }

        private void BtnExportPNG_Click(object sender, EventArgs e)
        {
            if (LstTextures.SelectedIndex > -1)
            {
                using (SaveFileDialog SaveDlg = new SaveFileDialog())
                {
                    SaveDlg.Filter = "PNG image|*.png";

                    if (SaveDlg.ShowDialog() == DialogResult.OK)
                    {
                        IDDTexture Texture = CurrentFile.Textures[LstTextures.SelectedIndex];

                        byte[] Data = Texture.TextureData;
                        int Width = Texture.Resolution.Width;
                        int Height = Texture.Resolution.Height;

                        switch (Texture.Format)
                        {
                            case TextureFormat.DXT1: DXT.DXT1_Decode(Data, Width, Height).Save(SaveDlg.FileName); break;
                            case TextureFormat.DXT3: DXT.DXT3_Decode(Data, Width, Height).Save(SaveDlg.FileName); break;
                            case TextureFormat.DXT5: DXT.DXT5_Decode(Data, Width, Height).Save(SaveDlg.FileName); break;
                        }
                    }
                }
            }
        }

        private void BtnExportDDS_Click(object sender, EventArgs e)
        {
            if (LstTextures.SelectedIndex > -1)
            {
                using (SaveFileDialog SaveDlg = new SaveFileDialog())
                {
                    SaveDlg.Filter = "DDS texture|*.dds";

                    if (SaveDlg.ShowDialog() == DialogResult.OK)
                    {
                        IDDTexture Texture = CurrentFile.Textures[LstTextures.SelectedIndex];
                        DDS.Export(Texture, SaveDlg.FileName);
                    }
                }
            }
        }

        private void BtnImportPNG_Click(object sender, EventArgs e)
        {
            if (LstTextures.SelectedIndex > -1)
            {
                using (OpenFileDialog OpenDlg = new OpenFileDialog())
                {
                    OpenDlg.Filter = "PNG image|*.png";

                    if (OpenDlg.ShowDialog() == DialogResult.OK && File.Exists(OpenDlg.FileName))
                    {
                        Bitmap Img = new Bitmap(OpenDlg.FileName);
                        IDDTexture Texture = CurrentFile.Textures[LstTextures.SelectedIndex];

                        if (Img.Size != Texture.Resolution)
                        {
                            MessageBox.Show(
                                "The image can't be imported because the size is different!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);

                            return;
                        }

                        byte[] Data = null;
                        switch (Texture.Format)
                        {
                            case TextureFormat.DXT1: Data = DXT.DXT1_Encode(Img); break;
                            case TextureFormat.DXT3: Data = DXT.DXT3_Encode(Img); break;
                            case TextureFormat.DXT5: Data = DXT.DXT5_Encode(Img); break;
                        }

                        if (Data != null)
                        {
                            Texture.TextureData = Data;
                            CurrentFile.Textures.RemoveAt(LstTextures.SelectedIndex);
                            CurrentFile.Textures.Insert(LstTextures.SelectedIndex, Texture);
                            DisplayTexture(Texture);
                        }
                    }
                }
            }
        }

        private void BtnImportDDS_Click(object sender, EventArgs e)
        {
            if (LstTextures.SelectedIndex > -1)
            {
                using (OpenFileDialog OpenDlg = new OpenFileDialog())
                {
                    OpenDlg.Filter = "DDS texture|*.dds";

                    if (OpenDlg.ShowDialog() == DialogResult.OK && File.Exists(OpenDlg.FileName))
                    {
                        DDSContent DDSTexture = DDS.Import(OpenDlg.FileName);
                        IDDTexture Texture = CurrentFile.Textures[LstTextures.SelectedIndex];

                        if (DDSTexture.Resolution != Texture.Resolution)
                        {
                            MessageBox.Show(
                                "The texture can't be imported because the size is different!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);

                            return;
                        }

                        if (DDSTexture.Format != Texture.Format)
                        {
                            MessageBox.Show(
                                string.Format("The texture can't be imported because the format is different! (Expected {0})", Texture.Format),
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);

                            return;
                        }

                        Texture.TextureData = DDSTexture.TextureData;
                        CurrentFile.Textures.RemoveAt(LstTextures.SelectedIndex);
                        CurrentFile.Textures.Insert(LstTextures.SelectedIndex, Texture);
                        DisplayTexture(Texture);
                    }
                }
            }
        }

        private void MenuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("IDDTool v0.1.0 made by gdkchan!", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
