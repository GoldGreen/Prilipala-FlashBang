using System;
using UnityEngine;
using UnityEngine.Events;

public enum DamageType
{
    physical,
    magic,
    electric
}

[Serializable]
public class Character : IUpdatable
{
    public float MaxHealth => maxHealth;
    [SerializeField] private float maxHealth;

    public float Health => health;
    [SerializeField] private float health;

    public float PhysArmor => physArmor;
    [SerializeField] private float physArmor;

    public float MagicArmor => magicArmor;
    [SerializeField] private float magicArmor;

    public float EletricArmor => eletricArmor;
    [SerializeField] private float eletricArmor;

    public UnityEvent<Character> OnDataChanged { get; set; } = new UnityEvent<Character>();

    public Character()
    {
        maxHealth = 100;
        health = 100;

        physArmor = 0;
        magicArmor = 0;
        eletricArmor = 0;
    }

    public void Damaged(float damage, DamageType damageType)
    {
        float armor = GetArmorByType(damageType);
        health -= damage * (1 - ((0.048f * armor) / (0.7f + 0.048f * Mathf.Abs(armor))));

        if (health < 0)
            health = 0;

        OnDataChanged.Invoke(this);
    }

    public void AddHealth(float addedHealth)
    {
        maxHealth += addedHealth;
        health += addedHealth;
        OnDataChanged.Invoke(this);
    }

    public void RestoreHealth(float restoredHealth)
    {
        if (health != maxHealth)
        {
            health += restoredHealth;

            if (health > maxHealth)
                health = maxHealth;

            OnDataChanged.Invoke(this);
        }
    }

    public void AddArmor(float phys, float magic, float electric)
    {
        physArmor += phys;
        magicArmor += magic;
        eletricArmor += electric;
        OnDataChanged.Invoke(this);
    }

    public void GlobalUpdate()
    {
        maxHealth = 100;
        health = 100;
        physArmor = 0;
        magicArmor = 0;
        eletricArmor = 0;

        OnDataChanged.Invoke(this);
    }

    public void LocalUpdate()
    {
        health = maxHealth;
        OnDataChanged.Invoke(this);
    }

    private float GetArmorByType(DamageType type)
    {
        if (type == DamageType.physical)
            return physArmor;
        else if (type == DamageType.magic)
            return magicArmor;
        else if (type == DamageType.electric)
            return eletricArmor;

        return 0;
    }

    public void Update() => OnDataChanged.Invoke(this);
}