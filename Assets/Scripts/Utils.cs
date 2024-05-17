using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    // ������      �����Ǹ�������ı�Ϊ��һ���� 0�ű�    ��˳ʱ�뷽��Ϊ 1�ű� 2�ű�... �ֱ��������������Ӧ
    static int[,] OddRow = { { 1, 0 }, { 0, 1 }, { -1, 1 }, { -1, 0 }, { -1, -1 }, { 0, -1 } };
    // ż����      �����Ǹ���������ı�Ϊ��һ���� 0�ű�    ��˳ʱ�뷽��Ϊ 1�ű� 2�ű�... �ֱ��������������Ӧ
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
        // �ж���ż��    �����ż���� ��ʹ��EvenRow����
        if (currentRowcol.y % 2 == 0)
        {
            for (int i = 0; i < 6; i++)
            {
                
                //Debug.Log(moveCurrentRowcol.x + EvenRow[i, 0]+ "," + (moveCurrentRowcol.y + EvenRow[i, 1]));
                // �ж��ɵ�ǰ�� �䵽 Ŀ��� ������    ��������ͷ�������ֵ��ȷ������
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
                // �ж��ɵ�ǰ�� �䵽 Ŀ��� ������    ��������ͷ�������ֵ��ȷ������
                if (currentRowcol.x + OddRow[i, 0] == targetRowcol.x &&
                    currentRowcol.y + OddRow[i, 1] == targetRowcol.y)
                {
                    return i;
                }
            }

        }
        return -1;
    }

    public static int BFS(Vector2Int currentRowcol,Vector2Int targetRowcol)                  //��bfs�ж���Ŀ���֮�����̾���
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
        int[] dx1 = { 1, 1, 0, -1, 0, 1 };                     //�����ǰ�������Ϊż��
        int[] dy1 = { 0, 1, 1, 0, -1, -1 };
        int[] dx2 = { 1, 0, -1, -1, -1, 0 };                   //�����ǰ�������Ϊ����
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
        Debug.Log("��̾��룡" + currentRowcol.x+ "    "+ currentRowcol.y+"    "+(targetRowcol.x)+"  "+(targetRowcol.y)  +"  " + d[targetRowcol.x, targetRowcol.y]);
        return d[targetRowcol.x, targetRowcol.y];
    }

    public static int JudgeDistance(Vector2Int currentRowcol,Vector2Int targetRowcol)
    {
        for (int i = 0; i < 36; i++)
            for (int j = 0; j < 64; j++)
                g[i, j] = 0;
        
        return BFS(currentRowcol,targetRowcol);
    }

    public static GameObject getChildObject(GameObject obj, string name)  // ��obj��ѯ�������ֵ�������
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
