public class Q_Distance : QueryableValue<int>
{
    public Q_Distance(int distance) : base(distance)
    {
    }
}

public class Q_Movement : QueryableValue<int>
{
    public Q_Movement(int distance) : base(distance)
    {
    }
}

public class Q_AtackAmount : QueryableValue<int>
{
    public Q_AtackAmount(int amount) : base(amount)
    {
    }
}

public class Q_AtackDamage : QueryableValue<int>
{
    public Q_AtackDamage(int amount) : base(amount)
    {
    }
}

public class Q_AtackRange : QueryableValue<float>
{
    public Q_AtackRange(float distance) : base(distance)
    {
    }
}


public class Q_Damage : QueryableValue<int>
{
    public Q_Damage(int damage) : base(damage)
    {
    }
}

public class Q_Danger : QueryableValue<int>
{
    public Q_Danger(int danger) : base(danger)
    {
    }
}

public class Q_EndurancePerLevel : QueryableValue<int>
{
    public Q_EndurancePerLevel(int endurance) : base(endurance)
    {
    }
}


public class Q_BaseEnduranceLevel : QueryableValue<int>
{
    public Q_BaseEnduranceLevel(int enduranceLevel) : base(enduranceLevel)
    {
    }
}




public class Q_Damage_Fire : Q_Damage
{
    public Q_Damage_Fire(int damage) : base(damage)
    {
    }
}