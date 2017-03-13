#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Runtime.CompilerServices;

using Comzept.Library.Drawing.Internal;

#endregion

namespace Comzept.Library.Drawing
{

	public class RoundSelectionBrush : Brush
	{

		#region Constructors

		/// <summary>
		/// Costruttore della classe RoundSelectionBrush
		/// </summary>
		/// <param name="rectangle">Oggetto Rectangle che rappresenta l'ingombro del brush</param>
		public RoundSelectionBrush(Rectangle rectangle)
			: this(rectangle, DEFAULT_GRADIENT_WIDTH, Color.Empty, Color.Empty, Color.Empty, DEFAULT_ROUNDED)
		{
		}
		/// <summary>
		/// Costruttore della classe RoundSelectionBrush
		/// </summary>
		/// <param name="rectangle">Oggetto Rectangle che rappresenta l'ingombro del brush</param>
		/// <param name="gradientWidth">Valore che indica la larghezza della sfumatura al bordo (Color1)</param>
		public RoundSelectionBrush(Rectangle rectangle, float gradientWidth)
			: this(rectangle, gradientWidth, Color.Empty, Color.Empty, Color.Empty, DEFAULT_ROUNDED)
		{
		}
		/// <summary>
		/// Costruttore della classe RoundSelectionBrush
		/// </summary>
		/// <param name="rectangle">Oggetto Rectangle che rappresenta l'ingombro del brush</param>
		/// <param name="gradientWidth">Valore che indica la larghezza della sfumatura al bordo (Color1)</param>
		/// <param name="color1">Colore esterno</param>
		/// <param name="color2">Colore medio</param>
		/// <param name="color3">Colore interno</param>
		public RoundSelectionBrush(Rectangle rectangle, float gradientWidth, Color color1, Color color2, Color color3)
			: this(rectangle, gradientWidth, color1, color2, color3, DEFAULT_ROUNDED)
		{
		}
		/// <summary>
		/// Costruttore della classe RoundSelectionBrush
		/// </summary>
		/// <param name="rectangle">Oggetto Rectangle che rappresenta l'ingombro del brush</param>
		/// <param name="gradientWidth">Valore che indica la larghezza della sfumatura al bordo (Color1)</param>
		/// <param name="color1">Colore esterno</param>
		/// <param name="color2">Colore medio</param>
		/// <param name="color3">Colore interno</param>
		/// <param name="roundWidth">Determina la rotondità degli angoli</param>
		public RoundSelectionBrush(Rectangle rectangle, float gradientWidth, Color color1, Color color2, Color color3, int roundWidth)
		{
			if (color1 == Color.Empty)
				m_color1 = DEFAULT_COLOR_1;
			else
				m_color1 = color1;

			if (color2 == Color.Empty)
				m_color2 = DEFAULT_COLOR_2;
			else
				m_color2 = color2;

			if (color3 == Color.Empty)
				m_color3 = DEFAULT_COLOR_3;
			else
				m_color3 = color3;

			m_rounded = roundWidth;
			m_rect = rectangle;
			m_gradient = gradientWidth;

			CreateBrushInternal();
		}

		#endregion

		#region Public

		/// <summary>
		/// Ottiene il Path del contorno del Brush
		/// </summary>
		/// <value></value>
		public GraphicsPath BrushPath
		{
			get
			{
				return m_gp;
			}
		}
		/// <summary>
		/// Ottiene o imposta il rettangolo di ingombro del Brush
		/// </summary>
		/// <value></value>
		public Rectangle BrushRectangle
		{
			get
			{
				return m_rect;
			}
			set
			{
				if (value != Rectangle.Empty)
				{
					m_rect = value;
					CreateBrushInternal();
				}
			}
		}
		/// <summary>
		/// Ottiene o imposta il colore esterno della sfumatura 
		/// </summary>
		/// <value></value>
		public Color Color1
		{
			get
			{
				return m_color1;
			}
			set
			{
				if (value != Color.Empty)
				{
					m_color1 = value;
					CreateBrushInternal();
				}
			}
		}
		/// <summary>
		/// Ottiene o imposta il colore medio della sfumatura
		/// </summary>
		/// <value></value>
		public Color Color2
		{
			get
			{
				return m_color2;
			}
			set
			{
				if (value != Color.Empty)
				{
					m_color2 = value;
					CreateBrushInternal();
				}
			}
		}
		/// <summary>
		/// Ottiene o imposta il colore interno della sfumatura
		/// </summary>
		/// <value></value>
		public Color Color3
		{
			get
			{
				return m_color3;
			}
			set
			{
				if (value != Color.Empty)
				{
					m_color3 = value;
					CreateBrushInternal();
				}
			}
		}
		/// <summary>
		/// Ottiene o imposta un Array di Oggetti Color
		/// </summary>
		/// <value>L'Array può contenere al massimo tre elementi</value>
		public Color[] Colors
		{
			get
			{
				return new Color[] { m_color1, m_color2, m_color3 };
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("Colors", "Il valore non può essere null, ");
				else if (value.Length > 3)
					throw new ArgumentOutOfRangeException("Colors", "Il numero massimo dei colori è 3, ");
				else
				{
					m_color1 = value[0];
					m_color2 = value[1];
					m_color3 = value[2];

					CreateBrushInternal();
				}
			}
		}
		/// <summary>
		/// Ottiene o imposta un valore che indica la larghezza della sfumatura esterna
		/// </summary>
		/// <value>Il valore deve essere positivo</value>
		public float GradientWidth
		{
			get
			{
				return m_gradient;
			}
			set
			{
				if (value < 0)
					m_gradient = 0;
				else
					m_gradient = value;

				CreateBrushInternal();
			}
		}
		/// <summary>
		/// Ottiene o imposta un valore che indica la rotondità degli angoli
		/// </summary>
		/// <value>Il valore deve essere positivo</value>
		public int RoundWidth
		{
			get
			{
				return m_rounded;
			}
			set
			{
				if (value < 0)
					m_rounded = 0;
				else
					m_rounded = value;

				CreateBrushInternal();
			}
		}


		#endregion

		#region Internal

		/// <summary>
		/// Restituisce un puntatore gestito all'oggetto Brush
		/// </summary>
		/// <value></value>
		internal IntPtr NativeBrush
		{
			get
			{
				return m_nativeBrush;
			}
		}

		#endregion

		#region Private

		private const float DEFAULT_GRADIENT_WIDTH = 12f;
		private const int DEFAULT_ROUNDED = 10;
		private Color DEFAULT_COLOR_1 = Color.FromArgb(100, SystemColors.Highlight);
		private Color DEFAULT_COLOR_2 = Color.FromArgb(150, Color.White);
		private Color DEFAULT_COLOR_3 = Color.FromArgb(50, Color.White);

		private Point m_location;
		private Size m_size;
		private Rectangle m_rect;
		private Point m_center;
		private float m_gradient;
		private int m_rounded;
		private GraphicsPath m_gp;

		private float m_factor = 1;

		private Color m_color1;
		private Color m_color2;
		private Color m_color3;

		private IntPtr m_nativeBrush;

		[SecurityPermissionAttribute(SecurityAction.Deny, UnmanagedCode = true)]
		private void CreateBrushInternal()
		{
			m_location = m_rect.Location;
			m_size = m_rect.Size;
			m_center = new Point(m_location.X + (m_size.Width / 2), m_location.Y + (m_size.Height / 2));

			if (m_size.Height < 16 || m_size.Width < 16)
			{
				m_factor = 0.25f;
			}
			if (m_size.Height < 35 || m_size.Width < 35)
			{
				m_factor = 0.5f;
			}
			else
			{
				m_factor = 1;
			}

			//m_gradient *= m_factor;

			double dX = ((double)m_size.Width / (10f * m_factor)) / 4f;
			double dY = ((double)m_size.Height / (10f * m_factor)) / 4f;
			double pX = m_gradient / dX;
			double pY = m_gradient / dY;

			double gX = pX / m_gradient;
			double gY = pY / m_gradient;

			double gradient = m_gradient / (float)(40f / 2f);
			double gradW = 4f / ((float)m_size.Width / 2f);
			double gradH = 4f / ((float)m_size.Height / 2f);

			double gradX = 1 - gX;
			double gradY = 1 - gY;

			float ray = m_rounded / 2;

			//			double alpha = 45 * (Math.PI / 180);
			//			float delta = (float)(ray * Math.Sin(alpha));
			//
			//			PointF[] pts = new PointF[17]
			//				{
			//					/* 0 */  new PointF(m_center.X,m_location.Y),
			//					/* 1 */  new PointF(m_rect.Right-ray,m_location.Y),
			//					/* 2 */  new PointF((m_rect.Right-ray)+delta,m_location.Y+(ray-delta)),
			//					/* 3 */  new PointF(m_rect.Right,m_location.Y+ray),
			//					/* 4 */  new PointF(m_rect.Right,m_center.Y),
			//					/* 5 */  new PointF(m_rect.Right,m_rect.Bottom-ray),
			//					/* 6 */  new PointF((m_rect.Right-ray)+delta,m_rect.Bottom-(ray-delta)),
			//					/* 7 */  new PointF(m_rect.Right-ray,m_rect.Bottom),
			//					/* 8 */  new PointF(m_center.X,m_rect.Bottom),
			//					/* 9 */  new PointF(m_location.X+ray,m_rect.Bottom),
			//					/* 10 */ new PointF(m_location.X+(ray-delta),m_rect.Bottom-(ray-delta)),
			//					/* 11 */ new PointF(m_location.X,m_rect.Bottom-ray),
			//					/* 12 */ new PointF(m_location.X,m_center.Y),
			//					/* 13 */ new PointF(m_location.X,m_location.Y+ray),
			//					/* 14 */ new PointF(m_location.X+(ray-delta),m_location.Y+(ray-delta)), 
			//					/* 15 */ new PointF(m_location.X+ray,m_location.Y),
			//					/* 16 */ new PointF(m_center.X,m_location.Y)
			//				};


			////***************** NON UTILIZZATE A CAUSA DELLE API *******************////
			//GraphicsPath gp = new GraphicsPath();
			//gp.AddCurve(pts, 0, 16, 0.05F);
			////**********************************************************************////

			m_gp = new GraphicsPath();
			//m_gp.AddCurve(pts, 0, 16, 0.05F);

			m_gp.AddLine(new PointF(m_center.X, m_location.Y), new PointF(m_rect.Right - ray, m_location.Y));
			m_gp.AddArc(new RectangleF(m_rect.Right - m_rounded, m_location.Y, m_rounded, m_rounded), 270, 90);
			m_gp.AddLine(new PointF(m_rect.Right, m_location.Y + ray), new PointF(m_rect.Right, m_rect.Bottom - ray));
			m_gp.AddArc(new RectangleF(m_rect.Right - m_rounded, m_rect.Bottom - m_rounded, m_rounded, m_rounded), 0, 90);
			m_gp.AddLine(new PointF(m_rect.Right - ray, m_rect.Bottom), new PointF(m_location.X + ray, m_rect.Bottom));
			m_gp.AddArc(new RectangleF(m_location.X, m_rect.Bottom - m_rounded, m_rounded, m_rounded), 90, 90);
			m_gp.AddLine(new PointF(m_location.X, m_rect.Bottom - ray), new PointF(m_location.X, m_location.Y + ray));
			m_gp.AddArc(new RectangleF(m_location.X, m_location.Y, m_rounded, m_rounded), 180, 90);
			m_gp.AddLine(new PointF(m_location.X + ray, m_location.Y), new PointF(m_center.X, m_location.Y));
			m_gp.CloseFigure();


			//// ******** PROVA ********** ////
			IntPtr nativePath = IntPtr.Zero;
			IntPtr ptrPoints = GDIplus.ConvertPointToMemory(m_gp.PathPoints);
			int iRet = GDIplus.GdipCreatePath((int)FillMode.Alternate, ref nativePath);
			HandleRef pathRef = new HandleRef(null, nativePath);
			iRet = GDIplus.GdipAddPathLine(pathRef, m_center.X, m_location.Y, m_rect.Right - ray, m_location.Y);
			iRet = GDIplus.GdipAddPathArc(pathRef, m_rect.Right - m_rounded, m_location.Y, m_rounded, m_rounded, 270, 90);
			iRet = GDIplus.GdipAddPathLine(pathRef, m_rect.Right, m_location.Y + ray, m_rect.Right, m_rect.Bottom - ray);
			iRet = GDIplus.GdipAddPathArc(pathRef, m_rect.Right - m_rounded, m_rect.Bottom - m_rounded, m_rounded, m_rounded, 0, 90);
			iRet = GDIplus.GdipAddPathLine(pathRef, m_rect.Right - ray, m_rect.Bottom, m_location.X + ray, m_rect.Bottom);
			iRet = GDIplus.GdipAddPathArc(pathRef, m_location.X, m_rect.Bottom - m_rounded, m_rounded, m_rounded, 90, 90);
			iRet = GDIplus.GdipAddPathLine(pathRef, m_location.X, m_rect.Bottom - ray, m_location.X, m_location.Y + ray);
			iRet = GDIplus.GdipAddPathArc(pathRef, m_location.X, m_location.Y, m_rounded, m_rounded, 180, 90);
			iRet = GDIplus.GdipAddPathLine(pathRef, m_location.X + ray, m_location.Y, m_center.X, m_location.Y);
			iRet = GDIplus.GdipClosePathFigure(pathRef);
			//// ************************* ////

			ColorBlend cblend = new ColorBlend();
			float[] positions = new float[] { 0.0F, (float)gradient, 1F };
			Color[] colors = new Color[] { m_color1, m_color2, m_color3 };
			cblend.Positions = positions;
			cblend.Colors = colors;

			////***************** NON UTILIZZATE A CAUSA DELLE API *******************////
			//PathGradientBrush pgb = new PathGradientBrush(m_gp.PathPoints);
			//pgb.CenterPoint = m_center;
			//pgb.CenterColor = m_centercolor;
			//pgb.InterpolationColors = cblend;
			//pgb.FocusScales = new PointF((float)gradX, (float)gradY);
			////**********************************************************************////

			// crea il PathGradientBrush
			m_nativeBrush = IntPtr.Zero;
			//IntPtr j = GDIplus.ConvertPointToMemory(m_gp.PathPoints);//pts
			// /*int*/ iRet = GDIplus.GdipCreatePathGradient(new HandleRef(null, j), (int)pts.Length, (int)WrapMode.Clamp, out m_nativeBrush);
			iRet = GDIplus.GdipCreatePathGradientFromPath(pathRef, out m_nativeBrush);
			HandleRef brushRef = new HandleRef(null, m_nativeBrush);
			// imposta il CenterPoint
			iRet = GDIplus.GdipSetPathGradientCenterPoint(brushRef, new GPPOINTF(m_center));
			// imposta InterpolationColors
			this.SetInterpolationColorsInternal(cblend);
			// imposta FocusScales
			iRet = GDIplus.GdipSetPathGradientFocusScales(brushRef, (float)gradX, (float)gradY);

			base.SetNativeBrush(m_nativeBrush);
		}

		[SecurityPermissionAttribute(SecurityAction.Deny, UnmanagedCode = true)]
		private void SetInterpolationColorsInternal(ColorBlend blend)
		{
			int i1 = (int)blend.Colors.Length;
			IntPtr j1 = Marshal.AllocHGlobal(4 * i1);
			IntPtr k = Marshal.AllocHGlobal(4 * i1);
			int[] nums = new int[i1];
			for (int i2 = 0; i2 < i1; i2++)
			{
				nums[i2] = blend.Colors[i2].ToArgb();
			}
			Marshal.Copy(nums, 0, j1, i1);
			Marshal.Copy(blend.Positions, 0, k, i1);
			int j2 = GDIplus.GdipSetPathGradientPresetBlend(new HandleRef(this, m_nativeBrush), new HandleRef(null, j1), new HandleRef(null, k), i1);
			Marshal.FreeHGlobal(j1);
			Marshal.FreeHGlobal(k);
			if (j2 != 0)
			{
				throw new Exception(j2.ToString());
			}
			else
			{
				return;
			}
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Crea una copia dell'oggetto
		/// </summary>
		/// <returns></returns>
		public override object Clone()
		{
			return new RoundSelectionBrush(m_rect, m_gradient, m_color1, m_color2, m_color3, m_rounded);
		}


		#endregion
	}
}
