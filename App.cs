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

			Container.RegisterInstance(typeof(BandSensorBase<BandAmbientLightReading>), string.Empty, bandClient.SensorManager.AmbientLight, new ContainerControlledLifetimeManager());

			await Task.Delay(StepDelay); // ditto

			return true;
		}
	}
}

