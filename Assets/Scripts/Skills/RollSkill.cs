using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Roll Skill", menuName = "Skills/Roll")]
public class RollSkill : BaseSkill
{

    [Header("Roll Settings")]
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    public override bool CanExecute(GameObject caster)
    {
        // Verifica se o jogador está no estado "Dashing" (correndo)
        return caster.GetComponent<PlayerController>().GetState().Equals(PlayerController.PlayerState.Moving);
    }

    public override void Execute(Transform caster, Vector3 targetPosition)
    {
        caster.GetComponent<PlayerController>().SetState(PlayerController.PlayerState.Dashing);
        var rb = caster.GetComponent<Rigidbody2D>();
        var runner = caster.GetComponent<MonoBehaviour>();
        Vector2 moveDirection = caster.GetComponent<PlayerController>().GetMoveDirection();
        runner.StartCoroutine(PerformDash(caster, moveDirection, rb));
    }

    public IEnumerator PerformDash(Transform transform, Vector2 moveDirection, Rigidbody2D rb)
    {

        // Posição inicial e final
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + (moveDirection * dashDistance);

        // Verifica se o dash não vai colidir com algo (opcional)
        // RaycastHit2D hit = Physics2D.Raycast(startPosition, moveDirection, dashDistance);
        // if (hit.collider != null)
        // {
        //     endPosition = hit.point - (moveDirection * 0.5f); // Para um pouco antes da colisão
        // }

        float elapsedTime = 0f;

        // Animação suave do dash
        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / dashDuration;

            // Curva de animação (começa rápido, desacelera no final)
            float easedProgress = 1 - Mathf.Pow(1 - progress, 3);

            Vector2 currentPosition = Vector2.Lerp(startPosition, endPosition, easedProgress);
            rb.MovePosition(currentPosition);

            yield return null;
        }

        // Garante que chegue na posição final
        rb.MovePosition(endPosition);
        transform.GetComponent<PlayerController>().SetState(PlayerController.PlayerState.Idle);

        // Cooldown
        yield return new WaitForSeconds(dashCooldown);
    }
    

}
