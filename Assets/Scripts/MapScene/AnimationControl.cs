using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private static List<Anima> AnimaList = new List<Anima>();
    public static float moveTime = 0.2f;

    private class Anima
    {
        GameObject gameObject;
        Vector2 startPoint,endPoint;
        public float time = moveTime;

        public Anima(GameObject gameObject, int oldX, int oldY, int newX, int newY)
        {
            this.gameObject = gameObject;
            startPoint = new Vector2(oldX, oldY);
            endPoint = new Vector2(newX, newY);
        }

        public void move(float deltaTime)
        {
            time -= deltaTime;
            if (time < 0) time = 0;
            gameObject.transform.position = endPoint - (endPoint - startPoint) * time/ moveTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBusy())
        {
            foreach (Anima anima in AnimaList)
            {
                anima.move(Time.deltaTime);
            }
            for (int i=0; i< AnimaList.Count; i++) 
            {
                if(AnimaList[i].time == 0)
                {
                    AnimaList.RemoveAt(i);
                    i--;
                    if (!isBusy()) break;
                }
            }
        }
    }

    public static bool isBusy()
    {
        return AnimaList.Count > 0;
    }

    public static void addMapMove(GameObject gameObject, int oldX, int oldY, int newX, int newY)
    {
        if(gameObject != null) AnimaList.Add(new Anima(gameObject, oldX, oldY, newX, newY));
    }
}
