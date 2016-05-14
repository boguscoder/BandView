namespace bandview
{
	using System;

	using Android.App;

	public static class FragmentFactory
	{
		public static Fragment GetSensingFragment(SensorType type)
		{
			switch (type)
			{
				case SensorType.AmbientLight: return new AmbientLightFragment();
				
				default:
					throw new NotImplementedException();
			}
		}
	}
}

