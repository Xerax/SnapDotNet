﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using SnapDotNet.Apps.Attributes;
using SnapDotNet.Apps.ViewModels;

namespace SnapDotNet.Apps.Pages.SignedIn
{
	[RequiresAuthentication]
	public sealed partial class MainPage
	{
		public MainViewModel ViewModel { get; private set; }

		private MediaCapture _mediaCapture;
		private MediaCaptureInitializationSettings _mediaCaptureSettings;
		private DeviceInformationCollection _cameraInfoCollection;
		private int _currentSelectedCameraDevice;
		private DeviceInformationCollection _microphoneInfoCollection;
		private int _currentSelectedAudioDevice;

		public MainPage()
		{
			InitializeComponent();

			DataContext = ViewModel = new MainViewModel();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			CameraInitialStartupSequencer();

			StatusBar.GetForCurrentView().BackgroundColor = new Color { A = 0x00, R = 0x00, G = 0x00, B = 0x00, };
			ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
		}

		protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			if (_mediaCapture != null)
				_mediaCapture.StopPreviewAsync();
		}

		private async void CameraInitialStartupSequencer()
		{
			await GetCameraDevices();

			_mediaCaptureSettings = new MediaCaptureInitializationSettings();

			//generate default settings
			_currentSelectedAudioDevice = 0;
			_currentSelectedCameraDevice = 0;
			var cameraInfo = _cameraInfoCollection[_currentSelectedCameraDevice]; //default to first device
			var microphoneInfo = _microphoneInfoCollection[_currentSelectedAudioDevice]; //default to first device

			_mediaCaptureSettings.VideoDeviceId = cameraInfo.Id;
			_mediaCaptureSettings.AudioDeviceId = microphoneInfo.Id;
			_mediaCaptureSettings.PhotoCaptureSource = PhotoCaptureSource.VideoPreview;
			_mediaCaptureSettings.StreamingCaptureMode = StreamingCaptureMode.Video;

			SetUiCameraXamlElements();
			await InitialiseCameraDevice();
			ButtonCamera.IsEnabled = true;
			ButtonRecord.IsEnabled = true; //not implemented
		}

		private async Task GetCameraDevices()
		{
			_cameraInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
			_microphoneInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture);
		}
		private async Task InitialiseCameraDevice() //must manually.stopPreviewAsync Before re-initialising.
		{
			ButtonCamera.IsEnabled = false;
			ButtonRecord.IsEnabled = false;

			_mediaCapture = new MediaCapture();

			Debug.WriteLine("Initialising Camera");
			Debug.WriteLine("Using VDevice " + _currentSelectedCameraDevice + ", ID: " + _mediaCaptureSettings.VideoDeviceId);
			await _mediaCapture.InitializeAsync(_mediaCaptureSettings);
			Debug.WriteLine("Initialising Camera: OK");

			Debug.WriteLine("Starting Camera Preview");
			CapturePreview.Source = _mediaCapture;
			await _mediaCapture.StartPreviewAsync();
			Debug.WriteLine("Starting Camera Preview: OK");

			ButtonCamera.IsEnabled = true;
			ButtonRecord.IsEnabled = true;
		}
		private void SetUiCameraXamlElements()
		{
			if (_cameraInfoCollection.Count < 2)
			{
				ButtonFrontFacing.IsEnabled = false;
			}
		}

		private async void CapturePhoto() //Also trigger off shutter key? IDK how in 8.1, no more CameraButtons.ShutterKeyPressed etc.
		{
			var stream = new InMemoryRandomAccessStream();
			var imageProperties = ImageEncodingProperties.CreateJpeg();

			Debug.WriteLine("Capping Photo");
			await _mediaCapture.CapturePhotoToStreamAsync(imageProperties, stream);
			if (stream.Size > 0) { Debug.WriteLine("Capping Photo: OK, stream size " + stream.Size); }
		}

		private void ButtonPhoto_OnClick(object sender, RoutedEventArgs e)
		{
			CapturePhoto();
		}

		private void ButtonRecord_OnClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}


		private void ButtonMessages_OnClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private async void ButtonFrontFacing_CheckChanged(object sender, RoutedEventArgs e)
		{
			_currentSelectedCameraDevice = _currentSelectedCameraDevice == 0 ? 1 : 0;
			_mediaCaptureSettings.VideoDeviceId = _cameraInfoCollection[_currentSelectedCameraDevice].Id;

			await _mediaCapture.StopPreviewAsync();
			await InitialiseCameraDevice();

			var toggleButton = sender as ToggleButton;
			if (toggleButton == null) return;
			if (toggleButton.IsChecked == null) return;

			var imagePath = (bool)toggleButton.IsChecked
				? new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.off.png")
				: new Uri("ms-appx:///Assets/Icons/appbar.camera.flip.png");
			FrontFacingImage.Source = new BitmapImage(imagePath);
		}

		private void FlashButton_CheckChanged(object sender, RoutedEventArgs e)
		{
			var toggleButton = sender as ToggleButton;
			if (toggleButton == null) return;
			if (toggleButton.IsChecked == null) return;

			_mediaCapture.VideoDeviceController.FlashControl.Enabled = (bool) !toggleButton.IsChecked;

			var imagePath = (bool) toggleButton.IsChecked
				? new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.off.png")
				: new Uri("ms-appx:///Assets/Icons/appbar.camera.flash.png");
			FlashImage.Source = new BitmapImage(imagePath);

			Debug.WriteLine("FlashControl.Enabled set to: " + _mediaCapture.VideoDeviceController.FlashControl.Enabled);
		}

		private void ButtonFriends_OnClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
