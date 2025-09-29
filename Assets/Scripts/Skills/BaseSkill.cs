using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Base Skill")]
public abstract class BaseSkill : ScriptableObject
{
    [Header("Skill Info")]
    public string skillName;
    public string description;
    public Sprite icon;
    public float cooldown = 1f;
    public float manaCost = 0f;
    
    public abstract void Execute(Transform caster, Vector3 targetPosition);
    public abstract bool CanExecute(GameObject caster);
}