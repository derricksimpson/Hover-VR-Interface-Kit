﻿using System;

namespace Hover.Demo.CastCubes.Items {

	/*================================================================================================* /
	public class DemoCustomSizeListener : DemoBaseListener<ISliderItem> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		protected override void Setup() {
			base.Setup();
			Item.GetFormattedLabel = (s => Component.Label+": "+Math.Round(s.RangeValue));
			Item.OnValueChanged += HandleValueChanged;
		}

		/*--------------------------------------------------------------------------------------------* /
		protected override void BroadcastInitialValue() {
			HandleValueChanged(Item);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		private void HandleValueChanged(ISelectableItem<float> pItem) {
			int size = (int)Math.Round(Item.RangeValue);
			ItemSett.UpdateAllSettings((x => ((ItemVisualSettingsStandard)x).TextSize = size));
		}

	}*/

}
