namespace bandview
{
	using System;

	using Android.App;

	using Microsoft.Band.Sensors;

	public static class FragmentFactory
	{
		public static Fragment GetSensingFragment(SensorType type)
		{
			switch (type)
			{
				case SensorType.Accelerometer:		return new AccelerometerFragment();
				case SensorType.HeartRate: 			return new HeartRateFragment();
				case SensorType.Pedometer:			return new RawSensorFragment<IBandPedometerReading>();
				case SensorType.RRInterval:			return new RawSensorFragment<IBandRRIntervalReading>();
				case SensorType.SkinTemperature:	return new RawSensorFragment<IBandSkinTemperatureReading>();
				case SensorType.UltravioletLight:	return new RawSensorFragment<IBandUVReading>();
				case SensorType.Gsr:				return new RawSensorFragment<IBandGsrReading>();
				case SensorType.Altimeter:			return new RawSensorFragment<IBandAltimeterReading>();
				case SensorType.AmbientLight:		return new AmbientLightFragment();
				case SensorType.Barometer:			return new RawSensorFragment<IBandBarometerReading>();
				case SensorType.Calories:			return new RawSensorFragment<IBandCaloriesReading>();
				case SensorType.Gyroscope:			return new RawSensorFragment<IBandGyroscopeReading>();
				case SensorType.Distance:			return new RawSensorFragment<IBandDistanceReading>();

				default:
					throw new NotImplementedException();
			}
		}
	}
}

