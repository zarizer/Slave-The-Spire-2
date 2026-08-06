using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public float emmitDuration;
    public float objectDuraction;
    public ParticleSystem effect;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Init(float emmit_time, float object_time)
    {
        emmitDuration = emmit_time;
        objectDuraction = object_time;
    }

    public static void OneTimeBurst(GameObject effect, Transform pos)
    {
        var cur_effect = Burst(effect, pos, true);
        LeanTween.delayedCall(cur_effect.objectDuraction, () => { Destroy(cur_effect.gameObject); });
    }

    public static EffectManager Burst(GameObject effect, Transform pos, bool copy = false)
    {
        EffectManager cur_effect;
        if (copy) cur_effect = Instantiate(effect).GetComponent<EffectManager>();
        else cur_effect = effect.GetComponent<EffectManager>();
        cur_effect.transform.position = pos.position;
        cur_effect.effect.Play();
        LeanTween.delayedCall(cur_effect.emmitDuration, () => { cur_effect.effect.Stop(); });
        return cur_effect;
    }


}
