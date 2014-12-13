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
using System.Net;
using System.Threading;

namespace WpfParSiteAn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool in_progress;
        public MainWindow()
        {
            InitializeComponent();
            in_progress = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ts = new CancellationTokenSource();
            if (in_progress)
            {
                ts.Cancel();
                Status.Content = "Status: Cancelled";
                in_progress = false;
            }
            else
                in_progress = true;

            CancellationToken ct = ts.Token;
            Task.Factory.StartNew(() => SiteParser.AnalyseSites(SitesEdit.GetAllNonEmptyLines(), KeywordsEdit.GetAllNonEmptyLines(), x =>
            {
                Status.Dispatcher.BeginInvoke(new Action(delegate() // WPF does not allow to work with contorl in thread that is not owner of the control
                {
                    Status.Content = "Status: done";
                    Result.Content = string.Join("\n", x);
                }));
            }, ct));

            in_progress = false;
        }
    }
}
