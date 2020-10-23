using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TwitchConfig;

namespace TwitchLiveChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            HandleCommandlineVars(Environment.GetCommandLineArgs());

            InitializeComponent();
            InitialFillListBox();
        }

        private void HandleCommandlineVars(string[] args)
        {
            if (Array.IndexOf(args, "--minimized") >= 0)
            {
                this.WindowState = WindowState.Minimized;
            }

        }

        private async void ButtonAdd_ClickAsync(object sender, RoutedEventArgs e)
        {
            Config config = Config.GetConfig();
            TwitchAPI apiclient = new TwitchAPI();

            AddChannelWindow addwindow = new AddChannelWindow(this);
            addwindow.ShowDialog();

            string channelname = addwindow.ChannelName.ToLower();


            if (!string.IsNullOrEmpty(addwindow.ChannelName))
            {
                List<TwitchChannel> channels = await apiclient.GetChannelsAsync(new string[] { channelname });
                ChannelListBox.Items.Add(channels[0]);
                config.AddChannel(channelname);
                config.Save();
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ChannelListBox.SelectedItem != null)
            {
                Config config = Config.GetConfig();
                TwitchChannel item = (ChannelListBox.SelectedItem as TwitchChannel);
                ChannelListBox.Items.Remove(item);
                config.RemoveChannel(item.Name);
            }

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

        private async void InitialFillListBox()
        {
            foreach (string channel in _cm.GetChannels())
            {
                ChannelListBox.Items.Add(new TwitchChannel(channel));
            }
            //await RefreshChannelStatus(true);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            _cm.Save();
        }

        private async void ButtonCheckChannels_ClickAsync(object sender, RoutedEventArgs e)
        {
            ButtonCheckChannels.IsEnabled = false;
            await RefreshChannelStatus();
            ButtonCheckChannels.IsEnabled = true;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshUpdateTimestamp()
        {
            string now = String.Format("{0:HH:mm:ss}", DateTime.Now);

            LastCheckedLabel.Content = $"Last checked: {now}";
        }

        private int? GetCorrespondingListItemIndex(string channel)
        {
            int index = 0;
            string chanelname = channel.ToLower();
            foreach (TwitchChannel listboxitem in ChannelListBox.Items)
            {
                if (listboxitem.Name.ToLower() == channelname)
                {
                    return index;
                }
                index++;
            }
            return null;
        }

        private async Task RefreshChannelStatus(bool notify = false)
        {
            TwitchChecker checker = new TwitchChecker(_cm.GetApiKey());

            foreach (string item in _cm.GetChannels())
            {
                int? itemindex = GetCorrespondingListItemIndex(item);
                TwitchChannel chan = await checker.CheckChannel(item);
                if (itemindex != null)
                {
                    ChannelListBox.Items.RemoveAt((int)itemindex);
                    ChannelListBox.Items.Insert((int)itemindex, chan);
                }
                else
                {
                    ChannelListBox.Items.Add(chan);
                }
                if (chan.Status != "offline")
                {
                    //todo: Notify
                }
            }
            RefreshUpdateTimestamp();
        }

        private void ChannelListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ChannelListBox.SelectedItem != null)
            {
                TwitchChannel channel = (TwitchChannel)ChannelListBox.SelectedItem;
                if (channel.Status == "live")
                {
                    System.Diagnostics.Process.Start($"https://twitch.tv/{channel.Name}");
                }
            }
        }
    }
}
