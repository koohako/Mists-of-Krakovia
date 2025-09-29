using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Skill", menuName = "Skills/Projectile Skill")]
public class ProjectileSkill : BaseSkill
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;
    public int projectileCount = 1;
    public float spreadAngle = 0f;
    
    public override void Execute(Transform caster, Vector3 targetPosition)
    {
        Transform shootPoint = caster.Find("ShootPoint") ?? caster;
        
        if (projectileCount == 1)
        {
            Vector2 direction = (targetPosition - shootPoint.position).normalized;
            var go = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            // Projétil único
            go.GetComponent<Projectile>().Init(direction);

        }
        else
        {
            // Múltiplos projéteis com spread
            if (projectileCount > 1)
            {
                for (int i = 0; i < projectileCount; i++)
                {
                    float angle = -spreadAngle / 2 + (spreadAngle / (projectileCount - 1)) * i;
                    Vector2 baseDir = (targetPosition - shootPoint.position).normalized;
                    Vector2 dir = Quaternion.Euler(0, 0, angle) * baseDir;
                    var go = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
                    go.GetComponent<Projectile>().Init(dir);
                }
            }
        }
    }
    
    public override bool CanExecute(GameObject caster)
    {
        // Verificar mana, cooldown, etc.
        return true;
    }
}