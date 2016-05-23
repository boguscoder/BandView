namespace bandview
{
	using System;

	using Android.Widget;
	using Android.Graphics;

	using Com.Lilarcor.Cheeseknife;

	using Microsoft.Band.Sensors;

	public class AmbientLightFragment : SensingFragmentBase<IBandAmbientLightReading>
	{
		[InjectView(Resource.Id.level)]
		TextView _level;

		[InjectView(Resource.Id.levelImg)]
		ImageView _levelImg;

		private const int MIN_TEMP = 0;
		private const int MAX_TEMP = 25000;

		private Color _imgColor;
		private Color _txtColor;

		protected override int LayoutId { get; } = Resource.Layout.AmbientLight;

		protected override void OnSensorData(IBandAmbientLightReading data)
		{
			_level.Text = $"{data.Brightness}";

			int r = 0, g = 0, b = 0;

			// conversion to 'long rainbow' kindly found in internet ocean
			double f = ((double)data.Brightness - MIN_TEMP) / (MAX_TEMP - MIN_TEMP);

			var a = (1 - f) / 0.2;
			var X = (int) Math.Floor(a);
			var Y = (int) Math.Floor(255 * (a - X));

			switch (X)
			{
				case 0: r = 255; g = Y; b = 0; break;
				case 1: r = 255 - Y; g = 255; b = 0; break;
				case 2: r = 0; g = 255; b = Y; break;
				case 3: r = 0; g = 255 - Y; b = 255; break;
				case 4: r = Y; g = 0; b = 255; break;
				case 5: r = 255; g = 0; b = 255; break;
			}

			int iR = 255 - r, iG = 255 - g, iB = 255 - b;

			_imgColor = Color.Argb(255, r, g, b);
			_txtColor = Color.Argb(255, iR, iG, iB);

			_levelImg.SetColorFilter(_imgColor);
			_level.SetTextColor(_txtColor);
		}
	}
}

