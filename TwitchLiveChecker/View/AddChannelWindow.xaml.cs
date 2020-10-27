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

namespace TwitchLiveChecker
{
    /// <summary>
    /// Interaction logic for AddChannel.xaml
    /// </summary>
    public partial class AddChannelWindow : Window
    {
        private string channelname;

        public string ChannelName { get => channelname;}

        private MainWindow parent;

        public AddChannelWindow(MainWindow mw)
        {
            parent = mw;
            InitializeComponent();
            ChannelAddTextBox.Focus();
        }

        private void ChannelAddOK_Click(object sender, RoutedEventArgs e)
        {
            channelname = ChannelAddTextBox.Text;
            this.Close();
        }

        private void ChannelAddCancel_Click(object sender, RoutedEventArgs e)
        {
            channelname = null;
            this.Close();
        }

        private void ChannelAddTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ( String.IsNullOrEmpty(ChannelAddTextBox.Text) 
                 || String.IsNullOrWhiteSpace(ChannelAddTextBox.Text) 
                 || parent.CheckIfChannelInListbox(ChannelAddTextBox.Text)
               )
            {
                ChannelAddOK.IsEnabled = false;
            }
            else
            {
                ChannelAddOK.IsEnabled = true;
            }
        }
    }
}
