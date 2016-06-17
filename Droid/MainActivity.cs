using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Graphics;
using System.Linq;

//sid says cowbey
using Java.IO;
using System.Collections.Generic;
using System.IO;

namespace Page2.Droid
{
	[Activity (Label = "Page2", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		
		//public string icon=Android.OS.Environment.ExternalStorageDirectory+"/Download/Template";
		public string IconPath=Android.OS.Environment.ExternalStorageDirectory+"/Download/Template";
		List<FileSystemInfo> visibleThings = new List<FileSystemInfo>();
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			GridView gridview = FindViewById<GridView> (Resource.Id.gridview);

			//檢查範本資料夾是否建立
			this.DirCheck(new Java.IO.File (IconPath));

			//呼叫找資料的方法,回傳找到範本資料夾底下的檔案路徑
			var ReturnIcons=this.FindTemplateIcon (IconPath,visibleThings);
			gridview.Adapter = new ImageAdapter (this,IconPath,ReturnIcons);


			gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args) {
				Toast.MakeText (this, args.Position.ToString (), ToastLength.Short).Show ();
			};
		}
		private  List<FileSystemInfo> FindTemplateIcon (string icoopath,List<FileSystemInfo> visibleThings)
		{
			DirectoryInfo DirInfo = new DirectoryInfo (icoopath);
			foreach (var AllFile in DirInfo.GetFileSystemInfos ().Where (item => item.Exists)) {
				bool IsPngFile = AllFile.Extension.ToLower ().EndsWith (".png");
				if (IsPngFile) {
					visibleThings.Add (AllFile);
				}
			}
			return visibleThings;
		}
		private void DirCheck(Java.IO.File dir)
		{
			if(!dir.Exists())
			{
				dir.Mkdirs ();
				return;
			}
		}
	}

	public class ImageAdapter : BaseAdapter
	{
		List<FileSystemInfo> _visibleThings;


//		int[] thumbIds = {
//			Resource.Drawable.sample_0, Resource.Drawable.sample_1,
//			Resource.Drawable.sample_2, Resource.Drawable.sample_3,
//			Resource.Drawable.sample_4, Resource.Drawable.sample_5,
//			Resource.Drawable.sample_6, Resource.Drawable.sample_7
//		};
		private Activity _context;

		public ImageAdapter (Activity context,string icoopath,List<FileSystemInfo> visibleThings)
		{
			_context = context;
			_visibleThings = visibleThings;
			var HowManyFile=visibleThings.Count;

		}

		public override int Count {
			get { return _visibleThings.Count; }
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
				imageView = new ImageView (_context);
				imageView.LayoutParameters = new GridView.LayoutParams (125, 125);
				imageView.SetScaleType (ImageView.ScaleType.CenterCrop);
				imageView.SetPadding (8, 8, 8, 8);
			} else {
				imageView = (ImageView)convertView;
			}
			var filepath = _visibleThings [position];
			Bitmap bmp = BitmapFactory.DecodeFile(filepath.FullName);

			//imageView.SetImageResource (thumbIds[position]);
			imageView.SetImageBitmap(bmp);
			return imageView;
		}

		// references to our images

	}
}


