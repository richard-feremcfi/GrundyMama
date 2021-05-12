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
using System.Windows.Media.Media3D;


namespace GrundyMama
{
    public partial class MainWindow : Window
    {
        
        
       public static Random random;

        int gamestate = 0;

        string playerOne;
        string playerTwo;

        bool turn;

        int meshbuttoncount = 0;

        //"kupacok" lista létrehozása, hogy legyen miben eltárolni az áthelyezendő gombok számát
        public List<Kupac> kupacok = new List<Kupac>();

        List<Gomb> cursorgombok = new List<Gomb>();



        int prevkupac = -1;

        //Egy gomb 3D modelljét tárolja el

        Point3D[] buttonMeshPoints = new Point3D[]
        {
            new Point3D(-0.05f,-0.05f,0),
            new Point3D(0.05f,-0.05f,0),
            new Point3D(-0.05f,0.05f,0),
            new Point3D(0.05f,0.05f,0)
        };

        Vector3D[] buttonMeshNormals = new Vector3D[]
       {
            new Vector3D(0,0,1),
            new Vector3D(0,0,1),
            new Vector3D(0,0,1),
            new Vector3D(0,0,1)
       };
        //hogyan használja a textúrákat
        Point[] buttonMeshUvs = new Point[]
        {
            new Point(0,0),
            new Point(1,0),
            new Point(0,1),
            new Point(1,1)
        };
        //milyen sorrendben kösse össze a pontokat 
        int[] buttonMeshIndices = new int[]
        {
            0,1,2,3,2,1
        };


        //összes gomb megjelenítése kupacokban 

        void DrawAllKupac()
        {
            ClearButtons();
            for (int i = 0;i < kupacok.Count;i++)
            {
                DrawKupac(kupacok[i]);
            }
        }

        //egyéni kupacok kirajzolása
        void DrawKupac(Kupac kupac)
        {
            for (int i = 0;i < kupac.gombok.Count;i++)
            {
                DrawButton(new Point(kupac.pos.X + kupac.gombok[i].relativePos.X *Math.Pow(kupac.gombok.Count - 1, 1.0f / 5.0f), kupac.pos.Y + kupac.gombok[i].relativePos.Y * Math.Pow(kupac.gombok.Count - 1, 1.0f / 5.0f)));
            }
        }


        //gomnok eltüntetése

        void ClearButtons()
        {
            ButtonGeometry.TextureCoordinates.Clear();
            ButtonGeometry.Positions.Clear();
            ButtonGeometry.TriangleIndices.Clear();
            ButtonGeometry.Normals.Clear();
            meshbuttoncount = 0;
        }

        void DrawButton(Point pos)
        {
            Point3D pos3D = new Point3D(pos.X, pos.Y, meshbuttoncount * 0.001f);
            for (int i = 0; i < buttonMeshUvs.Length; i++)
            {
                ButtonGeometry.TextureCoordinates.Add(buttonMeshUvs[i]);
            }
            int indiceOffset = meshbuttoncount * buttonMeshPoints.Length;
            for (int i = 0; i < buttonMeshIndices.Length; i++)
            {

                ButtonGeometry.TriangleIndices.Add(buttonMeshIndices[i] + indiceOffset);
            }
            for (int i = 0; i < buttonMeshPoints.Length; i++)
            {
                Point3D vertexPos = buttonMeshPoints[i];
                ButtonGeometry.Positions.Add(new Point3D(vertexPos.X + pos3D.X, vertexPos.Y + pos3D.Y, vertexPos.Z + pos3D.Z));
            }
            for (int i = 0; i < buttonMeshNormals.Length; i++)
            {
                ButtonGeometry.Normals.Add(buttonMeshNormals[i]);
            }
            meshbuttoncount++;
        }


        //ablak kezdő pozíció indítása

        public MainWindow()
        {
            random = new Random();
            InitializeComponent();
            NameConfirmButton.IsEnabled = false;
            NameConfirmButton.Visibility = Visibility.Hidden;
            PlayerNameInput.IsEnabled = false;
            PlayerNameInput.Visibility = Visibility.Hidden;
            InfoLabel.IsEnabled = false;
            InfoLabel.Visibility = Visibility.Hidden;
            GameInput.IsEnabled = false;
            GameInput.Visibility = Visibility.Hidden;
            Player1Indicator.IsEnabled = false;
            Player1Indicator.Visibility = Visibility.Hidden;
            Player2Indicator.IsEnabled = false;
            Player2Indicator.Visibility = Visibility.Hidden;
            WinnerDisplay.IsEnabled = false;
            WinnerDisplay.Visibility = Visibility.Hidden;
            AGAINBUTTON.IsEnabled = false;
            AGAINBUTTON.Visibility = Visibility.Hidden;
        }
        //start utáni pozíció leírása
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Button button = (Button)sender;
            button.IsEnabled = false;
            button.Visibility = Visibility.Hidden;
            gamestate = 1;
            NameConfirmButton.IsEnabled = true;
            NameConfirmButton.Visibility = Visibility.Visible;
            PlayerNameInput.IsEnabled = true;
            PlayerNameInput.Visibility = Visibility.Visible;
            InfoLabel.IsEnabled = true;
            InfoLabel.Visibility = Visibility.Visible;
            InfoLabel.Content = "Első Játékos Neve:";

        }
        //a második játékos nevének megerősításe után el indul a játék
        private void NameConfirmButton_Click(object sender, RoutedEventArgs e)
        {


            //második játékos nevének megadása

            if (gamestate == 1)
            {
                playerOne = PlayerNameInput.Text;
                PlayerNameDisplay1.Content = playerOne;
                if (!string.IsNullOrWhiteSpace(playerOne))
                {
                    gamestate = 2;
                    PlayerNameInput.Text = "";
                    InfoLabel.Content = "Második Játékos Neve:";
                }
            }
            else
            {
                if (gamestate == 2)
                {
                    playerTwo = PlayerNameInput.Text;
                    PlayerNameDisplay2.Content = playerTwo;
                    if (!string.IsNullOrWhiteSpace(playerTwo))
                    {
                        gamestate = 3;
                        PlayerNameInput.Text = "";
                        NameConfirmButton.IsEnabled = false;
                        NameConfirmButton.Visibility = Visibility.Hidden;
                        PlayerNameInput.IsEnabled = false;
                        PlayerNameInput.Visibility = Visibility.Hidden;
                        InfoLabel.IsEnabled = false;
                        InfoLabel.Visibility = Visibility.Hidden;
                        GameInput.IsEnabled = true;
                        GameInput.Visibility = Visibility.Visible;



                        if (random.NextDouble() < 0.5)
                        {
                            turn = true;
                        }
                        else
                        {
                            turn = false;
                        }

                        

                        if (turn)
                        {
                            Player1Indicator.IsEnabled = true;
                            Player1Indicator.Visibility = Visibility.Visible;
                            Player2Indicator.IsEnabled = false;
                            Player2Indicator.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            Player1Indicator.IsEnabled = false;
                            Player1Indicator.Visibility = Visibility.Hidden;
                            Player2Indicator.IsEnabled = true;
                            Player2Indicator.Visibility = Visibility.Visible;
                        }


                        //kezdő mennyiség
                        Kupac kupac = new Kupac(10,new Point(0,0));
                        kupacok.Add(kupac);
                        DrawAllKupac();



                    }
                }
            }



        }


        private void GameInput_Click(object sender, RoutedEventArgs e)
        {

            
            int kupac = findKupac();
            if (kupac != -1)
            {

                    if (prevkupac == kupac || prevkupac < 0)
                    {

                        Kupac thiskup = kupacok[kupac];
                        if (thiskup.gombok.Count > 2)
                        {
                            prevkupac = kupac;
                            cursorgombok.Add(thiskup.gombok[thiskup.gombok.Count - 1]);
                            thiskup.gombok.RemoveAt(thiskup.gombok.Count - 1);
                            if (thiskup.gombok.Count == cursorgombok.Count)
                            {
                                cursorgombok.Add(thiskup.gombok[thiskup.gombok.Count - 1]);
                                thiskup.gombok.RemoveAt(thiskup.gombok.Count - 1);
                            }
                            DrawAllKupac();
                            CursorButtonCount.Content = cursorgombok.Count.ToString();

                        }
                        else
                        {
                            if (cursorgombok.Count < 1)
                            {
                                kupacok.RemoveAt(kupac);
                                DrawAllKupac();
                            }
                        }
                    }
            }
            else
            {
                // egér poziciójának megállapítása majd kupacok listához való rendelés

                if (cursorgombok.Count > 0) {
                    Point MousePos = Mouse.GetPosition(GameView);

                    MousePos = new Point((MousePos.X / GameView.Width - 0.5f) * 2.0f, ((1 - MousePos.Y / (float)GameView.Height) - 0.5f) * 1.05f);

                    Kupac kupacc = new Kupac(cursorgombok.Count, MousePos);
                    cursorgombok.Clear();
                    CursorButtonCount.Content = cursorgombok.Count.ToString();
                    kupacok.Add(kupacc);
                    DrawAllKupac();


                    prevkupac = -1;
                    turn = !turn;
                    //kör váltás
                    if (turn)
                    {
                        Player1Indicator.IsEnabled = true;
                        Player1Indicator.Visibility = Visibility.Visible;
                        Player2Indicator.IsEnabled = false;
                        Player2Indicator.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        Player1Indicator.IsEnabled = false;
                        Player1Indicator.Visibility = Visibility.Hidden;
                        Player2Indicator.IsEnabled = true;
                        Player2Indicator.Visibility = Visibility.Visible;
                    }

                    //aktuális játék állas megvizsgálása/nyertes megállapítása
                    bool win = true;
                    for (int i = 0;i < kupacok.Count;i++)
                    {
                        if (kupacok[i].gombok.Count > 2)
                        {
                            win = false;
                        }
                    }


                    if (win)
                    {
                        Player1Indicator.IsEnabled = false;
                        Player1Indicator.Visibility = Visibility.Hidden;
                        Player2Indicator.IsEnabled = false;
                        Player2Indicator.Visibility = Visibility.Hidden;
                        WinnerDisplay.IsEnabled = true;
                        WinnerDisplay.Visibility = Visibility.Visible;
                        AGAINBUTTON.IsEnabled = true;
                        AGAINBUTTON.Visibility = Visibility.Visible;
                        if (turn) {
                            WinnerDisplay.Text = "A győztes:\n" + playerTwo;
                        }
                        else
                        {
                            WinnerDisplay.Text = "A győztes:\n" + playerOne;
                        }
                    }







                }



            }
            
            
        }

        //egyesével való gombok felvétele

        int findKupac()
        {
            Point MousePos = Mouse.GetPosition(GameView);

            MousePos = new Point((MousePos.X / GameView.Width - 0.5f) * 2.0f, ((1 - MousePos.Y / (float)GameView.Height) - 0.5f) * 1.05f);


            // AddButton(new Point(MousePos.X, MousePos.Y));

            float lastdist = 999999.0f;
            int closestINDX = -1;
            for (int i = 0; i < kupacok.Count; i++)
            {
                float limit = (float)Math.Pow(kupacok[i].gombok.Count, 1.0f / 5.0f) * 0.05f;
                float nowdist = distance(MousePos, kupacok[i].pos);
                if (limit > nowdist)
                {
                    if (nowdist < lastdist)
                    {
                        lastdist = nowdist;
                        closestINDX = i;
                    }
                }
            }
            return closestINDX;
        }




        //elhelyezésük

        float distance(Point p1, Point p2)
        {
            float dist = 0;
            double xdiff = p1.X - p2.X;
            double ydiff = p1.Y - p2.Y;
            dist = (float)Math.Sqrt(xdiff * xdiff + ydiff * ydiff);


            return dist;
        }
        //WIN BUTTON
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {



            NameConfirmButton.IsEnabled = false;
            NameConfirmButton.Visibility = Visibility.Hidden;
            PlayerNameInput.IsEnabled = false;
            PlayerNameInput.Visibility = Visibility.Hidden;
            InfoLabel.IsEnabled = false;
            InfoLabel.Visibility = Visibility.Hidden;
            GameInput.IsEnabled = false;
            GameInput.Visibility = Visibility.Hidden;
            Player1Indicator.IsEnabled = false;
            Player1Indicator.Visibility = Visibility.Hidden;
            Player2Indicator.IsEnabled = false;
            Player2Indicator.Visibility = Visibility.Hidden;
            WinnerDisplay.IsEnabled = false;
            WinnerDisplay.Visibility = Visibility.Hidden;
            AGAINBUTTON.IsEnabled = false;
            AGAINBUTTON.Visibility = Visibility.Hidden;
            StartButton.IsEnabled = true;
            StartButton.Visibility = Visibility.Visible;
            gamestate = 0;
            kupacok.Clear();
            DrawAllKupac();

        }
    }

    public class Gomb
    {
        public Point relativePos;
    }
    //gombok lista létrehozása
    public class Kupac
    {
        public List<Gomb> gombok= new List<Gomb>();
        public Point pos;
        public float colliderRad;


        public Kupac(int count, Point Pos)
        {

            for (int i = 0;i < count;i++)
            {
                Gomb gomb = new Gomb();
                
                gombok.Add(gomb);
            }
            pos = Pos;
            CalcRandomPoses();
    
        }
        //pontok véletlenszerű elhelyezése
        void CalcRandomPoses()
        {
            for (int i = 0; i < gombok.Count; i++)
            {
                Point p = new Point((MainWindow.random.NextDouble() - 0.5f) * 0.1f, (MainWindow.random.NextDouble() - 0.5f) * 0.1f);
                float pl = (float)Math.Sqrt((float)(p.X * p.X + p.Y * p.Y));
                
                p.Y /= pl * 10;
                p.X /= pl * 10;
                p.Y *= (float)(MainWindow.random.NextDouble() - 0.5f);
                p.X *= (float)(MainWindow.random.NextDouble() - 0.5f);
                gombok[i].relativePos = new Point(p.X,p.Y);
            }
        }



    }



    

}
