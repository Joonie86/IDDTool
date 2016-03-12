using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace IDDTool.IO.Format
{
    public class IDDTextureElement
    {
        public uint Offset;
        public int Id;
        public float X1;
        public float Y1;
        public float X2;
        public float Y2;
    }

    public enum Console
    {
        Playstation3,
        Xbox360
    }

    public class IDDTexture
    {
        public TextureFormat Format;
        public Console Platform;
        public Size Resolution;
        public uint TextureOffset;
        public byte[] TextureData;
        public List<IDDTextureElement> Elements;

        public IDDTexture()
        {
            Elements = new List<IDDTextureElement>();
        }
    }

    public class IDDContent
    {
        public List<IDDTexture> Textures;

        public IDDContent()
        {
            Textures = new List<IDDTexture>();
        }
    }

    /// <summary>
    ///     Handles IDD files from Bayonetta.
    /// </summary>
    class IDD
    {
        /// <summary>
        ///     Saves the changes to a Bayonetta *.idd file.
        /// </summary>
        /// <param name="Output">The output *.idd file</param>
        /// <param name="ModifiedContent">The changes to be written</param>
        public static void Save(Stream Output, IDDContent ModifiedContent)
        {
            EndianBinaryWriter Writer = new EndianBinaryWriter(Output, Endian.Big);

            for (int Index = 0; Index < ModifiedContent.Textures.Count; Index++)
            {
                IDDTexture Texture = ModifiedContent.Textures[Index];

                byte[] Data = Texture.TextureData;
                if (Texture.Platform == Console.Xbox360)
                {
                    Data = XTextureScramble(Data, Texture, true);
                    Data = XEndian16(Data);
                }

                Output.Seek(Texture.TextureOffset, SeekOrigin.Begin);
                Writer.Write(Data);

                foreach (IDDTextureElement Element in Texture.Elements)
                {
                    Output.Seek(Element.Offset, SeekOrigin.Begin);

                    Writer.Write((ushort)Element.Id);
                    Writer.Write((ushort)Index);
                    Writer.Write(Element.X1);
                    Writer.Write(Element.Y1);
                    Writer.Write(Element.X2);
                    Writer.Write(Element.Y2);
                }
            }

            Output.Close();
        }

        /// <summary>
        ///     Loads the textures of a Bayonetta *.idd file into a object.
        /// </summary>
        /// <param name="Input">The input Stream with the IDD data</param>
        /// <returns>The textures and mappings</returns>
        public static IDDContent Load(Stream Input)
        {
            IDDContent Output = new IDDContent();

            EndianBinaryReader Reader = new EndianBinaryReader(Input, Endian.Big);

            string Signature = StringUtilities.ReadASCIIString(Input); //IDD
            uint UsedSectionsCount = Reader.ReadUInt32();

            uint TEXOffset = 0;
            uint TUVOffset = 0;

            for (int i = 0; i < UsedSectionsCount; i++)
            {
                Input.Seek(8 + i * 8, SeekOrigin.Begin);

                uint Index = Reader.ReadUInt32();
                uint Offset = Reader.ReadUInt32();

                switch (Index)
                {
                    case 0: TEXOffset = Offset; break;
                    case 1: TUVOffset = Offset; break;
                }
            }

            Input.Seek(TEXOffset + 4, SeekOrigin.Begin);
            if (Reader.ReadUInt32() == 0) throw new Exception("This file doesn't contain any texture!");
            Reader.ReadUInt32();
            uint BaseTextureOffset = TEXOffset + Reader.ReadUInt32();
            Input.Seek(BaseTextureOffset, SeekOrigin.Begin);

            uint WTBSignature = Reader.ReadUInt32();
            Reader.ReadUInt32();
            uint TexturesCount = Reader.ReadUInt32();
            uint AddressTableOffset = Reader.ReadUInt32();
            uint LengthTableOffset = Reader.ReadUInt32();
            Reader.ReadUInt32(); //Another ??? table offset

            for (int i = 0; i < TexturesCount; i++)
            {
                IDDTexture Texture = new IDDTexture();

                Input.Seek(BaseTextureOffset + AddressTableOffset + i * 4, SeekOrigin.Begin);
                Input.Seek(BaseTextureOffset + Reader.ReadUInt32(), SeekOrigin.Begin);

                if (Reader.ReadUInt32() == 3)
                {
                    //Xbox 360
                    uint Count = Reader.ReadUInt32();
                    Input.Seek(0xc, SeekOrigin.Current); //0x0
                    Reader.ReadUInt32(); //0xFFFF0000
                    Reader.ReadUInt32(); //0xFFFF0000
                    Reader.ReadUInt32(); //0x81000002
                    uint TextureFormat = Reader.ReadUInt32();
                    uint TextureDescriptor = Reader.ReadUInt32();
                    Reader.ReadUInt32(); //0xD10
                    uint Mipmaps = ((Reader.ReadUInt32() >> 6) & 7) + 1;
                    uint OriginalLength = Reader.ReadUInt32();

                    int Width = (int)(TextureDescriptor & 0x1fff) + 1;
                    int Height = (int)((TextureDescriptor >> 13) & 0x1fff) + 1;

                    switch (TextureFormat)
                    {
                        case 0x52: Texture.Format = Format.TextureFormat.DXT1; break;
                        case 0x53: Texture.Format = Format.TextureFormat.DXT3; break;
                        case 0x54: Texture.Format = Format.TextureFormat.DXT5; break;
                        default: throw new Exception(string.Format("Unsupported IDD X360 texture format 0x{0:X2}!", TextureFormat));
                    }
                    Texture.Platform = Console.Xbox360;
                    Texture.Resolution = new Size(Width, Height);
                    Texture.TextureOffset = (uint)Input.Position;

                    int Length = Width * Height;
                    if (Texture.Format == Format.TextureFormat.DXT1) Length /= 2;
                    byte[] Data = new byte[Length];
                    Reader.Read(Data, 0, Data.Length);
                    Data = XEndian16(Data);
                    Data = XTextureScramble(Data, Texture, false);
                    Texture.TextureData = Data;
                }
                else
                {
                    //Playstation 3
                    uint Length = Reader.ReadUInt32();
                    uint TextureCount = Reader.ReadUInt32();
                    uint Id = Reader.ReadUInt32();
                    uint TextureDataOffset = Reader.ReadUInt32();
                    uint TextureDataLength = Reader.ReadUInt32();
                    byte TextureFormat = Reader.ReadByte();
                    byte Mipmaps = Reader.ReadByte();
                    byte Dimension = Reader.ReadByte();
                    byte Cubemaps = Reader.ReadByte();
                    uint Remap = Reader.ReadUInt32();
                    ushort Width = Reader.ReadUInt16();
                    ushort Height = Reader.ReadUInt16();
                    ushort Depth = Reader.ReadUInt16();
                    ushort Pitch = Reader.ReadUInt16();
                    ushort Location = Reader.ReadUInt16();
                    uint TextureOffset = Reader.ReadUInt16();
                    Reader.Seek(8, SeekOrigin.Current);

                    bool IsSwizzle = (TextureFormat & 0x20) == 0;
                    bool IsNormalized = (TextureFormat & 0x40) == 0;
                    TextureFormat = (byte)(TextureFormat & ~0x60);

                    switch (TextureFormat)
                    {
                        case 0x86: Texture.Format = Format.TextureFormat.DXT1; break;
                        case 0x87: Texture.Format = Format.TextureFormat.DXT3; break;
                        case 0x88: Texture.Format = Format.TextureFormat.DXT5; break;
                        default: throw new Exception(string.Format("Unsupported IDD PS3 texture format 0x{0:X2}!", TextureFormat));
                    }
                    Texture.Platform = Console.Playstation3;
                    Texture.Resolution = new Size(Width, Height);
                    Texture.TextureOffset = (uint)Input.Position;
                    Texture.TextureData = new byte[Length];
                    Reader.Read(Texture.TextureData, 0, (int)Length);
                }

                //Texture Map stuff (needs optimization)
                Input.Seek(TUVOffset + 0xc, SeekOrigin.Begin);
                uint BaseMapOffset = TUVOffset + Reader.ReadUInt32();
                Input.Seek(BaseMapOffset, SeekOrigin.Begin);

                uint Entries = Reader.ReadUInt32();
                for (int j = 0; j < Entries; j++)
                {
                    Input.Seek(BaseMapOffset + 4 + j * 8, SeekOrigin.Begin);

                    ushort Offset = Reader.ReadUInt16(); //???
                    ushort StartIndex = Reader.ReadUInt16();
                    ushort Count = Reader.ReadUInt16();
                    Reader.ReadUInt16(); //???

                    long ElementOffset = BaseMapOffset + 4 + Entries * 8 + StartIndex * 0x14;
                    Input.Seek(ElementOffset, SeekOrigin.Begin);

                    for (int k = 0; k < Count; k++)
                    {
                        ushort ElementId = Reader.ReadUInt16();
                        ushort TextureIndex = Reader.ReadUInt16();
                        float X1 = Reader.ReadSingle();
                        float Y1 = Reader.ReadSingle();
                        float X2 = Reader.ReadSingle();
                        float Y2 = Reader.ReadSingle();

                        if (TextureIndex == i)
                        {
                            IDDTextureElement Element = new IDDTextureElement();

                            Element.Offset = (uint)(ElementOffset + k * 0x14);
                            Element.Id = ElementId;
                            Element.X1 = X1;
                            Element.Y1 = Y1;
                            Element.X2 = X2;
                            Element.Y2 = Y2;

                            Texture.Elements.Add(Element);
                        }
                    }
                }

                Output.Textures.Add(Texture);
            }

            Input.Close();
            return Output;
        }

        #region "Xbox 360 swizzling"
        //Xbox 360 texture un-swizzling shit starts here
        private static byte[] XEndian16(byte[] Data)
        {
            byte[] Output = new byte[Data.Length];

            for (int i = 0; i < Data.Length; i += 2)
            {
                Output[i] = Data[i + 1];
                Output[i + 1] = Data[i];
            }

            return Output;
        }

        //This code was adapted from GTA XTD Viewer tool
        private static byte[] XTextureScramble(byte[] Data, IDDTexture Texture, bool ToLinear = false)
        {
            byte[] Output = new byte[Data.Length];

            int BlockSize = 0;
            int TexelPitch = 0;

            if (Texture.Format == TextureFormat.DXT1)
            {
                BlockSize = 4;
                TexelPitch = 8;
            }
            else
            {
                BlockSize = 4;
                TexelPitch = 16;
            }

            int BlockWidth = Texture.Resolution.Width / BlockSize;
            int BlockHeight = Texture.Resolution.Height / BlockSize;

            for (int j = 0; j < BlockHeight; j++)
            {
                for (int i = 0; i < BlockWidth; i++)
                {
                    int BlockOffset = j * BlockWidth + i;

                    int X = XGAddress2DTiledX(BlockOffset, BlockWidth, TexelPitch);
                    int Y = XGAddress2DTiledY(BlockOffset, BlockWidth, TexelPitch);

                    int SrcOffset = j * BlockWidth * TexelPitch + i * TexelPitch;
                    int DstOffset = Y * BlockWidth * TexelPitch + X * TexelPitch;

                    if (ToLinear)
                        Buffer.BlockCopy(Data, DstOffset, Output, SrcOffset, TexelPitch);
                    else
                        Buffer.BlockCopy(Data, SrcOffset, Output, DstOffset, TexelPitch);
                }
            }

            return Output;
        }

        private static int XGAddress2DTiledX(int Offset, int Width, int TexelPitch)
        {
            int AlignedWidth = (Width + 31) & ~31;

            int LogBpp = (TexelPitch >> 2) + ((TexelPitch >> 1) >> (TexelPitch >> 2));
            int OffsetB = Offset << LogBpp;
            int OffsetT = ((OffsetB & ~4095) >> 3) + ((OffsetB & 1792) >> 2) + (OffsetB & 63);
            int OffsetM = OffsetT >> (7 + LogBpp);

            int MacroX = ((OffsetM % (AlignedWidth >> 5)) << 2);
            int Tile = ((((OffsetT >> (5 + LogBpp)) & 2) + (OffsetB >> 6)) & 3);
            int Macro = (MacroX + Tile) << 3;
            int Micro = ((((OffsetT >> 1) & ~15) + (OffsetT & 15)) & ((TexelPitch << 3) - 1)) >> LogBpp;

            return Macro + Micro;
        }

        private static int XGAddress2DTiledY(int Offset, int Width, int TexelPitch)
        {
            int AlignedWidth = (Width + 31) & ~31;

            int LogBpp = (TexelPitch >> 2) + ((TexelPitch >> 1) >> (TexelPitch >> 2));
            int OffsetB = Offset << LogBpp;
            int OffsetT = ((OffsetB & ~4095) >> 3) + ((OffsetB & 1792) >> 2) + (OffsetB & 63);
            int OffsetM = OffsetT >> (7 + LogBpp);

            int MacroY = ((OffsetM / (AlignedWidth >> 5)) << 2);
            int Tile = ((OffsetT >> (6 + LogBpp)) & 1) + (((OffsetB & 2048) >> 10));
            int Macro = (MacroY + Tile) << 3;
            int Micro = ((((OffsetT & (((TexelPitch << 6) - 1) & ~31)) + ((OffsetT & 15) << 1)) >> (3 + LogBpp)) & ~1);

            return Macro + Micro + ((OffsetT & 16) >> 4);
        }
        #endregion
    }
}
