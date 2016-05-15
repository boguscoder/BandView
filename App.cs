namespace bandview
{

	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Android.Content;

	using Microsoft.Band.Portable;
	using Microsoft.Band.Portable.Sensors;
	using Microsoft.Practices.Unity;

	public static class App
	{
		private const int StepDelay = 700; 

		public static UnityContainer Container { get; } = new UnityContainer();

		public static async Task<bool> InitializeSensors(Context ctx, Action<string> progressReporter = null)
		{
			var bandClientManager = BandClientManager.Instance;
			var pairedBands = await bandClientManager.GetPairedBandsAsync();
			var bandInfo = pairedBands.FirstOrDefault();

			if (bandInfo == null)
			{
				progressReporter?.Invoke(ctx.GetString((Resource.String.status_notfound)));
				return false;
			}

			await Task.Delay(StepDelay); // purely for display purposes 
			progressReporter?.Invoke($"{ctx.GetString(Resource.String.status_connecting)} {bandInfo.Name}");

			var bandClient = await bandClientManager.ConnectAsync(bandInfo);

			await Task.Delay(StepDelay); // ditto

			if (bandClient == null)
			{
				progressReporter?.Invoke($"{ctx.GetString(Resource.String.status_failed)}");
				return false;
			}

			progressReporter?.Invoke($"{ctx.GetString(Resource.String.status_connected)} {bandInfo.Name}");

			BindSensors(bandClient);
			await Task.Delay(StepDelay); // ditto

			return true;
		}

		private static void BindSensors(BandClient bandClient)
		{
			Container.RegisterInstance(typeof(BandSensorBase<BandAccelerometerReading>), string.Empty, bandClient.SensorManager.Accelerometer, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandHeartRateReading>), string.Empty, bandClient.SensorManager.HeartRate, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandPedometerReading>), string.Empty, bandClient.SensorManager.Pedometer, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandRRIntervalReading>), string.Empty, bandClient.SensorManager.RRInterval, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandSkinTemperatureReading>), string.Empty, bandClient.SensorManager.SkinTemperature, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandUltravioletLightReading>), string.Empty, bandClient.SensorManager.UltravioletLight, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandGsrReading>), string.Empty, bandClient.SensorManager.Gsr, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandAltimeterReading>), string.Empty, bandClient.SensorManager.Altimeter, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandAmbientLightReading>), string.Empty, bandClient.SensorManager.AmbientLight, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandBarometerReading>), string.Empty, bandClient.SensorManager.Barometer, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandCaloriesReading>), string.Empty, bandClient.SensorManager.Calories, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandGyroscopeReading>), string.Empty, bandClient.SensorManager.Gyroscope, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(BandSensorBase<BandDistanceReading>), string.Empty, bandClient.SensorManager.Distance, new ContainerControlledLifetimeManager());
		}
	}
}

