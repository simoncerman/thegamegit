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
    public class Walliind // for log of walls
    {
        public int Wallleft { get; set; }
        public int Wallright { get; set; }
        public int Wallup { get; set; }
        public int Walldown { get; set; }
        public int Eontop { get; set; } //enemy on top
        public bool Wasontop { get; set; } //enemy was top - helpful bool
    }
    public class Enemlog // for log of enemy
    {
        public int Left { get; set; } 
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Wl { get; set; } // left of wall where enemy stand
        public int Wt { get; set; } // right of wall where eneym stand
    }
    public class Buletinf // for bullet tracking 
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public bool Side { get; set; } //side (left / right) -> ((if true -> go right/ if false -> go left))
        public FrameworkElement El { get; set; } //saving bullet as element
        public UIElement Ue { get; set; } //the same  -||-
    }
    public partial class MainWindow : Window
    {
        public static int workinggrid_width = 900; // not changing - for reading (mainly for character -> will be used for enemy too)
        public static int workinggrid_height = 750; // -||-
        private readonly Character ivan; // main character 
        readonly DispatcherTimer time = new DispatcherTimer(DispatcherPriority.Render); // buging if no Dispa.ity.Render 
        public MainWindow()
        {
            InitializeComponent();
            Scoreboard score;
            time.Interval = new TimeSpan(0, 0, 0, 0, 1);
            time.Tick += Clock;
            time.Start();
            score = new Scoreboard(mriz, 700, 40); //score with x and y
            ivan = new Character(mriz); //creating character
            /* wall creating*/
            Wall wall1;
            Wall wall2;
            Wall wall3;
            wall1 = new Wall(100, 300, 150, 50, mriz);
            wall2 = new Wall(600, 300, 150, 50, mriz);
            wall3 = new Wall(350, 500, 150, 50, mriz);
        }
        readonly int ts = 500; //time when funcion jump is called
        int interval = 0; //using for timing of jump  - must be 0 -> every round +1 (every milisec)
        int spawningtime = 0; // same like interval -> used for enemies
        public static int doublejump = 0; //is used for doublejump -_-
        public static bool right, left, jump; //some bools maybe used in time more
        readonly int gravity = 30; //acualy not gravity but jump-up force - its like anti-gravity
        public static int force = 0; //changing when (jump - Increase / fall - Decrease)
        private void Clock(object sender, EventArgs e) //clasic timing thing
        {
            if (right == true) { ivan.Moveright(); }
            if (left == true) { ivan.Moveleft(); }
            interval++;
            spawningtime++;
            if (interval == 1000 / ts)
            {
                ivan.Jumpe(force);
                interval = 0;
            }
            if (spawningtime == 150) //spawn of enemy type 1
            {
                EnType1 t1 = new EnType1(mriz);
                spawningtime = 0;
            }
            Bullet.Bulletchange(mriz);
        }
        private void Mousemainclick(object sender, MouseButtonEventArgs e) //shoot on click
        {
            Bullet b;
            b = new Bullet(mriz);
        }
        private void KeyDown1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D) { right = true; } //go to right
            if (e.Key == Key.Left || e.Key == Key.A) { left = true; } //go to left
            if (e.Key == Key.Space && doublejump < 1) //change force if space key down
            {
                doublejump += 1;
                force = gravity;
            }
            if (e.Key == Key.F) //shooting like on click (left)
            {
                Bullet b;
                b = new Bullet(mriz);
            }
        }
        private void KeyUp1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right || e.Key == Key.D) { right = false; }
            if (e.Key == Key.Left || e.Key == Key.A) { left = false; }
        }
    }
    public class Scoreboard //score class 
    {
        public static Label scor;
        public Scoreboard(Grid mriz, int sleft, int stop) // create score lable 
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
        public static void Scoreplus(int plus) //funcion to add points
        {
            string sscoreusing = scor.Content.ToString();
            int scoreusing = Convert.ToInt32(sscoreusing);
            scor.Content = scoreusing + plus;
        }
    }
    class Wall //wall funcion
    {
        public static List<Walliind> wali = new List<Walliind>(); //this is list of walls
        public Rectangle w;
        public Wall(int x, int y, int sirka, int vyska, Grid mriz) //constructor funcion
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
            Jevois(x, y, sirka, vyska); //funcion to address wall to log of walls 
            w.Fill = new ImageBrush(wallimg); //filling wall with photo
        }
        private static void Jevois(int x, int y, int sirka, int vyska) //funcion to address wall to log of walls 
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
            Wall.wali.Add(save); //save into wall info list
        }
    }
    class Enemy //enemy class 
    {
        public static List<UIElement> itemstoremove = new List<UIElement>(); // list to save uielement (full instance of enemy)
        public static List<Enemlog> enemiesave = new List<Enemlog>(); // list to save info about enemy
        public static int destroycount = 0; 
        public static void Destroy(Grid mriz, Enemlog gay) //class for remove enemy from grid and from list
        {
            int ix = gay.Left;
            int yp = gay.Top;
            foreach (FrameworkElement ui in itemstoremove) 
            {
                double x = ui.Margin.Left;
                double y = ui.Margin.Top;
                if (ix == x && y == yp)
                {
                    foreach (Walliind w in Wall.wali) //changing info of wall where was enemy to controll spawn on walls
                    {
                        if (w.Wallleft == gay.Wl && w.Wallup == gay.Wt)
                        {
                            w.Eontop = 0;
                        }
                    }
                    itemstoremove.Remove(ui);
                    mriz.Children.Remove(ui);
                    Scoreboard.Scoreplus(1);
                    destroycount++;
                    break;
                }
            }
        }
        public void Elog(int x, int y, int sirka, int vyska, int wll, int wtt) //funcion to log enemy
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
            Enemy.enemiesave.Add(save);
        }
    }
    class EnType1 : Enemy // enemy type 1
    {
        public EnType1(Grid mriz) // create enemy type 1
        {
            Random rand = new Random();
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

            //randomly spawn - fix 
            List<Walliind> timewalilist = new List<Walliind>(); // create "temporarily" List - for randomly choose
            while (timewalilist.Count!=Wall.wali.Count) //while - all items in list will not be in new list
            {
                int randomized = rand.Next(0, Wall.wali.Count()); //random number what you take from list
                Walliind randomly_choosed = Wall.wali[randomized]; //randomly choosed item
                if (timewalilist.Contains(randomly_choosed)!= true) //test if randomly choosed item is in the list
                {
                    timewalilist.Add(randomly_choosed); //add to the randomized list
                }
            }

            foreach (Walliind wall in timewalilist)
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
                    Elog(a, b, Convert.ToInt32(e1.Width), Convert.ToInt32(e1.Height), wall.Wallleft, wall.Wallup);
                    itemstoremove.Add(e1);
                }
            }
            foreach (Walliind walii in Wall.wali)
            {
                if (walii.Wasontop == true)
                {
                    ontruetest++;
                }
            }
            if (ontruetest == 3)
            {
                foreach (Walliind walis in Wall.wali)
                {
                    walis.Wasontop = false;
                    ontruetest = 0;
                }
            }
        }
    }
    class EnType2 : Enemy //enemy type 2 on prepare
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
            foreach (Walliind wall in Wall.wali)
            {
                if (wall.Eontop == 0 && onecreate == 0)
                {
                    onecreate = 1;
                    wall.Eontop = 1;
                    e2.Margin = new Thickness((wall.Wallright - wall.Wallleft) / 2 + wall.Wallleft - (e2.Width / 2), wall.Wallup - e2.Width - 10, 0, 0);
                    mriz.Children.Add(e2);
                    int a = Convert.ToInt32((wall.Wallright - wall.Wallleft) / 2 + wall.Wallleft - (e2.Width / 2));
                    int b = Convert.ToInt32(wall.Wallup - e2.Width - 10);
                    Elog(a, b, Convert.ToInt32(e2.Width), Convert.ToInt32(e2.Height), wall.Wallleft, wall.Wallup);
                    itemstoremove.Add(e2);
                }
            }
        }
    }
    class Bullet //bullet class
    {
        public static List<Buletinf> bulletlog = new List<Buletinf>();
        public Rectangle bul;
        public Bullet(Grid mriz) //bullet create funcion 
        {
            bul = new Rectangle();
            bul.Width = 10;
            bul.Height = 2;
            bul.Fill = new SolidColorBrush(Colors.Red);
            bul.VerticalAlignment = VerticalAlignment.Top;
            bul.HorizontalAlignment = HorizontalAlignment.Left;
            bul.Margin = new Thickness(Character.charposleft + 20, Character.charposttop + 40, 0, 0);
            mriz.Children.Add(bul);
            Buletinf bullet = new Buletinf //log bullet to list of bullets
            {
                Left = Convert.ToInt32(bul.Margin.Left),
                Top = Convert.ToInt32(bul.Margin.Top),
                Side = Character.charonright,
                Ue = bul,
                El = bul
            };
            bulletlog.Add(bullet);
        }
        public static void Bulletchange(Grid mriz) //every time changing bullet position
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

                    foreach (Enemlog gay in Enemy.enemiesave) //if bullet cross enemy -> kill enemy -> bullet not destroyed
                    {
                        if (bullet.Left + bullet.El.Width > gay.Left && 
                            bullet.Top + bullet.El.Height > gay.Top &&
                            bullet.Left < gay.Right &&
                            bullet.Top < gay.Bottom)
                        {
                            Enemy.Destroy(mriz, gay);
                            Enemy.enemiesave.Remove(gay);
                            break;
                        }
                    }
                }

            }
        }
    }
    class Character // main chracter class
    {
        readonly int go_down_speed = 3; //go down speed - speed decresing in time
        readonly int char_width = 80; // player width
        readonly int char_height = 80; // player height
        public static bool charonright = true;
        public static int charposleft = 0;
        public static int charposttop = 0;
        readonly BitmapImage charleft = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/charleft.png"));
        readonly BitmapImage charright = new BitmapImage(new Uri("http://dod.vos-sps-jicin.cz/wp-content/uploads/simnsgame/charright.png"));
        readonly Rectangle c;
        public Character(Grid mriz)
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
        public void Echeck(Grid mriz) //Oncross enemy destroy -> not working in this time
        {
            int left = Convert.ToInt32(c.Margin.Left);
            int up = Convert.ToInt32(c.Margin.Top);
            foreach (Enemlog gay in Enemy.enemiesave)
            {
                if (left + char_width > gay.Left && up + char_height > gay.Top && left < gay.Right && up < gay.Bottom)
                {
                    Enemy.Destroy(mriz, gay);
                    Enemy.enemiesave.Remove(gay);
                    break;
                }
            }
        }
        public void Moveleft() //funcion to move left
        {
            bool inner = true;
            int left = Convert.ToInt32(c.Margin.Left);
            int up = Convert.ToInt32(c.Margin.Top);
            foreach (Walliind w in Wall.wali)
            {
                if (left - 1 < w.Wallright && up < w.Walldown && up + char_height > w.Wallup && left + char_width > w.Wallright)
                {
                    inner = false;
                    /*Margin to the wall only if not falling - fixed of falling and going right*/
                    if (left - 1 < w.Wallright && up < w.Walldown && up > w.Wallup && left + char_width > w.Wallright)
                    {
                        c.Margin = new Thickness(w.Wallright, up, 0, 0);
                    }
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
        public void Moveright() //funcion to move right
        {
            bool inner = true;
            int left = Convert.ToInt32(c.Margin.Left);
            int up = Convert.ToInt32(c.Margin.Top);
            foreach (Walliind w in Wall.wali)
            {
                if (left + 1 + char_width > w.Wallleft && up < w.Walldown && up + char_height > w.Wallup && left < w.Wallleft)
                {
                    inner = false;
                    /*Margin to the wall only if not falling - fixed of falling and going right*/
                    if (left + 1 + char_width > w.Wallleft && up < w.Walldown && up > w.Wallup && left < w.Wallleft)
                    {
                        c.Margin = new Thickness(w.Wallleft - char_width, up, 0, 0);
                    }
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
        public void Jumpe(int force) //funcion to fall and jump 
        {
            bool objectontop = false;
            bool objectondown = false;
            bool borderontop = false;
            bool borderondown = false;
            int left = Convert.ToInt32(c.Margin.Left);
            int up = Convert.ToInt32(c.Margin.Top);

            foreach (Walliind w in Wall.wali)
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