using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    private const string IS_WALKING = "IsWalkingIn";
    [SerializeField]Player player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING,player.IsWalking());
    }
}
