using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Controls;
using System.Threading;
using Microsoft.Devices;

namespace Memoriam
{
    public partial class Play : PhoneApplicationPage
    {
        public Play()
        {                       
            InitializeComponent();
        }

        PlayStyle playStyle { get; set; }
        int[] GameTable { get; set; }
        int[] Lives { get; set; }
        int Count { get; set; }
        bool LastFail { get; set; } 
        Random seed = new Random();
        bool Cleaned { get; set; }
        int Opened { get; set; }
        string[] Tags { get; set; }
        int Difficulty;
        int m_currLives, m_currLevel, m_Levels;
        int CurrentLives { get { return m_currLives; } set { m_currLives = value; LivesView.Text = value.ToString(); } }
        int CurrentLevel { get { return m_currLevel; } set { m_currLevel = value; LevelView.Text = (1 + value).ToString(); IsolatedStorageSettings.ApplicationSettings[playStyle == PlayStyle.GeneralRun ? "g" : "t" + Difficulty.ToString()] = value; IsolatedStorageSettings.ApplicationSettings.Save(); } }
        int OverallLevels { get { return m_Levels; } set { m_Levels = value; LevelsView.Text = value.ToString(); } }

        private void SelectPlayStyle()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("style")) { NavigationService.GoBack(); return; }
            playStyle = (PlayStyle) IsolatedStorageSettings.ApplicationSettings["style"];            
            this.ApplicationBar.IsVisible = (playStyle == PlayStyle.GeneralRun);
        }
        private bool InitializeLevelSequence()
        {
            int i = 0;

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("level"))
            {
                NavigationService.Navigate(new Uri("/SelectLevel.xaml", UriKind.Relative));
                return false;
            }


            Difficulty = (int) IsolatedStorageSettings.ApplicationSettings["level"];

            switch (Difficulty)
            {
                case 0:
                    
                    GameTable = new int[OverallLevels = 13];
                    GameTable[0] = 3;
                    GameTable[1] = 4;
                    GameTable[2] = GameTable[3] = 5;
                    GameTable[4] = GameTable[5] = GameTable[6] = GameTable[7] = 6;
                    GameTable[8] = GameTable[9] = GameTable[10] = GameTable[11] = GameTable[12] = 7;

                    Lives = new int[13];

                    Lives[0] = Lives[1] = 1;
                    Lives[2] = Lives[3] = Lives[4] = Lives[5] = 2;
                    for (i = 6; i < 13; i++) Lives[i] = 3;

                    return true;
                case 1:
                    GameTable = new int[OverallLevels = 40];
                    Lives = new int[40];
                    for (i = 0; i < 5; i++) { GameTable[i] = 8; Lives[i] = 3; }
                    for (i = 0; i < 7; i++) { GameTable[5 + i] = 9; Lives[5 + i] = 3; }
                    for (i = 0; i < 7; i++) { GameTable[12 + i] = 10; Lives[12 + i] = 3; }
                    for (i = 0; i < 9; i++) { GameTable[19 + i] = 11; Lives[19 + i] = 4; }
                    for (i = 0; i < 12; i++) { GameTable[28 + i] = 12; Lives[28 + i] = 4; }
                    return true;
                case 2:
                    GameTable = new int[OverallLevels = 69];
                    Lives = new int[69];
                    for (i = 0; i < 10; i++) { GameTable[i] = 13; Lives[i] = 4; }
                    for (i = 0; i < 14; i++) { GameTable[10 + i] = 14; Lives[10 + i] = 5; }
                    for (i = 0; i < 20; i++) { GameTable[24 + i] = 15; Lives[24 + i] = 5; }
                    for (i = 0; i < 25; i++) { GameTable[44 + i] = 16; Lives[44 + i] = 6; }
                    return true;
                case 3:
                    GameTable = new int[OverallLevels = 98];
                    Lives = new int[98];
                    for (i = 0; i < 3; i++) { GameTable[i] = 3; Lives[i] = 1; }
                    for (i = 0; i < 5; i++) { GameTable[3 + i] = 4; Lives[3 + i] = 2; }
                    for (i = 0; i < 7; i++) { GameTable[8 + i] = 5; Lives[8 + i] = 2; }
                    for (i = 0; i < 11; i++) { GameTable[15 + i] = 6; Lives[15 + i] = 3; }
                    for (i = 0; i < 13; i++) { GameTable[26 + i] = 7; Lives[25 + i] = 3; }
                    for (i = 0; i < 17; i++) { GameTable[39 + i] = 8; Lives[39 + i] = 4; }
                    for (i = 0; i < 19; i++) { GameTable[56 + i] = 9; Lives[56 + i] = 4; }
                    for (i = 0; i < 23; i++) { GameTable[75 + i] = 10; Lives[75 + i] = 4; }
                    break;
                case 4:
                    GameTable = new int[OverallLevels = 192];
                    Lives = new int[192];
                    for (i = 0; i < 18; i++) { GameTable[i] = 11; Lives[i] = 4; }
                    for (i = 0; i < 21; i++) { GameTable[18 + i] = 12; Lives[18 + i] = 5; }
                    for (i = 0; i < 27; i++) { GameTable[39 + i] = 13; Lives[39 + i] = 5; }
                    for (i = 0; i < 33; i++) { GameTable[66 + i] = 14; Lives[66 + i] = 5; }
                    for (i = 0; i < 42; i++) { GameTable[99 + i] = 15; Lives[99 + i] = 6; }
                    for (i = 0; i < 51; i++) { GameTable[141 + i] = 16; Lives[141 + i] = 6; }
                    return true;
                case 5:
                    GameTable = new int[OverallLevels = 124];
                    Lives = new int[124];
                    for (i = 0; i < 3; i++) { GameTable[i] = 3; Lives[i] = 1; }
                    for (i = 0; i < 5; i++) { GameTable[3 + i] = 4; Lives[3 + i] = 1; }
                    for (i = 0; i < 7; i++) { GameTable[8 + i] = 5; Lives[8 + i] = 2; }
                    for (i = 0; i < 13; i++) { GameTable[15 + i] = 6; Lives[15 + i] = 2; }
                    for (i = 0; i < 17; i++) { GameTable[28 + i] = 7; Lives[28 + i] = 3; }
                    for (i = 0; i < 21; i++) { GameTable[45 + i] = 8; Lives[45 + i] = 4; }
                    for (i = 0; i < 25; i++) { GameTable[66 + i] = 9; Lives[66 + i] = 4; }
                    for (i = 0; i < 33; i++) { GameTable[91 + i] = 10; Lives[91 + i] = 5; }
                    break;
                case 6:
                    GameTable = new int[OverallLevels = 510];
                    Lives = new int[510];
                    for (i = 0; i < 39; i++) { GameTable[i] = 11; Lives[i] = 5; }
                    for (i = 0; i < 47; i++) { GameTable[39 + i] = 12; Lives[39 + i] = 6; }
                    for (i = 0; i < 64; i++) { GameTable[86 + i] = 13; Lives[86 + i] = 7; }
                    for (i = 0; i < 81; i++) { GameTable[150 + i] = 14; Lives[150 + i] = 7; }
                    for (i = 0; i < 109; i++) { GameTable[231 + i] = 15; Lives[231 + i] = 8; }
                    for (i = 0; i < 170; i++) { GameTable[340 + i] = 16; Lives[340 + i] = 8; }
                    return true;                    
                default:
                    Lives = GameTable = new int[OverallLevels = 1];                    
                    return true;
            }

            return true;
        }      
        private void InitializeLevel(int level)
        {
            CheckEndReached();
            level = CurrentLevel;
            Count = GameTable[level];
            CurrentLives = Lives[level];
            Cleaned = false;
            Opened = 0;
            
            var crc = Circle.GeneratePoints(Count, 460, 610);

            int num = 0;
            if (Difficulty < 5)
                GenerateTags(Count, seed.Next(0, 2) == 0 ? TagType.Digit : TagType.Letter, Difficulty > 2);
            else GenerateTags(Count, TagType.Any, Difficulty==6);

            UpdateSeqView();
            foreach (var bc in ContentPanel.Children)            
                if (bc.GetType() == typeof(Button)) { ((Button)bc).Click -= new RoutedEventHandler(PlayTap_Click); }
            
            ContentPanel.Children.Clear();
            foreach (var c in crc)
            {
                var b = new Button();
                b.BorderThickness = new Thickness(6);
                b.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, (byte)seed.Next(160, 240), (byte)seed.Next(160, 240), (byte)seed.Next(160, 240)));
                b.FontFamily = new FontFamily("Georgia");
                b.FontWeight = FontWeights.SemiBold;
                b.FontSize = 35;
                b.SetValue(Canvas.LeftProperty, c.X - 60);
                b.SetValue(Canvas.TopProperty, c.Y - 60);
                b.Width = b.Height = 2 * c.Radius;
                b.Style = Application.Current.Resources["RoundButton"] as Style;
                b.Content = b.Tag = Tags[num++];
                b.Click += new RoutedEventHandler(PlayTap_Click);
                ContentPanel.Children.Add(b);
            }
            WaitForIt();
        }             
        private void CleanAllTags()
        {
            var cpc = ContentPanel.Children;

            for (int i = 0; i < cpc.Count; i++)            
                if (cpc[i].GetType() == typeof(Button)) ((Button)cpc[i]).Content = null;
            Cleaned = true;
            Opened = 0;
        }
        private void WaitForIt()
        {
            if (playStyle == PlayStyle.TimedRun)
                new Thread(new ThreadStart(() => { Thread.Sleep(Count * 700 * diffM()); this.Dispatcher.BeginInvoke(new Action(CleanAllTags)); })).Start();
        }
        private bool Initialize()
        {            
            SelectPlayStyle();
            Tutorial.ShowTutorial();
            return InitializeLevelSequence();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (Initialize())
            {
                CheckForSaves();
                InitializeLevel(CurrentLevel);
            }
        }
        private void Ok_Click(object sender, EventArgs e)
        {
            CleanAllTags();
        }
        private void PlayTap_Click(object sender, RoutedEventArgs e)
        {
            if (!Cleaned) return;

            var curr = sender as Button;
            if (curr.Tag == null) return;

            if (Tags[Opened] == (string) curr.Tag)
            {
                curr.Content = Tags[Opened++];
                curr.Tag = null;                
                TrySuccess();
                UpdateSeqView();
            }
            else
            {                
                CurrentLives--;
                TryFail();
            }
        }
        private void TrySuccess()
        {
            if (Opened == Count)            
                InitializeLevel(++CurrentLevel);
        }
        private void TryFail()
        {
            if (CurrentLives < 0)
            {
                VibrateController.Default.Start(new TimeSpan(0, 0, 1));
                if (LastFail == true) InitializeLevel(--CurrentLevel);
                else
                {
                    LastFail = true;
                    InitializeLevel(CurrentLevel);
                }
            }
            else
                VibrateController.Default.Start(new TimeSpan(0, 0, 0, 0, 100));
        }
        private void CheckEndReached()
        {
            if (CurrentLevel < 0) CurrentLevel = 0;
            if (CurrentLevel == OverallLevels)
            {
                MessageBox.Show("You advanced through all levels on that difficulty. Now it's time to move on!", "Memoriam®", MessageBoxButton.OK);
                CurrentLevel = 0;
                NavigationService.Navigate(new Uri("/SelectLevel.xaml", UriKind.Relative));
            }
        }
        private void GenerateTags(int count, TagType type, bool random)
        {
            Tags = new string[count];

            for (int i = 0; i < count; i++)
                if (type == TagType.Digit)
                    Tags[i] = random ? seed.Next(1, 99).ToString() : (1 + i).ToString();
                else if (type == TagType.Letter)
                    Tags[i] = random ? ((char)('A' + seed.Next(0, 26))).ToString() : ((char)('A' + i)).ToString();
                else
                    if (random)
                        Tags[i] = seed.Next(0, 2) == 0 ? ((char)('A' + seed.Next(0, 26))).ToString() + ((char)('A' + seed.Next(0, 26))).ToString() : seed.Next(1, 99).ToString();           
                    else
                        Tags[i] = seed.Next(0, 2) == 0 ? ((char)('A' + seed.Next(0, 26))).ToString() : seed.Next(1, 99).ToString();           
        }
        private void UpdateSeqView()
        {
            SeqBefore.Text = "";
            SeqAfter.Text = "";
            
            for (int i = 0; i < Opened; i++)            
                SeqBefore.Text += Tags[i] + " ";
            
            SeqCurrent.Text = Tags[Opened] + " ";

            for (int i = Opened + 1; i < Tags.Length; i++)
                SeqAfter.Text += Tags[i] + " ";
        }
        private void CheckForSaves()
        {
            int lastlevel = 0;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(playStyle == PlayStyle.GeneralRun ? "g" : "t" + Difficulty.ToString())) lastlevel = (int)IsolatedStorageSettings.ApplicationSettings[playStyle == PlayStyle.GeneralRun ? "g" : "t" + Difficulty.ToString()];

            if (lastlevel == 0) return;

            var res = MessageBox.Show("Last level you played on this difficulty is " + (lastlevel + 1) + ". Do you want to continue or begin from scratch?", "Memoriam®", MessageBoxButton.OKCancel);

            if (res == MessageBoxResult.OK) CurrentLevel = lastlevel;
            else CurrentLevel = 0;
        }
        private int diffM()
        {
            if (Difficulty == 0 || Difficulty == 3 || Difficulty == 5) return 2;
            else if (Difficulty == 1 || Difficulty == 4) return 3;
            else if (Difficulty == 2) return 4;
            else return 5;

        }
    }
    enum TagType { Digit, Letter, Any };
    
}