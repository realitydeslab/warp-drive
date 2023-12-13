using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using VisualEffect = UnityEngine.VFX.VisualEffect;

public class VfxStreamControl : MonoBehaviour
{
    //此代码旨在平滑地减少vfx粒子数量，直到空间中的可见粒子数量为零，粒子完全汇聚于
    //石碑后，关闭protal1中的trigger（这一步在animation timeline中操作）
}
