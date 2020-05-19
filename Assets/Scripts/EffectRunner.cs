using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRunner : MonoBehaviour
{
    private void Start()
    {
        //sets up the effects below
        SOEffect[] AllEffects = SOEffect.getAll();
        for(int i = 0; i < AllEffects.Length; i++)
        {
            switch (AllEffects[i].ID)
            {
                case CONSTANTS.EFFECT_NOTHING:
                    //nothing
                    AllEffects[i].onEffect = Nothing;
                    break;
                case CONSTANTS.EFFECT_DEATH:
                    // death
                    AllEffects[i].onEffect = KillEntity;
                    break;
                case CONSTANTS.EFFECT_HEAL:
                    // heal
                    AllEffects[i].onEffect = HealEntity;
                    break;
                case CONSTANTS.EFFECT_CURE:
                    AllEffects[i].onEffect = CureEntity;
                    break;
                case CONSTANTS.EFFECT_POISON:
                    AllEffects[i].onEffect = PoisonEntity;
                    break;
                case CONSTANTS.EFFECT_REGEN:
                    AllEffects[i].onEffect = HealOverTime;
                    break;
                case CONSTANTS.EFFECT_ENLARGE:
                    //nothing
                    AllEffects[i].onEffect = EnlargeEntity;
                    break;
                case CONSTANTS.EFFECT_SHRINK:
                    // death
                    AllEffects[i].onEffect = ShrinkEntity;
                    break;
                case CONSTANTS.EFFECT_LOW_GRAV:
                    // heal
                    AllEffects[i].onEffect = LowGravEntity;
                    break;
                case CONSTANTS.EFFECT_HIGH_GRAV:
                    AllEffects[i].onEffect = HighGravEntity;
                    break;
                case CONSTANTS.EFFECT_SLOW:
                    AllEffects[i].onEffect = SlowEntity;
                    break;
                case CONSTANTS.EFFECT_SPEED:
                    AllEffects[i].onEffect = SpeedEntity;
                    break;
                default:
                    Debug.LogError("Effect runner is unable to handle effect " + AllEffects[i].GetDebugString());
                    break;
            }
        }
    }

    bool applyAffect(SOEffect s, IEffectable target)
    {
        if (target.AffectedBy(s))
        {
            return false;
        }
        target.AddEffect(s);
        return true;
    }

    // Effect functions

    public bool Nothing(IEffectable toPoison)
    {
        return true;
    }

    public bool PoisonEntity(IEffectable toPoison)
    {
        int damage = 1;
        int time = 10;
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_POISON);

        if (applyAffect(thisEffect, toPoison))
        {
            // Coroutine
            StartCoroutine(DoT(damage, time, toPoison, thisEffect));
        }       

        return true;
    }
    public bool HealOverTime(IEffectable toHeal)
    {
        int heal = 1;
        int time = 10;
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_REGEN);

        if (applyAffect(thisEffect, toHeal))
        {
            // Coroutine
            StartCoroutine(DoT(heal, time, toHeal, thisEffect));
        }

        return true;
    }

    IEnumerator DoT(int ammount, int time, IEffectable target, SOEffect effect)
    {
        if (ammount != 0)
        {
            bool isHeal = (effect.ID == CONSTANTS.EFFECT_REGEN || effect.ID == CONSTANTS.EFFECT_HEAL);
            for (int i = 0; i < time; i++)
            {
                if (!target.AffectedBy(effect))
                {
                    // perhaps they got cured
                    break;
                }
                if (isHeal)
                {
                    // hurting
                    target.Heal(ammount);
                }
                else
                {
                    // healing
                    target.Harm(ammount);
                }
                    
                yield return new WaitForSeconds(1);
            }
            target.RemoveEffect(effect);
        }
    }

    public bool HealEntity(IEffectable toHeal)
    {
        int value = 20;
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_HEAL);

        if (applyAffect(thisEffect, toHeal))
        {
            toHeal.Heal(value);
        }

        toHeal.RemoveEffect(thisEffect);

        return true;
    }

    public bool KillEntity(IEffectable toKill)
    {
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_DEATH);

        if(applyAffect(thisEffect, toKill))
        {
            toKill.Harm(toKill.MaxHP);
        }

        toKill.RemoveEffect(thisEffect);

        return true;
    }

    public bool CureEntity(IEffectable toCure)
    {
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_CURE);
        if (applyAffect(thisEffect, toCure))
        {
            toCure.RemoveEffect(SOEffect.getByID(CONSTANTS.EFFECT_POISON));
        }
        toCure.RemoveEffect(thisEffect);
        return false;
    }

    public bool EnlargeEntity(IEffectable target)
    {
        float scaleup = 2f;
        int time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.getByID(CONSTANTS.EFFECT_SHRINK));
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_ENLARGE);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(ShrinkLarge(scaleup, time, target, thisEffect));
        }

        return true;
    }

    public bool ShrinkEntity(IEffectable target)
    {
        float scaleup = 0.5f;
        int time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.getByID(CONSTANTS.EFFECT_ENLARGE));
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_SHRINK);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(ShrinkLarge(scaleup, time, target, thisEffect));
        }

        return true;
    }

    IEnumerator ShrinkLarge(float targetScale, int time, IEffectable target, SOEffect effect)
    {
        Vector3 tagetScaleV3 = target.GetDefaultScale() * targetScale;

        for (int i = 0; i < time; i++)
        {
            if (!target.AffectedBy(effect))
            {
                // perhaps they got cured
                break;
            }

            Transform t2 = target.GetTransform();
            t2.localScale = tagetScaleV3;
            target.SetTransform(t2);

            yield return new WaitForSeconds(1);
        }
        Transform t = target.GetTransform();
        t.localScale = target.GetDefaultScale();
        target.SetTransform(t);

        target.RemoveEffect(effect);
    }


    public bool SpeedEntity(IEffectable target)
    {
        float scaleup = 2f;
        int time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.getByID(CONSTANTS.EFFECT_SLOW));
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_SPEED);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(SpeedSlow(scaleup, time, target, thisEffect));
        }

        return true;
    }
    public bool SlowEntity(IEffectable target)
    {
        float scaleup = 0.5f;
        int time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.getByID(CONSTANTS.EFFECT_SPEED));
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_SLOW);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(SpeedSlow(scaleup, time, target, thisEffect));
        }

        return true;
    }

    IEnumerator SpeedSlow (float factor, int time, IEffectable target, SOEffect effect)
    {
        float targetSpeed = target.GetDefaultSpeed() * factor;

        for (int i = 0; i < time; i++)
        {
            if (!target.AffectedBy(effect))
            {
                // perhaps they got cured
                break;
            }

            target.SetSpeed(targetSpeed);

            yield return new WaitForSeconds(1);
        }
        target.SetSpeed(target.GetDefaultSpeed());
        target.RemoveEffect(effect);
    }



    public bool LowGravEntity(IEffectable target)
    {
        float scaleup = 0.5f;
        int time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.getByID(CONSTANTS.EFFECT_HIGH_GRAV));
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_LOW_GRAV);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(Gravupdown(scaleup, time, target, thisEffect));
        }

        return true;
    }
    public bool HighGravEntity(IEffectable target)
    {
        float scaleup = 5f;
        int time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.getByID(CONSTANTS.EFFECT_LOW_GRAV));
        SOEffect thisEffect = SOEffect.getByID(CONSTANTS.EFFECT_HIGH_GRAV);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(Gravupdown(scaleup, time, target, thisEffect));
        }

        return true;
    }


    IEnumerator Gravupdown(float factor, int time, IEffectable target, SOEffect effect)
    {
        float targetGrav = target.GetDefaultGravity() * factor;
        float jumpHeight = target.GetDefaultJumpHeight() / factor;

        for (int i = 0; i < time; i++)
        {
            if (!target.AffectedBy(effect))
            {
                // perhaps they got cured
                break;
            }

            target.SetGravity(targetGrav);
            target.SetJumpHeight(jumpHeight);

            yield return new WaitForSeconds(1);
        }
        target.SetGravity(target.GetDefaultGravity());
        target.SetJumpHeight(target.GetDefaultJumpHeight());
        target.RemoveEffect(effect);
    }


    IEnumerator LerpAValue(float originalVal, float targetvalue, float holdfor, SOEffect effect, IEffectable target)
    {
        // TODO
        yield return new WaitForSeconds(1);
    }

}
