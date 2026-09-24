using SideApp.ViewModels;
using System.Media;
using System.Windows;

namespace SideApp
{
    public partial class MainWindow : Window
    {
        // have to check this one
        static readonly Dictionary<string, string> romajiToHiragana = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
        // Basic Vowels
        { "a", "あ" }, { "i", "い" }, { "u", "う" }, { "e", "え" }, { "o", "お" },
        // K-row
        { "ka", "か" }, { "ki", "き" }, { "ku", "く" }, { "ke", "け" }, { "ko", "こ" },
        { "ga", "が" }, { "gi", "ぎ" }, { "gu", "ぐ" }, { "ge", "げ" }, { "go", "ご" },        
        // S-row
        { "sa", "さ" }, { "shi", "し" }, { "su", "す" }, { "se", "せ" }, { "so", "そ" },
        { "za", "ざ" }, { "ji", "じ" }, { "zu", "ず" }, { "ze", "ぜ" }, { "zo", "ぞ" },        
        // T-row
        { "ta", "た" }, { "chi", "ち" }, { "tsu", "つ" }, { "te", "て" }, { "to", "と" },
        { "da", "だ" }, { "de", "で" }, { "do", "ど" }, { "jji", "ぢ" },  { "zzu", "づ" },
        // N-row
        { "na", "な" }, { "ni", "に" }, { "nu", "ぬ" }, { "ne", "ね" }, { "no", "の" },        
        // H-row
        { "ha", "は" }, { "hi", "ひ" }, { "fu", "ふ" }, { "he", "へ" }, { "ho", "ほ" },
        { "ba", "ば" }, { "bi", "び" }, { "bu", "ぶ" }, { "be", "べ" }, { "bo", "ぼ" },
        { "pa", "ぱ" }, { "pi", "ぴ" }, { "pu", "ぷ" }, { "pe", "ぺ" }, { "po", "ぽ" },        
        // M-row
        { "ma", "ま" }, { "mi", "み" }, { "mu", "む" }, { "me", "め" }, { "mo", "も" },        
        // Y-row
        { "ya", "や" }, { "yu", "ゆ" }, { "yo", "よ" },        
        // R-row
        { "ra", "ら" }, { "ri", "り" }, { "ru", "る" }, { "re", "れ" }, { "ro", "ろ" },
        { "wa", "わ" }, { "wo", "を" }, { "nn", "ん" },
        // Small row
        { "la", "ぁ" }, { "li", "ぃ" }, { "lu", "ぅ" }, { "le", "ぇ" }, { "lo", "ぉ" },
        { "lya", "ゃ" }, { "lyu", "ゅ" }, { "lyo", "ょ" }, { "ltsu", "っ" },
        { "ltu", "っ" }, { "lsu", "っ" }
        };

        public MainWindow()
        {
            InitializeComponent();
            AnswerTextViewModel vm = new();
            DataContext = vm;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is AnswerTextViewModel vm && vm.KanjiLoad.CanExecute(null)) 
            {
                vm.KanjiLoad.Execute(null);
            }
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void btLater_Click(object sender, RoutedEventArgs e)
        {
            tbAnswer.Clear();
            this.Hide();
            await Task.Delay(5000); // 1000 = 1 sec
            this.Show();
            SystemSounds.Question.Play();
        }

        private async void btSkip_Click(object sender, RoutedEventArgs e)
        {
            tbAnswer.Clear();
            this.Hide();
            await Task.Delay(5000);
            Window_Loaded(sender, e);
            this.Show();
            SystemSounds.Question.Play();
        }

        // somewaysomehow has to constantly check the input from tbAnswer and change it accordingly
        static string EnJapConvert(string str) 
        {

            return str;
        }

        private async void btGuess_Click(object sender, RoutedEventArgs e)
        {
            // will check the answer and display changes accordingly to result
            // wait for few seconds and then hide, getting into waiting mode
            await Task.Delay(3000);
            this.Hide();
        }

        private async void btGuessAndExit_Click(object sender, RoutedEventArgs e)
        {
            // the same as the other one, but will close the app after waiting
            await Task.Delay(3000);
            Close();
        }

        private void tbError_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if(tbError.Text == "Can you guess this kanji?")            
                tbError.FontSize = 14;            
            else
                tbError.FontSize = 8;
        }

        /* Let me be clear
        First things first, we can implement the active translation from en input from tbAnswer to jap
        When everything is done in thinking department, we can implement it showing one of the kanjis in tblQuestion
        Then we'll have to implement the random choice thingie, pretty straightforward
        Then we can finally try and do the check for answer / question, with actions proceeding accordingly
        The first (barebones) rating system that won't work properly since we'll have to make a file to store it
        Maybe add some kind of sound thingie when the window appears again

        Add Wanikani's "Oops, Kanji reading was expected"?
        */

        /*
        In order to get the Api to work, we need to get the kanji objects right from the call from said api, and then process 
        needed parts of that objects in needed places. We need: 
        1. Kanji itself => Question
        2. It's ip (number) => For future documentation
        3. On. reading => Answer1  }
                                    }=> will need to differentiate the two and display which one is needed to be inputed 
        4. Kun. reading => Answer2 }
        5. Meaning => Displayed on the side as a label to ease the learning process
        */

        // add key binding to make pressing enter bound to btGuess for comfort

    }
}