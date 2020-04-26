using UnityEngine;

public class SlidingWall : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject barTrigger = null;  // bar trigger object
    [SerializeField] private GameObject slidingWall = null;  // sliding wall object
    [SerializeField] private ParticleSystem dust = null;  // dust effect object
    [SerializeField] private float slideSpeed = 1f;  // variable storing wall slide speed

    private Vector3 wallStartPosition;  // wall starting position
    private Vector3 wallEndPosition;  // wall ending position
    private Vector3 wallScale;  // wall object scale
    private Vector3 dustShapeScale;  // dust object scale
    private bool movingWall;  // variable storing flag which is raised while wall is moving

    #endregion

    private void Start()
    {
        var dustShape = dust.shape;  // variable storing dust.shape which is necessary to apply dust scale same as wall scale
        wallScale = slidingWall.transform.localScale;  // initializng variable storing wall size
        dustShapeScale = new Vector3(wallScale.x + 0.3f, wallScale.z + 0.3f);  // setting new dust shape which is .3f bigger than wall size
        dustShape.scale = dustShapeScale;  // setting dust scale to new one
        wallStartPosition = slidingWall.transform.position;  // initializng wall start position
        wallEndPosition = new Vector3(wallStartPosition.x, wallStartPosition.y - 1.1f, wallStartPosition.z);  // initializng wall end position
    }

    private void Update()
    {
        // if flag is set to true, moving wall to wall end position
        if (movingWall)
        {
            slidingWall.transform.position = Vector3.MoveTowards(slidingWall.transform.position, wallEndPosition, slideSpeed * Time.deltaTime);
        }
        // iff wall is in the wall end position, stop moving wall and drop down flag and destroy wall object
        if (slidingWall.transform.position == wallEndPosition)
        {
            movingWall = false;
            Destroy(gameObject);
        }
    }

    // Method responsible for destroying bar trigger object, raising movingWall flag and spawning dust effect object in wall position
    public void BarTrigger()
    {
        Destroy(barTrigger);
        movingWall = true;
        dust.transform.position = new Vector3(slidingWall.transform.position.x, -.3f, slidingWall.transform.position.z);
        dust.Play();
    }
}
