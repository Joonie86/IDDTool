using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace IDDTool.IO.Format
{
    public class DDSContent
    {
        public TextureFormat Format;
        public Size Resolution;
        public byte[] TextureData;
    }

    /// <summary>
    ///     Handles (DXT compressed only!) DDS files.
    /// </summary>
    class DDS
    {
        /// <summary>
        ///     Imports a DDS texture from a file.
        /// </summary>
        /// <param name="InFile">The full path to the file</param>
        /// <returns>The DDS texture content</returns>
        public static DDSContent Import(string InFile)
        {
            DDSContent Output = new DDSContent();

            using (FileStream Input = new FileStream(InFile, FileMode.Open))
            {
                BinaryReader Reader = new BinaryReader(Input);

                Input.Seek(0xc, SeekOrigin.Begin);
                Output.Resolution = new Size(Reader.ReadInt32(), Reader.ReadInt32());
                Input.Seek(0x54, SeekOrigin.Begin);
                string FourCC = StringUtilities.ReadASCIIString(Input, 4);
                switch (FourCC)
                {
                    case "DXT1": Output.Format = TextureFormat.DXT1; break;
                    case "DXT3": Output.Format = TextureFormat.DXT3; break;
                    case "DXT5": Output.Format = TextureFormat.DXT5; break;
                    default: throw new Exception("Unsupported DDS format! Only DXT compression is supported!");
                }

                byte[] Buffer = new byte[Input.Length - 0x80];
                Input.Seek(0x80, SeekOrigin.Begin);
                Input.Read(Buffer, 0, Buffer.Length);
                Output.TextureData = Buffer;
            }

            return Output;
        }

        /// <summary>
        ///     Exports a IDD texture to a DDS file.
        /// </summary>
        /// <param name="Texture">The texture to be exported</param>
        /// <param name="OutFile">Output file full path with name and extension</param>
        public static void Export(IDDTexture Texture, string OutFile)
        {
            using (FileStream Output = new FileStream(OutFile, FileMode.Create))
            {
                BinaryWriter Writer = new BinaryWriter(Output);

                Writer.Write(0x20534444); //DDS Signature
                Writer.Write(0x7cu); //Header size (without the signature)
                Writer.Write(0x00021007u); //DDS Flags
                Writer.Write(Texture.Resolution.Height);
                Writer.Write(Texture.Resolution.Width);
                Writer.Write(0u); //Stride
                Writer.Write(0u); //BPP
                Writer.Write(1u); //Mipmaps
                Output.Seek(0x2c, SeekOrigin.Current); //Reserved space for future use
                Writer.Write((uint)0x20); //PixelFormat structure size (32 bytes)
                Writer.Write(4u); //Pixel flags (4 = DXT compressed)

                switch (Texture.Format)
                {
                    case TextureFormat.DXT1: Writer.Write(Encoding.ASCII.GetBytes("DXT1")); break;
                    case TextureFormat.DXT3: Writer.Write(Encoding.ASCII.GetBytes("DXT3")); break;
                    case TextureFormat.DXT5: Writer.Write(Encoding.ASCII.GetBytes("DXT5")); break;
                    default: throw new Exception("Unsupported texture format!");
                }

                Output.Seek(20, SeekOrigin.Current);
                Writer.Write(0x400000u); //Caps 1
                Writer.Write(0u); //Caps 2
                Writer.Write(0u); //Unused stuff
                Writer.Write(0u);
                Writer.Write(0u);
                Writer.Write(Texture.TextureData);
            }
        }
    }
}
