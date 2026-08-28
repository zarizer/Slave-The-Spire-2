using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterMenuController : MonoBehaviour
{
    [SerializeField] GameObject CharacterCard;
    [SerializeField] GameObject View;
    HashSet<int> character_ids = new HashSet<int>();
    List<CharacterCard> cards = new List<CharacterCard>();
    public bool is_picking = false;
    static public List<CharacterCard> selected_characters = new List<CharacterCard>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        //LoadInventory();
        //AssignValueItems();
        selected_characters.Clear();
        LoadData();
    }

    private void OnDisable()
    {
        
    }

    public void OnSelect(CharacterCard card)
    {
        if (selected_characters.Contains(card))
        {
            card.UpdateSelection(0);
            selected_characters.Remove(card);
            CardsRenumerate();
        }
        else if (selected_characters.Count < 4)
        {
            selected_characters.Add(card);
            CardsRenumerate();
        }
        else
        {
            selected_characters[3].UpdateSelection(0);
            selected_characters.RemoveAt(3);
            selected_characters.Add(card);
            CardsRenumerate();
        }
    }

    void CardsRenumerate()
    {
        for (int i = 0; i<selected_characters.Count; i++)
        {
            selected_characters[i].UpdateSelection(i + 1);
        }
    } 

    public void LoadData()
    {
        StaticFuncs.DestroyChildren(View);
        cards.Clear();
        int i_ = 0;
        foreach (var i in ProfileManager.profile.CharacterIds)
        {
            character_ids.Add(i);
            var cur = Instantiate(CharacterCard, View.transform).GetComponent<CharacterCard>();
            cards.Add(cur);
            cur.character = GridCharacter.GetCharacterByID(i);
            cur.character.level = ProfileManager.profile.CharacterLevels[i_];
            cur.is_picking = is_picking;
            cur.menu = this;
            cur.UpdateInfo();
            i_++;
        }

        for(int i = 0; i<cards.Count; i++)
        {
            var t = cards[i].gameObject.GetComponent<RectTransform>();
            t.localPosition = new Vector3((150 + ((i) % 4) * 310), (-200 + (i/4)*410), 0);
            
        }
    }
}
