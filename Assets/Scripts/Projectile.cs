using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public AttackHandler attackHandler;

    private void OnCollisionEnter2D(Collision2D other)
    {
        attackHandler.ballPool.Release(this);
    }

    private IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(5);
        attackHandler.ballPool.Release(this);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForEnd());
    }
}