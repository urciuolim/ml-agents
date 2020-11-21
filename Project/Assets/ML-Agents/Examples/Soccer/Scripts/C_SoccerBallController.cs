using UnityEngine;

public class C_SoccerBallController : MonoBehaviour
{
    [HideInInspector]
    public C_SoccerFieldArea area;
    public string purpleGoalTag; //will be used to check if collided with purple goal
    public string blueGoalTag; //will be used to check if collided with blue goal

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(purpleGoalTag)) //ball touched purple goal
        {
            area.GoalTouched(C_AgentSoccer.Team.Blue);
        }
        else if (col.gameObject.CompareTag(blueGoalTag)) //ball touched blue goal
        {
            area.GoalTouched(C_AgentSoccer.Team.Purple);
        }
        else if (col.gameObject.tag.Contains("Agent"))
        {
            var agent = col.gameObject.GetComponent<C_AgentSoccer>();
            area.lastAgentKick[(int)agent.team] = agent.m_PlayerIndex;
        }
    }
}
