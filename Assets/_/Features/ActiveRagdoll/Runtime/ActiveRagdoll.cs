using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class ActiveRagdoll : MonoBehaviour
    {
        public int solverIterations = 8;
        public int solverVelocityIterations = 8;
        public float maxAngularVelocity = 20f;
        public GameObject animatedBody;
        public GameObject physicsBody;
        public Rigidbody playerRootRb;
        private readonly Dictionary<String, Transform> animatedTransformsDictionary = new();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            SetupAnimatedBody();
            SetupPhysicsBody();
            SetupJoints();
            animatedTransformsDictionary.Clear();
        }

        private void SetupAnimatedBody()
        {
            Animator animator = animatedBody.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }
            MeshRenderer renderer = animatedBody.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
            Transform[] transforms = animatedBody.GetComponentsInChildren<Transform>();
            foreach (Transform t in transforms)
            {
                animatedTransformsDictionary[t.name] = t;
            }
        }

        private void SetupPhysicsBody()
        {
            Rigidbody[] rigidbodies = physicsBody.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidbodies)
            {
                Debug.Log("RigidBody found " + rb.name);
                rb.solverIterations = solverIterations;
                rb.solverVelocityIterations = solverVelocityIterations;
                rb.maxAngularVelocity = maxAngularVelocity;
                rb.useGravity = true;
                rb.mass = 1;
            }
        }

        private void SetupJoints()
        {
            RecursiveJointSetup(physicsBody);
        }

        private void RecursiveJointSetup(GameObject parent, Rigidbody lastRb = null)
        {
            if (parent.transform.childCount > 0)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    Transform child = parent.transform.GetChild(i);
                    if (child.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        rb.useGravity = true;
                        ConfigurableJointExtended joint;
                        if (animatedTransformsDictionary.ContainsKey(child.gameObject.name))
                        {
                            joint = child.gameObject.AddComponent<ConfigurableJointExtended>();
                            if (lastRb != null)
                            {
                                joint.Initialize(animatedTransformsDictionary[child.gameObject.name].gameObject, lastRb);
                            }
                            else
                            {
                                rb.constraints = RigidbodyConstraints.FreezeAll;
                                joint.Initialize(animatedTransformsDictionary[child.gameObject.name].gameObject, playerRootRb);
                            }
                            Debug.Log("Configurable Join Added");

                        }
                        RecursiveJointSetup(child.gameObject, rb);
                    }
                    else
                    {
                        RecursiveJointSetup(child.gameObject, lastRb);
                    }
                }
            }
        }
    }
}
