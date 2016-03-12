using System.IO;
using System.Text;

namespace IDDTool.IO
{
    /// <summary>
    ///     Handy methods for reading and/or writting Strings inside Streams.
    /// </summary>
    class StringUtilities
    {
        /// <summary>
        ///     Reads a ASCII String, with a given length, from a Stream.
        /// </summary>
        /// <param name="Data">The Stream where the String is contained</param>
        /// <param name="Length">The number of bytes to read from the Stream</param>
        /// <returns>The String at the current position of the Stream</returns>
        public static string ReadASCIIString(Stream Data, int Length)
        {
            StringBuilder Output = new StringBuilder();
            while (Length-- > 0) Output.Append((char)Data.ReadByte());
            return Output.ToString();
        }

        /// <summary>
        ///     Reads a null-terminated ASCII String from a Stream.
        /// </summary>
        /// <param name="Data">The Stream where the String is contained</param>
        /// <returns>The String at the current position of the Stream</returns>
        public static string ReadASCIIString(Stream Data)
        {
            int Value = 0;
            StringBuilder Output = new StringBuilder();
            while ((Value = Data.ReadByte()) != 0) Output.Append((char)Value);
            return Output.ToString();
        }
    }
}
