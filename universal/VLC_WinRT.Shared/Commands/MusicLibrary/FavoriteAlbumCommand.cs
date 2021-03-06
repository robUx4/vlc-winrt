﻿/**********************************************************************
 * VLC for WinRT
 **********************************************************************
 * Copyright © 2013-2014 VideoLAN and Authors
 *
 * Licensed under GPLv2+ and MPLv2
 * Refer to COPYING file of the official project for license
 **********************************************************************/

using System.Linq;
using VLC_WinRT.Database;
using VLC_WinRT.Model.Music;
using VLC_WinRT.Utils;
using VLC_WinRT.ViewModels;

namespace VLC_WinRT.Commands.MusicLibrary
{
    public class FavoriteAlbumCommand : AlwaysExecutableCommand
    {
        public override async void Execute(object parameter)
        {
            var album = parameter as AlbumItem;
            if (album == null)
                return;
            // If the album is favorite, then now it is not
            // if the album was not favorite, now it is
            album.Favorite = !album.Favorite;
            var albumDatabase = new AlbumDatabase();
            // updating the FavoriteAlbums collection
            if (album.Favorite)
            {
                Locator.MusicLibraryVM.FavoriteAlbums.Add(album);
                // if the album is already in the list then don't add it, simply
                if (Locator.MusicLibraryVM.RandomAlbums.FirstOrDefault(x => x.Id == album.Id) == null)
                    Locator.MusicLibraryVM.RandomAlbums.Add(album);
            }
            else if (Locator.MusicLibraryVM.FavoriteAlbums.Contains(album))
            {
                Locator.MusicLibraryVM.FavoriteAlbums.Remove(album);
                Locator.MusicLibraryVM.RandomAlbums.Remove(album);
            }
            // Update database;
            await albumDatabase.Update(album);
        }
    }
}