using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUIManager : MonoBehaviour
{
    public static SkillsUIManager GetInstance() { return instance; }
    public static SkillsUIManager instance;

    [Header("Skills Sprites")]
    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private GameObject[] skillsPrefab;
    private float skillUseTime = 0.2f;
    private bool isUsing = false;
    List<Skill> currentSkills;

    [Header("Debug")]
    [SerializeField] private List<GameObject> placeholderComponents;
    [SerializeField] private List<GameObject> skillComponents;

    private bool skillUsed = false;

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
        if (skillsPanel == null) return;
        foreach(Transform child in skillsPanel.transform){
            placeholderComponents.Add(child.gameObject);
        }
        currentSkills = SkillsManager.instance.getAvailableSkills();
        
        createPlaceHolders();
        populatePlaceHolders();
    }

    void createPlaceHolders(){
        GameObject ph = skillsPanel.transform.GetChild(0).gameObject;
        int phAmt = 1;
        while(placeholderComponents.Count < currentSkills.Count){
            GameObject newPH = Instantiate(ph, skillsPanel.transform);
            phAmt += 1;
            newPH.name = ph.name + phAmt;
            placeholderComponents.Add(newPH);
        }
        ph.name = "Skill1";
    }
    void populatePlaceHolders(){
        for(int i = 0; i < currentSkills.Count; i++){
            GameObject ph = placeholderComponents[i];
            foreach(GameObject prefab in skillsPrefab){
                if(prefab.name == currentSkills[i].name){
                    ph.SetActive(true);
                    GameObject skillUIObj = Instantiate(prefab, ph.transform);
                    skillUIObj.name = prefab.name;
                    skillComponents.Add(skillUIObj);
                }
            }
        }
    }

    public void updateVisualUsed(){
        skillUsed = true;
    }

    public void updateSkillUI(){
        if(skillUsed){
            StartCoroutine(playUseSkill());
            skillUsed = false;
        }
        else if(!isUsing){
            StartCoroutine(checkForUIUpdate());
        }
    }

    void Update()
    {
        updateSkillUI();
    }

    IEnumerator playUseSkill(){
        isUsing = true;
        for(int i = 0; i < skillComponents.Count; i++){
            if(skillComponents[i].name == SkillsManager.instance.playerCurrentSkill.name){
                Image img = skillComponents[i].GetComponent<Image>();
                List<Sprite> sprites = skillComponents[i].GetComponent<SkillImageStorage>().playSprites;
                if(img != null){
                    foreach(Sprite s in sprites){
                        img.sprite = s;
                        yield return new WaitForSeconds(skillUseTime/sprites.Count);
                    }
                }
            }
        }
        isUsing = false;
        yield return null;
    }

    IEnumerator checkForUIUpdate(){
        for(int i = 0; i < skillComponents.Count; i++){
            Image img = skillComponents[i].GetComponent<Image>();
            if(img != null){
                foreach(Skill s in SkillsManager.instance.playerSkills){
                    if(s.name == skillComponents[i].name){
                        List<Sprite> sprites = skillComponents[i].GetComponent<SkillImageStorage>().progressSprites;
                        if(DataManager.instance.booksAmount < s.cost){
                            img.sprite = sprites[DataManager.instance.booksAmount];
                        }
                        else{
                            int last = sprites.Count-1;
                            img.sprite = sprites[last];
                        }
                    }
                }
            }
            if(SkillsManager.instance.playerCurrentSkill.name == skillComponents[i].name){
                placeholderComponents[i].GetComponent<Image>().color = Color.white;
            }
            else{
                placeholderComponents[i].GetComponent<Image>().color = new Color(0,0,0,0);
            }
        }
        yield return null;
    }

    // IEnumerator PlayAnimUI()
    // {
    //     foreach(GameObject skillPref in skillComponents)
    //     {
    //         var image = skillPref.GetComponent<Image>();

    //         if (DataManager.instance.booksAmount == SkillsManager.instance.playerCurrentSkill.cost)
    //         {
    //             StartCoroutine(PlayUseSkill(image));
    //         }
    //         else
    //         {
    //             if(DataManager.instance.booksAmount >= SkillsManager.instance.playerCurrentSkill.cost){
    //                 image.sprite = skillsSprites[]
    //             }
    //         }
    //     }

    //     yield return new WaitForEndOfFrame();
    // }

    // IEnumerator PlayUseSkill(Image m_Image)
    // {
    //     int index = 0;
    //     foreach (var skillImg in skillAvailableSprite)
    //     {
    //         m_Image.sprite = skillImg;

    //         index++;
    //         yield return new WaitForSeconds(m_Speed);
    //     }

    //     m_Image.sprite = skillSprites[skillSprites.Length - 1];
    // }
}
