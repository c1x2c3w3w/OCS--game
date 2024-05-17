using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnFlow
{
    public enum TurnStage
    {
        StageReinforement = 0,          // ��Ԯ�׶�
        StageMove = 1,                  // �ƶ��׶�
        AirforceStageAttack = 2,        // �վ������׶�
        ReactionStage = 3,              // ��Ӧ�׶�
        FightStage = 4                 // ս���׶�
    }

    public enum TurnPlayer
    {
        PlayerChinese = 0,        // ־Ը���뱱���ʾ�
        PlayerAmerica = 1,        // �������Ϻ���
    }

    public TurnPlayer turnPlayer;       // ��ǰ�غ� ����һ����ҵ��ж��׶Σ��¹� �� Ӣ��������v
    public TurnStage turnStage;         // ��ǰ�غ� ������һ�ж��׶Σ��ƶ��������ȣ�
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
        GameData.turnflow.turnStage = TurnFlow.TurnStage.StageMove;   //  �����ƶ��׶�
        OperationGUI.ShowGameObject(movePanel);
        OperationGUI.HideGameObject(attackPanel);
        ChooseGUI.HideGameObject(choosePanel);
        MessageBox.Show("�ƶ��׶�");
    }
    public void GoIntoAirforceStageAttack()
    {
        GameData.turnflow.turnStage = TurnFlow.TurnStage.AirforceStageAttack;   //  ��������׶�
        OperationGUI.HideGameObject(movePanel);
        OperationGUI.ShowGameObject(attackPanel);
        ChooseGUI.ShowGameObject(choosePanel);
        MessageBox.Show("�վ�����׶�");

    }
    public void GoIntoReactionStage()
    {

        GameData.turnflow.turnStage = TurnFlow.TurnStage.ReactionStage;   //  ���뷴Ӧ�׶�
        OperationGUI.HideGameObject(movePanel);
        OperationGUI.ShowGameObject(attackPanel);
        MessageBox.Show("��Ӧ�׶Σ����֣�");
    }
    public void GoIntoFightStage()
    {
        GameData.turnflow.turnStage = TurnFlow.TurnStage.FightStage;   //  ��������׶� 2nd
        OperationGUI.HideGameObject(movePanel);  
        OperationGUI.ShowGameObject(attackPanel);
        ChooseGUI.ShowGameObject(choosePanel);
        MessageBox.Show("ս���׶�");
    }
    public void GoChangeTurnPlayer()
    {
        // ��� ����ǰ��ҡ�
        if (turnPlayer == TurnPlayer.PlayerChinese) turnPlayer = TurnPlayer.PlayerAmerica;
        else turnPlayer = TurnPlayer.PlayerChinese;

    }
}
