using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using ActiveRagdoll.Runtime;
using UnityEditor;
using UnityEngine;

namespace ActiveRagdoll.Editor
{
    public class ActiveRagdollSetupEditor : EditorWindow
    {
        private GameObject animatedRig;
        private GameObject physicsRig;
        private Rigidbody playerRootRb;
        private bool showBonePreview = false;
        private bool drawJointAxes = false;

        private int solverIterations = 8;
        private int solverVelocityIterations = 8;
        private float maxAngularVelocity = 20f;
        private float defaultMass = 1f;

        private Dictionary<Transform, Transform> matchedBones = new();
        private readonly Dictionary<Transform, Rigidbody> parentRbMap = new();

        [MenuItem("Tools/Active Ragdoll/Full Setup")]
        private static void ShowWindow()
        {
            GetWindow<ActiveRagdollSetupEditor>("Active Ragdoll Setup");
        }

        private void OnGUI()
        {
            GUILayout.Label("Active Ragdoll Setup Tool", EditorStyles.boldLabel);

            animatedRig = (GameObject)EditorGUILayout.ObjectField("Animated Rig", animatedRig, typeof(GameObject), true);
            physicsRig = (GameObject)EditorGUILayout.ObjectField("Physics Rig", physicsRig, typeof(GameObject), true);
            playerRootRb = (Rigidbody)EditorGUILayout.ObjectField("Root Rigidbody", playerRootRb, typeof(Rigidbody), true);

            GUILayout.Space(5);
            GUILayout.Label("Rigidbody Settings", EditorStyles.boldLabel);
            solverIterations = EditorGUILayout.IntField("Solver Iterations", solverIterations);
            solverVelocityIterations = EditorGUILayout.IntField("Solver Velocity Iterations", solverVelocityIterations);
            maxAngularVelocity = EditorGUILayout.FloatField("Max Angular Velocity", maxAngularVelocity);
            defaultMass = EditorGUILayout.FloatField("Default Mass", defaultMass);

            GUILayout.Space(10);
            showBonePreview = EditorGUILayout.Toggle("Show Bone Preview", showBonePreview);
            drawJointAxes = EditorGUILayout.Toggle("Draw Joint Axes in Scene View", drawJointAxes);

            GUILayout.Space(10);
            if (GUILayout.Button("Run Full Setup"))
            {
                if (ValidateInput())
                {
                    SetupRagdoll();
                    SceneView.RepaintAll();
                }
            }

            if (showBonePreview && matchedBones.Count > 0)
            {
                GUILayout.Space(10);
                GUILayout.Label("Bone Matching Preview:", EditorStyles.boldLabel);
                foreach (var pair in matchedBones)
                {
                    GUILayout.Label($"{pair.Key.name} ? {pair.Value.name}");
                }
            }
        }

        private bool ValidateInput()
        {
            bool isValid = false;
            if (animatedRig != null && physicsRig != null && playerRootRb != null)
            {
                isValid = true;
            }
            else
            {
                EditorUtility.DisplayDialog("Missing Reference", "Please assign both animated and physics rigs + the root rigidbody.", "OK");
            }

            return isValid;
        }

        private void SetupRagdoll()
        {
            EditorGUI.BeginChangeCheck();
            matchedBones.Clear();
            parentRbMap.Clear();

            // Setup animator
            Animator animator = animatedRig.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }

            foreach (MeshRenderer mr in animatedRig.GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = false;
            }

            // Map animated bones by name
            Dictionary<string, Transform> animatedDict = new();
            foreach (Transform t in animatedRig.GetComponentsInChildren<Transform>())
            {
                if (!animatedDict.ContainsKey(t.name))
                    animatedDict[t.name] = t;
            }

            Rigidbody[] physicsRigidbodies = physicsRig.GetComponentsInChildren<Rigidbody>();

            matchedBones = new Dictionary<Transform, Transform>();
            foreach (var rb in physicsRigidbodies)
            {
                if (animatedDict.TryGetValue(rb.name, out Transform animatedBone))
                {
                    matchedBones[rb.transform] = animatedBone;
                }
                else
                {
                    Debug.LogWarning($"No animated bone found for {rb.name}");
                }
            }

            jointsAdded = 0;
            RecursiveJointSetup(physicsRig, playerRootRb);

            Debug.Log($"Setup complete. {jointsAdded} joints configured.");
            EditorUtility.DisplayDialog("Ragdoll Setup Complete", $"Configured {jointsAdded} joints with correct connections.", "OK");
            EditorGUI.EndChangeCheck();
            EditorUtility.SetDirty(this);
        }

        private int jointsAdded = 0;

        private void RecursiveJointSetup(GameObject parent, Rigidbody lastRb)
        {
            if (parent.transform.childCount > 0)
            {
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    Transform child = parent.transform.GetChild(i);
                    if (child.TryGetComponent<Rigidbody>(out Rigidbody rb))

                    {
                        Undo.RecordObject(rb, "Configure Rigidbody");
                        rb.useGravity = true;
                        rb.mass = defaultMass;
                        rb.solverIterations = solverIterations;
                        rb.solverVelocityIterations = solverVelocityIterations;
                        rb.maxAngularVelocity = maxAngularVelocity;

                        if (matchedBones.TryGetValue(child, out Transform animatedBone))
                        {
                            if (child.gameObject.TryGetComponent<CharacterJoint>(out CharacterJoint characterJoint))
                            {
                                Destroy(characterJoint);
                            }
                            if (child.gameObject.TryGetComponent<ConfigurableJointExtended>(out ConfigurableJointExtended configurableJointExtended))
                            {
                                Destroy(characterJoint);
                            }

                                ConfigurableJointExtended jointExt = child.gameObject.AddComponent<ConfigurableJointExtended>();

                                jointExt.Initialize(animatedBone.gameObject, lastRb);
                                jointsAdded++;
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

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += DrawSceneViewHandles;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= DrawSceneViewHandles;
        }

        private void DrawSceneViewHandles(SceneView view)
        {
            if (!drawJointAxes || matchedBones.Count == 0) return;

            Handles.color = Color.cyan;
            foreach (var pair in matchedBones)
            {
                Transform physicsBone = pair.Key;
                Transform animatedBone = pair.Value;

                if (physicsBone != null && animatedBone != null)
                {
                    Vector3 pos = physicsBone.position;
                    Handles.ArrowHandleCap(0, pos, physicsBone.rotation, 0.2f, EventType.Repaint);
                    Handles.DrawLine(pos, animatedBone.position);
                }
            }
        }
    }
}