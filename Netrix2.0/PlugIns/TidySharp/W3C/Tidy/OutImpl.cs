/*
* @(#)OutImpl.java   1.11 2000/08/16
*
*/

using System;
namespace Comzept.Genesis.Tidy
{
	/// <summary> 
/// Output Stream Implementation
/// 
/// (c) 1998-2000 (W3C) MIT, INRIA, Keio University
/// See Tidy.java for the copyright notice.
/// Derived from <a href="http://www.w3.org/People/Raggett/tidy">
/// HTML Tidy Release 4 Aug 2000</a>
/// 
/// </summary>
/// <author>   Dave Raggett dsr@w3.org
/// </author>
/// <author>   Andy Quick ac.quick@sympatico.ca (translation to Java)
/// </author>
/// <version>  1.0, 1999/05/22
/// </version>
/// <version>  1.0.1, 1999/05/29
/// </version>
/// <version>  1.1, 1999/06/18 Java Bean
/// </version>
/// <version>  1.2, 1999/07/10 Tidy Release 7 Jul 1999
/// </version>
/// <version>  1.3, 1999/07/30 Tidy Release 26 Jul 1999
/// </version>
/// <version>  1.4, 1999/09/04 DOM support
/// </version>
/// <version>  1.5, 1999/10/23 Tidy Release 27 Sep 1999
/// </version>
/// <version>  1.6, 1999/11/01 Tidy Release 22 Oct 1999
/// </version>
/// <version>  1.7, 1999/12/06 Tidy Release 30 Nov 1999
/// </version>
/// <version>  1.8, 2000/01/22 Tidy Release 13 Jan 2000
/// </version>
/// <version>  1.9, 2000/06/03 Tidy Release 30 Apr 2000
/// </version>
/// <version>  1.10, 2000/07/22 Tidy Release 8 Jul 2000
/// </version>
/// <version>  1.11, 2000/08/16 Tidy Release 4 Aug 2000
/// </version>
	public class OutImpl:Out
	{
		
		public OutImpl()
		{
			this.out_Renamed = null;
		}
		
		public override void  outc(sbyte c)
		{
			outc(((int) c) & 0xFF); // Convert to unsigned.
		}
		
		/* For mac users, should we map Unicode back to MacRoman? */
		public override void  outc(int c)
		{
			int ch;
			
			try
			{
				if (this.encoding == Configuration.UTF8)
				{
					if (c < 128)
						this.out_Renamed.WriteByte((System.Byte) c);
					else if (c <= 0x7FF)
					{
						ch = (0xC0 | (c >> 6)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | (c & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
					}
					else if (c <= 0xFFFF)
					{
						ch = (0xE0 | (c >> 12)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | ((c >> 6) & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | (c & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
					}
					else if (c <= 0x1FFFFF)
					{
						ch = (0xF0 | (c >> 18)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | ((c >> 12) & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | ((c >> 6) & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | (c & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
					}
					else
					{
						ch = (0xF8 | (c >> 24)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | ((c >> 18) & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | ((c >> 12) & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | ((c >> 6) & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
						ch = (0x80 | (c & 0x3F)); this.out_Renamed.WriteByte((System.Byte) ch);
					}
				}
				else if (this.encoding == Configuration.ISO2022)
				{
					if (c == 0x1b)
					/* ESC */
						this.state = StreamIn.FSM_ESC;
					else
					{
						switch (this.state)
						{
							
							case StreamIn.FSM_ESC: 
								if (c == '$')
									this.state = StreamIn.FSM_ESCD;
								else if (c == '(')
									this.state = StreamIn.FSM_ESCP;
								else
									this.state = StreamIn.FSM_ASCII;
								break;
							
							
							case StreamIn.FSM_ESCD: 
								if (c == '(')
									this.state = StreamIn.FSM_ESCDP;
								else
									this.state = StreamIn.FSM_NONASCII;
								break;
							
							
							case StreamIn.FSM_ESCDP: 
								this.state = StreamIn.FSM_NONASCII;
								break;
							
							
							case StreamIn.FSM_ESCP: 
								this.state = StreamIn.FSM_ASCII;
								break;
							
							
							case StreamIn.FSM_NONASCII: 
								c &= 0x7F;
								break;
							}
					}
					
					this.out_Renamed.WriteByte((System.Byte) c);
				}
				else
					this.out_Renamed.WriteByte((System.Byte) c);
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Error.WriteLine("OutImpl.outc: " + e.ToString());
			}
		}
		
		public override void  newline()
		{
			try
			{
				sbyte[] temp_sbyteArray;
				temp_sbyteArray = nlBytes;
				this.out_Renamed.Write(SupportClass.ToByteArray(temp_sbyteArray), 0, temp_sbyteArray.Length);
				this.out_Renamed.Flush();
			}
			catch (System.IO.IOException e)
			{
				//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.toString' may return a different value. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1043'"
				System.Console.Error.WriteLine("OutImpl.newline: " + e.ToString());
			}
		}
		
		//UPGRADE_NOTE: Final was removed from the declaration of 'nlBytes '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly sbyte[] nlBytes = SupportClass.ToSByteArray(SupportClass.ToByteArray((System.Environment.NewLine)));
	}
	
}