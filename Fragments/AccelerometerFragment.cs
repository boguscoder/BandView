namespace bandview
{
	using Android.Widget;
	using Com.Lilarcor.Cheeseknife;

	using Microsoft.Band.Portable.Sensors;

	public class AccelerometerFragment: SensingFragmentBase<BandAccelerometerReading>
	{
		[InjectView(Resource.Id.gl_view)]
		GlView _glView;

		[InjectView(Resource.Id.raw_data)]
		TextView _raw;

		protected override int LayoutId { get; } = Resource.Layout.Accelerometer;

		protected override void OnSensorData(BandAccelerometerReading data)
		{
			_glView.SpeedX = data.AccelerationX;
			_glView.SpeedY = data.AccelerationY;
			_glView.SpeedZ = data.AccelerationZ;

			_raw.Text = $"X:{data.AccelerationX:N2} Y:{data.AccelerationY:N2} Z:{data.AccelerationZ:N2}";
		}
	}
}

