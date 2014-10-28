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
        /// 	Represents the collection of <c>TextItem</c> objects in a <c>CircleControl</c> control.
        /// </summary>
		public class TextItemCollection : Collection<TextItem>
		{
			internal TextItemCollection(CircleControl cc) : base(cc)
			{ }

			/// <summary>
			/// 	Initializes a new instance of the <c>TextItem</c> with fixed size, unrotated text, and adds it to this <c>TextItemCollection</c>.
			/// </summary>
		    /// <param name="font"><c>System.Drawing.Font</c> that defines the format of the text.</param>
		    /// <param name="brush"><c>System.Drawing.Brush</c> that determines the color and texture of the text.</param>
		    /// <param name="text">Text to draw.</param>
		    /// <param name="position">The distance from the center of the control to the center of the text.</param>
		    /// <param name="angle">The angular location of the center of the text.</param>
			/// <remarks>
			///		Refer to <c>TextItem</c> for an explanation of fixed vs variable size text.
			///	</remarks>
			public void Add(Font font, Brush brush, string text, float position, float angle)
			{
				Add(new TextItem(font, brush, text, position, angle));
			}

			/// <summary>
		    /// 	Initializes a new instance of the <c>TextItem</c> class with fixed size text, and adds it to this <c>TextItemCollection</c>.
			/// </summary>
		    /// <param name="font"><c>System.Drawing.Font</c> that defines the format of the text.</param>
		    /// <param name="brush"><c>System.Drawing.Brush</c> that determines the color and texture of the text.</param>
		    /// <param name="text">Text to draw.</param>
		    /// <param name="position">The distance from the center of the control to the center of the text.</param>
		    /// <param name="angle">The angular location of the center of the text.</param>
			/// <param name="rotation">Text rotation in degrees.</param>
			/// <remarks>
			///		Refer to <c>TextItem</c> for an explanation of fixed vs variable size text.
			///	</remarks>
			public void Add(Font font, Brush brush, string text, float position, float angle, float rotation)
			{
				Add(new TextItem(font, brush, text, position, angle, rotation));
			}

			/// <summary>
		    /// 	Initializes a new instance of the <c>TextItem</c> class with variable size, unrotated text, and adds it to this <c>TextItemCollection</c>.
			/// </summary>
		    /// <param name="font"><c>System.Drawing.Font</c> that defines the format of the text.</param>
		    /// <param name="brush"><c>System.Drawing.Brush</c> that determines the color and texture of the text.</param>
			/// <param name="size">Size (height) of the text, where 1.0 is the distance from the center of the control to the nearest edge.  This value overrides the size specified in <paramref name="font" />.</param>
		    /// <param name="text">Text to draw.</param>
		    /// <param name="position">The distance from the center of the control to the center of the text.</param>
		    /// <param name="angle">The angular location of the center of the text.</param>
			/// <remarks>
			///		Refer to <c>TextItem</c> for an explanation of fixed vs variable size text.
			///	</remarks>
			public void Add(Font font, Brush brush, float size, string text, float position, float angle)
			{
				Add(new TextItem(font, brush, size, text, position, angle));
			}

			/// <summary>
		    /// 	Initializes a new instance of the <c>TextItem</c> class with variable size text, and adds it to this <c>TextItemCollection</c>.
			/// </summary>
		    /// <param name="font"><c>System.Drawing.Font</c> that defines the format of the text.</param>
		    /// <param name="brush"><c>System.Drawing.Brush</c> that determines the color and texture of the text.</param>
			/// <param name="size">Size (height) of the text, where 1.0 is the distance from the center of the control to the nearest edge.  This value overrides the size specified in <paramref name="font" />.</param>
		    /// <param name="text">Text to draw.</param>
		    /// <param name="position">The distance from the center of the control to the center of the text.</param>
		    /// <param name="angle">The angular location of the center of the text.</param>
			/// <param name="rotation">Text rotation in degrees.</param>
			/// <remarks>
			///		Refer to <c>TextItem</c> for an explanation of fixed vs variable size text.
			///	</remarks>
			public void Add(Font font, Brush brush, float size, string text, float position, float angle, float rotation)
			{
				Add(new TextItem(font, brush, size, text, position, angle, rotation));
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>TextItem</c> class with fixed size, unrotated text, drawn with a solid color, and adds it to this <c>TextItemCollection</c>.
			/// </summary>
		    /// <param name="font"><c>System.Drawing.Font</c> that defines the format of the text.</param>
			/// <param name="solidColor">Text color.</param>
		    /// <param name="text">Text to draw.</param>
		    /// <param name="position">The distance from the center of the control to the center of the text.</param>
		    /// <param name="angle">The angular location of the center of the text.</param>
			/// <remarks>
			///		Refer to <c>TextItem</c> for an explanation of fixed vs variable size text.
			///	</remarks>
			public void Add(Font font, Color solidColor, string text, float position, float angle)
			{
				Add(new TextItem(font, solidColor, text, position, angle));
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>TextItem</c> class with fixed size text, drawn in a solid color, and adds it to this <c>TextItemCollection</c>.
			/// </summary>
		    /// <param name="font"><c>System.Drawing.Font</c> that defines the format of the text.</param>
			/// <param name="solidColor">Text color.</param>
		    /// <param name="text">Text to draw.</param>
		    /// <param name="position">The distance from the center of the control to the center of the text.</param>
		    /// <param name="angle">The angular location of the center of the text.</param>
			/// <param name="rotation">Text rotation in degrees.</param>
			/// <remarks>
			///		Refer to <c>TextItem</c> for an explanation of fixed vs variable size text.
			///	</remarks>
			public void Add(Font font, Color solidColor, string text, float position, float angle, float rotation)
			{
				Add(new TextItem(font, solidColor, text, position, angle, rotation));
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>TextItem</c> class with variable size, unrotated text, drawn in a solid color, and adds it to this <c>TextItemCollection</c>.
			/// </summary>
		    /// <param name="font"><c>System.Drawing.Font</c> that defines the format of the text.</param>
			/// <param name="solidColor">Text color.</param>
			/// <param name="size">Size (height) of the text, where 1.0 is the distance from the center of the control to the nearest edge.  This value overrides the size specified in <paramref name="font" />.</param>
		    /// <param name="text">Text to draw.</param>
		    /// <param name="position">The distance from the center of the control to the center of the text.</param>
		    /// <param name="angle">The angular location of the center of the text.</param>
			/// <remarks>
			///		Refer to <c>TextItem</c> for an explanation of fixed vs variable size text.
			///	</remarks>
			public void Add(Font font, Color solidColor, float size, string text, float position, float angle)
			{
				Add(new TextItem(font, solidColor, size, text, position, angle));
			}

			/// <summary>
			/// 	Initializes a new instance of the <c>TextItem</c> class with variable size, drawn in a solid color, and adds it to this <c>TextItemCollection</c>.
			/// </summary>
		    /// <param name="font"><c>System.Drawing.Font</c> that defines the format of the text.</param>
			/// <param name="solidColor">Text color.</param>
			/// <param name="size">Size (height) of the text, where 1.0 is the distance from the center of the control to the nearest edge.  This value overrides the size specified in <paramref name="font" />.</param>
		    /// <param name="text">Text to draw.</param>
		    /// <param name="position">The distance from the center of the control to the center of the text.</param>
		    /// <param name="angle">The angular location of the center of the text.</param>
			/// <param name="rotation">Text rotation in degrees.</param>
			/// <remarks>
			///		Refer to <c>TextItem</c> for an explanation of fixed vs variable size text.
			///	</remarks>
			public void Add(Font font, Color solidColor, float size, string text, float position, float angle, float rotation)
			{
				Add(new TextItem(font, solidColor, size, text, position, angle, rotation));
			}
		}
	}
}
