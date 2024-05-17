using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnFlow
{
    public enum TurnStage
    {
        StageReinforement = 0,          // 增援阶段
        StageMove = 1,                  // 移动阶段
        AirforceStageAttack = 2,        // 空军进攻阶段
        ReactionStage = 3,              // 反应阶段
        FightStage = 4                 // 战斗阶段
    }

    public enum TurnPlayer
    {
        PlayerChinese = 0,        // 志愿军与北朝鲜军
        PlayerAmerica = 1,        // 美军与南韩军
    }

    public TurnPlayer turnPlayer;       // 当前回合 是哪一方玩家的行动阶段（德国 或 英美联军）v
    public TurnStage turnStage;         // 当前回合 处于哪一行动阶段（移动、进攻等）
    GameObject movePanel;
    GameObject attackPanel;
    GameObject choosePanel;
    public TurnFlow()
    {
        movePanel = GameObject.Find("OperationMoveStagePanel");
        attackPanel = GameObject.Find("OperationAttackStagePanel");
        choosePanel = GameObject.Find("ChooseBox");
    }

    public void GoIntoMoveStage()
    {
        GameData.turnflow.turnStage = TurnFlow.TurnStage.StageMove;   //  进入移动阶段
        OperationGUI.ShowGameObject(movePanel);
        OperationGUI.HideGameObject(attackPanel);
        ChooseGUI.HideGameObject(choosePanel);
        MessageBox.Show("移动阶段");
    }
    public void GoIntoAirforceStageAttack()
    {
        GameData.turnflow.turnStage = TurnFlow.TurnStage.AirforceStageAttack;   //  进入进攻阶段
        OperationGUI.HideGameObject(movePanel);
        OperationGUI.ShowGameObject(attackPanel);
        ChooseGUI.ShowGameObject(choosePanel);
        MessageBox.Show("空军射击阶段");

    }
    public void GoIntoReactionStage()
    {

        GameData.turnflow.turnStage = TurnFlow.TurnStage.ReactionStage;   //  进入反应阶段
        OperationGUI.HideGameObject(movePanel);
        OperationGUI.ShowGameObject(attackPanel);
        MessageBox.Show("反应阶段（对手）");
    }
    public void GoIntoFightStage()
    {
        GameData.turnflow.turnStage = TurnFlow.TurnStage.FightStage;   //  进入进攻阶段 2nd
        OperationGUI.HideGameObject(movePanel);  
        OperationGUI.ShowGameObject(attackPanel);
        ChooseGUI.ShowGameObject(choosePanel);
        MessageBox.Show("战斗阶段");
    }
    public void GoChangeTurnPlayer()
    {
        // 变更 “当前玩家”
        if (turnPlayer == TurnPlayer.PlayerChinese) turnPlayer = TurnPlayer.PlayerAmerica;
        else turnPlayer = TurnPlayer.PlayerChinese;

    }
}
