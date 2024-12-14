using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.EventSystems;

public class PlayerAgent : Agent
{
    [SerializeField] private Transform targetTransform;

    private Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(9f, -8f), 0, Random.Range(9f, -8f));
        targetTransform.localPosition = new Vector3(Random.Range(9f, -8f), 0, Random.Range(9f, -8f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveRotate = actions.ContinuousActions[0];
        float moveForward = actions.ContinuousActions[1];
        float moveSpeed = 3f;


        rb.MovePosition(transform.position + transform.forward * moveForward * moveSpeed * Time.deltaTime);
        transform.Rotate(0f, moveRotate * moveSpeed, 0f, Space.Self);

        //transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continiousActions = actionsOut.ContinuousActions;
        continiousActions[0] = Input.GetAxisRaw("Horizontal");
        continiousActions[1] = Input.GetAxisRaw("Vertical");

    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.transform.tag == "Moving Box") 
    //    {
    //        Rigidbody box = hit.collider.GetComponent<Rigidbody>();
    //        if (box != null)
    //        {
    //            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, 0);
    //            box.linearVelocity = pushDirection;
    //        }
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goall>(out Goall goall))
        {
            SetReward(+1f);
            EndEpisode();
        }

        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
}



