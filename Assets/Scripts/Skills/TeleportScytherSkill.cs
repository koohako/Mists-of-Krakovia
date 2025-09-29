using UnityEngine;

[CreateAssetMenu(fileName = "New Teleport Scyther Skill", menuName = "Skills/Teleport Scyther")]
public class TeleportScytherSkill : BaseSkill
{
    [Header("Teleport Scyther")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float maxDistance = 6f;

    private TeleportationScytherProjectile activeProjectile;
    private float originalCooldown;
    private bool awaitingTeleport = false;

    private void OnEnable()
    {
        originalCooldown = cooldown;
    }

    public override void Execute(Transform caster, Vector3 targetPosition)
    {
        // Segundo uso: teleporta
        if (awaitingTeleport && activeProjectile != null)
        {
            caster.position = activeProjectile.transform.position;
            activeProjectile.ForceFinish();
            activeProjectile = null;
            awaitingTeleport = false;
            cooldown = originalCooldown; // restaura cooldown para este ciclo
            return;
        }

        // Primeiro uso: lança
        if (activeProjectile == null)
        {
            cooldown = 0f; // evita iniciar cooldown agora
            SpawnProjectile(caster);
        }
    }

    private void SpawnProjectile(Transform caster)
    {
        Transform shootPoint = caster.Find("ShootPoint") ?? caster;
        var go = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        var proj = go.GetComponent<TeleportationScytherProjectile>();

        if (proj == null)
        {
            Debug.LogWarning("Prefab não possui TeleportationScytherProjectile.");
            return;
        }

        // Ajustar parâmetros dinamicamente se quiser
        proj.speed = projectileSpeed;
        proj.maxDistance = maxDistance;

        proj.Initialize(caster);
        proj.OnFinished += HandleProjectileFinished;

        activeProjectile = proj;
        awaitingTeleport = true;
    }

    private void HandleProjectileFinished(TeleportationScytherProjectile proj)
    {
        if (activeProjectile == proj)
        {
            activeProjectile = null;
            awaitingTeleport = false;
            cooldown = originalCooldown; // garante que próximo uso terá cooldown normal
        }
    }

    public override bool CanExecute(GameObject caster)
    {
        // Se está esperando teleporte, permitir SEM cooldown
        if (awaitingTeleport && activeProjectile != null)
            return true;

        // Caso normal (verificar mana etc.)
        return true;
    }
}
