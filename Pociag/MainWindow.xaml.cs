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
using System.ComponentModel;
using System.Data;
using System.Drawing; 
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;
//using System.Windows.Forms;

namespace Pociag
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TransformGroup ustawieniaStartoweAuta = new TransformGroup();
        TranslateTransform przesuniecieAuta = new TranslateTransform(100, 100);
        //TranslateTransform przesuniecieAuta2 = new TranslateTransform(100, 100);
        RotateTransform obrotO90 = new RotateTransform(-90.0);
        ScaleTransform zmniejszAuto = new ScaleTransform(0.1, 0.1);
        List<Thread> _listaWatkowAut = new List<Thread>();
        Thread ciopong;
        List<Car> _listaAut = new List<Car>();
        List<Train> _pociagi = new List<Train>();
        int[] tablicaAut = new int[100];
        int licznik = 0;
        int gl_idAuta = 0;
        int odlX = 100;
        int odlPociagu = -4500;
        int iloscDzieci = 0;
        bool czyJedziePociag = false;
        Random rnd; //Random generator




        //[STAThread]
        //static void Main()
        //{
        //	Application.EnableVisualStyles();
        //	Application.SetCompatibleTextRenderingDefault(false);
        //	Application.Run(new Form1());
        //}

        public MainWindow()
        {

            InitializeComponent();
        }

        void WatekAuta(int idAuta, MainWindow hook)
        {
            int odleglosc = 50;
            double top = 0;//           
            Car Auto = new Car(hook, idAuta, Wizualizacja);       //Tworzymy auto
            _listaAut.Add(Auto);                            //dodajemy do listy
            idAuta--;
            //iloscDzieci = idAuta;
            //TransformGroup ustawieniaStartoweAuta = new TransformGroup();


            Auto.ustawieniaStartoweAuta.Children.Add(Auto.zmniejszAuto);
            Auto.ustawieniaStartoweAuta.Children.Add(Auto.obrotO90);
            Auto.ustawieniaStartoweAuta.Children.Add(Auto.przesuniecieAuta);



            if (this.Wizualizacja.Dispatcher.CheckAccess())
            {
                Wizualizacja.Children.Add(Auto.obrazek);
            }
            else
            {
                Wizualizacja.Dispatcher.Invoke(new Action(() =>
                {                   

                    Wizualizacja.Children.Add(Auto.obrazek);

                }));
            }


            Wizualizacja.Dispatcher.Invoke(new Action(() =>
            {
                Canvas.SetTop(Auto.obrazek, 100);
                Canvas.SetLeft(Auto.obrazek, odlX);
                odlX = odlX + 60;
                if (odlX >= 280)
                    odlX = 100;
            }));


            while (_listaWatkowAut[idAuta].IsAlive)
            {
                Wizualizacja.Dispatcher.Invoke(new Action(() =>
                {
                    top = Canvas.GetTop((UIElement)Auto.obrazek);
                }));

                if (czyJedziePociag && top < 450)
                {
                    Auto.czyMogeJechac = false;
                    if (top > 300)
                        if (_listaWatkowAut[idAuta].IsAlive)
                            _listaWatkowAut[idAuta].Abort();
                }

                else
                    Auto.czyMogeJechac = true;

                if (Auto.czyMogeJechac)
                {

                    Wizualizacja.Dispatcher.Invoke(new Action(() =>
                    { 
                        Canvas.SetTop(Auto.obrazek, odleglosc);

                    })); 
                    odleglosc++;
                }
                Thread.Sleep(10);

                if(odleglosc > 600)
                {
                    
                    Wizualizacja.Dispatcher.Invoke(new Action(() =>
                    {
                    Wizualizacja.Children.RemoveAt(1);// iloscDzieci);
                        iloscDzieci--;
                    }));

                    if (_listaWatkowAut[idAuta].IsAlive)
                        _listaWatkowAut[idAuta].Abort();
                }
            }

        }



        delegate void ParametrizedMethodInvoker5(int idAuta);
        delegate void ParametrizedMethodInvoker6(List<Image> _listaAut, int _idAuta);
        delegate void ParametrizedMethodInvoker7(int _idAuta);
        delegate void ParametrizedMethodInvoker8(int _idAuta, Canvas Wizualizacja);



        private void XD_Click(object sender, RoutedEventArgs e)
        {
            _listaWatkowAut.Add(new Thread(() => WatekAuta(licznik, this)));
            _listaWatkowAut[licznik].SetApartmentState(ApartmentState.STA);
            _listaWatkowAut[licznik].Start();  //}
            licznik++;
            //iloscDzieci++;

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            foreach (Thread watek in _listaWatkowAut)
            {
                if (watek.IsAlive)
                    watek.Abort();
            }

            if (ciopong.IsAlive)
                ciopong.Abort();


        }

 

        private int RuszAutem(int _idAuta, int odleglosc)
        {


            _listaAut[_idAuta].ustawieniaStartoweAuta.Children.Clear();

            _listaAut[_idAuta].przesuniecieAuta.Y = odleglosc + 2;
            _listaAut[_idAuta].ustawieniaStartoweAuta.Children.Add(_listaAut[_idAuta].zmniejszAuto);
            _listaAut[_idAuta].ustawieniaStartoweAuta.Children.Add(_listaAut[_idAuta].obrotO90);
            _listaAut[_idAuta].ustawieniaStartoweAuta.Children.Add(_listaAut[_idAuta].przesuniecieAuta);


            return (int)_listaAut[_idAuta].przesuniecieAuta.Y;
        }

        Action emptyDelegate = delegate { };
        private void StartAnimationButton_Click(object sender, RoutedEventArgs e)
		{
            foreach (Car samochod in _listaAut)
                samochod.czyMogeJechac = true;
            foreach (Train ciopong in _pociagi)
                ciopong.czyMogeJechac = true;

        }

        private void Train_btn_Click(object sender, RoutedEventArgs e)
        {
            ciopong = new Thread(() => WatekPociagu(this));
            ciopong.SetApartmentState(ApartmentState.STA);
            ciopong.Start();
        }

        private void WatekPociagu(MainWindow mainWindow)
        {
            Train ciopong = new Train(mainWindow, Wizualizacja);
            _pociagi.Add(ciopong);
            //double odleglosc = -400;




            ciopong.ustawieniaStartowePociagu.Children.Add(ciopong.zmniejszPociagu);
            ciopong.ustawieniaStartowePociagu.Children.Add(ciopong.przesunieciePociagu);



            if (this.Wizualizacja.Dispatcher.CheckAccess())
            {
                Wizualizacja.Children.Add(ciopong.obrazek);
            }
            else
            {
                Wizualizacja.Dispatcher.Invoke(new Action(() =>
                {

                    Wizualizacja.Children.Add(ciopong.obrazek);

                }));
            }


            Wizualizacja.Dispatcher.Invoke(new Action(() =>
            {
                Canvas.SetTop(ciopong.obrazek, 100);
                Canvas.SetLeft(ciopong.obrazek, -1000);
            }));


            while (odlPociagu < 800)
            {
                ciopong.czyMogeJechac = true;
                czyJedziePociag = true;

                if (ciopong.czyMogeJechac)
                {

                    Wizualizacja.Dispatcher.Invoke(new Action(() =>
                    {
                        Canvas.SetLeft(ciopong.obrazek, odlPociagu);

                    }));
                    odlPociagu = odlPociagu + 3;
                    //if (odlPociagu > 400)
                    //    odlPociagu = -3500;
                }
                Thread.Sleep(10);

                if (odlPociagu > 400)
                {

                    //Wizualizacja.Dispatcher.Invoke(new Action(() =>
                    //{
                    //    Wizualizacja.Children.RemoveAt(1);// iloscDzieci);
                    //    iloscDzieci--;
                    //}));

                    //if (_listaWatkowAut[idAuta].IsAlive)
                    //    _listaWatkowAut[idAuta].Abort();
                }
            }
            czyJedziePociag = false;
        }
    }
}
