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
using System;
using Android.Database;

namespace Page2.Droid
{
	[Activity (Label = "Page2", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		private const int ImagePick=1000;
		private const int PdfPick=2000;
		Android.Net.Uri uri = Android.Provider.MediaStore.Images.Media.ExternalContentUri;
		//public string icon=Android.OS.Environment.ExternalStorageDirectory+"/Download/Template";
		public string IconPath=Android.OS.Environment.ExternalStorageDirectory+"/Download/Image";
		List<FileSystemInfo> visibleThings = new List<FileSystemInfo>();
		GridView Grid;
		Button BtnSlide ;
		Button BtnDoc;
		Button BtnAddDoc;
		Button BtnImage;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//
			init ();

			//檢查範本資料夾是否建立
			this.DirCheck(new Java.IO.File (IconPath));

			//呼叫找資料的方法,回傳找到範本資料夾底下的檔案路徑
			var ReturnIcons=this.FindTemplateIcon (IconPath,visibleThings);

			Grid.Adapter = new ImageAdapter (this,IconPath,ReturnIcons);

			Grid.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args) {
				Toast.MakeText (this, ReturnIcons[args.Position].FullName, ToastLength.Short).Show ();
			};
			BtnImage.Click += (object sender, System.EventArgs e) => 
			{
				var imageIntent = new Intent (Intent.ActionPick,uri);
				imageIntent.SetType ("image/png");
				imageIntent.PutExtra(Intent.ExtraAllowMultiple,true);

				StartActivityForResult (Intent.CreateChooser (imageIntent, "選取您要匯入的檔案"), ImagePick);
			};
			BtnAddDoc.Click+= (object sender, EventArgs e) => 
			{
				
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

		void init ()
		{
			Grid = FindViewById<GridView> (Resource.Id.gridview);
			BtnSlide = FindViewById<Button> (Resource.Id.BtnSlides);
			BtnDoc = FindViewById<Button> (Resource.Id.BtnDocuments);
			BtnAddDoc = FindViewById<Button> (Resource.Id.BtnAddDocuments);
			BtnImage = FindViewById<Button> (Resource.Id.BtnImages);
		}
		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{

			string publicDir = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath+"/Download/";
			base.OnActivityResult (requestCode, resultCode, data);

			if(resultCode==Result.Ok && requestCode==ImagePick)
			{
				if (data.Data != null) 
				{
					if (resultCode == Result.Ok) {

						//var imageView = FindViewById<ImageView> (Resource.Id.myImageView);
						//imageView.SetImageURI (data.Data);
						string Source = GetPathToImage(data.Data);

						string Des=System.IO.Path.Combine(publicDir,new Java.IO.File (Source).Name);
						if(new Java.IO.File(Des).Exists())
						{
							this.copy (new Java.IO.File(Source), new Java.IO.File(Des));
						}
						this.copy (new Java.IO.File(Source), new Java.IO.File(Des));
					}
				}
				else
				{
					ClipData clipData = data.ClipData;
					int count = clipData.ItemCount;
					if (count > 0) {
						Android.Net.Uri[] uris = new Android.Net.Uri[count];
						for (int i = 0; i < count; i++) {
							uris [i] = clipData.GetItemAt (i).Uri;
							string Source=GetPathToImage(uris [i]);
							string Des=System.IO.Path.Combine(publicDir,new Java.IO.File (Source).Name);
							if(new Java.IO.File(Des).Exists())
							{
								this.copy (new Java.IO.File(Source), new Java.IO.File(Des));
							}
							this.copy (new Java.IO.File(Source), new Java.IO.File(Des));
						}
					}
				}
			}
			if(resultCode==Result.Canceled)
			{
				//NotThink
			}
			if(resultCode==Result.Ok && requestCode==PdfPick)
			{

			}

		}
		private String GetPathToImage(Android.Net.Uri uri)
		{
			string path = null;
			// The projection contains the columns we want to return in our query.
			string[] projection = new[] { Android.Provider.MediaStore.Audio.Media.InterfaceConsts.Data };
			using (ICursor cursor = ManagedQuery(uri, projection, null, null, null))
			{
				if (cursor != null)
				{
					int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Audio.Media.InterfaceConsts.Data);
					cursor.MoveToFirst();
					path = cursor.GetString(columnIndex);
				}
			}
			return path;
		}
		private void ShowAlert(string message, EventHandler<Android.Content.DialogClickEventArgs> positiveButtonClickHandle)
		{

			AlertDialog.Builder alert = new AlertDialog.Builder (this);

			alert.SetTitle (message);

			alert.SetPositiveButton ("OK!", positiveButtonClickHandle);

			RunOnUiThread (() => {
				alert.Show();
			});
		}
		public void copy(Java.IO.File src, Java.IO.File dst)  {
			InputStream sou = new FileInputStream(src);
			OutputStream des = new FileOutputStream(dst);

			// Transfer bytes from in to out
			byte[] buf = new byte[1024];
			int len;
			while ((len = sou.Read(buf)) > 0) {
				des.Write(buf, 0, len);
			}
			sou.Close();
			des.Close();
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


