package com.example.bryan.gasstationfarmacy;

import android.content.Intent;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.v7.app.AppCompatActivity;
import android.text.InputType;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.Toast;

/**
 * Clase que contiene la lògica de la vista para cambiar la contraseña se un usuario
 */
public class CambioDeContrasena extends AppCompatActivity implements View.OnClickListener{
    ImageButton btnContrasenaActual, btnContrasenaNueva1,btnContrasenaNueva2;//Se declaran los botones necesarios
    EditText contrasenaActual,contrasenaNueva1, contrasenaNueva2;//Se declaran los campos de texto necesarios
    boolean cActual,cNueva1,cNueva2;//Estos booleanos es para saber si la contraseña esta visible o no visible
    Global global;

    /**
     * Metodo que se ejecuta al instanciar o crear una ventana para cambiar ka contraseña
     * @param savedInstanceState:
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_cambio_contrasena);

        global = (Global)getApplicationContext();
        System.out.println("Cambio contraseña " + global.getToken());

        //Intancia un actionBar y agrega el boton de ir a atras
        android.support.v7.app.ActionBar actionBar= getSupportActionBar();
        actionBar.setDisplayHomeAsUpEnabled(true);

        //Inicializacion de los botones con su respectivo id en el layout
        btnContrasenaActual = (ImageButton)findViewById(R.id.btnCA);
        btnContrasenaNueva1 = (ImageButton)findViewById(R.id.btnCN1);
        btnContrasenaNueva2 = (ImageButton)findViewById(R.id.btnCN2);

        //Inicializacion de los campos de texto con su resoectivo id en el layout
        contrasenaActual = (EditText)findViewById(R.id.contrasenaActual);
        contrasenaNueva1 = (EditText)findViewById(R.id.contrasenaNueva1);
        contrasenaNueva2 = (EditText)findViewById(R.id.contrasenaNueva2);

        //Agrega un listener para realizar una accion cuando se presiona un boton
        btnContrasenaActual.setOnClickListener(this);
        btnContrasenaNueva1.setOnClickListener(this);
        btnContrasenaNueva2.setOnClickListener(this);

        //Se agregan las imagenes a los botones
        btnContrasenaActual.setImageResource(R.drawable.contrasenanovisible1);
        btnContrasenaNueva1.setImageResource(R.drawable.contrasenanovisible1);
        btnContrasenaNueva2.setImageResource(R.drawable.contrasenanovisible1);

        //Se inicializan las variables booleanas en false ya que la contraseña no està visible
        cActual=cNueva1=cNueva2=false;
    }

    /**
     * Agrega un menu al action bar para verificar los datos y ejecutar la modificacion de la contraseña
     * @param menu: menu que se va a usar en el actionBar
     * @return:
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.menu_contrasena,menu);
        return super.onCreateOptionsMenu(menu);
    }

    /**
     * Este metodo se ejecuta cuando se selecciona un item dek menu del actionBar en este caso solo posee un item
     * @param item: Item seleccionado
     * @return:
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        //Se obtiene el ID del item selesccionado
        int id = item.getItemId();
        //Si es el item de verificar y ejecutar la actualizacion hace los siguientes pasos
        if(id==R.id.verificarContrasena){
            //Verifica que los campos no esten vacios
            if(contrasenaActual.getText().toString().equals("")||contrasenaNueva1.getText().toString().equals("")
                    ||contrasenaNueva2.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe llenar todos los campos",Toast.LENGTH_LONG).show();
            }

            else if(!contrasenaNueva1.getText().toString().equals(contrasenaNueva2.getText().toString())){
                Toast.makeText(getApplicationContext(),"Su nueva contraseña no coincide",Toast.LENGTH_LONG).show();
            }//Se verifica que la contraseña actual este correcta
            //else if(contrasenaActual.getText().toString() no conincide con la de la base de datos?){Toast.makeText(getApplicationContext(),"Contraseña actual incorrecta",Toast.LENGTH_LONG).show();}
            //Si se cumplen las condiciones anteriores entonces se actualiza la contraseña en la base de datos y se vuekve a la vista de clientes
            else{
                global.getPostGetRequest().cambiarContrasena(contrasenaActual.getText().toString(), contrasenaNueva1.getText().toString());
                StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
                StrictMode.setThreadPolicy(policy);
                String resultado = global.getPostGetRequest().Post(global.getToken());

                if (resultado.equals("1")) {
                    Toast.makeText(getApplicationContext(),"Contraseña actualizada exitosamente",Toast.LENGTH_LONG).show();
                    startActivity(new Intent(getBaseContext(), MainActivity.class));
                }else{
                    Toast.makeText(getApplicationContext(),"Contraseña actual incorrecta",Toast.LENGTH_LONG).show();
                }

            }
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    /**
     * Este metodo pertenece al listener de la clase, cuando se clickea algun elemento dentro de la oantlla que implemmente este tipo de listener se podrà realizar una accion
     * @param v: Elemento de la pantalla clickeado
     */
    @Override
    public void onClick(View v) {
        //Las siguientes condiciones detectan cual de los botones para visualizar la contraseña ha sido clickeado
        //Dentro de cada condicion de estas hay otra condicion que usa los booleanos declarados al inicio
        //Si el booleano es false: se cambia la imagen del boton y se cambia el tipo de entrada a al campo de texto que corresponde el boton a tipo texto para poder visualizar la contraseña
        //Si el booleano es verdadero: se cambia la imagen y el tipo de entrada del campo de texto correspondiente a tipo password
        //Ademas se cambia el booleano al tipo contrario
        if(v==btnContrasenaActual){
            if(!cActual){
               btnContrasenaActual.setImageResource(R.drawable.contrasenavisible1);
                contrasenaActual.setInputType(InputType.TYPE_CLASS_TEXT);
                cActual=true;
            }else{
                btnContrasenaActual.setImageResource(R.drawable.contrasenanovisible1);
                contrasenaActual.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);
                cActual=false;
            }
        }else if(v==btnContrasenaNueva1){
            if(!cNueva1){
                btnContrasenaNueva1.setImageResource(R.drawable.contrasenavisible1);
                contrasenaNueva1.setInputType(InputType.TYPE_CLASS_TEXT);
                cNueva1=true;
            }else{
                btnContrasenaNueva1.setImageResource(R.drawable.contrasenanovisible1);
                contrasenaNueva1.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);
                cNueva1=false;
            }
        }else if(v==btnContrasenaNueva2){
            if(!cNueva2){
                btnContrasenaNueva2.setImageResource(R.drawable.contrasenavisible1);
                contrasenaNueva2.setInputType(InputType.TYPE_CLASS_TEXT);
                cNueva2=true;
            }else{
                btnContrasenaNueva2.setImageResource(R.drawable.contrasenanovisible1);
                contrasenaNueva2.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);
                cNueva2=false;
            }
        }
    }
}
