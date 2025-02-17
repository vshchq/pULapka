﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private object[] obj = { };

  
    public void SaveGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("WasGameSaved", 1);
        if (obj.Length > 0)
        {
            System.Array.Clear(obj, 0, obj.Length);
        }
        //tablica przechowujaca wszystkie obiekty
        obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            if (g.activeInHierarchy)
            {
                //dla kazdego aktywnego obiektu sprawdzamy tag (tagi te opisuja rozne typy obiektow, ktore w jakis sposob znikaja z planszy)
                //wszystkie te obiekty zapisujemy do PlayerPrefs, aby moc je potem zaladowac
                if (g.tag == "collectable" || g.tag == "enemy" || g.tag == "healthpotion" || g.tag == "powerup")
                {
                    //zapisywanie obiektow 
                    PlayerPrefs.SetInt(g.name, 1);
                }
                if (g.tag == "player")
                {
                    //zapis pozycji gracza
                    PlayerPrefs.SetFloat("PlayerPositionX", g.transform.position.x);
                    PlayerPrefs.SetFloat("PlayerPositionY", g.transform.position.y);
                }
            }
        }
        //zapis zycia i punktow
        PlayerPrefs.SetInt("health", this.GetComponent<PlayerController>().getHealth());
        PlayerPrefs.SetInt("ects", this.GetComponent<PlayerController>().getECTS());
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("WasGameSaved"))
        {
            object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
            foreach (object o in obj)
            {
                GameObject g = (GameObject)o;

                if (g.tag == "collectable" || g.tag == "enemy" || g.tag == "healthpotion" || g.tag == "powerup")
                {
                    if (!PlayerPrefs.HasKey(g.name))
                    {
                        //jezeli obiekt nie jest zapisany w PlayerPrefs, to znaczy, ze w chwili zapisu nie byl aktywny, a wiec usuwamy go z planszy, aby przywrocic stan z momentu zapisu
                        Destroy(g);
                    }
                }
                if (g.tag == "player")
                {
                    //ustawiamy pozycje gracza na pozycje ostatniego zapisu
                    float posX = PlayerPrefs.GetFloat("PlayerPositionX");
                    float posY = PlayerPrefs.GetFloat("PlayerPositionY");
                    g.transform.position = new Vector2(posX, posY + 1);
                }
            }
        }
    }
}
