using System;

[Flags]
public enum EnemyType
{
    Ground = 0b1,
    Fly = 0b10,
    Universal = Ground | Fly,
    Enemy = 0b100,
    Boss = 0b1000,
    EnemyBoss = Enemy | Boss
}