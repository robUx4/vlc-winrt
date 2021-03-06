﻿using System;
using Windows.Networking.Connectivity;
using VLC_WinRT.Model.Events;

namespace VLC_WinRT.Services.RunTime
{
    class NetworkListenerService
    {
        public event EventHandler<InternetConnectionChangedEventArgs> InternetConnectionChanged;

        public NetworkListenerService()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (InternetConnectionChanged == null) return;
            NotifyNetworkChanged();
        }

        void NotifyNetworkChanged()
        {
            var arg = new InternetConnectionChangedEventArgs(IsConnected);
            InternetConnectionChanged(null, arg);
        }
        
        public static bool IsConnected
        {
            get
            {
                var profile = NetworkInformation.GetInternetConnectionProfile();
                var isConnected = (profile != null && profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
                return isConnected;
            }
        }
    }
}
