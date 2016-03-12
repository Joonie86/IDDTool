//DXT Decoding/Encoding Class by gdkchan
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace IDDTool.IO.Compression
{
    class DXT
    {
        private enum Alpha_Mode
        {
            None,
            DXT3,
            DXT5
        }

        /// <summary>
        ///     Decodifica uma textura com compressão DXT1.
        /// </summary>
        /// <param name="Data">O Array de Bytes com a textura no formato DXT1</param>
        /// <param name="Width">A largura da textura</param>
        /// <param name="Height">A altura da textura</param>
        /// <returns>A imagem</returns>
        public static Bitmap DXT1_Decode(byte[] Data, int Width, int Height)
        {
            byte[] Output = new byte[(Width * Height * 4)];
            int Offset = 0;

            for (int Y = 0; Y < Height / 4; Y++)
            {
                for (int X = 0; X < Width / 4; X++)
                {
                    byte[] Block = new byte[8];
                    Buffer.BlockCopy(Data, Offset, Block, 0, 8);
                    Offset += 8;
                    Block = DXT_Decode_Block(Block, true);

                    for (int TY = 0; TY < 4; TY++)
                    {
                        for (int TX = 0; TX < 4; TX++)
                        {
                            int Out_Offset = (X * 4 + TX + ((Y * 4 + TY) * Width)) * 4;
                            int Block_Offset = (TX + (TY * 4)) * 4;
                            Buffer.BlockCopy(Block, Block_Offset, Output, Out_Offset, 3);
                            Output[Out_Offset + 3] = 0xff;
                        }
                    }
                }
            }

            return Get_Bitmap_From_Array(Output, Width, Height);
        }

        /// <summary>
        ///     Decodifica uma textura com compressão DXT3.
        /// </summary>
        /// <param name="Data">O Array de Bytes com a textura no formato DXT3</param>
        /// <param name="Width">A largura da textura</param>
        /// <param name="Height">A altura da textura</param>
        /// <returns>A imagem</returns>
        public static Bitmap DXT3_Decode(byte[] Data, int Width, int Height)
        {
            byte[] Output = new byte[(Width * Height * 4)];
            int Offset = 0;

            for (int Y = 0; Y < Height / 4; Y++)
            {
                for (int X = 0; X < Width / 4; X++)
                {
                    byte[] Block = new byte[8];
                    Buffer.BlockCopy(Data, Offset + 8, Block, 0, 8);
                    Block = DXT_Decode_Block(Block, false);
                    ulong Alpha = BitConverter.ToUInt64(Data, Offset);
                    Offset += 16;

                    for (int TY = 0; TY < 4; TY++)
                    {
                        for (int TX = 0; TX < 4; TX++)
                        {
                            int Out_Offset = (X * 4 + TX + ((Y * 4 + TY) * Width)) * 4;
                            int Block_Offset = (TX + (TY * 4)) * 4;
                            Buffer.BlockCopy(Block, Block_Offset, Output, Out_Offset, 3);
                            ulong AlphaVal = (Alpha >> ((TY * 16) + (TX * 4))) & 0xF;
                            Output[Out_Offset + 3] = (byte)((AlphaVal << 4) | AlphaVal);
                        }
                    }
                }
            }

            return Get_Bitmap_From_Array(Output, Width, Height);
        }

        /// <summary>
        ///     Decodifica uma textura com compressão DXT5.
        /// </summary>
        /// <param name="Data">O Array de Bytes com a textura no formato DXT5</param>
        /// <param name="Width">A largura da textura</param>
        /// <param name="Height">A altura da textura</param>
        /// <returns>A imagem</returns>
        public static Bitmap DXT5_Decode(byte[] Data, int Width, int Height)
        {
            byte[] Output = new byte[(Width * Height * 4)];
            int Offset = 0;

            for (int Y = 0; Y < Height / 4; Y++)
            {
                for (int X = 0; X < Width / 4; X++)
                {
                    byte[] Block = new byte[8];
                    Buffer.BlockCopy(Data, Offset + 8, Block, 0, 8);
                    Block = DXT_Decode_Block(Block, true);
                    byte[] Alpha = new byte[8];
                    Alpha[0] = Data[Offset];
                    Alpha[1] = Data[Offset + 1];
                    DXT5_Calculate_Alphas(Alpha);

                    ulong AlphaVal = BitConverter.ToUInt64(Data, Offset + 2) & 0xffffffffffff;
                    Offset += 16;

                    for (int TY = 0; TY < 4; TY++)
                    {
                        for (int TX = 0; TX < 4; TX++)
                        {
                            int Out_Offset = (X * 4 + TX + ((Y * 4 + TY) * Width)) * 4;
                            int Block_Offset = (TX + (TY * 4)) * 4;
                            Buffer.BlockCopy(Block, Block_Offset, Output, Out_Offset, 3);
                            Output[Out_Offset + 3] = Alpha[(AlphaVal >> ((TY * 12) + (TX * 3))) & 7];
                        }
                    }
                }
            }

            return Get_Bitmap_From_Array(Output, Width, Height);
        }

        /// <summary>
        ///     Codifica uma imagem em uma textura DXT1.
        /// </summary>
        /// <param name="Img">A imagem de origem</param>
        /// <returns>A imagem comprimida em DXT</returns>
        public static byte[] DXT1_Encode(Bitmap Img)
        {
            return DXT_Encode(Img, true, Alpha_Mode.None);
        }

        /// <summary>
        ///     Codifica uma imagem em uma textura DXT3.
        /// </summary>
        /// <param name="Img">A imagem de origem</param>
        /// <returns>A imagem comprimida em DXT</returns>
        public static byte[] DXT3_Encode(Bitmap Img)
        {
            return DXT_Encode(Img, false, Alpha_Mode.DXT3);
        }

        /// <summary>
        ///     Codifica uma imagem em uma textura DXT5.
        /// </summary>
        /// <param name="Img">A imagem de origem</param>
        /// <returns>A imagem comprimida em DXT</returns>
        public static byte[] DXT5_Encode(Bitmap Img)
        {
            return DXT_Encode(Img, true, Alpha_Mode.DXT5);
        }

        private static byte[] DXT_Encode(Bitmap Img, bool DXT1, Alpha_Mode AlphaMode)
        {
            BitmapData ImgData = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadOnly, Img.PixelFormat);
            byte[] Data = new byte[ImgData.Height * ImgData.Stride];
            Marshal.Copy(ImgData.Scan0, Data, 0, Data.Length);
            Img.UnlockBits(ImgData);

            int BPP = 3;
            if (Img.PixelFormat == PixelFormat.Format32bppArgb) BPP = 4;

            byte[] Output = new byte[(((Img.Width / 4) * (Img.Height / 4)) * (AlphaMode != Alpha_Mode.None ? 16 : 8))];
            int Out_Offset = 0;

            for (int Tile_Y = 0; Tile_Y < Img.Height / 4; Tile_Y++)
            {
                for (int Tile_X = 0; Tile_X < Img.Width / 4; Tile_X++)
                {
                    byte[] Block = new byte[8];

                    //Calculo de Luminância mín/máx
                    byte Min_Luma = 0xff;
                    byte Max_Luma = 0;

                    byte MinR = 0, MaxR = 0;
                    byte MinG = 0, MaxG = 0;
                    byte MinB = 0, MaxB = 0;

                    for (int Y = 0; Y < 4; Y++)
                    {
                        for (int X = 0; X < 4; X++)
                        {
                            int Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * BPP;

                            byte R = Data[Offset + 2];
                            byte G = Data[Offset + 1];
                            byte B = Data[Offset];

                            byte Luma = (byte)(0.257f * R + 0.504f * G + 0.098f * B + 16);

                            if (Luma < Min_Luma)
                            {
                                MinR = R;
                                MinG = G;
                                MinB = B;

                                Min_Luma = Luma;
                            }

                            if (Luma > Max_Luma)
                            {
                                MaxR = R;
                                MaxG = G;
                                MaxB = B;

                                Max_Luma = Luma;
                            }
                        }
                    }

                    Block[2] = (byte)((MinB >> 3) | ((MinG & 0x1C) << 3));
                    Block[3] = (byte)(((MinG & 0xE0) >> 5) | (MinR & 0xF8));
                    Block[0] = (byte)((MaxB >> 3) | ((MaxG & 0x1C) << 3));
                    Block[1] = (byte)(((MaxG & 0xE0) >> 5) | (MaxR & 0xF8));

                    Color[] Pixel_Color = new Color[4];
                    ushort c0 = Read16(Block, 0);
                    ushort c1 = Read16(Block, 2);
                    Pixel_Color[0] = Get_Color_From_BGR565(c0);
                    Pixel_Color[1] = Get_Color_From_BGR565(c1);
                    Pixel_Color[2] = Process_DXT1_Color_Channel(2, c0, c1, true);
                    Pixel_Color[3] = Process_DXT1_Color_Channel(3, c0, c1, true);

                    for (int Y = 0; Y < 4; Y++)
                    {
                        for (int X = 0; X < 4; X++)
                        {
                            int Image_Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * BPP;

                            byte R = Data[Image_Offset + 2];
                            byte G = Data[Image_Offset + 1];
                            byte B = Data[Image_Offset];

                            int Luma = (int)(0.257f * R + 0.504f * G + 0.098f * B + 16);

                            int Min_Diff = 0xff;
                            int Index = 0;
                            for (int i = 0; i < 4; i++)
                            {
                                int Test_Luma = (int)(0.257f * Pixel_Color[i].R + 0.504f * Pixel_Color[i].G + 0.098f * Pixel_Color[i].B + 16);
                                int Diff = Math.Abs(Test_Luma - Luma);
                                if (Diff < Min_Diff)
                                {
                                    Min_Diff = Diff;
                                    Index = i;
                                }
                            }

                            Block[4 + Y] |= (byte)(Index << (X * 2));
                        }
                    }

                    byte[] Alpha_Block = new byte[8];
                    if (AlphaMode == Alpha_Mode.DXT3)
                    {
                        for (int Y = 0; Y < 4; Y++)
                        {
                            for (int X = 0; X < 4; X++)
                            {
                                int Image_Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * BPP;

                                byte A = (byte)(Data[Image_Offset + 3] >> 4);

                                Alpha_Block[(Y * 2) + (X / 2)] |= (byte)(A << ((X % 2) * 4));
                            }
                        }

                        Buffer.BlockCopy(Alpha_Block, 0, Output, Out_Offset, Alpha_Block.Length);
                        Out_Offset += Alpha_Block.Length;
                    }
                    else if (AlphaMode == Alpha_Mode.DXT5)
                    {
                        byte Min_Alpha = 0xff;
                        byte Max_Alpha = 0;

                        for (int Y = 0; Y < 4; Y++)
                        {
                            for (int X = 0; X < 4; X++)
                            {
                                int Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * BPP;

                                byte A = Data[Offset + 3];

                                if (A < Min_Alpha) Min_Alpha = A;
                                if (A > Max_Alpha) Max_Alpha = A;
                            }
                        }

                        byte[] Alpha = new byte[8];
                        Alpha[0] = Min_Alpha;
                        Alpha[1] = Max_Alpha;
                        Alpha_Block[0] = Alpha[0];
                        Alpha_Block[1] = Alpha[1];
                        DXT5_Calculate_Alphas(Alpha);

                        uint AlphaVal = 0;
                        byte Shift = 0;
                        int Alpha_Block_Offset = 2;
                        for (int Y = 0; Y < 4; Y++)
                        {
                            for (int X = 0; X < 4; X++)
                            {
                                int Image_Offset = ((Tile_X * 4) + X + (((Tile_Y * 4) + Y) * Img.Width)) * BPP;

                                byte A = Data[Image_Offset + 3];

                                int Min_Diff = 0xff;
                                byte Index = 0;
                                for (int i = 0; i < 8; i++)
                                {
                                    int Diff = Math.Abs(Alpha[i] - A);
                                    if (Diff < Min_Diff)
                                    {
                                        Min_Diff = Diff;
                                        Index = (byte)i;
                                    }
                                }

                                AlphaVal |= (uint)((Index & 7) << Shift);
                                Shift += 3;
                                if (Shift == 24)
                                {
                                    Buffer.BlockCopy(BitConverter.GetBytes(AlphaVal), 0, Alpha_Block, Alpha_Block_Offset, 3);
                                    AlphaVal = 0;
                                    Shift = 0;
                                    Alpha_Block_Offset += 3;
                                }
                            }
                        }

                        Buffer.BlockCopy(Alpha_Block, 0, Output, Out_Offset, Alpha_Block.Length);
                        Out_Offset += Alpha_Block.Length;
                    }

                    Buffer.BlockCopy(Block, 0, Output, Out_Offset, Block.Length);
                    Out_Offset += Block.Length;
                }
            }

            return Output;
        }

        private static void DXT5_Calculate_Alphas(byte[] Alpha)
        {
            for (int i = 2; i < 8; i++)
            {
                if (Alpha[0] > Alpha[1])
                {
                    Alpha[i] = (byte)(((8 - i) * Alpha[0] + (i - 1) * Alpha[1]) / 7);
                }
                else
                {
                    if (i < 6)
                    {
                        Alpha[i] = (byte)(((6 - i) * Alpha[0] + (i - 1) * Alpha[1]) / 7);
                    }
                    else
                    {
                        if (i == 6)
                            Alpha[i] = 0;
                        else if (i == 7)
                            Alpha[i] = 0xff;
                    }
                }
            }
        }

        private static byte[] DXT_Decode_Block(byte[] Data, bool DXT1)
        {
            Color[] Pixel_Color = new Color[4];
            ushort c0 = Read16(Data, 0);
            ushort c1 = Read16(Data, 2);
            Pixel_Color[0] = Get_Color_From_BGR565(c0);
            Pixel_Color[1] = Get_Color_From_BGR565(c1);
            Pixel_Color[2] = Process_DXT1_Color_Channel(2, c0, c1, DXT1);
            Pixel_Color[3] = Process_DXT1_Color_Channel(3, c0, c1, DXT1);

            byte[] Output = new byte[(4 * 4 * 4)];

            int Index = 3;
            int Index_Shift = 0;
            uint Index_Bits = Read32(Data, 4);
            for (int Y = 0; Y < 4; Y++)
            {
                for (int X = 0; X < 4; X++)
                {
                    long Color_Index = (Index_Bits & Index) >> Index_Shift;
                    Color Pixel = Pixel_Color[Color_Index];
                    Index <<= 2;
                    Index_Shift += 2;

                    Output[(Y * 4 + X) * 4] = Pixel.B;
                    Output[((Y * 4 + X) * 4) + 1] = Pixel.G;
                    Output[((Y * 4 + X) * 4) + 2] = Pixel.R;
                }
            }

            return Output;
        }

        private static Color Process_DXT1_Color_Channel(byte cX, ushort c0, ushort c1, bool DXT1)
        {
            Color Output = new Color();
            Color C0 = Get_Color_From_BGR565(c0);
            Color C1 = Get_Color_From_BGR565(c1);

            if (c0 > c1 || !DXT1)
            {
                if (cX == 2)
                    Output = Color.FromArgb((2 * C0.R + C1.R) / 3, (2 * C0.G + C1.G) / 3, (2 * C0.B + C1.B) / 3);
                else
                    Output = Color.FromArgb((2 * C1.R + C0.R) / 3, (2 * C1.G + C0.G) / 3, (2 * C1.B + C0.B) / 3);
            }
            else
            {
                if (cX == 2)
                    Output = Color.FromArgb((C0.R + C1.R) / 2, (C0.G + C1.G) / 2, (C0.B + C1.B) / 2);
                else
                    Output = Color.Black;
            }

            return Output;
        }

        private static Bitmap Get_Bitmap_From_Array(byte[] Array, int Width, int Height)
        {
            Bitmap Img = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData ImgData = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            Marshal.Copy(Array, 0, ImgData.Scan0, Array.Length);
            Img.UnlockBits(ImgData);
            return Img;
        }

        private static Color Get_Color_From_BGR565(ushort Value)
        {
            byte R = (byte)((Value >> 11) & 0x1f);
            byte G = (byte)((Value >> 5) & 0x3f);
            byte B = (byte)((Value) & 0x1f);

            R = (byte)((R << 3) | (R >> 2));
            G = (byte)((G << 2) | (G >> 4));
            B = (byte)((B << 3) | (B >> 2));

            return Color.FromArgb(R, G, B);
        }

        private static uint Read32(byte[] Data, int Address)
        {
            return BitConverter.ToUInt32(Data, Address);
        }

        private static ushort Read16(byte[] Data, int Address)
        {
            return BitConverter.ToUInt16(Data, Address);
        }
    }
}