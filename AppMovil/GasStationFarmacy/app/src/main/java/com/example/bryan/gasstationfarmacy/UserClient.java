package com.example.bryan.gasstationfarmacy;

import retrofit2.Call;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.POST;

/**
 * Interfaz para hacer el logeo de clientes usando FormUrlEncoded
 */
public interface UserClient {
    @FormUrlEncoded
    @POST("token")
    Call<Result> sendUserFeedback(
            @Field("username") String username,
            @Field("password") String contrasena,
            @Field("grant_type") String tipo,
            @Field("client_id") String rol
    );
}
