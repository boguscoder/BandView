namespace bandview
{
	using System;

	using Android.Support.V7.Widget;
	using Android.Views;
	using Android.Widget;

	using Com.Lilarcor.Cheeseknife;

	public class SensorViewHolder : RecyclerView.ViewHolder
	{
		[InjectView(Resource.Id.sensorIcon)]
		public ImageView Icon { get; private set; }

		[InjectView(Resource.Id.sensorName)]
		public TextView Name { get; private set; }

		[InjectView(Resource.Id.sensorDesc)]
		public TextView Desc { get; private set; }

		public SensorViewHolder(View itemView, Action<int> listener) : base(itemView)
		{
			Cheeseknife.Inject(this, itemView);
			itemView.Click += (sender, e) => listener(base.AdapterPosition);
		}
	}
}

