using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ClickHandler();
    }

    void ClickHandler() {
        if (Input.GetMouseButtonDown(0)) {
            var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit == null || hit.collider == null) return;
            var rayCastHandler = hit.collider.GetComponent<RayCastHandler>();
            if (rayCastHandler == null) return;
            rayCastHandler.RayHit();
        }
    }
}
