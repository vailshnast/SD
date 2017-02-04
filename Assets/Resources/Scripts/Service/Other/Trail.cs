using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Vectrosity;
using UnityEngine.EventSystems;
public class Trail : MonoBehaviour
{
    public GestureSet myGestures;

    public float lineDistance = 0;
    public float minDrawDist = 0;


    private Spell_System spell_System;

    private HyperGlyphResult match;
    private VectorObject2D line;
    VectorLine copy;
    private float hue, saturation, value;
    
    private Vector2 pos;

    void Start()
    {
        spell_System = FindObjectOfType<Spell_System>();
        HyperGlyph.Init(myGestures);
        line = GetComponent<VectorObject2D>();
        
        line.vectorLine.points2.Clear();
        
        line.vectorLine.Draw();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            line.vectorLine.color = Color32_HSV_To_RGB(Random.Range(0, 1f), Random.Range(0.2f, 0.5f), 1f);
            if(copy !=null)
            copy.color = line.vectorLine.color;
        }

        if (Input.GetMouseButtonUp(0))
        {
            match = HyperGlyph.Recognize();

            spell_System.CastSpell(match.glyphname);

            line.vectorLine.points2.Clear();
            copy.Draw();

        }
        if (Input.GetMouseButton(0))
        {
            Draw();
        }
   
    }
    /*
    public void drawEffect()
    {
        StartCoroutine(drawEffectCoroutine());
    }

    private IEnumerator drawEffectCoroutine()
    {
        VectorLine copy;

        copy = new VectorLine("copy", line.vectorLine.points2, line.vectorLine.lineWidth, LineType.Continuous, Joins.Fill);
        Color.RGBToHSV(line.vectorLine.color, out hue, out saturation, out value);
        copy.color = Color32_HSV_To_RGB(hue, 1f, value);
        
        copy.rectTransform.SetParent(GameObject.Find("Lines").transform);
        copy.Draw();
        yield return new WaitForSeconds(0.5f);

        Destroy(copy.rectTransform.gameObject);
    }

    */
    public Color32 Color32_HSV_To_RGB(float hue,float saturation,float value)
    {
        Color color = Color.HSVToRGB(hue,saturation,value);
        return new Color32((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), 255);
    }

    public float dist()
    {
        float dist = 0;

        for (int i = 0; i < line.vectorLine.points2.Count; i++)
        {
            if (i > 1)
                dist += Vector2.Distance(line.vectorLine.points2[i - 1], line.vectorLine.points2[i]);           
        } 
        return dist;
    }

    private void Draw()
    {
        if (Application.platform == RuntimePlatform.Android)
            pos = Input.GetTouch(0).position;
        else
            pos = Input.mousePosition;

        

        line.vectorLine.smoothWidth = true;

        if (line.vectorLine.points2.Count == 0)
        {
            HyperGlyph.AddPoint(pos);
            line.vectorLine.points2.Add(pos);
        }
        else if (!line.vectorLine.points2.Contains(pos) && Vector2.Distance(line.vectorLine.points2[line.vectorLine.points2.Count - 1], pos) > lineDistance)
        {
            HyperGlyph.AddPoint(pos);
            line.vectorLine.points2.Add(pos);
        }

        if (copy == null)
        {
            Debug.Log("1");
            copy = new VectorLine("copy", line.vectorLine.points2, line.vectorLine.lineWidth, LineType.Continuous, Joins.Fill);
            copy.color = line.vectorLine.color;
            copy.rectTransform.SetParent(GameObject.Find("Lines").transform);
        }
        if (dist() > minDrawDist)
            copy.Draw();
    }
}
