using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EnemyInfoHolder))]
[AddComponentMenu("Tower Defense/Enemy/Enemy Moving")]
public class EnemyMoving : MonoBehaviour
{
    public const float Sqrt2 = 1.41421356237f;
    
    public Path Path;

    public EnemyInfoHolder enemyInfoHolder;

    public float Speed => enemyInfoHolder.EnemyInfo.speed;
    public int TakeLife => enemyInfoHolder.EnemyInfo.damage;
    
    
    [SerializeField]
    private IEnumerator<Vector2> _pathEnumerable;

    // Start is called before the first frame update
    void Start()
    {
        enemyInfoHolder = GetComponent<EnemyInfoHolder>();

        _pathEnumerable = Path
            .Points
            .Select(t => (Vector2)t.position)
            .GetEnumerator();

        _pathEnumerable.MoveNext();
        transform.position = _pathEnumerable.Current;
        _pathEnumerable.MoveNext();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lastPosition = transform.position;
        Vector2 currentPosition = Vector2.MoveTowards(
            lastPosition, 
            _pathEnumerable.Current,
            Speed * Sqrt2 * Time.deltaTime
        );

        transform.position = currentPosition;

        if (lastPosition == currentPosition)
            OnEndPoint();
    }

    private void OnEndPoint()
    {
        if (_pathEnumerable.MoveNext()) 
            return;
        
        OnEndPath();
    }

    private void OnEndPath()
    {
        print("End path");
        enemyInfoHolder.spawner.AddKill();
        LifeManager.Instance.TakeLife(TakeLife);
        Destroy(gameObject);
    }

    
}
