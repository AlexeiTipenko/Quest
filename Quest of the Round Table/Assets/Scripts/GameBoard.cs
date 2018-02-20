using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameBoard : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public void OnDrop(PointerEventData eventData) {
		//Debug.Log ("Dropping to " + gameObject.name);

		CardUI card = eventData.pointerDrag.GetComponent<CardUI>();

		if (card != null) 
			card.parentToReturnTo = this.transform;
		
			
	}

	public void OnPointerEnter(PointerEventData eventData) {

		if (eventData.pointerDrag == null)
			return;
		
		CardUI card = eventData.pointerDrag.GetComponent<CardUI>();

		if (card != null) 
			card.placeholderParent = this.transform;
		
	}

	public void OnPointerExit(PointerEventData eventData) {

		if (eventData.pointerDrag == null)
			return;
		
		CardUI card = eventData.pointerDrag.GetComponent<CardUI>();

		if (card != null && card.placeholderParent == this.transform) 
			card.placeholderParent = card.parentToReturnTo;
	}
}
