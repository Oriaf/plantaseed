using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllLava : MonoBehaviour
{
    int targetTime; 

    public Animator _animator; 

    private void Start() {
        
        _animator.Play("Lava", -1,Random.Range(0.0f, 5.0f));
    }
    
    
}
