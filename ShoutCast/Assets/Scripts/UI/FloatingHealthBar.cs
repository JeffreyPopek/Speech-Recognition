using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FloatingHealthBar : MonoBehaviour
{
    // value
        [SerializeField] private Slider slider;
        private Camera cam;
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        
    
        private void Start()
        {
            cam = Camera.main;
        }
    
        public void UpdateHealthbar(float currentHealth, float maxHealth)
        {
            slider.value = currentHealth / maxHealth;
        }
    
        private void Update()
        {
            transform.rotation = cam.transform.rotation;
            transform.position = target.position + offset;
        }
}
