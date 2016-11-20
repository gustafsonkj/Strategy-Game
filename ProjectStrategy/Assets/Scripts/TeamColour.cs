using UnityEngine;
using System.Collections;

public class TeamColour : MonoBehaviour
{

    public Material MaterialNormal;
    public Material MaterialRed;
    public Material MaterialBlue;

    public Shader ShaderNormal;
    public Shader ShaderAlpha;

    public void SetTeam(int team, Color colorOffset = default(Color), float colorMultiplier = 1)
    {
        if ((team == 1 && MaterialRed == null) || (team == 1 && MaterialBlue == null) || (team != 1 && team != 2 && MaterialNormal == null))
        {
            // Tint
            Color color;
            switch (team)
            {
                case 1: color = Color.blue; break;
                case 2: color = Color.yellow; break;
                default: color = Color.white; break;
            }
            if (GetComponent<Renderer>() != null)
               GetComponent<Renderer>().material.SetColor("_Color", (color - colorOffset) * colorMultiplier);
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                if (r == null)
                    r.material.SetColor("_Color", (color - colorOffset) * colorMultiplier);
            }
        }
        else
        {
            // Change Material
            switch (team)
            {
                case 1:
                    foreach (Renderer r in GetComponentsInChildren<Renderer>())
                        r.material = MaterialRed;
                    break;
                case 2:
                    foreach (Renderer r in GetComponentsInChildren<Renderer>())
                        r.material = MaterialBlue;
                    break;
                default:
                    foreach (Renderer r in GetComponentsInChildren<Renderer>())
                        r.material = MaterialNormal;
                    break;
            }
            GetComponentInChildren<Renderer>().material.SetColor("_Color", (Color.white - colorOffset) * colorMultiplier);

            if (colorOffset.a > 0)
            {
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                    r.material.shader = ShaderAlpha;
            }
            else
            {
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                    r.material.shader = ShaderNormal;
            }
        }
    }
}
