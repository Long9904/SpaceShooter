using UnityEngine;

[System.Serializable]
public class LayerBackground
{
    public Renderer target;
    public Vector2 speed = new Vector2(0.1f, 0f);
}
public class Parallax : MonoBehaviour
{
    public LayerBackground[] layers;
    public static Parallax instance;
    public float globalSpeed = 1f;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        foreach(LayerBackground layer in layers)
        {
            Vector2 offset = layer.target.material.mainTextureOffset;
            offset += layer.speed * Time.deltaTime * globalSpeed;
            layer.target.material.mainTextureOffset = offset;
        }
    }
}
