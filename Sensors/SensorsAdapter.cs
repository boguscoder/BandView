namespace bandview
{
	using System;

	using Android.Content;
	using Android.Support.V7.Widget;
	using Android.Views;

	public class SensorsAdapter : RecyclerView.Adapter
	{
		string[] _names;
		string[] _descs;

		public override int ItemCount => _names.Length;

		public event EventHandler<int> ItemClick;

		public SensorsAdapter(Context ctx)
		{
			_names = ctx.Resources.GetStringArray(Resource.Array.sensor_names);
			_descs = ctx.Resources.GetStringArray(Resource.Array.sensor_descriptions);
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var sensor = holder as SensorViewHolder;

			sensor.Name.Text = _names[position];
			sensor.Desc.Text = _descs[position];
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SensorView, parent, false);
			return new SensorViewHolder(itemView, (position) => ItemClick?.Invoke(this, position));
		}
	}
}

