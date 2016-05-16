namespace bandview
{
	using System;

	using Android.App;
	using Android.Content.PM;
	using Android.OS;
	using Android.Support.V7.Widget;
	using Android.Views;
	using Android.Widget;

	using Com.Lilarcor.Cheeseknife;

	[Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@mipmap/band", ScreenOrientation = ScreenOrientation.Portrait)]
	public class BandViewActivity : Activity
	{
		[InjectView(Resource.Id.statusText)]
		TextView _status;

		[InjectView(Resource.Id.progressBar)]
		ProgressBar _progressBar;

		[InjectView(Resource.Id.selector)]
		View _selector;

		[InjectView(Resource.Id.sensorList)]
		RecyclerView _sensorList;

		[InjectView(Resource.Id.sensorContainer)]
		View _sensorView;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);
			Cheeseknife.Inject(this);

			bool success = await App.InitializeSensors(this, (progress) => RunOnUiThread(() => _status.Text = progress));

			_progressBar.Visibility = ViewStates.Gone;
			_sensorList.Visibility = ViewStates.Visible;

			if (!success)
				return;

			var adapter = new SensorsAdapter(this);
			adapter.ItemClick += (sender, pos) => ShowSensorPage((SensorType)pos);

	        _sensorList.SetAdapter(adapter);
			_sensorList.SetLayoutManager(new LinearLayoutManager(this));
		}

		private void ShowSensorPage(SensorType type)
		{
			try
			{
				var fragment = FragmentFactory.GetSensingFragment(type);
				var fragmentTransaction = FragmentManager.BeginTransaction();
				fragmentTransaction.Replace(Resource.Id.sensorContainer, fragment);
				fragmentTransaction.Commit();

				_sensorView.Visibility = ViewStates.Visible;
				_selector.Visibility = ViewStates.Gone;
			}
			catch (NotImplementedException)
			{
				Toast.MakeText(this, GetString(Resource.String.status_noimpl), ToastLength.Short).Show();
			}
		}

		private bool HideSensorPage()
		{
			var frag = FragmentManager.FindFragmentById(Resource.Id.sensorContainer);

			if (frag != null)
			{
				var fragmentTransaction = FragmentManager.BeginTransaction();
				fragmentTransaction.Remove(frag);
				fragmentTransaction.Commit();
			}

			return frag != null;
		}

		public override void OnBackPressed()
		{
			// though its not optimal to kill fragments like this on every backpress
			// its more convinient to manage sensors if fragments are destroyed asap
			if (HideSensorPage())
			{
				_sensorView.Visibility = ViewStates.Gone;
				_selector.Visibility = ViewStates.Visible;
			}
			else base.OnBackPressed();
		} 

	}
}


