﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Entity))]

public class Player : MonoBehaviour
{
    Entity e;
    Vector2 cursorPos = Vector2.zero;
    public Vector2 cursorBoundsMax;
    public Vector2 cursorBoundsMin;
    public float cursorSpeed = 50;
    float h = 0;
    void Start()
    {
        e = GetComponent<Entity>();
        SaveData.playerData = new PlayerData(e);
    }
    void Update()
    {
        if (h > 0) h -= Time.deltaTime;
        e.movementHorizontal(Input.GetAxis("Horizontal") * e.movementSpeed);
        if(Input.GetAxis("Switch Item") < 0 && h <= 0) {
            e.setSelectedItem(e.getSelectedItem() - 1);
            h = 0.1f;
        }
        //cursorPos += new Vector2(Input.GetAxis("Cursor Horizontal") * cursorSpeed * Time.deltaTime, Input.GetAxis("Cursor Vertical") * cursorSpeed * Time.deltaTime);
        cursorPos = new Vector2(Mathf.Clamp(cursorPos.x + (Input.GetAxis("Cursor Horizontal") * cursorSpeed * Time.deltaTime), cursorBoundsMin.x, cursorBoundsMax.x), Mathf.Clamp(cursorPos.y + (Input.GetAxis("Cursor Vertical") * cursorSpeed * Time.deltaTime), cursorBoundsMin.y, cursorBoundsMax.y));
        if(Input.GetAxis("Switch Item") > 0 && h <= 0) {
            e.setSelectedItem(e.getSelectedItem() + 1);
            h = 0.1f;
        }
        
        if(Input.GetAxis("Jump") > 0) {
            e.jump(e.jumpForce * Input.GetAxis("Jump"));
        }   
        if(Input.GetAxis("Fire1") > 0 || Input.GetMouseButton(0)) {
            e.useItem(e.getSelectedItem(), (Vector2)Camera.main.transform.position + cursorPos );
        }
        if(Input.GetAxis("Fire1") > 0) {
            e.mineBlock(e.selectedBlock);
        }
    
        Vector3 pz = Camera.main.transform.position + (Vector3)cursorPos;
        pz.z = 0;

        e.placeBlockPosition = pz;

        GameObject.Find("BPD").transform.position = (Vector3)e.placeBlockPosition + new Vector3(0, 0, 100);

        if(Input.GetButtonDown("Drop")) {
            e.dropItem(e.getSelectedItem(), false);
        }
        
        
        if(e.getSelectedItem() > e.storedItems.Count-1) {
            e.setSelectedItem(e.storedItems.Count - 1);
            return;
        }
        if(Input.GetAxis("Place") > 0) {
            if (e.startBlockPlace(Resources.Load<GameObject>("Prefabs/Block-" + e.storedItems[e.getSelectedItem()].id), (int)Mathf.Round(pz.x), (int)Mathf.Round(pz.y)))
            e.consumeItem(e.getSelectedItem());
        }
    }
}