﻿using Hover.Layouts.Rect;
using Hover.Renderers.Utils;
using Hover.Utils;
using UnityEngine;

namespace Hover.Layouts.Arc {

	/*================================================================================================*/
	public class HoverLayoutArcRow : HoverLayoutArcGroup, IArcLayoutable, IRectLayoutable {
		
		public const string OuterRadiusName = "OuterRadius";
		public const string InnerRadiusName = "InnerRadius";
		public const string ArcAngleName = "ArcAngle";
		public const string RectAnchorName = "RectAnchor";

		public enum ArrangementType {
			Forward,
			Reverse
		}
		
		[DisableWhenControlled(DisplaySpecials=true)]
		public ArrangementType Arrangement = ArrangementType.Forward;
		
		[DisableWhenControlled(RangeMin=0)]
		public float OuterRadius = 0.1f;
		
		[DisableWhenControlled(RangeMin=0)]
		public float InnerRadius = 0.04f;

		[DisableWhenControlled(RangeMin=0, RangeMax=360)]
		public float ArcAngle = 135;
		
		[DisableWhenControlled(RangeMin=0)]
		public float RadiusPadding = 0;

		[DisableWhenControlled(RangeMin=0, RangeMax=90)]
		public float AnglePadding = 0;

		[DisableWhenControlled(RangeMin=-180, RangeMax=180)]
		public float StartingAngle = 0;

		[DisableWhenControlled]
		public AnchorType RectAnchor = AnchorType.MiddleCenter;

		private Vector2? vRectSize;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override void TreeUpdate() {
			base.TreeUpdate();
			UpdateLayoutWithFixedSize();

			if ( vRectSize == null ) {
				Controllers.Set(RectAnchorName, this);
				RectAnchor = AnchorType.MiddleCenter;
			}

			vRectSize = null;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetArcLayout(float pOuterRadius, float pInnerRadius, 
													float pArcAngle, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);
			Controllers.Set(InnerRadiusName, pController);
			Controllers.Set(ArcAngleName, pController);

			OuterRadius = pOuterRadius;
			InnerRadius = pInnerRadius;
			ArcAngle = pArcAngle;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController) {
			Controllers.Set(OuterRadiusName, pController);

			OuterRadius = Mathf.Min(pSizeX, pSizeY)/2;
			vRectSize = new Vector2(pSizeX, pSizeY);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void UpdateLayoutWithFixedSize() {
			int itemCount = vChildItems.Count;

			if ( itemCount == 0 ) {
				return;
			}

			bool isRev = (Arrangement == ArrangementType.Reverse);
			float angleSumPad = AnglePadding*(itemCount-1);// + RadiusPadding*2;
			float relSumArcAngle = 0;
			float paddedOuterRadius = OuterRadius-RadiusPadding;
			float paddedInnerRadius = InnerRadius+RadiusPadding;
			float availAngle = ArcAngle-angleSumPad;
			float angle = StartingAngle-ArcAngle/2;

			Vector2 anchorPos = RendererUtil.GetRelativeAnchorPosition(RectAnchor);
			anchorPos.x *= (vRectSize == null ? OuterRadius*2 : ((Vector2)vRectSize).x);
			anchorPos.y *= (vRectSize == null ? OuterRadius*2 : ((Vector2)vRectSize).y);
			
			for ( int i = 0 ; i < itemCount ; i++ ) {
				HoverLayoutArcGroupChild item = vChildItems[i];
				relSumArcAngle += item.RelativeArcAngle;
			}

			for ( int i = 0 ; i < itemCount ; i++ ) {
				int childI = (isRev ? itemCount-i-1 : i);
				HoverLayoutArcGroupChild item = vChildItems[childI];
				IArcLayoutable elem = item.Elem;
				float elemRelAngle = availAngle*item.RelativeArcAngle/relSumArcAngle;
				float relInset = (paddedOuterRadius-paddedInnerRadius)*(1-item.RelativeThickness)/2;
				float elemThick = paddedOuterRadius-paddedInnerRadius-relInset*2;
				float elemRelArcAngle = availAngle*item.RelativeArcAngle;
				float radiusOffset = elemThick*item.RelativeRadiusOffset;
				float elemStartAngle = angle + elemRelAngle/2 +
					elemRelArcAngle*item.RelativeStartAngleOffset;

				elem.SetArcLayout(
					paddedOuterRadius-relInset+radiusOffset,
					paddedInnerRadius+relInset+radiusOffset,
					elemRelAngle,
					this
				);

				elem.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".x", this);
				elem.Controllers.Set(SettingsControllerMap.TransformLocalPosition+".y", this);
				elem.Controllers.Set(SettingsControllerMap.TransformLocalRotation, this);

				Vector3 localPos = elem.transform.localPosition;
				localPos.x = anchorPos.x;
				localPos.y = anchorPos.y;

				elem.transform.localPosition = localPos;
				elem.transform.localRotation = Quaternion.AngleAxis(elemStartAngle, Vector3.back);

				angle += elemRelAngle+AnglePadding;
			}
		}

	}

}
