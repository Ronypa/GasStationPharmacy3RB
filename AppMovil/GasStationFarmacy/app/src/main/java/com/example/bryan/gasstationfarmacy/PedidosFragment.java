package com.example.bryan.gasstationfarmacy;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.design.widget.FloatingActionButton;
import android.support.v4.app.Fragment;
import android.view.ContextMenu;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

/**
 * Clase del fragment que tiene la lista de pedidos
 */
public class PedidosFragment extends Fragment {
    private ArrayList<String> pedidosNombreNumero,pedidosNumero;//Lista de pedidos del cliente
    private ArrayAdapter<String> pedidosAdapter;//Adapter del pedido
    private Global global;//Contiene el token

    /**
     *Al crear el fragment
     */
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        return inflater.inflate(R.layout.fragment_mis_pedidos, container, false);

    }

    /**
     * Al crear el activity que contiene el fragment
     * @param state; estado
     */
    @Override
    public void onActivityCreated(Bundle state) {
        super.onActivityCreated(state);

        //Instancias de las variables anteriores
        global = (Global)getActivity().getApplicationContext();
        System.out.println("Pedidos " + global.getToken());

        pedidosNumero=new ArrayList<>();
        pedidosNombreNumero=new ArrayList<>();

        //En las siguientes 3 lineas de codigo se solicita al servidor la lista de pedidos del cliente para mostrarlas en pantalla
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
        global.getPostGetRequest().modificarURLGet("consultarPedidos");
        JSONArray respuesta = global.getPostGetRequest().Get(global.getToken());

        System.out.println(respuesta);

        //Se verifica que la lista de pedidos enviada desde el servidor sea valida
        if(respuesta !=null){
            pedidosNumero=descomponerPedidos(respuesta);//Se descompone el JSON recibido para obtener los nombres de los pedidos
        }else{
            //Si no se obtiene una respuesta valida del servidor se envia un mensaje al cliente
            Toast.makeText(getActivity().getApplicationContext(),"Error en el servidor: no se cargaron los pedidos",Toast.LENGTH_LONG).show();
        }

        ListView listaPedidos = (ListView) getView().findViewById(R.id.listaPedidos);
        //Se crea el adapter de la lista de pedidos con los nombres de estos
        pedidosAdapter = new ArrayAdapter<>(getContext(), android.R.layout.simple_expandable_list_item_1, pedidosNombreNumero);
        listaPedidos.setAdapter(pedidosAdapter);
        registerForContextMenu(listaPedidos);

        //Este es el boton para agregar un pedido el cual cambia de ventana, va a la de productos
        FloatingActionButton fab = (FloatingActionButton) getView().findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent i = new Intent(getActivity().getBaseContext(), Productos.class);
                i.putExtra("anterior", "pedido");
                startActivity(i);
            }
        });
    }

    /**
     * Este método es para crear un menú al mantener presionado un pedido para que al usuario le aparezca la opcion de editar o eliminar un pedido
     * @param menu: menu
     * @param v: item de la lista que fue seleccionado
     * @param menuInfo: informacion del menu
     */
    @Override
    public void onCreateContextMenu(ContextMenu menu, View v, ContextMenu.ContextMenuInfo menuInfo) {
        if (v.getId() == R.id.listaPedidos) {
            AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo) menuInfo;
            menu.setHeaderTitle(pedidosNombreNumero.get(info.position));
            menu.add(Menu.NONE, 0, 0, "Editar");
            menu.add(Menu.NONE, 1, 1, "Eliminar");
        }
    }

    /**
     * Se ejecuta cuando se selecciona un item de la lista
     * @param item: item seleccionado
     * @return:
     */
    @Override
    public boolean onContextItemSelected(final MenuItem item) {
        if (getUserVisibleHint()) {//Si está visible al usuario, lista pedido o lista receta
            final AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo) item.getMenuInfo();
            final int menuItemIndex = item.getItemId();//se obtiene al id del item
            String[] menuItems = {"Editar", "Eliminar"};//Lista de opciones del menu
            String menuItemName = menuItems[menuItemIndex]; //Opcion del menu: editar o eliminar

            if (menuItemName.equals("Editar")) {//Si se elige editar
                if(pedidosNombreNumero.get(info.position).split(":")[2].equals(" Nuevo")) {
                    Intent i = new Intent(getActivity().getBaseContext(), Pedido.class);//Se pasa la a la vista de informacion del pedido
                    i.putExtra("info", pedidosNumero.get(info.position));//y se le manda la informacion para la que la despliegue en los campos respectivos
                    startActivity(i);
                }else{
                    Toast.makeText(getContext().getApplicationContext(),"Este pedido no se puede actualizar ya que está"+pedidosNombreNumero.get(info.position).split(":")[2],Toast.LENGTH_LONG).show();
                }
            } else if (menuItemName.equals("Eliminar")) {//Si se elige eliminar
                AlertDialog.Builder dialogo = new AlertDialog.Builder(getContext());
                dialogo.setMessage("¿Está seguro(a) que quiere eliminar este pedido?").setCancelable(false)//Se presenta un dilogo que pregunta si de verdad se quiere eliminar
                        .setPositiveButton("Sí", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {//Si decide eliminarlo
                                //Las siguientes tres lineas de codigo hacen el proceso para solicitarle al servidor que elimine el pedido de la BD
                                global.getPostGetRequest().eliminarPedido(pedidosNumero.get(info.position));
                                StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
                                StrictMode.setThreadPolicy(policy);
                                String resultado = global.getPostGetRequest().Post(global.getToken());//se captura la respuesta del servidor
                                if(resultado.equals("1")){
                                    pedidosNombreNumero.remove(info.position);
                                    pedidosNumero.remove(info.position);
                                    pedidosAdapter.notifyDataSetChanged();
                                    Toast.makeText(getContext(),"Pedido eliminado",Toast.LENGTH_LONG).show();
                                }else{
                                    Toast.makeText(getContext(),"No se puede eliminar un pedido que está:"+pedidosNombreNumero.get(info.position).split(":")[2],Toast.LENGTH_LONG).show();
                                }
                            }
                        })
                        .setNegativeButton("No", new DialogInterface.OnClickListener() {//si no se desea eliminar el pedido
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.cancel();//Se cancela el dialogo
                            }
                        });
                AlertDialog dialog = dialogo.create();
                dialog.setTitle("¡ATENCIÓN!");//Titulo del dialogo
                dialog.show();
            }
            return true;
        }
        return false;
    }

    /**
     * Obtiene el nombre de todos los pedidos del cliente del JSON que se recibe del servidor para mostrarlos en la lista
     * @param json: JSON recibido del servidor
     * @return: lista de nombres de pedidos
     */
    public ArrayList<String> descomponerPedidos(JSONArray json){
        JSONObject pedido;
        ArrayList<String> numero=new ArrayList<>();
        for(int i=0;i<json.length();++i){
            try {
                pedido=json.getJSONObject(i);
                numero.add(pedido.getString("opcion2"));
                String estado = pedido.getString("opcion3");
                switch (estado) {
                    case "n":
                        estado = "Nuevo";
                        break;
                    case "p":
                        estado = "Preparado";
                        break;
                    case "f":
                        estado = "Facturado";
                        break;
                    case "r":
                        estado = "Retirado";
                        break;
                }
                pedidosNombreNumero.add(pedido.getString("opcion2")+": "+pedido.getString("opcion")+": "+estado);
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        return numero;
    }
}
