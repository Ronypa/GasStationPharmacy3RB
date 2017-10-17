package com.example.bryan.gasstationfarmacy;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.Iterator;

import javax.net.ssl.HttpsURLConnection;

/**
 * Esta clase se encarga de realizar los POSTs Y GETs al servidor para poder desplegar y consultar toda la informacion necesaria
 */
public class PostGetRequest {
    private JSONObject postDataParams;//JSON que se enviará al servidor
    private String direccion;//Direccion http del servidor para realizar algún POST
    private String URLGet;//Direccion http del servidor para realizar algún GET
    private String DIR="http://192.168.100.6/WebApi/";

    /**
     * Método que crea el JSON que se enviará al servidor para actualizar los datos de un cliente
     * @param nombre1: primer nombre del cliente
     * @param nombre2: segundo nombre del cliente
     * @param apellido1: primer apellido del cliente
     * @param apellido2: segundo apellido del cliente
     * @param provincia: provincia donde vive el cliente
     * @param ciudad: ciudad donde vive el cliente
     * @param senas: señas de la direccion del cliente
     * @param nacimiento: fecha de nacimiento del cliente
     * @param telefonos: telefonos del cliente
     * @param padecimiento: padecimientos del cliente
     * @param ano: año en que el cliente padeció
     */
    public void actualizarDatos(String nombre1, String nombre2,String apellido1,String apellido2,String provincia
            ,String ciudad,String senas,String nacimiento,ArrayList<String> telefonos,ArrayList<String> padecimiento,ArrayList<String> ano){
        this.direccion=DIR+"actualizarDatos";//direccion para actalizar datos del cliente
        postDataParams = new JSONObject();//Creacion de un nuevo JSON
        try {
            //Se agrega al JSON la informacion que el cliente desea actualizar
            postDataParams.put("nombre1", nombre1);
            postDataParams.put("nombre2", nombre2);
            postDataParams.put("apellido1", apellido1);
            postDataParams.put("apellido2", apellido2);
            postDataParams.put("provincia", provincia);
            postDataParams.put("ciudad", ciudad);
            postDataParams.put("senas", senas);
            postDataParams.put("fechaNacimiento", nacimiento);
            postDataParams.put("telefonos",telefonos);
            //creacion de la lista de padecimientos con el año de padecimiento
            JSONArray jsonArray = new JSONArray();
            JSONObject padece;
            for(int i=0;i<padecimiento.size();++i){
                padece=new JSONObject();
                padece.put("padecimiento",padecimiento.get(i));
                if(!ano.get(i).equals("")){
                    padece.put("año",Integer.parseInt(ano.get(i)));
                }else{
                    padece.put("año",null);
                }
                jsonArray.put(padece);
            }
            postDataParams.put("padecimientos",jsonArray);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Método que crea el JSON que se enviará al servidor para actualizar la contraseña de un cliente
     * @param constraActual: contraseña actual
     * @param contraNueva: contraseña nueva
     */
    public void cambiarContrasena(String constraActual,String contraNueva){
        this.direccion=DIR+"actualizarContrasena";//direccion para actalizar la contraseña del cliente
        postDataParams = new JSONObject();
        try {
            //Se agrega al JSON la contraseña actual para verificar que es la correcta y la nueva
            postDataParams.put("opcion",constraActual);
            postDataParams.put("opcion2",contraNueva);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Método que crea el JSON que se enviará al servidor para eliminar un cliente
     */
    public void eliminarCliente(){
        this.direccion=DIR+"borrarCliente";//direccion para eliminar al cliente
        postDataParams = new JSONObject();
        try {
            postDataParams.put("opcion","");
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     *  Método que crea el JSON que se enviará al servidor para eliminar una receta
     * @param numero: número de receta que se desea eliminar
     */
    public void eliminarReceta(String numero){
        this.direccion=DIR+"borrarReceta";//direccion para eliminar una receta
        postDataParams = new JSONObject();
        try {
            postDataParams.put("opcion",numero);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     *  Método que crea el JSON que se enviará al servidor para eliminar un pedido
     * @param numero: numero del pedido que se desea eliminar
     */
    public void eliminarPedido(String numero){
        this.direccion=DIR+"borrarPedido";//direccion para eliminar un pedido
        postDataParams = new JSONObject();
        try {
            postDataParams.put("opcion",numero);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     *  Método que crea el JSON que se enviará al servidor para crear un pedido
     * @param nombre: nombre del pedido
     * @param sucursal: sucursal de recojo
     * @param telefono: telefono preferido del cliente
     * @param fecha: fecha de recojo
     * @param hora: hora de recojo
     * @param medicamentos: medicamentos que contiene el pedido
     * @param cantidad: cantidad de cada medicamento
     * @param recetas: se envia el numero de la receta si algun medicamento requiere prescripcion
     */
    public void agregarPedido(String nombre, String sucursal, String telefono,String fecha,String hora,ArrayList<String> medicamentos,ArrayList<String> cantidad,ArrayList<String> recetas){
        this.direccion=DIR+"agregarPedido";//direccion para agregar un pedido
        postDataParams = new JSONObject();
        try {
            //Se agrega al JSON la informacion necesaria para crear un pedido
            postDataParams.put("nombre", nombre);
            postDataParams.put("sucursal", sucursal);
            postDataParams.put("telefono", telefono);
            postDataParams.put("fecha_recojo", fecha+" "+hora+":00");
            //lista de medicamentos con su cantidad y receta si lo requiere
            JSONArray jsonArray=new JSONArray();
            JSONObject jsonObject;
            for(int i=0;i<medicamentos.size();++i){
                jsonObject = new JSONObject();
                jsonObject.put("medicamento",medicamentos.get(i));
                jsonObject.put("cantidad", Integer.parseInt(cantidad.get(i)));
                System.out.println(recetas.get(i));
                if(!recetas.get(i).equals("")){
                    jsonObject.put("receta",Integer.parseInt(recetas.get(i)));
                }else{
                    jsonObject.put("receta",0);
                }
                jsonArray.put(jsonObject);
            }
            postDataParams.put("productos", jsonArray);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     *  Método que crea el JSON que se enviará al servidor para actualizar un pedido
     * @param numero: numero del pedido
     * @param nombre: nombre del pedido
     * @param sucursal: sucursal de recojo
     * @param telefono: telefono preferido del cliente
     * @param fecha: fecha de recojo
     * @param hora: hora de recojo
     * @param medicamentos: lista de medicamentos que el pedido contiene
     * @param cantidad: cantidad de cada medicamento
     * @param recetas: receta de cada medicamento si lo necesita
     */
    public void actualizarPedido(String numero,String nombre, String sucursal, String telefono, String fecha, String hora, ArrayList<String> medicamentos, ArrayList<String> cantidad, ArrayList<String> recetas) {
        this.direccion=DIR+"actualizarPedido";//direccion para actualiar los pedidos
        postDataParams = new JSONObject();
        try {
            //Se agrega la informacion actualizada al JSON
            postDataParams.put("numero",Integer.parseInt(numero));
            postDataParams.put("nombre", nombre);
            postDataParams.put("sucursal", sucursal);
            postDataParams.put("telefono", telefono);
            postDataParams.put("fecha_recojo", fecha+" "+hora+":00");
            //Lista de medicamentos del pedido
            JSONArray jsonArray=new JSONArray();
            JSONObject jsonObject;
            for(int i=0;i<medicamentos.size();++i){
                jsonObject = new JSONObject();
                jsonObject.put("medicamento",medicamentos.get(i));
                jsonObject.put("cantidad", Integer.parseInt(cantidad.get(i)));
                System.out.println(recetas.get(i));
                if(!recetas.get(i).equals("")){
                    jsonObject.put("receta",Integer.parseInt(recetas.get(i)));
                }else{
                    jsonObject.put("receta",0);
                }
                jsonArray.put(jsonObject);
            }
            postDataParams.put("productos", jsonArray);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Método que crea el JSON que se enviará al servidor para agregar una receta
     * @param nombre: nombre de la receta
     * @param numero: numero de la receta
     * @param doctor: doctor que emitió la receta
     * @param imagen: imagen de la receta
     * @param medicamentos: lista de medicamentos de la reta
     * @param cantidad: cantidad de los medicamentos
     */
    public void agregarReceta(String nombre,String numero,String doctor,String imagen,ArrayList<String> medicamentos,ArrayList<String> cantidad){
        this.direccion= DIR+"agregarReceta";//direccion para agregar una receta
        postDataParams = new JSONObject();
        try {
            postDataParams.put("nombre", nombre);
            postDataParams.put("numero", Integer.parseInt(numero));
            postDataParams.put("doctor", doctor);
            postDataParams.put("imagen", imagen);
            //lista de medicamentos de la receta
            JSONArray jsonArray = new JSONArray();
            JSONObject medicamento;
            for(int i=0;i<medicamentos.size();++i){
                medicamento=new JSONObject();
                medicamento.put("medicamento", medicamentos.get(i));
                medicamento.put("cantidad", Integer.parseInt(cantidad.get(i)));
                jsonArray.put(medicamento);
            }
            postDataParams.put("medicamentos",jsonArray);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Método que crea el JSON que se enviará al servidor para actualizar una receta
     * @param nombre: nombre de la receta
     * @param numero: numero de la receta
     * @param doctor: doctor que la emitió
     * @param encodedImage: imagen de la receta
     * @param lista: lista de medicamentos de la receta
     * @param cantidad: cantidad de medicamentos
     */
    public void cambiarReceta(String nombre, String numero, String doctor, String encodedImage, ArrayList<String> lista, ArrayList<String> cantidad) {
        this.direccion= DIR+"actualizarReceta";//direccion para actualizar la receta
        postDataParams = new JSONObject();
        try {
            //Se agrega la informacion de la receta al JSON
            postDataParams.put("nombre", nombre);
            postDataParams.put("numero", Integer.parseInt(numero));
            postDataParams.put("doctor", doctor);
            //Lista de medicamentos de la receta
            JSONArray jsonArray = new JSONArray();
            JSONObject medicamento;
            for(int i=0;i<lista.size();++i){
                medicamento=new JSONObject();
                medicamento.put("medicamento", lista.get(i));
                medicamento.put("cantidad", Integer.parseInt(cantidad.get(i)));
                jsonArray.put(medicamento);
            }
            postDataParams.put("medicamentos",jsonArray);
            postDataParams.put("imagen", encodedImage);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Método que crea el JSON que se enviará al servidor para consultar una receta
     * @param numero: numero de receta
     */
    public void consultarReceta(String numero) {
        this.direccion=DIR+"consultarReceta";//direccion para consultar recetas
        postDataParams = new JSONObject();
        try {
            postDataParams.put("opcion",numero);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Método que crea el JSON que se enviará al servidor para consultar pedidos
     * @param numero: numero de pedido
     */
    public void consultarPedido(String numero) {
        this.direccion=DIR+"consultarDetallePedido";//direccion para consultar pedidos
        postDataParams = new JSONObject();
        try {
            postDataParams.put("opcion",numero);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Método que crea el JSON que se enviará al servidor para consultar las sucursales de una compañía
     * @param compania: compañia de la que se desea consultar sucursales
     */
    public void consultarSucursales(String compania) {
        this.direccion=DIR+"consultarSucursalesC";//direccion para consultar sucursales de una compañia
        postDataParams = new JSONObject();
        try {
            postDataParams.put("opcion",compania);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Método que crea el JSON que se enviará al servidor para agregar un cliente
     * @param cedula: cedula del cliente
     * @param nombre1: primer nombre
     * @param nombre2: segundo nombre
     * @param apellido1: primer apellido
     * @param apellido2: segundo apellido
     * @param provincia: provincia de donde vive el cliente
     * @param ciudad: ciudad donde habita
     * @param senas: señas de la direccion
     * @param nacimiento: fecha de nacimiento
     * @param telefonos: telefonos del cliente
     * @param padecimiento: padecimientos del cliente
     * @param ano: año del padecimiento
     * @param contrasena: contraseña
     * @param prioridad: prioridad
     */
    public void agregarCliente(String cedula, String nombre1, String nombre2,String apellido1,String apellido2,String provincia
            ,String ciudad,String senas,String nacimiento,ArrayList<String> telefonos,ArrayList<String> padecimiento,ArrayList<String> ano
            ,String contrasena, String prioridad){
        this.direccion=DIR+"agregarCliente";//direccion para agregar un cliente
        postDataParams = new JSONObject();
        try {
            //Se agraga al JSON los datos del cliente
            postDataParams.put("cedula", cedula);
            postDataParams.put("nombre1", nombre1);
            postDataParams.put("nombre2", nombre2);
            postDataParams.put("apellido1", apellido1);
            postDataParams.put("apellido2", apellido2);
            postDataParams.put("provincia", provincia);
            postDataParams.put("ciudad", ciudad);
            postDataParams.put("senas", senas);
            postDataParams.put("fechaNacimiento", nacimiento);
            postDataParams.put("contrasena", contrasena);
            postDataParams.put("prioridad", prioridad);
            postDataParams.put("telefonos", telefonos);
            //lista de padecimientos
            JSONArray jsonArray = new JSONArray();
            JSONObject padece;
            for(int i=0;i<padecimiento.size();++i){
                padece=new JSONObject();
                padece.put("padecimiento",padecimiento.get(i));
                if(!ano.get(i).equals("")){
                    padece.put("año",Integer.parseInt(ano.get(i)));
                }else{
                    padece.put("año",null);
                }
                jsonArray.put(padece);
            }
            postDataParams.put("padecimientos", jsonArray);
            System.out.println(jsonArray);
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    /**
     * Metodo que realiza los POSTs al servidor
     * @param token: token del cliente para identificanrlo
     * @return: respuesta del servidor, ya sea de confirmacion o informacion para desplegar/usar en la aplicacion
     */
    public String Post(String token) {
        try {
            URL url = new URL(direccion); // URL definida en los métodos anteriores segun sea la consulta que se desea realizar

            //Conexion con el servidor
            HttpURLConnection conn = (HttpURLConnection) url.openConnection();
            conn.setReadTimeout(15000);
            conn.setConnectTimeout(15000);
            conn.setRequestMethod("POST");
            conn.setDoInput(true);
            conn.setDoOutput(true);

            //Datos que se enviaran al servidor para identificacion/autenticacion
            String encodedAuth="Bearer "+token;
            conn.setRequestProperty("Authorization", encodedAuth);

            //Salida de datos(Se abre conexion y se envia JSON)
            OutputStream os = conn.getOutputStream();
            BufferedWriter writer = new BufferedWriter(new OutputStreamWriter(os, "UTF-8"));
            writer.write(getPostDataString(postDataParams));
            writer.flush();
            writer.close();
            os.close();

            //Codigo de respuesta
            int responseCode = conn.getResponseCode();
            //Si la consulta se realizó exitosamente
            if (responseCode == HttpsURLConnection.HTTP_OK) {
                //Se obtiene la respuesta del servidor
                BufferedReader in = new BufferedReader(new InputStreamReader(conn.getInputStream()));
                StringBuilder sb = new StringBuilder("");
                String line;
                while ((line = in.readLine()) != null) {

                    sb.append(line);
                    break;
                }
                in.close();
                return sb.toString();
                //Si el codigo de la respuesta no es HttpOk retorna el codigo de la excepcion
            } else {
                return "false : " + responseCode;
            }
            //Si se captura alguna otra excepcion
        } catch (Exception e) {
            return "Exception: " + e.getMessage();
        }
    }

    /**
     * Pasa el JSON a String para enviarlo al servidor
     * @param params: JSON
     * @return: String
     * @throws Exception
     */
    public String getPostDataString(JSONObject params) throws Exception {
        StringBuilder result = new StringBuilder();
        boolean first = true;
        Iterator<String> itr = params.keys();
        while (itr.hasNext()) {
            String key = itr.next();
            Object value = params.get(key);
            if (first)
                first = false;
            else
                result.append("&");
            result.append(URLEncoder.encode(key, "UTF-8"));
            result.append("=");
            result.append(URLEncoder.encode(value.toString(), "UTF-8"));
        }
        return result.toString();
    }

    public void modificarURLGet(String PURLGet){
        URLGet=DIR+PURLGet;
    }

    /**
     * Metodo que realiza GETs al servidor
     * @param token: token del cliente para identificarlo
     * @return: JSONArray con respuesta de servidor
     */
    public JSONArray Get(String token) {
        //Crea la variable para la conexion
        HttpURLConnection urlConnection = null;
        try {
            // Crea la conexion
            URL urlToRequest = new URL(URLGet);
            urlConnection = (HttpURLConnection)urlToRequest.openConnection();
            urlConnection.setConnectTimeout(15000);
            urlConnection.setReadTimeout(15000);
            ///Datos que se enviaran al servidor para identificacion/autenticacion
            String encodedAuth="Bearer "+token;
            urlConnection.setRequestProperty("Authorization", encodedAuth);

            // Codigo de la respuesta
            int statusCode = urlConnection.getResponseCode();
            if (statusCode == HttpURLConnection.HTTP_OK) {
                // Se lee la respuesta del servidor y se retorna en un JSONArray
                BufferedReader rd = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                String content = "", line;
                while ((line = rd.readLine()) != null) {
                    content += line + "\n";
                }
                return new JSONArray(content);
            }else{
                return new JSONArray("");
            }//Captura excepciones de url incorrectas, JSONExceptions, etc...
        } catch (IOException | JSONException ignored) {
        } finally {
            if (urlConnection != null) {
                urlConnection.disconnect();//Se cierra la conexion
            }
        }
        return null;
    }
}