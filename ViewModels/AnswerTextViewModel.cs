using SideApp.MVVM;

namespace SideApp.ViewModels
{
    internal class AnswerTextViewModel : ViewModelBase
    {
        // public ObservableCollection<Kanji> Kanjis { get; set; }
        public ButtonCommands Guessing => new ButtonCommands(execute => CheckAnswer(), canExecute => Answer != "");
        public ButtonCommands TEST => new ButtonCommands(execute => TEST1(), canExecute =>  true);

        // public ButtonCommands Adding => new ButtonCommands(execute => AddKanji(), canExecute => true);

        private void CheckAnswer()
        {
            if (Answer == Question)
            {
                Answer = "Correct!";
                Question = "Correct!";
            }
            else { Question = "False!"; }
        }

        private string question = "Haro";
        public string Question
        {
            get => question; 
            set
            { 
                question = value;
                OnPropertyChanged();
            }
        }        

        private string answer;
        public string Answer
        {
            get => answer;
            set
            {
                answer = value; 
                OnPropertyChanged();
            }
        }
        
        /*
        private void AddKanji()
        {
            Kanjis.Add(new Kanji 
            {
                Name = "",
                Id = 0,
                ReadingKun = "",
                ReadingOn = ""
            });
        }
        */

        private void TEST1()
        {
            Question = "Haro";
            Answer = "Haro?";
        }

    }
}
