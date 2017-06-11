using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Wapps.Forms.Controls
{
	public partial class WStackLayout : AbsoluteLayout
	{
		public ImageSource BackgroundImage
		{
			get { return Image.Source; }
			set { Image.Source = value; }
		}

		public Thickness BackgroundCapInsets
		{
			get { return Image.CapInsets; }
			set { Image.CapInsets = value; }
		}

        public double Spacing
		{
            get { return StackLayoutContent.Spacing; }
			set { StackLayoutContent.Spacing = value; }
		}

        public Thickness ContentPadding
		{
            get { return StackLayoutContent.Padding; }
			set { StackLayoutContent.Padding = value; }
		}

		IList<View> _children = new ObservableCollection<View>();
		public new IList<View> Children
		{
			get { return _children; }
			private set { _children = value; }
		}

		WStretchableImage Image { get; set; }

		StackLayout StackLayoutContent { get; set; }

		public WStackLayout()
		{
			var children = new ObservableCollection<View>();
			children.CollectionChanged += Children_CollectionChanged;
			Children = children;

			Image = new WStretchableImage();
			AbsoluteLayout.SetLayoutBounds(Image, new Rectangle(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(Image, AbsoluteLayoutFlags.All);
			base.Children.Add(Image);
            
			StackLayoutContent = new StackLayout();
			AbsoluteLayout.SetLayoutBounds(StackLayoutContent, new Rectangle(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(StackLayoutContent, AbsoluteLayoutFlags.All);
			base.Children.Add(StackLayoutContent);
		}

		void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
			{
				foreach (var item in e.NewItems)
				{
					StackLayoutContent.Children.Add(item as View);
				}
			}
		}
	}
}
