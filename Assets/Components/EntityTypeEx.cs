using UnityEngine;
using UnityEngine.UI;

public static class EntityTypeEx
{
    public static void SetIcons(this EnemyType shopItemTargetType, Image enemy, Image boss, Image ground, Image fly)
    {
        enemy.color = EnemyColor(EnemyType.Enemy);
        boss.color = EnemyColor(EnemyType.Boss);
        ground.color = EnemyColor(EnemyType.Ground);
        fly.color = EnemyColor(EnemyType.Fly);

        Color EnemyColor(EnemyType type)
            => (shopItemTargetType & type) == 0 ? Color.clear : Color.white;
    }
}