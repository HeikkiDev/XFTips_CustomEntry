using XFTips.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace XFTips.CustomControls
{
    /// <summary>
    /// Custom control heredado de Entry.
    /// Usa Renderers para permitir definir el color del borde del Entry,
    /// y para definir el tipo del botón 'Return' del teclado y controlar su click
    /// </summary>
    public class CustomEntry : Entry
    {
        public const string ReturnKeyPropertyName = nameof(ReturnKeyType);

        // Override del handler de Completed porque no se ejecuta solo, aunque no se por qué :(
        public new event EventHandler<EventArgs> Completed;

        public CustomEntry()
        {
            this.Completed += CustomEntry_Completed;
        }

        ~CustomEntry()
        {
            this.Completed -= CustomEntry_Completed;
        }

        private void CustomEntry_Completed(object sender, EventArgs e)
        {
            var entry = ((CustomEntry)sender);
            if ( entry != null)
            {
                switch (entry.ReturnKeyType)
                {
                    case ReturnKeyTypes.Next:
                        if(entry.NextEntry != null)
                            entry.NextEntry.Focus();
                        break;
                    case ReturnKeyTypes.Done:
                        if (entry.DoneButton != null && entry.DoneButton.Command != null && entry.DoneButton.Command.CanExecute(true))
                            entry.DoneButton.Command.Execute(true);
                        break;
                    case ReturnKeyTypes.Go:
                        //TODO
                        break;
                    case ReturnKeyTypes.Google:
                        //TODO
                        break;
                    case ReturnKeyTypes.Search:
                        //TODO
                        break;
                    case ReturnKeyTypes.Send:
                        //TODO
                        break;
                    case ReturnKeyTypes.Continue:
                        //TODO
                        break;
                    case ReturnKeyTypes.Default:
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Invoca el evento Completer cuando el botón 'Return' del teclado es pulsado
        /// </summary>
        public void InvokeCompleted()
        {
            if (this.Completed != null)
                this.Completed?.Invoke(this, null);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            propertyName: nameof(BorderColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEntry),
            defaultValue: Color.DarkGray);

        /// <summary>
        /// Color del borde del Entry
        /// </summary>
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set {
                SetValue(BorderColorProperty, value);
            }
        }

        public static readonly BindableProperty DoneButtonProperty = BindableProperty.Create(
            propertyName: nameof(DoneButton),
            returnType: typeof(Button),
            declaringType: typeof(CustomEntry),
            defaultValue: null);

        /// <summary>
        /// Referencia a un botón Save, Done, etc. del que se ejecutará su Command
        /// </summary>
        public Button DoneButton
        {
            get { return (Button)GetValue(DoneButtonProperty); }
            set { SetValue(DoneButtonProperty, value); }
        }

        public static readonly BindableProperty NextEntryProperty = BindableProperty.Create(
            propertyName: nameof(NextEntry),
            returnType: typeof(Entry),
            declaringType: typeof(CustomEntry),
            defaultValue: null);

        /// <summary>
        /// Referencia al siguiente Entry al que mandar el foco
        /// </summary>
        public Entry NextEntry
        {
            get { return (Entry)GetValue(NextEntryProperty); }
            set { SetValue(NextEntryProperty, value); }
        }

        public static readonly BindableProperty ReturnKeyTypeProperty = BindableProperty.Create(
            propertyName: nameof(ReturnKeyType),
            returnType: typeof(ReturnKeyTypes),
            declaringType: typeof(CustomEntry),
            defaultValue: ReturnKeyTypes.Done);

        /// <summary>
        /// Tipo del botón 'Return' del teclado
        /// </summary>
        public ReturnKeyTypes ReturnKeyType
        {
            get { return (ReturnKeyTypes)GetValue(ReturnKeyTypeProperty); }
            set { SetValue(ReturnKeyTypeProperty, value); }
        }
    }
}
