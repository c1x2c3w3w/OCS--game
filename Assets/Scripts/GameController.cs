
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static TurnFlow;
using System.Text.RegularExpressions;
using System.Threading;


public class GameController : MonoBehaviour
{
    public GameObject hexGridPrefab;  // 六角格物体的prefab 
    public GameObject unitPrefab;

    public int mouseLeftClickCoolDown = 0;  // 鼠标左键点击后，需要有一定的冷却时间。在update中更新。
    public int mouseRightClickCoolDown = 0; // 鼠标右键点击后，需要有一定的冷却时间。在update中更新。
    public int mouseClickCoolDown = 0;
    bool isWaitingForLeftClick = true;
    bool backAction = false;                //  未进入战后后退

    public int[] selectedArmyIndex;         // 记录当前左上角GUI处被选定的部队的编号
    int[] choooseArmyIndex;                 // 记录攻击队列GUI的部队编号
    List<int> selectedJoinAttackArmyIndex;  // 记录被选定参与联合进攻的部队编号
    List<int> selectedJoinchoooseArmyIndex;
    Vector2Int backenemyArmyGrid;           // 攻击GUI里被攻击的敌人格子的坐标（行、列）

    Vector2Int enemyArmyGrid;               // 被攻击的敌人格子的坐标（行、列）
    List<int> enemyInTargetHexGrid;         // 记录攻击GUI被攻击的部队编号

    Vector2Int moveCurrentRowcol;           // 记录移动操作中，左键点击的那个格子（要移动的部队的格子）。
    List<int> armyInTargetHexGrid;          // 记录移动操作“右键点击”时，要去的目标格子上的部队编号
    public static MapHexElement targetHex;  // 记录移动操作“右键点击”时，要去的目标格子的地形元素

    public bool stratageMovingState;        // 记录移动操作“右键点击”时,战略移动模式状态  true 或false
    public bool combatState;                // 战斗模式状态
    public bool movingState;                // 移动模式状态

    public int[] moveCount;                  // 判断转换移动模式之前是否移动
    public bool moveReady = false;           // 判断本回合是否移动
    public bool attackReady = false;          // 判断本回合是否攻击
    public bool fightReady = false;            // 判断战斗阶段战斗状态是否结束
    private void Awake()
    {
        globalSetup();
        selectedArmyIndex = new int[4];
        choooseArmyIndex = new int[15];
        moveCount = new int[19];
        for (int i = 0; i < selectedArmyIndex.Length; i++) { selectedArmyIndex[i] = -1; }
        selectedJoinAttackArmyIndex = new List<int>();
        for (int i = 0; i < choooseArmyIndex.Length; i++) { choooseArmyIndex[i] = -1; }
        selectedJoinchoooseArmyIndex = new List<int>();
        enemyArmyGrid.x = -1;   // 代表未选中任何格子用于执行进攻
        GameData.turnflow.turnPlayer = TurnPlayer.PlayerChinese;

    }
    void Update()
    {
        if (mouseLeftClickCoolDown > 0) mouseLeftClickCoolDown--;
        if (Input.GetMouseButton(0) && mouseLeftClickCoolDown == 0)
        {
            mouseLeftClickCoolDown = 30;
            HandleInput_LeftMouse();
        }

        if (mouseRightClickCoolDown > 0) mouseRightClickCoolDown--;
        if (Input.GetMouseButton(1) && mouseRightClickCoolDown == 0)
        {
            mouseRightClickCoolDown = 30;
            HandleInput_RightMouse();
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        DebugGo();
        

    }

    void DebugGo()
    {
        // 调式阶段 的代码初始化工作
        GameData.turnflow.GoIntoMoveStage();

    }
    private void globalSetup()
    {
        GameData.hexGrid = new GameObject[36, 64];//地图36行，64列
        for (int i = 0; i < 36; i++)
            for (int j = 0; j < 64; j++)
            {
                Vector3 pos = getHexPosition(i, j);
                GameData.hexGrid[i, j] = Instantiate(hexGridPrefab, pos, transform.rotation);//生成新的六角格
                GameData.hexGrid[i, j].name = "Hex_" + i.ToString() + "_" + j.ToString();

                MapHexElement mhe = MapDataBase.Instance.SetHex(i, j);
                GameData.hexGrid[i, j].GetComponent<HexGrid>().setMapHexElement(mhe);
                GameData.hexGrid[i, j].GetComponent<HexGrid>().rowcolpos.x = i;
                GameData.hexGrid[i, j].GetComponent<HexGrid>().rowcolpos.y = j;

            }
        int numUnits = UnitDataBase.Instance.units.Length;
        GameData.units = new GameObject[numUnits];
        for(int i = 0; i<numUnits; i++)
        {
            GameData.units[i] = GenerateUnit(i);
        }
       
    }
    private GameObject GenerateUnit(int i)
    {
        GameObject newUnit;  // 新建一个unit  函数最后return。
        UnitElement u = UnitDataBase.Instance.units[i];
        Vector3 pos = GameData.getHexWorldCoordinate(u.comeRow, u.comeCol);
        newUnit = Instantiate(unitPrefab,pos, transform.rotation);
        newUnit.GetComponent<Unit>().setInitValue(u.comeRow, u.comeCol, u.counterType, u.division);

        int counterType = UnitDataBase.Instance.units[i].counterType;
        newUnit.GetComponent<SpriteRenderer>().sprite = GetComponent<CounterImage>().getCounterPicture(counterType);
        return newUnit;
    }


    Vector3 getHexPosition(int i, int j)
    {
        Vector3 pos;
        float gridWidth = 0.9495f; //格子宽
        float gridHeight = gridWidth * 1.155f; //格子高度根据地图的宽和高的比为：3：2*sqrt(3)
        if (j % 2 == 1) pos = new Vector3((float)(-30.0 + j * gridWidth), (float)(-19.2 - gridHeight / 2 + i * gridHeight), 0f);
        else pos = new Vector3((float)(-30.0 + j * gridWidth), (float)(-19.2 + i * gridHeight), 0f);
        return pos;
    }
    // Update is called once per frame

    void HandleInput_LeftMouse()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            // 下来是从collider中获取特定类型组件，如果击中的物体确实带这个组件，说明它被点击
            HexGrid hex = hit.collider.GetComponent<HexGrid>();
            if (hex)
            {
                Vector3 point = hit.point;
                hideAllRedBorder();
                Vector2Int rowcol = GameData.getPosOfObject(hex.name);

                // 获取左键点击时的行、列
                moveCurrentRowcol = rowcol;
                GameData.hexGrid[rowcol.x, rowcol.y].GetComponent<HexGrid>().onSelected();
   
                // 点中的格子中，如有部队，在左上角GUI中显示各个部队。
                selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(rowcol);
               
                // 战略移动模式,战斗模式状态值变成 false
                stratageMovingState = false;
                combatState = false;
                movingState = true;
            }
            //FightArmy f = hit.collider.GetComponent<FightArmy>();
            //if (f)
            //{
               // choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, armyInTargetHexGrid);

           // }
        }
    }

    void HandleInput_RightMouse()
    {
        // 从鼠标位置发送一个射线，若射线能击中物体，可以判断击中什么物体。
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 使用物理系统，发射射线，击中的物体放在hit变量;  后半部分条件是处理 鼠标在GUI控件上方的情况
        if (Physics.Raycast(inputRay, out hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            // 下来是从collider中获取特定类型组件，如果击中的物体确实带这个组件，说明它被点击
            HexGrid hex = hit.collider.GetComponent<HexGrid>();
            if (hex)    // 若不为NULL，说明击中的是“带HexGrid组件”的物体
            {
                //Vector3 point = hit.point;     // 获得击中的位置
               
                
                    Vector2Int rowcol = GameData.getPosOfObject(hex.name);
                    // 得到目标格子上的部队编号
                    armyInTargetHexGrid = hex.getHexArmy();

                    // 获得目标格子的地形元素信息
                    targetHex = MapDataBase.Instance.SetHex(rowcol.x, rowcol.y);
                    
                    PerformRightClick(rowcol);
                

            }
        }

    }

    void PerformRightClick(Vector2Int rowcol)
    {
        // 获取当前的 “回合阶段”
        TurnStage currentStage = GameData.turnflow.turnStage;
        
        // 如果是增援阶段，直接退出
        if (currentStage == TurnStage.StageReinforement) return;

        // 如果是进攻阶段
        if(currentStage == TurnStage.AirforceStageAttack || currentStage == TurnStage.ReactionStage || currentStage == TurnStage.FightStage){
            for (int i = 0; i <= 18; i++)
            {
                moveCount[i] = 0;
            }
            
            // 要判断一种特殊情况：
            if(selectedJoinAttackArmyIndex.Count != 0 && backAction == false)
            {
                ErrorMessageBox.Show("错误：此前的进攻行动还未完成执行。");
                return;
            }

            // 要判断被点击的格子是否有部队
            List<int> hexArmy = GameData.hexGrid[rowcol.x, rowcol.y].GetComponent<HexGrid>().getHexArmy();
            if (backAction == false)
            {
                if (hexArmy.Count == 0)  // 说明格子中没有部队
                {
                    ErrorMessageBox.Show("错误：格子中没有部队，无法进攻该格子。");
                    return;
                }
                else
                {
                    // 进而要判断被点击的格子是否有 “敌人”的部队
                    if (!JudgeIsEnemy())
                    {
                        ErrorMessageBox.Show("错误：格子中没有敌军部队，无法进攻该格子。");
                        return;
                    }
                }
            }
            

            // 条件都满足，这个格子有敌军，将被攻击。把当前格显示黄色边框，并记录格子。
            hideAllYellowBorder();
            GameData.hexGrid[rowcol.x, rowcol.y].GetComponent<HexGrid>().onAttackGridSelected();
            enemyArmyGrid = rowcol;   // 记录被攻击的格子



        }

        // 如果是移动阶段
        if(currentStage == TurnStage.StageMove || currentStage == TurnStage.StageMove2)
        {
            if (CanMoveArmy(rowcol))    // 执行部队移动判断
            {
                
                // 进行移动，并且更新位置
                Execute_Move_Action(rowcol);
                selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(rowcol);
                

                // 移动到目标位置显示红框
                hideAllRedBorder();
                GameData.hexGrid[rowcol.x, rowcol.y].GetComponent<HexGrid>().onSelected();

                // 更新"当前格子"
                moveCurrentRowcol = rowcol;

            }
        }
    }

     bool CanMoveArmy(Vector2Int targetRowcol)
    {
        // 首先判断当前是否为“移动阶段”, 不是移动阶段则直接退出
        TurnPlayer currentPlayer = GameData.turnflow.turnPlayer;
        int currentHex_CounterType = GameData.units[selectedArmyIndex[0]].GetComponent<Unit>().counterID;
        int currentHex_armyNation = Counter.counter[currentHex_CounterType].nation;
        int n = 0;
        if (currentPlayer == TurnPlayer.PlayerChinese)
        {
            if(currentHex_armyNation == 1)
            {
                ErrorMessageBox.Show("请选择志愿军");
                return false;
            }  
        }
        if (currentPlayer == TurnPlayer.PlayerAmerica)
        {
            if (currentHex_armyNation == 0)
            {
                ErrorMessageBox.Show("请选择美军");
                return false;
            }

        }
        // 判断是否左键选择了部队
        if (selectedArmyIndex[0] == -1)
        {
            ErrorMessageBox.Show("请选择部队");
            return false;
        }
        // 判断移动的目标格是否与原格子相邻
        if (Utils.JudgeDirection(moveCurrentRowcol, targetRowcol) == -1)
        {
            ErrorMessageBox.Show("请移动到相邻格");
            return false;
        }
        // 判断是否为空地
        if(armyInTargetHexGrid.Count != 0)
        {

            // 判断右键点击的格子，是否存在敌军，则直接退出；如果存在自己的部队，提示“被阻挡无法移动”
            if (JudgeIsEnemy())
            {
                ErrorMessageBox.Show("被阻挡无法移动");
                return false;
            }
            for(int i = 0; i < selectedArmyIndex.Length; i++)
            {
                if (selectedArmyIndex[i] != -1) n++;            // 记录左上角GUI实际存在算子的个数
            }
            if((armyInTargetHexGrid.Count + n) > 4)
            {
                Debug.Log(armyInTargetHexGrid.Count +"  "+ n);
                ErrorMessageBox.Show("格子部队已满");
                return false;
            }
        }

        // 判断其他可能需要判断的情况（例如坦克在山地地形，则移动路线必须有道路）
        
        int direction = Utils.JudgeDirection(targetRowcol, moveCurrentRowcol);
        //Debug.Log("targetHex.roads[direction]" + targetHex.roads[direction]);
        if (IsContainsTankArmy())
        {
            
            if (targetHex.terrain == 5)
            {

                if(targetHex.roads[direction] == 0)
                {
                    ErrorMessageBox.Show("坦克部队无法移动到目标格子");
                    return false;
                }
            }
        }

        // 若是再判断剩余的移动力（MovingPoint 是否足够移动到下一格），可能提示“移动力不足”
        // 还有注意是否是“战略移动模式（固定的高移动点数）”
        List<int> armyToMove = getActionArmy();
        for(int i = 0; i < armyToMove.Count; i++)
        {
            if (armyToMove[i] < 0) continue;
            Unit unit = GameData.units[armyToMove[i]].GetComponent<Unit>();

            //判断当前unit是不是坦克
            int index = armyToMove[i];
            GameObject unitObject = GameData.units[index];
            Unit units = unitObject.GetComponent<Unit>();
            int countId = units.counterID;
            bool containTank = false;
            if(moveCount[countId] == 2)
            {
                ErrorMessageBox.Show("已结束移动！");
                return false;
            }
            moveCount[countId] = 1;                            //表示移动过

            if (Counter.counter[countId].type == 2)
            {
                containTank = true;
            }
            // 判定移动力是否充足

            int currentMP = unit.movingPoint;
            int needed = MovingPointsNeeded(targetRowcol, containTank, currentMP);

            //Debug.Log("需要的移动力" + needed);

            if (needed > currentMP)    // 移动力不足
            {
                ErrorMessageBox.Show("移动力不足");
                return false;
  
            }

        }

        return true;
    }

    bool JudgeIsEnemy()
    {
        // 当前格子的部队编号
        int currentHex_CounterType = GameData.units[selectedArmyIndex[0]].GetComponent<Unit>().counterID;
        int currentHex_armyNation = Counter.counter[currentHex_CounterType].nation;
        // 目标格子的部队编号
        int targetHex_CounterType = GameData.units[armyInTargetHexGrid[0]].GetComponent<Unit>().counterID;
        int targetHex_armyNation = Counter.counter[targetHex_CounterType].nation;
        // 如果当前格子上和目标格子上的部队编号都是0，也就是志愿军
        if (currentHex_armyNation == 0  && currentHex_armyNation == targetHex_armyNation)
            return false;
        // 如果当前格子上和目标格子上的部队编号都不是0，也就是美联军
        else if (currentHex_armyNation != 0 && targetHex_armyNation != 0)
            return false;
        // 如果不满足上述条件 则目标格子是敌军
        else
            return true;
    }

    bool BackJudgeIsEnemy(Unit u)
    {
        // 当前格子的部队编号
        int currentHex_CounterType = u.counterID;
        int currentHex_armyNation = Counter.counter[currentHex_CounterType].nation;
        // 目标格子的部队编号
        int targetHex_CounterType = GameData.units[armyInTargetHexGrid[0]].GetComponent<Unit>().counterID;
        int targetHex_armyNation = Counter.counter[targetHex_CounterType].nation;
        // 如果当前格子上和目标格子上的部队编号都是0，也就是志愿军
        if (currentHex_armyNation == 0 && currentHex_armyNation == targetHex_armyNation)
            return false;
        // 如果当前格子上和目标格子上的部队编号都不是0，也就是美联军
        else if (currentHex_armyNation != 0 && targetHex_armyNation != 0)
            return false;
        // 如果不满足上述条件 则目标格子是敌军
        else
            return true;
    }
    bool IsContainsTankArmy()
    {
        bool containsTank = false;
        List<int> armyToMove = getActionArmy();
        for (int i = 0; i<armyToMove.Count; i++)
        {
            int index = armyToMove[i];
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            int countId = unit.counterID;
            if(Counter.counter[countId].type == 2)
            {
                containsTank = true;
                break;
            }
        }
        return containsTank;
    }


    int MovingPointsNeeded(Vector2Int targetRowcol,bool containTank,int currentMP)  // 返回 从当前格进入目标格，需要多少移动力
    {
        int movingPoints = 0;
        // 获得目标格与当前格接壤的边
        int targetHexDirection = Utils.JudgeDirection(moveCurrentRowcol,targetRowcol);
        if (targetHexDirection == -1) return movingPoints;
        // 处理信息
        // 河流

        switch (targetHex.rivers[targetHexDirection])
        {
            
            case 0: 
                movingPoints += 0;break;
            case 1:
                if (containTank)   //判断是坦克还是步兵
                {
                    movingPoints += 3; break;
                }
                else { movingPoints += 5; break; }

        }
        // 道路
        switch (targetHex.roads[targetHexDirection])
        {
            case 0:
                {
                    //地形
                    switch (targetHex.terrain)
                    {
                        case 1: 
                        case 2: movingPoints += 1; break;
                        case 3:
                            if (containTank) //判断是否为坦克
                            {
                                movingPoints += 2; break;
                            }
                            else { movingPoints += 1; break; }
                        case 4:
                            if (containTank)
                            {
                                movingPoints += 3; break;
                            }
                            else { movingPoints += 2; break; }
                            
                        case 5:
                            if (containTank)
                            {
                                movingPoints += 100; break;
                            }
                            else {
                                int m = currentMP - movingPoints; //步兵将消耗所有移动力
                                if (m == 0) { movingPoints += 100; break; }
                                else { movingPoints += m; break; }
                                }
                        case 6: break;
                    }
                    break;
                }

            case 1: movingPoints += 1; 
                    break;

        }
        return movingPoints;
    }
    void Execute_Move_Action(Vector2Int targetRowcol)
    {
        List<int> armyToMove = getActionArmy();   // 选出格子中的被选中的部队
        for (int i = 0; i < armyToMove.Count; i++)
        {
            int index = armyToMove[i];  // 部队编号
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            Unit units = GameData.units[armyToMove[i]].GetComponent<Unit>();
            // 修改部队的位置
            unit.UpdateUnitPosition(targetRowcol);
            // 支付移动点数
            bool containTank = false;
            if (Counter.counter[unit.counterID].type == 2)
            {
                containTank = true;
            }
            int currentMP = units.movingPoint;
            int pointNeeded = MovingPointsNeeded(targetRowcol, containTank, currentMP);
            unit.useMovingPoint(pointNeeded);
            moveReady = true;
            unit.finishTurnMoved = moveReady;
        }
        }

    public List<int> getActionArmy()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            GameObject armyIcon = GetComponent<OperationGUI>().getArmyIconSelectedFromIndex(i);
            if (armyIcon.GetComponent<Image>().sprite != GetComponent<CounterImage>().getCounterPicture(-1))
            {
                GameObject redRectBorder = Utils.getChildObject(armyIcon, "RedRectBorder");

                if (redRectBorder.GetComponent<RedRectBorder>().isSelected)
                    list.Add(selectedArmyIndex[i]);
            }
           
        }
        return list;
    }

    public List<int> getChooseFightArmy()           //获取所选攻击队列的算子编号
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            GameObject armyIcon2 = GetComponent<ChooseGUI>().getFightIconSelectedFromIndex(i);
            if (armyIcon2.GetComponent<Image>().sprite != GetComponent<CounterImage>().getCounterPicture(-1))
            {
                GameObject redRectBorder2 = Utils.getChildObject(armyIcon2, "RedSquareBorder");
                if (redRectBorder2.GetComponent<RedRectBorder>().isSelected2) list.Add(choooseArmyIndex[i]);
            }

           
        }
 
        return list;
    }

    public List<int> getChooseDefenseArmy()      //获取所选防守队列的算子编号
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            GameObject armyIcon = GetComponent<ChooseGUI>().getDenfenseIconSelectedFromIndex(i);
            if (armyIcon.GetComponent<Image>().sprite != GetComponent<CounterImage>().getCounterPicture(-1))
            {
                GameObject redRectBorder = Utils.getChildObject(armyIcon, "RedSquareBorder");

                if (redRectBorder.GetComponent<RedRectBorder>().isSelected2)
                {
                    list.Add(choooseArmyIndex[i + selectedJoinAttackArmyIndex.Count]);
                }
            }

        }
        return list;
    }
    //隐藏所有格的边框
    void hideAllRedBorder()
    {
        for (int i = 0; i < 36; i++)
            for (int j = 0; j < 64; j++)
            {
                GameData.hexGrid[i, j].GetComponent<HexGrid>().setRedBorderStatus(false);
            }
    }

    void hideAllYellowBorder()
    {
        for (int i = 0; i < 36; i++)
            for (int j = 0; j < 64; j++)
            {
                GameData.hexGrid[i, j].GetComponent<HexGrid>().setYellowBorderStatus(false);
            }
    }
    public void OnBtnChangeToFightMove()
    {

        if (stratageMovingState == false)
        {
            combatState = !combatState;
            movingState = !movingState;
            
            List<int> armyToMove = getActionArmy();   // 选出格子中的被选中的部队
            for (int i = 0; i < armyToMove.Count; i++)
            {
                int index = armyToMove[i];  // 部队编号
                GameObject unitObject = GameData.units[index];
                Unit unit = unitObject.GetComponent<Unit>();
                
                if (moveCount[unit.counterID] == 1)
                {
                    ErrorMessageBox.Show("已移动过");
                    combatState = !combatState;
                    movingState = !movingState;
                    return;
                }
                else if (moveCount[unit.counterID] == 2)
                {
                    ErrorMessageBox.Show("已结束移动！");
                    combatState = !combatState;
                    movingState = !movingState;
                    return;
                }
                unit.fullState = combatState;
                unit.movingPoint = unit.fullState ? Counter.counter[unit.counterID].moveAllowInjure : Counter.counter[unit.counterID].moveAllow;
            }
            MessageBox.Show(combatState ? "战斗模式开启" : "移动模式开启");
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
        }
        else{
            MessageBox.Show("未结束战略模式");

        }
    }

    public void OnBtnChangeToStrategyMove()
    {
              
        // 战略移动状态变量  取反， 并且显示提示。 
        stratageMovingState = !stratageMovingState;
        // 选出格子中的被选中的部队
        List<int> armyToMove = getActionArmy();
        for (int i = 0; i < armyToMove.Count; i++)
        {
            int index = armyToMove[i];  // 部队编号
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            unit.strategyMove = stratageMovingState;
            // 如果部队已经曾经移动过（但还没有结束移动），那么不允许改变“战略移动状态变量”的
            if (moveCount[unit.counterID] == 1)
            {
                ErrorMessageBox.Show("已移动过");
                stratageMovingState = !stratageMovingState;
                return;
            }
            else if (moveCount[unit.counterID] == 2)
            {
                ErrorMessageBox.Show("已结束移动！");
                stratageMovingState = !stratageMovingState;
                return;
            }
            else if (stratageMovingState)
            {
                unit.movingPoint = unit.fullState ? Counter.counter[unit.counterID].moveAllowInjure*2 :  Counter.counter[unit.counterID].moveAllow*2;
            }
            else
            {
                unit.movingPoint = unit.fullState ?  Counter.counter[unit.counterID].moveAllowInjure : Counter.counter[unit.counterID].moveAllow ;
            }
           
        }
       
        MessageBox.Show(stratageMovingState ? "战略移动开启" : "普通移动模式");
        selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
    }

    public void OnBtnEndCurrentArmyMove()
    {
        List<int> armyToMove = getActionArmy();
        for (int i = 0; i < armyToMove.Count; i++)
        {
            int index = armyToMove[i];  // 部队编号
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            moveCount[unit.counterID] = 2;
            moveReady = true;
            unit.finishTurnMoved = moveReady;
            MessageBox.Show("已结束当前移动");
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
        }
        }
    public void OnBtnEndMoveStage()
    {
        if (GameData.turnflow.turnStage == TurnFlow.TurnStage.StageMove)
        {
            GameData.turnflow.GoIntoAirforceStageAttack();
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
            
        }
        else if (GameData.turnflow.turnStage == TurnFlow.TurnStage.StageMove2)
        { 
            GameData.turnflow.GoIntoReactionStage();
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);

        }

    }
    public void OnBtnAddToAttackList()
    {
        // 如果还没选择要被攻击的格子
        if(enemyArmyGrid.x == -1){
            ErrorMessageBox.Show("错误：未选中进攻格子。");
            return;
        }
        // 把当前红框内 又处于选中状态的部队，加入到 “进攻部队的队列”

        List<int> armyToAttack = getActionArmy();
        if (armyToAttack.Count == 0) return;
        bool canAdd = CanAddToAttackList(enemyArmyGrid, armyToAttack);
        if (canAdd)
        {
            for (int i = 0; i < armyToAttack.Count; i++)
            {
                selectedJoinAttackArmyIndex.Add(armyToAttack[i]);
                MessageBox.Show("已添加到攻击队列");
            }

        }
        else return;
        for (int i = 0; i < selectedJoinAttackArmyIndex.Count; i++)
        {
            Debug.Log("已添加的队列！！" + selectedJoinAttackArmyIndex[i]);
        }
        enemyInTargetHexGrid = armyInTargetHexGrid;
        
        choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, enemyInTargetHexGrid);
    }
    public void OnBtnClearAttackList()
    {
        // 清除当前的 “进攻部队的队列”
        if(selectedJoinAttackArmyIndex.Count >0)
        {
            selectedJoinAttackArmyIndex.Clear();
            choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, enemyInTargetHexGrid);
        }
        MessageBox.Show("队列已清除");
    }
    public void OnBtnAttack()
    {
        // 执行进攻
        // 参与进攻的所有部队，是“进攻部队的队列”中的所有部队
        // 被进攻的部队，是enemyArmyGrid变量的格中的所有敌军部队

        // 结算进攻结果
        if (CanAttackArmy(enemyArmyGrid))
        {
            fightReady = false;
            Execute_Attack_Action(enemyArmyGrid);
        }
        // 进攻结果结算完成后
        
    }
    public void OnBtnEndAttackStage()
    {
        TurnStage currentStage = GameData.turnflow.turnStage;
        if (currentStage == TurnStage.FightStage)
        {
            List<int> armyToMove = getActionArmy();
            for (int i = 0; i < armyToMove.Count; i++)
            {
                int index = armyToMove[i];  // 部队编号
                GameObject unitObject = GameData.units[index];
                Unit unit = unitObject.GetComponent<Unit>();
                moveReady = false;
                unit.finishTurnMoved = moveReady;             //判断本回合是否已移动
            }
        selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
        }
        if (GameData.turnflow.turnStage == TurnFlow.TurnStage.AirforceStageAttack)
        {
            
            GameData.turnflow.GoIntoMoveStage2();
            GameData.turnflow.GoChangeTurnPlayer();
            hideAllYellowBorder();
        }

        else if (GameData.turnflow.turnStage == TurnFlow.TurnStage.ReactionStage)
        {
            
            GameData.turnflow.GoIntoFightStage();
            GameData.turnflow.GoChangeTurnPlayer();
            fightReady = false;
        }
        else if (GameData.turnflow.turnStage == TurnFlow.TurnStage.FightStage)
        {
            if(fightReady == false)
            {
                ErrorMessageBox.Show("战斗状态未结束");
                return;
            }
            GameData.turnflow.GoIntoMoveStage();
            // 进行完一个回合后移动力恢复为原来值 
            int numUnits = UnitDataBase.Instance.units.Length;
            for (int i = 0; i < numUnits; i++)
            {
                Unit unit = GameData.units[i].GetComponent<Unit>();
                int countId = unit.counterID;
                unit.movingPoint = unit.fullState ? Counter.counter[countId].moveAllow : Counter.counter[countId].moveAllowInjure;
                unit.alreadyRaiseAttack = false;  //恢复为未攻击
                unit.finishTurnMoved = false;  // 恢复为未移动
            }
            GameData.turnflow.GoChangeTurnPlayer();
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
        }


    }

    bool CanAddToAttackList(Vector2Int enemyArmyGrid, List<int> armyToAttack)
    {
        TurnPlayer currentPlayer = GameData.turnflow.turnPlayer;
        int currentHex_CounterType = GameData.units[selectedArmyIndex[0]].GetComponent<Unit>().counterID;
        int currentHex_armyNation = Counter.counter[currentHex_CounterType].nation;
        if (currentPlayer == TurnPlayer.PlayerChinese)
        {
            if (currentHex_armyNation == 1)
            {
                ErrorMessageBox.Show("请选择志愿军");
                return false;
            }
        }
        if (currentPlayer == TurnPlayer.PlayerAmerica)
        {
            if (currentHex_armyNation == 0)
            {
                ErrorMessageBox.Show("请选择美军");
                return false;
            }

        }
       
        if (JudgeIsEnemy() == false)                                  //相同联军不能添加
        {
            ErrorMessageBox.Show("友军无法添加队列");
            return false;
        }
        for (int i = 0; i < armyToAttack.Count; i++)                 
        {
            int index = armyToAttack[i];
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            int countId = unit.counterID;
            TurnStage currentStage = GameData.turnflow.turnStage;
            if (currentStage == TurnStage.AirforceStageAttack)
            {
                if(Counter.counter[countId].type != 3)
                {
                    ErrorMessageBox.Show("未选中空军");
                    return false;
                }
            }
            if (currentStage == TurnStage.ReactionStage)
            {
                if (Counter.counter[countId].type == 0 )
                {
                    ErrorMessageBox.Show("未选中射击部队");
                    return false;
                }
            }
            if (unit.alreadyRaiseAttack == true)               //本回合已经攻击过的部队无法添加
            {
                ErrorMessageBox.Show("已经发起过攻击，无法添加");
                return false;
            }

            if (unit.strategyMove == true)                   //判断每次要加入队列里有没有存在战略模式的无法进入
            {
                ErrorMessageBox.Show("战略模式无法添加队列");
                return false;
            }
            if (Counter.counter[countId].type == 1 || Counter.counter[countId].type == 3)                  //超出攻击范围就无法添加：炮、飞机
            {
                if (Utils.JudgeDistance(moveCurrentRowcol, enemyArmyGrid) > Counter.counter[countId].attackArea)
                {
                    ErrorMessageBox.Show("超出范围，无法添加队列");
                    return false;
                }

            }
            else
            {
                if (Utils.JudgeDirection(moveCurrentRowcol, enemyArmyGrid) == -1) // 判断是否相邻格
                {
                    ErrorMessageBox.Show("超出范围，无法添加队列");
                    return false;
                }
            }

        }

        for (int i = 0; i < selectedJoinAttackArmyIndex.Count; i++)
        {     
            for (int j = 0; j < armyToAttack.Count; j++)
            {
                if(selectedJoinAttackArmyIndex[i] == armyToAttack[j] )                //已在攻击部队里的不能重复添加
                {
                    ErrorMessageBox.Show("已在攻击队列里");
                    return false;
                }
            }
        }
     return true;
    }

    bool CanAttackArmy(Vector2Int rowcol)  // 判断部队是否可以攻击
    {
        if (enemyArmyGrid.x == -1)
        {
            ErrorMessageBox.Show("错误：未选中进攻格子");
            return false;
        }
        if(selectedJoinAttackArmyIndex.Count == 0)
        {
            ErrorMessageBox.Show("错误：未选中进攻算子");
            return false;
        }
        return true;
    }

    void Execute_Attack_Action(Vector2Int rowcol)
    {
        int injury = 0;      // 受伤值
        int i = Random.Range(0, armyInTargetHexGrid.Count);                                //随机选择一个受伤算子
        int index = armyInTargetHexGrid[i];
        GameObject unitObject = GameData.units[index];
        Unit unit = unitObject.GetComponent<Unit>();
        TurnStage currentStage = GameData.turnflow.turnStage;
        // 按照规则进行查表计算
        if (currentStage == TurnStage.AirforceStageAttack || currentStage == TurnStage.ReactionStage) //空军射击阶段、反应阶段
        {
            string injure;
            injure = LookRuleTable.LookUpRuleTable(selectedJoinAttackArmyIndex, armyInTargetHexGrid);
            if (injure == "0") injury = 0;

            if (injure == "1/2")
            {

                int ra = Random.Range(1, 7);
                if (ra >= 4 && ra <= 6)
                {
                    injury = 1;
                    injure = "DG";
                }
                else injure = "DG";

            }

            if (injure == "DG")                                                    //DG状态战斗值，移动值减半
            {

                if (unit.fullState)
                {
                    Counter.counter[unit.counterID].attackInjure /= 2;
                    Counter.counter[unit.counterID].moveAllowInjure /= 2;
                }
                else
                {
                    Counter.counter[unit.counterID].attack /= 2;
                    Counter.counter[unit.counterID].moveAllow /= 2;
                }
            }
            if (injure == "1" || injure == "2" || injure == "3")
            {
                injury = int.Parse(injure);
            }

            unit.remainDefense -= injury;
            if (unit.remainDefense <= 0) GameData.units[index].SetActive(false);    //算子清除
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
            MessageBox.Show("受伤值:" + injury);
            Debug.Log("受伤值为：" + injury);
            afterAttack();
            choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, enemyInTargetHexGrid);
        }
        
        if (currentStage == TurnStage.FightStage)
        {
            StartCoroutine(MainPhase());
            
        }
        // 如果受伤值 = 1 需要把部队改为受伤状态


            // 受伤部队再次受伤，被消灭

    }
    IEnumerator MainPhase()
    {
        string input = LookRuleTable.LookUpFightRuleTable(selectedJoinAttackArmyIndex, enemyInTargetHexGrid);
        yield return new WaitForSeconds(1f);
        var matchA = Regex.Match(input, @"A(?<A>([a-zA-Z]\d)+).*?D");
        var matchD = Regex.Match(input, @"D(?<D>[a-zA-Z0-9]+)");

        //Debug.Log($"A: {matchA.Groups["A"].Value}");
        string capturedValue = matchA.Groups["A"].Value;
        for (int j = 0; j < capturedValue.Length; j += 2)
        {
            string twoChars = capturedValue.Substring(j, 2);
            Debug.Log($"A: {twoChars}");
            MessageBox.Show($"A: {twoChars}");
            bool shouldContinue = true;
            yield return StartCoroutine(FightPhase(twoChars, result => shouldContinue = result));
            if (!shouldContinue)
            {
            Debug.Log("终止MainPhase，因为enemyExist.Count == 0");
            afterAttack();
            choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, enemyInTargetHexGrid);
            fightReady = true;
            MessageBox.Show("战斗结算结束");
            yield break; // 提前退出协程
            }
        }
       
        string capturedValue2 = matchD.Groups["D"].Value;
        for (int j = 0; j < capturedValue2.Length; j += 2)
        {
            string twoChars = capturedValue2.Substring(j, 2);
            Debug.Log($"D: {twoChars}");

            MessageBox.Show($"D: {twoChars}");
            // 执行 DefensePhase
            bool shouldContinue = true;
            yield return StartCoroutine(DefensePhase(twoChars, result => shouldContinue = result));

            if (!shouldContinue)
            {
                Debug.Log("终止MainPhase，因为enemyExist.Count == 0");
                afterAttack();
                choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, enemyInTargetHexGrid);
                fightReady = true;
                MessageBox.Show("战斗结算结束");
                yield break; // 提前退出协程
            }
        }

        afterAttack();
        choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, enemyInTargetHexGrid);
        fightReady = true;

    }
    IEnumerator FightPhase(string c, System.Action<bool> callback)
    {
        isWaitingForLeftClick = true; // 开始等待鼠标左键点击
        bool shouldContinue = true;
        // 获取 EventSystem 和 GraphicRaycaster                              
        EventSystem eventSystem = EventSystem.current;
        GraphicRaycaster graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        // 等待直到左键被按下
        while (isWaitingForLeftClick)
        {
            if (IsPointerOverUIElement(eventSystem, graphicRaycaster))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    yield return new WaitForSeconds(1f); 
                    List<int> enemyExist = new List<int>();
                    List<int> Fightarmy = getChooseFightArmy();
                    for (int k = 0; k < Fightarmy.Count; k++)
                    {
                        int fightIndex = Fightarmy[k];  // 部队编号
                        GameObject unitobject = GameData.units[fightIndex];
                        Unit u = unitobject.GetComponent<Unit>();
                        yield return StartCoroutine(afterFightTable(c, u));
                        if (u.remainDefense <= 0) GameData.units[fightIndex].SetActive(false);    //算子清除
                        for (int j = 0; j < enemyInTargetHexGrid.Count; j++)
                        {
                            if (unitobject.activeSelf) enemyExist.Add(enemyInTargetHexGrid[j]);
                        }
                        choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, enemyExist);
                        if (enemyExist.Count == 0)
                        {
                            shouldContinue = false;
                            isWaitingForLeftClick = false; // 结束等待
                            break; // 退出循环
                        }
                    }
                    isWaitingForLeftClick = false; // 结束等待
                }
            }
            yield return null;
        }

        callback(shouldContinue);
    }


    IEnumerator DefensePhase(string c, System.Action<bool> callback)
    {
        isWaitingForLeftClick = true; // 开始等待鼠标左键点击
        bool shouldContinue = true;
        // 获取 EventSystem 和 GraphicRaycaster                              
        EventSystem eventSystem = EventSystem.current;
        GraphicRaycaster graphicRaycaster = FindObjectOfType<GraphicRaycaster>();

        // 等待直到左键被按下
        while (isWaitingForLeftClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUIElement2(eventSystem, graphicRaycaster))
                {
                        
                        yield return new WaitForSeconds(1f);
                        List<int> enemyExist = new List<int>(); 
                        List<int> Defensearmy = getChooseDefenseArmy();
                        for (int k = 0; k < Defensearmy.Count; k++)
                        {
                            int DefenseIndex = Defensearmy[k];  // 部队编号
                            GameObject unitobject = GameData.units[DefenseIndex];
                            Unit u = unitobject.GetComponent<Unit>();
                            yield return StartCoroutine(afterFightTable(c, u));
                            if (u.remainDefense <= 0) GameData.units[DefenseIndex].SetActive(false);    //算子清除
                            for (int j = 0; j < enemyInTargetHexGrid.Count; j++)
                            {
                            if (unitobject.activeSelf) enemyExist.Add(enemyInTargetHexGrid[j]);
                            }
                            choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, enemyExist);
                            if (enemyExist.Count == 0)
                            {
                                shouldContinue = false;
                                isWaitingForLeftClick = false; // 结束等待
                                break; // 退出循环
                            }
                    }
                        isWaitingForLeftClick = false; // 结束等待
                    }
                    
                }
                yield return null;
        }
        callback(shouldContinue);
        // 在这里执行需要在左键点击后执行的代码
    }


    bool IsPointerOverUIElement(EventSystem eventSystem, GraphicRaycaster graphicRaycaster)      //进行DG,L,O选择时确保点击在ChooseGUI里面
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.GetComponent<FightArmy>() != null)
            {
                return true; // 检测到 FightArmy 组件
            }
        }

        return false; // 未检测到 FightArmy 组件
    }

    bool IsPointerOverUIElement2(EventSystem eventSystem, GraphicRaycaster graphicRaycaster)      //进行DG,L,O选择时确保点击在ChooseGUI里面
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.GetComponent<DefenseArmy>() != null)
            {
                return true; // 检测到 DefenseArmy 组件
            }
        }

        return false; // 未检测到 DefenseArmy 组件
    }
    IEnumerator afterFightTable(string twoChars,Unit u)
    {
        int n = int.Parse(twoChars[1].ToString());
        if (twoChars[0] == 'L')
        {
            u.remainDefense -= n;
            if (u.remainDefense <= 0) StopCoroutine(afterFightTable(twoChars, u));    //算子清除
        }
        if (twoChars == "DG")
        {
            if (u.remainDefense <= 0) StopCoroutine(afterFightTable(twoChars, u));
            if (u.fullState)
            {
                Counter.counter[u.counterID].attackInjure /= 2;
                Counter.counter[u.counterID].moveAllowInjure /= 2;
            }
            else
            {
                Counter.counter[u.counterID].attack /= 2;
                Counter.counter[u.counterID].moveAllow /= 2;
            }
        }
        if(twoChars[0] == 'o')
        {
            if (u.remainDefense <= 0) StopCoroutine(afterFightTable(twoChars, u));
            yield return StartCoroutine(WaitForNumericInput(u,twoChars[1]));

        }
    }

    IEnumerator WaitForNumericInput(Unit u,char c)
    {
        MessageBox.Show("等待键盘输入数字:0是后退，1是损失");
        int n = int.Parse(c.ToString());
        bool judge = true;
        // 循环等待直到输入有效的数字键
        while (judge)
        {
   
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    Debug.Log("输入数字:0 " + "往后退");
                    // 在检测到输入后等待0.1秒
                    yield return new WaitForSeconds(1f);
                    yield return StartCoroutine(WaitForRightClickInput(n, u));
                // 数字输入后继续执行后续代码
                judge = false;
                yield break;
                }
                
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Debug.Log("输入数字:1 " + "损失");
                    yield return new WaitForSeconds(1f);
                    u.remainDefense -= n;
                // 数字输入后继续执行后续代码
                    judge = false;
                    yield break;
                }
            // 等待下一帧
            yield return null;
        }
    }

    IEnumerator WaitForRightClickInput(int n,Unit u)
    {
        bool isWaitingForRightClick = true; // 开始等待鼠标右键点击
        backAction = true;
        for (int i = 0; i < n; i++)
        {
        while (isWaitingForRightClick)  // 等待直到右键被按下
        {
                if (Input.GetMouseButtonDown(1))
                {
                    yield return new WaitForSeconds(1f);
                    backenemyArmyGrid = enemyArmyGrid;
                    List<int> hexArmy = GameData.hexGrid[backenemyArmyGrid.x, backenemyArmyGrid.y].GetComponent<HexGrid>().getHexArmy();
                    if (hexArmy.Count != 0)
                    {
                        
                        if (BackJudgeIsEnemy(u) == true)
                        {
                            ErrorMessageBox.Show("有敌军，无法移动");
                            isWaitingForRightClick = true;
                            yield return null;
                            continue;
                        }
                    }
                        if (Utils.JudgeBackDirection(backenemyArmyGrid, u) == -1 )
                        {
                            ErrorMessageBox.Show("算子只能后退");
                            isWaitingForRightClick = true;
                            yield return null;
                            continue;
                    }
                    u.UpdateUnitPosition(backenemyArmyGrid);
                    
                    isWaitingForRightClick = false;
                }
                else yield return null;
            }
            isWaitingForRightClick = true;
        }
    }
    public void afterAttack()
    {
        for (int i = 0; i < selectedJoinAttackArmyIndex.Count; i++)                       //左上角的GUI显示是否发起攻击
        {
            int index = selectedJoinAttackArmyIndex[i];
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            attackReady = true;
            unit.alreadyRaiseAttack = attackReady;
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
        }

        selectedJoinAttackArmyIndex.Clear();   // 清除进攻队列
        hideAllYellowBorder();  // 隐藏黄色格子。
        enemyArmyGrid.x = -1;   // 赋值成为一个不可能的格子
    }


}
