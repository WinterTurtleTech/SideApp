using SideApp.Model;
using SideApp.MVVM;

namespace SideApp.ViewModels
{
    internal class AnswerTextViewModel : ViewModelBase
    {
        static int randomRead = 0;
        private readonly KanjiService _kanjiService = new KanjiService();
        public ButtonCommands Guessing => new ButtonCommands(execute => CheckAnswer(), canExecute => Answer != "");
        public ButtonCommands KanjiChoosenLoad => new ButtonCommands(execute => LoadChoosenKanji(), canExecute => true);
        public ButtonCommands KanjiLoad => new ButtonCommands(execute => LoadKanji(), canExecute => true);

        #region MyPrivates
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

        private string meaning;
        public string Meaning
        {
            get => meaning;
            set
            {
                meaning = value;
                OnPropertyChanged();
            }
        }

        private string error;
        public string Error
        {
            get => error;
            set
            {
                error = value;
                OnPropertyChanged();
            }
        }

        private string? kun_Reading;
        public string Kun_Reading
        {
            get => kun_Reading;
            set
            {
                kun_Reading = value;
                OnPropertyChanged();
            }
        }
        
        private string on_Reading;
        public string On_Reading
        {
            get => on_Reading;
            set
            {
                on_Reading = value;
                OnPropertyChanged();
            }
        }

        private int grade;
        public int Grade
        {
            get => grade;
            set
            {
                grade = value;
                OnPropertyChanged();
            }
        }
        
        #endregion
        
        private void CheckAnswer()
        {
            if (Answer == kun_Reading)
            {
                Answer = "Correct!";
            }
            else { Answer = "False!"; }
        }
        
        private async void LoadChoosenKanji() 
        {            
            string character = "蛍";
            randomizeVal();
            if (randomRead == 0) { Kun_Reading = character; }
            else { On_Reading = character; }    
            try
            {
                Kanji info = await _kanjiService.GetKanjiAsync(character);
                Meaning = info.Meaning[0]; // better change to some expanding element later
                Question = info.kanji;
                // random decision which one will be chosen + check if On is null
                Kun_Reading = info.ReadingKun[0];
                On_Reading = info.ReadingOn[0];
                Grade = info.Grade;
            }
            catch (Exception ex)
            {
                Error = ex.Message;                
            }
        }
        static int iteration = 0;
        static Kanji[] info;
        private async void LoadKanji() 
        {
            string list = "jlpt-5-enriched";
            int RandNumKanji = Random.Shared.Next(0, 50);
            randomizeVal();
            try 
            {
                if(iteration == 0) {
                    info = await _kanjiService.GetListJLPT5Async(list);
                    iteration++;
                }
                Kanji LuckyOne = info[RandNumKanji];
                Question = LuckyOne.kanji;
                Meaning = LuckyOne.Meaning[0];
                Kun_Reading = LuckyOne.ReadingKun[0];
                On_Reading = LuckyOne.ReadingOn[0];
                Grade = LuckyOne.Grade;
            }
            catch(Exception ex) 
            {
                Error = ex.Message;
            }
        }

        static public void randomizeVal() 
        {
            randomRead = Random.Shared.Next(0,2);
        }

        /*
         
        */
    }
}
