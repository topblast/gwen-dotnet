﻿using System;
using System.Drawing;
using Gwen.ControlsInternal;

namespace Gwen.Controls
{
    /// <summary>
    /// Static label.
    /// </summary>
    public class Label : Base
    {
        protected readonly Text m_Text;
        protected Pos m_Align;
        protected Padding m_TextPadding;

        /// <summary>
        /// Text alignment.
        /// </summary>
        public Pos Alignment { get { return m_Align; } set { m_Align = value; Invalidate(); } }

        /// <summary>
        /// Text.
        /// </summary>
        public String Text { get { return m_Text.String; } set { SetText(value); } }

        /// <summary>
        /// Font.
        /// </summary>
        public Font Font
        {
            get { return m_Text.Font; }
            set
            {
                m_Text.Font = value;
                if (AutoSizeToContents)
                    SizeToContents();
                Redraw();
            }
        }
        
        /// <summary>
        /// Text color.
        /// </summary>
        public Color TextColor { get { return m_Text.TextColor; } set { m_Text.TextColor = value; } }

        /// <summary>
        /// Override text color (used by tooltips).
        /// </summary>
        public Color TextColorOverride { get { return m_Text.TextColorOverride; } set { m_Text.TextColorOverride = value; } }

        /// <summary>
        /// Width of the text (in pixels).
        /// </summary>
        public int TextWidth { get { return m_Text.Width; } }

        /// <summary>
        /// Height of the text (in pixels).
        /// </summary>
        public int TextHeight { get { return m_Text.Height; } }

        public int TextX { get { return m_Text.X; } }
        public int TextY { get { return m_Text.Y; } }

        /// <summary>
        /// Text length (in characters).
        /// </summary>
        public int TextLength { get { return m_Text.Length; } }
        public int TextRight { get { return m_Text.Right; } }
        public virtual void MakeColorNormal() { TextColor = Skin.Colors.Label.Default; }
        public virtual void MakeColorBright() { TextColor = Skin.Colors.Label.Bright; }
        public virtual void MakeColorDark() { TextColor = Skin.Colors.Label.Dark; }
        public virtual void MakeColorHighlight() { TextColor = Skin.Colors.Label.Highlight; }

        /// <summary>
        /// Determines if the control should autosize to it's text.
        /// </summary>
        public bool AutoSizeToContents { get { return m_Text.AutoSizeToContents; } set { m_Text.AutoSizeToContents = value; } }

        /// <summary>
        /// Text padding.
        /// </summary>
        public Padding TextPadding { get { return m_TextPadding; } set { m_TextPadding = value; Invalidate(); InvalidateParent(); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="parent">Parent control.</param>
        public Label(Base parent) : base(parent)
        {
            m_Text = new Text(this);
            //m_Text.Font = Skin.DefaultFont;

            MouseInputEnabled = false;
            SetBounds(0, 0, 100, 10);
            Alignment = Pos.Left | Pos.Top;

            AutoSizeToContents = false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            m_Text.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Handler for text changed event.
        /// </summary>
        protected virtual void onTextChanged()
        {}

        /// <summary>
        /// Lays out the control's interior according to alignment, padding, dock etc.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Layout(Skin.Base skin)
        {
            base.Layout(skin);

            Pos align = m_Align;

            int x = m_TextPadding.Left + m_Padding.Left;
            int y = m_TextPadding.Top + m_Padding.Top;

            if (align.HasFlag(Pos.Right)) 
                x = Width - m_Text.Width - m_TextPadding.Right - m_Padding.Right;
            if (align.HasFlag(Pos.CenterH))
                x = Global.Trunc((m_TextPadding.Left + m_Padding.Left) + ((Width - m_Text.Width - m_TextPadding.Left - m_Padding.Left - m_TextPadding.Right - m_Padding.Right) * 0.5f));

            if (align.HasFlag(Pos.CenterV))
                y = Global.Trunc((m_TextPadding.Top + m_Padding.Top) + ((Height - m_Text.Height) * 0.5f) - m_TextPadding.Bottom - m_Padding.Bottom);
            if (align.HasFlag(Pos.Bottom)) 
                y = Height - m_Text.Height - m_TextPadding.Bottom - m_Padding.Bottom;

            m_Text.SetPos(x, y);
        }

        /// <summary>
        /// Sets the label text.
        /// </summary>
        /// <param name="str">Text to set.</param>
        /// <param name="doEvents">Determines whether to invoke "text changed" event.</param>
        public virtual void SetText(String str, bool doEvents = true)
        {
            if (Text == str)
                return;

            m_Text.String = str;
            if (AutoSizeToContents)
                SizeToContents();
            Redraw();

            if (doEvents)
                onTextChanged();
        }

        public virtual void SizeToContents()
        {
            m_Text.SetPos(m_TextPadding.Left + m_Padding.Left, m_TextPadding.Top + m_Padding.Top);
            m_Text.RefreshSize();

            SetSize(m_Text.Width + m_Padding.Left + m_Padding.Right + m_TextPadding.Left + m_TextPadding.Right, 
                m_Text.Height + m_Padding.Top + m_Padding.Bottom + m_TextPadding.Top + m_TextPadding.Bottom);
        }

        /// <summary>
        /// Gets the coordinates of specified character.
        /// </summary>
        /// <param name="index">Character index.</param>
        /// <returns>Character coordinates (local).</returns>
        public virtual Point GetCharacterPosition(int index)
        {
            Point p = m_Text.GetCharacterPosition(index);
            return new Point(p.X + m_Text.X, p.Y + m_Text.Y);
        }

        /// <summary>
        /// Renders the control using specified skin.
        /// </summary>
        /// <param name="skin">Skin to use.</param>
        protected override void Render(Skin.Base skin)
        {
        }
    }
}
