package com.example.bryan.gasstationfarmacy;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.os.StrictMode;
import android.support.design.widget.TabLayout;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Toast;

/**
 * Activity que contiene las listas de pedidos y recetas (es la vista principal del cliente)
 */
public class VistaCliente extends AppCompatActivity {
    private Global global;//Contiene el token del cliente

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_vista_cliente);

        //Instancia de las variables globales, del ViewPager y el Adapter
        global = (Global)getApplicationContext();
        SectionsPagerAdapter mSectionsPagerAdapter = new SectionsPagerAdapter(getSupportFragmentManager());
        ViewPager mViewPager = (ViewPager) findViewById(R.id.container);
        assert mViewPager != null;
        mViewPager.setAdapter(mSectionsPagerAdapter);
        TabLayout tabLayout = (TabLayout) findViewById(R.id.tabs);
        assert tabLayout != null;
        tabLayout.setupWithViewPager(mViewPager);
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
    }

    /**
     * Crea el menu del action bar
     * @param menu: menu
     * @return: si se crea
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_vista_cliente, menu);//menu para agregar opciones en el action bar
        return true;
    }

    /**
     * Se ejecuta cuando se elige una opcion del menu del action bar
     * @param item: item elegido
     * @return: se si proceso: true
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();//Se obtiene el id del item seleccionado
        if(id == R.id.actualizarme){//Si se elige actualizar los datos del cliente
            startActivity(new Intent(getBaseContext(), ActualizacionDeDatos.class));//Se pasa a la ventana de actualizacion
        }else if(id == R.id.cambioContrasena){//Sise elige cambiar contraseña
            startActivity(new Intent(getBaseContext(), CambioDeContrasena.class));//Se pasa a la ventana de cambio de contraseña
        }else if(id == R.id.eliminarme){//Si se elige eliminar la cuenta
            AlertDialog.Builder dialogo = new AlertDialog.Builder(VistaCliente.this);//Se muestra un dialogo para verificar si desea eliminar la cuenta
            dialogo.setMessage("¿Está seguro(a) que quiere eliminar su cuenta?").setCancelable(false)
                    .setPositiveButton("Sí", new DialogInterface.OnClickListener() {//Si elige que desea eliminar la cuenta
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            global.getPostGetRequest().eliminarCliente();//Se elimina al cliente de la base de datos
                            String resultado = global.getPostGetRequest().Post(global.getToken());
                            if(resultado.equals("1")){//Si se elimina correctamente lo notifica y pasa a la ventana de logeo y registro
                                Toast.makeText(getApplicationContext(),"Cuenta eliminada",Toast.LENGTH_LONG).show();
                                startActivity(new Intent(getBaseContext(), MainActivity.class));
                            }else{//Si no se logra eliminar a cliente en la base de datos se notifica
                                Toast.makeText(getApplicationContext(),"Error",Toast.LENGTH_LONG).show();
                            }
                        }
                    })
                    .setNegativeButton("No", new DialogInterface.OnClickListener() {//Si elige no eliminar la cuenta
                        @Override
                        public void onClick(DialogInterface dialog, int which) {
                            dialog.cancel();//Se cancela el dialogo
                        }
                    });
            AlertDialog dialog = dialogo.create();
            dialog.setTitle("¡ATENCIÓN!");
            dialog.show();
        }
        return super.onOptionsItemSelected(item);
    }

    //Esta es la clase del Adapter de los Fragments
    public class SectionsPagerAdapter extends FragmentPagerAdapter {
        public SectionsPagerAdapter(FragmentManager fm) {
            super(fm);
        }
        @Override
        public Fragment getItem(int position) {//Si se escoge una posicion del menu o un fragment se crea una nueva instancia del fragment correspondiente
            switch (position) {
                case 0:
                    return new PedidosFragment();
                case 1:
                    return new RecetaFragment();
            }
            return null;
        }
        //Cuneta la cantidad de fragments o paginas del menu
        @Override
        public int getCount() {
            return 2;
        }

        /**
         * Se da nombre a los fragments
         * @param position: posicion del fragment
         * @return: titulo del fragment
         */
        @Override
        public CharSequence getPageTitle(int position) {
            switch (position) {
                case 0:
                    return "MIS PEDIDOS";
                case 1:
                    return "RECETAS";
            }
            return null;
        }
    }

    /**
     * Si se presiona el botón de atrás
     */
    @Override
    public void onBackPressed() {
        AlertDialog.Builder dialogo = new AlertDialog.Builder(VistaCliente.this);//Se crea un dialogo para confirmar que desea salir a la ventana de logeo
        dialogo.setMessage("¿Está seguro(a) que desea salir?").setCancelable(false)
                .setPositiveButton("Sí", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {//Si elige salir
                        startActivity(new Intent(getApplicationContext(), MainActivity.class));//Se va a la ventana de logeo y registro
                    }
                })
                .setNegativeButton("No", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {//Si elige no salir se cancela el dialogo
                        dialog.cancel();
                    }
                });
        AlertDialog dialog = dialogo.create();
        dialog.setTitle("¡ATENCIÓN!");
        dialog.show();
    }
}