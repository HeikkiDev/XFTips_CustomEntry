using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using XFTips.Constants;
using XFTips.CustomControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(XFTips.iOS.CustomEntryRenderer))]
namespace XFTips.iOS
{
    public class CustomEntryRenderer : EntryRenderer
    {
        ~CustomEntryRenderer()
        {
            if(Control != null)
                Control.ShouldReturn -= TextFieldShouldReturn;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            // Para cambiar el botón Return del teclado
            if ((Control != null) && (e.NewElement != null))
            {
                Control.ReturnKeyType = (e.NewElement as CustomEntry).ReturnKeyType.GetValueFromDescription();
                Control.ShouldReturn -= TextFieldShouldReturn;
                Control.ShouldReturn += TextFieldShouldReturn;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || Element == null) return;

            // Para cambiar el botón Return del teclado
            if (e.PropertyName == CustomEntry.ReturnKeyPropertyName)
            {
                Control.ReturnKeyType = (sender as CustomEntry).ReturnKeyType.GetValueFromDescription();
                Control.ShouldReturn -= TextFieldShouldReturn;
                Control.ShouldReturn += TextFieldShouldReturn;
            }

            // Para definir el color del borde del Entry
            if (e.PropertyName == CustomEntry.BorderColorProperty.PropertyName)
            {
                var customColor = (sender as CustomEntry).BorderColor.ToCGColor();

                Control.Layer.BorderColor = customColor;
                Control.Layer.BorderWidth = 1;
            }
        }

        // Handler del click en el botón Return del teclado
        private bool TextFieldShouldReturn(UITextField textField)
        {
            CustomEntry customEntry = (CustomEntry)Element;

            if (customEntry?.ReturnKeyType != ReturnKeyTypes.Next)
                customEntry?.Unfocus();

            customEntry?.InvokeCompleted();

            return true;
        }
    }

    public static class EnumExtensions
    {
        public static UIReturnKeyType GetValueFromDescription(this ReturnKeyTypes value)
        {
            var type = typeof(UIReturnKeyType);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value.ToString())
                        return (UIReturnKeyType)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return (UIReturnKeyType)field.GetValue(null);
                }
            }
            throw new NotSupportedException($"Not supported on iOS: {value}");
        }
    }
}