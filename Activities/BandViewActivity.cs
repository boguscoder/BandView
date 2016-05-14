namespace bandview
{
	using Android.App;
	using Android.OS;
	using Android.Views;
	using Android.Widget;

	using Com.Lilarcor.Cheeseknife;

	[Activity(Label = "bandview", MainLauncher = true, Icon = "@mipmap/icon")]
	public class BandViewActivity : Activity
	{
		[InjectView(Resource.Id.statusText)]
		TextView _status;

		[InjectView(Resource.Id.progress)]
		View _progressContainer;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);
			Cheeseknife.Inject(this);

			await App.InitializeSensors(this, (progress) => RunOnUiThread(() => _status.Text = progress ) );

			_progressContainer.Visibility = Android.Views.ViewStates.Gone;

			var fragment = FragmentFactory.GetSensingFragment(SensorType.AmbientLight);
			var fragmentTransaction = FragmentManager.BeginTransaction();
			fragmentTransaction.Replace(Resource.Id.sensorContainer, fragment);
			fragmentTransaction.Commit();
		}
	}
}


