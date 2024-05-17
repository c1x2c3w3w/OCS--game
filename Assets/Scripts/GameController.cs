
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
    public GameObject hexGridPrefab;  // ���Ǹ������prefab 
    public GameObject unitPrefab;

    public int mouseLeftClickCoolDown = 0;  // �������������Ҫ��һ������ȴʱ�䡣��update�и��¡�
    public int mouseRightClickCoolDown = 0; // ����Ҽ��������Ҫ��һ������ȴʱ�䡣��update�и��¡�
    public int mouseClickCoolDown = 0;
    bool isWaitingForLeftClick = true; 

    int[] selectedArmyIndex;                // ��¼��ǰ���Ͻ�GUI����ѡ���Ĳ��ӵı��
    int[] choooseArmyIndex;                 // ��¼��������GUI�Ĳ��ӱ��
    List<int> selectedJoinAttackArmyIndex;  // ��¼��ѡ���������Ͻ����Ĳ��ӱ��
    List<int> selectedJoinchoooseArmyIndex;
    Vector2Int enemyArmyGrid;               // �������ĵ��˸��ӵ����꣨�С��У�

    Vector2Int moveCurrentRowcol;           // ��¼�ƶ������У����������Ǹ����ӣ�Ҫ�ƶ��Ĳ��ӵĸ��ӣ���
    List<int> armyInTargetHexGrid;          // ��¼�ƶ��������Ҽ������ʱ��Ҫȥ��Ŀ������ϵĲ��ӱ��
    public static MapHexElement targetHex;  // ��¼�ƶ��������Ҽ������ʱ��Ҫȥ��Ŀ����ӵĵ���Ԫ��

    public bool stratageMovingState;        // ��¼�ƶ��������Ҽ������ʱ,ս���ƶ�ģʽ״̬  true ��false
    public bool combatState;                // ս��ģʽ״̬
    public bool movingState;                // �ƶ�ģʽ״̬

    public int[] moveCount;                  // �ж�ת���ƶ�ģʽ֮ǰ�Ƿ��ƶ�
    public bool moveReady = false;           // �жϱ��غ��Ƿ��ƶ�
    public bool attackReady = false;          // �жϱ��غ��Ƿ񹥻�
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
        enemyArmyGrid.x = -1;   // ����δѡ���κθ�������ִ�н���


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
        // ��ʽ�׶� �Ĵ����ʼ������
        GameData.turnflow.GoIntoMoveStage();

    }
    private void globalSetup()
    {
        GameData.hexGrid = new GameObject[36, 64];//��ͼ36�У�64��
        for (int i = 0; i < 36; i++)
            for (int j = 0; j < 64; j++)
            {
                Vector3 pos = getHexPosition(i, j);
                GameData.hexGrid[i, j] = Instantiate(hexGridPrefab, pos, transform.rotation);//�����µ����Ǹ�
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
        GameObject newUnit;  // �½�һ��unit  �������return��
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
        float gridWidth = 0.9495f; //���ӿ�
        float gridHeight = gridWidth * 1.155f; //���Ӹ߶ȸ��ݵ�ͼ�Ŀ�͸ߵı�Ϊ��3��2*sqrt(3)
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
            // �����Ǵ�collider�л�ȡ�ض����������������е�����ȷʵ����������˵���������
            HexGrid hex = hit.collider.GetComponent<HexGrid>();
            if (hex)
            {
                Vector3 point = hit.point;
                hideAllRedBorder();
                Vector2Int rowcol = GameData.getPosOfObject(hex.name);

                // ��ȡ������ʱ���С���
                moveCurrentRowcol = rowcol;
                GameData.hexGrid[rowcol.x, rowcol.y].GetComponent<HexGrid>().onSelected();
   
                // ���еĸ����У����в��ӣ������Ͻ�GUI����ʾ�������ӡ�
                selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(rowcol);
               
                // ս���ƶ�ģʽ,ս��ģʽ״ֵ̬��� false
                stratageMovingState = false;
                combatState = false;
                movingState = true;
            }
            FightArmy f = hit.collider.GetComponent<FightArmy>();
            if (f)
            {
                choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, armyInTargetHexGrid);

            }
        }
    }

    void HandleInput_RightMouse()
    {
        // �����λ�÷���һ�����ߣ��������ܻ������壬�����жϻ���ʲô���塣
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ʹ������ϵͳ���������ߣ����е��������hit����;  ��벿�������Ǵ��� �����GUI�ؼ��Ϸ������
        if (Physics.Raycast(inputRay, out hit) && !EventSystem.current.IsPointerOverGameObject())
        {
            // �����Ǵ�collider�л�ȡ�ض����������������е�����ȷʵ����������˵���������
            HexGrid hex = hit.collider.GetComponent<HexGrid>();
            if (hex)    // ����ΪNULL��˵�����е��ǡ���HexGrid�����������
            {
                //Vector3 point = hit.point;     // ��û��е�λ��
               
                
                    Vector2Int rowcol = GameData.getPosOfObject(hex.name);
                    // �õ�Ŀ������ϵĲ��ӱ��
                    armyInTargetHexGrid = hex.getHexArmy();

                    // ���Ŀ����ӵĵ���Ԫ����Ϣ
                    targetHex = MapDataBase.Instance.SetHex(rowcol.x, rowcol.y);
                    
                    PerformRightClick(rowcol);
                

            }
        }

    }

    void PerformRightClick(Vector2Int rowcol)
    {
        // ��ȡ��ǰ�� ���غϽ׶Ρ�
        TurnStage currentStage = GameData.turnflow.turnStage;

        // �������Ԯ�׶Σ�ֱ���˳�
        if (currentStage == TurnStage.StageReinforement) return;

        // ����ǽ����׶�
        if(currentStage == TurnStage.AirforceStageAttack || currentStage == TurnStage.ReactionStage || currentStage == TurnStage.FightStage){
            for (int i = 0; i <= 18; i++)
            {
                moveCount[i] = 0;
            }
            
            // Ҫ�ж�һ�����������
            if(selectedJoinAttackArmyIndex.Count != 0)
            {
                ErrorMessageBox.Show("���󣺴�ǰ�Ľ����ж���δ���ִ�С�");
                return;
            }

            // Ҫ�жϱ�����ĸ����Ƿ��в���
            List<int> hexArmy = GameData.hexGrid[rowcol.x, rowcol.y].GetComponent<HexGrid>().getHexArmy();
            if (hexArmy.Count == 0)  // ˵��������û�в���
            {
                Debug.Log("countΪ��" + hexArmy.Count);
                ErrorMessageBox.Show("���󣺸�����û�в��ӣ��޷������ø��ӡ�");
                return;
            }
            else
            {
                // ����Ҫ�жϱ�����ĸ����Ƿ��� �����ˡ��Ĳ���

            }

            // ���������㣬��������ео��������������ѵ�ǰ����ʾ��ɫ�߿򣬲���¼���ӡ�
            hideAllYellowBorder();
            GameData.hexGrid[rowcol.x, rowcol.y].GetComponent<HexGrid>().onAttackGridSelected();
            enemyArmyGrid = rowcol;   // ��¼�������ĸ���



        }

        // ������ƶ��׶�
        if(currentStage == TurnStage.StageMove)
        {
            if (CanMoveArmy(rowcol))    // ִ�в����ƶ��ж�
            {

                // �����ƶ������Ҹ���λ��
                Execute_Move_Action(rowcol);
                selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(rowcol);


                // �ƶ���Ŀ��λ����ʾ���
                hideAllRedBorder();
                GameData.hexGrid[rowcol.x, rowcol.y].GetComponent<HexGrid>().onSelected();

                // ����"��ǰ����"
                moveCurrentRowcol = rowcol;

            }
        }
    }

     bool CanMoveArmy(Vector2Int targetRowcol)
    {
        // �����жϵ�ǰ�Ƿ�Ϊ���ƶ��׶Ρ�, �����ƶ��׶���ֱ���˳�

        // �ж��Ƿ����ѡ���˲���
        if (selectedArmyIndex[0] == -1)
        {
            ErrorMessageBox.Show("��ѡ�񲿶�");
            return false;
        }
        // �ж��ƶ���Ŀ����Ƿ���ԭ��������
        if (Utils.JudgeDirection(moveCurrentRowcol, targetRowcol) == -1)
        {
            ErrorMessageBox.Show("���ƶ������ڸ�");
            return false;
        }
        // �ж��Ƿ�Ϊ�յ�
        if(armyInTargetHexGrid.Count != 0)
        {

            // �ж��Ҽ�����ĸ��ӣ��Ƿ���ڵо�����ֱ���˳�����������Լ��Ĳ��ӣ���ʾ�����赲�޷��ƶ���
            if (JudgeIsEnemy())
            {
                ErrorMessageBox.Show("���赲�޷��ƶ�");
                return false;
            }
        }

        // �ж�����������Ҫ�жϵ����������̹����ɽ�ص��Σ����ƶ�·�߱����е�·��
        
        int direction = Utils.JudgeDirection(targetRowcol, moveCurrentRowcol);
        //Debug.Log("targetHex.roads[direction]" + targetHex.roads[direction]);
        if (IsContainsTankArmy())
        {
            
            if (targetHex.terrain == 5)
            {

                if(targetHex.roads[direction] == 0)
                {
                    ErrorMessageBox.Show("̹�˲����޷��ƶ���Ŀ�����");
                    return false;
                }
            }
        }

        // �������ж�ʣ����ƶ�����MovingPoint �Ƿ��㹻�ƶ�����һ�񣩣�������ʾ���ƶ������㡱
        // ����ע���Ƿ��ǡ�ս���ƶ�ģʽ���̶��ĸ��ƶ���������
        List<int> armyToMove = getActionArmy();
        for(int i = 0; i < armyToMove.Count; i++)
        {
            if (armyToMove[i] < 0) continue;
            Unit unit = GameData.units[armyToMove[i]].GetComponent<Unit>();

            //�жϵ�ǰunit�ǲ���̹��
            int index = armyToMove[i];
            GameObject unitObject = GameData.units[index];
            Unit units = unitObject.GetComponent<Unit>();
            int countId = units.counterID;
            bool containTank = false;
            if(moveCount[countId] == 2)
            {
                ErrorMessageBox.Show("�ѽ����ƶ���");
                return false;
            }
            moveCount[countId] = 1;                            //��ʾ�ƶ���

            if (Counter.counter[countId].type == 2)
            {
                containTank = true;
            }
            // �ж��ƶ����Ƿ����

            int currentMP = unit.movingPoint;
            int needed = MovingPointsNeeded(targetRowcol, containTank, currentMP);

            //Debug.Log("��Ҫ���ƶ���" + needed);

            if (needed > currentMP)    // �ƶ�������
            {
                ErrorMessageBox.Show("�ƶ�������");
                return false;
  
            }

        }

        return true;
    }

    bool JudgeIsEnemy()
    {
        // ��ǰ���ӵĲ��ӱ��
        int currentHex_CounterType = GameData.units[selectedArmyIndex[0]].GetComponent<Unit>().counterID;
        int currentHex_armyNation = Counter.counter[currentHex_CounterType].nation;
        // Ŀ����ӵĲ��ӱ��
        int targetHex_CounterType = GameData.units[armyInTargetHexGrid[0]].GetComponent<Unit>().counterID;
        int targetHex_armyNation = Counter.counter[targetHex_CounterType].nation;
        // �����ǰ�����Ϻ�Ŀ������ϵĲ��ӱ�Ŷ���0��Ҳ����־Ը��
        if (currentHex_armyNation == 0  && currentHex_armyNation == targetHex_armyNation)
            return false;
        // �����ǰ�����Ϻ�Ŀ������ϵĲ��ӱ�Ŷ�����0��Ҳ����������
        else if (currentHex_armyNation != 0 && targetHex_armyNation != 0)
            return false;
        // ����������������� ��Ŀ������ǵо�
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


    int MovingPointsNeeded(Vector2Int targetRowcol,bool containTank,int currentMP)  // ���� �ӵ�ǰ�����Ŀ�����Ҫ�����ƶ���
    {
        int movingPoints = 0;
        // ���Ŀ����뵱ǰ������ı�
        int targetHexDirection = Utils.JudgeDirection(moveCurrentRowcol,targetRowcol);
        if (targetHexDirection == -1) return movingPoints;
        // ������Ϣ
        // ����

        switch (targetHex.rivers[targetHexDirection])
        {
            
            case 0: 
                movingPoints += 0;break;
            case 1:
                if (containTank)   //�ж���̹�˻��ǲ���
                {
                    movingPoints += 3; break;
                }
                else { movingPoints += 5; break; }

        }
        // ��·
        switch (targetHex.roads[targetHexDirection])
        {
            case 0:
                {
                    //����
                    switch (targetHex.terrain)
                    {
                        case 1: 
                        case 2: movingPoints += 1; break;
                        case 3:
                            if (containTank) //�ж��Ƿ�Ϊ̹��
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
                                int m = currentMP - movingPoints; //���������������ƶ���
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
        List<int> armyToMove = getActionArmy();   // ѡ�������еı�ѡ�еĲ���
        for (int i = 0; i < armyToMove.Count; i++)
        {
            int index = armyToMove[i];  // ���ӱ��
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            Unit units = GameData.units[armyToMove[i]].GetComponent<Unit>();
            // �޸Ĳ��ӵ�λ��
            unit.UpdateUnitPosition(targetRowcol);
            // ֧���ƶ�����
            bool containTank = false;
            if (Counter.counter[unit.counterID].type == 2)
            {
                containTank = true;
            }
            int currentMP = units.movingPoint;
            int pointNeeded = MovingPointsNeeded(targetRowcol, containTank, currentMP);
            unit.useMovingPoint(pointNeeded);
        }
        }

    public List<int> getActionArmy()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            GameObject armyIcon = GetComponent<OperationGUI>().getArmyIconSelectedFromIndex(i);
            GameObject redRectBorder = Utils.getChildObject(armyIcon, "RedRectBorder");

            if (redRectBorder.GetComponent<RedRectBorder>().isSelected)
                list.Add(selectedArmyIndex[i]);
        }
        return list;
    }

    public List<int> getChooseFightArmy()           //��ȡ��ѡ�������е����ӱ��
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 10; i++)
        {
            GameObject armyIcon2 = GetComponent<ChooseGUI>().getFightIconSelectedFromIndex(i);
            GameObject redRectBorder2 = Utils.getChildObject(armyIcon2, "RedSquareBorder");

            Debug.Log("��Χ�ģ�"+ armyIcon2 + redRectBorder2.GetComponent<RedRectBorder>().isSelected2);
            if (redRectBorder2.GetComponent<RedRectBorder>().isSelected2)list.Add(choooseArmyIndex[i]);
           
        }
 
        return list;
    }

    public List<int> getChooseDefenseArmy()      //��ȡ��ѡ���ض��е����ӱ��
    {
        List<int> list = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            GameObject armyIcon = GetComponent<ChooseGUI>().getDenfenseIconSelectedFromIndex(i);
            GameObject redRectBorder = Utils.getChildObject(armyIcon, "RedSquareBorder");

            if (redRectBorder.GetComponent<RedRectBorder>().isSelected2)
            {
                list.Add(choooseArmyIndex[i+ selectedJoinAttackArmyIndex.Count]);
            }

        }
        return list;
    }
    //�������и�ı߿�
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
            
            List<int> armyToMove = getActionArmy();   // ѡ�������еı�ѡ�еĲ���
            for (int i = 0; i < armyToMove.Count; i++)
            {
                int index = armyToMove[i];  // ���ӱ��
                GameObject unitObject = GameData.units[index];
                Unit unit = unitObject.GetComponent<Unit>();
                
                if (moveCount[unit.counterID] == 1)
                {
                    ErrorMessageBox.Show("���ƶ���");
                    combatState = !combatState;
                    movingState = !movingState;
                    return;
                }
                else if (moveCount[unit.counterID] == 2)
                {
                    ErrorMessageBox.Show("�ѽ����ƶ���");
                    combatState = !combatState;
                    movingState = !movingState;
                    return;
                }
                unit.fullState = combatState;
                unit.movingPoint = unit.fullState ? Counter.counter[unit.counterID].moveAllowInjure : Counter.counter[unit.counterID].moveAllow;
            }
            MessageBox.Show(combatState ? "ս��ģʽ����" : "�ƶ�ģʽ����");
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
        }
        else{
            MessageBox.Show("δ����ս��ģʽ");

        }
    }

    public void OnBtnChangeToStrategyMove()
    {
              
        // ս���ƶ�״̬����  ȡ���� ������ʾ��ʾ�� 
        stratageMovingState = !stratageMovingState;
        // ѡ�������еı�ѡ�еĲ���
        List<int> armyToMove = getActionArmy();
        for (int i = 0; i < armyToMove.Count; i++)
        {
            int index = armyToMove[i];  // ���ӱ��
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            unit.strategyMove = stratageMovingState;
            // ��������Ѿ������ƶ���������û�н����ƶ�������ô������ı䡰ս���ƶ�״̬��������
            if (moveCount[unit.counterID] == 1)
            {
                ErrorMessageBox.Show("���ƶ���");
                stratageMovingState = !stratageMovingState;
                return;
            }
            else if (moveCount[unit.counterID] == 2)
            {
                ErrorMessageBox.Show("�ѽ����ƶ���");
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
       
        MessageBox.Show(stratageMovingState ? "ս���ƶ�����" : "��ͨ�ƶ�ģʽ");
        selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
    }

    public void OnBtnEndCurrentArmyMove()
    {
        List<int> armyToMove = getActionArmy();
        for (int i = 0; i < armyToMove.Count; i++)
        {
            int index = armyToMove[i];  // ���ӱ��
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            moveCount[unit.counterID] = 2;
            moveReady = true;
            unit.finishTurnMoved = moveReady;
            MessageBox.Show("�ѽ�����ǰ�ƶ�");
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

    }
    public void OnBtnAddToAttackList()
    {
        // �����ûѡ��Ҫ�������ĸ���
        if(enemyArmyGrid.x == -1){
            ErrorMessageBox.Show("����δѡ�н������ӡ�");
            return;
        }
        // �ѵ�ǰ����� �ִ���ѡ��״̬�Ĳ��ӣ����뵽 ���������ӵĶ��С�

        List<int> armyToAttack = getActionArmy();
        if (armyToAttack.Count == 0) return;
        bool canAdd = CanAddToAttackList(enemyArmyGrid, armyToAttack);
        if (canAdd)
        {
            for (int i = 0; i < armyToAttack.Count; i++)
            {
                selectedJoinAttackArmyIndex.Add(armyToAttack[i]);
                MessageBox.Show("����ӵ���������");
            }

        }
        else return;
        for (int i = 0; i < selectedJoinAttackArmyIndex.Count; i++)
        {
            Debug.Log("����ӵĶ��У���" + selectedJoinAttackArmyIndex[i]);
        }
        choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, armyInTargetHexGrid);
    }
    public void OnBtnClearAttackList()
    {
        // �����ǰ�� ���������ӵĶ��С�
        selectedJoinAttackArmyIndex.Clear();
        MessageBox.Show("���������");
    }
    public void OnBtnAttack()
    {
        // ִ�н���
        // ������������в��ӣ��ǡ��������ӵĶ��С��е����в���
        // �������Ĳ��ӣ���enemyArmyGrid�����ĸ��е����ео�����



        // ����������
        if (CanAttackArmy(enemyArmyGrid))
        {
            Execute_Attack_Action(enemyArmyGrid);
        }
        // �������������ɺ�
        
    }
    public void OnBtnEndAttackStage()
    {
        TurnStage currentStage = GameData.turnflow.turnStage;
        if (currentStage == TurnStage.FightStage)
        {
            List<int> armyToMove = getActionArmy();
            for (int i = 0; i < armyToMove.Count; i++)
            {
                int index = armyToMove[i];  // ���ӱ��
                GameObject unitObject = GameData.units[index];
                Unit unit = unitObject.GetComponent<Unit>();
                moveReady = false;
                unit.finishTurnMoved = moveReady;             //�жϱ��غ��Ƿ����ƶ�
            }
        selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
        }
        if (GameData.turnflow.turnStage == TurnFlow.TurnStage.AirforceStageAttack)
        {
            GameData.turnflow.GoIntoReactionStage();
        }
        else if (GameData.turnflow.turnStage == TurnFlow.TurnStage.ReactionStage)
        {
            GameData.turnflow.GoIntoFightStage();
        }
        else if (GameData.turnflow.turnStage == TurnFlow.TurnStage.FightStage)
        {
            GameData.turnflow.GoIntoMoveStage();
        }
        

    }

    bool CanAddToAttackList(Vector2Int enemyArmyGrid, List<int> armyToAttack)
    {

       
        if (JudgeIsEnemy() == false)                                  //��ͬ�����������
        {
            ErrorMessageBox.Show("�Ѿ��޷���Ӷ���");
            return false;
        }
        for (int i = 0; i < armyToAttack.Count; i++)                 
        {
            int index = armyToAttack[i];
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            int countId = unit.counterID;
            if (unit.alreadyRaiseAttack == true)               //���غ��Ѿ��������Ĳ����޷����
            {
                ErrorMessageBox.Show("�Ѿ�������������޷����");
                return false;
            }

            if (unit.strategyMove == true)                   //�ж�ÿ��Ҫ�����������û�д���ս��ģʽ���޷�����
            {
                ErrorMessageBox.Show("ս��ģʽ�޷���Ӷ���");
                return false;
            }
            if (Counter.counter[countId].type == 1 || Counter.counter[countId].type == 3)                  //����������Χ���޷���ӣ��ڡ��ɻ�
            {
                if (Utils.JudgeDistance(moveCurrentRowcol, enemyArmyGrid) > Counter.counter[countId].attackArea)
                {
                    ErrorMessageBox.Show("������Χ���޷���Ӷ���");
                    return false;
                }

            }
            else
            {
                if (Utils.JudgeDirection(moveCurrentRowcol, enemyArmyGrid) == -1) // �ж��Ƿ����ڸ�
                {
                    ErrorMessageBox.Show("������Χ���޷���Ӷ���");
                    return false;
                }
            }

        }

        for (int i = 0; i < selectedJoinAttackArmyIndex.Count; i++)
        {     
            for (int j = 0; j < armyToAttack.Count; j++)
            {
                if(selectedJoinAttackArmyIndex[i] == armyToAttack[j] )                //���ڹ���������Ĳ����ظ����
                {
                    ErrorMessageBox.Show("���ڹ���������");
                    return false;
                }
            }
        }
     return true;
    }

    bool CanAttackArmy(Vector2Int rowcol)  // �жϲ����Ƿ���Թ���
    {
        if (enemyArmyGrid.x == -1)
        {
            ErrorMessageBox.Show("����δѡ�н������ӡ�");
            return false;
        }
        return true;
    }

    void Execute_Attack_Action(Vector2Int rowcol)
    {
        int injury = 0;      // ����ֵ
        int i = Random.Range(0, armyInTargetHexGrid.Count);                                //���ѡ��һ����������
        int index = armyInTargetHexGrid[i];
        GameObject unitObject = GameData.units[index];
        Unit unit = unitObject.GetComponent<Unit>();
        TurnStage currentStage = GameData.turnflow.turnStage;
        // ���չ�����в�����
        if (currentStage == TurnStage.AirforceStageAttack) //�վ�����׶�
        {
            string injure;
            injure = LookRuleTable.LookUpRuleTable(selectedJoinAttackArmyIndex, armyInTargetHexGrid);
            if (injure == "0") injury = 0;

            if (injure == "1/2")
            {

                int ra = Random.Range(1, 7);
                Debug.Log("Ͷ������" + ra);
                if (ra >= 4 && ra <= 6)
                {
                    injury = 1;
                    injure = "DG";
                }
                else injure = "DG";

            }

            if (injure == "DG")                                                    //DG״̬ս��ֵ���ƶ�ֵ����
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
            if (unit.remainDefense <= 0) GameData.units[index].SetActive(false);    //�������
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
            MessageBox.Show("����ֵ:" + injury);
            Debug.Log("����ֵΪ��" + injury);
        }
        
        if (currentStage == TurnStage.FightStage)
        {
            //MatchCollection matches = Regex.Matches(LookRuleTable.LookUpFightRuleTable(selectedJoinAttackArmyIndex, armyInTargetHexGrid), @"(?<=A)(\w+)(\d+)|(?<=D)(\w+)(\d+)");

            StartCoroutine(MainPhase());

                //Debug.Log($"D: {matchD.Value.Substring(1)}");



        }
        // �������ֵ = 1 ��Ҫ�Ѳ��Ӹ�Ϊ����״̬


            // ���˲����ٴ����ˣ�������

    }
    IEnumerator MainPhase()
    {
        string input = LookRuleTable.LookUpFightRuleTable(selectedJoinAttackArmyIndex, armyInTargetHexGrid);
        var matchA = Regex.Match(input, @"A(?<A>([a-zA-Z]\d)+).*?D");
        var matchD = Regex.Match(input, @"D(?<D>[a-zA-Z0-9]+)");

        //Debug.Log($"A: {matchA.Groups["A"].Value}");
        string capturedValue = matchA.Groups["A"].Value;
        for (int j = 0; j < capturedValue.Length; j += 2)
        {
            string twoChars = capturedValue.Substring(j, 2);
            Debug.Log($"A: {twoChars}");
            MessageBox.Show($"A: {twoChars}");
            yield return StartCoroutine(FightPhase(twoChars));

        }
        // ִ�� FightPhase
        string capturedValue2 = matchD.Groups["D"].Value;
        for (int j = 0; j < capturedValue2.Length; j += 2)
        {
            string twoChars = capturedValue2.Substring(j, 2);
            Debug.Log($"D: {twoChars}");

            MessageBox.Show($"D: {twoChars}");
            // ִ�� DefensePhase
            yield return StartCoroutine(DefensePhase(twoChars));
           

        }

        afterAttack();
        choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, armyInTargetHexGrid);
    }
    IEnumerator FightPhase(string c)
    {
        isWaitingForLeftClick = true; // ��ʼ�ȴ����������
        // �ȴ�ֱ�����������
        while (isWaitingForLeftClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                yield return new WaitForSeconds(1f); ;
                List<int> Fightarmy = getChooseFightArmy();
                Debug.Log("����" + Fightarmy.Count);

                for (int k = 0; k < Fightarmy.Count; k++)
                {
                    int fightIndex = Fightarmy[k];  // ���ӱ��
                    GameObject unitobject = GameData.units[fightIndex];
                    Unit u = unitobject.GetComponent<Unit>();

                    afterFightTable(c, u);
                    if (u.remainDefense <= 0) GameData.units[fightIndex].SetActive(false);    //�������

                }

                isWaitingForLeftClick = false; // �����ȴ�
            }
            else yield return null;
        }
        
        choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, armyInTargetHexGrid);
        Debug.Log("Left mouse button is pressed. Continuing with the rest of the code...");
        // ������ִ����Ҫ����������ִ�еĴ���
    }


    IEnumerator DefensePhase(string c)
    {
        isWaitingForLeftClick = true; // ��ʼ�ȴ����������
        // �ȴ�ֱ�����������
        while (isWaitingForLeftClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("�ڶ����������������������");
                yield return new WaitForSeconds(1f); 
                List<int> Defensearmy = getChooseDefenseArmy();
                for (int k = 0; k < Defensearmy.Count; k++)
                {
                    int DefenseIndex = Defensearmy[k];  // ���ӱ��
                    GameObject unitobject = GameData.units[DefenseIndex];
                    Unit u = unitobject.GetComponent<Unit>();
                    afterFightTable(c, u);
                    if (u.remainDefense <= 0) GameData.units[DefenseIndex].SetActive(false);    //�������
                    Debug.Log("���ӱ��" + Defensearmy[k]);

                }
                isWaitingForLeftClick = false; // �����ȴ�
            }
            else yield return null;
        }
        choooseArmyIndex = GetComponent<ChooseGUI>().showChooseArmy(selectedJoinAttackArmyIndex, armyInTargetHexGrid);
        Debug.Log("Left mouse button is pressed. Continuing with the rest of the code...");
        // ������ִ����Ҫ����������ִ�еĴ���
    }


    public void afterFightTable(string twoChars,Unit u)
    {
        int n = int.Parse(twoChars[1].ToString());
        if(twoChars[0] == 'L') u.remainDefense -= n; 
        if(twoChars == "DG")
        {
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
            StartCoroutine(WaitForNumericInput(u,twoChars[1]));
        }
    }

    IEnumerator WaitForNumericInput(Unit u,char c)
    {
        Debug.Log("�ȴ�������������...");
        int n = int.Parse(c.ToString());

        // ѭ���ȴ�ֱ��������Ч�����ּ�
        while (true)
        {
   
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    Debug.Log("��������:0 " + "������");

                    // �ڼ�⵽�����ȴ�0.1��
                    yield return new WaitForSeconds(1f);

                    // ������������ִ�к�������
                   
                    yield break;
                }
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Debug.Log("��������:1 " + "��ʧ");

                    // �ڼ�⵽�����ȴ�0.1��
                    yield return new WaitForSeconds(1f);
                    u.remainDefense -= n;
                    // ������������ִ�к�������

                    yield break;
                }
            

            // �ȴ���һ֡
            yield return null;
        }
    }
    public void afterAttack()
    {
        for (int i = 0; i < selectedJoinAttackArmyIndex.Count; i++)                       //���Ͻǵ�GUI��ʾ�Ƿ��𹥻�
        {
            int index = selectedJoinAttackArmyIndex[i];
            GameObject unitObject = GameData.units[index];
            Unit unit = unitObject.GetComponent<Unit>();
            attackReady = true;
            unit.alreadyRaiseAttack = attackReady;
            selectedArmyIndex = GetComponent<OperationGUI>().showArmyInSelectdGrid(moveCurrentRowcol);
        }

        selectedJoinAttackArmyIndex.Clear();   // �����������
        hideAllYellowBorder();  // ���ػ�ɫ���ӡ�
        enemyArmyGrid.x = -1;   // ��ֵ��Ϊһ�������ܵĸ���
    }


}
