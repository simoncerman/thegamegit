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
    public class Walliind
    {
        public int Wallleft { get; set; }
        public int Wallright { get; set; }
        public int Wallup { get; set; }
        public int Walldown { get; set; }
        public int Eontop { get; set; }
        public bool Wasontop { get; set; }
    }
    public class Enemlog
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Wl { get; set; }
        public int Wt { get; set; }
    }
    public class Buletinf
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public bool Side { get; set; }
        public FrameworkElement El { get; set; }
        public UIElement Ue { get; set; }
    }
    public partial class MainWindow : Window
    {
        public static int workinggrid_width = 900;
        public static int workinggrid_height = 750;
        character ivan;
        DispatcherTimer time = new DispatcherTimer(DispatcherPriority.Render);
        public MainWindow()
        {
            InitializeComponent();
            Scoreboard score;
            wall wall1;
            wall wall2;
            wall wall3;
            time.Interval = new TimeSpan(0, 0, 0, 0, 1);
            time.Tick += Clock;
            time.Start();
            score = new Scoreboard(mriz, 700, 40);
            ivan = new character(mriz);
            wall1 = new wall(100, 300, 150, 50, mriz);
            wall2 = new wall(600, 300, 150, 50, mriz);
            wall3 = new wall(350, 500, 150, 50, mriz);
        }
        int ts = 500;
        int interval = 0;
        int spawningtime = 0;
        public static int doublejump = 0;
        public static bool right, left, jump;
        int gravity = 30;
        public static int force = 0;
        private void Clock(object sender, EventArgs e)
        {
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
                force = gravity;
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
    }
    public class Scoreboard
    {
        public static Label scor;
        public Scoreboard(Grid mriz, int sleft, int stop)
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
    class wall
    {
        public static List<Walliind> wali = new List<Walliind>();
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
            BitmapImage wallimg = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/testwall.png"));
            jevois(x, y, sirka, vyska);
            w.Fill = new ImageBrush(wallimg);
        }
        private static void jevois(int x, int y, int sirka, int vyska)
        {
            Walliind save = new Walliind()
            {
                Walldown = y + vyska,
                Wallleft = x,
                Wallright = x + sirka,
                Wallup = y,
                Eontop = 0,
                Wasontop = false
            };
            wall.wali.Add(save);
        }
    }
    class enemy
    {
        public static List<UIElement> itemstoremove = new List<UIElement>();
        public static List<Enemlog> enemiesave = new List<Enemlog>();
        public static int destroycount = 0;
        public static void destroy(Grid mriz, Enemlog gay)
        {
            int ix = gay.Left;
            int yp = gay.Top;
            foreach (FrameworkElement ui in itemstoremove)
            {
                double x = ui.Margin.Left;
                double y = ui.Margin.Top;
                if (ix == x && y == yp)
                {
                    foreach (Walliind w in wall.wali)
                    {
                        if (w.Wallleft == gay.Wl && w.Wallup == gay.Wt)
                        {
                            w.Eontop = 0;
                        }
                    }
                    itemstoremove.Remove(ui);
                    mriz.Children.Remove(ui);
                    Scoreboard.scoreplus(1);
                    destroycount++;
                    break;
                }
            }
        }
        public void elog(int x, int y, int sirka, int vyska, int wll, int wtt)
        {
            Enemlog save = new Enemlog()
            {
                Left = x,
                Top = y,
                Right = x + sirka,
                Bottom = y + vyska,
                Wl = wll,
                Wt = wtt,
            };
            enemy.enemiesave.Add(save);
        }
    }
    class EnType1 : enemy
    {
        public EnType1(Grid mriz)
        {
            int ontruetest = 0;
            BitmapImage point = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/en1.png"));
            Rectangle e1;
            int onecreate = 0;
            e1 = new Rectangle();
            e1.Width = 40;
            e1.Height = 70;
            e1.VerticalAlignment = VerticalAlignment.Top;
            e1.HorizontalAlignment = HorizontalAlignment.Left;
            e1.Fill = new SolidColorBrush(Colors.Black);
            foreach (Walliind wall in wall.wali)
            {
                if (wall.Eontop == 0 && onecreate == 0 && wall.Wasontop == false)
                {
                    wall.Wasontop = true;
                    onecreate = 1;
                    wall.Eontop = 1;
                    e1.Fill = new ImageBrush(point);
                    e1.Margin = new Thickness((wall.Wallright - wall.Wallleft) / 2 + wall.Wallleft - (e1.Width / 2), wall.Wallup - e1.Height, 0, 0);
                    mriz.Children.Add(e1);
                    int a = Convert.ToInt32((wall.Wallright - wall.Wallleft) / 2 + wall.Wallleft - (e1.Width / 2));
                    int b = Convert.ToInt32(wall.Wallup - e1.Height);
                    elog(a, b, Convert.ToInt32(e1.Width), Convert.ToInt32(e1.Height), wall.Wallleft, wall.Wallup);
                    itemstoremove.Add(e1);
                }
            }
            foreach (Walliind walii in wall.wali)
            {
                if (walii.Wasontop == true)
                {
                    ontruetest++;
                }
            }
            if (ontruetest == 3)
            {
                foreach (Walliind walis in wall.wali)
                {
                    walis.Wasontop = false;
                    ontruetest = 0;
                }
            }
        }
    }
    class EnType2 : enemy
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
            foreach (Walliind wall in wall.wali)
            {
                if (wall.Eontop == 0 && onecreate == 0)
                {
                    onecreate = 1;
                    wall.Eontop = 1;
                    e2.Margin = new Thickness((wall.Wallright - wall.Wallleft) / 2 + wall.Wallleft - (e2.Width / 2), wall.Wallup - e2.Width - 10, 0, 0);
                    mriz.Children.Add(e2);
                    int a = Convert.ToInt32((wall.Wallright - wall.Wallleft) / 2 + wall.Wallleft - (e2.Width / 2));
                    int b = Convert.ToInt32(wall.Wallup - e2.Width - 10);
                    elog(a, b, Convert.ToInt32(e2.Width), Convert.ToInt32(e2.Height), wall.Wallleft, wall.Wallup);
                    itemstoremove.Add(e2);
                }
            }
        }
    }
    class bullet
    {
        public static List<Buletinf> bulletlog = new List<Buletinf>();
        public Rectangle bul;
        public bullet(Grid mriz)
        {
            bul = new Rectangle();
            bul.Width = 10;
            bul.Height = 2;
            bul.Fill = new SolidColorBrush(Colors.Red);
            bul.VerticalAlignment = VerticalAlignment.Top;
            bul.HorizontalAlignment = HorizontalAlignment.Left;
            bul.Margin = new Thickness(character.charposleft + 20, character.charposttop + 40, 0, 0);
            mriz.Children.Add(bul);
            Buletinf bullet = new Buletinf
            {
                Left = Convert.ToInt32(bul.Margin.Left),
                Top = Convert.ToInt32(bul.Margin.Top),
                Side = character.charonright,
                Ue = bul,
                El = bul
            };
            bulletlog.Add(bullet);
        }
        public static void bulletchange(Grid mriz)
        {
            foreach (Buletinf bullet in bulletlog)
            {
                FrameworkElement save = bullet.El;
                if (save.Margin.Left < 0 || save.Margin.Left > MainWindow.workinggrid_width)
                {
                    mriz.Children.Remove(save);
                }
                else
                {
                    mriz.Children.Remove(bullet.El);
                    if (bullet.Side == true)
                    {
                        save.Margin = new Thickness(save.Margin.Left + 10, save.Margin.Top, 0, 0);
                        bullet.Left = Convert.ToInt32(save.Margin.Left);
                    }
                    if (bullet.Side == false)
                    {
                        bullet.Left = Convert.ToInt32(save.Margin.Left);
                        save.Margin = new Thickness(save.Margin.Left - 10, save.Margin.Top, 0, 0);
                    }
                    mriz.Children.Add(save);

                    foreach (Enemlog gay in enemy.enemiesave)
                    {
                        if (bullet.Left + bullet.El.Width > gay.Left &&
                            bullet.Top + bullet.El.Height > gay.Top &&
                            bullet.Left < gay.Right &&
                            bullet.Top < gay.Bottom)
                        {
                            enemy.destroy(mriz, gay);
                            enemy.enemiesave.Remove(gay);
                            break;
                        }
                    }
                }

            }
        }
    }
    class character
    {
        int go_down_speed = 3;
        int char_width = 80; //šířka hráče 
        int char_height = 80; //výška hráče 
        public static bool charonright = true;
        public static int charposleft = 0;
        public static int charposttop = 0;
        BitmapImage charleft = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/charleft.png"));
        BitmapImage charright = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/charright.png"));
        Rectangle c;
        public character(Grid mriz)
        {
            Image charac1 = new Image();
            charac1.Width = char_width;
            charac1.Height = char_height;

            c = new Rectangle();
            c.Width = char_width;
            c.Height = char_height;
            c.Fill = new SolidColorBrush(Colors.Yellow);
            c.VerticalAlignment = VerticalAlignment.Top;
            c.HorizontalAlignment = HorizontalAlignment.Left;
            c.Margin = new Thickness(50, 100, 0, 0);
            mriz.Children.Add(c);
            c.Fill = new ImageBrush(charright);
        }
        public void echeck(Grid mriz)
        {
            int left = Convert.ToInt32(c.Margin.Left);
            int up = Convert.ToInt32(c.Margin.Top);
            foreach (Enemlog gay in enemy.enemiesave)
            {
                if (left + char_width > gay.Left && up + char_height > gay.Top && left < gay.Right && up < gay.Bottom)
                {
                    enemy.destroy(mriz, gay);
                    enemy.enemiesave.Remove(gay);
                    break;
                }
            }
        }
        public void moveleft()
        {
            bool inner = true;
            int left = Convert.ToInt32(c.Margin.Left);
            int up = Convert.ToInt32(c.Margin.Top);
            foreach (Walliind w in wall.wali)
            {
                if (left - 1 < w.Wallright && up < w.Walldown && up + char_height > w.Wallup && left + char_width > w.Wallright)
                {
                    inner = false;
                    c.Margin = new Thickness(w.Wallright, up, 0, 0);
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
            foreach (Walliind w in wall.wali)
            {
                if (left + 1 + char_width > w.Wallleft && up < w.Walldown && up + char_height > w.Wallup && left < w.Wallleft)
                {
                    inner = false;
                    c.Margin = new Thickness(w.Wallleft - char_width, up, 0, 0);
                }
            }
            if (left + char_width > MainWindow.workinggrid_width - 5)
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

            foreach (Walliind w in wall.wali)
            {
                if (left + char_width > w.Wallleft && left < w.Wallright && up < w.Walldown && up + char_height > w.Walldown)
                {
                    objectontop = true;
                    c.Margin = new Thickness(left, w.Walldown, 0, 0);
                }
                if (up + char_height >= w.Wallup && left + char_width > w.Wallleft && left < w.Wallright && up < w.Wallup)
                {
                    objectondown = true;
                    if (force < 0)
                    {
                        c.Margin = new Thickness(left, w.Wallup - char_height, 0, 0);
                    }
                }
            }
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
            if (char_height + up >= MainWindow.workinggrid_height - 6)
            {
                borderondown = true;
                if (force > 0)
                {
                    c.Margin = new Thickness(left, up - force, 0, 0);
                }
                else
                {
                    c.Margin = new Thickness(left, MainWindow.workinggrid_height - char_height, 0, 0);
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
                MainWindow.force -= go_down_speed;
                force -= go_down_speed;
                c.Margin = new Thickness(left, up - force, 0, 0);
                Debug.WriteLine(force);
            }
            charposleft = Convert.ToInt32(c.Margin.Left);
            charposttop = Convert.ToInt32(c.Margin.Top);
        }
    }
}