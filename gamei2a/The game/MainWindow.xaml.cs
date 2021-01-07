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
using System.Data;
using System.Diagnostics;
namespace The_game
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        //workplace
        public class walliind
        {
            public int wallleft { get; set; }
            public int wallright { get; set; }
            public int wallup { get; set; }
            public int walldown { get; set; }
        }
        public static List<walliind> wali = new List<walliind>();
        //
        public static int testonly = 0;
        public static int workinggrid_width = 900;
        public static int workinggrid_height = 750;
        //character 
        public static int char_width = 40;
        public static int char_height = 90;
        public static int sup = 5;
        //wall arrays
        //
        character ivan;
        wall wall1;
        wall wall2;
        wall wall3;
        DispatcherTimer time = new DispatcherTimer(DispatcherPriority.Render);

        public MainWindow()
        {
            InitializeComponent();
            time.Interval = new TimeSpan(0, 0, 0, 0, 1);
            time.Tick += Clock;
            time.Start();
            //object creating
            ivan = new character(mriz);
            // stěny ///
            wall1 = new wall(100, 300, 150, 50, mriz);
            wall2 = new wall(600, 300, 150, 50, mriz);
            wall3 = new wall(350, 500, 150, 50, mriz);
            //

        }
        int interval = 0;
        int ts = 500;
        public static int doublejump = 0;
        public static bool right, left, jump;
        int gravity = 30;
        public static int force = 0;
        public static BitmapImage wallimg = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/testwall.png"));
        private void Clock(object sender, EventArgs e)
        {
            if (right == true) { ivan.moveright(); }
            if (left == true) { ivan.moveleft(); }
            interval++;
            if (interval == 1000 / ts)
            {
                ivan.jumpe(force);
                interval = 0;
            }
        }

        private void KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right) { right = true; }
            if (e.Key == Key.Left) { left = true; }
            if (e.Key == Key.Space&&doublejump<1)
            {
                doublejump += 1;
                MainWindow.force = gravity;
            }
        }
        private void KeyUp1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right) { right = false; }
            if (e.Key == Key.Left) { left = false; }
        }
        // main wall class (class for making wall omegalul)
        class wall
        {
            public Rectangle w;
            public wall(int x, int y, int sirka, int vyska, Grid mriz)
            {
                w = new Rectangle();
                w.Width = sirka;
                w.Height = vyska;
                w.Margin = new Thickness(x, y, 0, 0);
                w.Fill = new SolidColorBrush(Colors.Red);
                w.VerticalAlignment = VerticalAlignment.Top;
                w.HorizontalAlignment = HorizontalAlignment.Left;
                mriz.Children.Add(w);
                jevois(x, y, sirka, vyska);
                w.Fill = new ImageBrush(wallimg);
            }

            private static void jevois(int x, int y, int sirka, int vyska)
            {
                walliind save = new walliind()
                {
                    walldown = y + vyska,
                    wallleft = x,
                    wallright = x + sirka,
                    wallup = y,
                };
                MainWindow.wali.Add(save);
            }
        }

        //main character class 

        class character
        {
            BitmapImage charleft = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/characterpicrigleft.png"));
            BitmapImage charright = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/characterpicrigright.png"));
            Rectangle c;
            public character(Grid mriz)
            {
                Image charac1 = new Image();
                charac1.Width = MainWindow.char_width;
                charac1.Height = MainWindow.char_height;

                c = new Rectangle();
                c.Width = MainWindow.char_width;
                c.Height = MainWindow.char_height;
                c.Fill = new SolidColorBrush(Colors.Yellow);
                c.VerticalAlignment = VerticalAlignment.Top;
                c.HorizontalAlignment = HorizontalAlignment.Left;
                c.Margin = new Thickness(50, /*MainWindow.workinggrid_height - MainWindow.char_height*/100, 0, 0);
                mriz.Children.Add(c);
                c.Fill = new ImageBrush(charright);

            }

            public void moveleft()
            {
                bool inner = true;
                int left = Convert.ToInt32(c.Margin.Left);
                int up = Convert.ToInt32(c.Margin.Top);
                foreach (walliind w in wali)
                {
                    if (left - 1 < w.wallright && up < w.walldown && up + MainWindow.char_height > w.wallup && left + MainWindow.char_width > w.wallright)
                    {
                        inner = false;
                        c.Margin = new Thickness(w.wallright, up, 0, 0);
                    }
                }
                if (left < 5)
                {
                    inner = false;
                }
                if (inner == true)
                {
                    c.Margin = new Thickness(left - 5, up, 0, 0);
                    c.Fill = new ImageBrush(charleft);
                }
            }
            public void moveright()
            {
                bool inner = true;
                int left = Convert.ToInt32(c.Margin.Left);
                int up = Convert.ToInt32(c.Margin.Top);
                foreach (walliind w in wali)
                {
                    if (left + 1 + MainWindow.char_width > w.wallleft && up < w.walldown && up + MainWindow.char_height > w.wallup && left < w.wallleft)
                    {
                        inner = false;
                        c.Margin = new Thickness(w.wallleft - MainWindow.char_width, up, 0, 0);
                    }
                }
                if (left + MainWindow.char_width > MainWindow.workinggrid_width-5)
                {
                    inner = false;
                }
                if (inner == true)
                {
                    c.Margin = new Thickness(left + 5, up, 0, 0);
                    c.Fill = new ImageBrush(charright);
                }
            }
            public void jumpe(int force)
            {
                bool objectontop = false;
                bool objectondown = false;
                bool borderontop = false;
                bool borderondown = false;
                int left = Convert.ToInt32(c.Margin.Left);
                int up = Convert.ToInt32(c.Margin.Top);

                foreach (walliind w in wali)
                {
                    //kontrola wall nad
                    if (left+MainWindow.char_width>w.wallleft&&left<w.wallright&&up<w.walldown&&up+MainWindow.char_height>w.walldown)
                    {
                        objectontop = true;
                        c.Margin = new Thickness(left, w.walldown, 0, 0);
                    }
                    //kontrola wall pod 
                    if (up+MainWindow.char_height>=w.wallup&&left+MainWindow.char_width>w.wallleft&&left<w.wallright&&up<w.wallup)
                    {
                        objectondown = true;
                        if (force < 0)
                        {
                            c.Margin = new Thickness(left, w.wallup - MainWindow.char_height, 0, 0);
                        }
                    }
                }
                //kontrola border nad
                if(objectondown == true)
                {
                    MainWindow.doublejump = 0; 
                    if (force > 0)
                    {
                        c.Margin = new Thickness(left, up - force, 0, 0);
                    }
                    if (force <= 0)
                    {
                        force = 0;
                        MainWindow.force = 0;
                    }

                }
                if (up < 0)
                {
                    borderontop = true;
                }
                if (objectontop == true)
                {
                     force = -3;
                    MainWindow.force = -3;
                }
                //kontrola border pod
                if (MainWindow.char_height + up >= MainWindow.workinggrid_height - 6)
                {
                    borderondown = true;
                    if (force > 0)
                    {
                        c.Margin = new Thickness(left, up - force, 0, 0);
                    }
                    else
                    {
                        c.Margin = new Thickness(left, MainWindow.workinggrid_height - MainWindow.char_height, 0, 0);
                        MainWindow.force = 0;
                    }
                    MainWindow.doublejump = 0;

                }
                if (borderontop == true)
                {
                    c.Margin = new Thickness(left, 0, 0, 0);
                    force = -3;
                    MainWindow.force = -3;
                }
                if (borderondown == false && borderontop == false && objectondown == false)
                {
                    MainWindow.force = MainWindow.force - 3;
                    force = force - 3;
                    c.Margin = new Thickness(left, up - force, 0, 0);
                    Debug.WriteLine(force);
                }

                //provedení akcí -_-
                //Pokud jsme v prostoru a pod námi není nic



            }
        }

    }
}