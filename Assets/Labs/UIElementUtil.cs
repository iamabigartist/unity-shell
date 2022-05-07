using UnityEngine;
using UnityEngine.UIElements;
namespace Labs
{
	public static class UIElementUtil
	{
		public static void SetBorder(this VisualElement ve, float width, Color color)
		{
			ve.style.borderLeftWidth = width;
			ve.style.borderLeftColor = color;
			ve.style.borderRightWidth = width;
			ve.style.borderRightColor = color;
			ve.style.borderTopWidth = width;
			ve.style.borderTopColor = color;
			ve.style.borderBottomWidth = width;
			ve.style.borderBottomColor = color;
		}
	}
}