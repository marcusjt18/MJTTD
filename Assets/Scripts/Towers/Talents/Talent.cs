using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Talent : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> Dependencies = new List<GameObject>();

    private Image talentImage;
    private Image borderImage;

    [SerializeField]
    private int id;

    private string name;
    private string description;

    private TowerUI parentUI;
    public bool IsActivated { get;  set; }
    public int Id { get => id; set => id = value; }
    public string Description { get => description; set => description = value; }
    public string Name { get => name; set => name = value; }

    private bool initialized = false;

    private Button talentButton;

    private LineDrawer lineDrawer;

    private void Start()
    { 

    }

    public void Initialize(LineDrawer ld)
    {
        if (initialized) return;

        lineDrawer = ld;

        parentUI = GetComponentInParent<TowerUI>();

        Image[] images = GetComponentsInChildren<Image>(true);

        foreach (Image image in images)
        {
            if (image.gameObject.name == "TalentImage")
            {
                talentImage = image;
            }
            else if (image.gameObject.name == "Border")
            {
                borderImage = image;
            }
        }

        talentButton = GetComponent<Button>();

        // Add a listener to the button's OnClick event
        talentButton.onClick.AddListener(() => Activate(parentUI.GetComponent<TowerUI>().CurrentTower));

        UpdateColor();

        initialized = true;
    }

    public bool CanActivate()
    {
        if (IsActivated)
            return false;

        if (Dependencies.Count == 0) // No dependencies, so can activate
            return true;

        foreach (var dependencyObject in Dependencies)
        {
            Talent dependency = dependencyObject.GetComponent<Talent>();
            if (dependency.IsActivated)
                return true; // If any dependency is activated, return true
        }

        return false; // If none of the dependencies are activated, return false
    }


    public void Activate(Tower tower)
    {
        if (!CanActivate() || tower.TalentPoints < 1)
            return;

        ApplyEffect(tower);

        tower.TalentPoints--;

        tower.AddTalentId(Id);

        IsActivated = true;

        UpdateColor();

        foreach (var dependency in Dependencies)
        {
            lineDrawer.UpdateLineThickness(GetComponent<RectTransform>(), dependency.GetComponent<RectTransform>(), true);
        }

        parentUI.UpdateInfoText(tower);
        parentUI.UpdateTalentColors();
    }

    public abstract void ApplyEffect(Tower tower);


    public void UpdateColor()
    {
        if (IsActivated)
        {
            talentImage.color = Color.white;
            borderImage.color = new Color(255 / 255f, 220 / 255f, 69 / 255f, 1);
        }
        else if (CanActivate())
        {
            talentImage.color = Color.gray;
            borderImage.color = Color.white;
        }
        else
        {
            talentImage.color = Color.gray;
            borderImage.color = Color.red;
        }
    }

}


