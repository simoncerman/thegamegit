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
            public int wl { get; set; }
            public int wt { get; set; }

        }
        public static List<enemlog> enemiesave = new List<enemlog>();
        public class buletinf
        {
            public int left { get; set; }
            public int top { get; set; }
            public bool side { get; set; }
            public FrameworkElement el { get; set; }
            public UIElement ue { get; set; }
        }
        public static List<buletinf> bulletlog = new List<buletinf>();

        //grid info
        public static int workinggrid_width = 900; //šířka celého plátna 
        public static int workinggrid_height = 750; //výška celého plátna
        //character 
        public static int char_width = 80; //šířka hráče 
        public static int char_height = 80; //výška hráče 
        character ivan;
        wall wall1;
        wall wall2;
        wall wall3;
        DispatcherTimer time = new DispatcherTimer(DispatcherPriority.Render);
        scoreboard score;
        public MainWindow()
        { 
            InitializeComponent();
            time.Interval = new TimeSpan(0, 0, 0, 0, 1);
            time.Tick += Clock;

            time.Start();
            //score 
            score = new scoreboard(mriz, 700, 40);


            //object creating
            ivan = new character(mriz);
            // stěny ///
            wall1 = new wall(100, 300, 150, 50, mriz);
            wall2 = new wall(600, 300, 150, 50, mriz);
            wall3 = new wall(350, 500, 150, 50, mriz);

            //

        }
        public class scoreboard
        {
            public static Label scor;
            public scoreboard(Grid mriz, int sleft, int stop)
            {
                scor = new Label();
                scor.Content = 0;
                scor.FontSize = 50;
                scor.Width = 120;
                scor.Height = 70;
                scor.VerticalAlignment = VerticalAlignment.Top;
                scor.HorizontalAlignment = HorizontalAlignment.Left;
                scor.Margin = new Thickness(sleft, stop, 0, 0);
                mriz.Children.Add(scor);
            }
            public static void scoreplus(int plus)
            {
                string sscoreusing = scor.Content.ToString();
                int scoreusing = Convert.ToInt32(sscoreusing);
                scor.Content = scoreusing + plus;
            }
        }
        public static int go_down_speed = 3;
        int ts = 500;
        int interval = 0;
        int spawningtime = 0;
        public static int doublejump = 0;
        public static bool right, left, jump;
        int gravity = 30;
        public static int force = 0;
        public static BitmapImage wallimg = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/testwall.png"));

        public static List<UIElement> itemstoremove = new List<UIElement>();

        private void Clock(object sender, EventArgs e)
        {

            //ivan.echeck(mriz); - sběr bonus point
            if (right == true) { ivan.moveright(); }
            if (left == true) { ivan.moveleft(); }
            interval++;
            spawningtime++;
            if (interval == 1000 / ts)
            {
                ivan.jumpe(force);
                interval = 0;
            }
            if (spawningtime == 150)
            {
                EnType1 t1;
                t1 = new EnType1(mriz);
                spawningtime = 0;
            }
            bullet.bulletchange(mriz);
        }
        private void mousemainclick(object sender, MouseButtonEventArgs e)
        {
                bullet b;
                b = new bullet(mriz);
        }
        private void KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D) { right = true; }
            if (e.Key == Key.Left || e.Key == Key.A) { left = true; }
            if (e.Key == Key.Space && doublejump < 1)
            {
                doublejump += 1;
                MainWindow.force = gravity;
            }
            if (e.Key == Key.F)
            {
                bullet b;
                b = new bullet(mriz);
            }
        }
        private void KeyUp1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D) { right = false; }
            if (e.Key == Key.Left || e.Key == Key.A) { left = false; }
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
        public class enemy
        {
            public static void destroy(Grid mriz, enemlog gay)
            {
                int ix = gay.left;
                int yp = gay.top;
                foreach (FrameworkElement ui in itemstoremove)
                {
                    double x = ui.Margin.Left;
                    double y = ui.Margin.Top;
                    if (ix == x && y == yp)
                    {
                        foreach (walliind w in wali)
                        {
                            if (w.wallleft == gay.wl && w.wallup == gay.wt)
                            {
                                w.eontop = 0;
                            }
                        }
                        itemstoremove.Remove(ui);
                        mriz.Children.Remove(ui);
                        scoreboard.scoreplus(1);
                        break;
                    }
                }
            }
            public void elog(int x, int y, int sirka, int vyska, int wll, int wtt)
            {
                enemlog save = new enemlog()
                {
                    left = x,
                    top = y,
                    right = x + sirka,
                    bottom = y + vyska,
                    wl = wll, //wall of enemie spawn left 
                    wt = wtt, // -||- top
                };
                MainWindow.enemiesave.Add(save);
            }
        }
        public class EnType1 : enemy
        {
            public EnType1(Grid mriz)
            {
                BitmapImage point = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/en1.png"));
                Rectangle e1;
                int onecreate = 0; // creat only one in row
                e1 = new Rectangle();
                e1.Width = 40;
                e1.Height = 70;
                e1.VerticalAlignment = VerticalAlignment.Top;
                e1.HorizontalAlignment = HorizontalAlignment.Left;
                e1.Fill = new SolidColorBrush(Colors.Black);
                foreach (walliind wall in wali)
                {
                    if (wall.eontop == 0 && onecreate == 0)
                    {
                        onecreate = 1;
                        wall.eontop = 1;
                        e1.Fill = new ImageBrush(point);
                        e1.Margin = new Thickness((wall.wallright - wall.wallleft) / 2 + wall.wallleft - (e1.Width / 2), wall.wallup - e1.Height, 0, 0);
                        mriz.Children.Add(e1);
                        int a = Convert.ToInt32((wall.wallright - wall.wallleft) / 2 + wall.wallleft - (e1.Width / 2));
                        int b = Convert.ToInt32(wall.wallup - e1.Height);
                        elog(a, b, Convert.ToInt32(e1.Width), Convert.ToInt32(e1.Height), wall.wallleft, wall.wallup);
                        itemstoremove.Add(e1);
                    }
                }

            }
            
        }
        public class EnType2 : enemy
        {
            public EnType2(Grid mriz)
            {
                Rectangle e2;
                int onecreate = 0;
                e2 = new Rectangle();
                e2.Width = 40;
                e2.Height = 90;
                e2.HorizontalAlignment = HorizontalAlignment.Left;
                e2.VerticalAlignment = VerticalAlignment.Top;
                e2.Fill = new SolidColorBrush(Colors.Violet);
                foreach (walliind wall in wali)
                {
                    if (wall.eontop == 0 && onecreate == 0)
                    {
                        onecreate = 1;
                        wall.eontop = 1;
                        e2.Margin = new Thickness((wall.wallright - wall.wallleft) / 2 + wall.wallleft - (e2.Width / 2), wall.wallup - e2.Width - 10, 0, 0);
                        mriz.Children.Add(e2);
                        int a = Convert.ToInt32((wall.wallright - wall.wallleft) / 2 + wall.wallleft - (e2.Width / 2));
                        int b = Convert.ToInt32(wall.wallup - e2.Width - 10);
                        elog(a, b, Convert.ToInt32(e2.Width), Convert.ToInt32(e2.Height), wall.wallleft, wall.wallup);
                        itemstoremove.Add(e2);
                    }
                }
            }
        }


        class bullet
        {
            public Rectangle bul;
            public bullet(Grid mriz)
            {
                bul = new Rectangle();
                bul.Width = 10;
                bul.Height = 2;
                bul.Fill = new SolidColorBrush(Colors.Red);
                bul.VerticalAlignment = VerticalAlignment.Top;
                bul.HorizontalAlignment = HorizontalAlignment.Left;
                bul.Margin = new Thickness(character.charposleft+20,character.charposttop+40,0,0);
                mriz.Children.Add(bul);
                buletinf bullet = new buletinf
                {
                    left = Convert.ToInt32(bul.Margin.Left),
                    top = Convert.ToInt32(bul.Margin.Top),
                    side = character.charonright,
                    ue = bul,
                    el = bul
                };
                bulletlog.Add(bullet);
            }
            public static void bulletchange(Grid mriz)
            {
                foreach (buletinf bullet in bulletlog)
                {
                    FrameworkElement save = bullet.el;
                    if (save.Margin.Left<0 || save.Margin.Right>MainWindow.workinggrid_width)
                    {
                        mriz.Children.Remove(save);
                    }
                    else
                    {
                        mriz.Children.Remove(bullet.el);
                        if (bullet.side == true)
                        {
                            save.Margin = new Thickness(save.Margin.Left + 10, save.Margin.Top, 0, 0);
                            bullet.left = Convert.ToInt32(save.Margin.Left);
                        }
                        if (bullet.side == false)
                        {
                            bullet.left = Convert.ToInt32(save.Margin.Left);
                            save.Margin = new Thickness(save.Margin.Left - 10, save.Margin.Top, 0, 0);
                        }
                        mriz.Children.Add(save);

                        foreach (enemlog gay in enemiesave)
                        {
                            if (bullet.left + bullet.el.Width > gay.left &&
                                bullet.top + bullet.el.Height > gay.top &&
                                bullet.left < gay.right &&
                                bullet.top < gay.bottom)
                            {
                                enemy.destroy(mriz, gay);
                                enemiesave.Remove(gay);
                                break;
                            }
                        }
                    }

                }
            }
        }
        class character
        {
            public static bool charonright = true;
            public static int charposleft = 0;
            public static int charposttop =0;
            BitmapImage charleft = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/charleft.png"));
            BitmapImage charright = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/charright.png"));
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
                        enemy.destroy(mriz, gay);
                        enemiesave.Remove(gay);
                        break;
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
                    charonright = false;
                    c.Margin = new Thickness(left - 5, up, 0, 0);
                    c.Fill = new ImageBrush(charleft);
                }
                charposleft = Convert.ToInt32(c.Margin.Left);
                charposttop = Convert.ToInt32(c.Margin.Top);

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
                if (left + MainWindow.char_width > MainWindow.workinggrid_width - 5)
                {
                    inner = false;
                }
                if (inner == true)
                {
                    charonright = true;
                    c.Margin = new Thickness(left + 5, up, 0, 0);
                    c.Fill = new ImageBrush(charright);
                }
                charposleft = Convert.ToInt32(c.Margin.Left);
                charposttop = Convert.ToInt32(c.Margin.Top);
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
                    if (left + MainWindow.char_width > w.wallleft && left < w.wallright && up < w.walldown && up + MainWindow.char_height > w.walldown)
                    {
                        objectontop = true;
                        c.Margin = new Thickness(left, w.walldown, 0, 0);
                    }
                    //kontrola wall pod 
                    if (up + MainWindow.char_height >= w.wallup && left + MainWindow.char_width > w.wallleft && left < w.wallright && up < w.wallup)
                    {
                        objectondown = true;
                        if (force < 0)
                        {
                            c.Margin = new Thickness(left, w.wallup - MainWindow.char_height, 0, 0);
                        }
                    }
                }
                //kontrola border nad
                if (objectondown == true)
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
                    MainWindow.force = MainWindow.force - MainWindow.go_down_speed;
                    force = force - MainWindow.go_down_speed;
                    c.Margin = new Thickness(left, up - force, 0, 0);
                    Debug.WriteLine(force);
                }
                charposleft = Convert.ToInt32(c.Margin.Left);
                charposttop = Convert.ToInt32(c.Margin.Top);
                //provedení akcí -_-
                //Pokud jsme v prostoru a pod námi není nic 



            }
        }

    }
}