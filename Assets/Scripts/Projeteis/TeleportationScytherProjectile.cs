using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportationScytherProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 5f;

    private Vector2 direction;
    private bool returning = false;
    private Transform target;
    private Transform caster;

    public Action<TeleportationScytherProjectile> OnFinished; // Evento para a skill

    public void Initialize(Transform casterTransform)
    {
        target = casterTransform;
    }

    void Start()
    {
        SetDirection();
        StartCoroutine(BoomerangRoutine());
    }

    private IEnumerator BoomerangRoutine()
    {
        float distanceTraveled = 0f;

        // Fase 1: ida
        while (distanceTraveled < maxDistance)
        {
            float step = speed * Time.deltaTime;
            transform.position += (Vector3)direction * step;
            distanceTraveled += step;
            yield return null;
        }

        // Fase 2: retorno
        returning = true;
        while (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) <= 0.05f)
                break;

            yield return null;
        }

        Finalizar();
    }

    public void ForceFinish() // Caso teleporte antes do retorno natural
    {
        Finalizar();
    }

    private void Finalizar()
    {
        OnFinished?.Invoke(this);
        Destroy(gameObject);
    }

    private void SetDirection()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        direction = (mouseWorldPosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}