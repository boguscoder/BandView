namespace bandview
{
	using Android.Widget;

	using Com.Lilarcor.Cheeseknife;

	using Microsoft.Band.Portable.Sensors;

	public class RawSensorFragment <T> : SensingFragmentBase<T> where T : IBandSensorReading
	{
		[InjectView(Resource.Id.raw_data)]
		TextView _raw;
		
		protected override int LayoutId { get; } = Resource.Layout.RawSensor;

		protected override void OnSensorData(T data)
		{
			_raw.Text = data.ToString();
		}
	}
}

