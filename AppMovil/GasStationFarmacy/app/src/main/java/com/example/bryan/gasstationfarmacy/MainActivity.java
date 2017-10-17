package com.example.bryan.gasstationfarmacy;

import android.content.Intent;
import android.graphics.Typeface;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatActivity;
import android.text.InputType;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

/**
 * Main Activity la clase que posee la logica de la ventana principal
 */
public class MainActivity extends AppCompatActivity implements View.OnClickListener {
    //Varibles globales
    private EditText usuario,contrasena;//Campos de texto para el login
    private Button login,register;//Botones de ingresar y registrarse
    private ImageButton contraseñaVisible;//Boton para ver u ocultar la contraseña
    private Global global;//Objeto que va a almacenar el token
    private boolean contVisible=false;//Booleano para saber si la contraseña está visible o no
    private static Retrofit retrofit;//Objeto necesario para realizar concxion con el servidor



    /**
     * Al crear una ventana principal
     * @param savedInstanceState:
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //Intancias de las variables anteriores
        global=(Global) getApplicationContext();
        retrofit = new Retrofit.Builder().baseUrl("http://192.168.100.8/WebApi/").addConverterFactory(GsonConverterFactory.create()).build();
        Typeface arista = Typeface.createFromAsset(getAssets(), "fuentes/Arista.ttf");
        TextView bienvenida = (TextView) findViewById(R.id.bienvenido);
        bienvenida.setTypeface(arista);
        usuario = (EditText) findViewById(R.id.usuario);
        contrasena = (EditText) findViewById(R.id.contrasena);
        login = (Button) findViewById(R.id.login);
        login.setOnClickListener(this);
        register = (Button) findViewById(R.id.register);
        register.setOnClickListener(this);
        contraseñaVisible = (ImageButton)findViewById(R.id.btnCI);
        contraseñaVisible.setOnClickListener(this);
        contraseñaVisible.setImageResource(R.drawable.contrasenanovisible1);
    }

    /**
     * Este método es para que la aplicación se cierre al presionar el boton de atrás
     */
    @Override
    public void onBackPressed() {
        Intent intent = new Intent(Intent.ACTION_MAIN);
        intent.addCategory(Intent.CATEGORY_HOME);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        startActivity(intent);
    }

    /**
     * Este método es para mandarle al servidor la informacion del cliente en el login y verificar que esté registrado
     * @param user: nombre de usuario
     * @param cont: contraseña
     * @param tipo: tipo de
     * @param rol: rol que desempeña(cliente)
     */
    private void executeSendFeedBackForm(String user, final String cont,String tipo,String rol){
        UserClient userClient=retrofit.create(UserClient.class);//Intancia una nueva interfaz userClient
        Call<Result> call = userClient.sendUserFeedback(user,cont,tipo,rol);//Intancia un nuevo call que se utiliza para mandar la informacion al servidor
        call.enqueue(new Callback<Result>() {
            @Override
            public void onResponse(@NonNull Call<Result> call, @NonNull retrofit2.Response<Result> response) {//Si se recibe una respuesta por parte del servidor
                if (response.body() != null) {//si la respuesta no es nula
                    System.out.println("Principal " + response.body().getAccessToken());
                    global.setToken(response.body().getAccessToken());//Se guarda el token que le asigna el servidor
                    startActivity(new Intent(getBaseContext(), VistaCliente.class));//se pasa a la vista general de clientes
                } else {
                    Toast.makeText(getApplicationContext(), "Usuario o contraseña incorrecta", Toast.LENGTH_LONG).show();//Si se recibe respuesta pero nula entonces el usuario o la contraseña está mala
                }
            }

            @Override
            public void onFailure(@NonNull Call<Result> call, @NonNull Throwable throwable) {//Si no se recibe ninguna respuesta
                Toast.makeText(MainActivity.this, "No hay conexión con el servidor", Toast.LENGTH_LONG).show();//Se muestra un mensaje de que no hay conxion con el servidor
            }
        });
    }

    /**
     * Metodo que se ejecuta cuando se da click a un objeto en pantalla
     * @param v: objeto al que se le da click
     */
    @Override
    public void onClick(View v) {
        if(v==login){//Si el objeto es el boton para ingresar ejecuta un metodo que manda la informacion del usuario y la contraseña al servidor para ver si está registrado
            executeSendFeedBackForm(usuario.getText().toString(), contrasena.getText().toString(), "password", "cliente");
        }else if(v==register){//Si el cliente desea registrarse pasa a la ventana registro
            startActivity(new Intent(getBaseContext(), Registro.class));
        }else if(v==contraseñaVisible){//Si el cliente quiere hacer su contraseña visible o ocultarla se ejecuta esta seccion
            if(!contVisible){//Si la contraseña no está visible
                contraseñaVisible.setImageResource(R.drawable.contrasenavisible1);//Cambia la imagen del boton
                contrasena.setInputType(InputType.TYPE_CLASS_TEXT);//Cambia el input type a texto
                contVisible=true;//Cambia el valor del booleano
            }else{//Si la contraseña está visible
                contraseñaVisible.setImageResource(R.drawable.contrasenanovisible1);//Cambia la imagen del boton
                contrasena.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);//Cambia el input type a password
                contVisible=false;//Cambia el valor del booleano
            }
        }
    }
}

