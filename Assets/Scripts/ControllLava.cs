using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllLava : MonoBehaviour
{
    public Animator _animator; 

    private void Start() {
        _animator.SetTrigger("Play");
    }
    
}
