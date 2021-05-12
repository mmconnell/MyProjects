using UnityEngine;

[CreateAssetMenu(fileName = nameof(PrefabPool), menuName = "Custom/Pool/" + nameof(PrefabPool))]
public class PrefabPool : A_Pool<PoolableBehaviour>
{
    public GameObject prefab;

    protected override PoolableBehaviour InternalBuildObject()
    {
        GameObject go = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        PoolableBehaviour poolable = go.GetComponent<PoolableBehaviour>();
        if (!poolable)
        {
            poolable = go.AddComponent<PoolableBehaviour>();
        }
        return poolable;
    }
}
