﻿namespace bandview
{
	using System;

	using Android.App;
	using Android.OS;
	using Android.Support.V7.Widget;
	using Android.Views;
	using Android.Widget;

	using Com.Lilarcor.Cheeseknife;

	[Activity(Label = "bandview", MainLauncher = true, Icon = "@mipmap/icon")]
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

			await App.InitializeSensors(this, (progress) => RunOnUiThread(() => _status.Text = progress ) );

			_progressBar.Visibility = ViewStates.Gone;
			_sensorList.Visibility = ViewStates.Visible;
			_status.SetText(Resource.String.status_ready);

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

		public override void OnBackPressed()
		{
			if (_sensorView.Visibility == ViewStates.Visible)
			{
				_sensorView.Visibility = ViewStates.Gone;
				_selector.Visibility = ViewStates.Visible;
			}
			else base.OnBackPressed();
		} 

	}
}


