using SideApp.Model;
using SideApp.MVVM;
using System.Windows;

namespace SideApp.ViewModels
{
    internal class AnswerTextViewModel : ViewModelBase
    {
        static int randomRead = 0;
        string?[] Kun_Reading;
        string[] On_Reading;
        readonly string[] HappyEmoticon = { "(♥ω♥*)", "(⁄•⁄ω⁄•⁄)", "(≧◡≦) ♡", "(っ´ω`c)♡", "‘(´▽`ʃ♡ƪ)", "( ‾ʖ̫‾)",
        "(๑˃ᴗ˂)ﻭ", "(✧ω✧)", "(´｡• ᵕ •｡`)", "(* ´ ▽ ` *)", "(^_<)〜☆", "(≧∇≦)/", "(≧◡≦)", "(＾ω＾)", "＼(*T▽T*)／",
        "(*^▽^*)🎂", "(☞ﾟヮﾟ)☞", "(っ˘ω˘ς )", "(*≧∀≦*)", "ヽ(*ﾟдﾟ)ノｶｲﾊﾞｰ" };
        readonly string[] SadEmoticon = { "(Ｔ▽Ｔ)", "(；一_一)", "(´；Д；｀)", "（πーπ）", "（＞д＜）", "( #`⌂´)/┌┛",
        "(ﾉ｀□´)ﾉ⌒┻━┻", "(」｀□´)", "(●´⌓`●)", "(\\｀ﾛ´)\\", "(≧ヘ≦)", "(⇀‸↼‶)", "(*≧Д≦)", "(눈_눈)", "¯\\(°_o)/¯",
        "(￣〜￣;)", "(⊙_☉)", "(ಠ_ಠ)" };
        readonly string NeutralEmoticon = "?(´・ω・｀)";
        private readonly KanjiService _kanjiService = new KanjiService();

        public ButtonCommands Guessing => new(execute => CheckAnswer(), canExecute => Answer != "");
        public ButtonCommands KanjiLoad => new(execute => LoadKanji(), canExecute => true);
        public ButtonCommands ShowHint => new(execute => ReadingShow(), canExecute => true);
        public ButtonCommands WaitingMode => new(execute => WaitingProcess(), canExecute => true);

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

        private string answer = string.Empty;
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

        private string reading;
        public string Reading
        {
            get => reading;
            set
            {
                reading = value;
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

        private string emoticon = "?(´・ω・｀)";
        public string Emoticon
        {
            get => emoticon;
            set
            {
                emoticon = value;
                OnPropertyChanged();
            }
        }

        private string script = "Hiragana";
        public string Script
        {
            get => script;
            set
            {
                script = value;
                OnPropertyChanged();
            }
        }

        private Visibility hintVisibility = Visibility.Hidden;
        public Visibility HintVisibility
        {
            get => hintVisibility;
            set
            {
                hintVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility windowVisibility = Visibility.Visible;
        public Visibility WindowVisibility
        {
            get => windowVisibility;
            set
            {
                windowVisibility = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region KanjiLoading

        static int iteration = 0;
        static Kanji[] info;
        int ListSize = 0;
        Kanji LuckyOne;

        private async void LoadKanji()
        {
            newIterationReset();
            string list = "jlpt-5-enriched";
            randomizeVal();
            try
            {
                if (iteration == 0)
                {
                    info = await _kanjiService.GetListJLPT5Async(list);
                    ListSize = info.Length;
                    iteration++;
                }
                int RandNumKanji = Random.Shared.Next(0, ListSize);
                LuckyOne = info[RandNumKanji];
                Question = LuckyOne.kanji;
                Meaning = LuckyOne.Meaning[0];
                Grade = LuckyOne.Grade;

                if (randomRead == 1 && LuckyOne.ReadingKun != null)
                {
                    Kun_Reading = LuckyOne.ReadingKun;
                    Script = "Hiragana";
                    Reading = "Enter kunyomi reading!";
                }
                else if (randomRead == 0 || (randomRead == 1 && LuckyOne.ReadingKun == null))
                {
                    On_Reading = LuckyOne.ReadingOn;
                    Script = "Katakana";
                    Reading = "Enter onyomi reading!";
                }

            }
            catch (Exception ex)
            {
                Error = ex.Message; GiveItAnotherTry(); iteration = 0;
            }
        }
        static public void randomizeVal()
        {
            randomRead = Random.Shared.Next(0, 2);
        }
        public void newIterationReset()
        {
            Emoticon = NeutralEmoticon;
            HintVisibility = Visibility.Hidden;
            Error = "Can you guess this kanji?";
            Answer = "";
        }

        #endregion

        #region AnswerCheking
        private void CheckAnswer()
        {
            bool correct = false;
            if (randomRead == 0)
            { correct = ArrayCheck(On_Reading); }
            else if (randomRead == 1)
            { correct = ArrayCheck(Kun_Reading); }
            else { Error = "Well, I fucked up"; }

            if (correct)
            {
                Question = "Correct!";
                GiveMeASmile();
                WaitingProcess();
            }
            else if (correct == false)
            { Question = "Wrong!"; GiveItAnotherTry(); HintVisibility = Visibility.Visible; }
            else { Question = "something broke"; GiveItAnotherTry(); }
        }
        private bool ArrayCheck(string[] array)
        {
            bool found = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (Answer == array[i])
                {
                    found = true;
                }
            }
            if (found == false) { return false; }
            return true;
        }
        private void GiveMeASmile()
        {
            int Rand = Random.Shared.Next(0, HappyEmoticon.Length);
            Emoticon = HappyEmoticon[Rand];
        }
        private void GiveItAnotherTry()
        {
            int Rand = Random.Shared.Next(0, SadEmoticon.Length);
            Emoticon = SadEmoticon[Rand];
        }

        #endregion

        #region HintCommand
        private void ReadingShow()
        {
            Question = LuckyOne.kanji;
            var result = MessageBox.Show(PrintAllOfTheReading(), "Reading", MessageBoxButton.OK);
        }

        private string PrintAllOfTheReading()
        {
            string Fullreading = "";
            string[] Readings;
            if (randomRead == 0) Readings = On_Reading;
            else Readings = Kun_Reading;

            foreach (var reading in Readings)
                Fullreading += $"\"{reading}\" ";

            return Fullreading;
        }

        #endregion

        #region WaitingCommand
        private async void WaitingProcess() 
        {
            await Task.Delay(3000);

            WindowVisibility = Visibility.Hidden;
            newIterationReset();

            LoadKanji();
            int RandomWaitTime = Random.Shared.Next(180_000, 1_080_000); // from 30 minutes to 3 hours
            await Task.Delay(RandomWaitTime);

            WindowVisibility = Visibility.Visible;
        }
        #endregion

    }
}
