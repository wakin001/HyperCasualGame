using System;
using UnityEngine;
using Obi;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(ObiParticlePicker))]
public class CustomRopeDragger : MonoBehaviour
{
    public float springStiffness = 500;
    public float springDamping = 50;
    public bool drawSpring = true;

    private LineRenderer lineRenderer;
    private ObiParticlePicker picker;
    private ObiParticlePicker.ParticlePickEventArgs pickArgs;

    public float vibrateDeltaDistance = 100f;
    public float vibrateDeltaTime = 1f;

    private Vector2 previousPos = Vector2.zero;
    private float totalTime = 0f;

    void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        picker = GetComponent<ObiParticlePicker>();
        picker.OnParticlePicked.AddListener(Picker_OnParticlePicked);
        picker.OnParticleDragged.AddListener(Picker_OnParticleDragged);
        picker.OnParticleReleased.AddListener(Picker_OnParticleReleased);
    }

    void OnDisable()
    {
        picker.OnParticlePicked.RemoveListener(Picker_OnParticlePicked);
        picker.OnParticleDragged.RemoveListener(Picker_OnParticleDragged);
        picker.OnParticleReleased.RemoveListener(Picker_OnParticleReleased);
        lineRenderer.positionCount = 0;
    }

    void FixedUpdate()
    {
        ObiSolver solver = picker.solver;

        if (solver != null && pickArgs != null)
        {

            // Calculate picking position in solver space:
            Vector4 targetPosition = solver.transform.InverseTransformPoint(pickArgs.worldPosition);
            targetPosition.y = 0.5f;
            //Debug.Log("targetpos, x :" + targetPosition.x + ", y: " + targetPosition.y + ", z:" + targetPosition.z);

            // Calculate effective inverse mass:
            float invMass = solver.invMasses[pickArgs.particleIndex];

            if (invMass > 0)
            {
                // Calculate and apply spring force:
                Vector4 position = solver.positions[pickArgs.particleIndex];
                Vector4 velocity = solver.velocities[pickArgs.particleIndex];
                solver.externalForces[pickArgs.particleIndex] = ((targetPosition - position) * springStiffness - velocity * springDamping) / invMass;


                if (drawSpring)
                {
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, pickArgs.worldPosition);
                    lineRenderer.SetPosition(1, solver.transform.TransformPoint(position));
                }
                else
                {
                    lineRenderer.positionCount = 0;
                }
            }

        }
    }

    void Picker_OnParticlePicked(ObiParticlePicker.ParticlePickEventArgs e)
    {
        previousPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        VibrationPlayer.Vibrate(VibrationType.Short);
        Picker_OnParticleDragged(e);
    }

    void Picker_OnParticleDragged(ObiParticlePicker.ParticlePickEventArgs e)
    {
        pickArgs = e;
        RopeCtrl.instance.fingerImage.gameObject.SetActive(true);
        Vector2 curPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        RopeCtrl.instance.UpdateFingerPos(curPos);

        totalTime += Time.deltaTime;
        float deltaDistance = (curPos - previousPos).sqrMagnitude;
        if (totalTime > vibrateDeltaTime && deltaDistance > vibrateDeltaDistance)
        {
            VibrationPlayer.Vibrate(VibrationType.Short);
            totalTime = 0f;
            previousPos = curPos;
        }
    }

    void Picker_OnParticleReleased(ObiParticlePicker.ParticlePickEventArgs e)
    {
        VibrationPlayer.Vibrate(VibrationType.Short);
        pickArgs = null;
        lineRenderer.positionCount = 0;
        RopeCtrl.instance.fingerImage.gameObject.SetActive(false);
    }

}

