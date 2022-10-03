using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace TMall.util
{
    public class RecommendCal
    {
        private RecommendCal() { } // 禁止產生實體

        private const int K = 2000;
        private const int recommendNumber = 200; // 設置每個用戶最多保存的推薦的物品的個數
        public static void Run()
        {
            List<int> items = Respository.Recommend.GetItems(); // 獲取所有的物品id列表

            List<HashSet<string>> itemsOfusers = new List<HashSet<string>>();// 每個物品對應的被喜愛的用戶列表
            foreach (int item_id in items)
            { // 每個物品依次查詢
                itemsOfusers.Add(Respository.Recommend.GetUsers(item_id));
            }

            // 獲取物品的相似度矩陣
            List<Dictionary<int, double>> similityMatrix = GetSimilityMatrix(itemsOfusers);

            // 獲取每個使用者喜愛的物品集合, 相當於把itemOfUsers鍵值反轉一下 (這裡保存的是items列表裡第i個物品, 而不是物品id)
            Dictionary<string, SortedSet<int>> usersOfitems = new Dictionary<string, SortedSet<int>>();
            for (int i = 0; i < itemsOfusers.Count; ++i)
            {
                foreach (string username in itemsOfusers[i])
                {
                    if (usersOfitems.TryGetValue(username, out SortedSet<int> s))
                    {
                        s.Add(i);
                    }
                    else
                    {// 如果沒有包含這個user,就新建一個這個user代表的集合
                        s = new SortedSet<int> { i };
                        usersOfitems.Add(username, s);
                    }
                }
                itemsOfusers[i].Clear(); // 用過了就馬上清空釋放記憶體
                itemsOfusers[i] = null;
            }
            itemsOfusers.Clear();

            // 獲取每個物品的, 與它相似度前k大的列表
            // 其實可以自己定義一個結構體,也可以用這個KeyVaulePair<int,double>, 表示對第int個item的相似度為double
            List<List<KeyValuePair<int, double>>> similityItems = new List<List<KeyValuePair<int, double>>>();
            for (int i = 0; i < items.Count; ++i)
            {
                // 對稱矩陣只保存了主對角線的下面一半, 所以對於i來說查詢分兩部分,
                // 一部分是大於i的, 在similityMatrix[i]裡面存著, 
                // 另一部分是小於i的, 需要查找similityMatrix[k] 裡面是否包含i

                // 第一部分
                List<KeyValuePair<int, double>> tmp = similityMatrix[i].ToList();

                // 第二部分
                for (int j = 0; j < i; ++j)
                {
                    if (similityMatrix[j].TryGetValue(i, out double sim))
                    {// 如果包含i,j的關係,就保存下來
                        tmp.Add(new KeyValuePair<int, double>(j, sim));
                    }
                }

                // 獲取之後,就要排序, 只保留前K大的.
                tmp.Sort((x1, x2) => {
                    if (x1.Value < x2.Value) return 1;
                    else return 0;
                });
                // 然後把後面超過K的刪除(省點記憶體)
                while (tmp.Count > K)
                {
                    tmp.RemoveAt(tmp.Count - 1);
                }
                // 最後把tmp加入對應的列表中
                similityItems.Add(tmp);
            }
            similityMatrix.Clear(); // 不用了就馬上清空

            // 到了這裡, 就可以計算每個用戶的推薦值了:
            foreach (var userPair in usersOfitems)
            {
                Dictionary<int, double> recommend = new Dictionary<int, double>(); // 給用戶推薦的值, 這個int就保存item_id了
                foreach (var userLike in userPair.Value)
                { // 遍歷用戶喜歡的物品
                    foreach (var itemSimility in similityItems[userLike])
                    { // 遍歷與用戶喜歡的物品相似的前K個
                        if (userPair.Value.Contains(itemSimility.Key)) continue; // 如果這個物品用戶已經喜愛了, 就不推薦了
                        int item_id = items[itemSimility.Key]; // 獲取item_id
                        if (recommend.TryGetValue(item_id, out double sim))
                        {
                            recommend[item_id] = sim + itemSimility.Value; // 相似度疊加
                        }
                        else
                        {
                            recommend[item_id] = itemSimility.Value;
                        }
                    }
                }
                List<KeyValuePair<int, double>> tmp = recommend.ToList();

                // 將大於給定個數的時候,刪除感興趣值程度小的
                if (tmp.Count > recommendNumber)
                {
                    tmp.Sort((x1, x2) => {  // 從大到小排序
                        if (x1.Value < x2.Value) return 1;
                        else return 0;
                    });
                    while (tmp.Count > recommendNumber) tmp.RemoveAt(tmp.Count - 1);
                }
                // 保存到資料庫
                Respository.Recommend.UpdateRecommend(userPair.Key, tmp);

                recommend.Clear();
                tmp.Clear(); // 清空
            }
        }

        // 定義一個閾值, 如果物品相似度小於這個值, 就表示兩個物品無關, 就不加入相似度矩陣
        private const double similityThreshold = 0.001;

        // 計算相似度矩陣
        private static List<Dictionary<int, double>> GetSimilityMatrix(List<HashSet<string>> itemsOfusers)
        {
            List<Dictionary<int, double>> result = new List<Dictionary<int, double>>();
            for (int i = 0; i < itemsOfusers.Count; ++i)
            {
                result.Add(new Dictionary<int, double>());
                for (int j = i + 1; j < itemsOfusers.Count; ++j)
                { // 計算兩兩相似度
                    int Nj = itemsOfusers[j].Count();
                    int Ni = itemsOfusers[i].Count();
                    if (Ni == 0 || Nj == 0) continue;
                    int Nij = 0;
                    foreach (var user_i in itemsOfusers[i])
                    { // 計算交集個數
                        if (itemsOfusers[j].Contains(user_i)) ++Nij;
                    }
                    double simility = Convert.ToDouble(Nij) / Math.Sqrt(Ni * Nj);
                    if (simility > similityThreshold)
                    {// 如果相似度大於這個閾值,就加進去
                        result[i].Add(j, simility);
                    }
                }
            }
            return result;
        }
    }
}
