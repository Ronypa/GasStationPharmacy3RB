package com.example.bryan.gasstationfarmacy;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.Spinner;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

/**
 * En esta ventana el cliente es capaz de actualizar sus datos en la base de datos
 */
public class ActualizacionDeDatos extends AppCompatActivity implements View.OnClickListener {
    //Declaracion de variables globales
    private String[] provincias = new String[] {"San José","Cartago","Alajuela","Heredia","Puntarenas","Guanacaste","Limón"};//Arreglo para el spinner de provincia
    private EditText nombre1,nombre2,apellido1,apellido2,ciudad,senas,nacimiento, telefono,padecimiento,anoPadece;//Campos de texto para llenar con la informacion del cliente
    private Spinner provincia; //Spinner con las provincias
    private int dia,mes,ano; //Variables para capturar la fecha de nacimiento
    Date date;
    DateFormat dateFormat;
    private ImageButton masTelefono,menosTelefono, masPadecimiento, menosPadecimiento; //Botones para agregar/eliminar telefonos/padecimientos
    private LinearLayout layo,layoPadecimientos,layoAno; // Layouts que contienen a los padecimientos y año en el que padeció
    private ArrayList<EditText> telefonosAdicionles,padecimientosAdicionales,anoPadecimiento; //Lista con los campos de texto de los telefonos y los padecimientos
    private Global global; //Variable que almacena el token y que tiene la instancia de la clase que hace POST y GET al servidor

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_actualizacion_datos);
        android.support.v7.app.ActionBar actionBar= getSupportActionBar();
        actionBar.setDisplayHomeAsUpEnabled(true);

        //Instancias de las variables anteriores
        global = (Global)getApplicationContext();
        nombre1 = (EditText)findViewById(R.id.nombre1A);
        nombre2 = (EditText)findViewById(R.id.nombre2A);
        apellido1 = (EditText)findViewById(R.id.apellido1A);
        apellido2 = (EditText)findViewById(R.id.apellido2A);
        provincia = (Spinner)findViewById(R.id.provinciaA);
        ciudad = (EditText)findViewById(R.id.ciudadA);
        senas = (EditText)findViewById(R.id.senasA);
        nacimiento = (EditText)findViewById(R.id.nacimientoA);
        telefono = (EditText)findViewById(R.id.telefonoA);
        padecimiento = (EditText)findViewById(R.id.padecimientoA);
        anoPadece = (EditText)findViewById(R.id.anoPadecimientoA);
        nacimiento.setOnClickListener(this);
        masTelefono = (ImageButton)findViewById(R.id.masTelefonoA);
        masPadecimiento = (ImageButton)findViewById(R.id.masPadecimientoA);
        menosTelefono = (ImageButton)findViewById(R.id.menosTelefonoA);
        menosPadecimiento = (ImageButton)findViewById(R.id.menosPadecimientoA);
        masTelefono.setOnClickListener(this);
        masPadecimiento.setOnClickListener(this);
        menosTelefono.setOnClickListener(this);
        menosPadecimiento.setOnClickListener(this);
        layo = (LinearLayout)findViewById(R.id.layoA);
        layoPadecimientos = (LinearLayout)findViewById(R.id.layoPadecimientoA);
        layoAno = (LinearLayout)findViewById(R.id.layoAnoA);
        telefonosAdicionles=new ArrayList<>();
        padecimientosAdicionales = new ArrayList<>();
        anoPadecimiento=new ArrayList<>();
        ArrayAdapter<String> adapterProvincias = new ArrayAdapter<>(getBaseContext(), android.R.layout.simple_spinner_item, provincias);//Adapter que popula el spinner de provincias
        provincia.setAdapter(adapterProvincias);
        //Se hace un GET en el OnCreate para llenar los campos con los datos de la base de datos del cliente
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);// Esquivar problemas con el Main Thread
        global.getPostGetRequest().modificarURLGet("consultarCliente");
        JSONArray respuesta = global.getPostGetRequest().Get(global.getToken());//GET al servidor de los datos del cliente
        descifrarInformacion(respuesta);//ejecuta el siguiente método para llenar los campos con la informacion del cliente

        dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
        date = new Date();
        dia=Integer.parseInt(dateFormat.format(date).substring(8,10));
        mes =Integer.parseInt(dateFormat.format(date).substring(5,7));
        ano=Integer.parseInt(dateFormat.format(date).substring(0, 4));
    }

    //Crea un menu en el Action Bar
    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.menu_actualizar_datos,menu);
        return super.onCreateOptionsMenu(menu);
    }

    //Se ejecuta si se selecciona una opcion del menu del Action Bar
    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        int id = item.getItemId();
        if(id==R.id.verificarDatos){//Este if se cumple si se presiona el boton con icono de check en el action bar para verificar y guardar los nuevos datos
            if(nombre1.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe ingresar su nombre",Toast.LENGTH_LONG).show();
            }else if(apellido1.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe ingresar su apellido",Toast.LENGTH_LONG).show();
            }else if(nacimiento.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe ingresar su fecha de nacimiento",Toast.LENGTH_LONG).show();
            }else if(telefono.getText().toString().equals("")&&noHay()){
                Toast.makeText(getApplicationContext(),"Debe ingresar al menos un número teléfono",Toast.LENGTH_LONG).show();
            }else {
                //Se se cumplen las condiciones anteriores se verifica que no hayan telefonos o padecimientos repetidos y telefonos con más o menos de 8 digitos
                boolean telefonosRepetidos,telefonosIncorrectos,padecimientosRepetidos;
                telefonosRepetidos=telefonosIncorrectos=padecimientosRepetidos=false;
                //Verifica que los telefonos tengan 8 digitos
                ArrayList<String> telefonos=new ArrayList<>();
                if(!telefono.getText().toString().equals("")){
                    telefonos.add(telefono.getText().toString());
                    if(telefono.getText().toString().length()!=8){
                        telefonosIncorrectos=true;
                    }
                }
                //Verifica que los telefonos adicionales tengan 8 digitos
                if(telefonosAdicionles.size()!=0){
                    for(int i=0;i< telefonosAdicionles.size();++i){
                        if(!telefonosAdicionles.get(i).getText().toString().equals("")){
                            telefonos.add(telefonosAdicionles.get(i).getText().toString());
                            if(telefonosAdicionles.get(i).getText().toString().length()!=8){
                                telefonosIncorrectos=true;
                            }
                        }
                    }
                    //Verifica que los telefonos no esten repetidos
                    for(int i=0;i<telefonos.size()-1;++i){
                        for(int j=i+1;j<telefonos.size();++j){
                            if(telefonos.get(i).equals(telefonos.get(j))){
                                telefonosRepetidos=true;
                                break;
                            }
                        }
                    }
                }
                //Se verifica que los medicamentos no se repitan en nombre y año
                ArrayList<String> padece=new ArrayList<>();
                ArrayList<String> ano=new ArrayList<>();
                if(!padecimiento.getText().toString().equals("")){
                    padece.add(padecimiento.getText().toString());
                    ano.add(anoPadece.getText().toString());
                }
                if(padecimientosAdicionales.size()!=0){
                    for(int i=0;i< padecimientosAdicionales.size();++i){
                        if(!padecimientosAdicionales.get(i).getText().toString().equals("")){
                            padece.add(padecimientosAdicionales.get(i).getText().toString());
                            ano.add(anoPadecimiento.get(i).getText().toString());
                        }
                    }
                    for(int i=0;i<padece.size()-1;++i){
                        for(int j=i+1;j<padece.size();++j){
                            if(padece.get(i).equals(padece.get(j))){
                                if((!ano.get(i).equals("")&&!ano.get(j).equals(""))&&ano.get(i).equals(ano.get(j))){
                                    padecimientosRepetidos=true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if(telefonosRepetidos||telefonosIncorrectos) {//Si existe algun telefono repetido o incorrecto se notifica
                    Toast.makeText(getApplicationContext(), "Existe algún teléfono repetido o incorrecto", Toast.LENGTH_LONG).show();
                }else if(padecimientosRepetidos){//Si existe algun padecimiento repetido se notifica
                    Toast.makeText(getApplicationContext(),"Existe algún padecimiento repetido",Toast.LENGTH_LONG).show();
                }else{
                    //Si toda la información digitada está correcta se envia a la base de datos
                    global.getPostGetRequest().actualizarDatos(nombre1.getText().toString(), nombre2.getText().toString(), apellido1.getText().toString(), apellido2.getText().toString(), provincia.getSelectedItem().toString(), ciudad.getText().toString(), senas.getText().toString(), nacimiento.getText().toString(), telefonos, padece, ano);
                    //Se obtiene el resultado que emite el servidor
                    if (global.getPostGetRequest().Post(global.getToken()).equals("1")) {//Si se guardó correctamente se notifica y se pasa a la ventana de logeo y registro
                        Toast.makeText(getApplicationContext(), "Datos actualizados", Toast.LENGTH_LONG).show();
                        startActivity(new Intent(getBaseContext(), VistaCliente.class));
                    } else {//Si no se guarda correctamente se notifica
                        Toast.makeText(getApplicationContext(), "Error al actualizar los datos, inténtelo de nuevo", Toast.LENGTH_LONG).show();
                    }
                }
            }
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    /**
     * Si se da click a algun elemento en pantalla
     * @param v: elemento al que se da click
     */
    @Override
    public void onClick(View v) {
        EditText editAgregarEliminar;
        if(v==nacimiento){//Si se dio click para agrgar fecha de nacimiento se crea un datePickerDialog para escoger la fecha de nacimiento
            final Calendar c = Calendar.getInstance();
            dia=c.get(Calendar.DAY_OF_MONTH);
            mes=c.get(Calendar.MONTH);
            ano=c.get(Calendar.YEAR);

            DatePickerDialog datePickerDialog = new DatePickerDialog(this, new DatePickerDialog.OnDateSetListener() {
                @Override
                public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
                    nacimiento.setText(dayOfMonth+"/"+(monthOfYear+1)+"/"+year);
                }
            }
                    ,ano-20,mes,dia);
            datePickerDialog.getDatePicker().setMinDate(anosMinimo());//Se elige una fecha minima
            datePickerDialog.getDatePicker().setMaxDate(anosMaximo());//Se elige una fecha maxima
            datePickerDialog.show();
        }else if(v==masTelefono){//Si se presiona el boton para agregar teléfono verifica que los espacios en blanco ya hayan sido llenados, si es así crea un editText nuevo
            if(telefonosAdicionles.size()==0){
                if(telefono.getText().toString().equals("")){
                    Toast.makeText(this, "Debe llenar el espacio anterior primero", Toast.LENGTH_LONG).show();
                }else{
                    agregarTelefono();
                }
            }else{
                if(telefono.getText().toString().equals("")||hayVacios()){
                    Toast.makeText(this,"Debe llenar los espacios en blanco primero",Toast.LENGTH_LONG).show();
                }else{
                    agregarTelefono();
                }
            }
        }else if(v==masPadecimiento){//Si se presiona el boton para agregar padecimientos verifica que los espacios en blanco ya hayan sido llenados, si es así crea un editText nuevo
            if(padecimientosAdicionales.size()==0) {
                if (padecimiento.getText().toString().equals("")) {
                    Toast.makeText(this, "Debe llenar el espacio anterior primero", Toast.LENGTH_LONG).show();
                }else{
                    agregarPadecimiento();
                }
            }else{
                if(padecimientosAdicionales.get(padecimientosAdicionales.size()-1).getText().toString().equals("")){
                    Toast.makeText(this,"Debe llenar el espacio anterior primero",Toast.LENGTH_LONG).show();
                }else{
                    agregarPadecimiento();
                }
            }
        }else if(v==menosTelefono){//Si se presiona el boton para eliminar telefono, se eliminar el ultimo editText que se agregó
            if(telefonosAdicionles.size()>0) {
                editAgregarEliminar = telefonosAdicionles.get(telefonosAdicionles.size() - 1);
                layo.removeView(editAgregarEliminar);
                telefonosAdicionles.remove(telefonosAdicionles.size() - 1);
            }
        }else if(v==menosPadecimiento){//Si se presiona el boton para eliminar padecimiento, se eliminar el ultimo editText que se agregó
            if(padecimientosAdicionales.size()>0) {
                editAgregarEliminar = padecimientosAdicionales.get(padecimientosAdicionales.size() - 1);
                layoPadecimientos.removeView(editAgregarEliminar);
                padecimientosAdicionales.remove(padecimientosAdicionales.size() - 1);

                editAgregarEliminar = anoPadecimiento.get(anoPadecimiento.size() - 1);
                layoAno.removeView(editAgregarEliminar);
                anoPadecimiento.remove(anoPadecimiento.size() - 1);
            }
        }
    }

    /**
     * Si se desea agregar nuevos telefonos se crean nuevos campos de texto
     */
    public void agregarTelefono(){
        EditText editAgregarEliminar = new EditText(getApplicationContext());
        editAgregarEliminar.setHint("Teléfono");
        editAgregarEliminar.setWidth(telefono.getWidth());
        editAgregarEliminar.setHeight(telefono.getHeight());
        editAgregarEliminar.setHintTextColor(telefono.getHintTextColors());
        editAgregarEliminar.setTextColor(Color.parseColor("#000000"));
        editAgregarEliminar.setInputType(telefono.getInputType());
        editAgregarEliminar.setTranslationX(5);
        layo.addView(editAgregarEliminar, 13+telefonosAdicionles.size(), telefono.getLayoutParams());
        telefonosAdicionles.add(editAgregarEliminar);
    }

    /**
     * Si se desea agregar mas campos de texto para ingresar padecimientos
     */
    public void agregarPadecimiento(){
        EditText editAgregarEliminar = new EditText(getApplicationContext());
        editAgregarEliminar.setHint("Enfermedad");
        editAgregarEliminar.setWidth(padecimiento.getWidth());
        editAgregarEliminar.setHeight(48);
        editAgregarEliminar.setHintTextColor(padecimiento.getHintTextColors());
        editAgregarEliminar.setTextColor(Color.parseColor("#000000"));
        editAgregarEliminar.setTranslationX(5);
        layoPadecimientos.addView(editAgregarEliminar, padecimientosAdicionales.size() + 1, padecimiento.getLayoutParams());
        padecimientosAdicionales.add(editAgregarEliminar);

        editAgregarEliminar = new EditText(getApplicationContext());
        editAgregarEliminar.setHint("Año");
        editAgregarEliminar.setWidth(anoPadece.getWidth());
        editAgregarEliminar.setHeight(anoPadece.getHeight());
        editAgregarEliminar.setHintTextColor(anoPadece.getHintTextColors());
        editAgregarEliminar.setTextColor(Color.parseColor("#000000"));
        editAgregarEliminar.setTranslationX(5);
        editAgregarEliminar.setInputType(anoPadece.getInputType());
        layoAno.addView(editAgregarEliminar,anoPadecimiento.size()+1, anoPadece.getLayoutParams());
        anoPadecimiento.add(editAgregarEliminar);
    }

    /**
     * Descifra el JSON enviado por el servidor con la informacion del cliente
     * @param respuesta: JSON que envia el servidor
     */
    private void descifrarInformacion(JSONArray respuesta) {
        try {
            JSONObject informacion;
            informacion=respuesta.getJSONObject(0);
            nombre1.setText(informacion.getString("nombre1"));
            if(!informacion.getString("nombre2").equals("null")){
                nombre2.setText(informacion.getString("nombre2"));
            }
            apellido1.setText(informacion.getString("apellido1"));
            if(!informacion.getString("apellido2").equals("null")){
                apellido2.setText(informacion.getString("apellido2"));
            }
            for(int i=0;i<provincias.length;++i){
                if(informacion.getString("provincia").equals(provincias[i])){
                    provincia.setSelection(i);
                }
            }
            if(!informacion.getString("ciudad").equals("null")){
                ciudad.setText(informacion.getString("ciudad"));
            }
            if(!informacion.getString("senas").equals("null")){
                senas.setText(informacion.getString("senas"));
            }
            nacimiento.setText(informacion.getString("fechaNacimiento")
                    .substring(0, informacion.getString("fechaNacimiento").length() - 9));
            JSONArray telPad;
            telPad = informacion.getJSONArray("telefonos");
            telefono.setText(telPad.get(0).toString());
            if(telPad.length()>1){
                for(int i=1;i<telPad.length();++i){
                    agregarTelefono();
                    telefonosAdicionles.get(i - 1).setText(telPad.get(i).toString());
                }
            }
            telPad = informacion.getJSONArray("padecimientos");
            padecimiento.setText(telPad.getJSONObject(0).getString("padecimiento"));
            anoPadece.setText(telPad.getJSONObject(0).getString("año"));
            if(telPad.length()>1){
                for(int i=1;i<telPad.length();++i){
                    agregarPadecimiento();
                    padecimientosAdicionales.get(i-1).setText(telPad.getJSONObject(i).getString("padecimiento"));
                    if(!telPad.getJSONObject(i).getString("año").equals("0")){
                        anoPadecimiento.get(i-1).setText(telPad.getJSONObject(i).getString("año"));
                    }
                }
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * @return: retorna en long del tiempo de hoy hace cien años
     */
    public long anosMinimo(){
        Calendar calendar = new GregorianCalendar();
        calendar.set(Calendar.DAY_OF_MONTH, dia);
        calendar.set(Calendar.MONTH, mes);
        calendar.set(Calendar.YEAR, ano-100);
        calendar.set(Calendar.HOUR_OF_DAY,0);
        calendar.set(Calendar.MINUTE,0);
        calendar.set(Calendar.SECOND,0);
        return calendar.getTimeInMillis();
    }

    /**
     * @return: retorna en long del tiempo de hoy hace un año
     */
    public long anosMaximo(){
        Calendar calendar = new GregorianCalendar();
        calendar.set(Calendar.DAY_OF_MONTH, dia);
        calendar.set(Calendar.MONTH, mes);
        calendar.set(Calendar.YEAR, ano-1);
        calendar.set(Calendar.HOUR_OF_DAY,0);
        calendar.set(Calendar.MINUTE,0);
        calendar.set(Calendar.SECOND,0);
        return calendar.getTimeInMillis();
    }

    /**
     * @return: verifica si no hay telefonos
     */
    public boolean noHay(){
        for(int i=0;i<telefonosAdicionles.size();++i){
            if(!telefonosAdicionles.get(i).getText().toString().equals("")){
                return false;
            }
        }
        return true;
    }

    /**
     * @return: verifica si hay camposd etexto de telefono vacios
     */
    public boolean hayVacios(){
        for(int i=0;i<telefonosAdicionles.size();++i){
            if(telefonosAdicionles.get(i).getText().toString().equals("")){
                return true;
            }
        }
        return false;
    }
}
