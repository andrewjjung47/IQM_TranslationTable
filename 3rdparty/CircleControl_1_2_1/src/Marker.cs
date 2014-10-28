using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GAW
{
    partial class CircleControl
    {
		/// <summary>
		/// 	Represents a marker object.
		/// </summary>
		public class Marker
		{
			internal Marker(Color borderColor, float borderSize, PointF[] points, float offsetAngle, MouseButtons dragButtons, bool visible)
			{
				CheckPoints(points);

				this.borderColor = borderColor;
				this.borderSize = borderSize;
				this.points = new PointF[points.Length];
				points.CopyTo(this.points, 0);

				this.offsetAngle = CircleControl.AdjustOffsetAngle(offsetAngle);
				this.dragButtons = dragButtons;
				this.visible = visible;

				this.gradientMode = LinearGradientMode.Vertical;
				this.hatchStyle = HatchStyle.Cross;
				this.color1 = Color.Black;
				this.color2 = Color.White;
				this.point1 = Point.Empty;
				this.point2 = Point.Empty;

				this.ms = null;
				this.path = null;
				this.brush = null;
			}

			/// <summary>
			/// 	Specifies how a <c>Marker</c> is filled.
			/// </summary>
			public enum BrushMode
			{
			    /// <summary>
			    /// 	Specifies a solid fill.
			    /// </summary>
				Solid,
				
				/// <summary>
				/// 	Specifies a radial gradient fill.
				/// 	The coloring of the marker does not change when the marker changes position.
				/// </summary>
				RadialGradient,
				
				/// <summary>
				/// 	Specifies a linear gradient fill.
				/// 	The gradient is spread across the client area of the control, so the coloring of the marker
				/// 	changes as the marker changes position.
				/// </summary>
				LinearGradient,
				
				/// <summary>
				/// 	Specifies a hatch fill.
				/// </summary>
				Hatch
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>Marker</c> class with a solid color at offset angle 0.0.
			/// 	The marker can be dragged with the left mouse button and is visible.
			/// </summary>
			/// <param name="solidColor">Internal color.</param>
			/// <param name="borderColor">Border color.</param>
			/// <param name="borderSize">Border thickness (in pixels).</param>
			/// <param name="points">Array of <c>System.Drawing.PointF</c> structures that represent the vertices of the marker when the marker is at zero degrees.</param>
			/// <remarks>
			///		Refer to the Overview for a discussion of the coordinate system used by <c>points</c>.
			///	</remarks>
			public Marker(Color solidColor, Color borderColor, float borderSize, PointF[] points)
						: this(solidColor, borderColor, borderSize, points, 0.0f, MouseButtons.Left, true)
			{
				this.brushMode = BrushMode.Solid;
				this.color1 = solidColor;
				this.color2 = solidColor;
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>Marker</c> class with a solid color.
			/// 	The marker is visible.
			/// </summary>
			/// <param name="solidColor">Internal color.</param>
			/// <param name="borderColor">Border color.</param>
			/// <param name="borderSize">Border thickness (in pixels).</param>
			/// <param name="points">Array of <c>System.Drawing.PointF</c> structures that represent the vertices of the marker when the marker is at zero degrees.</param>
			/// <param name="offsetAngle">Offset angle of marker within <c>MarkerSet</c>.</param>
			/// <param name="dragButtons"><c>System.Windows.Forms.MouseButtons</c> value indicating which mouse button(s) drag this marker.  If the value is <c>MouseButtons.None</c>, then this marker cannot be dragged.</param>
			/// <remarks>
			///		Refer to the Overview for a discussion of the coordinate system used by <c>points</c>.
			/// </remarks>
			public Marker(Color solidColor, Color borderColor, float borderSize, PointF[] points, float offsetAngle, MouseButtons dragButtons)
						: this(borderColor, borderSize, points, offsetAngle, dragButtons, true)
			{
				this.brushMode = BrushMode.Solid;
				this.color1 = solidColor;
				this.color2 = solidColor;
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>Marker</c> class with a solid color.
			/// </summary>
			/// <param name="solidColor">Internal color.</param>
			/// <param name="borderColor">Border color.</param>
			/// <param name="borderSize">Border thickness (in pixels).</param>
			/// <param name="points">Array of <c>System.Drawing.PointF</c> structures that represent the vertices of the marker when the marker is at zero degrees.</param>
			/// <param name="offsetAngle">Offset angle of marker within <c>MarkerSet</c>.</param>
			/// <param name="dragButtons"><c>System.Windows.Forms.MouseButtons</c> value indicating which mouse button(s) drag this marker.  If the value is <c>MouseButtons.None</c>, then this marker cannot be dragged.</param>
			/// <param name="visible"><c>True</c> if the marker is visible, <c>false</c> otherwise.</param>
			/// <remarks>
			///		Refer to the Overview for a discussion of the coordinate system used by <c>points</c>.
			/// </remarks>
			public Marker(Color solidColor, Color borderColor, float borderSize, PointF[] points, float offsetAngle, MouseButtons dragButtons, bool visible)
						: this(borderColor, borderSize, points, offsetAngle, dragButtons, visible)
			{
				this.brushMode = BrushMode.Solid;
				this.color1 = solidColor;
				this.color2 = solidColor;
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>Marker</c> class with radial gradient coloring.
			/// </summary>
			/// <param name="startPoint">Starting point of gradient.</param>
			/// <param name="endPoint">Ending point of gradient.</param>
			/// <param name="startColor">Color of marker at starting point of gradient.</param>
			/// <param name="endColor">Color of marker at ending point of gradient.</param>
			/// <param name="borderColor">Border color.</param>
			/// <param name="borderSize">Border thickness (in pixels).</param>
			/// <param name="points">Array of <c>System.Drawing.PointF</c> structures that represent the vertices of the marker when the marker is at zero degrees.</param>
			/// <param name="offsetAngle">Offset angle of marker within <c>MarkerSet</c>.</param>
			/// <param name="dragButtons"><c>System.Windows.Forms.MouseButtons</c> value indicating which mouse button(s) drag this marker.  If the value is <c>MouseButtons.None</c>, then this marker cannot be dragged.</param>
			/// <param name="visible"><c>True</c> if the marker is visible, <c>false</c> otherwise.</param>
			/// <remarks>
			///		Refer to the Overview for a discussion of the coordinate system used by <c>startPoint</c>, <c>endPoint</c>, and <c>points</c>.
			/// </remarks>
			public Marker(PointF startPoint, PointF endPoint, Color startColor, Color endColor, Color borderColor, float borderSize, PointF[] points, float offsetAngle, MouseButtons dragButtons, bool visible)
						: this(borderColor, borderSize, points, offsetAngle, dragButtons, visible)
			{
				this.brushMode = BrushMode.RadialGradient;
				this.color1 = startColor;
				this.color2 = endColor;
				this.point1 = startPoint;
				this.point2 = endPoint;
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>Marker</c> class with linear gradient coloring.
			/// </summary>
			/// <param name="gradientMode">A <c>System.Drawing.Drawing2D.LinearGradientMode</c> enumeration value that specifies the orientation of the gradient.</param>
			/// <param name="startColor">Starting color.</param>
			/// <param name="endColor">Ending color.</param>
			/// <param name="borderColor">Border color.</param>
			/// <param name="borderSize">Border thickness (in pixels).</param>
			/// <param name="points">Array of <c>System.Drawing.PointF</c> structures that represent the vertices of the marker when the marker is at zero degrees.</param>
			/// <param name="offsetAngle">Offset angle of marker within <c>MarkerSet</c>.</param>
			/// <param name="dragButtons"><c>System.Windows.Forms.MouseButtons</c> value indicating which mouse button(s) drag this marker.  If the value is <c>MouseButtons.None</c>, then this marker cannot be dragged.</param>
			/// <param name="visible"><c>True</c> if the marker is visible, <c>false</c> otherwise.</param>
			/// <remarks>
			///		Refer to the Overview for a discussion of the coordinate system used by <c>points</c>.
			/// </remarks>
			public Marker(LinearGradientMode gradientMode, Color startColor, Color endColor, Color borderColor, float borderSize, PointF[] points, float offsetAngle, MouseButtons dragButtons, bool visible)
						: this(borderColor, borderSize, points, offsetAngle, dragButtons, visible)
			{
				this.brushMode = BrushMode.LinearGradient;
				this.gradientMode = gradientMode;
				this.color1 = startColor;
				this.color2 = endColor;
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>Marker</c> class with a hatch pattern.
			/// </summary>
			/// <param name="hatchStyle">A <c>System.Drawing.Drawing2D.HatchStyle</c> enumeration value that specifies the style of hatching.</param>
			/// <param name="foreColor">Hatch lines color.</param>
			/// <param name="backColor">Background color.</param>
			/// <param name="borderColor">Border color.</param>
			/// <param name="borderSize">Border thickness (in pixels).</param>
			/// <param name="points">Array of <c>System.Drawing.PointF</c> structures that represent the vertices of the marker when the marker is at zero degrees.</param>
			/// <param name="offsetAngle">Offset angle of marker within <c>MarkerSet</c>.</param>
			/// <param name="dragButtons"><c>System.Windows.Forms.MouseButtons</c> value indicating which mouse button(s) drag this marker.  If the value is <c>MouseButtons.None</c>, then this marker cannot be dragged.</param>
			/// <param name="visible"><c>True</c> if the marker is visible, <c>false</c> otherwise.</param>
			/// <remarks>
			///		Refer to the Overview for a discussion of the coordinate system used by <c>points</c>.
			/// </remarks>
			public Marker(HatchStyle hatchStyle, Color foreColor, Color backColor, Color borderColor, float borderSize, PointF[] points, float offsetAngle, MouseButtons dragButtons, bool visible)
						: this(borderColor, borderSize, points, offsetAngle, dragButtons, visible)
			{
				this.brushMode = BrushMode.Hatch;
				this.hatchStyle = hatchStyle;
				this.color1 = foreColor;
				this.color2 = backColor;
			}

			/// <summary>
			/// 	Produces a copy of this <c>Marker</c>.
			/// 	The new instance does not belong to a <c>MarkerSet</c> or a <c>CircleControl</c>.
			/// </summary>
			/// <returns>
			///		A new <c>Marker</c>.
			/// </returns>
			public Marker Clone()
			{
				Marker m = new Marker(borderColor, borderSize, points, offsetAngle, dragButtons, visible);

				m.brushMode = brushMode;
				m.gradientMode = gradientMode;
				m.hatchStyle = hatchStyle;
				m.point1 = point1;
				m.point2 = point2;
				m.color1 = color1;
				m.color2 = color2;

				return m;
			}

			private MarkerSet ms;
			/// <summary>
			/// 	Gets the <c>MarkerSet</c> to which this marker belongs.
			/// </summary>
			/// <value>
			/// 	The <c>MarkerSet</c> to which this marker belongs.
			/// </value>
			public MarkerSet Ms
			{
				[DebuggerStepThrough]
				get { return ms; }
				internal set { ms = value; }
			}

			/// <summary>
			/// 	Gets the <c>CircleControl</c> to which this marker belongs.
			/// </summary>
			/// <value>
			/// 	The <c>CircleControl</c> to which this marker belongs.
			/// </value>
			public CircleControl Cc
			{
				[DebuggerStepThrough]
				get { return (ms == null) ? null : ms.Cc; }
			}

			private void Redraw()
			{
				if (visible)
				{
					Redraw2();
				}
			}

			private void Redraw2()
			{
				if (Cc != null)
				{
					Cc.Redraw();
				}
			}

			private Brush brush;

			// Internal function for generating a brush
			// These parameters are only required for the radial gradient brush
			//   - there has to be a cleaner way than this...
			internal Brush GetBrush(float xmid, float ymid,
									float radius, float cos, float sin)
			{
				switch (brushMode)
				{
					case BrushMode.Solid : 
					{
						if (brush == null)
						{
							brush = new SolidBrush(color1);
						}
						break;
					}
					case BrushMode.RadialGradient :
					{
						ClearBrush();

						PointF p1 = new PointF(xmid + radius * point1.X * cos - radius * point1.Y * sin,
											   ymid - radius * point1.X * sin - radius * point1.Y * cos);
						PointF p2 = new PointF(xmid + radius * point2.X * cos - radius * point2.Y * sin,
											   ymid - radius * point2.X * sin - radius * point2.Y * cos);

						brush = new LinearGradientBrush(p1, p2, color1, color2);
						break;
					}
					case BrushMode.LinearGradient :
					{
						ClearBrush();
                        brush = new LinearGradientBrush(Cc.ClientRectangle, color1, color2, gradientMode);
						break;
					}
					case BrushMode.Hatch :
					{
						if (brush == null)
						{
							brush = new HatchBrush(hatchStyle, color1, color2);
						}
						break;
					}
				}
				return brush;
			}

			private void ClearBrush()
			{
				if (brush != null)
				{
					brush.Dispose();
					brush = null;
				}
			}

			private BrushMode brushMode;
			/// <summary>
			/// 	Gets or sets the <c>BrushMode</c>.
			/// </summary>
			/// <value>
			/// 	One of the <c>BrushMode</c> values.
			/// </value>
			public BrushMode MarkerBrushMode
			{
				get { return brushMode; }
				set
				{
					if (value != brushMode)
					{
						brushMode = value;
						ClearBrush();
						Redraw();
					}
				}
			}

			private LinearGradientMode gradientMode;
			/// <summary>
			/// 	Gets or sets the <c>LinearGradientMode</c>.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.LinearGradient</c>.
			/// </summary>
			/// <value>
			/// 	One of the <c>LinearGradientMode</c> values.
			/// </value>
			public LinearGradientMode GradientMode
			{
				get { return gradientMode; }
				set
				{
					if (value != gradientMode || brushMode != BrushMode.LinearGradient)
					{
						gradientMode = value;
						brushMode = BrushMode.LinearGradient;
						ClearBrush();
						Redraw();
					}
				}
			}

			private HatchStyle hatchStyle;
			/// <summary>
			/// 	Gets or sets the <c>HatchStyle</c>.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.Hatch</c>.
			/// </summary>
			/// <value>
			/// 	One of the <c>HatchStyle</c> values.
			/// </value>
			public HatchStyle HatchStyle
			{
				get { return hatchStyle; }
				set
				{
					if (value != hatchStyle || brushMode != BrushMode.Hatch)
					{
						hatchStyle = value;
                        brushMode = BrushMode.Hatch;
						ClearBrush();
						Redraw();
					}
				}
			}

            private Color color1;
			private Color color2;

			/// <summary>
			/// 	Gets or sets the solid fill color.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.Solid</c>.
			/// </summary>
			/// <value>
			/// 	The solid fill color.
			/// </value>
			public Color SolidColor
			{
				get { return color1; }
				set
				{
					if (brushMode != BrushMode.Solid || color1.ToArgb() != value.ToArgb())
					{
						color1 = value;
						brushMode = BrushMode.Solid;
						ClearBrush();
						Redraw();
					}
				}
			}

			/// <summary>
			/// 	Gets or sets the color at the starting point of the radial gradient.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.RadialGradient</c>.
			/// </summary>
			/// <value>
			/// 	The radial gradient inner color.
			/// </value>
			public Color RadialGradientStartColor
			{
				get { return color1; }
				set	{ SetRadialGradientColors(value, color2); }
			}
			
			/// <summary>
			/// 	Gets or sets the color at the ending point of the radial gradient.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.RadialGradient</c>.
			/// </summary>
			/// <value>
			/// 	The radial gradient outer color.
			/// </value>
			public Color RadialGradientEndColor
			{
				get { return color2; }
				set	{ SetRadialGradientColors(color1, value); }
			}
			
			/// <summary>
			/// 	Sets the radial gradient starting and ending colors, and forces <c>MarkerBrushMode</c> to <c>BrushMode.RadialGradient</c>.
			/// </summary>
			/// <param name="startColor">Color of marker at starting point of gradient.</param>
			/// <param name="endColor">Color of marker at ending point of gradient.</param>
			public void SetRadialGradientColors(Color startColor, Color endColor)
			{
				if (brushMode != BrushMode.RadialGradient || startColor.ToArgb() != color1.ToArgb() || endColor.ToArgb() != color2.ToArgb())
				{
					color1 = startColor;
					color2 = endColor;
					brushMode = BrushMode.RadialGradient;
					ClearBrush();
					Redraw();
				}
			}

			/// <summary>
			/// 	Gets or sets the linear gradient start color.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.LinearGradient</c>.
			/// </summary>
			/// <value>
			/// 	The linear gradient start color.
			/// </value>
			public Color LinearGradientStartColor
			{
				get { return color1; }
				set	{ SetLinearGradientColors(value, color2); }
			}
			
			/// <summary>
			/// 	Gets or sets the linear gradient end color.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.LinearGradient</c>.
			/// </summary>
			/// <value>
			/// 	The linear gradient end color.
			/// </value>
			public Color LinearGradientEndColor
			{
				get { return color2; }
				set	{ SetLinearGradientColors(color1, value); }
			}

			private PointF point1;
			private PointF point2;

			/// <summary>
			/// 	Gets or sets the starting point of the radial gradient.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.RadialGradient</c>.
			/// </summary>
			/// <value>
			/// 	The starting point of the radial gradient.
			/// </value>
			public PointF RadialGradientStartPoint
			{
				get { return point1; }
				set { SetRadialGradientPoints(value, point2); }
			}

			/// <summary>
			/// 	Gets or sets the ending point of the radial gradient.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.RadialGradient</c>.
			/// </summary>
			/// <value>
			/// 	The ending point of the radial gradient.
			/// </value>
			public PointF RadialGradientEndPoint
			{
				get { return point2; }
				set { SetRadialGradientPoints(point1, value); }
			}

			/// <summary>
			/// 	Sets the starting and ending points of the radial gradient, and forces <c>MarkerBrushMode</c> to <c>BrushMode.RadialGradient</c>.
			/// </summary>
			/// <param name="startPoint">Starting point.</param>
			/// <param name="endPoint">Outer color.</param>
			public void SetRadialGradientPoints(PointF startPoint, PointF endPoint)
			{
				if (brushMode != BrushMode.LinearGradient || this.point1 != startPoint || this.point2 != endPoint)
				{
					this.point1 = startPoint;
					this.point2 = endPoint;
					brushMode = BrushMode.RadialGradient;
					ClearBrush();
					Redraw();
				}
			}
			
			/// <summary>
			/// 	Sets the linear gradient start and end colors, and forces <c>MarkerBrushMode</c> to <c>BrushMode.LinearGradient</c>.
			/// </summary>
			/// <param name="startColor">Start color.</param>
			/// <param name="endColor">End color.</param>
			public void SetLinearGradientColors(Color startColor, Color endColor)
			{
				if (brushMode != BrushMode.LinearGradient || startColor.ToArgb() != color1.ToArgb() || endColor.ToArgb() != color2.ToArgb())
				{
					this.color1 = startColor;
					this.color2 = endColor;
					brushMode = BrushMode.LinearGradient;
					ClearBrush();
					Redraw();
				}
			}

			/// <summary>
			/// 	Gets or sets the hatched foreground color.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.Hatch</c>.
			/// </summary>
			/// <value>
			/// 	The hatched foreground color.
			/// </value>
			public Color HatchForeColor
			{
				get { return color1; }
				set	{ SetHatchColors(value, color2); }
			}
			
			/// <summary>
			/// 	Gets or sets the hatched background color.
			/// 	Setting this value forces <c>MarkerBrushMode</c> to <c>BrushMode.Hatch</c>.
			/// </summary>
			/// <value>
			/// 	The hatched background color.
			/// </value>
			public Color HatchBackColor
			{
				get { return color2; }
				set	{ SetHatchColors(color1, value); }
			}
			
			/// <summary>
			/// 	Sets the hatched foreground and background colors, and forces <c>MarkerBrushMode</c> to <c>BrushMode.Hatch</c>.
			/// </summary>
			/// <param name="foreColor">Foreground color.</param>
			/// <param name="backColor">Background color.</param>
			public void SetHatchColors(Color foreColor, Color backColor)
			{
				if (brushMode != BrushMode.Hatch || foreColor.ToArgb() != color1.ToArgb() || backColor.ToArgb() != color2.ToArgb())
				{
					color1 = foreColor;
					color2 = backColor;
					brushMode = BrushMode.Hatch;
					ClearBrush();
					Redraw();
				}
			}

            private Color borderColor;
            /// <summary>
            /// 	Gets or set the border color.
            /// </summary>
            /// <value>
            /// 	The border color.
            /// </value>
			public Color BorderColor
			{
				get { return borderColor; }
				set
				{
					// Only update if new value is different
					if (borderColor.ToArgb() != value.ToArgb())
					{
						borderColor = value;
						Redraw();
					}
				}
			}

            private float borderSize;
            /// <summary>
            /// 	Gets or sets the border thickness (in pixels).
			/// 	Set to zero for no border.
            /// </summary>
            /// <value>
            /// 	The border thickness (in pixels).
            /// </value>
			public float BorderSize
			{
				get { return borderSize; }
				set
				{
					float v = Math.Max(0.0F, value);
					if (borderSize != v)
					{
						borderSize = v;
						Redraw();
					}
				}
			}

            private PointF[] points;
            /// <summary>
            /// 	Gets or sets the array of points that defines the shape of the marker.
            /// </summary>
            /// <value>
            /// 	The array of <c>System.Drawing.PointF</c> structures tha represent the vertices of the marker when the marker is at zero degrees.
            /// </value>
			public PointF[] Points
			{
				get { return points; }
				set 
				{
					CheckPoints(value);
					points = new PointF[value.Length];
					value.CopyTo(points, 0);
					Redraw();
				}
			}

			private float offsetAngle;
			/// <summary>
			/// 	Gets or sets the offset angle within the <c>MarkerSet</c>.
			/// </summary>
			/// <value>
			/// 	The offset angle within the <c>MarkerSet</c>.
			/// </value>
			public float OffsetAngle
			{
				get { return offsetAngle; }
				set
				{
					float a = CircleControl.AdjustOffsetAngle(value);
					if (a != offsetAngle)
					{
						offsetAngle = a;
						Redraw();
					}
				}
			}

			private MouseButtons dragButtons;
			/// <summary>
			/// 	Gets or sets the <c>System.Windows.Forms.MouseButtons</c> value indicating which mouse button(s) drag this marker.
			///		<para>
			///		If the value is <c>MouseButtons.None</c>, then this marker cannot be dragged.
			///		</para><para>
			/// 	If this marker is currently being dragged when this value is set, it does not take effect until after the marker is released.
			///		</para>
			/// </summary>
			/// <value>
			/// 	The <c>System.Windows.Forms.MouseButtons</c> value indicating which mouse button(s) drag this marker.
			/// </value>
			public MouseButtons DragButtons
			{
				get { return dragButtons; }
				set { dragButtons = value; }
			}

			private bool visible;
			/// <summary>
			/// 	Gets or sets whether this marker is visible.
			/// </summary>
			/// <value>
			/// 	Boolean value indicating whether this marker is visible.
			/// </value>
			public bool Visible
			{
				get { return visible; }
				set
				{
					if (visible != value)
					{
						visible = value;
						Redraw2();
					}
				}
			}

			private GraphicsPath path;
			internal GraphicsPath Path
			{
				get { return path; }
				set
				{
					ClearPath();
					path = value;
				}
			}

			internal void ClearPath()
			{
				if (path != null)
				{
					path.Dispose();
					path = null;
				}
			}
		}
	}
}
