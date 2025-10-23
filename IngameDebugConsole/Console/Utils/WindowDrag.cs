using Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InGameDebugConsole;

public class WindowDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private RectTransform Parent;
	[SerializeField] private RectTransform Canvas;
	
	private bool _isDragging = false;
	private Vector2 _offset;
	

	public void OnBeginDrag(PointerEventData eventData)
	{
		_isDragging = true;
		_offset = (eventData.position - (Vector2)Parent.position)/* * Canvas.localScale.x*/;
		// Debug.Log(
			// $"Offset: {_offset}, pos: {eventData.position}, Window pos: {(Vector2)Parent.position}");
	}

	public void OnDrag(PointerEventData eventData)
	{
		Parent.position = eventData.position - _offset;
		bool topBarSurpassedCanvasTop = Parent.anchoredPosition.y > 0;
		float topBarHeight = 20;
		bool topBarSurpassedCanvasBottom = Parent.anchoredPosition.y - topBarHeight < -Canvas.sizeDelta.y;
		if (topBarSurpassedCanvasTop)
		{
			Parent.anchoredPosition = new Vector2(Parent.anchoredPosition.x, 0);
		}else if (topBarSurpassedCanvasBottom)
		{
			Parent.anchoredPosition = new Vector2(Parent.anchoredPosition.x, -Canvas.sizeDelta.y + topBarHeight);
		}

		const float spaceForDragging = 20;
		const float spaceForDraggingRightSide = 60;
		bool surpassedCanvasRight = Parent.anchoredPosition.x + spaceForDragging > Canvas.sizeDelta.x;
		bool surpassedCanvasLeft = Parent.anchoredPosition.x - spaceForDraggingRightSide < -Parent.sizeDelta.x;

		if (surpassedCanvasRight)
		{
			Parent.anchoredPosition = new Vector2(Canvas.sizeDelta.x - spaceForDragging, Parent.anchoredPosition.y);
		}else if (surpassedCanvasLeft)
		{
			Parent.anchoredPosition = new Vector2((Parent.sizeDelta.x - spaceForDraggingRightSide) * -1,
				Parent.anchoredPosition.y);
		}

	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DebugLogManager.Instance.SaveLocation();
		_isDragging = false;
	}
}