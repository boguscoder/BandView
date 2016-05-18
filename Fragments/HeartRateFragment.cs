namespace bandview
{
	using System;

	using Android.Widget;
	using Android.Graphics;

	using Com.Lilarcor.Cheeseknife;

	using Microsoft.Band.Portable.Sensors;
	using Android.Views.Animations;
	using Android.Animation;

	public class HeartRateFragment : SensingFragmentBase<BandHeartRateReading>
	{
		[InjectView(Resource.Id.command)]
		TextView _command;

		[InjectView(Resource.Id.heart)]
		TextView _heartSym;

		[InjectView(Resource.Id.beats)]
		TextView _beats;

		private const int DEFAULT_DURATION = 800;
		private const int BASELINE_BEAT = 60;
		private readonly Color LOCKED_COLOR = Color.Red;
		private readonly Color UNLOCKED_COLOR = Color.SlateGray;

		private Animation _animation;
		private double _durationScaler = 1.0f;

		protected override int LayoutId { get; } = Resource.Layout.HeartRate;

		public override void OnResume()
		{
			base.OnResume();

			_heartSym.Text = "♥";
			_heartSym.SetTextColor(UNLOCKED_COLOR);

			_animation = AnimationUtils.LoadAnimation(Activity, Resource.Animation.heartbeat_anim);
			_animation.Duration = DEFAULT_DURATION;
			_heartSym.StartAnimation(_animation);

			_animation.AnimationEnd += (sender, e) =>
			{
				_animation.Duration = (long) (_durationScaler * DEFAULT_DURATION);
				_heartSym.StartAnimation(_animation);
			};
		}

		protected override void OnSensorData(BandHeartRateReading data)
		{
			_beats.Text = data.HeartRate.ToString();
			_heartSym.SetTextColor(data.Quality == HeartRateQuality.Locked ? LOCKED_COLOR : UNLOCKED_COLOR);
			_command.SetText(data.Quality == HeartRateQuality.Locked ? Resource.String.command_heartrate_locked : Resource.String.command_heartrate_acquiring);
			_durationScaler = (double) BASELINE_BEAT / data.HeartRate;
		}
	}
}

