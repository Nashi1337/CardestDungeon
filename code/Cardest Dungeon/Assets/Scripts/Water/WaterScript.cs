using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterScript : MonoBehaviour
{
    [SerializeField]
    private GameObject streamPrefab;
    [SerializeField]
    private GameObject sourcePrefab;
    [SerializeField]
    public float speed;
    [SerializeField]
    private bool isSource = false;
    [SerializeField]
    private AudioSource waterSound;
    public int isFlowing = 1; //Flowing modifier.
    private float unitsPerSecond; //How many percent of the default sprite size one unity unit equals
    private Vector2 spriteSize;
    private Vector3 startingPosition;
    private Vector3 startingScale;
    private WaterScript[] children;
    [NonSerialized]
    public GameObject pointOfOrigin;
    private SpriteRenderer spriteRender;


    void Start()
    {
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
        unitsPerSecond = speed / spriteSize.x;
        startingPosition = transform.position;
        startingScale = transform.localScale;
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlowing != 0)
        {
            if (isFlowing == -1) //if drying out
            {
                if (Mathf.Abs(transform.localScale.x) < 0.5f)
                {
                    //Destroy self and tell all children to dry out.
                    Destroy(gameObject);
                    if (children != null && children.Length != 0)
                    {
                        foreach (WaterScript stream in children)
                        {
                            stream.DryOut();
                        }
                    }
                }
            }
            if (waterSound != null && !waterSound.isPlaying)
                waterSound.Play();
            
            //flow
            float addedScale = unitsPerSecond * Time.deltaTime * isFlowing;
            transform.localScale += new Vector3(addedScale, 0, 0);
            float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector3 addedPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * unitsPerSecond * Time.deltaTime * spriteSize * 0.5f;
            transform.position += addedPosition;
        }
    }

    public void StartFlowing()
    {
        if (waterSound != null && !waterSound.isPlaying)
            PlayWaterSound();
        isFlowing = 1;
    }

    private void StopFlowing()
    {
        if (waterSound != null)
            StopWaterSound();
        isFlowing = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ignore origin
        if(pointOfOrigin == collision.gameObject || isFlowing < 0)
        {
            return;
        }

        WaterControlPoint wcp = collision.GetComponent<WaterControlPoint>();
        if(wcp != null)
        {
            if (wcp.StopsWater)
            {
                StopFlowing();
                //Don't create new water elements
                return;
            }
            Vector2[] flowDirections = new Vector2[wcp.FlowDirections.Length];
            Array.Copy(wcp.FlowDirections, flowDirections, flowDirections.Length);
            flowDirections = RemoveOriginDirection(flowDirections);
            children = new WaterScript[flowDirections.Length];

            foreach (Vector2 flowDirection in flowDirections)
            {
                StartCoroutine(CreateNewWater(flowDirection, collision.gameObject.transform.position, collision.bounds.size.x, collision.gameObject));
            }

        }
    }

    public void DryOut()
    {
        isFlowing = -1;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>the source that replaces the old source.</returns>
    public WaterScript TrackNewPath()
    {
        DryOut();
        if (isSource)
        {
            GameObject newWater = Instantiate(sourcePrefab, startingPosition, transform.rotation);
            newWater.transform.localScale = startingScale;
            newWater.GetComponent<SpriteRenderer>().size = startingScale;
            newWater.GetComponent<WaterScript>().StartFlowing();
            newWater.GetComponent<WaterScript>().sourcePrefab = sourcePrefab;
            return newWater.GetComponent<WaterScript>();
        }

        return null;
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

        GameObject newWater = Instantiate(streamPrefab, origin, Quaternion.Euler(angle));
        newWater.transform.localScale = startingScale;
        newWater.GetComponent<SpriteRenderer>().size = startingScale;

        AddWaterToChildren(newWater.GetComponent<WaterScript>());

        WaterScript waterScript = newWater.GetComponent<WaterScript>();
        waterScript.pointOfOrigin = controlPoint;
        waterScript.streamPrefab = streamPrefab;
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

        Vector2 directionAsVector = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));
        for (int i = 0; i < directions.Length; i++)
        {
            if (!(directions[i] == -directionAsVector))
            {
                directions[index] = directions[i];
                index++;
            }
        }


        //If an element was deleted
        if (index == directions.Length - 1)
        {
            Array.Resize(ref directions, directions.Length - 1);
        }
        return directions;
    }

    private void AddWaterToChildren(WaterScript water)
    {
        int index = 0;
        while(index < children.Length)
        {
            if(children[index] == null)
            {
                children[index] = water;
                return;
            }
            index++;
        }
        throw new Exception("There was no place left for another child of " + this);
    }

    public void PlayWaterSound()
    {
        waterSound.Play();
        Debug.Log("Watersound is playing");
    }
    public void StopWaterSound()
    {
        Debug.Log("Watersound is not playing");
        waterSound.Stop();
    }

}
