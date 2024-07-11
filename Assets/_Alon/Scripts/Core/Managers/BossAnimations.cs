using System;

public class StringValueAttribute : Attribute
{
    /// <summary>
    /// Public Fields
    /// </summary>
    public string StringValue { get; private set; }

    // End Of Local Variables

    public StringValueAttribute(string stringValue)
    {
        StringValue = stringValue;
    }
}


public enum BossAnimations
{
    [StringValue("idle")] PLAYER_IDLE,

    [StringValue("attack")] PLAYER_ATTACK,

    [StringValue("walk")] PLAYER_WALK,

    [StringValue("run")] PLAYER_RUN,

    [StringValue("death")] PLAYER_DEATH,

    [StringValue("fly")] PLAYER_FLY,

    [StringValue("fly_attack")] PLAYER_FLY_ATTACK
}