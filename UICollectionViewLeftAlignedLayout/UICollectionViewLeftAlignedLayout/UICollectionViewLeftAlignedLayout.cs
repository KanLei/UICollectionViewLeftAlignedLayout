using System;
using System.Collections.Generic;
using System.Linq;

using UIKit;
using Foundation;
using CoreGraphics;

using ObjCRuntime;

namespace UICollectionViewLeftAlignedLayout
{
	// Extension Class
	public static class UICollectionViewLayoutAttributesExt
	{
		public static void LeftAlignFrame (this UICollectionViewLayoutAttributes attributes, UIEdgeInsets sectionInsets)
		{
			CGRect frame = attributes.Frame;
			frame.X = sectionInsets.Left;
			attributes.Frame = frame;
		}
	}

	public class UICollectionViewLeftAlignedLayout:UICollectionViewFlowLayout
	{
		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect (CGRect rect)
		{
			var resultAttributes = base.LayoutAttributesForElementsInRect (rect);
			foreach (var attributes in resultAttributes) {
				if (attributes.RepresentedElementKind == null) {
					var index = Array.IndexOf (resultAttributes, attributes);
					resultAttributes [index] = LayoutAttributesForItem (attributes.IndexPath);
				}
			}

			return resultAttributes;
		}

		public override UICollectionViewLayoutAttributes LayoutAttributesForItem (NSIndexPath indexPath)
		{
			var currentItemAttributes = base.LayoutAttributesForItem (indexPath).Copy () as UICollectionViewLayoutAttributes;
			var sectionInset = EvaluatedSectionInsetForItem (indexPath.Section);

			bool isFirstItemInSection = indexPath.Item == 0;
			nfloat layoutWidth = CollectionView.Frame.Width - sectionInset.Left - sectionInset.Right;

			if (isFirstItemInSection) {
				currentItemAttributes.LeftAlignFrame (sectionInset);
				return currentItemAttributes;
			}

			var previousIndexPath = NSIndexPath.FromItemSection (indexPath.Item - 1, indexPath.Section);
			CGRect previousFrame = LayoutAttributesForItem (previousIndexPath).Frame;
			nfloat previousFrameRightPoint = previousFrame.X + previousFrame.Width;
			CGRect currentFrame = currentItemAttributes.Frame;
			CGRect strecthedCurrentFrame = new CGRect (sectionInset.Left, currentFrame.Y, layoutWidth, currentFrame.Height);

			// if the current frame, once left aligned to the left and strectched to the full collection view
			//  width intersects the previous frame then they are on the same line
			bool isFirstItemInRow = CGRect.Intersect (previousFrame, strecthedCurrentFrame) == CGRect.Empty;

			if (isFirstItemInRow) {
				// make sure the first item on a line is left aligned
				currentItemAttributes.LeftAlignFrame (sectionInset);
				return currentItemAttributes;
			}

			CGRect frame = currentItemAttributes.Frame;
			frame.X = previousFrameRightPoint + EvaluatedMinimumInteritemSpacing (indexPath.Section);
			currentItemAttributes.Frame = frame;
			return currentItemAttributes;
		}


		private nfloat EvaluatedMinimumInteritemSpacing (nint section)
		{
			var delegateFlowLayout = CollectionView.Delegate as UICollectionViewDelegateFlowLayout;
			if (delegateFlowLayout != null &&
			    delegateFlowLayout.RespondsToSelector (new Selector ("collectionView:layout:minimumInteritemSpacingForSectionAtIndex:"))) {
				return delegateFlowLayout.GetMinimumInteritemSpacingForSection (CollectionView, this, section);
			} else {
				return MinimumInteritemSpacing;
			}
		}

		private UIEdgeInsets EvaluatedSectionInsetForItem (nint section)
		{
			var delegateFlowLayout = CollectionView.Delegate as UICollectionViewDelegateFlowLayout;

			if (delegateFlowLayout != null &&
			    delegateFlowLayout.RespondsToSelector (new Selector ("collectionView:layout:insetForSectionAtIndex:"))) {
				return delegateFlowLayout.GetInsetForSection (CollectionView, this, section);
			} else {
				return SectionInset;
			}
		}
	}
}

