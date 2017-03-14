using System;
using System.Collections.Generic;
using System.ComponentModel;
using UIKit;
using Wapps.Forms;
using Wapps.Forms.Controls;
using Wapps.Forms.Controls.IOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WStretchableImage), typeof(WStretchableImageRenderer))]

namespace Wapps.Forms.Controls.IOS
{
	public class WStretchableImageRenderer : ImageRenderer
	{
		Dictionary<string, UIImage> _sources = new Dictionary<string, UIImage>();

		public WStretchableImageRenderer()
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			var view = (WStretchableImage)e.NewElement;
			if (view != null)
			{
				SetSource(view);
			}

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
					var image = UIImage.FromBundle(fileName);
					image = image.CreateResizableImage(new UIEdgeInsets((nfloat)view.CapInsets.Top, (nfloat)view.CapInsets.Left, (nfloat)view.CapInsets.Bottom, (nfloat)view.CapInsets.Right), UIImageResizingMode.Stretch);
					_sources[fileName] = image;
				}

				Control.ContentMode = UIViewContentMode.ScaleToFill;
				Control.Image = _sources[fileName];
			}
		}
	}

}
