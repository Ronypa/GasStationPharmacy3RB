package com.example.bryan.gasstationfarmacy;

/**
 * Clase Medicamentos
 */
public class Medicamentos {
    private String cantidad="";
    private String nombreReceta="";
    private boolean receta = false;
    private String nombre;
    private String precio;

    /**
     * Constructor
     * @param nombre: nombre del medicamento
     * @param precio: precio del medicamento
     */
    public Medicamentos(String nombre, String precio) {
        this.nombre = nombre;
        this.precio = precio;
    }
    //-------------Getters and Setters---------------------//
    public String getNombre(){return nombre;}
    public void setNombre(String nombre){this.nombre = nombre;}
    public String getPrecio() {return precio;}
    public String getCantidad() {return cantidad;}
    public void setCantidad(String cantidad) {this.cantidad = cantidad;}
    public boolean isReceta(){return receta;}
    public void setReceta(boolean receta){this.receta = receta;}
    public String getNombreReceta(){return nombreReceta;}
    public void setNombreReceta(String nombreReceta){this.nombreReceta = nombreReceta;}

}

