using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Vector2 direction;

    private bool isFlowing;
    private Vector2 spriteSize;
    private Vector2 scaleDirection;

    // Start is called before the first frame update
    void Start()
    {
        isFlowing = true;
        spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
        ChangeDirection(direction);
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlowing)
        {
            Vector3 addedScale = scaleDirection * speed * Time.deltaTime;
            transform.localScale += addedScale;
            Vector3 addedPosition = direction * speed * Time.deltaTime * spriteSize * 0.5f ;
            transform.position += addedPosition;

            Debug.Log("Skalierung: " + scaleDirection + " * " + speed + " * " + Time.deltaTime + " = " + addedScale);
            Debug.Log("Position: " + direction + " * " + speed + " * " + Time.deltaTime  + " * " + spriteSize + " = " + addedPosition);
        }
    }

    public void StartFlowing()
    {
        isFlowing = true;
    }

    public void ChangeDirection(Vector2 direction)
    {
        direction.Normalize();
        this.direction = direction;
        scaleDirection.x = Mathf.Abs(direction.x);
        scaleDirection.y = Mathf.Abs(direction.y);
    }
}
