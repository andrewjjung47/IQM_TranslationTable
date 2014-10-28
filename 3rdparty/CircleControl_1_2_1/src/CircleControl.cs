using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GAW
{
	/// <summary>
	/// 	Circle control.
	/// </summary>

    public partial class CircleControl : UserControl
    {
        /// <summary>
        /// 	Default constructor.
        /// </summary>
		/// <remarks>
		/// 	The default constructor creates a control with one <c>MarkerSet</c> composed of one <c>Marker</c>.
		/// </remarks>
        public CircleControl()
        {
            InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            markerSets = new Collection<MarkerSet>(this);
            textItems = new TextItemCollection(this);
            rings = new RingCollection(this);

			MarkerSet ms = new MarkerSet();
			markerSets.Add(ms);
			PointF[] markerPoly = new PointF[4];
			markerPoly[0] = new PointF(0.35F,  0.00F);
			markerPoly[1] = new PointF(0.80F,  0.20F);
			markerPoly[2] = new PointF(0.70F,  0.00F);
            markerPoly[3] = new PointF(0.80F, -0.20F);
			ms.Add(new Marker(MakeArgb(0.7f, Color.DarkGray), Color.Black, 2.0f, markerPoly));

			mouseMode = MouseMode.CannotDrag;

			ResetPaintTimeCounters();
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.SuspendLayout();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CircleControl";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CircleControl_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CircleControl_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CircleControl_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CircleControl_MouseUp);
            this.SizeChanged += new System.EventHandler(this.CircleControl_SizeChanged);
            this.ResumeLayout(false);
        }

        #endregion

		/// <summary>
		/// 	Helper method to generate a new <c>Color</c> object by applying an opacity value to an existing color.
		/// </summary>
		/// <param name="opacity">Opacity value.  Must be in the range [0.0,1.0].</param>
		/// <param name="color">Color value.</param>
		/// <returns>
		///		New color.
		/// </returns>
		static public Color MakeArgb(float opacity, Color color)
		{
			Debug.Assert(opacity >= 0.0f && opacity <= 1.0f);
			Color c = Color.FromArgb((int)(255.0f * opacity), color);
			return c;
		}

		static internal float AdjustOffsetAngle(float offsetAngle)
		{
			float a = offsetAngle % 360.0f;
			if (a < 0.0f)
			{
				a += 360f;
			}
			return a;
		}

		static internal void CheckPoints(PointF[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("PointF[] points");
			}
			if (points.Length < 2)
			{
				throw new ArgumentException("Parameter points must be an array with at least two elements");
			}
		}

		private Collection<MarkerSet> markerSets;
		/// <summary>
		/// 	Gets the collection of <c>MarkerSet</c> objects for this <c>CircleControl</c>.
		/// </summary>
		/// <value>
		/// 	The collection of <c>MarkerSet</c> objects for this <c>CircleControl</c>.
		/// </value>
		/// <remarks>
		///		This value is never <c>null</c>.  If there are no marker sets, the collection is empty.
		/// </remarks>
		public Collection<MarkerSet> MarkerSets
		{
			[DebuggerStepThrough]
			get { return markerSets; }
		}

		private TextItemCollection textItems;
		/// <summary>
		/// 	Gets the collection of <c>TextItem</c> objects for this <c>CircleControl</c>.
		/// </summary>
		/// <value>
		/// 	The collection of <c>TextItem</c> objects for this <c>CircleControl</c>.
		/// </value>
		/// <remarks>
		///		This value is never <c>null</c>.  If there are no text items, the collection is empty.
		/// </remarks>
		public TextItemCollection TextItems
		{
			[DebuggerStepThrough]
			get { return textItems; }
		}

		private RingCollection rings;
		/// <summary>
		/// 	Gets the collection of <c>Ring</c> objects for this <c>CircleControl</c>.
		/// </summary>
		/// <value>
		/// 	The collection of <c>Ring</c> objects for this <c>CircleControl</c>.
		/// </value>
		/// <remarks>
		///		This value is never <c>null</c>.  If there are no rings, the collection is empty.
		/// </remarks>
		public RingCollection Rings
		{
			[DebuggerStepThrough]
			get { return rings; }
		}

        /// <summary>
        /// 	Provides data for the <c>AngleChanged</c> event.
        /// </summary>
		public class AngleChangedArgs : EventArgs
		{
			/// <summary>
			/// 	State of mouse button when angle changed.
			/// </summary>
			/// <remarks>
			/// 	It's not really the "state" of the mouse button, it's more like a grungy combination of mouse state and recent mouse events.
			///		Forgive the lack of purity...
			/// </remarks>
			public enum MouseState
			{
				/// <summary>
				/// 	Mouse button has just been clicked down, and the marker has been grabbed.
				/// 	Next state is Dragging or Up.
				/// </summary>
				Down,

				/// <summary>
				/// 	Mouse button was previously clicked down, and the marker is being dragged.
				/// 	Next state is Up.
				/// </summary>
				Dragging,

				/// <summary>
				/// 	Mouse button was just released, dragging has completed.
				/// 	Next state is Down.
				/// </summary>
				Up,

				/// <summary>
				/// 	Mouse state is not known - angle was been changed by setting <c>Angle</c> property.
				/// </summary>
				Unknown,
			}

			internal AngleChangedArgs(MarkerSet ms, float angle, MouseState mouseState)
					: this(ms, angle, 0.0f, mouseState)
			{ }

			internal AngleChangedArgs(MarkerSet ms, float angle, float angleChange, MouseState mouseState)
			{
				this.ms = ms;
				this.angle = angle;
                this.angleChange = angleChange;
				this.mouseState = mouseState;
			}

			private MarkerSet ms;
			/// <summary>
			/// 	Gets the <c>MarkerSet</c> which is/was being dragged.
			/// </summary>
			/// <value>
			/// 	The <c>MarkerSet</c> which is/was being dragged.
			/// </value>
			public MarkerSet Ms
			{
				get { return ms; }
			}

			private float angle;
			/// <summary>
			/// 	Gets the angle of the <c>MarkerSet</c> which is/was being dragged.
			/// </summary>
			/// <value>
			/// 	The angle of the <c>MarkerSet</c> which is/was being dragged.
			/// </value>
			public float Angle
			{
				get { return angle; }
			}

			private float angleChange;
			/// <summary>
			/// 	Gets Indicates the amount the angle value has changed since the last event.
			/// </summary>
			/// <value>
			/// 	Amount angle has changed since last event for this <c>MarkerSet</c>.
			/// </value>
			public float AngleChange
			{
				get { return angleChange; }
			}

			private MouseState mouseState;
			/// <summary>
			/// 	Gets the state of the mouse when the angle changed.
			/// </summary>
			/// <value>
			/// 	One of the <c>MouseState</c> values.
			/// </value>
			public MouseState Mouse
			{
				get { return mouseState; }
			}
		}

        /// <summary>
        /// 	Represents the method that is called when the angle of a MarkerSet changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		public delegate void AngleChangedHandler(object sender, AngleChangedArgs e);

		/// <summary>
		/// 	This delegate is called when the angle of a MarkerSet changes.
		/// </summary>
		public event AngleChangedHandler AngleChanged;

		internal void AngleChangedEvent(MarkerSet ms, float angleChange, AngleChangedArgs.MouseState state)
		{
			if (AngleChanged != null)
			{
				if (angleWraps)
				{
					if (angleChange >= 180f)
					{
						angleChange = angleChange - 360f;
					}
					else if (angleChange <= -180f)
					{
						angleChange = angleChange + 360f;
					}
				}
				AngleChanged(this, new AngleChangedArgs(ms, ms.Angle, angleChange, state));
			}
		}

		private bool angleWraps = true;
		/// <summary>
		/// 	Indicates whether angle wrapping is enabled.
		/// </summary>
		/// <value>
		/// 	Enables or disable angle wrapping.  Default value is <c>true</c>.
		/// </value>
		/// <remarks>
		///		If <c>true</c>, the angle range is always [0,360).
		/// 	If <c>false</c>, then the valid angle range is from <c>AngleMin</c> to <c>AngleMax</c>, inclusive.
		/// 	The difference between this values can exceed 360.0.  The control keeps track of the actual location
		/// 	of each marker.
		/// </remarks>
		[Category("Behavior"), DefaultValue("true")]
		public bool AngleWraps
		{
			get { return angleWraps; }
			set
			{
				if (value != angleWraps)
				{
					angleWraps = value;
					AdjustAngles();
				}
			}
		}

		private float angleMin = 0.0F;
		/// <summary>
		/// 	Gets or sets the minimum angle value.
		/// </summary>
		/// <value>
		/// 	Minimum angle value.
		/// </value>
		/// <remarks>
		/// 	Only applicable if <c>AngleWraps</c> is <c>false</c>.
		/// </remarks>
		[Category("Behavior"), DefaultValue(0.0)]
		public float AngleMin
		{
			get { return angleMin; }
			set
			{
				if (value != angleMin)
				{
					angleMin = value;
					angleMax = Math.Max(angleMin, angleMax);
					AdjustAngles();
				}
			}
		}

		private float angleMax = 360.0F;
		/// <summary>
		/// 	Gets or sets the maximum angle value.
		/// </summary>
		/// <value>
		/// 	Maximum angle value.
		/// </value>
		/// <remarks>
		/// 	Only applicable if <c>AngleWraps</c> is <c>false</c>.
		/// </remarks>
		[Category("Behavior"), DefaultValue(360.0)]
		public float AngleMax
		{
			get { return angleMax; }
			set
			{
				if (value != angleMax)
				{
					angleMax = value;
					angleMin = Math.Min(angleMin, angleMax);
					AdjustAngles();
				}
			}
		}

		/// <summary>
        /// 	Set minimum and maximum angle value.
		/// </summary>
		/// <param name="min">Minimum angle.</param>
		/// <param name="max">Maximum angle.</param>
		/// <remarks>
		///		If, by chance, <c>min</c> is greater than <c>max</c>, the two values are silently flipped.
		/// 	Only applicable if <c>AngleWraps</c> is <c>false</c>.
		/// </remarks>
		public void SetAngleMinMax(float min, float max)
		{
			float v1 = Math.Min(min, max);
			float v2 = Math.Max(min, max);
			if (v1 != angleMin || v2 != angleMax)
			{
				angleMin = v1;
				angleMax = v2;
				AdjustAngles();
			}
		}

		// Adjust angle value depending if angle wrapping is enabled.
		private float AdjustAngle(float a)
		{
			if (angleWraps)
			{
				a %= 360.0F;
				if (a < 0.0F)
				{
					a += 360.0F;
				}
				return a;
			}
			return Math.Max(Math.Min(a, angleMax), angleMin);
		}

		private void AdjustAngles()
		{
			bool redraw = false;
			for (int i = 0; i < markerSets.Count; i++)
			{
				MarkerSet ms = markerSets[i];
				float a = AdjustAngle(ms.Angle);
				if (a != ms.Angle)
				{
					ms.Angle = a;
					redraw = true;
				}
			}
			if (redraw)
			{
				Redraw();
			}
		}

		private bool fixedBackground = false;
		/// <summary>
		/// 	Gets or sets the fixed background flag.
		/// </summary>
		/// <value>
		/// 	<c>True</c> if rings, ticks, and text items are considered fixed, <c>false</c> otherwise.
		///		<para>
		///		If fixed, the background color, rings, major and minor tick marks, and text items
		/// 	are rendered onto a bitmap, which is displayed during the Paint event.
		/// 	This bitmap is only re-rendered if and when any of these objects change,
		/// 	or the size of the control changes.
		///		</para>
		///		<para>
		/// 	Set this property to <c>true</c> if the rings, ticks and text items
		/// 	are relatively unchanging, and the time to repaint the control should decrease.
		/// 	Set to <c>false</c> if any of the the background objects change frequently.
		///		</para>
		/// </value>
		/// <remarks>
		/// 	Use the <c>PaintTimeCount</c> and <c>PaintTimeMean</c> properties to determine 
		/// 	which setting for this property is faster.
		/// </remarks>
		/// <seealso cref="PaintTimeCount" />
		/// <seealso cref="PaintTimeMean" />
		/// <seealso cref="ResetPaintTimeCounters" />
		/// <seealso cref="MarkerSet.IncludeInFixedBackground" />
		[Category("Behavior"), DefaultValue("false")]
		public bool FixedBackground
		{
			get { return fixedBackground; }
			set
			{
				if (value != fixedBackground)
				{
					fixedBackground = value;
					Redraw(true);
				}
			}
		}

		private double paintTimeSum;
		private int paintTimeCount;
		/// <summary>
		/// 	Gets the number of times the control has been repainted since creation,
		///		or the last time <c>ResetPaintTimeCounters()</c> was called.
		/// </summary>
		/// <value>
		/// 	Number of repaints.
		/// </value>
		/// <seealso cref="FixedBackground" />
		/// <seealso cref="PaintTimeMean" />
		/// <seealso cref="ResetPaintTimeCounters" />
		public int PaintTimeCount
		{
			get { return paintTimeCount; }
		}

		/// <summary>
		/// 	Gets the mean paint time (in milliseconds) since creation,
		/// 	or the last time <c>ResetPaintTimeCounters()</c> was called.
		/// </summary>
		/// <value>
		/// 	Mean paint time (in milliseconds).
		/// </value>
		/// <remarks>
		/// 	The PaintTime parameters (<c>PaintTimeCount</c> and <c>PaintTimeMean</c>)
		/// 	are intended to assist the developer in determining whether painting is faster
		/// 	with <c>FixedBackground</c> set to <c>true</c> or <c>false</c>.
		/// </remarks>
		/// <seealso cref="FixedBackground" />
		/// <seealso cref="PaintTimeCount" />
		/// <seealso cref="ResetPaintTimeCounters" />
		public double PaintTimeMean
		{
			get
			{
				if (paintTimeCount > 0)
				{
					return paintTimeSum / (double)paintTimeCount;
				}
				return 0.0;
			}
		}

		/// <summary>
		/// 	Sets the <c>PaintTimeCount</c> and <c>PaintTimeMean</c> values to zero.
		/// </summary>
		/// <seealso cref="FixedBackground" />
		/// <seealso cref="PaintTimeCount" />
		/// <seealso cref="PaintTimeMean" />
		public void ResetPaintTimeCounters()
		{
			paintTimeSum = 0.0;
			paintTimeCount = 0;
		}


		/// <summary>
		/// 	Shortcut to first <c>MarkerSet</c>.
		/// </summary>
		/// <value>
		/// 	The first <c>MarkerSet</c>.
		/// </value>
		/// <remarks>
		///		Equivalent to <c>MarkerSets[0]</c>.
		/// </remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		Thrown if the collection of of <c>MarkerSet</c> objects is empty.
		/// </exception>
		public MarkerSet PrimaryMarkerSet
		{
			get { return markerSets[0]; }
		}

		/// <summary>
		/// 	Shortcut to the first <c>Marker</c> in the first <c>MarkerSet</c>.
		/// </summary>
		/// <value>
		/// 	The first <c>Marker</c> in the first <c>MarkerSet</c>.
		/// </value>
		/// <remarks>
		/// 	Equivalent to <c>MarkerSets[0].Marker[0]</c>.
		/// </remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		Thrown if the collection of <c>MarkerSet</c> objects is empty, or if the the first <c>MarkerSet</c> has no markers.
		/// </exception>
		public Marker PrimaryMarker
		{
			get { return PrimaryMarkerSet[0]; }
		}

		/// <summary>
		/// 	Shortcut to the angle of the first <c>MarkerSet</c>.
		/// </summary>
		/// <value>
		/// 	The angle of the first <c>MarkerSet</c>.
		/// </value>
		/// <remarks>
		/// 	Equivalent to <c>MarkerSets[0].Angle</c>.
		/// </remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		Thrown if the collection of <c>MarkerSet</c> objects is empty, or if the the first <c>MarkerSet</c> has no markers.
		/// </exception>
		[Category("Behavior"), DefaultValue(45.0)]
		public float PrimaryMarkerAngle
		{
			get { return PrimaryMarkerSet.Angle; }
			set { PrimaryMarkerSet.Angle = value; }
		}

		/// <summary>
		/// 	Shortcut to the solid fill color of the first <c>Marker</c> in the first <c>MarkerSet</c>.
		/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.Solid</c>.
		/// </summary>
		/// <value>
		/// 	The solid fill color of the first <c>Marker</c> in the first <c>MarkerSet</c>.
		///		Default value is <c>DarkGray</c>.
		/// </value>
		/// <remarks>
		/// 	Equivalent to <c>MarkerSets[0].Marker[0].SolidColor</c>.
		/// </remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		Thrown if the collection of <c>MarkerSet</c> objects is empty, or if the the first <c>MarkerSet</c> has no markers.
		/// </exception>
		[Category("Appearance"), DefaultValue("DarkGray")]
		public Color PrimaryMarkerSolidColor
		{
			get { return PrimaryMarker.SolidColor; }
			set { PrimaryMarker.SolidColor = value; }
		}

		/// <summary>
		/// 	Shortcut to the border color of the first <c>Marker</c> in the first <c>MarkerSet</c>.
		/// </summary>
		/// <value>
		/// 	The border color of the first <c>Marker</c> in the first <c>MarkerSet</c>.
		/// </value>
		/// <remarks>
		/// 	Equivalent to <c>MarkerSets[0].Markers[0].BorderColor</c>.
		/// </remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 	Thrown if the collection of <c>MarkerSet</c> objects is empty, or if the the first <c>MarkerSet</c> has no markers.
		/// </exception>
		[Category("Appearance"), DefaultValue("Black")]
		public Color PrimaryMarkerBorderColor
		{
			get { return PrimaryMarker.BorderColor; }
			set { PrimaryMarker.BorderColor = value; }
		}

		/// <summary>
        /// 	Shortcut to the border thickness (in pixels) of the first <c>Marker</c> in the first <c>MarkerSet</c>.
		/// </summary>
		/// <value>
        /// 	The border thickness (in pixels) of the first <c>Marker</c> in the first <c>MarkerSet</c>.
		/// 	Set to zero for no border.
		/// </value>
		/// <remarks>
		/// 	Equivalent to <c>MarkerSets[0].Markers[0].BorderSize</c>.
		/// </remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		Thrown if the collection of <c>MarkerSet</c> objects is empty, or if the the first <c>MarkerSet</c> has no markers.
		/// </exception>
		[Category("Appearance"), DefaultValue(2.0)]
		public float PrimaryMarkerBorderSize
		{
			get { return PrimaryMarker.BorderSize; }
			set { PrimaryMarker.BorderSize = value; }
		}

		/// <summary>
        /// 	Shortcut to the array of points that defines the shape of the of the first <c>Marker</c> in the first <c>MarkerSet</c>.
		/// </summary>
		/// <value>
        /// 	The array of <c>System.Drawing.PointF</c> structures that represent the vertices of the marker when the marker is at zero degrees
		/// 	of the first <c>Marker</c> in the first <c>MarkerSet</c>.
		/// </value>
		/// <remarks>
		/// 	Equivalent to <c>MarkerSets[0].Markers[0].Points</c>.
		/// </remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 	Thrown if the collection of <c>MarkerSet</c> objects is empty, or if the the first <c>MarkerSet</c> has no markers.
		/// </exception>
		public PointF[] PrimaryMarkerPoints
		{
			get { return (PointF[])PrimaryMarker.Points.Clone(); }
            set { PrimaryMarker.Points = (PointF[])value.Clone(); }
		}

		private int majorTicks = 10;
		/// <summary>
		/// 	Gets or sets the number of major ticks.
		/// </summary>
		/// <value>
		/// 	The number of major ticks.
		///		<para>
		/// 	The major ticks are placed around the control at even angular intervals, with the first at zero degrees.
		/// 	Set to zero for no major ticks.
		///		</para>
		/// </value>
		[Category("Appearance"), DefaultValue(4)]
		public int MajorTicks
		{
			get { return majorTicks; }
			set
			{
				majorTicks = Math.Max(value, 0);
				Redraw(true);
			}
		}

		/// <summary>
		/// 	Returns the angle of the major tick at the specified angle.
		/// </summary>
		/// <param name="idx">The zero-based index of the major tick.</param>
		/// <returns>
		///		The angle of the major tick mark at the specified.
		/// </returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		///		Thrown if <paramref name="idx" /> is less than zero or greater than <c>MajorTicks</c>.
		/// </exception>
		/// <remarks>
		/// 	The angle of major tick at index zero is always zero.
		/// </remarks>
		public float GetMajorTickAngle(int idx)
		{
			if (idx < 0 || idx >= MajorTicks)
			{
				throw new System.ArgumentOutOfRangeException("idx");
			}
			return 360.0f * (float)idx / (float)MajorTicks;
		}

		private Color majorTickColor = Color.Black;
		/// <summary>
		/// 	Gets or set the color of the major ticks.
		/// </summary>
		/// <value>
		/// 	The color of the major ticks.
		/// </value>
		[Category("Appearance"), DefaultValue("Black")]
		public Color MajorTickColor
		{
			get { return majorTickColor; }
			set
			{
				majorTickColor = value;
				Redraw(true);
			}
		}

		private float majorTickStart = 0.40f;
		/// <summary>
		/// 	Gets or sets the start location of major ticks.
		/// </summary>
		/// <value>
		/// 	The start location of major ticks.
		///		<para>
		/// 	This value is the distance from the center of the control, where 1.0 is defined
		/// 	as the distance from the center of the control to the nearest edge.
		///		</para>
		/// </value>
		/// <exception cref="System.ArgumentException">
		///		Thrown if value is less than zero.
		/// </exception>
		[Category("Appearance"), DefaultValue(0.40)]
		public float MajorTickStart
		{
			get { return majorTickStart; }
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException("MajorTickStart");
				}
				if (value != majorTickStart)
				{
					majorTickStart = value;
					Redraw(true);
				}
			}
		}

		private float majorTickSize = 0.45f;
		/// <summary>
		/// 	Gets or sets the size of the major ticks.
		/// </summary>
		/// <value>
		/// 	The size of the major ticks.
		///		<para>
		/// 	This value indicates the length of the major ticks, where 1.0 is defined 
		/// 	as the distance from the center of the control to the nearest edge.
		///		</para>
		/// </value>
		/// <exception cref="System.ArgumentException">
		///		Thrown if value is less than or equal to zero.
		/// </exception>
		[Category("Appearance"), DefaultValue(0.45)]
		public float MajorTickSize
		{
			get { return majorTickSize; }
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentException("MajorTickSize");
				}
				if (value != majorTickSize)
				{
					majorTickSize = value;
					Redraw(true);
				}
			}
		}

		private float majorTickThickness = 0.45f;
		/// <summary>
		/// 	Gets or sets the thickness of the major ticks.
		/// </summary>
		/// <value>
		/// 	The thickness (in pixels) of the major ticks.
		/// </value>
		/// <exception cref="System.ArgumentException">
		///		Thrown if value is less than zero.
		/// </exception>
		[Category("Appearance"), DefaultValue(1.0)]
		public float MajorTickThickness
		{
			get { return majorTickThickness; }
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException("MajorTickThickness");
				}
				if (value != majorTickThickness)
				{
					majorTickThickness = value;
					Redraw(true);
				}
			}
		}

		private int minorTicksPerMajorTick = 0;
		/// <summary>
		/// 	Gets or sets the number of minor ticks per major tick.
		/// </summary>
		/// <value>
		/// 	The number of minor ticks per major tick.
		///		<para>
		/// 	Minor ticks are placed between major ticks, at regular angular intervals.
		/// 	This value is only significant if the number of major ticks is greater than zero.
		///		</para>
		/// </value>
		[Category("Appearance"), DefaultValue(0)]
		public int MinorTicksPerMajorTick
		{
			get { return minorTicksPerMajorTick; }
			set
			{
				minorTicksPerMajorTick = Math.Max(value, 0);
				Redraw(true);
			}
		}

		private Color minorTickColor = Color.Black;
		/// <summary>
		/// 	Gets or sets the color of the minor ticks.
		/// </summary>
		/// <value>
		/// 	The color of the minor ticks.
		/// </value>
		[Category("Appearance"), DefaultValue("Black")]
		public Color MinorTickColor
		{
			get { return minorTickColor; }
			set
			{
				minorTickColor = value;
				Redraw(true);
			}
		}

		private float minorTickStart = 0.50f;
		/// <summary>
		/// 	Gets or sets the start location of minor ticks.
		/// </summary>
		/// <value>
		/// 	The start location of minor ticks.
		///		<para>
		/// 	This value is the distance from the center of the control, where 1.0 is defined
		/// 	as the distance from the center of the control to the nearest edge.
		///		</para>
		/// </value>
		/// <exception cref="System.ArgumentException">
		///		Thrown if value is less than zero.
		/// </exception>
		[Category("Appearance"), DefaultValue(0.50)]
		public float MinorTickStart
		{
			get { return minorTickStart; }
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException("MinorTickStart");
				}
				if (value != minorTickStart)
				{
					minorTickStart = value;
					Redraw(true);
				}
			}
		}

		private float minorTickSize = 0.25f;
		/// <summary>
		/// 	Gets or sets the size of the minor ticks.
		/// </summary>
		/// <value>
		/// 	The size of the minor ticks.
		///		<para>
		/// 	This value indicates the length of the minor ticks, where 1.0 is defined 
		/// 	as the distance from the center of the control to the nearest edge.
		///		</para>
		/// </value>
		/// <exception cref="System.ArgumentException">
		///		Thrown if value is less than or equal to zero.
		/// </exception>
		[Category("Appearance"), DefaultValue(0.25)]
		public float MinorTickSize
		{
			get { return minorTickSize; }
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentException("MinorTickSize");
				}
				if (value != minorTickSize)
				{
					minorTickSize = value;
					Redraw(true);
				}
			}
		}

		private float minorTickThickness = 0.45f;
		/// <summary>
		/// 	Gets or sets the thickness of the minor ticks.
		/// </summary>
		/// <value>
		/// 	The thickness (in pixels) of the minor ticks.
		/// </value>
		/// <exception cref="System.ArgumentException">
		///		Thrown if value is less than zero.
		/// </exception>
		[Category("Appearance"), DefaultValue(1.0)]
		public float MinorTickThickness
		{
			get { return minorTickThickness; }
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException("MinorTickThickness");
				}
				if (value != minorTickThickness)
				{
					minorTickThickness = value;
					Redraw(true);
				}
			}
		}

		private SmoothingMode smoothing = SmoothingMode.AntiAlias;
		/// <summary>
		/// 	Gets or sets the rendering quality used internally by <c>Graphics</c>.
		/// </summary>
		/// <value>
		///		One of the <c>Graphics.SmoothingMode</c> values.
		/// </value>
		[Category("Appearance"), DefaultValue("AntiAlias")]
		public SmoothingMode Smoothing
		{
			get { return smoothing; }
			set
			{
				smoothing = value;
				Redraw(true);
			}
		}

        private Cursor cursorCannotDrag = Cursors.Default;
        /// <summary>
        /// 	Gets or sets the cursor that is displayed when the mouse pointer is over a <c>Marker</c> which cannot be dragged.
        /// </summary>
        /// <value>
		///		A <c>Cursor</c> value that represents the cursor to display when the mouse pointer is over a <c>Marker</c> which cannot be dragged.
        /// 	Default is <c>Cursors.Default</c>.
        /// </value>
        [Category("Appearance"), DefaultValue("Default")]
        public Cursor CursorCannotDrag
        {
            get { return cursorCannotDrag; }
            set
            {
                if (value != null)
                {
                    cursorCannotDrag = value;
                    UpdateCursor();
                }
            }
        }

        private Cursor cursorCanDrag = Cursors.Hand;
        /// <summary>
        /// 	Gets or sets the cursor that is displayed when the mouse pointer is over a <c>Marker</c> which can be dragged.
        /// </summary>
        /// <value>
		///		A <c>Cursor</c> value that represents the cursor to display when the mouse pointer is over a <c>Marker</c> which can be dragged.
        /// 	Default is <c>Cursors.Hand</c>.
        /// </value>
        [Category("Appearance"), DefaultValue("Hand")]
        public Cursor CursorCanDrag
        {
            get { return cursorCanDrag; }
            set
            {
                if (value != null)
                {
                    cursorCanDrag = value;
                    UpdateCursor();
                }
            }
        }

        private Cursor cursorDragging;
        /// <summary>
        /// 	Gets or sets the cursor that is displayed when the mouse pointer is dragging a <c>Marker</c>.
        /// </summary>
        /// <value>
		///		A <c>Cursor</c> value that represents the cursor to display when the mouse pointer is dragging a <c>Marker</c>.
        /// 	Default is <c>Cursors.SizeAll</c>.
        /// </value>
        [Category("Appearance"), DefaultValue("SizeAll")]
        public Cursor CursorDragging
        {
            get { return cursorDragging; }
            set
            {
                if (value != null)
                {
                    cursorDragging = value;
                    UpdateCursor();
                }
            }
        }

		static private float FixRatio(float r)
		{
			return (float)Math.Max(r, 0.0f);
		}

		private void Redraw(bool redrawFixedBackground)
		{
			if (redrawFixedBackground)
			{
				ClearFixedBackground();
			}
			Invalidate();
		}

		private void Redraw()
		{
			Redraw(false);
		}

		private Bitmap bgBitmap = null;

		private void ClearFixedBackground()
		{
			if (bgBitmap != null)
			{
				bgBitmap.Dispose();
				bgBitmap = null;
			}
		}

        private void CircleControl_SizeChanged(object sender, EventArgs e)
        {
			Redraw(true);
        }

		// Painting radials
		private const float RAD = (float)(Math.PI / 180.0);
		private const float RADInv = 1.0F / RAD;

		// Helper method - calc cosine of angle in degrees
		private static float COS(float angle)
		{
			float rad = RAD * angle;
			float cos = (float)Math.Cos(rad);
			return cos;
		}

		// Helper method - calc sine of angle in degrees
		private static float SIN(float angle)
		{
			float rad = RAD * angle;
			float sin = (float)Math.Sin(rad);
			return sin;
		}

		private float xmid;
		private float ymid;
		private float radius;

        private void CircleControl_Paint(object sender, PaintEventArgs e)
        {
			Stopwatch sw = Stopwatch.StartNew();

            Graphics g = e.Graphics;
			g.SmoothingMode = smoothing;

			int dy = this.ClientSize.Height;
			int dx = this.ClientSize.Width;

			xmid = (float)(dx - 1) / 2.0F;
			ymid = (float)(dy - 1) / 2.0F;
            radius = Math.Min(xmid, ymid);

			if (fixedBackground)
			{
				if (bgBitmap == null)
				{
					bgBitmap = new Bitmap(dx, dy, PixelFormat.Format24bppRgb);
					Graphics gb = Graphics.FromImage(bgBitmap);
					gb.SmoothingMode = smoothing;
					DrawBackground(gb);
					gb.Dispose();
				}
				g.DrawImage(bgBitmap, 0, 0);
			}
			else
			{
				DrawBackground(g);
			}

			// Draw unfixed marker sets - in reverse order
			for (int i = markerSets.Count - 1; i >= 0; i--)
			{
				MarkerSet ms = markerSets[i];
				if (!ms.IncludeInFixedBackground)
				{
					DrawMarkerSet(g, ms);
				}
			}

			sw.Stop();
			paintTimeSum += sw.Elapsed.TotalMilliseconds;
			paintTimeCount++;
        }

		private void DrawBackground(Graphics g)
		{
			Brush br = new SolidBrush(BackColor);
			// Tricky - FillRectangle fills the INTERNAL rectangle area
			// So must inflate
			Rectangle r = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
			r.Inflate(1, 1);
			g.FillRectangle(br, r);
			br.Dispose();

			DrawRings(g);
			DrawTicks(g);
			DrawTextItems(g);

			// Draw fixed markers - in reverse order
			for (int i = markerSets.Count - 1; i >= 0; i--)
			{
				MarkerSet ms = markerSets[i];
				if (ms.IncludeInFixedBackground)
				{
					DrawMarkerSet(g, ms);
				}
			}
		}

		private void DrawRings(Graphics g)
		{
			// Draw rings from outside in, then draw borders
			if (rings.Count > 0)
			{
				RectangleF[] rect = new RectangleF[rings.Count];
				float offset = 0.0f;
				for (int i = 0; i < rings.Count; i++)
				{
					float rad = radius * (offset + rings[i].Size);
					float diam = 2.0f * rad;
					rect[i] = new RectangleF(xmid - rad, ymid - rad, diam, diam);
					offset += rings[i].Size;
				}
				for (int i = rings.Count - 1; i >= 0; i--)
				{
			 		DrawRing(g, rings[i], rect[i]);
				}
				for (int i = rings.Count - 1; i >= 0; i--)
				{
					DrawRingBorder(g, rings[i], rect[i]);
				}
			}
		}

		private GraphicsPath GetRingPath(Ring r, RectangleF rect)
		{
			GraphicsPath path = new GraphicsPath();
			path.AddEllipse(rect);
			return path;
		}

		private void DrawRing(Graphics g, Ring r, RectangleF rect)
		{
			GraphicsPath path = GetRingPath(r, rect);
			g.FillPath(r.GetBrush(rect), path);
			path.Dispose();
		}

		private void DrawRingBorder(Graphics g, Ring r, RectangleF rect)
		{
			if (r.BorderSize > 0.0f)
			{
				Pen pen = new Pen(r.BorderColor, r.BorderSize);
				GraphicsPath path = GetRingPath(r, rect);
				g.DrawPath(pen, path);
				path.Dispose();
				pen.Dispose();
			}
		}

		private void DrawTicks(Graphics g)
		{
			if (majorTicks > 0)
			{
				Pen pen = new Pen(majorTickColor, majorTickThickness);
				Pen pen2 = new Pen(minorTickColor, minorTickThickness);

				double tickDiv = 360.0 / (double)(majorTicks * (minorTicksPerMajorTick + 1));

				for (int i = 0; i < majorTicks; i++)
				{
					double majorAng = 360.0 * (double)i/(double)majorTicks;
					DrawTick(g, pen, majorAng, majorTickStart, majorTickSize);

					if (minorTicksPerMajorTick > 0)
					{
						for (int j = 1; j <= minorTicksPerMajorTick; j++)
						{
							double minorAng = majorAng + (double)j * tickDiv;
							DrawTick(g, pen2, minorAng, minorTickStart, minorTickSize);
						}
					}
				}

				pen2.Dispose();
				pen.Dispose();
			}
		}

		private void DrawTick(Graphics g, Pen pen, double angle, float tickStart, float tickSize)
		{
			float cos = COS((float)angle);
			float sin = SIN((float)angle);
			float tickEnd = tickStart + tickSize;
			float xa = radius * tickStart * cos;
			float xb = radius * tickEnd * cos;
			float ya = radius * -tickStart * sin; // flip on y-axis
			float yb = radius * -tickEnd * sin;

			g.DrawLine(pen, xmid + xa, ymid + ya, xmid + xb, ymid + yb);
		}

		private void DrawTextItems(Graphics g)
		{
			for (int i = 0; i < textItems.Count; i++)
			{
				TextItem ti = textItems[i];

				float cos = COS(ti.Angle);
				float sin = SIN(ti.Angle);

				// x,y is center of text
				float x = radius * ti.Position * cos;
				float y = radius * ti.Position * sin;

				ti.Draw(g, radius, xmid + x, ymid - y);
			}
		}

		private void DrawMarkerSet(Graphics g, MarkerSet ms)
		{
			for (int i = ms.Count - 1; i >= 0; i--)
			{
				if (ms[i].Visible)
				{
					DrawMarker(g, ms[i], ms.Angle + ms[i].OffsetAngle);
				}
			}
		}

		private void DrawMarker(Graphics g, Marker m, float angle)
		{
			float cos = COS(angle);
			float sin = SIN(angle);

			// Build a new polygon using angle
			PointF[] p = new PointF[m.Points.Length];
			for (int i = 0; i < m.Points.Length; i++)
			{
				float x = m.Points[i].X * radius;
				float y = -m.Points[i].Y * radius; // flip on y-axis

				p[i].X = xmid + x * cos + y * sin;
				p[i].Y = ymid - x * sin + y * cos;
			}

			m.ClearPath();
			m.Path = new GraphicsPath();
			m.Path.AddPolygon(p);
			m.Path.CloseFigure();

			Brush br = m.GetBrush(xmid, ymid, radius, cos, sin);
			g.FillPath(br, m.Path);

			Pen pen = new Pen(m.BorderColor, m.BorderSize);
			g.DrawPath(pen, m.Path);
			pen.Dispose();
		}

		private float CalcMouseAngle(MouseEventArgs e)
        {
			float x = (float)e.X;
			float y = (float)e.Y;

			float dx =   x - xmid;
			float dy = -(y - ymid);
			float ang = RADInv * (float)Math.Acos(dx / Math.Sqrt(dx*dx + dy*dy));
			if (dy < 0.0F)
			{
				ang = 360.0F - ang;
			}

			return ang;
		}

		internal enum MouseMode { CannotDrag, CanDrag, Dragging };

		private MouseMode mouseMode;
		internal MouseMode CurMouseMode
		{
			get { return mouseMode; }
		}

		private MarkerSet mouseMarkerSet;
		// Allows a marker set to determine if it is being dragged (to prevent certain programatic angle changes
		// while being dragged).
		internal MarkerSet MouseMarkerSet
		{
			get { return mouseMarkerSet; }
		}

		private float mouseAngle;
		private float mouseAngleOffset;

		private void UpdateCursor()
		{
			if (mouseMode == MouseMode.CanDrag)
			{
				this.Cursor = CursorCanDrag;
			}
			else if (mouseMode == MouseMode.Dragging)
			{
				this.Cursor = CursorDragging;
			}
			else
			{
				this.Cursor = CursorCannotDrag;
			}
		}

		private MarkerSet FindMarkerSetUnderMouse(Point mouseLocation, MouseButtons mouseButtons)
		{
			for (int i = 0; i < markerSets.Count; i++)
			{
				MarkerSet ms = markerSets[i];
				for (int j = 0; j < ms.Count; j++)
				{
					Marker m = ms[j];
					if (   (m.DragButtons & mouseButtons) != MouseButtons.None
						&& m.Path != null && m.Path.IsVisible(mouseLocation.X, mouseLocation.Y))
					{
						return ms;
					}
				}
			}
			return null;
		}

        private void CircleControl_MouseDown(object sender, MouseEventArgs e)
        {
			mouseMarkerSet = FindMarkerSetUnderMouse(e.Location, e.Button);

			if (mouseMarkerSet != null)
			{
				mouseAngle = CalcMouseAngle(e);
				mouseAngleOffset = mouseMarkerSet.Angle - mouseAngle;
				mouseMode = MouseMode.Dragging;
				mouseMarkerSet.SetAngle(mouseMarkerSet.Angle, AngleChangedArgs.MouseState.Down);
//				SetAngle(mouseMarkerSet, mouseMarkerSet.Angle, 0.0f, AngleChangedArgs.MouseState.Down);
			}
			else
			{
				mouseMode = MouseMode.CannotDrag;
			}
			UpdateCursor();
        }

        private void CircleControl_MouseUp(object sender, MouseEventArgs e)
        {
			if (mouseMode == MouseMode.Dragging)
			{
				mouseMarkerSet.SetAngle(mouseMarkerSet.Angle, AngleChangedArgs.MouseState.Up);
//				SetAngle(mouseMarkerSet, mouseMarkerSet.Angle, 0.0f, AngleChangedArgs.MouseState.Up);
				mouseMode = MouseMode.CanDrag;
				UpdateCursor();
				mouseMarkerSet = null;
			}
        }

        private void CircleControl_MouseMove(object sender, MouseEventArgs e)
        {
			if (mouseMode == MouseMode.Dragging)
			{
				float newMouseAngle = CalcMouseAngle(e);
				float angleChange = newMouseAngle - mouseAngle;

				// If change is positive, motion is CCW, if negative, motion is CW
				if (angleChange >= 180.0f)
				{
					angleChange %= 360.0f;
					if (angleChange >= 180.0f)
					{
						angleChange -= 360.0f;
					}
				}
				else if (angleChange <= -180.0f)
				{
					angleChange %= 360.0f;
					if (angleChange <= -180.0f)
					{
						angleChange += 360.0f;
					}
				}

				mouseAngle += angleChange;

				float markerAngle = mouseAngle + mouseAngleOffset;

				mouseMarkerSet.SetAngle(markerAngle, AngleChangedArgs.MouseState.Dragging);

/*				if (!angleWraps)
				{
					if (markerAngle < angleMin)
					{
						angleChange += angleMin - markerAngle;
						markerAngle = angleMin;
					}
					else if (markerAngle > angleMax)
					{
						angleChange += angleMax - markerAngle;
						markerAngle = angleMax;
					}
				}

				SetAngle(mouseMarkerSet, markerAngle, angleChange, AngleChangedArgs.MouseState.Dragging); */
			}
			else if (FindMarkerSetUnderMouse(e.Location, MouseButtons.Left|MouseButtons.Middle|MouseButtons.Right) != null)
			{
				mouseMode = MouseMode.CanDrag;
			}
			else
			{
				mouseMode = MouseMode.CannotDrag;
			}
			UpdateCursor();
        }
    }
}
