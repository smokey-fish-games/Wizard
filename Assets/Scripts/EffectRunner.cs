using System;
using System.Collections;
using UnityEngine;

public class EffectRunner : MonoBehaviour
{
    GameController gc;
    private void Awake()
    {
        gc = GetComponent<GameController>();
        DeveloperConsole.instance.RegisterCommand("applyeffect",  "[ID] Applies a given effect to the player.", DevCommandAddEffect);
        DeveloperConsole.instance.RegisterCommand("removeeffect", "[ID] Removes a given effect from the player.", DevCommandRemoveEffect);

        //sets up the effects below
        SOEffect[] AllEffects = SOEffect.GetAll();
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

    bool DevCommandAddEffect(string[] parms)
    {
        if(parms.Length != 1)
        {
            DeveloperConsole.instance.writeError("Missing Effect ID or too many parameters.");
            return false;
        }
        if(!Int32.TryParse(parms[0].Trim(), out int id))
        {
            DeveloperConsole.instance.writeError("Effect ID must be a number.");
            return false;
        }

        SOEffect s = SOEffect.GetByID(id);
        if(s==null)
        {
            DeveloperConsole.instance.writeError("Unknown Effect ID " + id);
            return false;
        }
        GameObject charo = gc.GetCharacter();
        if(charo == null)
        {
            DeveloperConsole.instance.writeError("Unable to get player.(1)");
            return false;
        }
        IEffectable toEffect = charo.GetComponent<IEffectable>();
        if(toEffect == null)
        {
            DeveloperConsole.instance.writeError("Unable to get player.(2)");
            return false;
        }

        s.onEffect(toEffect,1);
        DeveloperConsole.instance.writeMessage("Applied effect " + s.PrintString() + " to player");
        return true;
    }

    bool DevCommandRemoveEffect(string[] parms)
    {
        if (parms.Length != 1)
        {
            DeveloperConsole.instance.writeError("Missing Effect ID or too many parameters.");
            return false;
        }
        if (!Int32.TryParse(parms[0].Trim(), out int id))
        {
            DeveloperConsole.instance.writeError("Effect ID must be a number.");
            return false;
        }

        SOEffect s = SOEffect.GetByID(id);
        if (s == null)
        {
            DeveloperConsole.instance.writeError("Unknown Effect ID " + id);
            return false;
        }
        GameObject charo = gc.GetCharacter();
        if (charo == null)
        {
            DeveloperConsole.instance.writeError("Unable to get player.(1)");
            return false;
        }
        IEffectable toEffect = charo.GetComponent<IEffectable>();
        if (toEffect == null)
        {
            DeveloperConsole.instance.writeError("Unable to get player.(2)");
            return false;
        }

        toEffect.RemoveEffect(s);
        DeveloperConsole.instance.writeMessage("Removed effect " + s.PrintString() + " from player");
        return true;
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

    public bool Nothing(IEffectable noop1, float noop2)
    {
        return true;
    }

    public bool PoisonEntity(IEffectable toPoison, float potency)
    {
        float damage = 1;
        int time = 10;
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_POISON);

        if (applyAffect(thisEffect, toPoison))
        {
            // Coroutine
            StartCoroutine(DoT(damage * potency, time, toPoison , thisEffect));
        }       

        return true;
    }
    public bool HealOverTime(IEffectable toHeal, float potency)
    {
        float heal = 1;
        int time = 10;
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_REGEN);

        if (applyAffect(thisEffect, toHeal))
        {
            // Coroutine
            StartCoroutine(DoT(heal * potency, time, toHeal, thisEffect));
        }

        return true;
    }

    IEnumerator DoT(float ammount, int time, IEffectable target, SOEffect effect)
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

    public bool HealEntity(IEffectable toHeal, float potency)
    {
        float value = 20;
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_HEAL);

        if (applyAffect(thisEffect, toHeal))
        {
            toHeal.Heal(value * potency);
        }

        toHeal.RemoveEffect(thisEffect);

        return true;
    }

    public bool KillEntity(IEffectable toKill, float noop)
    {
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_DEATH);

        if(applyAffect(thisEffect, toKill))
        {
            toKill.Harm(toKill.MaxHP);
        }

        toKill.RemoveEffect(thisEffect);

        return true;
    }

    public bool CureEntity(IEffectable toCure, float noop)
    {
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_CURE);
        if (applyAffect(thisEffect, toCure))
        {
            toCure.RemoveEffect(SOEffect.GetByID(CONSTANTS.EFFECT_POISON));
        }
        toCure.RemoveEffect(thisEffect);
        return false;
    }

    public bool EnlargeEntity(IEffectable target, float potency)
    {
        float scaleup = 2f;
        float time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.GetByID(CONSTANTS.EFFECT_SHRINK));
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_ENLARGE);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(ShrinkLarge(scaleup, time * potency, target, thisEffect));
        }

        return true;
    }

    public bool ShrinkEntity(IEffectable target, float potency)
    {
        float scaleup = 0.5f;
        float time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.GetByID(CONSTANTS.EFFECT_ENLARGE));
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_SHRINK);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(ShrinkLarge(scaleup, time * potency, target, thisEffect));
        }

        return true;
    }

    IEnumerator ShrinkLarge(float targetScale, float time, IEffectable target, SOEffect effect)
    {
        Vector3 tagetScaleV3 = target.GetDefaultScale() * targetScale;

        for (float i = 0; i < time; i=i+1f)
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


    public bool SpeedEntity(IEffectable target, float potency)
    {
        float scaleup = 2f;
        float time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.GetByID(CONSTANTS.EFFECT_SLOW));
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_SPEED);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(SpeedSlow(scaleup, time * potency, target, thisEffect));
        }

        return true;
    }
    public bool SlowEntity(IEffectable target, float potency)
    {
        float scaleup = 0.5f;
        float time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.GetByID(CONSTANTS.EFFECT_SPEED));
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_SLOW);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(SpeedSlow(scaleup, time * potency, target, thisEffect));
        }

        return true;
    }

    IEnumerator SpeedSlow (float factor, float time, IEffectable target, SOEffect effect)
    {
        float targetSpeed = target.GetDefaultSpeed() * factor;

        for (float i = 0; i < time; i=i+1f)
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



    public bool LowGravEntity(IEffectable target, float potency)
    {
        float scaleup = 0.5f;
        float time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.GetByID(CONSTANTS.EFFECT_HIGH_GRAV));
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_LOW_GRAV);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(Gravupdown(scaleup, time * potency, target, thisEffect));
        }

        return true;
    }
    public bool HighGravEntity(IEffectable target, float potency)
    {
        float scaleup = 5f;
        float time = 10;

        // Conflicting
        target.RemoveEffect(SOEffect.GetByID(CONSTANTS.EFFECT_LOW_GRAV));
        SOEffect thisEffect = SOEffect.GetByID(CONSTANTS.EFFECT_HIGH_GRAV);

        if (applyAffect(thisEffect, target))
        {
            StartCoroutine(Gravupdown(scaleup, time * potency, target, thisEffect));
        }

        return true;
    }


    IEnumerator Gravupdown(float factor, float time, IEffectable target, SOEffect effect)
    {
        float targetGrav = target.GetDefaultGravity() * factor;
        float jumpHeight = target.GetDefaultJumpHeight() / factor;

        for (float i = 0; i < time; i=i+1f)
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
