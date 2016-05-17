namespace bandview
{
	using System;

	using Android.App;

	using Microsoft.Band.Portable.Sensors;

	public static class FragmentFactory
	{
		public static Fragment GetSensingFragment(SensorType type)
		{
			switch (type)
			{
				case SensorType.Accelerometer:		return new RawSensorFragment<BandAccelerometerReading>();
				case SensorType.HeartRate: 			return new HeartRateFragment();
				case SensorType.Pedometer:			return new RawSensorFragment<BandPedometerReading>();
				case SensorType.RRInterval:			return new RawSensorFragment<BandRRIntervalReading>();
				case SensorType.SkinTemperature:	return new RawSensorFragment<BandSkinTemperatureReading>();
				case SensorType.UltravioletLight:	return new RawSensorFragment<BandUltravioletLightReading>();
				case SensorType.Gsr:				return new RawSensorFragment<BandGsrReading>();
				case SensorType.Altimeter:			return new RawSensorFragment<BandAltimeterReading>();
				case SensorType.AmbientLight:		return new AmbientLightFragment();
				case SensorType.Barometer:			return new RawSensorFragment<BandBarometerReading>();
				case SensorType.Calories:			return new RawSensorFragment<BandCaloriesReading>();
				case SensorType.Gyroscope:			return new RawSensorFragment<BandGyroscopeReading>();
				case SensorType.Distance:			return new RawSensorFragment<BandDistanceReading>();

				default:
					throw new NotImplementedException();
			}
		}
	}
}

