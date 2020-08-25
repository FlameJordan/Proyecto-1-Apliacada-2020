/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package GUI;

import Backend_Aplicada.Main;
import Objeto.Producto;
import java.awt.Color;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import javax.swing.BoxLayout;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.ListSelectionModel;
import javax.swing.table.DefaultTableModel;

/**
 *
 * @author brend
 */
public class MainWindow extends JFrame implements ActionListener {

    private JButton jbtnPrueba;
    private JTable jTableG;
    int filaCheck;

    public void init() {
        filaCheck = 0;
        this.setLayout(null);
        this.setBounds(0, 0, 800, 600);
        this.setDefaultCloseOperation(EXIT_ON_CLOSE);
        panelTablaProductos panel = new panelTablaProductos();

        this.add(panel);
        this.setVisible(true);

    }

    @Override
    public void actionPerformed(ActionEvent e) {

    }

    public class panelTablaProductos extends JPanel {

        JScrollPane scroll;
        JTable jTable;

        public panelTablaProductos() {
            this.setLayout(null);
            this.setBounds(20, 20, 400, 500);
            this.setBackground(Color.red);
            initJtable();
            setUpTableData();
            this.setVisible(true);
        }

        public void initJtable() {
            String[] colName = {"ID Producto", "ID Empresa", "Nombre", "Precio", "Descripción", "Cantidad", "Estado"};
            if (jTable == null) {
                jTable = new JTable() {

                    @Override
                    public Class getColumnClass(int column) {
                        switch (column) {
                            case 0:
                                return int.class;
                            case 1:
                                return int.class;
                            case 2:
                                return String.class;
                            case 3:
                                return int.class;
                            case 4:
                                return String.class;
                            case 5:
                                return int.class;
                            default:
                                return Boolean.class;
                        }
                    }

                    @Override
                    public boolean isCellEditable(int nRow, int nCol) {
                        if (nCol == 6) {
                            return true;
                        }
                        return false;
                    }

                };
            }

            DefaultTableModel contactTableModel = (DefaultTableModel) jTable
                    .getModel();
            contactTableModel.setColumnIdentifiers(colName);
            jTable.addMouseListener(new MouseAdapter() {
                @Override
                public void mouseClicked(MouseEvent e) {
                    if (e.getClickCount() == 1) {
                        JTable target = (JTable) e.getSource();
                        if (target.getSelectedColumn() == 6) {
                            int row = target.getSelectedRow();
                            boolean estado = (boolean) contactTableModel.getValueAt(row, 6);
                            Producto p = new Producto();
                            p.setIdProducto(Integer.parseInt((String) jTable.getValueAt(row, 0)));
                            p.setIdEmpresa(Integer.parseInt((String) jTable.getValueAt(row, 1)));
                            p.setNombre((String) jTable.getValueAt(row, 2));
                            p.setPrecio(Integer.parseInt((String) jTable.getValueAt(row, 3)));
                            p.setDescripcion((String) jTable.getValueAt(row, 4));
                            p.setCantStock(Integer.parseInt((String) jTable.getValueAt(row, 5)));
                            if (jTable.getValueAt(row, 6).equals(true)) {
                                p.setEstado(0);
                            } else {
                                p.setEstado(1);
                            }
                            System.out.println("ID producto: " + p.getIdProducto());
                            System.out.println("ID Empresa: " + p.getIdEmpresa());
                            System.out.println("Nombre: " + p.getNombre());
                            System.out.println("Descripcion: " + p.getDescripcion());
                            System.out.println("Cantidad: "+p.getCantStock());
                            System.out.println("Estado: "+p.getEstado());
                        }
                    }
                }

            });

            jTable.setSelectionMode(ListSelectionModel.SINGLE_SELECTION);
            jTable.setAutoResizeMode(JTable.AUTO_RESIZE_ALL_COLUMNS);

            scroll = new JScrollPane(jTable);
            scroll.setBounds(0, 0, 400, 500);
            scroll.setVisible(true);
            this.add(scroll);

        }

        public void setUpTableData() {
            DefaultTableModel tableModel = (DefaultTableModel) jTable.getModel();
            Main.con.select();
            for (int i = 0; i < Main.con.listaProducto.size(); i++) {
                String[] data = new String[6];
                data[0] = String.valueOf(Main.con.listaProducto.get(i).getIdProducto());
                data[1] = String.valueOf(Main.con.listaProducto.get(i).getIdEmpresa());
                data[2] = String.valueOf(Main.con.listaProducto.get(i).getNombre());
                data[3] = String.valueOf(Main.con.listaProducto.get(i).getPrecio());
                data[4] = String.valueOf(Main.con.listaProducto.get(i).getDescripcion());
                data[5] = String.valueOf(Main.con.listaProducto.get(i).getCantStock());

                tableModel.addRow(data);
                if (Main.con.listaProducto.get(i).getEstado() == 0) {
                    tableModel.setValueAt(true, i, 6);
                } else {
                    tableModel.setValueAt(false, i, 6);
                }
            }
            tableModel.fireTableDataChanged();
            jTable.setModel(tableModel);
            jTableG = jTable;
            Main.con.listaProducto.clear();

            /**/
        }

    }

}
