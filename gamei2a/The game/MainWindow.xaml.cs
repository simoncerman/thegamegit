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
        public static int char_width = 75;
        public static int char_height = 75;
        public static int sup = 5;
        //wall arrays
        //
        character ivan;
        wall wall1;
        wall wall2;
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
            wall1 = new wall(500, 600, 150, 200, mriz);
            wall2 = new wall(200, 200, 200, 200, mriz);
            //

        }
        int interval = 0;
        int ts = 300;
        public static bool right, left, jump;
        int gravity = 25;
        public static int force = 0;
        public static bool objectontopwas;
        public static bool objectondownwas;
        public static bool borderontopwas;
        public static bool borderondownwas;
        private void Clock(object sender, EventArgs e)
        {
            if (right == true) { ivan.moveright(); }
            if (left == true) { ivan.moveleft(); }
            interval++;
            if (interval == 1000/ ts)
            {
                ivan.jumpe(force);
                interval = 0;
            }
        }

        private void KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right) { right = true; }
            if (e.Key == Key.Left) { left = true; }
            if (e.Key == Key.Space)
            {
                MainWindow.force = gravity;
            }
        }
        private void KeyUp1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right) { right = false; }
            if (e.Key == Key.Left){ left = false; }
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
            }

            private static void jevois(int x, int y, int sirka, int vyska)
            {
                walliind save = new walliind()
                {
                    walldown = y+vyska,
                    wallleft = x,
                    wallright = x+sirka,
                    wallup = y,
                };
                MainWindow.wali.Add(save);
            }
        }

        //main character class 

        class character
        {
            Rectangle c;
            public character(Grid mriz)
            {
                c = new Rectangle();
                c.Width = MainWindow.char_width;
                c.Height = MainWindow.char_height;
                c.Fill = new SolidColorBrush(Colors.Yellow);
                c.VerticalAlignment = VerticalAlignment.Top;
                c.HorizontalAlignment = HorizontalAlignment.Left;
                c.Margin = new Thickness(50, /*MainWindow.workinggrid_height - MainWindow.char_height*/100, 0, 0);
                mriz.Children.Add(c);
            }

            public void moveleft()
            {
                bool inner = true;
                int left = Convert.ToInt32(c.Margin.Left);
                int up = Convert.ToInt32(c.Margin.Top);
                foreach (walliind w in wali)
                {
                    if (left < w.wallright && up < w.walldown && up+MainWindow.char_height>w.wallup && left+MainWindow.char_width>w.wallleft+5)
                    {
                        inner = false;
                    }
                }
                if(left<5)
                {
                    inner = false;
                }
                if (inner == true)
                {
                    c.Margin = new Thickness(left - 10, up, 0, 0);
                }
            }
            public void moveright()
            {
                bool inner = true;
                int left = Convert.ToInt32(c.Margin.Left);
                int up = Convert.ToInt32(c.Margin.Top);
                foreach (walliind w in wali)
                {
                    if(left + MainWindow.char_height > w.wallleft && up<w.walldown && up+MainWindow.char_height>w.wallup && left<w.wallleft)
                    {
                        inner = false;
                    }
                }
                if (left + MainWindow.char_width > MainWindow.workinggrid_width)
                {
                    inner = false;
                }
                if (inner == true)
                {
                    c.Margin = new Thickness(left + 10, up, 0, 0);
                }
            }
            public void jumpe(int force)
            {
                bool objectontop = true;
                bool objectondown = false;
                bool borderontop = false;
                bool borderondown = false;

                int left = Convert.ToInt32(c.Margin.Left);
                int up = Convert.ToInt32(c.Margin.Top);
                
                foreach (walliind w in wali)
                {
                    //kontrola wall nad
                    if (up<w.walldown&&left+MainWindow.char_width>w.wallleft&&left<w.wallright&&up+MainWindow.char_height>w.wallup)
                    {
                        objectontop = true;
                    }
                    //kontrola wall pod 
                    if (up+MainWindow.char_height>w.wallup&&left+MainWindow.char_width>w.wallleft&&left<w.wallright&&up<w.walldown)
                    {
                        objectondown = true;
                    }
                }
                //kontrola border nad
                if (up<0)
                {
                    borderontop = true;
                }
                //kontrola border pod
                if (MainWindow.char_height + up >= MainWindow.workinggrid_height)
                {
                    borderondown = true;
                    MainWindow.force = 0;
                    force = 0;
                }
                //provedení akcí -_-
                //Pokud jsme v prostoru a pod námi není nic
                if (borderondown == false && objectondown == false)
                {
                    MainWindow.force = MainWindow.force - 1;
                    force = force - 1;
                }
                c.Margin = new Thickness(left, up - force,0,0);
                

            }
        }

    }
}
