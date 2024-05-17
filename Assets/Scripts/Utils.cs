using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    // 奇数列      以六角格最上面的边为第一方向即 0号边    按顺时针方向为 1号边 2号边... 分别与数组的索引对应
    static int[,] OddRow = { { 1, 0 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }, { 0, -1 } };
    // 偶数列      以六角格子最上面的边为第一方向即 0号边    按顺时针方向为 1号边 2号边... 分别与数组的索引对应
    static int[,] EvenRow = { { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, -1 } };
    public class Tuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    const int N = 110;

    static int[,] g = new int[N, N];
    static int[,] d = new int[N, N];
    static int n = 36;
    static int m = 64;

    
    // Define a Queue for BFS
    static Queue<Tuple<int, int>> q = new Queue<Tuple<int, int>>();
    public static int JudgeDirection(Vector2Int targetRowcol, Vector2Int currentRowcol)
    {
        // 判断奇偶列    如果是偶数列 则使用EvenRow数组
        if (currentRowcol.y % 2 == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                
                //Debug.Log(moveCurrentRowcol.x + EvenRow[i, 0]+ "," + (moveCurrentRowcol.y + EvenRow[i, 1]));
                // 判断由当前格 变到 目标格 的条件    条件满足就返回索引值以确定方向
                //Debug.Log(currentRowcol.x + "," + EvenRow[i, 0] + "," + targetRowcol.x + "," + currentRowcol.y + "," + EvenRow[i, 1] + "," + targetRowcol.y);
                if (currentRowcol.x + EvenRow[i, 0] == targetRowcol.x &&
                    currentRowcol.y + EvenRow[i, 1] == targetRowcol.y)
                {
                    return i;
                }
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                // 判断由当前格 变到 目标格 的条件    条件满足就返回索引值以确定方向
                if (currentRowcol.x + OddRow[i, 0] == targetRowcol.x &&
                    currentRowcol.y + OddRow[i, 1] == targetRowcol.y)
                {
                    return i;
                }
            }

        }
        return -1;
    }

    public static int BFS(Vector2Int currentRowcol,Vector2Int targetRowcol)                  //用bfs判断与目标格之间的最短距离
    {

        // Reset distances array
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                d[i, j] = -1;
            }
        }

        d[currentRowcol.x, currentRowcol.y] = 0;
        q.Enqueue(new Tuple<int, int>(currentRowcol.x, currentRowcol.y));
        int[] dx1 = { 1, 1, 0, -1, 0, 1 };                     //如果当前格的列数为偶数
        int[] dy1 = { 0, 1, 1, 0, -1, -1 };
        int[] dx2 = { 1, 0, -1, -1, -1, 0 };                   //如果当前格的列数为奇数
        int[] dy2 = { 0, 1, 1, 0, -1, -1 };
        int[] dx;
        int[] dy;
        while (q.Count > 0)
        {
            Tuple<int, int> t = q.Dequeue();
            for (int i = 0; i < 6; i++)
            {
                if (t.Item2 % 2 == 0)
                {
                    dx = dx1;
                    dy = dy1;
                }
                else
                {
                    dx = dx2;
                    dy = dy2;
                }
                int x = t.Item1 + dx[i];
                int y = t.Item2 + dy[i];

                if (x >= 0 && x < n && y >= 0 && y < m && g[x, y] == 0 && d[x, y] == -1)
                {
                    d[x, y] = d[t.Item1, t.Item2] + 1;
                    //Debug.Log(x + "   " + y + "  "+ d[x, y]);
                    q.Enqueue(new Tuple<int, int>(x, y));
                }
            }
        }
        Debug.Log("最短距离！" + currentRowcol.x+ "    "+ currentRowcol.y+"    "+(targetRowcol.x)+"  "+(targetRowcol.y)  +"  " + d[targetRowcol.x, targetRowcol.y]);
        return d[targetRowcol.x, targetRowcol.y];
    }

    public static int JudgeDistance(Vector2Int currentRowcol,Vector2Int targetRowcol)
    {
        for (int i = 0; i < 36; i++)
            for (int j = 0; j < 64; j++)
                g[i, j] = 0;
        
        return BFS(currentRowcol,targetRowcol);
    }

    public static GameObject getChildObject(GameObject obj, string name)  // 从obj查询给定名字的子物体
    {
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GameObject g = obj.transform.GetChild(i).gameObject;
            if (g != null && g.name == name)
            {
                return g;
            }
        }
        return null;
    }

}
