public static class EnemyTypeMethods
{
    public static bool IsAttack(this EnemyType enemy, EnemyType bullet)
    {
        return (enemy & EnemyType.EnemyBoss & bullet) != 0
               && (enemy & EnemyType.Universal & bullet) != 0;
    }

    public static bool IsValid(this EnemyType type)
    {
        return (type & (EnemyType.Boss | EnemyType.Enemy)) != 0 
               && (type & (EnemyType.Fly | EnemyType.Ground)) != 0;
    }
}