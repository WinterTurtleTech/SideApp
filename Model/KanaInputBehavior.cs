using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SideApp.Model
{
    public static class KanaInputBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(KanaInputBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));
        public static void SetIsEnabled(DependencyObject o, bool v) 
            => o.SetValue(IsEnabledProperty, v);
        public static bool GetIsEnabled(DependencyObject o) 
            => (bool)o.GetValue(IsEnabledProperty);
        private static readonly ConditionalWeakTable<TextBox, State> States = new();
        private sealed class State { public bool Suppress; }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox tb) return;
            if ((bool)e.NewValue) tb.TextChanged += OnTextChanged;
            else tb.TextChanged -= OnTextChanged;
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            var state = States.GetOrCreateValue(tb);

            // Reentrancy guard
            if (state.Suppress) return;

            var originalText = tb.Text;
            var originalCaret = tb.CaretIndex;

            var converted = EnToJapTrans.Convert(originalText);
            if (converted == originalText) return; // нечего менять — не трогаем каретку

            // Куда должна встать каретка в НОВОЙ строке?
            // Конвертируем только префикс до старой каретки — его длина и есть новая позиция.
            var convertedPrefix = EnToJapTrans.Convert(
                originalText.Substring(0, originalCaret));

            state.Suppress = true;
            tb.Text = converted;
            tb.CaretIndex = convertedPrefix.Length;
            state.Suppress = false;
        }
    }
}
