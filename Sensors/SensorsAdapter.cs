namespace bandview
{
	using System;

	using Android.Content;
	using Android.Content.Res;
	using Android.Support.V7.Widget;
	using Android.Views;

	public class SensorsAdapter : RecyclerView.Adapter
	{
		string[] _names;
		string[] _descs;
		TypedArray _icons;

		public override int ItemCount => _names.Length;

		public event EventHandler<int> ItemClick;

		public SensorsAdapter(Context ctx)
		{
			_names = ctx.Resources.GetStringArray(Resource.Array.sensor_names);
			_descs = ctx.Resources.GetStringArray(Resource.Array.sensor_descriptions);
			_icons = ctx.Resources.ObtainTypedArray(Resource.Array.sensor_icons);
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var sensor = holder as SensorViewHolder;

			sensor.Name.Text = _names[position];
			sensor.Desc.Text = _descs[position];
			sensor.Icon.SetImageDrawable(_icons.GetDrawable(position));
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SensorView, parent, false);
			return new SensorViewHolder(itemView, (position) => ItemClick?.Invoke(this, position));
		}
	}
}

