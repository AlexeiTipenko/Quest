using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameBoard : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public void OnDrop(PointerEventData eventData) {

		//eventData.pointerDrag.name will give you the name of the prefab dropped
		Debug.Log ("Dropping to " + gameObject.name);
		//this.transform.position = eventData.position;

		CardDraggable card = eventData.pointerDrag.GetComponent<CardDraggable>();

		if (card != null) {
			card.parentToReturnTo = this.transform;
		}
			
	}

	public void OnPointerEnter(PointerEventData eventData) {

	}

	public void OnPointerExit(PointerEventData eventData) {

	}
}
