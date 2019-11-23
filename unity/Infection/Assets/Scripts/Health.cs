﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Health
{
    public int maxHP;
    public int currentHP;

    public Health(int max)
    {
        maxHP = currentHP = max;
    }

    public virtual void GainHP(int amt)
    {
        currentHP += amt;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public virtual void LoseHP(int amt)
    {
        currentHP -= amt;
    }
}
