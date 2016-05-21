namespace bandview
{
	using System.Threading.Tasks;

	using Android.App;

	using Com.Lilarcor.Cheeseknife;

	using Microsoft.Band.Sensors;

	public abstract class SensingFragmentBase <T> : Fragment where T : IBandSensorReading
	{
		protected IBandSensor<T> Sensor { get; } = App.Container.Resolve(typeof(IBandSensor<T>), null) as IBandSensor<T>;

		protected abstract int LayoutId { get; }

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			var view = inflater.Inflate(LayoutId, container, false);
			Cheeseknife.Inject(this, view);
			return view;
		}

		public override async void OnAttach(Android.Content.Context context)
		{
			base.OnAttach(context);

			var consent = Sensor.GetCurrentUserConsent();
			bool allowed = consent == Microsoft.Band.UserConsent.Granted ? true : await Sensor.RequestUserConsentAsync();

			if (allowed)
			{
				if (Sensor != null)
				{
					Sensor.ReadingChanged += (sender, e) => Activity?.RunOnUiThread(
																() => OnSensorData(e.SensorReading));

					await Sensor.StartReadingsAsync();
				}
			}
		}

		public override async void OnDetach()
		{
			base.OnDetach();
			await (Sensor?.StopReadingsAsync() ?? Task.CompletedTask);
		}

		protected abstract void OnSensorData(T data);
	}
}

