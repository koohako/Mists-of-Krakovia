using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager : NetworkBehaviour
{
    public static CooldownManager Instance { get; private set; }
    Dictionary<NetworkIdentity, Dictionary<BaseSkill, float>> cooldowns = new Dictionary<NetworkIdentity, Dictionary<BaseSkill, float>>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void Update()
    {
        if (!isServer) return;
        foreach (var playerEntry in cooldowns)
        {
            var player = playerEntry.Key;
            var skillDict = playerEntry.Value;
            List<BaseSkill> keys = new List<BaseSkill>(skillDict.Keys);
            foreach (var skill in keys)
            {
                if (skillDict[skill] > 0f)
                {
                    skillDict[skill] -= Time.deltaTime;
                    if (skillDict[skill] < 0f)
                        skillDict[skill] = 0f;
                }
            }
        }
    }

    public void AddPlayer(NetworkIdentity player)
    {
        if (!cooldowns.ContainsKey(player))
            cooldowns[player] = new Dictionary<BaseSkill, float>();

        var skillManager = player.GetComponent<SkillManager>();

        if (skillManager != null)
        {
            foreach (var skill in skillManager.availableSkills)
            {
                if (!cooldowns[player].ContainsKey(skill))
                    cooldowns[player][skill] = 0f;
            }
        }
    }

    public void RemovePlayer(NetworkIdentity player)
    {
        if (cooldowns.ContainsKey(player))
            cooldowns.Remove(player);
    }

    public float GetCooldown(NetworkIdentity player, BaseSkill skill)
    {
        if (cooldowns.ContainsKey(player) && cooldowns[player].ContainsKey(skill))
            return cooldowns[player][skill];

        return -1f; // Indica que o jogador ou a habilidade não existe
    }
    
    public void SetCooldown(NetworkIdentity player, BaseSkill skill, float cooldown)
    {
        if (cooldowns.ContainsKey(player))
        {
            if (cooldowns[player].ContainsKey(skill))
                cooldowns[player][skill] = cooldown;
            else
                cooldowns[player][skill] = cooldown; // Adiciona a habilidade se não existir
        }
    }

    public bool CanUseSkill(NetworkIdentity player, BaseSkill skill)
    {
        if (cooldowns.ContainsKey(player) && cooldowns[player].ContainsKey(skill))
            return cooldowns[player][skill] == 0f;
        return false; // Jogador ou habilidade não existe
    }
}