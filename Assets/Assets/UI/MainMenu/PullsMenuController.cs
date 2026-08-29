using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PullsMenuController : MonoBehaviour
{
    public Banner cur_banner;

    public CharacterCard MainTarget;
    public CharacterCard SecondaryTarget1;
    public CharacterCard SecondaryTarget2;
    [SerializeField] TextMeshProUGUI BudgetValue;
    [SerializeField] List<Transform> ResultPositions;
    [SerializeField] GameObject DarkBackGround;
    [SerializeField] GameObject ResultTemplate;
    [SerializeField] LootGenerator lootManager;
    List<GameObject> results = new List<GameObject>();
    Coroutine cur_pulling;
    [SerializeField] TextMeshProUGUI pull_timer;
    void Start()
    {
        
    }

    public void FixedUpdate()
    {
        int time = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() % 3600;
        string minutes = (60 - time / 60 - 1).ToString();
        if (minutes.Length == 1) { minutes = "0" + minutes; }
        string seconds = (60 - time % 60 - 1 ).ToString();
        if (seconds.Length == 1) { seconds = "0" + seconds; }
        pull_timer.text = minutes+":"+ seconds;
    }

    public void DisableDarkBackground()
    {
        DarkBackGround.SetActive(false);
        foreach(var i in results)
        {
            StaticFuncs.DestroySingle(i.transform);
        }
        results.Clear();
        StopCoroutine(cur_pulling);
    }

    public void OnEnable()
    {
        cur_banner = GenerateBanner();
        BudgetValue.text = ProfileManager.profile.budget.ToString();
        CreateBanner();



    }

    void Update()
    {
        
    }


    public IEnumerator Pull1()
    {
        if (ProfileManager.profile.budget < 10) { ResoursesDict.GetClass<SoundMain>().Restrict(); yield return new WaitForSeconds(0); ; }
        DarkBackGround.SetActive(true);
        ProfileManager.profile.budget -= 10;
        var res = Instantiate(ResultTemplate, ResultPositions[7]);
        Loot loot = lootManager.GenerateGachaLoot(1)[0];
        if (loot.item == Item.character) { loot.character = cur_banner.GetCharacter(); }
        var l = new List<Loot>();
        l.Add(loot);
        GetLoot(l);
        yield return new WaitForSeconds(0.3f);
        res.GetComponent<PullResult>().loot = loot;
        res.GetComponent<PullResult>().pull_manager = this;
        res.GetComponent<PullResult>().UpdateData();
        results.Add(res);
        yield return new WaitForSeconds(0);

    }

    public IEnumerator Pull10()
    {
        if (ProfileManager.profile.budget < 100) { ResoursesDict.GetClass<SoundMain>().Restrict(); yield return new WaitForSeconds(0); ; }
        DarkBackGround.SetActive(true);
        ProfileManager.profile.budget -= 100;
        List<Loot> loot = lootManager.GenerateGachaLoot(10);
        foreach (Loot l in loot)
        {
            if (l.item == Item.character) { l.character = cur_banner.GetCharacter(); }
        }
        loot = SortLoot(loot);
        GetLoot(loot);

        for(int i = 0; i<10; i++)
        {
            yield return new WaitForSeconds(i/10f + 0.1f);
            Debug.Log(loot[i].item);
            var res = Instantiate(ResultTemplate, ResultPositions[i]);
            res.GetComponent<PullResult>().loot = loot[i];
            res.GetComponent<PullResult>().pull_manager = this;
            res.GetComponent<PullResult>().UpdateData();
            results.Add(res);
        }
        yield return new WaitForSeconds(0);
    }

    void GetLoot(List<Loot> loot)
    {
        foreach(Loot l in loot)
        {
            if (l.item != Item.character)
            {
                InventoryManager.AddItem(l.item, "", "", l.count);
            }
            else
            {
                if (CheckCharacter(l.character.id)) ProfileManager.profile.budget += 25;
                else
                {
                    ProfileManager.profile.CharacterIds.Add(l.character.id);
                    ProfileManager.profile.CharacterLevels.Add(1);
                }
            }
        }
        ProfileManager.SaveProfile();
        InventoryManager.SaveInventory();
        BudgetValue.text = ProfileManager.profile.budget.ToString();
    }

    bool CheckCharacter(int id)
    {
        foreach (int i in ProfileManager.profile.CharacterIds)
        {
            if (i == id)
            {
                return true;
            }
        }
        return false;
    }
    public void OnPull1()
    {
        cur_pulling = StartCoroutine(Pull1());
    }
    public void OnPull10()
    {
        cur_pulling = StartCoroutine(Pull10());
    }

    void CreateBanner()
    {
        MainTarget.character = cur_banner.MainTarget;
        MainTarget.UpdateInfo();
        SecondaryTarget1.character = cur_banner.SecondaryTarget1;
        SecondaryTarget1.UpdateInfo();
        SecondaryTarget2.character = cur_banner.SecondaryTarget2;
        SecondaryTarget2.UpdateInfo();
    }

    public Banner GenerateBanner()
    {
        Banner banner = new Banner();

        long hours = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 3600;
        var rand = new System.Random((int)hours);
        int count = DataDicts.CharacterTypes.Count;
        if (count >= 3)
        {
            int id1 = rand.Next(count);

            int id2 = rand.Next(count);
            while (id2 == id1)
            {
                id2 = rand.Next(count);
            }

            int id3 = rand.Next(count);
            while (id3 == id1 || id3 == id2)
            {
                id3 = rand.Next(count);
            }

            banner.MainTarget = GridCharacter.GetCharacterByID(id1);
            banner.SecondaryTarget1 = GridCharacter.GetCharacterByID(id2);
            banner.SecondaryTarget2 = GridCharacter.GetCharacterByID(id3);
        }
        else
        {
            banner.MainTarget = GridCharacter.GetCharacterByID(0);
            banner.SecondaryTarget1 = GridCharacter.GetCharacterByID(0);
            banner.SecondaryTarget2 = GridCharacter.GetCharacterByID(0);
        }
        return banner;
    }

    List<Loot> SortLoot(List<Loot> loots)
    {
        List<Loot> res = loots.OrderBy(loot => item_weights[loot.item]*loot.count).ToList();

        return res;
    }

    Dictionary<Item, int> item_weights = new Dictionary<Item, int>{
        {Item.xp_ticket, 1},
        {Item.stick1, 10 },
        {Item.stick2, 100 },
        {Item.stick3, 1000 },
        {Item.asphalt1, 31 },
        {Item.asphalt2, 301 },
        {Item.asphalt3, 3001 },
        {Item.mat1, 94 },
        {Item.mat2, 904 },
        {Item.mat3, 9004 },
        {Item.character, 100000000}
        };
}

public class Banner
{
    public CharacterBase MainTarget;
    public CharacterBase SecondaryTarget1;
    public CharacterBase SecondaryTarget2;

    public CharacterBase GetCharacter()
    {
        if (StaticFuncs.RandomRangeInclusive(1, 3) == 1)
        {
            if (StaticFuncs.RandomRangeInclusive(1, 2) == 1)
            {
                return SecondaryTarget1;
            }
            else
            {
                return SecondaryTarget2;
            }
        }
        else
        {
            return MainTarget;
        }
    }


}