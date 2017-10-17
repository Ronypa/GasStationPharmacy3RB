package com.example.bryan.gasstationfarmacy;
import android.app.Application;

/**
 * Clase que se instancia para tener acceso a sus atributos que son el token y la instancia de la clase que realiza POSTs y GETs al servidor
 */
public class Global extends Application{
    private String Token;
    private PostGetRequest postGetRequest =new PostGetRequest();
    public String getToken(){return Token;}
    public void setToken(String token){Token = token;}
    public PostGetRequest getPostGetRequest(){return postGetRequest;}
}
