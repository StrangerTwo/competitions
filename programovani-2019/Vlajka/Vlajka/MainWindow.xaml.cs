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

namespace Vlajka
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int stage = -1;

        Rectangle bilyObdelnik;
        Polygon cervenyPolygon;
        Polygon modryPolygon;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                stage++;

                switch (stage)
                {
                    case 0:
                        bilyObdelnik = new Rectangle();
                        bilyObdelnik.Fill = Brushes.White;

                        platno.Children.Add(bilyObdelnik);
                        break;
                    case 1:
                        cervenyPolygon = new Polygon();
                        cervenyPolygon.Fill = Brushes.Red;
                        
                        platno.Children.Add(cervenyPolygon);
                        break;
                    case 2:
                        modryPolygon = new Polygon();
                        modryPolygon.Fill = Brushes.Blue;
                        
                        platno.Children.Add(modryPolygon);
                        break;
                }
                //aktualizuj velikosti obrázku
                setDrawingSize();
            }
        }

        private void setDrawingSize()
        {
            if (bilyObdelnik != null)
            {
                bilyObdelnik.Width = platno.ActualWidth;
                bilyObdelnik.Height = platno.ActualHeight;
            }
            if (cervenyPolygon != null)
            {
                cervenyPolygon.Points = new PointCollection(new List<Point>() { new Point(0, platno.ActualHeight), new Point(platno.ActualWidth, platno.ActualHeight), new Point(platno.ActualWidth, platno.ActualHeight / 2), new Point(platno.ActualWidth / 3, platno.ActualHeight / 2) });
            }
            if (modryPolygon != null)
            {
                modryPolygon.Points = new PointCollection(new List<Point>() { new Point(0, 0), new Point(0, platno.ActualHeight), new Point(platno.ActualWidth / 3, platno.ActualHeight / 2) });
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size newSize = e.NewSize;

            double columns = (int)(newSize.Width / 50);
            double rows = (int)(newSize.Height / 50);

            if (columns < 12)
            {
                columns = 12;
            }
            if (columns > 24)
            {
                columns = 24;
            }
            if (rows < 8)
            {
                rows = 8;
            }
            if (rows > 16)
            {
                rows = 16;
            }

            if (columns * 2 < rows * 3)
            {
                rows = columns * 2 / 3;
            }
            else
            {
                columns = rows * 3 / 2;
            }

            platno.Width = columns * 50;
            platno.Height = rows * 50;

            //aktualizuj velikosti obrázku
            setDrawingSize();
        }
    }
}
