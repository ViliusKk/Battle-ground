using System;
using UnityEngine;

[Serializable]
public class AttackInfo : MonoBehaviour
{
    public string name;
    public float delay = 0.5f;
    public GameObject vfx;
    public Transform position;
    public AudioClip audio;
}