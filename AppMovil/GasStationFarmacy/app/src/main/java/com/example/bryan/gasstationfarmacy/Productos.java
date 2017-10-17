package com.example.bryan.gasstationfarmacy;

import android.content.Intent;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ListView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

/**
 * Ventana que despliega los productos
 */
public class Productos extends AppCompatActivity {
    private Bundle extras;//extras para saber cual pantalla anrterior lo invocó
    private static ListView listViewProductos;//List view de los productos
    private ArrayList<String> productos,precios;//lista de medicamentos sus precios
    private ArrayList<Boolean> prescripcion;//lsita de si los medicamentos ocupan prescripcion
    private static ArrayList<Medicamentos> listaProductos;//lista de peroductos del la farmacia

    /**
     * Cuando se crea la ventana
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_productos);
        android.support.v7.app.ActionBar actionBar= getSupportActionBar();
        actionBar.setDisplayHomeAsUpEnabled(true);

        //Intancia de las variables anteriores
        final Global global = (Global) getApplicationContext();//Contiene el token y metodos para comunicarse con e servidor
        System.out.println("Productos " + global.getToken());

        //Las siguientes lineas de codigo solicitan al servidor todos los productos
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
        global.getPostGetRequest().modificarURLGet("consultarMedicamentos");
        JSONArray respuesta = global.getPostGetRequest().Get(global.getToken());
        System.out.println(respuesta);

        productos=descomponerProductos(respuesta);//Se obtiene el nombre de los productos
        precios=descomponerPrecios(respuesta);//Se obtiene el precio de los productos
        prescripcion=descomponerPrescripcion(respuesta);//Se obtiene la prescripcion de los productos(boolean)

        listViewProductos = (ListView)findViewById(R.id.productosDisponibles);
        mostrarProductos();//Se muestran los productos

        extras = getIntent().getExtras();
    }


    /**
     *Para crear el action bar
      * @param menu: menu
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        getMenuInflater().inflate(R.menu.menu_productos,menu);
        return super.onCreateOptionsMenu(menu);
    }

    /**
     * Cuando selecciona boton del action bar se ejecuta este metodo
     * @param item: boton u opcion seleccionada
     * @return: se se procesa la operacion
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item){
        int id = item.getItemId();// se obtiene el id del boton/item para saber cual es
        if(id==R.id.verificarProductos){//si el item seleccionado es el boton para pasar a la siguiente pantalla
            if(ProductosAdapter.getSubmedicamentosList().size()==0){//verifica que se haya selleccionadoal menos un producto
                Toast.makeText(getApplicationContext(),"Debe seleccionar al menos un producto",Toast.LENGTH_LONG).show();
            }else {//si hay medicamentos seleccionados
                //Se crean tres listas nuevas para pasar la informacion a la siguiente ventana
                ArrayList<String> nombre = new ArrayList<>();//nombres de los medicamentos selccionados
                ArrayList<String> precio = new ArrayList<>();//precios de los medicamentos selecionados
                ArrayList<String> receta = new ArrayList<>();//si los medicamentos requieren prescipcion
                //Se inserta la informacion de los medicamentos selesccionados en las listas anteriores
                for (int i = 0; i < ProductosAdapter.getSubmedicamentosList().size(); ++i) {
                    nombre.add(i, ProductosAdapter.getSubmedicamentosList().get(i).getNombre());
                    precio.add(i, ProductosAdapter.getSubmedicamentosList().get(i).getPrecio());
                    receta.add(i, String.valueOf(ProductosAdapter.getSubmedicamentosList().get(i).isReceta()));
                }
                //Se extrae el extra recibido para ver de cual ventana viene para saber a cual pasar


                String anterior = extras.getString("anterior");
                if(anterior.equals("pedido")) {//si viene de la ventana de pedidos pasa a la ventana de la informacion del pedido y se mandan las listas creadas anteriormente
                    Intent i = new Intent(getBaseContext(), Pedido.class);
                    i.putExtra("lista", nombre);
                    i.putExtra("precio",precio);
                    i.putExtra("prescripcion",receta);
                    startActivity(i);
                }else if(anterior.equals("receta")){//si viene de la ventana de creacion de recetas pasa a la misma ventana y se mandan las listas creadas anteriormente ademas del nombre de la receta y la imagen
                    Intent i = new Intent(getBaseContext(), Receta.class);
                    i.putExtra("nombre", extras.getString("nombre"));
                    i.putExtra("imagen", extras.getString("imagen"));
                    i.putExtra("numero", extras.getString("numero"));
                    i.putExtra("doctor", extras.getString("doctor"));
                    i.putExtra("lista", nombre);
                    i.putExtra("precio",precio);
                    startActivity(i);
                }
            }
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    /**
     * Crea un medicamento con los correspondientes valores de las lista de los nombres precios y si requieren prescripcion los preoductos
     * y los mete en una lista para instanciar un adapter y mostrarlos en el listview de la ventana de productos
     */
    private void mostrarProductos(){
        listaProductos = new ArrayList<>();
        Medicamentos medicamentos;
        for(int i=0;i<productos.size();++i){
            medicamentos=new Medicamentos(productos.get(i),precios.get(i));
            medicamentos.setReceta(prescripcion.get(i));
            listaProductos.add(medicamentos);
        }
        ProductosAdapter productosAdapter = new ProductosAdapter(listaProductos, getApplicationContext());
        listViewProductos.setAdapter(productosAdapter);

    }

    /**
     * Obtiene una lista con los nombres de los productos
     * @param json: JSON que contiene la informacion de todos los productos
     * @return: lista con los nombres de todos los productos
     */
    public ArrayList<String> descomponerProductos(JSONArray json){
        JSONObject producto;
        ArrayList<String> nombres=new ArrayList<>();
        for(int i=0;i<json.length();++i){
            try {
                producto=json.getJSONObject(i);
                nombres.add(producto.getString("nombre"));
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        return nombres;
    }

    /**
     * Obtiene una lista con los precios de los productos
     * @param json: JSON que contiene la informacion de todos los productos
     * @return: lista con los precios de todos los productos
     */
    public ArrayList<String> descomponerPrecios(JSONArray json){
        JSONObject producto;
        ArrayList<String> nombres=new ArrayList<>();
        for(int i=0;i<json.length();++i){
            try {
                producto=json.getJSONObject(i);
                nombres.add(producto.getString("precio"));
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        return nombres;
    }

    /**
     * Obtiene una lista con los booleanos de los productos si ocupan prescripcion o no
     * @param json: JSON que contiene la informacion de todos los productos
     * @return: lista con los booleanos de todos los productos si ocupan prescripcion
     */
    public ArrayList<Boolean> descomponerPrescripcion(JSONArray json){
        JSONObject producto;
        ArrayList<Boolean> prescripcion=new ArrayList<>();
        for(int i=0;i<json.length();++i){
            try {
                producto=json.getJSONObject(i);
                System.out.println(producto.getString("prescripcion"));
                prescripcion.add(Boolean.valueOf(producto.getString("prescripcion")));
                System.out.println(prescripcion.get(i));
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        return prescripcion;
    }

    /**
     * @return: list view que despliega los productos
     */
    public static ListView getListViewProductos() {
        return listViewProductos;
    }

    /**
     * @return: lista de productos de la sucursal
     */
    public static ArrayList<Medicamentos> getListaProductos() {
        return listaProductos;
    }

}
