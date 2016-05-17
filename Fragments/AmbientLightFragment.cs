namespace bandview
{
	using System;

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

		private const int COLD_POINT = 1000;
		private const int HOT_POINT  = 10000;

		private Color _imgColor;
		private Color _txtColor;

		protected override int LayoutId { get; } = Resource.Layout.AmbientLight;

		protected override void OnSensorData(BandAmbientLightReading data)
		{
			_level.Text = $"{data.Brightness}";

			// TODO: sensor range is smth like 0 - 25000, would be nice to map it nicely to color temperature
			int r = data.Brightness < COLD_POINT ? Math.Min((byte)((double)data.Brightness / COLD_POINT * 255), (byte) 255) : 255;
			int g = r;
			int b = data.Brightness < COLD_POINT ? 0 : Math.Min((byte)((double)data.Brightness / HOT_POINT * 255), (byte) 255);

			int iR = 255 - r, iG = 255 - g, iB = 255 - b;

			_imgColor = Color.Argb(255, r, g, b);
			_txtColor = Color.Argb(255, iR, iG, iB);

			_levelImg.SetColorFilter(_imgColor);
			_level.SetTextColor(_txtColor);
		}
	}
}

