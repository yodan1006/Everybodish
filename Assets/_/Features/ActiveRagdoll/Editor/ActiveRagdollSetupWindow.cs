using System.Collections.Generic;
using ActiveRagdoll.Runtime;
using UnityEditor;
using UnityEngine;

namespace ActiveRagdoll.Editor
{
    public class ActiveRagdollSetupEditor : EditorWindow
    {
        public enum RagdollPreset
        {
            GTAStyle,
            Floppy,
            Stiff,
            Zombie,
            Jello
        }
        private RagdollPreset selectedPreset = RagdollPreset.GTAStyle;
        private GameObject animatedRig;
        private GameObject physicsRig;
        private Rigidbody playerRootRb;
        private bool showBonePreview = false;
        private bool drawJointAxes = false;

        private int solverIterations = 8;
        private int solverVelocityIterations = 8;
        private float maxAngularVelocity = 20f;

        private float driveStrengthMultiplier = 1f;
        private float driveDampingMultiplier = 1f;
        private float maxDriveForceMultiplier = 1f;

        private float rootMass = 1f;
        private AnimationCurve massFalloffCurve = AnimationCurve.Linear(0, 1f, 10, 0.05f);

        private AnimationCurve linearDampingCurve = AnimationCurve.EaseInOut(0, 0.1f, 10, 0.05f);
        private AnimationCurve angularDampingCurve = AnimationCurve.EaseInOut(0, 30f, 10, 15f);


        private Dictionary<Transform, Transform> matchedBones = new();
        private readonly Dictionary<Transform, Rigidbody> parentRbMap = new();

        // Drive parameters
        private float positionSpring = 400f;
        private float positionDamper = 40f;
        private float maximumForce = 2000f;

        // Limit spring params
        private float angularXLimitSpring = 400f;
        private float angularXLimitDamper = 50f;
        private float angularYZLimitSpring = 400f;
        private float angularYZLimitDamper = 50f;
        private float linearLimitSpring = 200f;
        private float linearLimitDamper = 50f;

        // Projection tuning
        private float projectionDistance = 0.04f;
        private float projectionAngle = 5f;

        [MenuItem("Tools/Active Ragdoll/Full Setup")]
        private static void ShowWindow()
        {
            GetWindow<ActiveRagdollSetupEditor>("Active Ragdoll Setup");
        }

        private void OnGUI()
        {
            GUILayout.Label("Active Ragdoll Setup Tool", EditorStyles.boldLabel);

            GUILayout.Space(5);
            GUILayout.Label("Presets", EditorStyles.boldLabel);

            selectedPreset = (RagdollPreset)EditorGUILayout.EnumPopup("Select Preset", selectedPreset);

            if (GUILayout.Button("Apply Preset"))
            {
                ApplyPreset(selectedPreset);
            }

            animatedRig = (GameObject)EditorGUILayout.ObjectField("Animated Rig", animatedRig, typeof(GameObject), true);
            physicsRig = (GameObject)EditorGUILayout.ObjectField("Physics Rig", physicsRig, typeof(GameObject), true);
            playerRootRb = (Rigidbody)EditorGUILayout.ObjectField("Root Rigidbody", playerRootRb, typeof(Rigidbody), true);

            GUILayout.Space(5);
            GUILayout.Label("Rigidbody Settings", EditorStyles.boldLabel);
            solverIterations = EditorGUILayout.IntField("Solver Iterations", solverIterations);
            solverVelocityIterations = EditorGUILayout.IntField("Solver Velocity Iterations", solverVelocityIterations);
            maxAngularVelocity = EditorGUILayout.FloatField("Max Angular Velocity", maxAngularVelocity);

            GUILayout.Space(5);
            GUILayout.Label("Mass Distribution Settings", EditorStyles.boldLabel);
            rootMass = EditorGUILayout.FloatField("Root Mass", rootMass);
            massFalloffCurve = EditorGUILayout.CurveField("Mass (by Depth)", massFalloffCurve);
            GUILayout.Space(5);
            GUILayout.Label("Damping Curves", EditorStyles.boldLabel);
            linearDampingCurve = EditorGUILayout.CurveField("Linear Damping (by Depth)", linearDampingCurve);
            angularDampingCurve = EditorGUILayout.CurveField("Angular Damping (by Depth)", angularDampingCurve);

            GUILayout.Space(5);
            GUILayout.Label("Joint Drive Multipliers", EditorStyles.boldLabel);
            driveStrengthMultiplier = EditorGUILayout.FloatField("Strength Multiplier", driveStrengthMultiplier);
            driveDampingMultiplier = EditorGUILayout.FloatField("Damping Multiplier", driveDampingMultiplier);
            maxDriveForceMultiplier = EditorGUILayout.FloatField("Max Force Multiplier", maxDriveForceMultiplier);

            GUILayout.Space(5);
            GUILayout.Label("Joint Drive Values", EditorStyles.boldLabel);
            positionSpring = EditorGUILayout.FloatField("Position Spring", positionSpring);
            positionDamper = EditorGUILayout.FloatField("Position Damper", positionDamper);
            maximumForce = EditorGUILayout.FloatField("Maximum Force", maximumForce);

            GUILayout.Space(5);
            GUILayout.Label("Angular Limit Spring", EditorStyles.boldLabel);
            angularXLimitSpring = EditorGUILayout.FloatField("Angular X Spring", angularXLimitSpring);
            angularXLimitDamper = EditorGUILayout.FloatField("Angular X Damper", angularXLimitDamper);
            angularYZLimitSpring = EditorGUILayout.FloatField("Angular YZ Spring", angularYZLimitSpring);
            angularYZLimitDamper = EditorGUILayout.FloatField("Angular YZ Damper", angularYZLimitDamper);

            GUILayout.Space(5);
            GUILayout.Label("Linear Limit Spring", EditorStyles.boldLabel);
            linearLimitSpring = EditorGUILayout.FloatField("Linear Spring", linearLimitSpring);
            linearLimitDamper = EditorGUILayout.FloatField("Linear Damper", linearLimitDamper);

            GUILayout.Space(5);
            GUILayout.Label("Joint Projection Settings", EditorStyles.boldLabel);
            projectionDistance = EditorGUILayout.FloatField("Projection Distance", projectionDistance);
            projectionAngle = EditorGUILayout.FloatField("Projection Angle", projectionAngle);

            GUILayout.Space(5);
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
                    GUILayout.Label($"{pair.Key.name} => {pair.Value.name}");
                }
            }
        }

        private void ApplyPreset(RagdollPreset preset)
        {
            switch (preset)
            {
                case RagdollPreset.GTAStyle:
                    positionSpring = 1200f;           // Medium-high spring
                    positionDamper = 180f;            // Moderate damper
                    maximumForce = 8000f;             // Strong max force

                    angularXLimitSpring = 1200f;
                    angularXLimitDamper = 250f;
                    angularYZLimitSpring = 1200f;
                    angularYZLimitDamper = 250f;

                    linearLimitSpring = 500f;
                    linearLimitDamper = 150f;

                    projectionDistance = 0.02f;
                    projectionAngle = 2f;

                    driveStrengthMultiplier = 1.0f;
                    driveDampingMultiplier = 1.2f;
                    maxDriveForceMultiplier = 1.3f;

                    linearDampingCurve = AnimationCurve.EaseInOut(0, 0.04f, 8, 0.012f);
                    angularDampingCurve = AnimationCurve.EaseInOut(0, 15f, 8, 8f);
                    break;

                case RagdollPreset.Zombie:
                    positionSpring = 2000f;           // Higher spring than GTAStyle
                    positionDamper = 350f;            // More damping for stiffness
                    maximumForce = 12000f;            // Strong max force

                    angularXLimitSpring = 2000f;
                    angularXLimitDamper = 400f;
                    angularYZLimitSpring = 2000f;
                    angularYZLimitDamper = 400f;

                    linearLimitSpring = 650f;
                    linearLimitDamper = 250f;

                    projectionDistance = 0.015f;
                    projectionAngle = 1.5f;

                    driveStrengthMultiplier = 1.3f;
                    driveDampingMultiplier = 1.8f;
                    maxDriveForceMultiplier = 1.7f;

                    linearDampingCurve = AnimationCurve.EaseInOut(0, 0.06f, 8, 0.02f);
                    angularDampingCurve = AnimationCurve.EaseInOut(0, 22f, 8, 12f);
                    break;

                case RagdollPreset.Jello:
                    positionSpring = 300f;
                    positionDamper = 20f;
                    maximumForce = 1000f;

                    angularXLimitSpring = 300f;
                    angularXLimitDamper = 25f;
                    angularYZLimitSpring = 300f;
                    angularYZLimitDamper = 25f;

                    linearLimitSpring = 150f;
                    linearLimitDamper = 10f;

                    projectionDistance = 0.03f;
                    projectionAngle = 4f;

                    driveStrengthMultiplier = 0.4f;
                    driveDampingMultiplier = 0.3f;
                    maxDriveForceMultiplier = 0.5f;

                    linearDampingCurve = AnimationCurve.EaseInOut(0, 0.03f, 8, 0.01f);
                    angularDampingCurve = AnimationCurve.EaseInOut(0, 8f, 8, 2f);
                    break;

                case RagdollPreset.Stiff:
                    positionSpring = 15000f;
                    positionDamper = 2500f;
                    maximumForce = 100000f;

                    angularXLimitSpring = 15000f;
                    angularXLimitDamper = 3000f;
                    angularYZLimitSpring = 15000f;
                    angularYZLimitDamper = 3000f;

                    linearLimitSpring = 5000f;
                    linearLimitDamper = 1500f;

                    projectionDistance = 0.005f;
                    projectionAngle = 0.25f;

                    driveStrengthMultiplier = 10f;
                    driveDampingMultiplier = 12f;
                    maxDriveForceMultiplier = 10f;

                    massFalloffCurve = AnimationCurve.EaseInOut(0, 1f, 10, 0.1f);
                    linearDampingCurve = AnimationCurve.EaseInOut(0, 1.2f, 8, 0.4f);
                    angularDampingCurve = AnimationCurve.EaseInOut(0, 120f, 8, 60f);
                    break;

                case RagdollPreset.Floppy:
                    positionSpring = 400f;             // Slightly stronger than Jello, but still floppy
                    positionDamper = 40f;
                    maximumForce = 1200f;

                    angularXLimitSpring = 400f;
                    angularXLimitDamper = 35f;
                    angularYZLimitSpring = 400f;
                    angularYZLimitDamper = 35f;

                    linearLimitSpring = 180f;
                    linearLimitDamper = 15f;

                    projectionDistance = 0.025f;
                    projectionAngle = 3.5f;

                    driveStrengthMultiplier = 0.5f;
                    driveDampingMultiplier = 0.45f;
                    maxDriveForceMultiplier = 0.55f;

                    linearDampingCurve = AnimationCurve.EaseInOut(0, 0.035f, 8, 0.012f);
                    angularDampingCurve = AnimationCurve.EaseInOut(0, 10f, 8, 4f);
                    break;
            }

            Repaint();
        }

        private bool ValidateInput()
        {
            if (animatedRig != null && physicsRig != null && playerRootRb != null)
                return true;

            EditorUtility.DisplayDialog("Missing Reference", "Please assign both animated and physics rigs + the root rigidbody.", "OK");
            return false;
        }

        private void SetupRagdoll()
        {
            EditorGUI.BeginChangeCheck();
            matchedBones.Clear();
            parentRbMap.Clear();

            Animator animator = animatedRig.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }

            foreach (MeshRenderer mr in animatedRig.GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = false;
            }

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
            RecursiveJointSetup(physicsRig, playerRootRb, 0);

            Debug.Log($"Setup complete. {jointsAdded} joints configured.");
            EditorUtility.DisplayDialog("Ragdoll Setup Complete", $"Configured {jointsAdded} joints with correct connections.", "OK");
            EditorGUI.EndChangeCheck();
            EditorUtility.SetDirty(this);
        }

        private int jointsAdded = 0;

        private void RecursiveJointSetup(GameObject parent, Rigidbody lastRb, int depth)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (child.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    Undo.RecordObject(rb, "Configure Rigidbody");

                    // Mass distribution based on depth
                    float calculatedMass = rootMass * massFalloffCurve.Evaluate(depth);
                    rb.mass = calculatedMass;
                    rb.useGravity = true;
                    rb.solverIterations = solverIterations;
                    rb.solverVelocityIterations = solverVelocityIterations;
                    rb.maxAngularVelocity = maxAngularVelocity;
                    rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    rb.interpolation = RigidbodyInterpolation.Interpolate;

                    // Damping from curves
                    rb.linearDamping = linearDampingCurve.Evaluate(depth);
                    rb.angularDamping = angularDampingCurve.Evaluate(depth);

                    if (matchedBones.TryGetValue(child, out Transform animatedBone))
                    {
                        ConfigurableJointExtended jointExt;
                        if (child.gameObject.TryGetComponent<CharacterJoint>(out CharacterJoint characterJoint))
                        {
                            DestroyImmediate(characterJoint);
                            child.gameObject.AddComponent<ConfigurableJoint>();
                        }
                        if (!child.gameObject.TryGetComponent<ConfigurableJointExtended>(out jointExt))
                        {
                            jointExt = child.gameObject.AddComponent<ConfigurableJointExtended>();
                        }

                        jointExt.Initialize(animatedBone.gameObject, lastRb);

                        jointExt.driveStrengthMultiplier = driveStrengthMultiplier;
                        jointExt.driveDampingMultiplier = driveDampingMultiplier;
                        jointExt.maxDriveForceMultiplier = maxDriveForceMultiplier;
                        // Apply custom joint drive parameters
                        jointExt.positionSpring = positionSpring;
                        jointExt.positionDamper = positionDamper;
                        jointExt.maximumForce = maximumForce;

                        jointExt.angularXLimitSpring = angularXLimitSpring;
                        jointExt.angularXLimitDamper = angularXLimitDamper;
                        jointExt.angularYZLimitSpring = angularYZLimitSpring;
                        jointExt.angularYZLimitDamper = angularYZLimitDamper;

                        jointExt.linearLimitSpring = linearLimitSpring;
                        jointExt.linearLimitDamper = linearLimitDamper;

                        jointExt.projectionDistance = projectionDistance;
                        jointExt.projectionAngle = projectionAngle;
                        //jointExt.ApplyAdaptiveConfigInEditor();
                        if (depth == 0)
                        {
                            jointExt.boneLength = Mathf.Infinity;
                        }
                        jointsAdded++;
                    }

                    RecursiveJointSetup(child.gameObject, rb, depth + 1);
                }
                else
                {
                    RecursiveJointSetup(child.gameObject, lastRb, depth);
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
