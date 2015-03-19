﻿namespace Hover.Common.Items {

	/*================================================================================================*/
	public interface IItemDatabase<T> where T : class {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetData(IBaseItem pItem, T pData);
		
		/*--------------------------------------------------------------------------------------------*/
		T GetData(IBaseItem pItem);

	}

}