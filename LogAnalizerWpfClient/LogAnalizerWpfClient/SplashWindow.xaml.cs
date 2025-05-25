using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace LogAnalizerWpfClient
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
        }

        private void SplashWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (FindResource("FadeStoryboard") is Storyboard storyboard)
            {
                storyboard.Completed += FadeOut_Completed;
                storyboard.Begin(this);
            }
        }

        private void FadeOut_Completed(object sender, EventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }
        
    }
    

}