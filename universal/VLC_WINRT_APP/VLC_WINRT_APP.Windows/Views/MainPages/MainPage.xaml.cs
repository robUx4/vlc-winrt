﻿/**********************************************************************
 * VLC for WinRT
 **********************************************************************
 * Copyright © 2013-2014 VideoLAN and Authors
 *
 * Licensed under GPLv2+ and MPLv2
 * Refer to COPYING file of the official project for license
 **********************************************************************/

using System;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
using Windows.Media;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using VLC_WINRT_APP.Model;
using VLC_WINRT_APP.Services.RunTime;
using VLC_WINRT_APP.Services.Interface;
using VLC_WINRT_APP.ViewModels;
using VLC_WINRT_APP.Views.UserControls;
using VLC_WINRT_APP.Views.VariousPages;

namespace VLC_WINRT_APP.Views.MainPages
{
    public sealed partial class MainPage : SwapChainPanel
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += SwapPanelLoaded;
            //Locator.MediaPlaybackViewModel._mediaService.SetMediaTransportControls(SystemMediaTransportControls.GetForCurrentView());
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            Responsive();
            Locator.MediaPlaybackViewModel._mediaService.SetSizeVideoPlayer((uint)sizeChangedEventArgs.NewSize.Width, (uint)sizeChangedEventArgs.NewSize.Height);
        }

        void Responsive()
        {
        }

        private void SwapPanelLoaded(object sender, RoutedEventArgs e)
        {
            Locator.MediaPlaybackViewModel._mediaService.Initialize(SwapChainPanel);
            SizeChanged += OnSizeChanged;
            Unloaded += MainPage_Unloaded;
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= OnSizeChanged;
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            Responsive();
            SettingsPane pane = SettingsPane.GetForCurrentView();
            pane.CommandsRequested += SettingsCommandRequested;
        }

        private void SettingsCommandRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();
            var privacyCommand = new SettingsCommand("privacy", resourceLoader.GetString("PrivacyStatement"),
                async h => await Launcher.LaunchUriAsync(new Uri("http://videolan.org/vlc/privacy.html")));

            var specialThanks = new SettingsCommand("specialThanks", resourceLoader.GetString("SpecialThanks"),
                command =>
                {
                    App.ApplicationFrame.Navigate(typeof(SpecialThanks));
                });

            var settings = new SettingsCommand("settings", resourceLoader.GetString("Settings"),
                command =>
                {
                    if (Locator.MediaPlaybackViewModel.PlayingType == PlayingType.Video)
                    {
                        Locator.MediaPlaybackViewModel.GoBack.Execute(typeof(SettingsPage));
                    }
                });

            var about = new SettingsCommand("about", "About The App",
                command =>
                {
                    App.ApplicationFrame.Navigate(typeof(AboutPage));
                });
            var license = new SettingsCommand("license", "License", command =>
            {
                var licenseFlyout = new LicenseFlyout();
                licenseFlyout.Show();
            });
            args.Request.ApplicationCommands.Clear();
            args.Request.ApplicationCommands.Add(privacyCommand);
            args.Request.ApplicationCommands.Add(specialThanks);
            args.Request.ApplicationCommands.Add(settings);
            args.Request.ApplicationCommands.Add(about);
            args.Request.ApplicationCommands.Add(license);
        }
        private void CommandBar_OnHomeButtonClicked(Button button, EventArgs e)
        {
            if (App.ApplicationFrame.CurrentSourcePageType != typeof(MainPageHome))
                App.ApplicationFrame.Navigate(typeof(MainPageHome));
        }
    }
}
