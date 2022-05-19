using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//장면에 들어갈때 대화가 나오는것 을 관리하는 클래스
public class CutSceneState : BattleState
{
    ConversationController conversationController;
    ConversationData data;

    protected override void Awake()
    {
        base.Awake();
        conversationController = owner.GetComponentInChildren<ConversationController>();
    }

    public override void Enter()
    {
        base.Enter();
        if (IsBattleOver())
        {
            if (DidPlayerWin())
            {
                data = Resources.Load<ConversationData>("Conversations/OutroSceneWin");
            }
            else
            {
                data = Resources.Load<ConversationData>("Conversations/OutroSceneLose");
            }
        }
        else
        {
            data = Resources.Load<ConversationData>("Conversations/IntroScene");
        }
        conversationController.Show(data);
    }
    public override void Exit()
    {
        base.Exit();
        if (data)
            Resources.UnloadAsset(data);
    }
    protected override void AddListeners()
    {
        base.AddListeners();
        ConversationController.completeEvent += OnCompleteConversation;
    }
    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        ConversationController.completeEvent -= OnCompleteConversation;
    }
    protected override void OnFire(object Sender, InfoEventArgs<int> e)
    {
        base.OnFire(Sender, e);
        conversationController.Next();
    }

    //대화 상태가 종료되면 battleEndstate로 전환 하는 함수
    void OnCompleteConversation(object sender, System.EventArgs e)
    {
        if (IsBattleOver())
        {
            owner.ChangeState<EndBattleState>();
        }
        else
        {
            owner.ChangeState<SelectUnitState>();
        }
    }
}
