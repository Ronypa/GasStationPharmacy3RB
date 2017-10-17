package com.example.bryan.gasstationfarmacy;

import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.TimePicker;
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
 * Activity que maneja la creacion y actualizacion de pedidos, formulario del pedido
 */
public class Pedido extends AppCompatActivity implements View.OnClickListener{
    private Bundle extras;
    private ListView pedido;
    private Spinner sucursal,telefono,compania;
    private EditText fechaRecojo,horaRecojo,nombrePedido;
    private PedidoAdapter adapter;
    private int hora,minutos,dia,mes,ano;
    private Date date;
    private ArrayList<Medicamentos> medicamentoArrayList;
    private Global global;
    private JSONArray resp;
    private ArrayAdapter<String> adapterSucur;
    private ArrayList<String> listaSucursales,listaTelefonos,listaConmpanias;
    private boolean primeraVezCompania=true;

    /**
     * Cuando se crea el activity
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_pedido);

        //Se instancian las variables anterioremente declaradas
        global = (Global)getApplicationContext();
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);

        //Se consultan las compañias de al servidor
        global.getPostGetRequest().modificarURLGet("consultarCompanias");
        resp = global.getPostGetRequest().Get(global.getToken());
        listaConmpanias = obtenerCompanias(resp);
        compania = (Spinner) findViewById(R.id.compania);
        ArrayAdapter<String> adapterCompanias = new ArrayAdapter<>(getBaseContext(), android.R.layout.simple_spinner_item, listaConmpanias);
        adapterCompanias.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        compania.setAdapter(adapterCompanias);
        //Se agrega un OnItemSelectedListener al espiner de compañias por si cambia cambiar las sucursales del spinner de sucursales
        compania.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if (primeraVezCompania) {
                    primeraVezCompania = false;
                } else {
                    //Consulta al servidor las sucursales de la compañia seleccionada
                    global.getPostGetRequest().consultarSucursales(listaConmpanias.get(position));
                    listaSucursales = obtenerSucursales(global.getPostGetRequest().Post(global.getToken()));
                    for (int i = 0; i < listaSucursales.size(); ++i) {
                        System.out.println(listaSucursales.get(i));
                    }
                    adapterSucur = new ArrayAdapter<>(getBaseContext(), android.R.layout.simple_spinner_item, listaSucursales);
                    sucursal.setAdapter(adapterSucur);
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        });

        //Se consultan las sucursales de la primera compañia
        global.getPostGetRequest().consultarSucursales(listaConmpanias.get(0));
        listaSucursales=obtenerSucursales(global.getPostGetRequest().Post(global.getToken()));
        sucursal = (Spinner) findViewById(R.id.sucursal);
        adapterSucur = new ArrayAdapter<>(getBaseContext(), android.R.layout.simple_spinner_item, listaSucursales);
        adapterSucur.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        sucursal.setAdapter(adapterSucur);

        //Se consultan los telefonos del cliente
        global.getPostGetRequest().modificarURLGet("consultarTelefonosC");
        resp = global.getPostGetRequest().Get(global.getToken());
        listaTelefonos = obtenerTelefonos(resp);
        telefono = (Spinner) findViewById(R.id.telefono);
        ArrayAdapter<String> adapterTelefono = new ArrayAdapter<>(getBaseContext(), android.R.layout.simple_spinner_item, listaTelefonos);
        adapterTelefono.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        telefono.setAdapter(adapterTelefono);

        //Se instancias los campos de texto y demas elementos del activity
        fechaRecojo = (EditText) findViewById(R.id.fechaRecojo);
        horaRecojo = (EditText) findViewById(R.id.horaRecojo);
        nombrePedido = (EditText) findViewById(R.id.nombrePedido);
        fechaRecojo.setOnClickListener(this);
        horaRecojo.setOnClickListener(this);
        pedido = (ListView) findViewById(R.id.pedido);
        pedido.setItemsCanFocus(true);

        //Se obtiene la fecha y hora actual en el sitio donde se encuentre el cliente
        DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
        date = new Date();
        dia=Integer.parseInt(dateFormat.format(date).substring(8, 10));
        mes =Integer.parseInt(dateFormat.format(date).substring(5,7))-1;
        ano=Integer.parseInt(dateFormat.format(date).substring(0, 4));
        hora=Integer.parseInt(dateFormat.format(date).substring(11, 13));
        minutos=Integer.parseInt(dateFormat.format(date).substring(14, 16));

        //Se declara la lista que almacenará los medicamentos del pedido
        medicamentoArrayList = new ArrayList<>();
        extras = getIntent().getExtras();//Si se reciben extras
        if(extras!=null) {
            if (extras.getString("info") != null) { //Si el cliente eligió actualizar el pedido
                //Se solicita la informacion del pedido al servidor y se descifra
                global.getPostGetRequest().consultarPedido(extras.getString("info"));
                obtenerInformacion(global.getPostGetRequest().Post(global.getToken()));
            }
            //Si el pedido se está creando se recuperan los medicamentos seleccionados y se muestran en el listview
            if (extras.getStringArrayList("lista") != null && extras.getStringArrayList("precio") != null && extras.getStringArrayList("prescripcion")!=null) {
                ArrayList<String> lista = extras.getStringArrayList("lista");
                ArrayList<String> precio = extras.getStringArrayList("precio");
                ArrayList<String> prescripcion = extras.getStringArrayList("prescripcion");
                //Se crea la el Array list de medicamentos para mandarlo al adapter del listview
                Medicamentos medicamento;
                for (int i = 0; i < lista.size(); i++) {
                    medicamento = new Medicamentos(lista.get(i), precio.get(i));
                    medicamento.setReceta(Boolean.valueOf(prescripcion.get(i)));
                    medicamento.setCantidad("");
                    medicamentoArrayList.add(medicamento);
                }
                adapter = new PedidoAdapter(this, R.layout.item_listview, medicamentoArrayList, new OnRecetaListener() {
                    //Si se selecciona el campo de texto para agregar una receta a un medicamento que ocupa prescripcion
                    @Override
                    public void onReceta(final String message) {
                        //Se recupera la lista de recetas del cliente
                        global.getPostGetRequest().modificarURLGet("consultarRecetas");
                        final CharSequence[] items = descomponerRecetas(global.getPostGetRequest().Get(global.getToken()));
                        AlertDialog.Builder dialogo = new AlertDialog.Builder(Pedido.this);//Y se muestra un dilogo con la lista de recetas para que el cliente elija cual usar
                        dialogo.setTitle("Elige receta").setItems(items, new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                for(int i=0;i<PedidoAdapter.getArrayMedicamentos().size();++i){
                                    if(PedidoAdapter.getArrayMedicamentos().get(i).getNombre().equals(message)){
                                        PedidoAdapter.getArrayMedicamentos().get(i).setNombreReceta(items[which].toString().split(":")[0]);
                                    }
                                }
                            }
                        });
                        AlertDialog dialog = dialogo.create();
                        dialog.show();
                    }
                });
                pedido.setAdapter(adapter);
            }
        }
    }

    /**
     * Se obtiene la lista de telefonos del cliente
     * @param telefonos: JSON que contiene los telefonos del cliente
     * @return: ArrayList con los telefonos
     */
    private ArrayList<String> obtenerTelefonos(JSONArray telefonos) {
        ArrayList<String> listaTelefonos=new ArrayList<>();
        for(int i=0;i<telefonos.length();++i){
            try {
                listaTelefonos.add(telefonos.getJSONObject(i).getString("opcion"));
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        return listaTelefonos;
    }

    /**
     * Obtiene la lista de sucursales de la compañia seleccionada
     * @param sucursales: String (JSON) con la lista de sucursales
     * @return: ArrayList con las sucursales
     */
    private ArrayList<String> obtenerSucursales(String sucursales) {
        ArrayList<String> nombresSucursales=new ArrayList<>();
        try {
            JSONArray jsonArray = new JSONArray(sucursales);
            for(int i=0;i<jsonArray.length();++i){
                nombresSucursales.add(jsonArray.getJSONObject(i).getString("opcion"));
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
        return nombresSucursales;
    }

    /**
     * Se obtiene la lista de compañia
     * @param companias: JSON que contiene la lista de compañias
     * @return: ArrayList con las compañias
     */
    private ArrayList<String> obtenerCompanias(JSONArray companias) {
        ArrayList<String> nombresCompanias=new ArrayList<>();
        for(int i=0;i<companias.length();++i){
            try {
                nombresCompanias.add(companias.getJSONObject(i).getString("opcion"));
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        return nombresCompanias;
    }

    /**
     * Método que llena los elementos de la ventana con la informacion recuperada
     * @param resultado: String (JSON) con la informacion del pedido
     */
    private void obtenerInformacion(String resultado) {
        try {
            JSONArray jsonArray = new JSONArray(resultado);
            nombrePedido.setText(jsonArray.getJSONObject(0).getString("nombre"));
            for(int i=0;i<listaConmpanias.size();++i){
                if(jsonArray.getJSONObject(0).getString("compania").equals(listaConmpanias.get(i))){
                    compania.setSelection(i);
                    global.getPostGetRequest().consultarSucursales(listaConmpanias.get(i));
                    String result = global.getPostGetRequest().Post(global.getToken());
                    listaSucursales=obtenerSucursales(result);
                    adapterSucur = new ArrayAdapter<>(getBaseContext(), android.R.layout.simple_spinner_item, listaSucursales);
                    sucursal.setAdapter(adapterSucur);
                    break;
                }
            }
            compania.setEnabled(false);
            for(int i=0;i<listaSucursales.size();++i){
                if(jsonArray.getJSONObject(0).getString("sucursal").equals(listaSucursales.get(i))){
                    sucursal.setSelection(i);
                }
            }
            for(int i=0;i<listaTelefonos.size();++i){
                if(jsonArray.getJSONObject(0).getString("telefono").equals(listaTelefonos.get(i))){
                    telefono.setSelection(i);
                }
            }
            String fecha=jsonArray.getJSONObject(0).getString("fecha_recojo");
            fechaRecojo.setText(fecha.substring(0, fecha.length() - 9));
            horaRecojo.setText(fecha.substring(fecha.length() - 8, fecha.length() - 3));

            JSONArray medicamentos=jsonArray.getJSONObject(0).getJSONArray("productos");
            for(int i=0;i<medicamentos.length();++i){
                Medicamentos medicamentosObj = new Medicamentos(medicamentos.getJSONObject(i).getString("medicamento"),medicamentos.getJSONObject(i).getString("precio"));
                medicamentosObj.setCantidad(String.valueOf(medicamentos.getJSONObject(i).getInt("cantidad")));
                if(!medicamentos.getJSONObject(i).getString("receta").equals("0")){
                    medicamentosObj.setNombreReceta(medicamentos.getJSONObject(i).getString("receta"));
                }
                medicamentoArrayList.add(medicamentosObj);
            }
            adapter = new PedidoAdapter(this, R.layout.item_listview, medicamentoArrayList, new OnRecetaListener() {
                @Override
                public void onReceta(String message) {
                    StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
                    StrictMode.setThreadPolicy(policy);
                    global.getPostGetRequest().modificarURLGet("consultarRecetas");
                    resp = global.getPostGetRequest().Get(global.getToken());
                    final CharSequence[] items = descomponerRecetas(resp);
                    AlertDialog.Builder dialogo = new AlertDialog.Builder(Pedido.this);
                    dialogo.setTitle("Elige receta").setItems(items, new DialogInterface.OnClickListener() {@Override public void onClick(DialogInterface dialog, int which) {}});
                    AlertDialog dialog = dialogo.create();
                    dialog.show();
                }
            });
            pedido.setAdapter(adapter);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Crea un menu en el action bar
     * @param menu: menu
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.menu_pedido,menu);
        return super.onCreateOptionsMenu(menu);
    }

    /**
     * al seleccionar un item del menu anterior
     * @param item: item seleccionado
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        int id = item.getItemId();
        if(id==R.id.realizarPedido){//Si se elige realizar el pedido se realizan las siguientes verificaciones
            if(nombrePedido.getText().toString().equals("")){
                Toast.makeText(getApplicationContext(),"Debe agregar un nombre al pedido",Toast.LENGTH_LONG).show();
            }else if(fechaRecojo.getText().toString().equals("")||horaRecojo.getText().toString().equals("")){
                Toast.makeText(Pedido.this, "Debe incluir fecha y hora de retiro", Toast.LENGTH_LONG).show();
            }else if(medicamentoArrayList.size()==0){
                Toast.makeText(getApplicationContext(),"Debe incluir al menos un producto",Toast.LENGTH_LONG).show();
            }else if(!cantidadTodos()){
                Toast.makeText(getApplicationContext(), "Debe agregar cantidad a todos los medicamentos", Toast.LENGTH_LONG).show();
            }else if(!recetaTodos()){
                Toast.makeText(getApplicationContext(), "Debe agregar receta a todos los medicamentos que lo requieran", Toast.LENGTH_LONG).show();
            }else{
                //Si se cumple con lo minimo entra a este else
                final ArrayList<String> medicament =new ArrayList<>();
                final ArrayList<String> cant =new ArrayList<>();
                final ArrayList<String> receta =new ArrayList<>();
                String medicamentos="";
                int total=0;
                if(extras.getString("info") != null) {
                    //Si desea actualizar debe dar 6 horas de tiempo para hacer los cambios necesarios
                    if(horasVentaja(Integer.parseInt(horaRecojo.getText().toString().split(":")[0]), Integer.parseInt(horaRecojo.getText().toString().split(":")[1]))<getSix()){
                        Toast.makeText(getApplicationContext(),"Debe haber al menos seis horas de tiempo para alistar el pedido",Toast.LENGTH_LONG).show();
                    }else {
                        //Si se cumplen las 6 horas se obtiene los nombres, cantidades, recetas de los medicamentos, asi como el total del pedido
                        for (int i = 0; i < medicamentoArrayList.size(); ++i) {
                            medicament.add(medicamentoArrayList.get(i).getNombre());
                            cant.add(medicamentoArrayList.get(i).getCantidad());
                            receta.add(medicamentoArrayList.get(i).getNombreReceta());
                            medicamentos += (medicamentoArrayList.get(i).getCantidad() + " " + medicamentoArrayList.get(i).getNombre() + "\n");
                            total += Integer.parseInt(medicamentoArrayList.get(i).getCantidad()) * Integer.parseInt(medicamentoArrayList.get(i).getPrecio());
                        }
                        actualizarPedido(medicament,cant,receta,medicamentos,total);//actualiza el pedido en base de datos
                    }
                }else{
                    //Si se está creando un pedido nuevo se obtiene los nombres, cantidades, recetas de los medicamentos, asi como el total del pedido
                    for(int i=0;i<PedidoAdapter.getArrayMedicamentos().size();++i){
                        medicament.add(PedidoAdapter.getArrayMedicamentos().get(i).getNombre());
                        cant.add(PedidoAdapter.getArrayMedicamentos().get(i).getCantidad());
                        receta.add(PedidoAdapter.getArrayMedicamentos().get(i).getNombreReceta());
                        medicamentos+=(PedidoAdapter.getArrayMedicamentos().get(i).getCantidad() + " " + PedidoAdapter.getArrayMedicamentos().get(i).getNombre() + "\n");
                        total+=Integer.parseInt(PedidoAdapter.getArrayMedicamentos().get(i).getCantidad())*Integer.parseInt(PedidoAdapter.getArrayMedicamentos().get(i).getPrecio());
                    }
                    crearPedido(medicament,cant,receta,medicamentos,total);//crea el pedido
                }
            }
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    /**
     * muestra un dialogo para confirmar toda la informacion del pedido y el total de este
     * @param medicament: lista de medicamentos pedidos
     * @param cant: cantidad de estos medicamentos
     * @param receta: receta de estos medicamentos
     * @param medicamentos: string para mostrar en dialogo (cantidad y nombre de medicamento)
     * @param total: total de la compra
     */
    private void crearPedido(final ArrayList<String> medicament, final ArrayList<String> cant, final ArrayList<String> receta,String medicamentos,int total) {
        AlertDialog.Builder dialogo = new AlertDialog.Builder(Pedido.this);//Se muestra un dialogo para verificar la informacion
        dialogo.setMessage("Nombre: "+nombrePedido.getText()+"\n"+
                "Compañía: "+compania.getSelectedItem()+"\n"+
                "Sucursal de recojo: "+sucursal.getSelectedItem()+"\n"+
                "Telefono favorito: "+telefono.getSelectedItem()+"\n"+
                "Fecha de recojo: "+fechaRecojo.getText()+"\n"+
                "Hora de recojo: "+horaRecojo.getText()+"\n"+
                "Medicamentos"+"\n"+
                medicamentos+"\n"+"\n"+
                "Total: "+total).setCancelable(false)
                .setPositiveButton("Aceptar", new DialogInterface.OnClickListener() {//Si elige aceptar se crea el pedido en la base de datos
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        //Se manda el servidor
                        global.getPostGetRequest().agregarPedido(nombrePedido.getText().toString(), sucursal.getSelectedItem().toString(), telefono.getSelectedItem().toString(), fechaRecojo.getText().toString(), horaRecojo.getText().toString(), medicament, cant, receta);
                        if(global.getPostGetRequest().Post(global.getToken()).equals("1")){//Si se agrega correctamente se notifica
                            Toast.makeText(getApplicationContext(),"Pedido creado",Toast.LENGTH_LONG).show();
                            startActivity(new Intent(getBaseContext(), VistaCliente.class));
                        }else{//Si se presenta un error se notifica
                            Toast.makeText(getApplicationContext(),"Error al crear el pedido",Toast.LENGTH_LONG).show();
                        }
                    }
                })
                .setNegativeButton("Cancelar", new DialogInterface.OnClickListener() {//Si elige no hacer el pedido
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        dialog.cancel();//Se cancela el dialogo
                    }
                });
        AlertDialog dialog = dialogo.create();
        dialog.setTitle("Información del pedido");
        dialog.show();
    }

    /**
     * muestra un dialogo para confirmar toda la informacion del pedido y el total de este
     * @param medicament: lista de medicamentos pedidos
     * @param cant: cantidad de estos medicamentos
     * @param receta: receta de estos medicamentos
     * @param medicamentos: string para mostrar en dialogo (cantidad y nombre de medicamento)
     * @param total: total de la compra
     */
    private void actualizarPedido(final ArrayList<String> medicament, final ArrayList<String> cant, final ArrayList<String> receta,String medicamentos,int total) {
        AlertDialog.Builder dialogo = new AlertDialog.Builder(Pedido.this);//Se muestra un dialogo para verificar la informacion
        dialogo.setMessage("Nombre: " + nombrePedido.getText() + "\n" +
                "Compañía: " + compania.getSelectedItem() + "\n" +
                "Sucursal de recojo: " + sucursal.getSelectedItem() + "\n" +
                "Telefono favorito: " + telefono.getSelectedItem() + "\n" +
                "Fecha de recojo: " + fechaRecojo.getText() + "\n" +
                "Hora de recojo: " + horaRecojo.getText() + "\n" +
                "Medicamentos" + "\n" +
                medicamentos + "\n" + "\n" +
                "Total: " + total).setCancelable(false)
                .setPositiveButton("Aceptar", new DialogInterface.OnClickListener() {//Si elige aceptar se actualiza el pedido en la base de datos
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        //Se manda el servidor
                        global.getPostGetRequest().actualizarPedido(extras.getString("info"), nombrePedido.getText().toString(), sucursal.getSelectedItem().toString(), telefono.getSelectedItem().toString(), fechaRecojo.getText().toString(), horaRecojo.getText().toString(), medicament, cant, receta);
                        if (global.getPostGetRequest().Post(global.getToken()).equals("1")) {//Si se actualiza correctamente se notifica
                            Toast.makeText(getApplicationContext(), "Pedido actualizado", Toast.LENGTH_LONG).show();
                            startActivity(new Intent(getBaseContext(), VistaCliente.class));
                        } else {//Si se presenta un error se notifica
                            Toast.makeText(getApplicationContext(), "Error", Toast.LENGTH_LONG).show();
                        }
                    }
                })
                .setNegativeButton("Cancelar", new DialogInterface.OnClickListener() {//Si elige no actualizar el pedido
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        dialog.cancel();//Se cancela el dialogo
                    }
                });
        AlertDialog dialog = dialogo.create();
        dialog.setTitle("Información del pedido");
        dialog.show();
    }

    /**
     * @return: Verifica que todos los medicamentos que ocupan prescripcion tengan una receta asociada
     */
    private boolean recetaTodos() {
        for (int i =0;i<medicamentoArrayList.size();++i){
            if(medicamentoArrayList.get(i).isReceta()&&medicamentoArrayList.get(i).getNombreReceta().equals("")){
                return false;
            }
        }
        return true;
    }

    /**
     * @return: Verifica que todos los medicamentos tengan una cantidad asociada
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
     * Si se da click a algún elemento en la pantalla
     * @param v: elemento al que se dio click
     */
    @Override
    public void onClick(View v) {
        if(v==fechaRecojo){//Si se da click para agregar la fecha de recojo se despliega un DatePickerDialog para facilidad del cliente
            DatePickerDialog datePickerDialog = new DatePickerDialog(this, new DatePickerDialog.OnDateSetListener() {
                @Override
                public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
                    if(!horaRecojo.getText().toString().equals("")){
                        if(horasVentaja2(dayOfMonth, (monthOfYear+1), year)<getSix()){//Si ya se habia determinado la hora de recojo verifica que hayan al menos 6 horas de tiempo para que el pedido sea alistado
                            Toast.makeText(getApplicationContext(),"Debe haber al menos seis horas de tiempo para alistar el pedido",Toast.LENGTH_LONG).show();
                        }else {
                            fechaRecojo.setText(dayOfMonth+"/"+(monthOfYear+1)+"/"+year);
                        }
                    }else{
                        fechaRecojo.setText(dayOfMonth+"/"+(monthOfYear+1)+"/"+year);
                    }
                }
            }
                    ,ano,mes,dia);
            datePickerDialog.getDatePicker().setMinDate(date.getTime());
            datePickerDialog.getDatePicker().setMaxDate(getFifteen());
            datePickerDialog.show();
        }else if(v==horaRecojo){//Si se da click para agregar la hora de recojo se despliega un TimePickerDialog para facilidad del cliente
            TimePickerDialog timePickerDialog = new TimePickerDialog(this, new TimePickerDialog.OnTimeSetListener(){
                @Override
                public void onTimeSet(TimePicker view, int hourOfDay, int minute){
                    if(!fechaRecojo.getText().toString().equals("")) {
                        if(horasVentaja(hourOfDay,minute)<getSix()){//Si ya se habia determinado la fecha de recojo verifica que hayan al menos 6 horas de tiempo para que el pedido sea alistado
                            Toast.makeText(getApplicationContext(), "Debe haber al menos seis horas de tiempo para alistar el pedido", Toast.LENGTH_LONG).show();
                        }else{
                            String min=String.valueOf(minute);
                            if(minute>=0&&minute<=9){
                                min="0"+minute;
                            }
                            horaRecojo.setText(hourOfDay + ":" + min);
                        }
                    }else{
                        String min=String.valueOf(minute);
                        if(minute>=0&&minute<=9){
                            min="0"+minute;
                        }
                        horaRecojo.setText(hourOfDay + ":" + min);
                    }

                }
            },hora,minutos,false);
            timePickerDialog.show();
        }
    }

    /**
     * Metodo que obtiene el nombre de las recetas a partir del JSON recibido enviado por el servidor
     * @param json: JSON recibido
     * @return: lista de recetas para asociar con un medicamento del pedido
     */
    public CharSequence[] descomponerRecetas(JSONArray json){
        CharSequence[] nombres;
            JSONObject pedido;
            nombres = new CharSequence[json.length()];
            for(int i=0;i<json.length();++i){
                try {
                    pedido=json.getJSONObject(i);
                    nombres[i]=pedido.getString("opcion2")+": "+pedido.getString("opcion");
                } catch (JSONException e) {
                    e.printStackTrace();
                }
            }
        return nombres;
    }

    /**
     * @return: retorna long (milisegundos) de 15 dias despues a partir de la fecha actual
     */
    public long getFifteen() {
        Calendar calendar = new GregorianCalendar();
        calendar.set(Calendar.DAY_OF_MONTH, dia+14);
        calendar.set(Calendar.MONTH, mes);
        calendar.set(Calendar.YEAR, ano);
        calendar.set(Calendar.HOUR_OF_DAY,0);
        calendar.set(Calendar.MINUTE,0);
        calendar.set(Calendar.SECOND,0);
        return calendar.getTimeInMillis();
    }

    /**
     * @return: Obtiene el long (fecha y hora) de 6 horas despues a partir del momento en que se va a realizar pedido
     */
    public long getSix(){
        Calendar calendar = new GregorianCalendar();
        calendar.set(Calendar.DAY_OF_MONTH, dia);
        calendar.set(Calendar.MONTH, mes+1);
        calendar.set(Calendar.YEAR, ano);
        calendar.set(Calendar.HOUR_OF_DAY,hora+6);
        calendar.set(Calendar.MINUTE,minutos);
        calendar.set(Calendar.SECOND,0);
        return calendar.getTimeInMillis();
    }

    /**
     * Obtiene el long de la fecha y hora seleccionada para verificar que hayan 6 horas de tiempo para alistar el pedido
     * @param hourOfDay: hora de recojo
     * @param minute: minutos de cecojo
     * @return: long de la fecha seleccionada
     */
    private long horasVentaja(int hourOfDay, int minute) {
        String[] fecha = fechaRecojo.getText().toString().split("/");
        Calendar calendar = new GregorianCalendar();
        calendar.set(Calendar.DAY_OF_MONTH, Integer.parseInt(fecha[0]));
        calendar.set(Calendar.MONTH, Integer.parseInt(fecha[1]));
        calendar.set(Calendar.YEAR, Integer.parseInt(fecha[2]));
        calendar.set(Calendar.HOUR_OF_DAY,hourOfDay);
        calendar.set(Calendar.MINUTE,minute);
        calendar.set(Calendar.SECOND,0);
        return calendar.getTimeInMillis();
    }

    /**
     * Obtiene el long de la fecha y hora seleccionada para verificar que hayan 6 horas de tiempo para alistar el pedido
     * @param dayOfMonth: dia del mes seleccionado
     * @param monthOfYear: mes del año selesccionado
     * @param year: año seleccionado
     * @return: long de la fecha y hora seleccionada
     */
    private long horasVentaja2(int dayOfMonth, int monthOfYear, int year) {
        String[] hora = horaRecojo.getText().toString().split(":");
        Calendar calendar = new GregorianCalendar();
        calendar.set(Calendar.DAY_OF_MONTH, dayOfMonth);
        calendar.set(Calendar.MONTH, monthOfYear);
        calendar.set(Calendar.YEAR, year);
        calendar.set(Calendar.HOUR_OF_DAY,Integer.parseInt(hora[0]));
        calendar.set(Calendar.MINUTE,Integer.parseInt(hora[1]));
        calendar.set(Calendar.SECOND,0);
        return calendar.getTimeInMillis();
    }
}



