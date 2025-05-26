using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    [Header("UI")]
    public Text woodText;
    public Text rockText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // 모든 enum 값을 Dictionary에 초기화
            foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 0;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
        UpdateUI();
    }

    public bool ConsumeResource(ResourceType type, int amount)
    {
        if (resources[type] >= amount)
        {
            resources[type] -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (woodText != null)
            woodText.text = $"Wood: {resources[ResourceType.Wood]}";
        if (rockText != null)
            rockText.text = $"Rock: {resources[ResourceType.Rock]}";
    }
}