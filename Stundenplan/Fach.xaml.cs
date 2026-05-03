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
using System.Windows.Shapes;

namespace Stundenplan
{
    /// <summary>
    /// Interaktionslogik für Fach.xaml
    /// </summary>
    public partial class Fach : Window
    {
        public Fach()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Titel.Text)) {
                string s = Titel.Text;
                Times n = new Times();
                n.umwandlung(Dauer.Text);

            }
            
        }
    }

    public class FachErstellen
    {
        string titel;
        int farbe;
        TimeSpan dauer;

        public FachErstellen(string t, int f, TimeSpan d)
        {
            titel = t;
            farbe = f;
            dauer = d;
        }

    }
}
