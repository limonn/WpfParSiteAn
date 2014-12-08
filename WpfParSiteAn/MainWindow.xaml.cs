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
        bool in_progress = false;
        public MainWindow()
        {
            InitializeComponent();
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

            //IList<String> sites = SitesEdit.GetAllNonEmptyLines();
            //IList<String> keywords = KeywordsEdit.GetAllNonEmptyLines();
            
            //var taskArray = sites.Select(x => Task<IDictionary<string, int>>.Factory.StartNew(() => GetKeywordCount(keywords, x))).ToList(); // get list of tasks. ToList forces lazy init. Otherwise tasks will be started when first accessed i.e. in foreach and because task.Result is synchronous operation it will lead to sequental site parsing
            //var keywordCounts = new List<string>();
            //for (var i = 0; i < taskArray.Count; i++)
            //{
            //    var task = taskArray[i];
            //    foreach (var keywordCount in task.Result)
            //    {
            //        keywordCounts.Add("On site '" + sites[i] + "' keyword '" + keywordCount.Key + "' was found " + keywordCount.Value + " times");
            //    }
            //}
            //updater(keywordCounts);
            //var analyseButton = ((Button)sender);
            ////analyseButton.IsEnabled = false;

            //Status.Content = "Status: processing...";

            //Task.Factory.StartNew(() => SiteParser.AnalyseSites(SitesEdit.GetAllNonEmptyLines(), KeywordsEdit.GetAllNonEmptyLines(), x =>
            //{
            //    Status.Dispatcher.BeginInvoke(new Action(delegate() // WPF does not allow to work with contorl in thread that is not owner of the control
            //    {
            //        Status.Content = "Status: done";
            //        Result.Content = string.Join("\n", x);
            //        //analyseButton.IsEnabled = true;
            //    }));
            //}));
            
        }
    }
}
