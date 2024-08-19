using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUIManager : MonoBehaviour
{
    public static SkillsUIManager GetInstance() { return instance; }
    public static SkillsUIManager instance;

    [Header("Skills Sprites")]
    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject[] skillsPrefab;
    [SerializeField] private Sprite[] skillSprites;

    [Header("Skill Available Sprite")]
    [SerializeField] private Sprite[] skillAvailableSprite;
    public float m_Speed = 1f;

    List<Skill> currentSkills;

    [Header("Debug")]
    [SerializeField] private List<GameObject> placeholderComponents;
    [SerializeField] private List<GameObject> skillComponents;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    void Start()
    {
        currentSkills = SkillsManager.instance.getAvailableSkills();
        CheckSkills();
    }

    void CheckSkills()
    {
        int index = 0;
        foreach (Skill skill in currentSkills)
        {
            AddSkillPrefab(skill.name, index);
            index++;
        }
    }

    void AddSkillPrefab(string skills, int index)
    {
        var placeholder = placeholderComponents[index];

        foreach(GameObject skillPrefab in skillsPrefab)
        {
            if (skillPrefab.name == skills)
            {
                placeholder.transform.gameObject.SetActive(true);
                GameObject skillObj = Instantiate(skillsPrefab[index], placeholder.transform);
                skillComponents.Add(skillObj);
            }
        }
    }

    public void UpdateVisualAbility()
    {
        StartCoroutine(PlayAnimUI());
    }



    void Update()
    {
        
    }

    IEnumerator PlayAnimUI()
    {
        foreach(GameObject skillPref in skillComponents)
        {
            var image = skillPref.GetComponent<Image>();

            if (DataManager.instance.booksAmount == SkillsManager.instance.playerCurrentSkill.cost)
            {
                StartCoroutine(PlayUseSkill(image));
            }
            else
            {
                // Add a checker for different skills
                image.sprite = skillSprites[DataManager.instance.booksAmount];
            }
        }

        yield return new WaitForEndOfFrame();
    }

    IEnumerator PlayUseSkill(Image m_Image)
    {
        int index = 0;
        foreach (var skillImg in skillAvailableSprite)
        {
            m_Image.sprite = skillImg;

            index++;
            yield return new WaitForSeconds(m_Speed);
        }

        m_Image.sprite = skillSprites[skillSprites.Length - 1];
    }
}
