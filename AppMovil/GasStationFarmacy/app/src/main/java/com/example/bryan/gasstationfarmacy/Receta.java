package com.example.bryan.gasstationfarmacy;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.drawable.BitmapDrawable;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.os.StrictMode;
import android.provider.MediaStore;
import android.support.v7.app.AppCompatActivity;
import android.util.Base64;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.util.ArrayList;

public class Receta extends AppCompatActivity implements View.OnClickListener{
    //Variables globales
    private Global global;//contiene el token
    private ImageView fotoReceta;//View donde va la foto de la receta
    private ImageButton btnFoto;//Boton para agregar receta
    private Button btnMedicamento;//Boton para agregar medicamentos
    private EditText nombreReceta,numeroReceta,doctorReceta;//Campo de texto de nombre de la receta
    private ListView lvMedicaReceta;//List view donde aparecen los medicamentos por receta
    private Bundle extras;// Extras donde viene la lista de medicamentos seleccionada para mostrarla
    private ArrayList<String> lista,precio;//listas con el nombre precio del producto
    private ArrayList<Medicamentos> medicamentoArrayList;
    private RecetaAdapter adapter;//adapter del listview de los medicamentos
    private boolean hayFoto;//Boolean para verificar si hay foto o no
    private String APP_DIRECTORY = "fotos/";//directorio donde se guardan las fotos tomadas
    private String MEDIA_DIRECTORY = APP_DIRECTORY+"media";
    private String TEMPORAL_PICTURE_NAME="temporal.jpg";
    private final int PHOTO_CODE=100;//codigos para ver si se abre la camara
    private final int SELECT_PICTURE=200;//si se selecciona una foto de la galeria

    /**
     *Al crear una ventana de receta se ejecuta
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_receta);

        //Intancia de las variables anteriores
        global = (Global)getApplicationContext();//contiene el token y la instancia de la clase que hace GET y POST al servidor
        System.out.println("Informacion receta " + global.getToken());
        hayFoto=false;//no hay foto al crearse
        nombreReceta = (EditText)findViewById(R.id.nombreReceta);
        numeroReceta = (EditText)findViewById(R.id.numeroReceta);
        doctorReceta = (EditText)findViewById(R.id.doctor);
        btnFoto = (ImageButton)findViewById(R.id.botonFoto);
        fotoReceta = (ImageView)findViewById(R.id.fotoReceta);
        lvMedicaReceta = (ListView) findViewById(R.id.lvMedicaReceta);
        lvMedicaReceta.setItemsCanFocus(true);
        btnMedicamento = (Button)findViewById(R.id.btnReceta);
        btnMedicamento.setOnClickListener(this);
        btnFoto.setOnClickListener(this);
        lista=new ArrayList<>();
        precio=new ArrayList<>();
        medicamentoArrayList=new ArrayList<>();

        extras = getIntent().getExtras();
        if(extras!=null) {//Si se reciben extras
            if (extras.getString("info") != null) {//Si el usuario seleccionó editar
                mostrarInformacion();//Metodo para recuperar la informacion desde el servidor
                hayFoto=true;
                btnMedicamento.setEnabled(false);
            }
            if (extras.getStringArrayList("lista") != null && extras.getStringArrayList("precio") != null) {//Lista de medicamentos
                mostrarListaMedicamentos();//Muestra la lista de medicamentos elegida
            }
            if(extras.getString("nombre")!=null){//Nombre e imagen en base 64
                recuperar();//Recupera la informacion que ya se habia digitado
            }
        }
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
    }

    /**
     * recupera el nombre, numero, doctor que emitio y la imagen de la receta al ir a seleccionar medicamentos para la receta
     */
    private void recuperar() {
        nombreReceta.setText(extras.getString("nombre"));
        numeroReceta.setText(extras.getString("numero"));
        doctorReceta.setText(extras.getString("doctor"));
        if(!extras.getString("imagen").equals("")){
            byte[] decodedString = Base64.decode(extras.getString("imagen"), Base64.DEFAULT);
            Bitmap decodedByte = BitmapFactory.decodeByteArray(decodedString, 0, decodedString.length);
            fotoReceta.setImageBitmap(decodedByte);
        }
    }

    /**
     * muestra la lista de medicamentos que se seleccionaron
     */
    private void mostrarListaMedicamentos() {
        lista = extras.getStringArrayList("lista");
        precio = extras.getStringArrayList("precio");
        btnMedicamento.setEnabled(false);
        Medicamentos medicamento;
        for (int i = 0; i < lista.size(); i++) {
            medicamento = new Medicamentos(lista.get(i), precio.get(i));
            medicamento.setReceta(false);
            medicamentoArrayList.add(medicamento);
        }
        adapter = new RecetaAdapter(this, R.layout.item_listreceta, medicamentoArrayList);
        lvMedicaReceta.setAdapter(adapter);
        for(int i=0;i<medicamentoArrayList.size();++i){
            System.out.println(medicamentoArrayList.get(i).getNombre());
        }
    }

    /**
     * muestra la informacion de la receta si es un editar y solicita al servidor los medicamentos que contiene esta receta
     */
    private void mostrarInformacion() {
        String info = extras.getString("info");
        numeroReceta.setText(info);
        numeroReceta.setEnabled(false);
        global.getPostGetRequest().consultarReceta(info);
        String resultado = global.getPostGetRequest().Post(global.getToken());
        obtenerInformacion(resultado);
    }

    /**
     * Metodo que recupera la informacion que se recibe del servidor al seleccionar editar
     * @param resultado: JSON que se recibe del servidor
     */
    private void obtenerInformacion(String resultado) {
        try {
            JSONArray jsonArray = new JSONArray(resultado);
            nombreReceta.setText(jsonArray.getJSONObject(0).getString("nombre"));//Se obtiene el nombre de la receta
            doctorReceta.setText(jsonArray.getJSONObject(0).getString("doctor"));//Se obtiene el doctor que emitio la receta
            String imagenCodoficada = jsonArray.getJSONObject(0).getString("imagen");//Se obtiene la imagen de la receta
            byte[] decodedString = Base64.decode(imagenCodoficada, Base64.DEFAULT);
            Bitmap decodedByte = BitmapFactory.decodeByteArray(decodedString, 0, decodedString.length);
            fotoReceta.setImageBitmap(decodedByte);
            JSONArray medicamentos=jsonArray.getJSONObject(0).getJSONArray("productos");//Se obtienen los medicamentos de la receta
            for(int i=0;i<medicamentos.length();++i){
                Medicamentos medicamentosObj = new Medicamentos(medicamentos.getJSONObject(i).getString("medicamento"),"");
                medicamentosObj.setCantidad(String.valueOf(medicamentos.getJSONObject(i).getInt("cantidad")));
                medicamentoArrayList.add(medicamentosObj);
            }
            adapter = new RecetaAdapter(this, R.layout.item_listreceta, medicamentoArrayList);
            lvMedicaReceta.setAdapter(adapter);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * crea el action bar
     * @param menu: menu
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.menu_receta, menu);
        return super.onCreateOptionsMenu(menu);
    }

    /**
     * Al elegir una opcion o presionar un boon del action bar se ejecuta este metodo
     * @param item: opcion o boton selccionado
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        int id = item.getItemId();//Id de la opcion o boton seleccionado
        if(id==R.id.registrarReceta){//Si se eligió el boton para guardar la receta
            if(nombreReceta.getText().toString().equals("")){//Verifica que la receta tenga nombre
                Toast.makeText(Receta.this, "Debe agregar un nombre a la receta", Toast.LENGTH_LONG).show();
            }else if(numeroReceta.getText().toString().equals("")){
                Toast.makeText(Receta.this, "Debe agregar un número a la receta", Toast.LENGTH_LONG).show();
            }else if(numeroReceta.getText().toString().equals("0")){
                Toast.makeText(Receta.this, "Número de receta no válido", Toast.LENGTH_LONG).show();
            }else if(doctorReceta.getText().toString().equals("")){
                Toast.makeText(Receta.this, "Debe agregar el doctor que emitió la receta", Toast.LENGTH_LONG).show();
            }else if(medicamentoArrayList.size()==0){
                Toast.makeText(Receta.this, "Debe agregar al menos un medicamento", Toast.LENGTH_LONG).show();
            }else if(!cantidadTodos()){
                Toast.makeText(Receta.this, "Debe agregar cantidad a todos los medicamentos", Toast.LENGTH_LONG).show();
            }else if(!hayFoto){//Verifica que haya foto
                Toast.makeText(Receta.this, "Debe agregar una foto de la receta", Toast.LENGTH_LONG).show();
            }else{
                Bitmap bitmap = ((BitmapDrawable) fotoReceta.getDrawable()).getBitmap();
                ByteArrayOutputStream baos = new ByteArrayOutputStream();
                bitmap.compress(Bitmap.CompressFormat.JPEG, 100, baos);
                byte[] imageInByte = baos.toByteArray();
                String encodedImage = Base64.encodeToString(imageInByte, Base64.DEFAULT);
                ArrayList<String> cantidad=new ArrayList<>();
                for(int i=0;i<medicamentoArrayList.size();++i){
                    cantidad.add(medicamentoArrayList.get(i).getCantidad());
                }

                if(extras.getString("info") != null){//Si se esta editando la receta se cumple esta condicion
                    for(int i=0;i<medicamentoArrayList.size();++i){
                        lista.add(medicamentoArrayList.get(i).getNombre());
                    }
                    //Se manda al servidor la actualizacion
                    global.getPostGetRequest().cambiarReceta(nombreReceta.getText().toString(), numeroReceta.getText().toString(), doctorReceta.getText().toString(), encodedImage, lista, cantidad);
                    String resultado = global.getPostGetRequest().Post(global.getToken());
                    if (resultado.equals("1")) {//Si se actualiza correctamente en la base de datos se notifica
                        Toast.makeText(getApplicationContext(), "Receta actualizada", Toast.LENGTH_LONG).show();
                        startActivity(new Intent(getBaseContext(), VistaCliente.class));
                    } else {//Si se presenta un error se notifica
                        Toast.makeText(getApplicationContext(), "Error al actualizar", Toast.LENGTH_LONG).show();
                    }
                }else {//Si se está creando la receta
                    //Se solicita al servidor crear la receta y se manda la informacion
                    global.getPostGetRequest().agregarReceta(nombreReceta.getText().toString(), numeroReceta.getText().toString(), doctorReceta.getText().toString(), encodedImage, lista, cantidad);
                    String resultado = global.getPostGetRequest().Post(global.getToken());
                    if (resultado.equals("1")) {//Si se crea correctamente se notifica
                        Toast.makeText(getApplicationContext(), "Receta creada", Toast.LENGTH_LONG).show();
                        startActivity(new Intent(getBaseContext(), VistaCliente.class));
                    } else {//Si se presenta un error se notifica
                        Toast.makeText(getApplicationContext(), "Error al crear receta", Toast.LENGTH_LONG).show();
                    }
                }
            }
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    /**
     * Verifica que todos los medicamentos tengan cantidad
     * @return: true si todos tienen, false si al menos uno no tiene
     */
    private boolean cantidadTodos() {
        for (int i =0;i<medicamentoArrayList.size();++i){
            if(medicamentoArrayList.get(i).getCantidad().equals("")){
                return false;
            }
        }
        return true;
    }

    /**
     * Método que abre la camara
     */
    private void openCamera() {
        File file = new File(Environment.getExternalStorageDirectory(),MEDIA_DIRECTORY);
        file.mkdirs();
        String path = Environment.getExternalStorageDirectory()+File.separator
                +MEDIA_DIRECTORY+File.separator+TEMPORAL_PICTURE_NAME;
        File newFile = new File(path);
        Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        intent.putExtra(MediaStore.EXTRA_OUTPUT, Uri.fromFile(newFile));
        startActivityForResult(intent, PHOTO_CODE);
    }

    /**
     * @param requestCode: codigo de la peticion: foto o galería
     * @param resultCode: codigo del resultado
     * @param data: dato seleccionado de galeria
     */
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        switch (requestCode){
            case PHOTO_CODE:
                if(resultCode==RESULT_OK){
                    String dir=Environment.getExternalStorageDirectory()+File.separator
                            +MEDIA_DIRECTORY+File.separator+TEMPORAL_PICTURE_NAME;
                    decodeBitmap(dir);
                }
            break;
            case SELECT_PICTURE:
                if(resultCode==RESULT_OK){
                    Uri path = data.getData();
                    fotoReceta.setImageURI(path);
                    hayFoto=true;
                }
            break;
        }
    }

    /**
     * @param dir:Decodifica la foto que se toma
     */
    private void decodeBitmap(String dir) {
        Bitmap bitmap;
        bitmap= BitmapFactory.decodeFile(dir);
        fotoReceta.setImageBitmap(bitmap);
        hayFoto=true;
    }

    /**
     * Al dar click a un boton se ejecuta este método
     * @param v: View que se presiona
     */
    @Override
    public void onClick(View v) {
        if(v==btnMedicamento){//Si se presiona el boton para agregar medicamentos se deben llevar los datos ya digitados para luego recuperarlos
            Intent i = new Intent(getBaseContext(), Productos.class);
            i.putExtra("anterior", "receta");
            i.putExtra("nombre",nombreReceta.getText().toString());
            i.putExtra("numero",numeroReceta.getText().toString());
            i.putExtra("doctor", doctorReceta.getText().toString());
            if(fotoReceta.getDrawable()!=null) {
                Bitmap bitmap = ((BitmapDrawable) fotoReceta.getDrawable()).getBitmap();
                ByteArrayOutputStream baos = new ByteArrayOutputStream();
                bitmap.compress(Bitmap.CompressFormat.JPEG, 100, baos);
                byte[] imageInByte = baos.toByteArray();
                String encodedImage = Base64.encodeToString(imageInByte, Base64.DEFAULT);
                i.putExtra("imagen", encodedImage);
            }else {
                i.putExtra("imagen", "");
            }
            startActivity(i);
        }else if(v==btnFoto){//Si se presiona el boton para agregar una foto
            final CharSequence[] options = {"Foto", "Galería", "Cancelar"};
            final AlertDialog.Builder builder = new AlertDialog.Builder(Receta.this);
            builder.setTitle("Seleccionar...");
            builder.setItems(options, new DialogInterface.OnClickListener() {
                //Se presenta un diagolo para que el usuario
                @Override
                public void onClick(DialogInterface dialog, int seleccion) {//Se crea un dialog para que el usuario elija si toma foto u obtener de galeria
                    if (options[seleccion] == "Foto") {
                        openCamera();
                    } else if (options[seleccion] == "Galería") {
                        Intent intent = new Intent(Intent.ACTION_PICK, MediaStore.Images.Media.EXTERNAL_CONTENT_URI);
                        intent.setType("image/*");
                        startActivityForResult(Intent.createChooser(intent, "Buscar en"), SELECT_PICTURE);
                    } else if (options[seleccion] == "Cancelar") {
                        dialog.dismiss();
                    }
                }
            });
            builder.show();
        }
    }
}
