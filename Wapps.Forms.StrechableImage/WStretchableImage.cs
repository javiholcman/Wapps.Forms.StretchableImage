using System;
using Xamarin.Forms;

namespace Wapps.Forms.Controls
{
	public class WStretchableImage : Image
	{
		public WStretchableImage()
		{
			
		}

		#region Property: CapInsets

		/// <summary>
		/// The MaxLength property
		/// </summary>
		public static readonly BindableProperty CapInsetsProperty = BindableProperty.Create("CapInsets", typeof(Thickness), typeof(WStretchableImage), new Thickness(0, 0, 0, 0));

		/// <summary>
		/// Gets or sets the CapInset Property
		/// </summary>
		public Thickness CapInsets
		{
			get { return (Thickness)this.GetValue(CapInsetsProperty); }
			set { this.SetValue(CapInsetsProperty, value); }
		}

		#endregion

		#region Flush Method

		public Action FlushDelegate;

		public void Flush()
		{
			FlushDelegate?.Invoke();
		}

		#endregion

	}
}
