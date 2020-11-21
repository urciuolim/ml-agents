using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class C_PlayerState
{
    public int playerIndex;
    [FormerlySerializedAs("agentRB")]
    public Rigidbody agentRb;
    public Vector3 startingPos;
    public C_AgentSoccer agentScript;
    public float ballPosReward;
}

public class C_SoccerFieldArea : MonoBehaviour
{
    public GameObject ball;
    [FormerlySerializedAs("ballRB")]
    [HideInInspector]
    public Rigidbody ballRb;
    public GameObject ground;
    public GameObject centerPitch;
    C_SoccerBallController m_BallController;
    public List<C_PlayerState> playerStates = new List<C_PlayerState>();
    [HideInInspector]
    public Vector3 ballStartingPos;
    public GameObject goalTextUI;
    [HideInInspector]
    public bool canResetBall;
    [HideInInspector]
    public int[] lastAgentKick = new int[]{ -1, -1 };

    EnvironmentParameters m_ResetParams;

    void Awake()
    {
        canResetBall = true;
        if (goalTextUI) { goalTextUI.SetActive(false); }
        ballRb = ball.GetComponent<Rigidbody>();
        m_BallController = ball.GetComponent<C_SoccerBallController>();
        m_BallController.area = this;
        ballStartingPos = ball.transform.position;

        m_ResetParams = Academy.Instance.EnvironmentParameters;
    }

    IEnumerator ShowGoalUI()
    {
        if (goalTextUI) goalTextUI.SetActive(true);
        yield return new WaitForSeconds(.25f);
        if (goalTextUI) goalTextUI.SetActive(false);
    }

    public void GoalTouched(C_AgentSoccer.Team scoredTeam)
    {
        for (int i = 0; i < playerStates.Count; i++)
        {
            var ps = playerStates[i];
            if (ps.agentScript.team == scoredTeam)
            {
                if (ps.agentScript.position == C_AgentSoccer.Position.Companion)
                {
                    if (lastAgentKick[(int)ps.agentScript.team] != ps.agentScript.m_PlayerIndex)
                    {
                        //Debug.Log("Companion assisted");
                        ps.agentScript.AddReward(1 + ps.agentScript.timePenalty);
                    }
                    //else
                        //Debug.Log("Companion scored");
                }
                else
                    ps.agentScript.AddReward(1 + ps.agentScript.timePenalty);
            }
            else
            {
                ps.agentScript.AddReward(-1);
            }

            ps.agentScript.EndEpisode();  //all agents need to be reset

            if (goalTextUI)
            {
                StartCoroutine(ShowGoalUI());
            }
        }
        lastAgentKick = new int[] { -1, -1 };
    }

    public void ResetBall()
    {
        ball.transform.position = ballStartingPos;
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;

        var ballScale = m_ResetParams.GetWithDefault("ball_scale", 0.015f);
        ballRb.transform.localScale = new Vector3(ballScale, ballScale, ballScale);
    }
}
