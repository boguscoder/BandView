namespace bandview
{
	using Android.Widget;
	using Android.Graphics;

	using Com.Lilarcor.Cheeseknife;

	using Microsoft.Band.Portable.Sensors;

	public class AmbientLightFragment : SensingFragmentBase<BandAmbientLightReading>
	{
		[InjectView(Resource.Id.level)]
		TextView _level;

		[InjectView(Resource.Id.levelImg)]
		ImageView _levelImg;

		private const int _pivotPoint = 1000; 

		private Color _imgColor;
		private Color _txtColor;

		protected override int LayoutId { get; } = Resource.Layout.AmbientLight;

		protected override void OnSensorData(BandAmbientLightReading data)
		{
			_level.Text = $"{data.Brightness}";

			int r = data.Brightness < _pivotPoint ? (byte)((double)data.Brightness / _pivotPoint * 255) : 255;
			int g = r;
			int b = 0;

			int iR = 255 - r;
			int iG = 255 - g;
			int iB = 255 - b;

			_imgColor = Color.Argb(255, r, g, b);
			_txtColor = Color.Argb(255, iR, iG, iB);

			_levelImg.SetColorFilter(_imgColor);
			_level.SetTextColor(_txtColor);
		}
	}
}

