using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static InventoryManager;
using static UnityEditor.Progress;

public class LootGenerator : MonoBehaviour
{
    LootTables tables = new LootTables();
    Dictionary<int, Pool> Pools = null;
    public List<Loot> GenerateLoot(BattleMain data, int type)
    {
        if (Pools == null)
        {
            Init();
        }
        List<Loot> loot = new List<Loot>();


        int k = 1;

        while (StaticFuncs.RandomRangeInclusive(1, 10) < 4)
        {
            k++;
        }

        for (int i = 0; i < k; i++)
        {
            loot.Add(Pools[1000 * type + data.chapter_num * 10 + data.difficulty].GetLoot());
        }


        return loot;
    }

    

    public void Init()
    {
        Pools = new Dictionary<int, Pool>
        {
            {1001, tables.Forest1 },
            {1002, tables.Forest2 },
            {1003, tables.Forest3 },
            {1004, tables.Forest4 },
            {1005, tables.Forest5 },

            {1011, tables.City1 },
            {1012, tables.City2 },
            {1013, tables.City3 },
            {1014, tables.City4 },
            {1015, tables.City5 },

            {1021, tables.Kach1 },
            {1022, tables.Kach2 },
            {1023, tables.Kach3 },
            {1024, tables.Kach4 },
            {1025, tables.Kach5 },

        };

    }


}

public class Loot 
{
    public Item item;
    public int count;

    public Loot(Loot other)
    {
        item = other.item;
        count = other.count;
    }
    public Loot(Item item_, int count_)
    {
        item = item_;
        count = count_;
    }
}

public class Pool
{
    public Dictionary<Loot, int> pool = new Dictionary<Loot, int>();
    public List<Loot> weighted_loot = null;

    public Pool(Dictionary<Loot, int> dict)
    {
        pool = dict;
    }
    public Loot GetLoot()
    {
        CreateWeightedLoot();
        return weighted_loot[StaticFuncs.RandomRangeInclusive(0, weighted_loot.Count - 1)];
    }
    public Pool(Pool other)
    {
        pool = other.pool;
    }

    public void CreateWeightedLoot()
    {
        if (weighted_loot != null) return;
        weighted_loot = new List<Loot>();
        foreach(Loot l in pool.Keys)
        {
            for (int i = 0; i < l.count; i++)
            {
                weighted_loot.Add(new Loot(l));
            }
        }
    }

    public static Pool operator +(Pool p1, Pool p2)
    {
        Pool p3 = new Pool(p1);
        foreach (var p in p2.pool)
        {
            p3.pool.Add(p.Key, p.Value);
        }
        return p3;
    }
}

class LootTables
{
    public Pool XPTickets11 = new Pool
    (
    new Dictionary<Loot, int>
    {
        { new Loot(Item.xp_ticket, 1), 9 },
        { new Loot(Item.xp_ticket, 2), 7 },
        { new Loot(Item.xp_ticket, 3), 5 },
        { new Loot(Item.xp_ticket, 4), 3 },
        { new Loot(Item.xp_ticket, 5), 1 },  
    }
    );

    public Pool XPTickets12 = new Pool
    (
    new Dictionary<Loot, int>
    {
        { new Loot(Item.xp_ticket, 2), 9 },
        { new Loot(Item.xp_ticket, 3), 7 },
        { new Loot(Item.xp_ticket, 4), 5 },
        { new Loot(Item.xp_ticket, 5), 3 },
        { new Loot(Item.xp_ticket, 6), 1 },
    }
    );

    public Pool XPTickets13 = new Pool
    (
    new Dictionary<Loot, int>
    {
        { new Loot(Item.xp_ticket, 3), 9 },
        { new Loot(Item.xp_ticket, 4), 7 },
        { new Loot(Item.xp_ticket, 5), 5 },
        { new Loot(Item.xp_ticket, 6), 3 },
        { new Loot(Item.xp_ticket, 7), 1 },
    }
    );

    public Pool XPTickets31 = new Pool
    (
    new Dictionary<Loot, int>
    {
        { new Loot(Item.xp_ticket, 2), 9 },
        { new Loot(Item.xp_ticket, 3), 7 },
        { new Loot(Item.xp_ticket, 4), 5 },
        { new Loot(Item.xp_ticket, 5), 3 },
        { new Loot(Item.xp_ticket, 6), 1 },
    }
    );

    public Pool XPTickets32 = new Pool
    (
    new Dictionary<Loot, int>
    {
        { new Loot(Item.xp_ticket, 3), 9 },
        { new Loot(Item.xp_ticket, 4), 7 },
        { new Loot(Item.xp_ticket, 5), 5 },
        { new Loot(Item.xp_ticket, 6), 3 },
        { new Loot(Item.xp_ticket, 7), 1 },
    }
    );

    public Pool XPTickets33 = new Pool
    (
    new Dictionary<Loot, int>
    {
        { new Loot(Item.xp_ticket, 4), 9 },
        { new Loot(Item.xp_ticket, 5), 7 },
        { new Loot(Item.xp_ticket, 6), 5 },
        { new Loot(Item.xp_ticket, 7), 3 },
        { new Loot(Item.xp_ticket, 8), 1 },
    }
    );

    public Pool XPTickets51 = new Pool
    (
    new Dictionary<Loot, int>
    {
        { new Loot(Item.xp_ticket, 4), 9 },
        { new Loot(Item.xp_ticket, 6), 7 },
        { new Loot(Item.xp_ticket, 8), 5 },
        { new Loot(Item.xp_ticket, 10), 3 },
        { new Loot(Item.xp_ticket, 12), 1 },
    }
    );

    public Pool XPTickets52 = new Pool
    (
    new Dictionary<Loot, int>
    {
        { new Loot(Item.xp_ticket, 6), 9 },
        { new Loot(Item.xp_ticket, 8), 7 },
        { new Loot(Item.xp_ticket, 10), 5 },
        { new Loot(Item.xp_ticket, 12), 3 },
        { new Loot(Item.xp_ticket, 14), 1 },
    }
    );

    public Pool XPTickets53 = new Pool
    (
    new Dictionary<Loot, int>
    {
        { new Loot(Item.xp_ticket, 8), 9 },
        { new Loot(Item.xp_ticket, 10), 7 },
        { new Loot(Item.xp_ticket, 12), 5 },
        { new Loot(Item.xp_ticket, 14), 3 },
        { new Loot(Item.xp_ticket, 16), 1 },
    }
    );

    public Pool Forest1 = new Pool
   (
   new Dictionary<Loot, int>
   {
        { new Loot(Item.stick1, 1), 6 },
        { new Loot(Item.stick1, 2), 4 },
        { new Loot(Item.stick1, 3), 2 },
   }
   );

    public Pool Forest2 = new Pool
  (
  new Dictionary<Loot, int>
  {
        { new Loot(Item.stick1, 2), 5 },
        { new Loot(Item.stick1, 3), 3 },
        { new Loot(Item.stick1, 4), 1 },
        { new Loot(Item.stick2, 2), 4 },
        { new Loot(Item.stick2, 3), 2 },
  }
  );

    public Pool Forest3 = new Pool
  (
  new Dictionary<Loot, int>
  {
        { new Loot(Item.stick2, 1), 6 },
        { new Loot(Item.stick2, 2), 4 },
        { new Loot(Item.stick2, 3), 2 },
  }
  );

    public Pool Forest4 = new Pool
  (
  new Dictionary<Loot, int>
  {
        { new Loot(Item.stick2, 2), 5 },
        { new Loot(Item.stick2, 3), 3 },
        { new Loot(Item.stick2, 4), 1 },
        { new Loot(Item.stick3, 2), 4 },
        { new Loot(Item.stick3, 3), 2 },
  }
  );

    public Pool Forest5 = new Pool
(
new Dictionary<Loot, int>
{
        { new Loot(Item.stick3, 1), 6 },
        { new Loot(Item.stick3, 2), 4 },
        { new Loot(Item.stick3, 3), 2 },
}
);

    public Pool City1 = new Pool
   (
   new Dictionary<Loot, int>
   {
        { new Loot(Item.asphalt1, 1), 6 },
        { new Loot(Item.asphalt1, 2), 4 },
        { new Loot(Item.asphalt1, 3), 2 },
   }
   );

    public Pool City2 = new Pool
  (
  new Dictionary<Loot, int>
  {
        { new Loot(Item.asphalt1, 2), 5 },
        { new Loot(Item.asphalt1, 3), 3 },
        { new Loot(Item.asphalt1, 4), 1 },
        { new Loot(Item.asphalt2, 2), 4 },
        { new Loot(Item.asphalt2, 3), 2 },
  }
  );

    public Pool City3 = new Pool
  (
  new Dictionary<Loot, int>
  {
        { new Loot(Item.asphalt2, 1), 6 },
        { new Loot(Item.asphalt2, 2), 4 },
        { new Loot(Item.asphalt2, 3), 2 },
  }
  );

    public Pool City4 = new Pool
  (
  new Dictionary<Loot, int>
  {
        { new Loot(Item.asphalt2, 2), 5 },
        { new Loot(Item.asphalt2, 3), 3 },
        { new Loot(Item.asphalt2, 4), 1 },
        { new Loot(Item.asphalt3, 2), 4 },
        { new Loot(Item.asphalt3, 3), 2 },
  }
  );

    public Pool City5 = new Pool
(
new Dictionary<Loot, int>
{
        { new Loot(Item.asphalt3, 1), 6 },
        { new Loot(Item.asphalt3, 2), 4 },
        { new Loot(Item.asphalt3, 3), 2 },
}
);

    public Pool Kach1 = new Pool
(
new Dictionary<Loot, int>
{
        { new Loot(Item.mat1, 1), 6 },
        { new Loot(Item.mat1, 2), 4 },
        { new Loot(Item.mat1, 3), 2 },
}
);

    public Pool Kach2 = new Pool
  (
  new Dictionary<Loot, int>
  {
        { new Loot(Item.mat1, 2), 5 },
        { new Loot(Item.mat1, 3), 3 },
        { new Loot(Item.mat1, 4), 1 },
        { new Loot(Item.mat2, 2), 4 },
        { new Loot(Item.mat2, 3), 2 },
  }
  );

    public Pool Kach3 = new Pool
  (
  new Dictionary<Loot, int>
  {
        { new Loot(Item.mat2, 1), 6 },
        { new Loot(Item.mat2, 2), 4 },
        { new Loot(Item.mat2, 3), 2 },
  }
  );

    public Pool Kach4 = new Pool
  (
  new Dictionary<Loot, int>
  {
        { new Loot(Item.mat2, 2), 5 },
        { new Loot(Item.mat2, 3), 3 },
        { new Loot(Item.mat2, 4), 1 },
        { new Loot(Item.mat3, 2), 4 },
        { new Loot(Item.mat3, 3), 2 },
  }
  );

    public Pool Kach5 = new Pool
(
new Dictionary<Loot, int>
{
        { new Loot(Item.mat3, 1), 6 },
        { new Loot(Item.mat3, 2), 4 },
        { new Loot(Item.mat3, 3), 2 },
}
);

    public LootTables()
    {
        Forest1 += XPTickets11;
        Forest2 += XPTickets11;
        Forest3 += XPTickets31;
        Forest4 += XPTickets31;
        Forest5 += XPTickets51;

        City1 += XPTickets12;
        City2 += XPTickets12;
        City3 += XPTickets32;
        City4 += XPTickets32;
        City5 += XPTickets52;

        Kach1 += XPTickets13;
        Kach2 += XPTickets13;
        Kach3 += XPTickets33;
        Kach4 += XPTickets33;
        Kach5 += XPTickets53;
    }
}

