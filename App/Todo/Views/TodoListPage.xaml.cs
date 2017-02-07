using System;
using System.Diagnostics;
using System.IO;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Todo.Data;
using Todo.Models;
using Todo.Views;

namespace Todo
{
	public partial class TodoListPage : ContentPage
	{
		public TodoListPage()
		{
			InitializeComponent();

		    RequestCameraPermission();
		    RequestStoragePermission();

		    BtnLaunchVideo.Command = new Command(() => ShouldTakeVideo());
		}

        private async void RequestStoragePermission()
        {
            var storagePermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (storagePermission != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    Debug.WriteLine("Need to request storage permissions");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                storagePermission = results[Permission.Storage];
            }

            if (storagePermission == PermissionStatus.Granted)
            {
                Debug.WriteLine("Storage permission granted");
            }
        }

        private async void RequestCameraPermission()
	    {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            
            if (cameraStatus != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                {
                    Debug.WriteLine("Need to request Camera permissions");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                cameraStatus = results[Permission.Camera];
            }

            if (cameraStatus == PermissionStatus.Granted)
            {
                Debug.WriteLine("Camera permission granted");
            }
        }

	    public event Action ShouldTakeVideo = () =>
	    {
	        
	    };
	
	    protected override async void OnAppearing()
		{
			base.OnAppearing();

			// Reset the 'resume' id, since we just want to re-start here
			((App)App.Current).ResumeAtTodoId = -1;
			listView.ItemsSource = await App.Database.GetItemsAsync();
		}

		async void OnItemAdded(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new TodoItemPage
			{
				BindingContext = new TodoItem()
			});
		}

		async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			((App)App.Current).ResumeAtTodoId = (e.SelectedItem as TodoItem).ID;
			Debug.WriteLine("setting ResumeAtTodoId = " + (e.SelectedItem as TodoItem).ID);

			await Navigation.PushAsync(new TodoItemPage
			{
				BindingContext = e.SelectedItem as TodoItem
			});
		}


        public async void HandleCapturedVideo(string filePath)
        {
            Debug.WriteLine($"Video captured at {filePath}");

            await App.Database.SaveClip(new VideoClip {CaptureTime = DateTime.Now, Path = filePath});


            //pull it out again to check
            var clips = await App.Database.GetClipsAsync();

            foreach (var clip in clips)
            {
                Debug.WriteLine($"Clip from DB {clip} taken at {clip.CaptureTime}");
            }
        }

        public async void ProcessCapturedVideoClip(byte[] thumbnail, string videoPath)
        {
            //imgFromCamera.Source = ImageSource.FromFile(thumbnailPath);

            //read the thumbnail
            await App.Database.SaveClip(new VideoClip
            {
                CaptureTime = DateTime.Now, Path = videoPath, Thumbnail = thumbnail
            });


        }


	    private async void OnCaptureFlowStarted(object sender, EventArgs e)
	    {
	        await Navigation.PushAsync(new CaptureFlowScrollPage());

	    }
	}
}
