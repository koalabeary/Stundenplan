using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
using Haley.Abstractions;
using Haley.Services;

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

        public bool gespeichert = false;
        public event Action<FachErstellen> SpeichernErfolgreich;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string s;
            TimeSpan dauer;
            Color fa = farbe.SelectedColor;

            // Prüfe auf Korrekte Eingabe und gebe das Objekt weiter

            try
            {
                if (!string.IsNullOrWhiteSpace(Titel.Text))
                {
                    s = Titel.Text;
                    Times n = new Times();
                    dauer = n.umwandlung(Dauer.Text);
                    FachErstellen f = new FachErstellen(s,dauer,fa);
                    gespeichert = true;
                    SpeichernErfolgreich?.Invoke(f);
                    this.Close();
                }

                else { MessageBox.Show("Der Titel fehlt."); }
            }

            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }


    }


    
    public class FachErstellen
    {
        private static int counter;
        private int id;
        private string titel;
        private TimeSpan dauer;
        private Color farbe;

       

        public FachErstellen(string t, TimeSpan d, Color c)
        {
            this.id = ++counter;
            titel = t;
            farbe = c;
            dauer = d;
           
        }

        public int getID() { return this.id; }

        public string getTitel() { return titel; }

        public Color getColor() { return farbe; }

    }


}
