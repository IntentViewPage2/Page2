using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Graphics;


//sid says cowbey

namespace Page2.Droid
{
	[Activity (Label = "Page2", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;
		string WTfF=null;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			var gridview = FindViewById<GridView> (Resource.Id.gridview);
			gridview.Adapter = new ImageAdapter (this);


			gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args) {
				Toast.MakeText (this, args.Position.ToString (), ToastLength.Short).Show ();
			};
		}
	}

	public class ImageAdapter : BaseAdapter
	{
		Context context;

		public ImageAdapter (Context c)
		{
			context = c;
		}

		public override int Count {
			get { return thumbIds.Length; }
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		// create a new ImageView for each item referenced by the Adapter
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ImageView imageView;

			if (convertView == null) {  // if it's not recycled, initialize some attributes
				imageView = new ImageView (context);
				imageView.LayoutParameters = new GridView.LayoutParams (125, 125);
				imageView.SetScaleType (ImageView.ScaleType.CenterCrop);
				imageView.SetPadding (8, 8, 8, 8);
			} else {
				imageView = (ImageView)convertView;
			}

			imageView.SetImageResource (thumbIds[position]);
			return imageView;
		}

		// references to our images
		int[] thumbIds = {
			Resource.Drawable.sample_0 ,Resource.Drawable.sample_0,
			Resource.Drawable.sample_4, Resource.Drawable.sample_5,
			Resource.Drawable.sample_6, Resource.Drawable.sample_7,
			Resource.Drawable.sample_0, Resource.Drawable.sample_1,
			Resource.Drawable.sample_2, Resource.Drawable.sample_3,
			Resource.Drawable.sample_4, Resource.Drawable.sample_5,
			Resource.Drawable.sample_6, Resource.Drawable.sample_7,
			Resource.Drawable.sample_0, Resource.Drawable.sample_1,
			Resource.Drawable.sample_2, Resource.Drawable.sample_3,
			Resource.Drawable.sample_4, Resource.Drawable.sample_5,
			Resource.Drawable.sample_6, Resource.Drawable.sample_7
		};
	}
}


