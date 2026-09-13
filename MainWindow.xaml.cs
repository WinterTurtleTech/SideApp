using System.Windows;

namespace SideApp
{
    public partial class MainWindow : Window
    {
        // need a dictionary with all of the en/jap counterparts

        public MainWindow()
        {
            InitializeComponent();
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
        }

        private void btSkip_Click(object sender, RoutedEventArgs e)
        {
            // hides window and then skips the current word, opens on next cycle
            // not implemented yet since idk how the whole system will look like
        }

        // somewaysomehow has to constantly check the input from tbAnswer and change it accordingly
        static string EnJapConvert(string str) 
        {

            return str;
        }

        private void btGuess_Click(object sender, RoutedEventArgs e)
        {
            // will check the answer and display changes accordingly to result
            // wait for few seconds and then hide, getting into waiting mode

        }

        private void btGuessAndExit_Click(object sender, RoutedEventArgs e)
        {
            // the same as the other one, but will close the app after waiting
            Close();
        }

        /* Let me be clear
        First things first, we can implement the active translation from en input from tbAnswer to jap
        After that one is done and tested, then we should properly get to know the Kanji API we'll be using
        When everything is done in thinking department, we can implement it showing one of the kanjis in tblQuestion
        Then we'll have to implement the random choice thingie, pretty straightforward
        Then we can finally try and do the check for answer / question, with actions proceeding accordingly
        The first (barebones) rating system that won't work properly since we'll have to make a file to store it
        And then we'll update the app accordingly

        kys
        */

    }
}