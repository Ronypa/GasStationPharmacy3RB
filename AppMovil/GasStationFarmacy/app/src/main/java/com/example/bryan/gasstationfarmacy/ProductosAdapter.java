package com.example.bryan.gasstationfarmacy;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.ListView;
import android.widget.TextView;

import java.util.ArrayList;

/**
 * Adapter de la lista de productos/medicamentos
 */
public class ProductosAdapter extends ArrayAdapter<Medicamentos> implements CompoundButton.OnCheckedChangeListener{
    private ArrayList<Medicamentos> medicamentosList;//Lista completa de medicamentos
    private static ArrayList<Medicamentos> submedicamentosList;//Lista de medicamentos seleccionados
    private Context context;//contexto

    /**
     * Constructor de la clase
     * @param medicamentosList: lista de medicamentos a mostrar en la lista
     * @param context: contxto de activity que lo invoca
     */
    public ProductosAdapter(ArrayList<Medicamentos> medicamentosList, Context context) {
        super(context, R.layout.item_medicamento,medicamentosList);
        this.medicamentosList=medicamentosList;
        this.context=context;
        submedicamentosList = new ArrayList<>();
    }

    /**
     * Metodo que se ejecuta cuanto se activa o desactiva un checkbox
     * @param buttonView: el check que se activo/desactivo
     * @param isChecked:si se marcó o se desmarcó
     */
    @Override
    public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
        int pos = Productos.getListViewProductos().getPositionForView(buttonView);
        if(pos!= ListView.INVALID_POSITION){
            Medicamentos m = Productos.getListaProductos().get(pos);
            if(isChecked){
                submedicamentosList.add(m); //Si se marcó se agrega a la lista
            }else{
                submedicamentosList.remove(m);//Si se desmarcó se quita de la lista
            }
        }
    }

    /**
     * El holder es para que no se desmarquen los checkboxes al ocultarse por la presencia de un scrollbar
     */
    public static class MedicamentoHolder{
        public TextView nombreMedicamento;
        public TextView precio;
        public CheckBox check;
        public MedicamentoHolder(){}
    }

    /**
     * Obtiene un view
     * @param position: posicion del view
     * @param convertView: objeto view
     * @param parent: padre del view
     * @return: el view
     */
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        View v =convertView;
        MedicamentoHolder holder =new MedicamentoHolder();

        if(convertView==null){
            LayoutInflater inflater=(LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            v=inflater.inflate(R.layout.item_medicamento,null);

            holder.nombreMedicamento=(TextView) v.findViewById(R.id.medicamento);
            holder.precio=(TextView) v.findViewById(R.id.precio);
            holder.check = (CheckBox) v.findViewById(R.id.check);
            holder.check.setOnCheckedChangeListener(ProductosAdapter.this);

        }else{
            holder = (MedicamentoHolder) v.getTag();
    }
        Medicamentos m = medicamentosList.get(position);
        if(holder!=null){
        holder.nombreMedicamento.setText(m.getNombre());
        holder.precio.setText(m.getPrecio());
        //holder.check.setChecked(parent.isSelected());
            holder.check.setChecked(parent.isSelected());
        holder.check.setTag(m);}
        return v;
    }

    /**
     * @return: lista de objetos seleccionados
     */
    public static ArrayList<Medicamentos> getSubmedicamentosList(){return submedicamentosList;}
}
