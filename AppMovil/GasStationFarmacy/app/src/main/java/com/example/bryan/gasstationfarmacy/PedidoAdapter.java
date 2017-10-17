package com.example.bryan.gasstationfarmacy;

import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.TextView;

import java.util.ArrayList;

/**
 * Adapter para la lista de pedidos
 */
public class PedidoAdapter extends ArrayAdapter<Medicamentos> {
    private LayoutInflater inflater;
    private static ArrayList<Medicamentos> arrayMedicamentos;//Lista de medicamentos que fueron selesccionados en el pedido
    private OnRecetaListener mListener;//Listener del boton por si se desea agregar la receta

    /**
     * Contructor del adapter
     * @param context: contexto del activity
     * @param resource:
     * @param arraymedicamentos: lista de medicamentos que hay que mostrar en la listview de pedidos
     * @param mListener: listener del boton "receta"
     */
    public PedidoAdapter(Context context, int resource, ArrayList<Medicamentos> arraymedicamentos,OnRecetaListener mListener) {
        super(context, resource);
        this.mListener = mListener;
        arrayMedicamentos = arraymedicamentos;
        this.inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    /**
     * Se obtiene el view
     * @param position: que fue selccionado en esta posicion
     * @param convertView:
     * @param parent: padre del view
     * @return: View
     */
    @Override
    public View getView(final int position, View convertView, ViewGroup parent) {
        final ViewHolder holder;
        final Medicamentos medicamento = arrayMedicamentos.get(position);
        System.out.println(medicamento.getNombre());
        if (convertView == null) {
            //Se crean los elementos que vana estar dentro de lis items de la lista de medicamentos de pedidos
            convertView = inflater.inflate(R.layout.item_listview, null);
            holder = new ViewHolder();
            holder.subMedicamento = (TextView) convertView.findViewById(R.id.subMedicamento);
            holder.subPrecio = (TextView) convertView.findViewById(R.id.subPrecio);
            holder.cantidad = (EditText) convertView.findViewById(R.id.cantidad);
            holder.receta = (EditText) convertView.findViewById(R.id.recetaEdit);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }
        //Asigna el valor correspondiente a los elementos anteriormente creados
        holder.subMedicamento.setText(medicamento.getNombre());
        holder.subPrecio.setText(medicamento.getPrecio());
        holder.receta.setText(medicamento.getNombreReceta());
        holder.receta.setEnabled(medicamento.isReceta());
        if(medicamento.isReceta()){
            holder.receta.setHintTextColor(Color.RED);
        }else{
            holder.receta.setHintTextColor(Color.LTGRAY);
        }
        holder.receta.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mListener != null)
                    mListener.onReceta(medicamento.getNombre());
            }
        });
        holder.receta.setId(position);
        holder.receta.setOnFocusChangeListener(new View.OnFocusChangeListener() {

            public void onFocusChange(View v, boolean hasFocus) {
                if (!hasFocus) {
                    final int position = v.getId();
                    final EditText Caption = (EditText) v;
                    Caption.setText(arrayMedicamentos.get(position).getNombreReceta());
                }
            }

        });
        holder.cantidad.setText(medicamento.getCantidad());
        holder.cantidad.setId(position);
        holder.cantidad.setOnFocusChangeListener(new View.OnFocusChangeListener() {

            public void onFocusChange(View v, boolean hasFocus) {
                if (!hasFocus) {
                    final int position = v.getId();
                    final EditText Caption = (EditText) v;
                    arrayMedicamentos.get(position).setCantidad(Caption.getText().toString());
                }
            }

        });
        return convertView;
    }

    /**
     * @return: cantidad de medicamentos en la listview
     */
    @Override
    public int getCount(){
        return arrayMedicamentos.size();
    }

    /**
     * Crea el holder del listview: es para que no se pierda datos si hay un scrollbar
     */
    static class ViewHolder {
        TextView subMedicamento;
        TextView subPrecio;
        EditText cantidad;
        EditText receta;
    }

    /**
     * @return: lista de medicamentos del pedido
     */
    public static ArrayList<Medicamentos> getArrayMedicamentos() {
        return arrayMedicamentos;
    }
}