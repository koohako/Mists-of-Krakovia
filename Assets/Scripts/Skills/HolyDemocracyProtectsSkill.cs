using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Holy Democracy Protects", menuName = "Skills/Holy Democracy Protects")]
public class HolyDemocracyProtectsSkill : BaseSkill
{
    public override bool CanExecute(GameObject caster)
    {
        return !caster.GetComponent<PlayerController>().GetState().Equals(PlayerController.PlayerState.Dashing);
    }

    public override void Execute(Transform caster, Vector3 targetPosition)
    {
        
        var runner = caster.GetComponent<MonoBehaviour>();
        runner.StartCoroutine(PerformHolyDemocracyProtects(caster));
    }

    IEnumerator PerformHolyDemocracyProtects(Transform transform)
    {
        transform.GetComponent<PlayerController>().SetState(PlayerController.PlayerState.Dashing);
        SpriteRenderer sprite = transform.GetComponent<SpriteRenderer>();
        sprite.color = new Color(1f, 1f, 1f, 0.5f); // Torna o sprite semi-transparente
        Collider2D collider = transform.GetComponent<Collider2D>();
        collider.enabled = false;
        yield return new WaitForSeconds(2f);
        collider.enabled = true;
        sprite.color = new Color(1f, 1f, 1f, 1f); // Restaura a opacidade original
        transform.GetComponent<PlayerController>().SetState(PlayerController.PlayerState.Idle);

        yield return null;
    }
}
