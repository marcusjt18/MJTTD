using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Talent : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> Dependencies = new List<GameObject>();

    [SerializeField]
    private Image talentImage;
    [SerializeField]
    private Image borderImage;

    [SerializeField]
    private int id;

    [SerializeField]
    private GameObject parentUI;
    public bool IsActivated { get;  set; }
    public int Id { get => id; set => id = value; }

    private Button talentButton;
    private void Start()
    {
        talentButton = GetComponent<Button>();

        // Ensure there is a Button component attached
        if (talentButton == null)
        {
            Debug.LogError("No Button component attached to Talent object.");
            return;
        }

        // Add a listener to the button's OnClick event
        talentButton.onClick.AddListener(() => Activate(parentUI.GetComponent<TowerUI>().CurrentTower));
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

        parentUI.GetComponent<TowerUI>().UpdateInfoText(tower);
    }

    public abstract void ApplyEffect(Tower tower);


    public void UpdateColor()
    {
        if (IsActivated)
        {
            talentImage.color = Color.white;
            borderImage.color = Color.white;
        }
        else if (CanActivate())
        {
            talentImage.color = Color.gray;
            borderImage.color = Color.green;
        }
        else
        {
            talentImage.color = Color.gray;
            borderImage.color = Color.red;
        }
    }

}


