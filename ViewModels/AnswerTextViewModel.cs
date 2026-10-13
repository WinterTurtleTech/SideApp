using Microsoft.VisualBasic;
using SideApp.Model;
using SideApp.MVVM;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Windows;

namespace SideApp.ViewModels
{
    internal class AnswerTextViewModel : ViewModelBase
    {
        static int randomRead = 1;
        string[] TheReading;       
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

        private string[] meaning;
        public string[] Meaning
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

        private double minTime = 5;
        public double MinTime
        {
            get { return minTime; }
            set 
            { 
                minTime = value;
                OnPropertyChanged();
            }
        }

        private double maxTime = 20;
        public double MaxTime
        {
            get { return maxTime; }
            set 
            {
                maxTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region KanjiLoading
        public ObservableCollection<JLPTListOption> AvaibleLists { get; } = new();
        public AnswerTextViewModel()
        {
            AvaibleLists.Add(new JLPTListOption { Id = "jlpt-5", DisplayName = "JLPT-5", IsSelected=true });
            AvaibleLists.Add(new JLPTListOption { Id = "jlpt-4", DisplayName = "JLPT-4", IsSelected=true });
            AvaibleLists.Add(new JLPTListOption { Id = "jlpt-3", DisplayName = "JLPT-3" });
            AvaibleLists.Add(new JLPTListOption { Id = "jlpt-2", DisplayName = "JLPT-2" });
            AvaibleLists.Add(new JLPTListOption { Id = "jlpt-1", DisplayName = "JLPT-1" });
        }
        public IReadOnlyList<string> SelectedListsIds =>
            AvaibleLists.Where(o => o.IsSelected).Select(o => o.Id).ToList();

        Kanji[] info;
        int ListSize;
        Kanji LuckyOne;
        string PrevList = "";
        int RandNumKanji = 0;
        private async void LoadKanji()
        {
            newIterationReset();
            randomizeVal();
            string list;
            if(SelectedListsIds.Count == 0)
            {
                list = "jlpt-5-enriched";
                Error = "Choose the Kanji List you want to study, moron!";
            }
            else                
            {
                string listID = SelectedListsIds[Random.Shared.Next(SelectedListsIds.Count)];
                list = $"{listID}-enriched";
            }
            try
            {
                if(PrevList != list)
                {
                    info = await _kanjiService.GetKanjiListAsync(list);
                    ListSize = info.Count();
                }
                RandNumKanji = Random.Shared.Next(0, ListSize);
                LuckyOne = info[RandNumKanji];
                Question = LuckyOne.kanji;
                Meaning = LuckyOne.Meaning;
                Grade = LuckyOne.Grade;

                if (randomRead == 1 && LuckyOne.ReadingKun != null)
                {
                    TheReading = LuckyOne.ReadingKun;
                    Script = "Hiragana";
                    Reading = "Enter kunyomi reading!";
                }
                else if (randomRead == 0 || (randomRead == 1 && LuckyOne.ReadingKun == null))
                {
                    TheReading = LuckyOne.ReadingOn;
                    Script = "Katakana";
                    Reading = "Enter onyomi reading!";
                }
                PrevList = list;
            }
            catch (Exception ex)
            {
                Error = ex.Message; GiveItAnotherTry(); PrevList = "";
                Answer = $"LS{ListSize}+Rand{RandNumKanji}+RR{randomRead}";
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
        private async Task CheckAnswer()
        {
            bool correct = false;            
            correct = ArrayCheck(TheReading); 
            
            if (correct)
            {
                Question = "Correct!";
                GiveMeASmile();
                await Task.Delay(3000);
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
            Readings = TheReading;

            foreach (var reading in Readings)
                Fullreading += $"\"{reading}\" ";

            return Fullreading;
        }

        #endregion

        #region WaitingCommand
        private async void WaitingProcess() 
        {
            WindowVisibility = Visibility.Hidden;
            newIterationReset();            
            LoadKanji();

            int RandomWaitTime;
            if (MinTime>MaxTime)
            {
                RandomWaitTime = Random.Shared.Next((int)(MaxTime * 60_000), (int)(MinTime * 60_000));
            }
            else
                RandomWaitTime = Random.Shared.Next((int)(MinTime * 60_000), (int)(MaxTime * 60_000));

            await Task.Delay(RandomWaitTime);

            WindowVisibility = Visibility.Visible;
        }
        #endregion

    }
}
