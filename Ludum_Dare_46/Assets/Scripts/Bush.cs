﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Interactable
{
    private bool onFire;
    private bool burned;
    [SerializeField]
    private float burnTime;

    [SerializeField]
    private AnimationCurve burnCurve;

    [SerializeField]
    private Color startColor;
    [SerializeField]
    private Color endColor;

    [SerializeField] 
    private GameObject deadBushPrefab;

    private BurnedBush _burnedBush;
    

    private float timer;
    
    [SerializeField]
    private GameObject firePrefab;
    private Fire fire;

    private new SpriteRenderer renderer;
    private new Transform transform;
    
    // Start is called before the first frame update
    private void Start()
    {
        transform = gameObject.transform;
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (burned)
            return;
        
        if (!onFire)
            return;

        UpdateFire();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (burned && !onFire)
            {
                GetComponent<Collider2D>().enabled = false;
                //TODO Play animation
                _burnedBush.BreakBush(() =>
                {
                    Destroy(gameObject);
                });
                return;
            }
            
            
            if (onFire && CaveMan.isHoldingTorch && !CaveMan.isTorchLit)
                CaveMan.SetTorchLit(true);

            return;
        }
        
        if (!other.gameObject.CompareTag("Torch"))
            return;

        if (!other.gameObject.GetComponent<Torch>().isLit)
            return;

        if (burned || onFire)
            return;
        
        StarFire();

    }

    private void StarFire()
    {
        fire = Instantiate(firePrefab, transform.position, Quaternion.identity).GetComponent<Fire>();
        //TODO Countdown burn time, change color

        onFire = true;
    }

    private void UpdateFire()
    {
        if (timer >= burnTime)
        {
            burned = true;
            renderer.sprite = null;

            _burnedBush = Instantiate(deadBushPrefab, transform.position, Quaternion.identity)
                .GetComponent<BurnedBush>();
            
            
            
            //TODO Change the sprite of the bush to burned one, disable collider
            fire.BurnOut(() =>
            {
                
                onFire = false;
                
            });

            return;
        }

        timer += Time.deltaTime;

        renderer.color = Color.Lerp(startColor, endColor, burnCurve.Evaluate(timer / burnTime));
    }
}
