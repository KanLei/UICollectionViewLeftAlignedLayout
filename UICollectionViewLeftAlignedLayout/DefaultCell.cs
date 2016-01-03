using System;

using Foundation;
using UIKit;

namespace UICollectionViewLeftAlignedLayout
{
	public partial class DefaultCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString ("DefaultCell");
		public static readonly UINib Nib;

		static DefaultCell ()
		{
			Nib = UINib.FromName ("DefaultCell", NSBundle.MainBundle);
		}

		public DefaultCell (IntPtr handle) : base (handle)
		{
		}
	}
}
