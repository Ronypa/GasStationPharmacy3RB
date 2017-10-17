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
 * Fragment que muestra la lista de recetas del cliente
 */
public class RecetaFragment extends Fragment {
    private ArrayList<String> recetasNombreNumero,recetasNumero;
    private ArrayAdapter<String> adapterRecetas;
    private Global global;

    /**
     * Se ejecuta al crear el Fragment
     * @param inflater:
     * @param container:
     * @param savedInstanceState:
     * @return:
     */
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        return inflater.inflate(R.layout.fragment_receta, container, false);
    }

    /**
     * Se ejecuta al crear el activity donde se ubica este fragment
     * @param state: estado
     */
    @Override
    public void onActivityCreated(Bundle state) {
        super.onActivityCreated(state);

        //Instancia de las variables anteriores
        global = (Global)getActivity().getApplicationContext();
        recetasNombreNumero=new ArrayList<>();
        recetasNumero=new ArrayList<>();
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
        //Se realiza un get de todas las recetas para mostrarlas al cliente
        global.getPostGetRequest().modificarURLGet("consultarRecetas");
        JSONArray respuesta = global.getPostGetRequest().Get(global.getToken());
        if(respuesta !=null){
            recetasNumero=descomponerRecetas(respuesta);
        }else{//Si la respuesta del servidor es nula se notifica
            Toast.makeText(getActivity().getApplicationContext(),"Error en el servidor: no se cargaron las recetas",Toast.LENGTH_LONG).show();
        }
        ListView listaRecetas;
        listaRecetas = (ListView) getView().findViewById(R.id.listaRecetas);
        adapterRecetas = new ArrayAdapter<>(getActivity(), android.R.layout.simple_expandable_list_item_1, recetasNombreNumero);
        listaRecetas.setAdapter(adapterRecetas);
        registerForContextMenu(listaRecetas);
        //Boton para agregar receta
        FloatingActionButton agregarReceta = (FloatingActionButton) getView().findViewById(R.id.agregarReceta);
        agregarReceta.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                startActivity(new Intent(getContext(), Receta.class));
            }
        });
    }

    /**
     * Se ejecuta para crear un menu al hacer un long click sobre una receta
     * @param menu: menu que se crea
     * @param v: sobre el view que se da click(listView de las recetas)
     * @param menuInfo: informacion del menu
     */
    @Override
    public void onCreateContextMenu(ContextMenu menu, View v, ContextMenu.ContextMenuInfo menuInfo) {
        if (v.getId() == R.id.listaRecetas) {
            AdapterView.AdapterContextMenuInfo info = (AdapterView.AdapterContextMenuInfo) menuInfo;
            menu.setHeaderTitle(recetasNombreNumero.get(info.position));
            menu.add(Menu.NONE, 0, 0, "Editar");//opciones del menu
            menu.add(Menu.NONE, 1, 1, "Eliminar");
        }
    }

    /**
     * Al seleccionar una opcion del menu que se crea con el metodo anterior
     * @param item: item seleccionado
     * @return: true si se proceso la accion
     */
    @Override
    public boolean onContextItemSelected(MenuItem item) {
        if (getUserVisibleHint()) {
            final AdapterView.AdapterContextMenuInfo informacion = (AdapterView.AdapterContextMenuInfo) item.getMenuInfo();
            final int menuItemIndex = item.getItemId();//Se obtiene el index del item que se presionó
            String[] menuItems = {"Editar", "Eliminar"};//Opciones del menu
            String menuItemName = menuItems[menuItemIndex]; //Opcion elegida del menu: editar o eliminar

            if (menuItemName.equals("Editar")) {//Si se elige la opcion de editar la receta
                Intent i = new Intent(getActivity().getBaseContext(), Receta.class);//Se pasa a la ventana de receta
                i.putExtra("info",recetasNumero.get(informacion.position));//Se envia el numero de receta para hacer un get de la informacion
                startActivity(i);

            } else if (menuItemName.equals("Eliminar")) {//Si se elige eliminar la receta se presenta un dialogo de verificacion
                AlertDialog.Builder dialogo = new AlertDialog.Builder(getContext());
                dialogo.setMessage("¿Está seguro(a) que quiere eliminar esta receta?").setCancelable(false)
                        .setPositiveButton("Sí", new DialogInterface.OnClickListener() {//Si se elige eliminarla
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                global.getPostGetRequest().eliminarReceta(recetasNumero.get(informacion.position));//Se elimina la receta de la base de datos
                                String resultado = global.getPostGetRequest().Post(global.getToken());
                                if(resultado.equals("1")){//Si se elimino corretamente la receta
                                    recetasNombreNumero.remove(informacion.position);//Se elimina de las listas que la contienen
                                    recetasNumero.remove(informacion.position);
                                    adapterRecetas.notifyDataSetChanged();//Se notifica el cambio a la listview
                                    Toast.makeText(getContext(),"Receta eliminada",Toast.LENGTH_LONG).show();//Se notifica que se elimino
                                }else{//Si no se pudo borrar es porque hay un pedido que la está ocuando, se notifica
                                    Toast.makeText(getContext(),"Receta requerida por un pedido que no ha sido retirado",Toast.LENGTH_LONG).show();
                                }
                            }
                        })
                        .setNegativeButton("No", new DialogInterface.OnClickListener() {//Si se decide no borrar
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.cancel();//Se cancela el dialogo
                            }
                        });
                AlertDialog dialog = dialogo.create();
                dialog.setTitle("¡ATENCIÓN!");
                dialog.show();
            }
            return true;
        }
        return false;
    }

    /**
     * Al hacer el get de la lista de recetas se manda el JSON a este metodo para obtener la informacion en un Arraylist
     * @param json: JSON que proviene del servidor, contiene la lista de recetas(Nombre y numero)
     * @return: lista de los numeros de la receta
     */
    public ArrayList<String> descomponerRecetas(JSONArray json){
        JSONObject pedido;
        ArrayList<String> numero=new ArrayList<>();
        for(int i=0;i<json.length();++i){
            try {
                pedido=json.getJSONObject(i);
                numero.add(pedido.getString("opcion2"));//En el key opcion2 vienen los numeros de las recetas
                recetasNombreNumero.add(pedido.getString("opcion2")+": "+pedido.getString("opcion"));//Lista que se pasara al adapter del listview(bumero+nombre)
            } catch (JSONException e) {
                e.printStackTrace();
            }
        }
        return numero;
    }
}
