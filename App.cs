namespace bandview
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	using Android.Content;

	using FakeBand.Fakes;

	using Microsoft.Band;
	using Microsoft.Band.Sensors;

	using Microsoft.Practices.Unity;

	public static class App
	{
		private const int ConnectTimeout = 10000;

		public static UnityContainer Container { get; } = new UnityContainer();

		// no thread-safety guarantees here, call only from UI thread
		public static async Task<bool> ConnectToBand(Context ctx, Action<string> progressReporter = null)
		{

			FakeBandClientManager.Configure(new FakeBandClientManagerOptions
			{
				Bands = new List<IBandInfo>
				  {
					  new FakeBandInfo(BandConnectionType.Bluetooth, "Fake Band"),
				  }
			});

			var bandClientManager = FakeBand.Fakes.FakeBandClientManager.Instance;
			var pairedBands = await bandClientManager.GetBandsAsync();
			var bandInfo = pairedBands.FirstOrDefault();

			if (bandInfo == null)
			{
				progressReporter?.Invoke(ctx.GetString((Resource.String.status_notfound)));
				return false;
			}

			progressReporter?.Invoke($"{ctx.GetString(Resource.String.status_connecting)} {bandInfo.Name}");

			var connectTask = bandClientManager.ConnectAsync(bandInfo);
		
			if (await Task.WhenAny(connectTask, Task.Delay(ConnectTimeout)) == connectTask)
			{
				var bandClient = await connectTask;

				if (bandClient == null)
				{
					progressReporter?.Invoke($"{ctx.GetString(Resource.String.status_failed)}");
					return false;
				}

				progressReporter?.Invoke($"{ctx.GetString(Resource.String.status_connected)} {bandInfo.Name}");

				BindSensors(bandClient);

				return true;
			}
			else
			{
				progressReporter?.Invoke($"{ctx.GetString(Resource.String.status_failed)}");
				return false;
			}
		}

		private static void BindSensors(IBandClient bandClient)
		{
			Container.RegisterInstance(typeof(IBandSensor<IBandAccelerometerReading>), string.Empty, bandClient.SensorManager.Accelerometer, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandHeartRateReading>), string.Empty, bandClient.SensorManager.HeartRate, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandPedometerReading>), string.Empty, bandClient.SensorManager.Pedometer, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandRRIntervalReading>), string.Empty, bandClient.SensorManager.RRInterval, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandSkinTemperatureReading>), string.Empty, bandClient.SensorManager.SkinTemperature, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandUVReading>), string.Empty, bandClient.SensorManager.UV, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandGsrReading>), string.Empty, bandClient.SensorManager.Gsr, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandAltimeterReading>), string.Empty, bandClient.SensorManager.Altimeter, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandAmbientLightReading>), string.Empty, bandClient.SensorManager.AmbientLight, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandBarometerReading>), string.Empty, bandClient.SensorManager.Barometer, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandCaloriesReading>), string.Empty, bandClient.SensorManager.Calories, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandGyroscopeReading>), string.Empty, bandClient.SensorManager.Gyroscope, new ContainerControlledLifetimeManager());
			Container.RegisterInstance(typeof(IBandSensor<IBandDistanceReading>), string.Empty, bandClient.SensorManager.Distance, new ContainerControlledLifetimeManager());
		}
	}
}

