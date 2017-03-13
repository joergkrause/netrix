using System;
using System.Collections;
using sys = System;

namespace Comzept.Library.Drawing.ImageInfo
{
	///<summary>This Class retrives Image Properties using Image.GetPropertyItem() method 
	/// and gives access to some of them trough its public properties. Or to all of them
	/// trough its public property PropertyItems.
	///</summary>
	public class Info
	{
		///<summary>Wenn using this constructor the Image property must be set before accessing properties.</summary>
		public Info()
		{
		}
		
		///<summary>Creates Info Class to read properties of an Image given from a file.</summary>
		/// <param name="imageFileName">A string specifiing image file name on a file system.</param>
		public Info(string imageFileName)
		{
			_image = sys.Drawing.Image.FromFile(imageFileName);
		}
		
		///<summary>Creates Info Class to read properties of a given Image object.</summary>
		/// <param name="anImage">An Image object to analise.</param>
		public Info(sys.Drawing.Image anImage)
		{
			_image = anImage;
		}

		sys.Drawing.Image _image;
		///<summary>Sets or returns the current Image object.</summary>
		public sys.Drawing.Image Image 
		{
			set{_image = value;}
			get{return _image;}
		}
		
		///<summary>
		/// Type is PropertyTagTypeShort or PropertyTagTypeLong
		///Information specific to compressed data. When a compressed file is recorded, the valid width of the meaningful image must be recorded in this tag, whether or not there is padding data or a restart marker. This tag should not exist in an uncompressed file.
		/// </summary>
		public uint PixXDim
		{
			get
			{
				object tmpValue = PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifPixXDim));
				if (tmpValue.GetType().ToString().Equals("System.UInt16")) return (uint)(ushort)tmpValue; 
				return (uint)tmpValue; 			
			}
		}
		///<summary>
		/// Type is PropertyTagTypeShort or PropertyTagTypeLong
		/// Information specific to compressed data. When a compressed file is recorded, the valid height of the meaningful image must be recorded in this tag whether or not there is padding data or a restart marker. This tag should not exist in an uncompressed file. Because data padding is unnecessary in the vertical direction, the number of lines recorded in this valid image height tag will be the same as that recorded in the SOF.
		/// </summary>
		public uint PixYDim
		{
			get
			{
				object tmpValue = PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifPixYDim));
				if (tmpValue.GetType().ToString().Equals("System.UInt16")) return (uint)(ushort)tmpValue; 
				return (uint)tmpValue; 			
			}
		}
		
		///<summary>
		///Number of pixels per unit in the image width (x) direction. The unit is specified by PropertyTagResolutionUnit
		///</summary>
		public Fraction XResolution
		{
			get
			{
				return (Fraction) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.XResolution));			
			}
		}
		
		///<summary>
		///Number of pixels per unit in the image height (y) direction. The unit is specified by PropertyTagResolutionUnit.
		///</summary>
		public Fraction YResolution
		{
			get 
			{ 
				return (Fraction) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.YResolution ));
			}
		}
		
		///<summary>
		///Unit of measure for the horizontal resolution and the vertical resolution.
		///2 - inch 3 - centimeter
		///</summary>
		public ResolutionUnit ResolutionUnit
		{
			get
			{
				return (ResolutionUnit) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ResolutionUnit ));			
			}
		}
		
		///<summary>
		///Brightness value. The unit is the APEX value. Ordinarily it is given in the range of -99.99 to 99.99.
		///</summary>
		public Fraction Brightness
		{
			get
			{
				return (Fraction) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifBrightness));			
			}
		}
		
		///<summary>
		/// The manufacturer of the equipment used to record the image.
		///</summary>
		public string EquipMake
		{
			get
			{
				return (string) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.EquipMake));			
			}
		}
		
		///<summary>
		/// The model name or model number of the equipment used to record the image.
		/// </summary>
		public string EquipModel
		{
			get
			{
				return (string) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.EquipModel));			
			}
		}
		
		///<summary>
		///Copyright information.
		///</summary>
		public string Copyright
		{
			get
			{
				return (string) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.Copyright));			
			}
		}
		

		///<summary>
		///Date and time the image was created.
		///</summary>		
		public string DateTime
		{
			get
			{
				return (string) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.DateTime));			
			}
		}
		
		//The format is YYYY:MM:DD HH:MM:SS with time shown in 24-hour format and the date and time separated by one blank character (0x2000). The character string length is 20 bytes including the NULL terminator. When the field is empty, it is treated as unknown.
		private static DateTime ExifDTToDateTime(string exifDT)
		{
			exifDT = exifDT.Replace(' ', ':');
			string[] ymdhms = exifDT.Split(':');
			int years 	= int.Parse(ymdhms[0]);
			int months	= int.Parse(ymdhms[1]);
			int days	= int.Parse(ymdhms[2]);
			int hours	= int.Parse(ymdhms[3]);
			int minutes	= int.Parse(ymdhms[4]);
			int seconds	= int.Parse(ymdhms[5]);
			return new DateTime(years, months, days, hours, minutes, seconds);
		}
		
		///<summary>
		///Date and time when the original image data was generated. For a DSC, the date and time when the picture was taken. 
		///</summary>
		public DateTime DTOrig
		{
			get 
			{
				string tmpStr = (string) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifDTOrig));			
				return ExifDTToDateTime(tmpStr);
			}
		}

		///<summary>
		///Date and time when the image was stored as digital data. If, for example, an image was captured by DSC and at the same time the file was recorded, then DateTimeOriginal and DateTimeDigitized will have the same contents.
		///</summary>
		public DateTime DTDigitized
		{
			get 
			{
				string tmpStr = (string) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifDTDigitized));			
				return ExifDTToDateTime(tmpStr);
			}
		}
		
		
		///<summary>
		///ISO speed and ISO latitude of the camera or input device as specified in ISO 12232.
		///</summary>		
		public ushort ISOSpeed
		{
			get
			{
				return (ushort) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifISOSpeed));			
			}
		}
		
		///<summary>
		///Image orientation viewed in terms of rows and columns.
		///</summary>
		public Orientation Orientation
		{
			get
			{
				return (Orientation) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.Orientation));			
			}
		}

		///<summary>
		///Actual focal length, in millimeters, of the lens. Conversion is not made to the focal length of a 35 millimeter film camera.
		///</summary>						
		public Fraction FocalLength
		{
			get
			{
				return (Fraction) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifFocalLength));			
			}
		}

		///<summary>
		///F number.
		///</summary>						
		public Fraction FNumber
		{
			get
			{
				return (Fraction) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifFNumber));			
			}
		}

		///<summary>
		///Class of the program used by the camera to set exposure when the picture is taken.
		///</summary>						
		public ExposureProg ExposureProg
		{
			get
			{
				return (ExposureProg) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifExposureProg));			
			}
		}

		///<summary>
		///Metering mode.
		///</summary>						
		public MeteringMode MeteringMode
		{
			get
			{
				return (MeteringMode) PropertyTag.getValue(_image.GetPropertyItem((int)PropertyTagId.ExifMeteringMode));			
			}
		}
		
		private Hashtable _propertyItems;
		///<summary>
		/// Returns a Hashtable of all available Properties of a gieven Image. Keys of this Hashtable are
		/// Display names of the Property Tags and values are transformed (typed) data.
		///</summary>
		/// <example>
		/// <code>
		/// if (openFileDialog.ShowDialog()==DialogResult.OK)
		///	{
		///		Info inf=new Info(Image.FromFile(openFileDialog.FileName));
		///		listView.Items.Clear();
		///		foreach (string propertyname in inf.PropertyItems.Keys)
		///		{
		///			ListViewItem item1 = new ListViewItem(propertyname,0);
		///		    item1.SubItems.Add((inf.PropertyItems[propertyname]).ToString());
		///			listView.Items.Add(item1);
		///		}
		///	}
		/// </code>
		///</example>
		public Hashtable PropertyItems
		{
			get 
			{
				if (_propertyItems==null)
				{
					_propertyItems= new Hashtable();
					foreach(int id in _image.PropertyIdList)
						_propertyItems[((PropertyTagId)id).ToString()]=PropertyTag.getValue(_image.GetPropertyItem(id));
						
				}
				return _propertyItems;
			}
		}
	}
}
