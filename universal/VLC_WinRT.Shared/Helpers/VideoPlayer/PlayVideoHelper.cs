﻿using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using VLC_WinRT.Model.Video;
using VLC_WinRT.ViewModels;
using VLC_WinRT.ViewModels.VideoVM;
using VLC_WinRT.Views.VideoPages;

namespace VLC_WinRT.Helpers.VideoPlayer
{
    public static class PlayVideoHelper
    {
        public static async Task Play(this VideoItem videoVm)
        {
            if (string.IsNullOrEmpty(videoVm.Token))
            {
                string token = StorageApplicationPermissions.FutureAccessList.Add(videoVm.File);
                LogHelper.Log("PLAYVIDEO: Getting video path token");
                videoVm.Token = token;
            }
            if (App.ApplicationFrame.CurrentSourcePageType != typeof (VideoPlayerPage))
                App.ApplicationFrame.Navigate(typeof (VideoPlayerPage));
            LogHelper.Log("PLAYVIDEO: Settings videoVm as Locator.VideoVm.CurrentVideo");
            Locator.VideoVm.CurrentVideo = videoVm;
            await Locator.MediaPlaybackViewModel.SetMedia(videoVm);
        }
    }
}