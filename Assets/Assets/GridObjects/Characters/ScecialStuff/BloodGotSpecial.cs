using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodGotSpecial : MonoBehaviour
{
    public ParticleSystem particle;
    public CharacterBase character;
    public float rate = 0;
    void Start()
    {
        particle = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        character = transform.parent.gameObject.GetComponent<GridCharacter>().GetCharacter();
    }

    void FixedUpdate()
    {

        foreach (BattleEffect effect in character.effects)
        {
            if (effect is EffectBloodLoss)
            {
                rate = effect.power / 10f;
            }
        }
        particle.emissionRate = rate;
    }
}
