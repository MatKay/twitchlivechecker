using System.Windows;
using System.Windows.Controls;

namespace TwitchLiveChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConfigManager _cm;

        public MainWindow()
        {
            InitializeComponent();
            _cm = new ConfigManager();
            InitialFillListBox();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddChannelWindow addwindow = new AddChannelWindow(this);
            addwindow.ShowDialog();

            if (!string.IsNullOrEmpty(addwindow.ChannelName))
            {
                TwitchChannel chan = new TwitchChannel(addwindow.ChannelName);
                ChannelListBox.Items.Add(chan);
                _cm.AddChannel(addwindow.ChannelName);
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ChannelListBox.SelectedItem != null)
            {
                var item = (ChannelListBox.SelectedItem as TwitchChannel);
                ChannelListBox.Items.Remove(item);
                _cm.RemoveChannel(item.Name);
                string test = item.ToString();
                string asd = null;
            }

            //var element = ChannelListBox.

        }

        private void ChannelListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChannelListBox.SelectedItem != null)
            {
                ButtonRemove.IsEnabled = true;
            }
            else
            {
                ButtonRemove.IsEnabled = false;
            }
        }

        public bool CheckIfChannelInListbox(string name)
        {
            ItemCollection bla = ChannelListBox.Items;
            foreach (var item in bla)
            {
                // TODO
            }
            return false;
        }

        private void InitialFillListBox()
        {
            foreach (string channel in _cm.GetChannels())
            {
                ChannelListBox.Items.Add(new TwitchChannel(channel));
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            _cm.Save();
        }

        private void ButtonCheckChannels_Click(object sender, RoutedEventArgs e)
        {
            var asd = new TwitchChecker(_cm.GetApiKey());



            asd.CheckChannel("mat_kay");
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
