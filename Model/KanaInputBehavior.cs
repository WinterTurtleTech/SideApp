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

        public static readonly DependencyProperty ScriptProperty =
            DependencyProperty.RegisterAttached(
                "Script",
                typeof(KanaScript),
                typeof(KanaInputBehavior),
                new PropertyMetadata(KanaScript.Hiragana, OnScriptChanged));

        public static void SetScript(DependencyObject o, KanaScript v) => o.SetValue(ScriptProperty, v);
        public static KanaScript GetScript(DependencyObject o) => (KanaScript)o.GetValue(ScriptProperty);

        public static void SetIsEnabled(DependencyObject o, bool v) => o.SetValue(IsEnabledProperty, v);
        public static bool GetIsEnabled(DependencyObject o) => (bool)o.GetValue(IsEnabledProperty);
        
        private static readonly ConditionalWeakTable<TextBox, State> States = new();
        private sealed class State { public bool Suppress; }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox tb) return;
            if ((bool)e.NewValue) tb.TextChanged += OnTextChanged;
            else tb.TextChanged -= OnTextChanged;
        }

        private static void OnScriptChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) 
        {
            if(d is TextBox tb) Reconvert(tb);
        }

        private static void OnTextChanged(object sender, TextChangedEventArgs e)
            => Reconvert((TextBox)sender);

        private static void Reconvert(TextBox tb)
        {
            var state = States.GetOrCreateValue(tb);
            if (state.Suppress) return;

            var script = GetScript(tb); 

            var originalText = tb.Text;
            var originalCaret = tb.CaretIndex;

            var converted = EnToJapTrans.Convert(originalText, script);
            if (converted == originalText) return; 

            var prefix = EnToJapTrans.Convert(originalText.Substring(0, originalCaret), script);

            state.Suppress = true;
            try
            {
                tb.Text = converted;
                tb.CaretIndex = prefix.Length;
            }
            finally
            {
                state.Suppress = false;
            }
        }
    }
}
