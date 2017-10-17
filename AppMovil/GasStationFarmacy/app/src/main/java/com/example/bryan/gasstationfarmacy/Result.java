package com.example.bryan.gasstationfarmacy;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

/**
 * Clase para capturar la respuesta del servidor al un cliente logearse
 */
public class Result {
    @SerializedName("access_token")
    @Expose
    private String accessToken="";

    //-----------Getters and Setters---------------//
    public String getAccessToken(){return accessToken;}
}