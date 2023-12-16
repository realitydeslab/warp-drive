using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using VisualEffect = UnityEngine.VFX.VisualEffect;

public class VfxStreamControl : MonoBehaviour
{
    public GameObject portalVfx;
    public GameObject finger;
    float timeHandSteleActivated; // 新增字段
    float timeWallHandActivated; // 新增字段

    void Start()
    {
        timeHandSteleActivated = 0f; // 初始化为0
        timeWallHandActivated = 0f; // 初始化为0
    }

    void Update()
    {
        var vfxComponent = portalVfx.GetComponent<VisualEffect>();
        if (vfxComponent != null)
        {
            if (vfxComponent.GetBool("Hand-Stele")) // 检查"Hand-Stele"是否为true
        {
            if (timeHandSteleActivated == 0f) // 如果还没有开始计时
            {
                timeHandSteleActivated = Time.time; // 记录Hand-Stele被激活的时间
                Debug.Log("Start timing."); // 输出日志信息
            }
            else if (Time.time - timeHandSteleActivated >= 15f) // 如果已经过去了15秒
            {
                vfxComponent.SetBool("Information module disappears", false); // 将"Information module disappears"设为false
            }
        }
        }
    }
}
/*public class Finger : MonoBehaviour
{
    public bool IsInTrigger { get; private set; } // 添加一个属性来跟踪finger是否在任何一个触发器内

    private void OnTriggerEnter(Collider other)
    {
        IsInTrigger = true; // 当finger进入一个触发器时，将IsInTrigger设为true
    }

    private void OnTriggerExit(Collider other)
    {
        IsInTrigger = false; // 当finger离开一个触发器时，将IsInTrigger设为false
    }
}*/
