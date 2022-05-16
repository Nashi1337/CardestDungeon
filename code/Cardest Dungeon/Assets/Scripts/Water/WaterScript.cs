using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    //[SerializeField]
    private Vector2 direction;

    private bool isFlowing;
    private float unitsPerSecond; //How many percent of the default sprite size one unity unit equals
    private Vector2 spriteSize;
    private Vector2 scaleDirection;
    private Vector3 startingScale;
    private WaterScript parent;
    [NonSerialized]
    public GameObject pointOfOrigin;

    // Start is called before the first frame update
    void Start()
    {
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
        UpdateDirection(transform.rotation.eulerAngles.z);
        unitsPerSecond = speed / spriteSize.x;
        startingScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlowing)
        {
            Vector3 addedScale = scaleDirection * unitsPerSecond * Time.deltaTime;
            transform.localScale += addedScale;
            float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector3 addedPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * unitsPerSecond * Time.deltaTime * spriteSize * 0.5f;
            transform.position += addedPosition;
        }

        //if(Input.GetKeyDown(KeyCode.F))
        //{
        //    StartFlowing();
        //}
    }

    public void StartFlowing()
    {
        isFlowing = true;
    }

    private void StopFlowing()
    {
        isFlowing = false;
    }

    public void UpdateDirection(float angle)
    {
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        scaleDirection.x = Mathf.Abs(direction.x);
        scaleDirection.y = Mathf.Abs(direction.y);
        direction.Normalize();
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
            angle.z = -Mathf.Acos(flowDirection.x);
        }
        else
        {
            angle.z = Mathf.Acos(flowDirection.x);
        }

        GameObject newWater = null;
        angle.z *= Mathf.Rad2Deg; 
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

        for(int i = 0; i < directions.Length; i++)
        {
            if(directions[i].Equals(-direction) == false)
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

        return directions;
    }
}
