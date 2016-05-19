namespace bandview
{
	using System;

	using Android.Widget;

	using Com.Lilarcor.Cheeseknife;

	using Microsoft.Band.Portable.Sensors;

	using OxyPlot;
	using OxyPlot.Axes;
	using OxyPlot.Series;
	using OxyPlot.Xamarin.Android;

	public class AccelerometerFragment: SensingFragmentBase<BandAccelerometerReading>
	{
		[InjectView(Resource.Id.plot_view)]
		PlotView _plotView; 

		PlotModel _model;
		LineSeries _seriesX, _seriesY, _seriesZ;

		private const int MAX_POINTS = 200;
		private long _counter = 0;


		protected override int LayoutId { get; } = Resource.Layout.Accelerometer;

		protected override BandSensorSampleRate SampleRate { get; } = BandSensorSampleRate.Ms128;

		public override void OnResume()
		{
			base.OnResume();
			InitPlotModel();
			_plotView.Model = _model;
		}

		protected override void OnSensorData(BandAccelerometerReading data)
		{
			_seriesX.Points.Add(new DataPoint(_counter, data.AccelerationX));
			_seriesY.Points.Add(new DataPoint(_counter, data.AccelerationY));
			_seriesZ.Points.Add(new DataPoint(_counter, data.AccelerationZ));

			if (_seriesX.Points.Count > MAX_POINTS)
			{
				_seriesX.Points.RemoveRange(0, 1);
				_seriesY.Points.RemoveRange(0, 1);
				_seriesZ.Points.RemoveRange(0, 1);
			}

			++_counter;
			_plotView.Model.InvalidatePlot(true);
		}

		private void InitPlotModel()
		{
			_model = new PlotModel()
			{
				Axes = { new LinearAxis { Position = AxisPosition.Left, Title = "Acceleration, m/s²", Minimum = -4.0, Maximum = 4.0}, 
					new LinearAxis { Position = AxisPosition.Bottom, Title = "Units" } 
				}
			};

			_seriesX = new LineSeries
			{
				MarkerType = MarkerType.Circle,
				MarkerStroke = OxyColors.Red
			};
				
			_seriesY = new LineSeries
			{
				MarkerType = MarkerType.Square,
				MarkerStroke = OxyColors.Green
			};

			_seriesZ = new LineSeries
			{
				MarkerType = MarkerType.Star,
				MarkerStroke = OxyColors.Blue
			};

			_model.Series.Add(_seriesX);
			_model.Series.Add(_seriesY);
			_model.Series.Add(_seriesZ);
		}
	}
}

