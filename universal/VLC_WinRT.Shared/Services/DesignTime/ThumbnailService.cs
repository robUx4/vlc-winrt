﻿/**********************************************************************
 * VLC for WinRT
 **********************************************************************
 * Copyright © 2013-2014 VideoLAN and Authors
 *
 * Licensed under GPLv2+ and MPLv2
 * Refer to COPYING file of the official project for license
 **********************************************************************/

using System;
using System.Threading.Tasks;
using VLC_WinRT.Services.Interface;
using Windows.Storage;
using Windows.Storage.FileProperties;
using libVLCX;

namespace VLC_WinRT.Services.DesignTime
{
    public class ThumbnailService : IThumbnailService
    {
        public async Task<StorageItemThumbnail> GetThumbnail(StorageFile file)
        {
            var uri = new Uri("ms-appx:///Assets/Logo.png");
            StorageFile image = await StorageFile.GetFileFromApplicationUriAsync(uri);

            StorageItemThumbnail thumb = await image.GetThumbnailAsync(ThumbnailMode.VideosView);
            return thumb;
        }

        public Task<PreparseResult> GetScreenshot(StorageFile file)
        {
            var res = new PreparseResult();
            return Task.FromResult(res);
        }
    }
}
