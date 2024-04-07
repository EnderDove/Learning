using UnityEngine;
using UnityEngine.Pool;

public class AttackHandler : MonoBehaviour
{
    public ObjectPool<Projectile> ballPool;
    [SerializeField] private GameObject prefab;

    private void Start()
    {
        ballPool = new ObjectPool<Projectile>(
            () => OnCreate(),
            (Projectile obj) => OnAttack(obj),
            (Projectile obj) => OnLifeEnd(obj),
            (Projectile obj) => Destroy(obj)
        );
    }

    private Projectile OnCreate()
    {
        Projectile obj = Instantiate(prefab).GetComponent<Projectile>();
        obj.attackHandler = this;
        OnAttack(obj);
        return obj;
    }

    private void OnAttack(Projectile obj)
    {
        obj.transform.position = transform.position;
        obj.gameObject.SetActive(true);
        obj.GetComponent<TrailRenderer>().Clear();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        obj.GetComponent<Rigidbody2D>().AddForce(10 * (mousePos - (Vector2)transform.position).normalized, ForceMode2D.Impulse);
    }

    private void OnLifeEnd(Projectile obj)
    {
        obj.gameObject.SetActive(false);
    }
}
