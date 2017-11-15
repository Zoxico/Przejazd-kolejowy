using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls; 

namespace Pociag
{
    class Car
    {
        public bool czyMogeJechac = false;
        public int carID = 0;
        public TransformGroup ustawieniaStartoweAuta = new TransformGroup();
        public TranslateTransform przesuniecieAuta = new TranslateTransform(100, 100);
        public RotateTransform obrotO90 = new RotateTransform(-90.0);
        public ScaleTransform zmniejszAuto = new ScaleTransform(0.1, 0.1);
        //public List<> listaParametrow;
        public Image obrazek;

        delegate void ParametrizedMethodInvoker5(Canvas WizualizacjaInv);
        delegate void ParametrizedMethodInvoker6(Canvas WizualizacjaInv);
        public Car(MainWindow hook, int przekazaneID, Canvas Wizualizacja)
        {
            hook.Dispatcher.Invoke(new Action(() =>
            {
                obrazek = new Image()
                {
                    Source = new BitmapImage(new Uri(@"uzyje.png", UriKind.Relative))
                };

            }));


            //wrzucDoCanvasa(Wizualizacja);
            {
            //bool done = false;
            //var dispatcher = Wizualizacja.Dispatcher;

            //bool dostep = dispatcher.CheckAccess();         // to jest tylko żeby móc zrobić Add Watch
            //Action action;

            //while (!done)
            //    if (dispatcher.CheckAccess())
            //    {
            //        Wizualizacja.Children.Add(obrazek);
            //        ustawieniaStartoweAuta.Children.Add(zmniejszAuto);
            //        ustawieniaStartoweAuta.Children.Add(obrotO90);
            //        ustawieniaStartoweAuta.Children.Add(przesuniecieAuta);
            //        obrazek.RenderTransform = ustawieniaStartoweAuta;

            //        done = true;
            //    }
            //    else
            //    {
            //        Wizualizacja.Dispatcher.Invoke(new ParametrizedMethodInvoker5(Car), przekazaneID, Wizualizacja);
            //        //dispatcher.Invoke(action = () => Car(przekazaneID,Wizualizacja));
            //        return;
            //    }
            }


            //return Auto;
        }

        void wrzucDoCanvasa(Canvas WizualizacjaInv)
        {
            bool done = false;
            var dispatcher = WizualizacjaInv.Dispatcher;

            bool dostep = dispatcher.CheckAccess();         // to jest tylko żeby móc zrobić Add Watch
            Action action;

            while (!done)
                if (dispatcher.CheckAccess())
                {
                    var dispatcherObr = obrazek.Dispatcher;
                    Action actionObr;
                    if (dispatcherObr.CheckAccess())
                        WizualizacjaInv.Children.Add(obrazek);
                    else
                        //dispatcherObr.Invoke(actionObr = () => nieWiem(WizualizacjaInv));
                        WizualizacjaInv.Dispatcher.Invoke(new ParametrizedMethodInvoker6(nieWiem), WizualizacjaInv);


                    ustawieniaStartoweAuta.Children.Add(zmniejszAuto);
                    ustawieniaStartoweAuta.Children.Add(obrotO90);
                    ustawieniaStartoweAuta.Children.Add(przesuniecieAuta);
                    obrazek.RenderTransform = ustawieniaStartoweAuta;

                    done = true;
                }
                else
                {
                    //WizualizacjaInv.Dispatcher.Invoke(new ParametrizedMethodInvoker5(wrzucDoCanvasa), WizualizacjaInv);
                    dispatcher.Invoke(action = () => wrzucDoCanvasa(WizualizacjaInv));
                    return;
                }
        }

        public void nieWiem(Canvas Wizuwizu)
        {
            var disp = obrazek.Dispatcher; 

            if (disp.CheckAccess())
                Wizuwizu.Children.Add(obrazek);
            else
                Wizuwizu.Dispatcher.Invoke(new ParametrizedMethodInvoker6(nieWiem), Wizuwizu); 
        }

    }
}
