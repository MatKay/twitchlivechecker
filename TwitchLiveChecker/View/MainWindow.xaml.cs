using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TwitchConfig;

namespace TwitchLiveChecker
{
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

                if (channels.Count > 0)
                {
                    ChannelListBox.Items.Add(channels[0]);
                }
                else
                {
                    ChannelListBox.Items.Add(new TwitchChannel(channelname, "offline"));
                }

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
                config.RemoveChannel(item.user_name);
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
            Config config = Config.GetConfig();
            foreach (string channel in config.Channels)
            {
                ChannelListBox.Items.Add(new TwitchChannel(channel));
            }
            await RefreshChannelStatus(true);
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Config config = Config.GetConfig();

            List<string> channels = new List<string> { };
            foreach (TwitchChannel item in ChannelListBox.Items)
            {
                channels.Add(item.user_name);
            }
            
            config.Save();
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
            string channelname = channel.ToLower();
            foreach (TwitchChannel listboxitem in ChannelListBox.Items)
            {
                //exception? why

                if (listboxitem.user_name.ToLower() == channelname)
                {
                    return index;
                }
                index++;
            }
            return null;
        }

        private async Task RefreshChannelStatus(bool notify = false)
        {
            Config config = Config.GetConfig();
            TwitchAPI apiclient = new TwitchAPI();

            List<TwitchChannel> channels = await apiclient.GetChannelsAsync(config.Channels.ToArray());


            foreach (string item in config.Channels)
            {
                if (channels.Any(ch => ch.user_name.ToLower() == item))
                {
                    TwitchChannel updatedchannel = channels.Find(ch => ch.user_name == item);
                    int? itemindex = GetCorrespondingListItemIndex(item);
                    if (itemindex != null)
                    {
                        ChannelListBox.Items.RemoveAt((int)itemindex);
                        ChannelListBox.Items.Insert((int)itemindex, updatedchannel);
                    }
                    else
                    {
                        ChannelListBox.Items.Add(updatedchannel);
                    }
                }
                else
                {
                    TwitchChannel offlinechannel = new TwitchChannel(item, "offline");
                    int? itemindex = GetCorrespondingListItemIndex(item);
                    if (itemindex != null)
                    {
                        ChannelListBox.Items.RemoveAt((int)itemindex);
                        ChannelListBox.Items.Insert((int)itemindex, offlinechannel);
                    }
                    else
                    {
                        ChannelListBox.Items.Add(offlinechannel);
                    }
                }
            }
            RefreshUpdateTimestamp();
        }

        private void ChannelListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ChannelListBox.SelectedItem != null)
            {
                TwitchChannel channel = (TwitchChannel)ChannelListBox.SelectedItem;
                if (channel.type == "live")
                {
                    System.Diagnostics.Process.Start($"https://twitch.tv/{channel.user_name}");
                }
            }
        }
    }
}
