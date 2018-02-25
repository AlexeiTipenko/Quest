using UnityEngine;
using UnityEngine.EventSystems;

public class CardHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {

        //transform.localScale += new Vector3(0.5F, 0.5F, 0);
        //transform.localPosition += new Vector3(0.25F, 0.25F, 0);

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        //transform.localScale -= new Vector3(0.5F, 0.5F, 0);
        //transform.localPosition -= new Vector3(0.25F, 0.25F, 0);

    }

}
