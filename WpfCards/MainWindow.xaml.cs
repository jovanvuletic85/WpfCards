using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfCards
{

    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        string[] images = { "seven", "queen", "king" };
        int timerCount = 0;
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();
            EnableImage(false);
            textBlockStatus.Text = "";
            timer.Tick += TimerOp;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            label1.Visibility = label2.Visibility = label3.Visibility = Visibility.Collapsed;
            image_0.Source = image_1.Source = image_2.Source = GetImage("back");
        }

        private void TimerOp(object sender, EventArgs e)
        {
            timerCount++;
            Mixing(images);
            if (timerCount < 8)
            {
                OpenImage();
            }
            else
            {
                image_0.Source = image_1.Source = image_2.Source = GetImage("back");
                label1.Visibility = label2.Visibility = label3.Visibility = Visibility.Visible;
                textBlockStatus.Text = "Pick a card";
                timer.Stop();
                timerCount = 0;
                buttonStart.IsEnabled = true;
                EnableImage(true);
            }
        }

        private BitmapImage GetImage(string img)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("images/" + img + ".png", UriKind.Relative);
            image.EndInit();
            return image;
        }

        private void Mixing(string[] array)
        {
            Random random = new Random();
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                int r = i + random.Next(n - i);
                string t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
            images = array;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string result = "";
            OpenImage();
            EnableImage(false);
            Image image = (Image)sender;
            char[] splitChar = { '_' };
            string[] numberString = image.Name.Split(splitChar);
            int imageNumber = int.Parse(numberString[1]);
            if (images[imageNumber] == "king")
            {
                result = "You found the king!";
                score += 2;
            }
            else
            {
                result = "False!";
                score -= 1;
            }
            textBlockStatus.Text = result;
            textBlockScore.Text = $"Your score is: {score}";
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            textBlockStatus.Text = "Wait a moment";
            EnableImage(false);
            buttonStart.IsEnabled = false;
            label1.Visibility = label2.Visibility = label3.Visibility = Visibility.Collapsed;
        }

        private void EnableImage(bool enable)
        {
            image_0.IsEnabled = image_1.IsEnabled = image_2.IsEnabled = enable;
        }

        private void OpenImage()
        {
            image_0.Source = GetImage(images[0]);
            image_1.Source = GetImage(images[1]);
            image_2.Source = GetImage(images[2]);
        }
    }
}
