using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBar : NetworkBehaviour
{
    public Canvas canvas;
    public Image healthBar;
    public Image healthBarBackground;
    public Image healthBarEndInfill;
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public Image manaBar;
    public Image manaBarBackground;
    public Image manaBarEndInfill;
    public float maxMana = 50f;
    public float currentMana = 50f;

    // Cache para evitar operações desnecessárias
    float lastAppliedMaxHealth = -1f;
    float lastAppliedMaxMana = -1f;
    float lastHealth = -1f;
    float lastMana = -1f;

    float fullHealthBarWidth = 6.25f; // largura padrão da barra cheia
    float fullManaBarWidth = 6.25f; // largura padrão da barra cheia

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fullHealthBarWidth = healthBarEndInfill.rectTransform.rect.width;
        fullManaBarWidth = manaBarEndInfill.rectTransform.rect.width;

        if (!isLocalPlayer && canvas != null)
        {
            canvas.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return; // evita trabalho em instâncias remotas
        RefreshIfNeeded();
    }

    void RefreshIfNeeded()
    {
        // Evita divisões por zero
        if (maxHealth <= 0f) maxHealth = 1f;
        if (maxMana <= 0f) maxMana = 1f;

        // Clamp dos valores atuais
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);

        // Atualiza tamanhos só quando o máximo muda
        if (Mathf.Abs(maxHealth - lastAppliedMaxHealth) > 0.001f)
        {
            if (healthBarBackground != null)
                healthBarBackground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth);
            if (healthBar != null)
                healthBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth + fullManaBarWidth);
            lastAppliedMaxHealth = maxHealth;
            lastHealth = -1f; // força recalcular fill no mesmo frame
        }

        if (Mathf.Abs(maxMana - lastAppliedMaxMana) > 0.001f)
        {
            if (manaBarBackground != null)
                manaBarBackground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxMana);
            if (manaBar != null)
                manaBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxMana + fullManaBarWidth);
            lastAppliedMaxMana = maxMana;
            lastMana = -1f; // força recalcular fill no mesmo frame
        }

        // Atualiza fill apenas quando muda
        if (Mathf.Abs(currentHealth - lastHealth) > 0.001f && healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
            lastHealth = currentHealth;
        }

        if (Mathf.Abs(currentMana - lastMana) > 0.001f && manaBar != null)
        {
            manaBar.fillAmount = currentMana / maxMana;
            lastMana = currentMana;
        }
    }

    // Métodos para outro script injetar valores reais
    public void SetHealth(float current, float max)
    {
        maxHealth = max;
        currentHealth = current;
        RefreshIfNeeded();
    }

    public void SetMana(float current, float max)
    {
        maxMana = max;
        currentMana = current;
        RefreshIfNeeded();
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        // Atualiza visual no Editor sem precisar dar Play
        RefreshIfNeeded();
    }
#endif
}
