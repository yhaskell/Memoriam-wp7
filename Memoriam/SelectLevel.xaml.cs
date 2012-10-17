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
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.Threading;

namespace Memoriam
{
    public partial class Select_Level : PhoneApplicationPage
    {
        public Select_Level()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LevelsView.ItemsSource = Levels.Items;
           
            IsolatedStorageSettings s = IsolatedStorageSettings.ApplicationSettings;

            if (s.Contains("level")) LevelsView.SelectedIndex = (int)s["level"];
            else LevelsView.SelectedIndex = 0;

            Do = true;
        }

        bool Do = false;

        private void Levels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveGoBack();
        }

        private void LevelsView_Tap(object sender, GestureEventArgs e)
        {
            SaveGoBack();
        }

        void SaveGoBack()
        {
            if (!Do) return;

            IsolatedStorageSettings.ApplicationSettings["level"] = LevelsView.SelectedIndex;
            IsolatedStorageSettings.ApplicationSettings.Save();
            new Thread(new ThreadStart(() => { Thread.Sleep(250); this.Dispatcher.BeginInvoke(new Action(() => NavigationService.GoBack())); })).Start();
            IsolatedStorageSettings.ApplicationSettings["first"] = true;

            Do = false;
        }
    }

    public class Level
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Levels
    {
        static IEnumerable<Level> levels;
        private static readonly object locker = new object();
        public static IEnumerable<Level> Items
        {
            get
            {
                if (levels != null) return levels;
                Monitor.Enter(locker);

                var t = new List<Level>();

                t.Add(new Level { Name = "Novice", Description = "Levels with 3-7 circles (1-7 or A-G)" });
                t.Add(new Level { Name = "Beginner", Description = "8-12 circles (1-12 or A-L)" });
                t.Add(new Level { Name = "Intermediate", Description = "Up to 16 circles (1-16 or A-P)" });
                t.Add(new Level { Name = "Advanced", Description = "3-10 circles (unordered numbers or letters)" });
                t.Add(new Level { Name = "Expert", Description = "11-16 circles (likewize)" });
                t.Add(new Level { Name = "Insane", Description = "Everything possible" });
                t.Add(new Level { Name = "Jedy", Description = "Everything possible, and even more" });

                Interlocked.Exchange(ref levels, t);
                Monitor.Exit(locker);

                return levels;
            }
        }

    }
}