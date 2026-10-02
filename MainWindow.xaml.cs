using SideApp.ViewModels;
using System.Media;
using System.Windows;

namespace SideApp
{
    public partial class MainWindow : Window
    {
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
        

        #region UpLeftBt
        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void btLater_Click(object sender, RoutedEventArgs e)
        {
            tbAnswer.Clear();
            this.Hide();
            await Task.Delay(600000); // 1000 = 1 sec
            this.Show();
            SystemSounds.Question.Play();
        }
        #endregion

        #region GuessingBt
        

        private async void btGuessAndExit_Click(object sender, RoutedEventArgs e)
        {
            // the same as the other one, but will close the app after waiting
            await Task.Delay(3000);
            Close();
        }

        #endregion

        private void tbError_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {
            if (tbError.Text == "Can you guess this kanji?")
                tbError.FontSize = 14;
            else
            { tbError.FontSize = 10; }

        }
        
        /* Ideas
        Add key binding to make pressing enter bound to btGuess for comfort

        Add a covered-up (needed) reading that will open little by little with each wrong guess, which makes 
        the guessing easier and learning possible without direct googling each unknown reading =>
        => partly solved by the addition of hint button
        */

        

    }
}