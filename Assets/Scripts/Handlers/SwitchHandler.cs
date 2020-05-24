using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHandler : RayCastHandler
{
    private SwitchBoxSwitch sw = null;
    // Start is called before the first frame update
    void Start()
    {
        sw = GetComponent<SwitchBoxSwitch>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void RayHit() {
        base.RayHit();
        if (sw == null) return;
        sw.SetSwitchOn(true);
    }
}
