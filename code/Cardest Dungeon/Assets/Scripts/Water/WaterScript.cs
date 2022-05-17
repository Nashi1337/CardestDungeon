using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterScript : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private bool isFlowing;
    private float unitsPerSecond; //How many percent of the default sprite size one unity unit equals
    private Vector2 spriteSize;
    private Vector3 startingScale;
    private WaterScript parent;
    [NonSerialized]
    public GameObject pointOfOrigin;

    // Start is called before the first frame update
    void Start()
    {
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
        unitsPerSecond = speed / spriteSize.x;
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlowing)
        {
            float addedScale = unitsPerSecond * Time.deltaTime;
            transform.localScale += new Vector3(addedScale, 0, 0);
            float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector3 addedPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * unitsPerSecond * Time.deltaTime * spriteSize * 0.5f;
            transform.position += addedPosition;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartFlowing();
        }
    }

    public void StartFlowing()
    {
        isFlowing = true;
    }

    private void StopFlowing()
    {
        isFlowing = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(pointOfOrigin == collision.gameObject)
        {
            return;
        }

        WaterControlPoint wcp = collision.GetComponent<WaterControlPoint>();
        if(wcp != null)
        {
            if (wcp.StopsWater)
            {
                //Don't create new water elements
                return;
            }

            Vector2[] flowDirections = RemoveOriginDirection(wcp.FlowDirections);
            
            foreach (Vector2 flowDirection in flowDirections)
            {
                StartCoroutine(CreateNewWater(flowDirection, collision.gameObject.transform.position, collision.bounds.size.x, collision.gameObject));
            }

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="flowDirection">The normalized direction of the water flow</param>
    /// <param name="origin"></param>
    /// <param name="colliderSize">The diameter of the collider the water collided with</param>
    /// <returns></returns>
    IEnumerator CreateNewWater(Vector2 flowDirection, Vector3 origin, float colliderSize, GameObject controlPoint)
    {
        yield return new WaitForSeconds(0.5f * (spriteSize.x + colliderSize) / speed);
        StopFlowing();
        Vector3 angle = Vector3.zero;
        if(flowDirection.y < 0)
        {
            angle.z = -Mathf.Acos(flowDirection.x) * Mathf.Rad2Deg;
        }
        else
        {
            angle.z = Mathf.Acos(flowDirection.x) * Mathf.Rad2Deg;
        }

        GameObject newWater = null;
        newWater = Instantiate(gameObject, origin, Quaternion.Euler(angle));
        newWater.transform.localScale = startingScale;

        WaterScript waterScript = newWater.GetComponent<WaterScript>();
        waterScript.pointOfOrigin = controlPoint;
        waterScript.parent = this;
        waterScript.StartFlowing();
    }

    /// <summary>
    /// Removes the direction from which the water touched the water control point. Error if water entered from unlisted direction
    /// </summary>
    /// <param name="directions"></param>
    /// <returns>returns the input without the origin direction of the water.</returns>
    private Vector2[] RemoveOriginDirection(Vector2[] directions)
    {
        int index = 0;

        Vector2 direction = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
        for(int i = 0; i < directions.Length; i++)
        {
            Debug.Log(-direction + " ?= " + directions[i] + " = " + (directions[i] ==-direction));
            if(!(directions[i] == -direction))
            {
                directions[index] = directions[i];
                index++;
            }
        }


        //If an element was deleted
        if(index == directions.Length - 1)
        {
            Array.Resize(ref directions, directions.Length - 1);
        }
        Debug.Log("directionen " + directions.Length);
        return directions;
    }
}
