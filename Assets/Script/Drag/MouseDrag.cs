using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frames
	void Update ()
    {
		if(DragManager.instance.Target == this.gameObject)
        {
            Debug.Log("ADSFASDF");
            Vector3 mousedragposition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 worldobjectposition = Camera.main.ScreenToWorldPoint(mousedragposition);

            float z = transform.position.z;
            worldobjectposition.z = z; //고정시키기
            transform.position = worldobjectposition;
        }

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -10.0f;
            bool isHit = Physics.Raycast(mousePos, Vector3.forward, out hit, 100);

            if (isHit && hit.collider.gameObject == this.gameObject)
            {
                Vector3 newPos = transform.position;
                newPos.z -= 1f;
                transform.position = newPos;
                DragManager.instance.Target = this.gameObject;
            }
        }
	}
}
