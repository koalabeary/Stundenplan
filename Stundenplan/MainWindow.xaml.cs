using System.CodeDom;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Stundenplan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Erstellen_Click(object sender, RoutedEventArgs e)
        {
            String start = Start.Text;
            String ende = Ende.Text;
            Times n = new Times();
            TimeSpan st = n.umwandlung(start);
            TimeSpan en = n.umwandlung(ende);
            MessageBox.Show("Die Startzeit ist: " + st + "\nDie Endzeit ist: " + en);


        }
    }

    public class Times
    {
       public TimeSpan umwandlung(string s) {
            try
            {
                char[] trenner = { ':', '.', '/', ';', ' ', '-', ',' };
                string[] teile = s.Split(trenner);
                if (teile.Length == 2)
                {

                    if (int.TryParse(teile[0], out int stunde) && int.TryParse(teile[1], out int minute))
                        if (stunde >= 0 && stunde < 24 && minute >= 0 && minute < 60)
                        {
                            TimeSpan start = new TimeSpan(stunde, minute, 0);
                            return start;
                        }
                }

                throw new FormatException("Das Format ist ungültig. Bitte HH:mm nutzen.");
            }

            catch(Exception ex)
            {
                throw new Exception("Ungültige Eingabe: " + ex.Message);
            }
        }
    }



    
}
