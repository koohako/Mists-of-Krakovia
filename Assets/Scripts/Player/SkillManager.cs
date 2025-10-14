using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : NetworkBehaviour
{
    [Header("Skills")]
    public List<BaseSkill> availableSkills = new List<BaseSkill>();
    
    [Header("Input")]
    public InputActionReference[] skillInputs = new InputActionReference[4];
    
    private Camera mainCamera;

    private CooldownManager cooldownManager;

    void Start()
    {
        cooldownManager = CooldownManager.Instance;
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        if (!isLocalPlayer) return;
        // Verificar inputs
        for (int i = 0; i < skillInputs.Length && i < availableSkills.Count; i++)
        {
            if (skillInputs[i].action.WasPressedThisFrame())
            {
                CmdUseSkill(i);
            }
        }
    }

    [Command]
    public void CmdUseSkill(int skillIndex)
    {
        if (skillIndex >= availableSkills.Count) return;
        
        BaseSkill skill = availableSkills[skillIndex];
        
        if (cooldownManager.CanUseSkill(netIdentity, skill))
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            skill.Execute(transform, mouseWorldPos);
            cooldownManager.SetCooldown(netIdentity, skill, skill.cooldown);
        }
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;
        return mouseWorldPos;
    }
}