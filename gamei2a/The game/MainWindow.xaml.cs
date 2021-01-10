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
        public class walliind //vlastní class na zaznamenávání pozice wall 
        {
            public int wallleft { get; set; } //levá strana stěny 
            public int wallright { get; set; } //pravá strana stěny 
            public int wallup { get; set; } //horní strana stěny
            public int walldown { get; set; } //dolní strana stěny
            public int eontop { get; set; } //nepřítel typu 1 nad stěnou
        }
        public static List<walliind> wali = new List<walliind>(); // vytvoření listu stěn
        //
        public class enemlog //vlastní class na zaznamenání pozic enemie
        {
            public int left { get; set; }
            public int right { get; set; }
            public int top { get; set; }
            public int bottom { get; set; }
            
        }
        public static List<enemlog> enemiesave = new List<enemlog>();
        //
        public static int workinggrid_width = 900; //šířka celého plátna 
        public static int workinggrid_height = 750; //výška celého plátna
        //character 
        public static int char_width = 40; //šířka hráče 
        public static int char_height = 90; //výška hráče 
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
        int spawningtime = 0;
        int ts = 500;
        public static int doublejump = 0;
        public static bool right, left, jump;
        int gravity = 30;
        public static int force = 0;
        public static BitmapImage wallimg = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/testwall.png"));

        public static List<UIElement> itemstoremove = new List<UIElement>();

        private void Clock(object sender, EventArgs e)
        {
            ivan.echeck(mriz);
            if (right == true) { ivan.moveright(); }
            if (left == true) { ivan.moveleft(); }
            interval++;
            spawningtime++;
            if (interval == 1000 / ts)
            {
                ivan.jumpe(force);
                interval = 0;
            }
            if(spawningtime == 100)
            {
                EnType1 t1;
                t1 = new EnType1(mriz);
            }
            if (spawningtime == 500)
            {
                EnType1 t2;
                t2 = new EnType1(mriz);
            }
            if (spawningtime == 1000)
            {
                EnType1 t3;
                t3 = new EnType1(mriz);
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
                    eontop = 0
                };
                MainWindow.wali.Add(save);
            }
        }

        //main character class 
        public class enemie
        {
            public static void destroy(Grid mriz,enemlog gay)
            {
                int ix = gay.left;
                int yp = gay.top;
                foreach(FrameworkElement ui in itemstoremove)
                {
                    double x = ui.Margin.Left;
                    double y = ui.Margin.Top;
                    if (ix == x && y==yp)
                    {
                        mriz.Children.Remove(ui);
                    }
                }
            }
            public void elog(int x, int y, int sirka, int vyska)
            {
                enemlog save = new enemlog()
                {
                    left = x,
                    top = y,
                    right = x + sirka,
                    bottom = y + vyska
                };
                MainWindow.enemiesave.Add(save);
            }
        }
        public class EnType1 : enemie
        {
            EnType1[] enTypes;
            public EnType1(Grid mriz)
            {
                Ellipse e1;
                int onecreate = 0; // creat only one in row
                e1 = new Ellipse();
                e1.Width = 50;
                e1.Height = 50;
                e1.VerticalAlignment = VerticalAlignment.Top;
                e1.HorizontalAlignment = HorizontalAlignment.Left;
                e1.Fill = new SolidColorBrush(Colors.Black);
                foreach (walliind wall in wali)
                {
                    if (wall.eontop == 0 && onecreate == 0)
                    {
                        onecreate = 1;
                        wall.eontop = 1;
                        e1.Margin = new Thickness((wall.wallright - wall.wallleft) / 2 + wall.wallleft - (e1.Width / 2), wall.wallup - e1.Width - 10, 0, 0);
                        mriz.Children.Add(e1);
                        int a = Convert.ToInt32((wall.wallright - wall.wallleft) / 2 + wall.wallleft - (e1.Width / 2));
                        int b = Convert.ToInt32(wall.wallup - e1.Width - 10);
                        elog(a, b, Convert.ToInt32(e1.Width),Convert.ToInt32(e1.Height));
                        itemstoremove.Add(e1);
                        
                    }
                }

            }
        }
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
            public void echeck(Grid mriz)
            {
                int left = Convert.ToInt32(c.Margin.Left);
                int up = Convert.ToInt32(c.Margin.Top);
                foreach (enemlog gay in enemiesave)
                {
                    if (left + MainWindow.char_width > gay.left && up + MainWindow.char_height > gay.top && left < gay.right && up < gay.bottom)
                    {
                        enemie.destroy(mriz,gay);
                    }
                }
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