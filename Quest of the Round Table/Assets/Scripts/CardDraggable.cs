using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public void OnBeginDrag(PointerEventData eventData) {
		Debug.Log ("Begin drag.");
		this.transform.SetParent(this.transform.parent);
	}

	public void OnDrag(PointerEventData eventData) {
		Debug.Log ("Dragging...");
		this.transform.position = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData) {
		Debug.Log ("End drag.");
	}

}
