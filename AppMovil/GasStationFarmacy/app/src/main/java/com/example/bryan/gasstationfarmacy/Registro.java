package com.example.bryan.gasstationfarmacy;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.v7.app.AppCompatActivity;
import android.text.InputType;
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

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

/**
 * Activity para registrar clientes
 */
public class Registro extends AppCompatActivity implements View.OnClickListener {
    private String[] provincias = new String[] {"San José","Cartago","Alajuela","Heredia","Puntarenas","Guanacaste","Limón"};
    private EditText cedula,nombre1,nombre2,apellido1,apellido2,ciudad,senas,nacimiento, telefono,padecimiento,anoPadece,contrasenaEdit,contrasenaVerificacionEdit;
    private Spinner provincia;
    private ImageButton masTelefono,menosTelefono, masPadecimiento, menosPadecimiento;
    private LinearLayout layo,layoPadecimientos,layoAno;
    private ImageButton btnContrasena, btnContrasenaVerificacion;
    private ArrayList<EditText> telefonosAdicionles,padecimientosAdicionales,anoPadecimiento;
    private boolean contrasena,contrasenaVerificacion;
    private int dia,mes,ano;
    private Global global;

    /**
     * Cuando se crea el activity
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);
        android.support.v7.app.ActionBar actionBar= getSupportActionBar();
        assert actionBar != null;
        actionBar.setDisplayHomeAsUpEnabled(true);

        //Intancia de las variables anteriores y elementos para poder registrarse (Campos de texto, botones)
        global = (Global)getApplicationContext();
        cedula = (EditText)findViewById(R.id.cedula);
        nombre1 = (EditText)findViewById(R.id.nombre1);
        nombre2 = (EditText)findViewById(R.id.nombre2);
        apellido1 = (EditText)findViewById(R.id.apellido1);
        apellido2 = (EditText)findViewById(R.id.apellido2);
        provincia = (Spinner)findViewById(R.id.provincia);
        ciudad = (EditText)findViewById(R.id.ciudad);
        senas = (EditText)findViewById(R.id.senas);
        nacimiento = (EditText)findViewById(R.id.nacimiento);
        telefono = (EditText)findViewById(R.id.telefono);
        padecimiento = (EditText)findViewById(R.id.padecimiento);
        anoPadece = (EditText)findViewById(R.id.anoPadecimiento);
        nacimiento.setOnClickListener(this);
        masTelefono = (ImageButton)findViewById(R.id.masTelefono);
        masPadecimiento = (ImageButton)findViewById(R.id.masPadecimiento);
        menosTelefono = (ImageButton)findViewById(R.id.menosTelefono);
        menosPadecimiento = (ImageButton)findViewById(R.id.menosPadecimiento);
        masTelefono.setOnClickListener(this);
        masPadecimiento.setOnClickListener(this);
        menosTelefono.setOnClickListener(this);
        menosPadecimiento.setOnClickListener(this);
        layo = (LinearLayout)findViewById(R.id.layo);
        layoPadecimientos = (LinearLayout)findViewById(R.id.layoPadecimiento);
        layoAno = (LinearLayout)findViewById(R.id.layoAno);
        ArrayAdapter<String> adapterProvincias = new ArrayAdapter<>(getBaseContext(), android.R.layout.simple_spinner_item, provincias);
        provincia.setAdapter(adapterProvincias);
        btnContrasena = (ImageButton)findViewById(R.id.btnContrasena);
        btnContrasenaVerificacion = (ImageButton)findViewById(R.id.btnContrasenaVerificacion);
        contrasenaEdit = (EditText)findViewById(R.id.contrasenaRegistro);
        contrasenaVerificacionEdit = (EditText)findViewById(R.id.contrasenaRegistroVerificacion);
        btnContrasena.setOnClickListener(this);
        btnContrasenaVerificacion.setOnClickListener(this);
        btnContrasena.setImageResource(R.drawable.contrasenanovisible1);
        btnContrasenaVerificacion.setImageResource(R.drawable.contrasenanovisible1);
        contrasena=contrasenaVerificacion=false;
        DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
        Date date = new Date();
        dia=Integer.parseInt(dateFormat.format(date).substring(8, 10));
        mes =Integer.parseInt(dateFormat.format(date).substring(5, 7));
        ano=Integer.parseInt(dateFormat.format(date).substring(0, 4));
        telefonosAdicionles = new ArrayList<>();
        padecimientosAdicionales = new ArrayList<>();
        anoPadecimiento = new ArrayList<>();
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
    }

    /**
     * Crea el menu ubicado en el action bar
     * @param menu: menu creado
     * @return: menu
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.menu_registro,menu);
        return super.onCreateOptionsMenu(menu);
    }

    /**
     * Se ejecuta cuando se selecciona una opcion del menu en el action bar
     * @param item: item seleccionado
     * @return: si se proceso: true
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        int id = item.getItemId();
        if(id==R.id.verificar){
            //Las siguientes condiciones corresponder a la verificacion de que se digite toda la informacion que debe ingresar el cliente obligatoriamente y que esta esté correcta y completa
            if(cedula.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe ingresar su cédula",Toast.LENGTH_LONG).show();
            }else if(cedula.getText().toString().length()<7){
                Toast.makeText(getApplicationContext(),"Debe ingresar su cédula correctamente",Toast.LENGTH_LONG).show();
            }else if(nombre1.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe ingresar su nombre",Toast.LENGTH_LONG).show();
            }else if(apellido1.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe ingresar su apellido",Toast.LENGTH_LONG).show();
            }else if(nacimiento.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe ingresar su fecha de nacimiento",Toast.LENGTH_LONG).show();
            }else if(telefono.getText().toString().equals("")&&noHayTelefonos()){
                Toast.makeText(getApplicationContext(),"Debe ingresar al menos un número de teléfono",Toast.LENGTH_LONG).show();
            }else if(contrasenaEdit.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe definir una contraseña",Toast.LENGTH_LONG).show();
            }else if(contrasenaVerificacionEdit.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe verificar su contraseña",Toast.LENGTH_LONG).show();
            }else if(!contrasenaEdit.getText().toString().equals(contrasenaVerificacionEdit.getText().toString())){
                Toast.makeText(getApplicationContext(),"Las contraseñas no coinciden",Toast.LENGTH_LONG).show();
            }else{
                //Se se cumplen las condiciones anteriores se verifica que no hayan telefonos o padecimientos repetidos y telefonos con más o menos de 8 digitos
                boolean telefonosRepetidos=false;
                boolean telefonoIncorrecto=false;
                boolean padecimiemtoRepetido=false;
                //Verifica que los telefonos tengan 8 digitos
                ArrayList<String> telefonos=new ArrayList<>();
                if(!telefono.getText().toString().equals("")){
                    telefonos.add(telefono.getText().toString());
                    if(telefono.getText().toString().length()!=8){
                        telefonoIncorrecto=true;
                    }
                }
                //Verifica que los telefonos adicionales tengan 8 digitos
                if(telefonosAdicionles.size()!=0){
                    for(int i=0;i< telefonosAdicionles.size();++i){
                        if(!telefonosAdicionles.get(i).getText().toString().equals("")){
                            telefonos.add(telefonosAdicionles.get(i).getText().toString());
                            if(telefonosAdicionles.get(i).getText().toString().length()!=8){
                                telefonoIncorrecto=true;
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
                                    padecimiemtoRepetido=true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if(telefonosRepetidos||telefonoIncorrecto) {//Si existe algun telefono repetido o incorrecto se notifica
                    Toast.makeText(getApplicationContext(),"Existe algún teléfono repetido o incorrecto",Toast.LENGTH_LONG).show();
                }else if(padecimiemtoRepetido){//Si existe algun padecimiento repetido se notifica
                    Toast.makeText(getApplicationContext(),"Existe algún padecimiento repetido",Toast.LENGTH_LONG).show();
                }else{
                    //Si toda la información digitada está correcta se envia a la base de datos
                    global.getPostGetRequest().agregarCliente(cedula.getText().toString(), nombre1.getText().toString(), nombre2.getText().toString(), apellido1.getText().toString(), apellido2.getText().toString(), provincia.getSelectedItem().toString(), ciudad.getText().toString(), senas.getText().toString(), nacimiento.getText().toString(), telefonos, padece, ano, contrasenaEdit.getText().toString(), "1");
                    //Se obtiene el resultado que emite el servidor
                    if (global.getPostGetRequest().Post("").equals("1")) {//Si se guardó correctamente se notifica y se pasa a la ventana de logeo y registro
                        Toast.makeText(getApplicationContext(), "Registrado exitosamente", Toast.LENGTH_LONG).show();
                        startActivity(new Intent(getBaseContext(), MainActivity.class));
                    } else {//Si no se guarda correctamente se notifica
                        Toast.makeText(getApplicationContext(),"Ya existe un usuario con esta cédula", Toast.LENGTH_LONG).show();
                    }
                }
            }
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    /**
     * Si se da click a algun boton se ejecuta este metodo
     * @param v: elemento al que se dio click
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
                    Toast.makeText(this,"Debe llenar el espacio anterior primero",Toast.LENGTH_LONG).show();
                }else{
                    agregarTelefono();
                }
            }else{
                if(telefono.getText().toString().equals("")||hayTelefonosVacios()){
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
                if(padecimiento.getText().toString().equals("")||hayPadecimientosVacios()){
                    Toast.makeText(this,"Debe llenar los espacios en blanco primero",Toast.LENGTH_LONG).show();
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
        }else if(v==btnContrasena){//Si se elige el boton para observar la contraseña, verifica que este oculta para mostrarla o visible para ocultarla y cambia la imagen del boton segun corresponda
            if(!contrasena){
                btnContrasena.setImageResource(R.drawable.contrasenavisible1);
                contrasenaEdit.setInputType(InputType.TYPE_CLASS_TEXT);
                contrasena=true;
            }else{
                btnContrasena.setImageResource(R.drawable.contrasenanovisible1);
                contrasenaEdit.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);
                contrasena=false;
            }
        }else if(v==btnContrasenaVerificacion){//Si se elige el boton para observar la contraseña de verificacion, verifica que este oculta para mostrarla o visible para ocultarla y cambia la imagen del boton segun corresponda
            if(!contrasenaVerificacion){
                btnContrasenaVerificacion.setImageResource(R.drawable.contrasenavisible1);
                contrasenaVerificacionEdit.setInputType(InputType.TYPE_CLASS_TEXT);
                contrasenaVerificacion=true;
            }else{
                btnContrasenaVerificacion.setImageResource(R.drawable.contrasenanovisible1);
                contrasenaVerificacionEdit.setInputType(InputType.TYPE_CLASS_TEXT | InputType.TYPE_TEXT_VARIATION_PASSWORD);
                contrasenaVerificacion=false;
            }
        }
    }

    /**
     * Metodo para agregar editTexts para telefonos adicionales
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
        layo.addView(editAgregarEliminar, 15+telefonosAdicionles.size(), telefono.getLayoutParams());
        telefonosAdicionles.add(editAgregarEliminar);
    }

    /**
     * Metodo para agregar editTexts para padecimientos adicionales
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
     * Calcula la minima fecha de nacimiento en el calendario con la fecha alctual(100 años atrás)
     * @return: long de los milisegundos de la fecha
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
     * Calcula la maxima fecha de nacimiento en el calendario con la fecha alctual(1 año atrás)
     * @return: long de los milisegundos de la fecha
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
     * Retorna true si no hay ni un telefono agregado, false si hay al menos uno
     * @return: booleano
     */
    public boolean noHayTelefonos(){
        for(int i=0;i<telefonosAdicionles.size();++i){
            if(!telefonosAdicionles.get(i).getText().toString().equals("")){
                return false;
            }
        }
        return true;
    }

    /**
     * Verifica si hay editText de telefonos vacios
     * @return: booleano
     */
    public boolean hayTelefonosVacios(){
        for(int i=0;i<telefonosAdicionles.size();++i){
            if(telefonosAdicionles.get(i).getText().toString().equals("")){
                return true;
            }
        }
        return false;
    }

    /**
     * Verifica si hay editText de padecimientos vacios
     * @return: booleano
     */
    public boolean hayPadecimientosVacios(){
        for(int i=0;i<padecimientosAdicionales.size();++i){
            if(padecimientosAdicionales.get(i).getText().toString().equals("")){
                return true;
            }
        }
        return false;
    }
}
