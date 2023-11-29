using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MyAgent : Agent
{
    public float speed = 5f;

    public Transform TargetTransform;

    private Vector3 startingPosition = new Vector3(0.0f,0.0f,-19f);
    private bool hasTouchedCheckpoint1 = false;
    private bool hasTouchedCheckpoint2 = false;

    private enum ACTIONS
    {
        LEFT = 0,
        FORWARD = 1,
        RIGHT = 2,
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = startingPosition;

        float xPosition = UnityEngine.Random.Range(-19, 19);
        float zPosition = UnityEngine.Random.Range(16, 19);

        TargetTransform.localPosition = new Vector3(xPosition, 0.0f, zPosition);
        hasTouchedCheckpoint1 = false;
        hasTouchedCheckpoint2 = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actions = actionsOut.DiscreteActions;

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == -1)
        {
            actions[0] = (int)ACTIONS.LEFT;
        }
        else if (horizontal == +1)
        {
            actions[0] = (int)ACTIONS.RIGHT;
        }
        else if (vertical == +1)
        {
            actions[0] = (int)ACTIONS.FORWARD;
        }
        else
        {
            actions[0] = 1;
        }

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
         var actionTaken = actions.DiscreteActions[0];

         switch (actionTaken)
         {
             case (int)ACTIONS.FORWARD:
                 transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
             case (int)ACTIONS.LEFT:
                 transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
             case (int)ACTIONS.RIGHT:
                 transform.rotation = Quaternion.Euler(0, +90, 0);
                break;
         }

         transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            AddReward(-0.1f);
            EndEpisode();
        }
        else if (other.tag == "Gem")
        {
            AddReward(1.0f);
            EndEpisode();
        }
        else if (other.tag == "Cactus")
        {
            AddReward(-1.0f);
            EndEpisode();
        }
       else if (other.tag == "Checkpoint1" && !hasTouchedCheckpoint1)
        {
            hasTouchedCheckpoint1 = true;
            AddReward(0.5f); 
        }
        else if (other.tag == "Checkpoint2" && !hasTouchedCheckpoint2)
        {
            hasTouchedCheckpoint2 = true;
            AddReward(0.5f); 
        }
    }
}
