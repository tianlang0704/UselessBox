using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DragonBones;

public class SwitchBoxSwitch : MonoBehaviour
{
    private Action<bool> callback = null;
    private bool isOn = false;
    private UnityArmatureComponent uae;
    // Start is called before the first frame update
    void Start()
    {
        uae = GetComponent<UnityArmatureComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSwitchOn(bool b) {
        if (isOn == b) return;
        // 设置值
        isOn = b;

        // 设置动画
        if (uae != null) {
            if (isOn) {
                uae.animation.FadeIn("on", 0);
            } else {
                uae.animation.FadeIn("off", 0);
            }
        }

        // 回调
        if (callback != null) {
            callback(isOn);
        }
    }

    public bool IsSwitchOn() {
        return isOn;
    }

    public void SetSwitchChangeCallback(Action<bool> cb) {
        callback = cb;
    }
}
