using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Graphics.Imaging;

namespace MultipleChoice.Helpers
{
    public class TileHelper
    {
        public enum TileSize { Square, Wide }

        public static async Task<string> ResizeForTile(string name, TileSize size)
        {
            var _SubName = "Media";
            var _Local = ApplicationData.Current.LocalFolder;
            var _LocalSub = await _Local.CreateFolderAsync(_SubName, CreationCollisionOption.OpenIfExists);

            var _Installed = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var _InstalledSub = await _Installed.GetFolderAsync(_SubName);
            var _Source = await _InstalledSub.GetFileAsync(name);

            var _FileName = System.IO.Path.GetFileNameWithoutExtension(_Source.Name);
            var _Extension = System.IO.Path.GetExtension(_Source.Name).TrimStart('.');

            var _STDFile = await _LocalSub.CreateFileAsync(_Source.Name, CreationCollisionOption.OpenIfExists);
            if (size == TileSize.Square)
                await Resize(_Source, _STDFile, (uint)150, (uint)150);
            else
                await Resize(_Source, _STDFile, (uint)310, (uint)150);

            return "ms-appdata:///local/Media/" + name;
        }

        private static async Task Resize(StorageFile source, StorageFile destination, uint height, uint width)
        {
            using (var _Source = await source.OpenAsync(FileAccessMode.Read))
            {
                var _BitmapDecoder = await BitmapDecoder.CreateAsync(_Source);

                var _BitmapTransform = new BitmapTransform()
                {
                    ScaledHeight = height,
                    ScaledWidth = width
                };

                var _PixelData = await _BitmapDecoder.GetPixelDataAsync(
                    BitmapPixelFormat.Rgba8,
                    BitmapAlphaMode.Straight,
                    _BitmapTransform,
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.DoNotColorManage);

                using (var _Target = await destination.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var _BitmapEncoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, _Target);
                    _BitmapEncoder.SetPixelData(BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied,
                        height, width, 96, 96, _PixelData.DetachPixelData());
                    await _BitmapEncoder.FlushAsync();
                }
            }
        }
    }
}
