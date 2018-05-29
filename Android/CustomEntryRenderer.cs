using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using XFTips.Constants;
using XFTips.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(XFTips.Droid.CustomEntryRenderer))]
namespace XFTips.Droid
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {

        }

        ~CustomEntryRenderer()
        {
            if (Control != null)
                Control.EditorAction -= Control_EditorAction;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || Element == null || e.OldElement != null) return;

            var customEntry = (e.NewElement as CustomEntry);

            if(customEntry == null) return;

            // Para cambiar el botón Return del teclado
            Control.ImeOptions = customEntry.ReturnKeyType.GetValueFromDescription();
            Control.SetImeActionLabel(customEntry.ReturnKeyType.ToString(), Control.ImeOptions);
            Control.EditorAction -= Control_EditorAction;
            Control.EditorAction += Control_EditorAction;

            // Para definir el color del borde del Entry
            var customColor = customEntry.BorderColor.ToAndroid();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Control.BackgroundTintList = ColorStateList.ValueOf(customColor);
            else
                Control.Background.SetColorFilter(customColor, PorterDuff.Mode.SrcAtop);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null || Element == null) return;

            var customEntry = (e.NewElement as CustomEntry);

            if(customEntry == null) return;

            // Para cambiar el botón Return del teclado
            if (e.PropertyName == CustomEntry.ReturnKeyPropertyName)
            {
                Control.ImeOptions = customEntry.ReturnKeyType.GetValueFromDescription();
                Control.SetImeActionLabel(customEntry.ReturnKeyType.ToString(), Control.ImeOptions);
                Control.EditorAction -= Control_EditorAction;
                Control.EditorAction += Control_EditorAction;
            }

            // Para definir el color del borde del Entry
            if (e.PropertyName == CustomEntry.BorderColorProperty.PropertyName)
            {
                var customColor = customEntry.BorderColor.ToAndroid();

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(customColor);
                else
                    Control.Background.SetColorFilter(customColor, PorterDuff.Mode.SrcAtop);
            }
        }

        // Handler del click en el botón Return del teclado
        private void Control_EditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            CustomEntry customEntry = (CustomEntry)Element;

            if (customEntry?.ReturnKeyType != ReturnKeyTypes.Next)
                customEntry?.Unfocus();

            customEntry?.InvokeCompleted();
        }
    }

    public static class EnumExtensions
    {
        public static ImeAction GetValueFromDescription(this ReturnKeyTypes value)
        {
            var type = typeof(ImeAction);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value.ToString())
                        return (ImeAction)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return (ImeAction)field.GetValue(null);
                }
            }
            throw new NotSupportedException($"Not supported on Android: {value}");
        }
    }
}