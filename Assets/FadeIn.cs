 using UnityEngine;
 
 public class FadeIn : MonoBehaviour
 {
     public float alpha = 1f;
     public float speed;     
     public bool fadingIn;

     public Material fadeMaterial;
 
     float start;
     void Awake()
     {
        //fadeMaterial = (fadeShader != null) ? new Material(fadeShader) : new Material(Shader.Find("Transparent/Diffuse"));
        start = Time.time;
     }
     

 
     void OnPostRender()
     {

        if(fadingIn){
            alpha = 1-Mathf.Clamp( (Time.time -start) / speed , 0 , 1 );
        }

         if (alpha > 0)
         {



             fadeMaterial.color = new Color(0f, 0f, 0f, alpha);
            
             fadeMaterial.SetPass(0);
             GL.PushMatrix();
             GL.LoadOrtho();
             GL.Color(fadeMaterial.color);
             GL.Begin(GL.QUADS);
             GL.Vertex3(0f, 0f, -12f);
             GL.Vertex3(0f, 1f, -12f);
             GL.Vertex3(1f, 1f, -12f);
             GL.Vertex3(1f, 0f, -12f);
             GL.End();
             GL.PopMatrix();
         }
     }



    public void FadeInEvent(){
        alpha = 1;
        start = Time.time;
        fadingIn = true;
    }



    

 }