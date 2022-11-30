using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBodyScript : MonoBehaviour
{
    public Color c0 = Color.red, c100 = Color.green;
    public ParticleSystem leftHand, rightHand, leftFoot, rightFoot;

    private int count = 2;

    void Start()
    {

    }

    public void Emit()
    {
        leftHand.Emit(count);
        rightHand.Emit(count);
        leftFoot.Emit(count);
        rightFoot.Emit(count);
    }

    public void SetParticle(bool set)
    {
        if (set == false)
        {
            leftHand.Stop();
            rightHand.Stop();
            leftFoot.Stop();
            rightFoot.Stop();
        }
        else
        {
            leftHand.Play();
            rightHand.Play();
            leftFoot.Play();
            rightFoot.Play();
        }
    }

    public void DoUpdate(PoseDetectionScript.BodyPartScore value)
    {
        SetColor(leftHand, value.mapped_left_hand / 100f);
        SetColor(rightHand, value.mapped_right_hand / 100f);
        SetColor(leftFoot, value.mapped_left_foot / 100f);
        SetColor(rightFoot, value.mapped_right_foot / 100f);
    }

    void SetColor(ParticleSystem particle, float value)
    {
        var main = particle.main;
        main.startColor = Color.Lerp(c0, c100, value);
    }

}
