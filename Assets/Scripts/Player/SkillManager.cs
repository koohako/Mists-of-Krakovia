using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    [Header("Skills")]
    public List<BaseSkill> availableSkills = new List<BaseSkill>();
    
    [Header("Input")]
    public InputActionReference[] skillInputs = new InputActionReference[4];
    
    private Dictionary<BaseSkill, float> skillCooldowns = new Dictionary<BaseSkill, float>();
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        // Inicializar cooldowns
        foreach (var skill in availableSkills)
        {
            skillCooldowns[skill] = 0f;
        }
    }
    
    void Update()
    {
        // Atualizar cooldowns
        var keys = new List<BaseSkill>(skillCooldowns.Keys);
        foreach (var skill in keys)
        {
            if (skillCooldowns[skill] > 0)
                skillCooldowns[skill] -= Time.deltaTime;
        }

        // Verificar inputs
        for (int i = 0; i < skillInputs.Length && i < availableSkills.Count; i++)
        {
            if (skillInputs[i].action.WasPressedThisFrame())
            {
                UseSkill(i);
            }
        }
    }
    
    public void UseSkill(int skillIndex)
    {
        if (skillIndex >= availableSkills.Count) return;
        
        BaseSkill skill = availableSkills[skillIndex];
        
        if (CanUseSkill(skill))
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            skill.Execute(transform, mouseWorldPos);
            skillCooldowns[skill] = skill.cooldown;
        }
    }
    
    private bool CanUseSkill(BaseSkill skill)
    {
        return skillCooldowns[skill] <= 0f && skill.CanExecute(gameObject);
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;
        return mouseWorldPos;
    }
}