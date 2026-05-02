using System.CodeDom;
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
            String titel = Titel.Text;
            
            try
            {
                if (string.IsNullOrWhiteSpace(ende) || string.IsNullOrWhiteSpace(start)) {
                    throw new FormatException("Die Eingabe darf nicht leer sein.");
                }
                TimeSpan st = n.umwandlung(start);
                TimeSpan en = n.umwandlung(ende);

                
                if (st <= en)
                {
                    MessageBox.Show("Die Startzeit ist: " + st + "\nDie Endzeit ist: " + en);
                    Tabelle tabelle = new Tabelle(st, en, titel);
                    Leinwand = tabelle.erstellen(Leinwand);

                }
                else {
                    throw new FormatException("Die Startzeit muss vor der Endzeit sein.");
                }
            }

            catch (FormatException ex) {
                MessageBox.Show(ex.Message);
            }

        

        }

        private void BtnFachHinzufuegen_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Times
    {
        public TimeSpan umwandlung(string s)
        {

            char[] trenner = { ':', '.', '/', ';', ' ', '-', ',', '_'};
            string[] teile = s.Split(trenner);
            if (teile.Length == 2)
            {

                if (int.TryParse(teile[0], out int stunde) && int.TryParse(teile[1], out int minute))
                {
                    if (stunde >= 0 && stunde < 24 && minute >= 0 && minute < 60)
                    {
                        TimeSpan start = new TimeSpan(stunde, minute, 0);
                        return start;
                    }

                    throw new FormatException("Ungueltige Zeit.");
                }

                throw new FormatException("Das Format ist ungueltig, verwenden Sie beispielsweise 13:00 oder 13/00");
            }

            else { throw new FormatException("Das Format ist ungueltig, verwenden Sie beispielsweise 13:00 oder 13/00"); }



        }
    }

    public class Tabelle
    {
        private string titel;
        private DateOnly erstellungszeitpunkt;
        private TimeSpan begin;
        private TimeSpan end;

        public Tabelle(TimeSpan start, TimeSpan ende, string titel)
        {
            // Sicherstellen, dass Start vor Ende liegt
            this.begin = start;
            this.end = ende;
            this.titel = titel;
            this.erstellungszeitpunkt = DateOnly.FromDateTime(DateTime.Now);
        }

        public Canvas erstellen(Canvas leinwand)
        {
            leinwand.Children.Clear();
            leinwand.Background = Brushes.White; // Sauberer weißer Hintergrund

            // --- LAYOUT PARAMETER ---
            double randLinks = 80;      // Etwas mehr Platz für die Uhrzeiten
            double randOben = 160;
            double randRechts = 40;
            double nutzbareBreite = leinwand.Width - randLinks - randRechts;
            double spaltenBreite = nutzbareBreite / 5;
            double stundenHoehe = 60;
            int anzahlStunden = (int)Math.Ceiling((end - begin).TotalHours);

            // --- 1. TITEL & DATUM ---
            TextBlock titelLabel = new TextBlock
            {
                Text = titel.ToUpper(),
                FontSize = 32,
                FontWeight = FontWeights.Black,
                Foreground = new SolidColorBrush(Color.FromRgb(44, 62, 80)), // Dark Blue-Grey
                
            };
            Canvas.SetLeft(titelLabel, randLinks);
            Canvas.SetTop(titelLabel, 40);
            leinwand.Children.Add(titelLabel);

            TextBlock infoLabel = new TextBlock
            {
                Text = $"STUNDENPLAN • STAND: {erstellungszeitpunkt:dd.MM.yyyy}",
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Foreground = Brushes.LightSlateGray
            };
            Canvas.SetLeft(infoLabel, randLinks);
            Canvas.SetTop(infoLabel, 85);
            leinwand.Children.Add(infoLabel);

            // --- 2. ZEBRA-STREIFEN & UHRZEITEN ---
            for (int i = 0; i < anzahlStunden; i++)
            {
                double yPos = randOben + (i * stundenHoehe);

                // Abwechselnder Hintergrund (Zebra-Muster)
                if (i % 2 == 0)
                {
                    Rectangle rowBack = new Rectangle
                    {
                        Width = nutzbareBreite,
                        Height = stundenHoehe,
                        Fill = new SolidColorBrush(Color.FromRgb(249, 250, 251)) // Ganz helles Grau
                    };
                    Canvas.SetLeft(rowBack, randLinks);
                    Canvas.SetTop(rowBack, yPos);
                    leinwand.Children.Add(rowBack);
                }

                // Uhrzeit links
                TextBlock zeitLabel = new TextBlock
                {
                    Text = begin.Add(TimeSpan.FromHours(i)).ToString(@"hh\:mm"),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromRgb(127, 140, 141))
                };
                Canvas.SetLeft(zeitLabel, 20);
                Canvas.SetTop(zeitLabel, yPos + (stundenHoehe / 2) - 10);
                leinwand.Children.Add(zeitLabel);

                // Horizontale Trennlinie
                Line hLine = new Line
                {
                    X1 = randLinks,
                    X2 = randLinks + nutzbareBreite,
                    Y1 = yPos,
                    Y2 = yPos,
                    Stroke = new SolidColorBrush(Color.FromRgb(236, 240, 241)),
                    StrokeThickness = 1
                };
                leinwand.Children.Add(hLine);
            }
            // Abschlusslinie unten
            Line bottomLine = new Line
            {
                X1 = randLinks,
                X2 = randLinks + nutzbareBreite,
                Y1 = randOben + (anzahlStunden * stundenHoehe),
                Y2 = randOben + (anzahlStunden * stundenHoehe),
                Stroke = Brushes.LightGray,
                StrokeThickness = 1
            };
            leinwand.Children.Add(bottomLine);

            // --- 3. HEADER (WOCHENTAGE) ---
            Rectangle headerBar = new Rectangle
            {
                Width = nutzbareBreite,
                Height = 45,
                Fill = new SolidColorBrush(Color.FromRgb(52, 73, 94)), // Kräftiges Dunkelblau
                RadiusX = 4,
                RadiusY = 4
            };
            Canvas.SetLeft(headerBar, randLinks);
            Canvas.SetTop(headerBar, randOben - 45);
            leinwand.Children.Add(headerBar);

            string[] tage = { "MONTAG", "DIENSTAG", "MITTWOCH", "DONNERSTAG", "FREITAG" };
            for (int i = 0; i < 5; i++)
            {
                // Wochentag Text
                TextBlock tagText = new TextBlock
                {
                    Text = tage[i],
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontSize = 13,
                    TextAlignment = TextAlignment.Center,
                    Width = spaltenBreite
                };
                Canvas.SetLeft(tagText, randLinks + (i * spaltenBreite));
                Canvas.SetTop(tagText, randOben - 32);
                leinwand.Children.Add(tagText);

                // Vertikale Trennlinien
                if (i > 0)
                {
                    Line vLine = new Line
                    {
                        X1 = randLinks + (i * spaltenBreite),
                        X2 = randLinks + (i * spaltenBreite),
                        Y1 = randOben - 45,
                        Y2 = randOben + (anzahlStunden * stundenHoehe),
                        Stroke = new SolidColorBrush(Color.FromRgb(230, 233, 237)),
                        StrokeThickness = 1
                    };
                    leinwand.Children.Add(vLine);
                }
            }

            return leinwand;
        }
    }


}
