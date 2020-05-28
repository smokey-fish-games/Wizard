using System.Collections.Generic;
using UnityEngine;
public abstract class IEffectable :  MonoBehaviour
{
    // Basic stats
    public float CurrentHP { get; set; }
    public float CurrentMana { get; set; }
    public float CurrentStamina { get; set; }
    public float MaxHP { get; set; }
    public float MaxMana { get; set; }
    public float MaxStamina { get; set; }

    // things that need to return something
    public abstract Transform GetTransform();
    public abstract Renderer GetRenderer();
    public abstract float GetSpeed();
    public abstract float GetGravity();
    public abstract Vector3 GetDefaultScale();
    public abstract float GetDefaultSpeed();
    public abstract float GetDefaultGravity();
    public abstract float GetDefaultJumpHeight();

    // Things that need to set something
    public abstract void SetTransform(Transform t);
    public abstract void SetRenderer(Renderer r);
    public abstract void SetSpeed(float speed);
    public abstract void SetGravity(float gravity);
    public abstract void SetJumpHeight(float height);

    // Standard methods
    List<int> currentEffects = new List<int>();

    public SOEffect[] getCurrentEffects()
    {
        int[] IDs = currentEffects.ToArray();
        List<SOEffect> temp = new List<SOEffect>(0);
        for (int i = 0; i < IDs.Length; i++)
        {
            temp.Add(SOEffect.GetByID(IDs[i]));
        }

        return temp.ToArray();
    }
    public void AddEffect(SOEffect e)
    {
        if(!AffectedBy(e))
        {
            currentEffects.Add(e.ID);
        }
    }
    public void RemoveEffect(SOEffect e)
    {
        if (AffectedBy(e))
        {
            currentEffects.Remove(e.ID);
        }
    }

    public bool AffectedBy(SOEffect e)
    {
        return currentEffects.Contains(e.ID);
    }

    public void Harm(float damage)
    {
        Debug.Log("hurting for " + damage);
        if (CurrentHP > 0)
        {
            CurrentHP -= damage;
        }
    }

    public void Heal(float heal)
    {
        Debug.Log("Healing for " + heal);
        CurrentHP += heal;
        if(CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
    }
}
