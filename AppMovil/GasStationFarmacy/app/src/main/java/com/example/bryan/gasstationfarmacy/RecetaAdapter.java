package com.example.bryan.gasstationfarmacy;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.TextView;

import java.util.ArrayList;

/**
 * Adapter de las listas de medicamentos que aparece en la vista de crear recetas
 */
public class RecetaAdapter extends ArrayAdapter<Medicamentos> {
    private LayoutInflater inflater;
    private ArrayList<Medicamentos> arrayMedicamentos;

    /**
     * Constructor del adapter
     * @param context: contexto de la vista recetas
     * @param resource: tipo de adapter para el list view
     * @param arraymedicamentos: lista de medicamentos a mostrar
     */
    public RecetaAdapter(Context context, int resource, ArrayList<Medicamentos> arraymedicamentos) {
        super(context, resource);
        this.arrayMedicamentos = arraymedicamentos;
        this.inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    /**
     * Para obtener un view especifico del adapter
     * @param position: posicion del view
     * @param convertView:
     * @param parent: padre del view
     * @return: view
     */
    @Override
    public View getView(final int position, View convertView, ViewGroup parent) {
        final ViewHolder holder;
        if (convertView == null) {
            //Inicializa los elementos que estrán dentro de cada item del list view
            convertView = inflater.inflate(R.layout.item_listreceta, null);
            holder = new ViewHolder();
            holder.subMedicamento = (TextView) convertView.findViewById(R.id.subMedicamentoR);
            holder.cantidad = (EditText) convertView.findViewById(R.id.cantidadR);
            convertView.setTag(holder);
        } else {
            holder = (ViewHolder) convertView.getTag();
        }
        //Asigna valores a los elementos si es necesario, como al editar deben aparecer los datos ya registrados
        Medicamentos medicamento = arrayMedicamentos.get(position);
        holder.subMedicamento.setText(medicamento.getNombre());
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
     * @return: Cantidad de elementos en la listview
     */
    @Override
    public int getCount() {return arrayMedicamentos.size();}

    /**
     * Holder del Adapter
     */
    static class ViewHolder {
        TextView subMedicamento;
        EditText cantidad;
    }

}