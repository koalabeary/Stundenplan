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
    }

    public class Times {

        int startHour;
        int startMinute;
        int endHour;
        int endMinute;
        
        public TimeSpan Start => new TimeSpan(startHour, startMinute, 0);
        public TimeSpan End => new TimeSpan(endHour, endMinute, 0);

    }
}