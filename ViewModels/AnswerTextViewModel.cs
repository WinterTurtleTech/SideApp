using SideApp.Model;
using SideApp.MVVM;

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
        "(￣〜￣;)", "(⊙_☉)", "(•ิ_•ิ)", "(ಠ_ಠ)" };
        readonly string NeutralEmoticon = "?(´・ω・｀)";
        private readonly KanjiService _kanjiService = new KanjiService();
        public ButtonCommands Guessing => new ButtonCommands(execute => CheckAnswer(), canExecute => Answer != "");
        // public ButtonCommands KanjiChoosenLoad => new ButtonCommands(execute => LoadChoosenKanji(), canExecute => true);
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

        //private string? kun_Reading;
        //public string Kun_Reading
        //{
        //    get => kun_Reading;
        //    set
        //    {
        //        kun_Reading = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private string? on_Reading;
        //public string On_Reading
        //{
        //    get => on_Reading;
        //    set
        //    {
        //        on_Reading = value;
        //        OnPropertyChanged();
        //    }
        //}

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
        
        #endregion

        #region KanjiLoading

        static int iteration = 0;
        static Kanji[] info;
        int ListSize = 0;

        /*
        private async void LoadChoosenKanji() 
        {            
            string character = "蛍";
            randomizeVal();
            if (randomRead == 0) { Kun_Reading[0] = character; }
            else { On_Reading[0] = character; }    
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
        */
        private async void LoadKanji() 
        {
            Emoticon = NeutralEmoticon;
            string list = "jlpt-5-enriched";            
            randomizeVal();
            try 
            {
                if(iteration == 0) {
                    info = await _kanjiService.GetListJLPT5Async(list);
                    ListSize = info.Length;
                    iteration++;
                }
                int RandNumKanji = Random.Shared.Next(0, ListSize);
                Kanji LuckyOne = info[RandNumKanji];
                Question = LuckyOne.kanji;
                Meaning = LuckyOne.Meaning[0];
                Grade = LuckyOne.Grade;
                
                if (randomRead == 1 && LuckyOne.ReadingKun != null)
                {
                    Kun_Reading = LuckyOne.ReadingKun;
                    Reading = "Enter kunyomi reading!";
                    Error = LuckyOne.ReadingKun[0];
                }
                else if(randomRead == 0 || (randomRead == 1 && LuckyOne.ReadingKun == null)) 
                {
                    On_Reading = LuckyOne.ReadingOn;
                    Reading = "Enter onyomi reading!";
                    Error = LuckyOne.ReadingOn[0];
                }
                                
            }
            catch(Exception ex) 
            {
                Error = ex.Message;
            }
        }
        
        #endregion

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
            }
            else if (correct == false) 
            { Question = "Wrong!"; GiveItAnotherTry(); }
            else { Question = "something broke"; GiveItAnotherTry(); }            
        }
        private bool ArrayCheck(string[] array)
        {
            bool found = false;
            for(int i = 0;  i < array.Length; i++) 
            { 
            if (Answer == array[i])
            {
                found = true;
            }            
            }            
            if (found == false) { return false; }
            return true;
        }

        static public void randomizeVal() 
        {
            randomRead = Random.Shared.Next(0,2);            
        }
        private void GiveMeASmile() 
        {
            int Rand = Random.Shared.Next(0,HappyEmoticon.Length);
            Emoticon = HappyEmoticon[Rand];
        }
        private void GiveItAnotherTry()
        {
            int Rand = Random.Shared.Next(0, SadEmoticon.Length);
            Emoticon = SadEmoticon[Rand];
        }
    }
}
