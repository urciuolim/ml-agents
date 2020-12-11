using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class C_PlayerState
{
    public int playerIndex;
    [FormerlySerializedAs("agentRB")]
    public Rigidbody agentRb;
    public Vector3 startingPos;
    public C_AgentSoccer agentScript;
    public float ballPosReward;
    public List<int> numGoalsScored;
    public List<int> numKicks;
    public List<float> totalReward;
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
    [HideInInspector]
    List<float> numEpisodes = new List<float>();
    public float nEpisodes;

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
        numEpisodes.Add(0f);
    }
    
    public void updateUI()
    {
        int GEN1 = 1;
        int GEN2 = 3;
        int COM = 2;
        int HUM = 0;
        int len = playerStates[0].numGoalsScored.Count;
        GameObject ui = GameObject.FindWithTag("scoreboard");
        Text sb = ui.GetComponent<Text>();
        sb.text = "Goals:\n";
        sb.text += "\tBlue: ";
        for (int i = 0; i < len; i++)
            sb.text += ((playerStates[GEN1].numGoalsScored[i] + playerStates[GEN2].numGoalsScored[i])/numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tGeneric 1: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[GEN1].numGoalsScored[i] / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tGeneric 2: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[GEN2].numGoalsScored[i] / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\tPurple: ";
        for (int i = 0; i < len; i++)
            sb.text += ((playerStates[COM].numGoalsScored[i] + playerStates[HUM].numGoalsScored[i]) / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tCompanion: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[COM].numGoalsScored[i] / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tHuman: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[HUM].numGoalsScored[i] / numEpisodes[i]).ToString("0.0") + " | ";

        sb.text += "\nKicks:\n";
        sb.text += "\tBlue: ";
        for (int i = 0; i < len; i++)
            sb.text += ((playerStates[GEN1].numKicks[i] + playerStates[GEN2].numKicks[i]) / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tGeneric 1: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[GEN1].numKicks[i] / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tGeneric 2: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[GEN2].numKicks[i] / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\tPurple: ";
        for (int i = 0; i < len; i++)
            sb.text += ((playerStates[COM].numKicks[i] + playerStates[HUM].numKicks[i]) / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tCompanion: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[COM].numKicks[i] / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tHuman: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[HUM].numKicks[i] / numEpisodes[i]).ToString("0.0") + " | ";

        sb.text += "\nReward:\n";
        sb.text += "\tBlue: ";
        for (int i = 0; i < len; i++)
            sb.text += ((playerStates[GEN1].totalReward[i] + playerStates[GEN2].totalReward[i]) / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tGeneric 1: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[GEN1].totalReward[i] / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tGeneric 2: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[GEN2].totalReward[i] / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\tPurple: ";
        for (int i = 0; i < len; i++)
            sb.text += ((playerStates[COM].totalReward[i] + playerStates[HUM].totalReward[i]) / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tCompanion: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[COM].totalReward[i] / numEpisodes[i]).ToString("0.0") + " | ";
        sb.text += "\n\t\tHuman: ";
        for (int i = 0; i < len; i++)
            sb.text += (playerStates[HUM].totalReward[i] / numEpisodes[i]).ToString("0.0") + " | ";

        if (numEpisodes[numEpisodes.Count - 1] >= nEpisodes)
        {
            numEpisodes.Add(0f);
            for (int i = 0; i < playerStates.Count; i++)
            {
                playerStates[i].numGoalsScored.Add(0);
                playerStates[i].numKicks.Add(0);
                playerStates[i].totalReward.Add(0f);
            }
        }
    }

    IEnumerator ShowGoalUI()
    {
        if (goalTextUI) goalTextUI.SetActive(true);
        yield return new WaitForSeconds(.25f);
        if (goalTextUI) goalTextUI.SetActive(false);
    }

    public void GoalTouched(C_AgentSoccer.Team scoredTeam)
    {
        var tmp = numEpisodes[numEpisodes.Count - 1];
        numEpisodes.RemoveAt(numEpisodes.Count - 1);
        numEpisodes.Add(tmp + 1f);
        for (int i = 0; i < playerStates.Count; i++)
        {
            var ps = playerStates[i];
            var r = 0f;
            if (ps.agentScript.team == scoredTeam)
            {
                if (ps.agentScript.position == C_AgentSoccer.Position.Companion)
                {
                    if (lastAgentKick[(int)ps.agentScript.team] != ps.agentScript.m_PlayerIndex)
                    {
                        //Debug.Log("Companion assisted");
                        ps.agentScript.AddReward(2 + ps.agentScript.timePenalty);
                        r = 2 + ps.agentScript.timePenalty;
                    }
                    else
                    {
                        ps.agentScript.AddReward(1 + ps.agentScript.timePenalty);
                        r = 1 + ps.agentScript.timePenalty;
                    }
                }
                else
                {
                    ps.agentScript.AddReward(1 + ps.agentScript.timePenalty);
                    r = 1 + ps.agentScript.timePenalty;
                }

                if (lastAgentKick[(int)ps.agentScript.team] == ps.agentScript.m_PlayerIndex)
                {
                    var tmp2 = ps.numGoalsScored[ps.numGoalsScored.Count - 1];
                    ps.numGoalsScored.RemoveAt(ps.numGoalsScored.Count - 1);
                    ps.numGoalsScored.Add(tmp2+1);
                }
            }
            else
            {
                ps.agentScript.AddReward(-1);
                r = -1;
            }

            ps.agentScript.EndEpisode();  //all agents need to be reset

            if (goalTextUI)
            {
                StartCoroutine(ShowGoalUI());
            }
            var tmp3 = ps.totalReward[ps.totalReward.Count - 1];
            ps.totalReward.RemoveAt(ps.totalReward.Count - 1);
            ps.totalReward.Add(tmp3+ r);
        }
        lastAgentKick = new int[] { -1, -1 };
        updateUI();
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
