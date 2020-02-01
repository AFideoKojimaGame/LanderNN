using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanderActions {
    TurnLeft,
    TurnRight,
    Thrust
}

public class Lander : MonoBehaviour {

    public Agent myAgent = null;
    int geneCount = 6;

    Vector2 direction = Vector2.zero;

    int index = -1;
    float actionTime = 0.5f;
    float actionCounter = 0;

    float moveSpeed = 2;

    GameObject target;

    LanderActions myAction;

    Rigidbody2D myRB;

    float distance;

    bool reached = false;

    float currentThrust;
    float currentRot;

    public Material normalMat, eliteMat;

    MeshRenderer myMR;

    Vector3 rotation = Vector3.zero;
    Vector2 movementVector = Vector2.zero;

    NN myNeuralNetwork;
    public bool weightsSet = true;

    float[] inputs;

    bool constant = false;

    float[] lastOutput;

	// Use this for initialization
	void Start () {
        actionCounter = 0;
        target = GameObject.FindGameObjectWithTag("Victory");
        myRB = GetComponent<Rigidbody2D>();
        myMR = GetComponent<MeshRenderer>();
        myNeuralNetwork = new NN(1, 5, 3, 1);
        lastOutput = new float[1];
        inputs = new float[1];
	}
	
	// Update is called once per frame
	void Update () {

        if (!weightsSet) {
            myNeuralNetwork.SetWeights(myAgent.myC.genes);
            weightsSet = true;
        }

        if (transform.position.x <= -15)
            transform.position = new Vector3(-15, transform.position.y, 0);
        if (transform.position.x >= 15)
            transform.position = new Vector3(15, transform.position.y, 0);
        if (transform.position.y >= 10)
            transform.position = new Vector3(transform.position.x, 10, 0);
        if(transform.position.y <= -2.5f)
            transform.position = new Vector3(transform.position.x, -2.5f, 0);

        distance = Vector3.Distance(transform.position, target.transform.position);


        if (reached) {

            myRB.Sleep();
            if(transform.position.y < target.transform.position.y)
                myAgent.SetFitness(20 - distance);
            else
                myAgent.SetFitness(20 - distance);
        }else {
            if (myAgent != null) {           
                //if(actionCounter < actionTime) {
                //    actionCounter += Time.deltaTime;
                //    PerformAction();
                //}else if(index >= 0) {
                //    index++;
                //    if (index < geneCount) {
                //        actionTime = myAgent.myC.genes[index].time;
                //        myAction = myAgent.myC.genes[index].action;
                //        currentThrust = myAgent.myC.genes[index].thrust;
                //        currentRot = myAgent.myC.genes[index].rotAmount;
                //        actionCounter = 0;
                //    }
                //}else
                //    index++;


                float angle = transform.eulerAngles.z % 360f;

                if (angle < 0f)
                    angle += 360f;

                Vector2 deltaVector = (target.transform.position - transform.position).normalized;

                float rad = Mathf.Atan2(deltaVector.y, deltaVector.x);
                rad *= Mathf.Rad2Deg;

                rad = rad % 360;
                if (rad < 0)
                {
                    rad = 360 + rad;
                }

                rad = 90f - rad;
                if (rad < 0f)
                {
                    rad += 360f;
                }

                rad = 360 - rad;
                rad -= angle;

                if (rad < 0)
                    rad = 360 + rad;

                if (rad >= 180f)
                {
                    rad = 360 - rad;
                    rad *= -1f;
                }

                rad *= Mathf.Deg2Rad;

                
                //if (actionCounter > actionTime) {

                    actionCounter = 0;
                    inputs[0] = rad / (Mathf.PI);

                    float[] output = myNeuralNetwork.ProcessInputs(inputs);

                    myRB.velocity = 2.5f * transform.up;
                    myRB.angularVelocity = 250 * output[0];
                    if (myRB.angularVelocity > 500)
                        myRB.angularVelocity = 500;
                    else if (myRB.angularVelocity < -500)
                        myRB.angularVelocity = -500;

                    if (output[0] == lastOutput[0])
                        constant = true;
                    else
                        constant = false;

                    lastOutput = output;

                    if(!constant)
                        myAgent.SetFitness(20 - distance);
                    else
                        myAgent.SetFitness(20 - distance);
                //}else
                //    actionCounter += Time.deltaTime;
            }
        }

        myAgent.myC.SetWeights(myNeuralNetwork.GetWeights());
	}

    public void PerformAction() {
        
        switch (myAction) {
            case LanderActions.TurnLeft:
                    //myRB.AddTorque(transform.forward);
                    //myRB.freezeRotation = false;
                    //rotation = transform.forward;
                    //transform.Rotate(rotation * moveSpeed);
                    //myRB.AddForce(transform.up * moveSpeed, ForceMode.Acceleration);
                rotation.z = -2;
                movementVector.y = 0;
                break;

            case LanderActions.TurnRight:
                    //myRB.AddTorque(transform.forward * -1);
                    //myRB.freezeRotation = false;
                    //rotation = -transform.forward;
                    //transform.Rotate(rotation * moveSpeed);
                    //myRB.AddForce(transform.up * moveSpeed, ForceMode.Acceleration);
                rotation.z = 2;
                movementVector.y = 0;
                break;

            case LanderActions.Thrust:
                    //myRB.freezeRotation = true;
                    //myRB.AddForce(transform.up * moveSpeed, ForceMode.Acceleration);
                movementVector.y = 1;
                break;
        }
        transform.Rotate(rotation);
        transform.Translate(movementVector * Time.deltaTime * 3);

        if (rotation.z > 0)
            rotation.z -= Time.deltaTime;

        if (rotation.z < 0)
            rotation.z += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col) {
        //myRB.Sleep();

        if (col.gameObject.tag == "Victory") {
            reached = true;
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        //myRB.Sleep();
    }

    public void ReplaceAgent(Agent a) {
        myMR.material = normalMat;
        //myRB.ResetCenterOfMass();
        //myRB.ResetInertiaTensor();
        //transform.rotation = Quaternion.identity;
        //myRB.Sleep();
        //myRB.rotation = Quaternion.identity;
        //myRB.velocity = Vector3.zero;
        //myRB.Sleep();
        transform.rotation = Quaternion.identity;
        rotation = Vector3.zero;
        movementVector = Vector3.zero;
        //myRB.velocity = Vector3.zero;
        //myRB.WakeUp();
        reached = false;
        index = -1;
        actionCounter = actionTime + 1;
    }

    public void UpdateWeights() {
        if(myNeuralNetwork != null)
            myNeuralNetwork.SetWeights(myAgent.myC.genes);
    }
}
