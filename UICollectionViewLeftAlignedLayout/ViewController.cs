using System;

using UIKit;

namespace UICollectionViewLeftAlignedLayout
{
	public partial class ViewController : UICollectionViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			CollectionView.RegisterNibForCell (DefaultCell.Nib, DefaultCell.Key);

			CollectionView.DataSource = new CollectionSource ();
			CollectionView.Delegate = new CollectionDelegate ();
			CollectionView.CollectionViewLayout = new UICollectionViewLeftAlignedLayout ();
		}

		public override bool PrefersStatusBarHidden ()
		{
			return true;
		}


		private class CollectionSource:UICollectionViewDataSource
		{
			
			public override nint NumberOfSections (UICollectionView collectionView)
			{
				return 2;
			}

			public override nint GetItemsCount (UICollectionView collectionView, nint section)
			{
				return  section == 0 ? 20 : 80;
			}

			public override UICollectionViewCell GetCell (UICollectionView collectionView, Foundation.NSIndexPath indexPath)
			{
				var cell = collectionView.DequeueReusableCell(DefaultCell.Key, indexPath) as DefaultCell;
				cell.ContentView.Layer.BorderColor = UIColor.Blue.CGColor;
				cell.ContentView.Layer.BorderWidth = 2;
				return cell;
			}
		}

		private class CollectionDelegate:UICollectionViewDelegateFlowLayout
		{
			private Random random;
			public CollectionDelegate ()
			{
				random = new Random();
			}

			public override CoreGraphics.CGSize GetSizeForItem (UICollectionView collectionView, UICollectionViewLayout layout, Foundation.NSIndexPath indexPath)
			{
				var width = random.Next (34, 90);
				return new CoreGraphics.CGSize (width, 60);
			}

			public override nfloat GetMinimumInteritemSpacingForSection (UICollectionView collectionView, UICollectionViewLayout layout, nint section)
			{
				return section == 0 ? 15 : 5;
			}

			public override UIEdgeInsets GetInsetForSection (UICollectionView collectionView, UICollectionViewLayout layout, nint section)
			{
				return new UIEdgeInsets (10, 10, 10, 10);
			}
		}
	}
}

