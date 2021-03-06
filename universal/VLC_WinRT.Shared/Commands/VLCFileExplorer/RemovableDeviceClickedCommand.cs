﻿using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using VLC_WinRT.Utils;
using VLC_WinRT.ViewModels;
using VLC_WinRT.ViewModels.Others.VlcExplorer;

namespace VLC_WinRT.Commands.VLCFileExplorer
{
    public class RemovableDeviceClickedCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            if ((parameter as SelectionChangedEventArgs).AddedItems.Count != 0)
            {
                FileExplorerViewModel fileExplorer = (parameter as SelectionChangedEventArgs).AddedItems[0] as FileExplorerViewModel;
                Locator.FileExplorerVM.CurrentStorageVM = fileExplorer;
                Task.Run(() => Locator.FileExplorerVM.CurrentStorageVM.GetFiles());
            }
        }
    }
}
