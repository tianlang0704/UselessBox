using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class SwitchBox : MonoBehaviour
{
    public SwitchBoxSwitch switchPrefab;
    public UnityEngine.Transform leftAnchor;
    public UnityEngine.Transform rightAnchor;
    public int switchCount = 1;
    private List<SwitchBoxSwitch> switchList = new List<SwitchBoxSwitch>();

    private UnityArmatureComponent uac = null;
    private DragonBones.AnimationState rodMoveState = null;
    private SwitchBoxSwitch curSwitch = null;
    private SwitchBoxSwitch lastSwitch = null;
    bool isUp = false;
    bool isUpLastUpdate = false;
    float curRodPosPerc = 0f;
    // Start is called before the first frame update
    void Start()
    {
        InitDB();
        InitSwitches();
    }

    void InitDB() {
        uac = GetComponentInChildren<UnityArmatureComponent>();
        uac.AddDBEventListener(EventObject.COMPLETE, (string type, EventObject eventObject) => {
            if(eventObject.animationState.name == "rod_up") {
                if (curSwitch != null) {
                    curSwitch.SetSwitchOn(false);
                }
                // UpdateUpStates();
            }
        });
        uac.animation.Play("idle");

        // // 设置移动动画
        // rodMoveState = uac.animation.FadeIn("rod_move", 0f, 1, 2, "moveGroup");
        // rodMoveState.resetToPose = false;
        // rodMoveState.Stop();
    }

    void InitSwitches() {
        if (switchPrefab == null || leftAnchor == null || rightAnchor == null) return;
        var startPos = leftAnchor.position;
        var endPos = rightAnchor.position;
        var stepProgress = (1f / (switchCount + 1));
        for (int i = 1; i <= switchCount; i++) {
            var instPos = Vector3.Lerp(startPos, endPos, stepProgress * i);
            var sw = Instantiate(switchPrefab, instPos, Quaternion.identity);   
            sw.SetSwitchChangeCallback((bool isOn) => {
                UpdateUpStates();
            });
            switchList.Add(sw);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 更新箱子开关
        if (isUp != isUpLastUpdate) {
            if (isUp) {
                PlayUp();
            } else {
                PlayDown();
            }
            isUpLastUpdate = isUp;
        }
    }

    void UpdateUpStates() {
        curSwitch = GetOneOnSwitch();
        isUp = curSwitch != null;
        if (curSwitch != null) {
            curRodPosPerc = 1 - GetMovePercent(curSwitch);
        }
    }

    float GetMovePercent(SwitchBoxSwitch sw) {
        var totalDist = (rightAnchor.position - leftAnchor.position).magnitude;
        var swPos = sw.transform.position;
        var swDist = (swPos - leftAnchor.position).magnitude;
        return swDist /totalDist;
    }

    SwitchBoxSwitch GetOneOnSwitch() {
        SwitchBoxSwitch onSwitch = null;
        foreach (var item in switchList) {
            if (item.IsSwitchOn()) {
                onSwitch = item;
            }
        }
        return onSwitch;
    }
    
    void PlayUp() {
        if (curSwitch == null) return;

        var openState = uac.animation.FadeIn("cap_open", 0f, 1, 0);
        var upState = uac.animation.FadeIn("rod_up", 0f, 1, 1);

        // 设置移动动画
        rodMoveState = uac.animation.FadeIn("rod_move", 0f, 1, 2, "moveGroup");
        rodMoveState.resetToPose = false;
        rodMoveState.Stop();
        rodMoveState.currentTime = curRodPosPerc * rodMoveState.totalTime;
    }

    void PlayDown() {
        var closeState = uac.animation.FadeIn("cap_close", 0f, 1, 0);
        var downState = uac.animation.FadeIn("rod_down", 0f, 1, 1);
    }
}
