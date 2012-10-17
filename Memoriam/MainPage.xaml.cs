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

namespace Memoriam
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        
        private void SelectLevel_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SelectLevel.xaml", UriKind.Relative));
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Memoriam is a simple game that is intended to train memory.\n\nRules are very simple:\n You are to memorize sequences of circles and then reopen them in right order.\nThat's all!\n\nDeveloped by Igor N. Dultsev, MSP @ NsU.", "About Memoriam", MessageBoxButton.OK);
        }

        private void Play_Click(object sender, EventArgs e)
        {
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings["style"] = PlayStyle.GeneralRun;
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();
            NavigationService.Navigate(new Uri("/Play.xaml", UriKind.Relative));
        }

        private void Timed_Click(object sender, EventArgs e)
        {
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings["style"] = PlayStyle.TimedRun;
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();
            NavigationService.Navigate(new Uri("/Play.xaml", UriKind.Relative));
        }

        private void Tutorial_Click(object sender, EventArgs e)
        {
            Tutorial.ShowTutorial(true);
        }
    }
}