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
    class Train
    {
        public bool czyMogeJechac = false;
        public TransformGroup ustawieniaStartowePociagu = new TransformGroup();
        public TranslateTransform przesunieciePociagu = new TranslateTransform(-300, 100);
        public RotateTransform obrotO90 = new RotateTransform(-90.0);
        public ScaleTransform zmniejszPociagu = new ScaleTransform(0.1, 0.1);
        //public List<> listaParametrow;
        public Image obrazek;

        delegate void ParametrizedMethodInvoker5(Canvas WizualizacjaInv);
        delegate void ParametrizedMethodInvoker6(Canvas WizualizacjaInv);
        public Train(MainWindow hook, Canvas Wizualizacja)
        {
            hook.Dispatcher.Invoke(new Action(() =>
            {
                obrazek = new Image()
                {
                    Source = new BitmapImage(new Uri(@"pociag.png", UriKind.Relative))
                };

            }));


        }



        //public void nieWiem(Canvas Wizuwizu)
        //{
        //    var disp = obrazek.Dispatcher;

        //    if (disp.CheckAccess())
        //        Wizuwizu.Children.Add(obrazek);
        //    else
        //        Wizuwizu.Dispatcher.Invoke(new ParametrizedMethodInvoker6(nieWiem), Wizuwizu);
        //}

    }
}
