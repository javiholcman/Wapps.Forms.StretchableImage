using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Java.Nio;
using Wapps.Forms;
using Wapps.Forms.Controls;
using Wapps.Forms.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WStretchableImage), typeof(WStretchableImageRenderer))]

namespace Wapps.Forms.Droid
{
	public class WStretchableImageRenderer : ImageRenderer
	{
		static int NO_COLOR = 0x00000001;

		Dictionary<string, Drawable> _sources = new Dictionary<string, Drawable>();

		public WStretchableImageRenderer()
		{
			
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			var view = (WStretchableImage)e.NewElement;
			if (view != null)
			{
				view.FlushDelegate = Flush;
				SetSource(view);
			}
			
			SetSource(Element as WStretchableImage);
		}

		void Flush()
		{
			_sources = new Dictionary<string, Drawable>();
			SetSource(Element as WStretchableImage);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (WStretchableImage)Element;

			if (e.PropertyName == Image.SourceProperty.PropertyName)
				SetSource(view);

			SetSource(view);
		}

	 	void SetSource(WStretchableImage view)
		{
			if (view == null)
				return;
			
			if (view.Source != null && view.Source is FileImageSource)
			{
				var fileName = ((FileImageSource)view.Source).File;
				if (!_sources.ContainsKey(fileName))
				{
					var ninePatch = CreateFixedNinePatch(fileName, (int)view.CapInsets.Top, (int)view.CapInsets.Left, (int)view.CapInsets.Bottom, (int)view.CapInsets.Right);
					var drawable = new NinePatchDrawable(Android.App.Application.Context.Resources, ninePatch);
					_sources[fileName] = drawable;
				}

				Control.SetScaleType(Android.Widget.ImageView.ScaleType.FitXy);
				Control.SetImageDrawable(_sources[fileName]);
			}
		}

		int px(int dp, Bitmap bm)
		{
            return dp * rate(bm);
            //return dp * 4;
		}

        int rate (Bitmap bm) 
        {
            return bm.Density / 160;
        }

		public NinePatch CreateFixedNinePatch(string fileName, int top, int left, int bottom, int right)
		{
			int resId = ResourceManager.GetDrawableByName(fileName);

			var options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeResource(Android.App.Application.Context.Resources, resId, options);
			var width = options.OutWidth;
			var height = options.OutHeight;

			Bitmap bitmap = BitmapFactory.DecodeResource(Android.App.Application.Context.Resources, resId);

			var srcName = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_strech." + System.IO.Path.GetExtension(fileName);
			srcName = Guid.NewGuid().ToString();
			top = px(top, bitmap);
			left = px(left, bitmap);
            bottom = bitmap.Width - px(bottom, bitmap);
            right = bitmap.Height - px(right, bitmap);

			var buffer = GetByteBufferFixed(top, left, bottom, right);

			buffer.Rewind();

			IntPtr classHandle = JNIEnv.FindClass("java/nio/ByteBuffer");
			IntPtr methodId = JNIEnv.GetMethodID(classHandle, "array", "()[B");
			IntPtr resultHandle = JNIEnv.CallObjectMethod(buffer.Handle, methodId);
			byte[] result = JNIEnv.GetArray<byte>(resultHandle);
			JNIEnv.DeleteLocalRef(resultHandle);
			NinePatch patch = new NinePatch(bitmap, result, srcName);
			return patch;
		}

		public static ByteBuffer GetByteBufferFixed(int top, int left, int bottom, int right)
		{
			//Docs check the NinePatchChunkFile
			ByteBuffer buffer = ByteBuffer.Allocate(84).Order(ByteOrder.NativeOrder());
			//was translated
			buffer.Put((sbyte)0x01);
			//divx size
			buffer.Put((sbyte)0x02);
			//divy size
			buffer.Put((sbyte)0x02);
			//color size
			buffer.Put((sbyte)0x09);

			//skip
			buffer.PutInt(0);
			buffer.PutInt(0);

			//padding
			buffer.PutInt(0);
			buffer.PutInt(0);
			buffer.PutInt(0);
			buffer.PutInt(0);

			//skip 4 bytes
			buffer.PutInt(0);

			buffer.PutInt(left);
			buffer.PutInt(right);
			buffer.PutInt(top);
			buffer.PutInt(bottom);
			buffer.PutInt(NO_COLOR);
			buffer.PutInt(NO_COLOR);
			buffer.PutInt(NO_COLOR);
			buffer.PutInt(NO_COLOR);
			buffer.PutInt(NO_COLOR);
			buffer.PutInt(NO_COLOR);
			buffer.PutInt(NO_COLOR);
			buffer.PutInt(NO_COLOR);
			buffer.PutInt(NO_COLOR);
			return buffer;
		}

	}

}
