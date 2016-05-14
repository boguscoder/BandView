namespace bandview
{
	using System.Threading.Tasks;

	using Android.App;

	using Com.Lilarcor.Cheeseknife;

	using Microsoft.Band.Portable.Sensors;

	public abstract class SensingFragmentBase <T> : Fragment where T : IBandSensorReading
	{
		protected BandSensorBase<T> Sensor { get; } = App.Container.Resolve(typeof(BandSensorBase<T>), null) as BandSensorBase<T>;

		protected abstract int LayoutId { get; }

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			var view = inflater.Inflate(LayoutId, container, false);
			Cheeseknife.Inject(this, view);
			return view;
		}

		public override async void OnResume()
		{
			base.OnResume();

			if (Sensor != null) // No Null-Conditional Operator here?
				Sensor.ReadingChanged += (sender, e) => Activity.RunOnUiThread(
															() => OnSensorData(e.SensorReading));

			await (Sensor?.StartReadingsAsync() ?? Task.CompletedTask);
		}

		public override async void OnPause()
		{
			base.OnPause();
			await (Sensor?.StopReadingsAsync() ?? Task.CompletedTask);
		}

		protected abstract void OnSensorData(T data);
	}
}

