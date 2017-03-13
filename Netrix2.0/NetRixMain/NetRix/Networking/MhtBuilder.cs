using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace GuruComponents.Netrix.Networking
{
	/// <summary>
	/// This class provides support for creation of MHTML format files.
	/// </summary>
    /// <remarks>
    /// MHTML stands for MIME HTML. It is a standard for including resources that in usual HTTP pages are linked externally, 
    /// such as images and sound files, in the same file as the HTML code. The included data files are encoded using MIME. 
    /// This format is sometimes referred to as MHT, after the suffix .mht given to such files by default when created by 
    /// Microsoft Word, Internet Explorer or Opera.
    /// <para>
    /// The main idea of the MHTML standard is that you send a HTML document, together with in-line graphics, applets, etc., and also other linked documents if you so wish, in a MIME multipart/related body part. Links in the HTML to other included parts can be provided by CID (Content-ID) URLs or by any other kind of URI, and the linked body part is identified in its heading by either a Content-ID (linked to by CID URLs) or a Content-Location (linked to by any other kind of URL). (In fact, the "Content-ID: foo@bar header" can be seen as a special case of the "Content-Location: CID: foo@bar header".) 
    /// </para>
    /// <para>The Content-Location identifies a URI for a content part, the part need not be universally retrievable using this base. 
    /// </para>
    /// <para>The Content-Base identifies a base URI for a content, or for all objects within it which do not have their own Content-Base. 
    /// </para>
    /// <para>
    /// URIs in HTML-formatted messages and in Content-Location headers can be absolute and relative. If they are relative, and a base can be found, they are to be converted to absolute URIs before matching with other body parts. If no base can be found, then exact matching of the relative URIs in the HTML and the Content-Location of the linked parts is performed instead. The base can be found in a surrounding absolute Content-Location header.
    /// </para>
    /// <b>Note: </b> Before NetRix v1.6 the editor does not provide support for loading MHT documents.
    /// </remarks>
	public class MhtBuilder
	{

        private const string BOUNDARYDECL = "----=_NextPart_000_0000_01C40921.4AB4F340";
        private const string BOUNDARY = "------=_NextPart_000_0000_01C40921.4AB4F340";
        private StringBuilder Mht;

        /// <summary>
        /// Creates an instance and the object is responsible for one MHT document.
        /// </summary>
        /// <param name="szUrl">URI of request which is packed as MHT.</param>
		public MhtBuilder(string szUrl)
		{
            Mht = new StringBuilder();
            Mht.Append("From: <NetRix Saved>" + Environment.NewLine);
            Mht.AppendFormat("Subject: {0}" + Environment.NewLine, szUrl);
            Mht.AppendFormat("Date: {0}" + Environment.NewLine, DateTime.Now.ToLongDateString());
            Mht.Append("MIME-Version: 1.0" + Environment.NewLine);
            Mht.Append("Content-Type: multipart/related;" + Environment.NewLine);
            Mht.Append("\tboundary=\"" + BOUNDARYDECL + "\"" + ";" + Environment.NewLine);
            Mht.Append("\ttype=\"text/html\"" + Environment.NewLine);
            Mht.Append("X-MimeOLE: Produced By Microsoft MimeOLE V6.00.2800.1106" + Environment.NewLine);
            Mht.Append(Environment.NewLine);
            Mht.Append("This is a multi-part message in MIME format." + Environment.NewLine);
            Mht.Append(Environment.NewLine);
            Mht.Append(BOUNDARY + Environment.NewLine);
            Mht.Append("Content-Type: text/html;" + Environment.NewLine);
            Mht.AppendFormat("\tcharset=\"{0}\"" + Environment.NewLine, "windows-1252");
            Mht.Append("Content-Transfer-Encoding: quoted-printable" + Environment.NewLine);
            Mht.AppendFormat("Content-Location: {0}" + Environment.NewLine, Path.GetFileName(szUrl));
            Mht.Append(Environment.NewLine);
		}

        /// <summary>
        /// <see cref="StringBuilder"/> which contains the MHT document data.
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetStringBuilder()
        {
            return this.Mht;
        }

        /// <summary>
        /// Add a quoted string to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="content">The content being added.</param>
        public void AddQuotedString(string content)
        {
            AddQuotedString(content, false);
        }

        /// <summary>
        /// Add a quoted string to the <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="content">The content being added.</param>
        /// <param name="addNewLine">Whether or not a new line is being added after content.</param>
        public void AddQuotedString(string content, bool addNewLine)
        {
            Mht.Append(QuotedPrintable.Encode(content));
            if (addNewLine)
            {
                AppendNewLine();
            }
        }

        /// <summary>
        /// Adds a newline.
        /// </summary>
        public void AppendNewLine()
        {
            Mht.Append(Environment.NewLine);
        }

        /// <summary>
        /// Appends text without any quotation.
        /// </summary>
        /// <param name="text">The text being added.</param>
        public void AppendText(string text)
        {
            Mht.Append(text);
        }

        /// <summary>
        /// Appends text without any quotation and a newline.
        /// </summary>
        /// <param name="text">The text being added.</param>
        /// <param name="addNewLine">Whether or not a new line is being added after text.</param>
        public void AppendText(string text, bool addNewLine)
        {
            this.AppendText(text);
            if (addNewLine)
            {
                AppendNewLine();
            }

        }

        /// <summary>
        /// Append boundary information.
        /// </summary>
        public void AppendBoundary()
        {
            Mht.Append(BOUNDARY + Environment.NewLine);
        }

        /// <summary>
        /// Append binary content Base64 encoded.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="chunk">The chunk length in bytes.</param>
        public void AppendChunkBase64(byte[] data, int chunk)
        {
            Mht.Append(ChunkBase64(data, chunk));
        }

        private string ChunkBase64(byte[] data, int chunkLen)
        {
            string baseString = Convert.ToBase64String(data);
            StringBuilder sb = new StringBuilder(data.Length);
            for (int i = 0; i < baseString.Length; i++)
            {
                sb.Append(baseString[i]);
                if ((i + 1) % chunkLen == 0)
                {
                    sb.Append(Environment.NewLine);
                }
            }
            return sb.ToString();
        }

        # region MHTML

        /// <summary>
        /// <para>
        /// Robust and fast implementation of Quoted Printable
        /// Multipart Internet Mail Encoding (MIME) which encodes every 
        /// character, not just "special characters" for transmission over 
        /// SMTP.
        /// </para>
        /// <para>
        /// More information on the quoted-printable encoding can be found
        /// here: http://www.freesoft.org/CIE/RFC/1521/6.htm
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// detailed in: RFC 1521
        /// </para>
        /// <para>
        /// more info: http://www.freesoft.org/CIE/RFC/1521/6.htm
        /// </para>
        /// <para>
        /// The QuotedPrintable class encodes and decodes strings and files
        /// that either were encoded or need encoded in the Quoted-Printable
        /// MIME encoding for Internet mail. The encoding methods of the class
        /// use pointers wherever possible to guarantee the fastest possible 
        /// encoding times for any size file or string. The decoding methods 
        /// use only the .NET framework classes.
        /// </para>
        /// <para>
        /// The Quoted-Printable implementation
        /// is robust which means it encodes every character to ensure that the
        /// information is decoded properly regardless of machine or underlying
        /// operating system or protocol implementation. The decode can recognize
        /// robust encodings as well as minimal encodings that only encode special
        /// characters and any implementation in between. Internally, the
        /// class uses a regular expression replace pattern to decode a quoted-
        /// printable string or file.
        /// </para>
        /// </remarks>
        /// <example>
        /// This example shows how to quoted-printable encode an html file and then
        /// decode it.
        /// <code>
        /// string encoded = QuotedPrintable.EncodeFile(
        /// 	@"C:\WEBS\wwwroot\index.html"
        /// 	);
        /// 
        /// string decoded = QuotedPrintable.Decode(encoded);
        /// 
        /// Console.WriteLine(decoded);
        /// </code>
        /// </example>
        public class QuotedPrintable
        {
            private QuotedPrintable()
            {
            }

            /// <summary>
            /// Gets the maximum number of characters per quoted-printable
            /// line as defined in the RFC minus 1 to allow for the =
            /// character (soft line break).
            /// </summary>
            /// <remarks>
            /// (Soft Line Breaks): The Quoted-Printable encoding REQUIRES 
            /// that encoded lines be no more than 76 characters long. If 
            /// longer lines are to be encoded with the Quoted-Printable 
            /// encoding, 'soft' line breaks must be used. An equal sign 
            /// as the last character on a encoded line indicates such a 
            /// non-significant ('soft') line break in the encoded text.
            /// </remarks>
            public const int RFC_1521_MAX_CHARS_PER_LINE = 75;

            /// <summary>
            /// Encodes a very large string into the Quoted-Printable
            /// encoding for transmission via SMTP
            /// </summary>
            /// <param name="toencode">
            /// the very large string to encode
            /// </param>
            /// <returns>The Quoted-Printable encoded string</returns>
            /// <exception cref="ObjectDisposedException">
            /// A problem occurred while attempting to read the encoded 
            /// string.
            /// </exception>
            /// <exception cref="OutOfMemoryException">
            /// There is insufficient memory to allocate a buffer for the
            /// returned string. 
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// A string is passed in as a null reference.
            /// </exception>
            /// <exception cref="IOException">
            /// An I/O error occurs, such as the stream being closed.
            /// </exception>  
            /// <exception cref="ArgumentOutOfRangeException">
            /// The charsperline argument is less than or equal to 0.
            /// </exception>
            /// <remarks>
            /// This method encodes a large string into the quoted-printable
            /// encoding and then properly formats it into lines of 76 characters
            /// using the <see cref="FormatEncodedString"/> method.
            /// </remarks>
            public static string Encode(string toencode)
            {
                return Encode(toencode, RFC_1521_MAX_CHARS_PER_LINE);
            }

            /// <summary>
            /// Encodes a very large string into the Quoted-Printable
            /// encoding for transmission via SMTP
            /// </summary>
            /// <param name="toencode">
            /// the very large string to encode
            /// </param>
            /// <param name="charsperline">
            /// the number of chars per line after encoding
            /// </param>
            /// <returns>The Quoted-Printable encoded string</returns>
            /// <exception cref="ObjectDisposedException">
            /// A problem occurred while attempting to read the encoded 
            /// string.
            /// </exception>
            /// <exception cref="OutOfMemoryException">
            /// There is insufficient memory to allocate a buffer for the
            /// returned string. 
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// A string is passed in as a null reference.
            /// </exception>
            /// <exception cref="IOException">
            /// An I/O error occurs, such as the stream being closed.
            /// </exception>  
            /// <exception cref="ArgumentOutOfRangeException">
            /// The charsperline argument is less than or equal to 0.
            /// </exception>
            /// <remarks>
            /// This method encodes a large string into the quoted-printable
            /// encoding and then properly formats it into lines of 
            /// charsperline characters using the <see cref="FormatEncodedString"/> 
            /// method.
            /// </remarks>
            public static string Encode(string toencode, int charsperline)
            {
                if (toencode == null)
                    throw new ArgumentNullException();

                if (charsperline <= 0)
                    throw new ArgumentOutOfRangeException();

                string line, encodedHtml = "";
                StringReader sr = new StringReader(toencode);
                try
                {
                    while((line=sr.ReadLine())!=null)
                        encodedHtml += EncodeSmallLine(line);

                    return FormatEncodedString(encodedHtml, charsperline);
                }
                finally
                {
                    sr.Close();
                    sr = null;
                }
            }

            /// <summary>
            /// Encodes a file's contents into a string using
            /// the Quoted-Printable encoding.
            /// </summary>
            /// <param name="filepath">
            /// The path to the file to encode.
            /// </param>
            /// <returns>The Quoted-Printable encoded string</returns>
            /// <exception cref="System.ObjectDisposedException">
            /// A problem occurred while attempting to encode the 
            /// string.
            /// </exception>
            /// <exception cref="System.OutOfMemoryException">
            /// There is insufficient memory to allocate a buffer for the
            /// returned string. 
            /// </exception>
            /// <exception cref="System.ArgumentNullException">
            /// A string is passed in as a null reference.
            /// </exception>
            /// <exception cref="System.IO.IOException">
            /// An I/O error occurs, such as the stream being closed.
            /// </exception>  
            /// <exception cref="System.IO.FileNotFoundException">
            /// The file was not found.
            /// </exception>
            /// <exception cref="System.Security.SecurityException">
            /// The caller does not have the required permission to open
            /// the file specified in filepath.
            /// </exception>
            /// <exception cref="UnauthorizedAccessException">
            /// filepath is read-only or a directory.
            /// </exception>
            /// <remarks>
            /// This method encodes a file's text into the quoted-printable
            /// encoding and then properly formats it into lines of 76 characters
            /// using the <see cref="FormatEncodedString"/> method.
            /// </remarks>
            public static string EncodeFile(string filepath)
            {
                
                return EncodeFile(filepath, RFC_1521_MAX_CHARS_PER_LINE);
            }

            /// <summary>
            /// Encodes a file's contents into a string using
            /// the Quoted-Printable encoding.
            /// </summary>
            /// <param name="filepath">
            /// The path to the file to encode.
            /// </param>
            /// <param name="charsperline">
            /// the number of chars per line after encoding
            /// </param>
            /// <returns>The Quoted-Printable encoded string</returns>
            /// <exception cref="System.ObjectDisposedException">
            /// A problem occurred while attempting to encode the 
            /// string.
            /// </exception>
            /// <exception cref="System.OutOfMemoryException">
            /// There is insufficient memory to allocate a buffer for the
            /// returned string. 
            /// </exception>
            /// <exception cref="System.ArgumentNullException">
            /// A string is passed in as a null reference.
            /// </exception>
            /// <exception cref="System.IO.IOException">
            /// An I/O error occurs, such as the stream being closed.
            /// </exception>  
            /// <exception cref="System.IO.FileNotFoundException">
            /// The file was not found.
            /// </exception>
            /// <exception cref="System.Security.SecurityException">
            /// The caller does not have the required permission to open
            /// the file specified in filepath.
            /// </exception>
            /// <exception cref="System.UnauthorizedAccessException">
            /// filepath is read-only or a directory.
            /// </exception>
            /// <remarks>
            /// This method encodes a file's text into the quoted-printable
            /// encoding and then properly formats it into lines of 
            /// charsperline characters using the <see cref="FormatEncodedString"/> 
            /// method.
            /// </remarks>
            public static string EncodeFile(string filepath, int charsperline)
            {
                if (filepath == null)
                    throw new ArgumentNullException();

                string encodedHtml = "", line;
                FileInfo f = new FileInfo(filepath);
			
                if (! f.Exists)
                    throw new FileNotFoundException();

                StreamReader sr = f.OpenText();
                try
                {
                    while((line=sr.ReadLine())!=null)
                        encodedHtml += EncodeSmallLine(line);

                    return FormatEncodedString(encodedHtml, charsperline);
                }
                finally
                {
                    sr.Close();
                    sr = null;
                    f = null;
                }
            }

            /// <summary>
            /// Encodes a small string into the Quoted-Printable encoding
            /// for transmission via SMTP. The string is not split
            /// into lines of X characters like the string that the 
            /// Encode method returns.
            /// </summary>
            /// <param name="s">
            /// The string to encode.
            /// </param>
            /// <returns>The Quoted-Printable encoded string</returns>
            /// <exception cref="ArgumentNullException">
            /// A string is passed in as a null reference.
            /// </exception>
            /// <remarks>
            /// This method encodes a small string into the quoted-printable
            /// encoding. The resultant encoded string has NOT been separated
            /// into lined results using the <see cref="FormatEncodedString"/>
            /// method.
            /// </remarks>
            public static string EncodeSmall(string s)
            {
                if (s == null)
                    throw new ArgumentNullException();
                string result = "";
                char[] pChar = s.ToCharArray();
                int pCurrent = 0;
                do
                {
                    int code = (int) pChar[pCurrent];
                    result += String.Format("={0}", code.ToString("X2"));
                    pCurrent++;
                }
                while (pCurrent < pChar.Length);
                return result;
            }

            /// <summary>
            /// Encodes a small string with an appended newline into the 
            /// Quoted-Printable encoding for transmission via SMTP. The 
            /// string is not split into lines of X characters like the 
            /// string that the Encode or the EncodeFile methods return.
            /// </summary>
            /// <param name="s">
            /// The string to encode.
            /// </param>
            /// <returns>The Quoted-Printable encoded string</returns>
            /// <exception cref="ArgumentNullException">
            /// A string is passed in as a null reference.
            /// </exception>
            /// <remarks>
            /// This method encodes a small string into the quoted-printable
            /// encoding. The resultant encoded string has NOT been separated
            /// into lined results using the <see cref="FormatEncodedString"/>
            /// method.
            /// </remarks>
            public static string EncodeSmallLine(string s)
            {
                if (s == null)
                    throw new ArgumentNullException();

                return EncodeSmall(s + "\r\n");
            }

            /// <summary>
            /// Formats a quoted-printable string into lines equal to maxcharlen,
            /// following all protocol rules such as byte stuffing. This method is
            /// called automatically by the Encode method and the EncodeFile method.
            /// </summary>
            /// <param name="qpstr">
            /// the quoted-printable encoded string.
            /// </param>
            /// <param name="maxcharlen">
            /// the number of chars per line after encoding.
            /// </param>
            /// <returns>
            /// The properly formatted Quoted-Printable encoded string in lines of
            /// 76 characters as defined by the RFC.</returns>
            /// <exception cref="ArgumentNullException">
            /// A string is passed in as a null reference.
            /// </exception>
            /// <exception cref="IOException">
            /// An I/O error occurs, such as the stream being closed.
            /// </exception>  
            /// <remarks>
            /// Formats a quoted-printable encoded string into lines of
            /// maxcharlen characters for transmission via SMTP.
            /// </remarks>
            public static string FormatEncodedString(string qpstr, int maxcharlen)
            {
                if (qpstr == null)
                    throw new ArgumentNullException();

                string strout = "";
                StringWriter qpsw = new StringWriter();
                try
                {
                    char[] pChr = qpstr.ToCharArray();
                    int pCurrent = 0;
                    int i = 0;
                    do
                    {
                        strout += pChr[pCurrent].ToString();
                        i++;
                        if (i == maxcharlen)
                        {
                            qpsw.WriteLine("{0}=", strout);
                            qpsw.Flush();
                            i=0;
                            strout = "";
                        }
                        pCurrent++;
                    }
                    while(pCurrent < pChr.Length);
                    qpsw.WriteLine(strout);
                    qpsw.Flush();

                    return qpsw.ToString();
                }
                finally
                {
                    qpsw.Close();
                    qpsw = null;
                }
            }

            static string HexDecoderEvaluator(Match m)
            {
                string hex = m.Groups[2].Value;
                int iHex = Convert.ToInt32(hex, 16);
                char c = (char) iHex;
                return c.ToString();
            }

            static string HexDecoder(string line)
            {
                if (line == null)
                    throw new ArgumentNullException();

                //parse looking for =XX where XX is hexadecimal
                Regex re = new Regex(
                    "(\\=([0-9A-F][0-9A-F]))", 
                    RegexOptions.IgnoreCase
                    );
                return re.Replace(line, new MatchEvaluator(HexDecoderEvaluator));
            }

            /// <summary>
            /// decodes an entire file's contents into plain text that 
            /// was encoded with quoted-printable.
            /// </summary>
            /// <param name="filepath">
            /// The path to the quoted-printable encoded file to decode.
            /// </param>
            /// <returns>The decoded string.</returns>
            /// <exception cref="System.ObjectDisposedException">
            /// A problem occurred while attempting to decode the 
            /// encoded string.
            /// </exception>
            /// <exception cref="System.OutOfMemoryException">
            /// There is insufficient memory to allocate a buffer for the
            /// returned string. 
            /// </exception>
            /// <exception cref="System.ArgumentNullException">
            /// A string is passed in as a null reference.
            /// </exception>
            /// <exception cref="System.IO.IOException">
            /// An I/O error occurs, such as the stream being closed.
            /// </exception>  
            /// <exception cref="FileNotFoundException">
            /// The file was not found.
            /// </exception>
            /// <exception cref="System.Security.SecurityException">
            /// The caller does not have the required permission to open
            /// the file specified in filepath.
            /// </exception>
            /// <exception cref="System.UnauthorizedAccessException">
            /// filepath is read-only or a directory.
            /// </exception>
            /// <remarks>
            /// Decodes a quoted-printable encoded file into a string
            /// of unencoded text of any size.
            /// </remarks>
            public static string DecodeFile(string filepath)
            {
                if (filepath == null)
                    throw new ArgumentNullException();

                string decodedHtml = "", line;
                FileInfo f = new FileInfo(filepath);

                if (! f.Exists)
                    throw new FileNotFoundException();

                StreamReader sr = f.OpenText();
                try
                {
                    while((line=sr.ReadLine())!=null)
                        decodedHtml += Decode(line);

                    return decodedHtml;
                }
                finally
                {
                    sr.Close();
                    sr = null;
                    f = null;
                }
            }

            /// <summary>
            /// Decodes a Quoted-Printable string of any size into 
            /// it's original text.
            /// </summary>
            /// <param name="encoded">
            /// The encoded string to decode.
            /// </param>
            /// <returns>The decoded string.</returns>
            /// <exception cref="ArgumentNullException">
            /// A string is passed in as a null reference.
            /// </exception>
            /// <remarks>
            /// Decodes a quoted-printable encoded string into a string
            /// of unencoded text of any size.
            /// </remarks>
            public static string Decode(string encoded)
            {
                if (encoded == null)
                    throw new ArgumentNullException();

                string line;
                StringWriter sw = new StringWriter();
                StringReader sr = new StringReader(encoded);
                try
                {
                    while((line=sr.ReadLine())!=null)
                    {
                        if (line.EndsWith("="))
                            sw.Write(HexDecoder(line.Substring(0, line.Length-1)));
                        else
                            sw.WriteLine(HexDecoder(line));

                        sw.Flush();
                    }
                    return sw.ToString();
                }
                finally
                {
                    sw.Close();
                    sr.Close();
                    sw = null;
                    sr = null;
                }
            }
        }


        # endregion


	}
}
