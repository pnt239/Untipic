using System;
using System.ComponentModel;
using System.Drawing;
using Untipic.Presentation;

namespace Untipic.UI.UntiUI.Extensions
{
    /// <summary>
    /// Represents a control that binds multiple editors together as a single composite unit.
    /// </summary>
    [DefaultEvent("ColorChanged")]
    public class ColorEditorManager : Component, IColorEditor
    {
        #region Instance Fields

        private Color _color;

        private ColorEditor _colorEditor;

        private HslColor _hslColor;

        private ColorWheel _wheel;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Color property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler ColorChanged;

        /// <summary>
        /// Occurs when the ColorEditor property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler ColorEditorChanged;

        /// <summary>
        /// Occurs when the Wheel property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler ColorWheelChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the component color.
        /// </summary>
        /// <value>The component color.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Color Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    _hslColor = new HslColor(value);

                    OnColorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the linked <see cref="ColorEditor"/>.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(ColorEditor), null)]
        public virtual ColorEditor ColorEditor
        {
            get { return _colorEditor; }
            set
            {
                if (ColorEditor != value)
                {
                    _colorEditor = value;

                    OnColorEditorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the linked <see cref="ColorWheel"/>.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(typeof(ColorWheel), null)]
        public virtual ColorWheel ColorWheel
        {
            get { return _wheel; }
            set
            {
                if (ColorWheel != value)
                {
                    _wheel = value;

                    OnColorWheelChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the component color as a HSL structure.
        /// </summary>
        /// <value>The component color.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual HslColor HslColor
        {
            get { return _hslColor; }
            set
            {
                if (HslColor != value)
                {
                    _hslColor = value;
                    _color = value.ToRgbColor();

                    OnColorChanged(EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets or sets a value indicating whether updating of linked components is disabled.
        /// </summary>
        /// <value><c>true</c> if updated of linked components is disabled; otherwise, <c>false</c>.</value>
        protected bool LockUpdates { get; set; }

        #endregion

        #region Protected Members

        /// <summary>
        /// Binds events for the specified editor.
        /// </summary>
        /// <param name="control">The <see cref="IColorEditor"/> to bind to.</param>
        protected virtual void BindEvents(IColorEditor control)
        {
            control.ColorChanged += ColorChangedHandler;
        }

        /// <summary>
        /// Raises the <see cref="ColorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnColorChanged(EventArgs e)
        {
            Synchronize(this);

            var handler = ColorChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ColorEditorChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnColorEditorChanged(EventArgs e)
        {
            if (ColorEditor != null)
            {
                BindEvents(ColorEditor);
            }

            var handler = ColorEditorChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ColorWheelChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnColorWheelChanged(EventArgs e)
        {
            if (ColorWheel != null)
            {
                BindEvents(ColorWheel);
            }

            var handler = ColorWheelChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Sets the color of the given editor.
        /// </summary>
        /// <param name="control">The <see cref="IColorEditor"/> to update.</param>
        /// <param name="sender">The <see cref="IColorEditor"/> triggering the update.</param>
        protected virtual void SetColor(IColorEditor control, IColorEditor sender)
        {
            if (control != null && control != sender)
            {
                control.Color = sender.Color;
            }
        }

        /// <summary>
        /// Synchronizes linked components with the specified <see cref="IColorEditor"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IColorEditor"/> triggering the update.</param>
        protected virtual void Synchronize(IColorEditor sender)
        {
            if (!LockUpdates)
            {
                try
                {
                    LockUpdates = true;
                    SetColor(ColorWheel, sender);
                    SetColor(ColorEditor, sender);
                }
                finally
                {
                    LockUpdates = false;
                }
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handler for linked controls.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ColorChangedHandler(object sender, EventArgs e)
        {
            if (!LockUpdates)
            {
                var source = (IColorEditor)sender;

                LockUpdates = true;
                Color = source.Color;
                LockUpdates = false;
                Synchronize(source);
            }
        }

        #endregion
    }
}
