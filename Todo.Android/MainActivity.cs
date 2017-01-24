using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using Android.Provider;
using Todo;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Graphics;
using Android.Media;
using File = Java.IO.File;
using Android.Runtime;
using Plugin.Permissions;

namespace Todo
{

    [Activity(Label = "Todo", Icon = "@drawable/icon", MainLauncher = true,
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity :
	global::Xamarin.Forms.Platform.Android.FormsApplicationActivity // superclass new in 1.3
	{
        MediaRecorder _recorder;

        private readonly List<string> _videoFileList;
        private readonly List<string> _fileGuids;

        private string _capturedVideoPath = string.Empty;


        public MainActivity()
	    {
            _videoFileList = new List<string>();
            _fileGuids = new List<string>();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle bundle)
	    {
	        base.OnCreate(bundle);

	        global::Xamarin.Forms.Forms.Init(this, bundle);

	        var app = new App();

	        LoadApplication(app);

            
            app.ListPage.ShouldTakeVideo += () =>
            {
                var vidIntent = new Intent(MediaStore.ActionVideoCapture);

                var g1 = Guid.NewGuid();

                var vidFileName = $"{g1}.mp4";

                _fileGuids.Add(g1.ToString());

                //TODO move this to SD card
                var videoFile = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMovies), vidFileName);

                vidIntent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile((videoFile)));

                _capturedVideoPath = videoFile.Path;

                _videoFileList.Add(vidFileName);

                StartActivityForResult(vidIntent, 0);
            };

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            //(Xamarin.Forms.Application.Current as App).MyMainPage.ShowImage(file.Path);

            //can we read the video and make a thumbnail?

            var app = Xamarin.Forms.Application.Current as App;

            if (_capturedVideoPath != string.Empty)
            {
                app?.ListPage.HandleCapturedVideo(_capturedVideoPath);

                Bitmap thumbBitmap = ThumbnailUtils.CreateVideoThumbnail(_capturedVideoPath, ThumbnailKind.MiniKind);

                var tmpPath = System.IO.Path.GetTempPath();
                //try saving the file
                //var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                var fileName = $"{_fileGuids.Last()}.png";
                var filePath = System.IO.Path.Combine(tmpPath, fileName);
                var fs = new FileStream(filePath, FileMode.Create);
                thumbBitmap.Compress(Bitmap.CompressFormat.Png, 100, fs);
                fs.Close();

                app?.ListPage.DisplayVideoThumb(filePath);


                //var bos = new MemoryStream();
                //thumbBitmap.Compress(Bitmap.CompressFormat.Png, 0, bos);
                //var bitmapData = bos.ToArray();
                //app?.MyMainPage.HandleCapturedThumbnail(bitmapData);
                //app?.MyMainPage.HandleCapturedThumbnail(Stream stream);

            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}
